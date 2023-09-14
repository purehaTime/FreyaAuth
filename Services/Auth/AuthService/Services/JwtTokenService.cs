using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Interfaces;
using ConfigHelper.Configs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    
    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<bool> VerifyToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validate = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey))
        });

        return validate.IsValid;

    }

    public string GenerateAuthToken(string userId, string email)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Sid, userId),
            new (ClaimTypes.Email, email)
        };
        
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey));
        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AuthExpires),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string GenerateRefreshToken()
    {
        return GenerateToken(64);
    }

    public string GenerateVerificationToken()
    {
        return GenerateToken(16);
    }

    public (string UserId, string Email) GetUserData(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var id = jwtToken.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Sid);
        var email = jwtToken.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email);

        return (id?.Value, email?.Value);
    }

    private string GenerateToken(int size)
    {
        var bytes = new byte[size];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(bytes);
        var refreshToken = Convert.ToBase64String(bytes);
        return refreshToken;
    }
}