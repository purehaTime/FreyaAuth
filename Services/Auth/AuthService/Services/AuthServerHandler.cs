using AuthService.Interfaces;
using GrpcHelper.Clients.Interfaces;
using GrpcHelper.Proto.Auth;
using GrpcHelper.Proto.Common;

namespace AuthService.Services;

public class AuthServerHandler : IAuthSeverHandler
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserDatabaseClient _dbClient;
    
    public AuthServerHandler(IUserDatabaseClient dbClient, IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _dbClient = dbClient;
    }

    public async Task<TokensResponse> UpdateToken(TokenRequest request)
    {
        var authToken = request.AuthToken;
        var isValid = await _jwtTokenService.VerifyToken(authToken);
        if (!isValid)
        {
            var user = _jwtTokenService.GetUserData(authToken);
            if (user.UserId != null && user.Email != null)
            {
                var dbToken = await _dbClient.GetRefreshToken(user.UserId);
                if (dbToken.Error == null)
                {
                    if (dbToken.Token == request.RefreshToken && DateTime.UtcNow < dbToken.Expired)
                    {
                        var refreshToken = _jwtTokenService.GenerateRefreshToken();
                        var tokenResult = await _dbClient.UpdateRefreshToken(user.UserId, refreshToken);
                        if (tokenResult.Updated)
                        {
                            var response = new TokensResponse()
                            {
                                AuthToken = _jwtTokenService.GenerateAuthToken(user.UserId, user.Email),
                                RefreshToken = refreshToken
                            };
                            return response;
                        }
                        
                        return new TokensResponse
                        {
                            Error = tokenResult.Error
                        };
                    }
                }
            }
        }

        return new TokensResponse
        {
            Error = new Error
            {
                Code = 10001,
                Message = "Token is invalid"
            }
        };
    }
}