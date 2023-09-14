using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using AuthService.Interfaces;
using ConfigHelper.Configs;
using Microsoft.Extensions.Options;
using Models.Enums;
using Models.Models.AuthService;
using ILogger = Serilog.ILogger;

namespace AuthService.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly EmailSettings _emailSettings;
    
    public EmailSender(ILogger logger, IOptions<EmailSettings> emailSettings)
    {
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }
    public async Task<EmailRegisterResult> SendVerificationMail(string token, string to, EmailType type)
    {
        var result = new EmailRegisterResult();
        try
        {
            if (_emailSettings.UseEmail)
            {
                var fromAddress = new MailAddress(_emailSettings.ServiceMailAddress, _emailSettings.ServiceMailName);
                var mail = new MailMessage(fromAddress, new MailAddress(to));
                mail.IsBodyHtml = true;


                _logger.Information("Token was generated: " + token);
                mail.Subject = "Hello there! This is Freya bot";
                mail.Body = await GetBody(type, token);

                var sender = new SmtpClient(_emailSettings.Smtp);
                sender.Credentials = new NetworkCredential(_emailSettings.ServiceMailAddress, _emailSettings.Password);
                sender.EnableSsl = true;
                await sender.SendMailAsync(mail);
            }
        }
        catch (Exception err)
        {
            _logger.Error(err, "EmailSender can't send email");
            result.ErrorMessage = "email can't be send";
            result.SendToUser = false;
            return result;
        }

        result.SendToUser = _emailSettings.UseEmail;
        return result;
    }

    private async Task<string> GetBody(EmailType type, string token)
    {
        var html = type switch
        {
            EmailType.Verified => await File.ReadAllTextAsync("../Emails/Verified.html"),
            EmailType.ChangePassword => await File.ReadAllTextAsync("../Emails/Password.html"),
            _ => await File.ReadAllTextAsync("../Emails/Password.html")
        };

        return string.Format(html, token);
    }
}