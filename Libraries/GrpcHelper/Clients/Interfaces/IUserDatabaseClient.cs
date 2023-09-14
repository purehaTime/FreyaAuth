using GrpcHelper.Proto.Common;
using GrpcHelper.Proto.Db;

namespace GrpcHelper.Clients.Interfaces;

public interface IUserDatabaseClient
{
    public Task<(bool Created, Error Error)> CreateUser(UserModel newUser);
    public Task<(string Token, DateTime Expired, Error Error)> GetRefreshToken(string id);
    public Task<(bool Updated, Error Error)> UpdateRefreshToken(string id, string token);
    public Task<(UserModel User, Error Error)> GetUserByEmail(string email);
}