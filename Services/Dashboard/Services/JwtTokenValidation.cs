using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ConfigHelper.Configs;
using Dashboard.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Services;

public class JwtTokenValidation : IJwtTokenValidation
{
    private readonly JwtSettings _jwtSettings;
    
    public JwtTokenValidation(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<bool> Validate(string token)
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
}