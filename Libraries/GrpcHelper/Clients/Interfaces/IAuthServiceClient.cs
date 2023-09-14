using GrpcHelper.Proto.Auth;

namespace GrpcHelper.Clients.Interfaces;

public interface IAuthServiceClient
{
    public Task<TokensResponse> ValidateUser(string token);
    
    public Task<TokensResponse> UpdateTokens(string authToken, string refreshToken);
}