using GrpcHelper.Clients.Interfaces;
using GrpcHelper.Proto.Auth;
using GrpcHelper.Proto.Db;

namespace GrpcHelper.Clients;

public class AuthServiceClient : IAuthServiceClient
{
    private Auth.AuthClient _client;
    
    public AuthServiceClient(Auth.AuthClient client)
    {
        _client = client;
    }

    public async Task<TokensResponse> ValidateUser(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<TokensResponse> UpdateTokens(string authToken, string refreshToken)
    {
        var response = await _client.UpdateTokenAsync(new TokenRequest
        {
            AuthToken = authToken,
            RefreshToken = refreshToken,
        });

        return response;
    }
}