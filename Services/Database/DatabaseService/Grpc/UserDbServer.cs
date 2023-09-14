using System.Data.Common;
using DatabaseService.Interfaces.Handlers;
using Grpc.Core;
using GrpcHelper.Proto.Common;
using GrpcHelper.Proto.Db;
using Status = GrpcHelper.Proto.Common.Status;

namespace DatabaseService.Grpc;

public class UserDbServer : User.UserBase
{
    private readonly IUserDbServerHandler _handler;
    
    public UserDbServer(IUserDbServerHandler handler)
    {
        _handler = handler;
    }
    
    public override async Task<RefreshTokenResponse> GetRefreshToken(Id request, ServerCallContext context)
    {
        return await _handler.GetRefreshToken(request);
    }

    public override async Task<Status> UpdateRefreshToken(UpdateTokenRequest request, ServerCallContext context)
    {
        return await _handler.UpdateRefreshToken(request);
    }

    public override async Task<UserModelResponse> GetUserByEmail(EmailRequest request, ServerCallContext context)
    {
        return await _handler.GetUserByEmail(request);
    }

    public override async Task<Status> CreateUser(UserModel request, ServerCallContext context)
    {
        return await _handler.CreateUser(request);
    }
}