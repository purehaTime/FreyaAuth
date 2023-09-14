using System.ComponentModel.DataAnnotations;
using AuthService.Interfaces;
using ConfigHelper.Configs;
using Google.Protobuf.WellKnownTypes;
using GrpcHelper.Clients.Interfaces;
using GrpcHelper.Proto.Db;
using Microsoft.Extensions.Options;
using Models.Enums;
using Models.Models.AuthService;
using OAuthType = GrpcHelper.Proto.Db.OAuthType;
using Roles = GrpcHelper.Proto.Db.Roles;
using ILogger = Serilog.ILogger;

namespace AuthService.Services;

public class UserService : IUserService
{
    private readonly IUserDatabaseClient _userDatabaseClient;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly IEmailSender _emailSender;
    private readonly Service _services;
    private readonly ILogger _logger;
    public UserService(IUserDatabaseClient userDatabaseClient,
        IPasswordService passwordService,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings,
        IOptions<Service> services,
        IEmailSender emailSender,
        ILogger logger)
    {
        _userDatabaseClient = userDatabaseClient;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
        _services = services.Value;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<UserAuthResult> Register(AuthModel authData)
    {
        var result = new UserAuthResult();
        var emailValidation = new EmailAddressAttribute();
        if ((string.IsNullOrWhiteSpace(authData.Email) && emailValidation.IsValid(authData.Email)) 
            || string.IsNullOrWhiteSpace(authData.Password) || authData.Password.Length < 5)
        {
            result.Message = "Incorrect email or password too short";
            return result;
        }

        var oldUser = await _userDatabaseClient.GetUserByEmail(authData.Email);
        if (oldUser.User != null)
        {
            result.Message = "Incorrect email or password too short";
            return result;
        }

        var hash = _passwordService.GetHashedPassword(authData.Email, authData.Password);
        if (string.IsNullOrWhiteSpace(hash))
        {
            result.Message = "Incorrect email or password too short";
            return result;
        }
        
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshExpire = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpires).ToTimestamp();
        var verificationToken = _jwtTokenService.GenerateVerificationToken();
        var verificationTokenExpire = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpires).ToTimestamp();

        var newUser = new UserModel
        {
            Id = "",
            Email = authData.Email,
            PasswordHash = hash,
            PasswordChanged = DateTime.UtcNow.ToTimestamp(),
            Created = DateTime.UtcNow.ToTimestamp(),
            Locked = false,
            Role = Roles.User,
            Verified = false,
            DisplayName = authData.Email?.Split('@').FirstOrDefault(),
            RefreshToken = refreshToken,
            RefreshTokenExpire = refreshExpire,
            VerificationToken = verificationToken,
            VerificationTokenExpire = verificationTokenExpire
        };
        
        var userResult = await _userDatabaseClient.CreateUser(newUser);
        if (userResult.Created)
        {
            var mailResult = await _emailSender.SendVerificationMail(verificationToken, authData.Email, EmailType.Verified);
            if (!string.IsNullOrWhiteSpace(mailResult.ErrorMessage))
            {
                _logger.Information("Can't send mail in creating user");
                result.Message = mailResult.ErrorMessage;
                return result;
            }

            result.Message = mailResult.SendToUser
                ? $"Complete! Verification link was send to {authData.Email}"
                : $"Complete! Please follow link to verify email: {_services.AuthPageLink}/Verify/{verificationToken}";
            
            _logger.Information("Creating user was complete successful");
            return result;
        }

        _logger.Information("New user not created");
        result.Message = "Something was wrong..";
        return result;
    }

    public async Task<UserAuthResult> Login(AuthModel loginData)
    {
        var result = new UserAuthResult();
        if (string.IsNullOrWhiteSpace(loginData.Email) || string.IsNullOrWhiteSpace(loginData.Password))
        {
            result.IsSuccess = false;
            result.Message = "email or password empty";
            return result;
        }

        var user = await _userDatabaseClient.GetUserByEmail(loginData.Email);


        return new UserAuthResult();
    }
}