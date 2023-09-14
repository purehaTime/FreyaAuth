using GrpcHelper.Proto.Auth;

namespace AuthService.Interfaces;

public interface IAuthSeverHandler
{
    Task<TokensResponse> UpdateToken(TokenRequest request);
}