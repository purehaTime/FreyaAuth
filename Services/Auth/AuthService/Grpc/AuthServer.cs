using AuthService.Interfaces;
using Grpc.Core;
using GrpcHelper.Proto.Auth;

namespace AuthService.Grpc;

public class AuthServer : Auth.AuthBase
{
    private readonly IAuthSeverHandler _handler;

    
    public AuthServer(IAuthSeverHandler handler)
    {
        _handler = handler;
    }

    public override async Task<TokensResponse> UpdateToken(TokenRequest request, ServerCallContext context)
    {
        return await _handler.UpdateToken(request);
    }
}