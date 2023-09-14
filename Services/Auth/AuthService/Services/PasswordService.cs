using Amazon.Runtime.Internal.Transform;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<string> _passwordHasher;
    
    public PasswordService(IPasswordHasher<string> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public string GetHashedPassword(string email,  string password)
    {
        return _passwordHasher.HashPassword(email, password);
    }

    public bool VerifyPassword(string email, string hash, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(email, hash, password);
        return result != PasswordVerificationResult.Failed;
    }
}