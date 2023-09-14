using GrpcHelper.Proto.Common;
using GrpcHelper.Proto.Db;

namespace DatabaseService.Interfaces.Handlers;

public interface IUserDbServerHandler
{
    public Task<RefreshTokenResponse> GetRefreshToken(Id request);
    public Task<Status> UpdateRefreshToken(UpdateTokenRequest request);
    public Task<UserModelResponse> GetUserByEmail(EmailRequest request);
    public Task<Status> CreateUser(UserModel request);
}