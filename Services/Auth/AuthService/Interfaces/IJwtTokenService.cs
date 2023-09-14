using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Interfaces;

public interface IJwtTokenService
{
    public Task<bool> VerifyToken(string token);
    public string GenerateAuthToken(string name, string email);
    public string GenerateRefreshToken();
    public string GenerateVerificationToken();
    public (string UserId, string Email) GetUserData(string token);
}