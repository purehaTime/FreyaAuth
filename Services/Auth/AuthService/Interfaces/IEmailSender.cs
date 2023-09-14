using Models.Enums;
using Models.Models.AuthService;

namespace AuthService.Interfaces;

public interface IEmailSender
{
    public Task<EmailRegisterResult> SendVerificationMail(string token, string to, EmailType type);
}