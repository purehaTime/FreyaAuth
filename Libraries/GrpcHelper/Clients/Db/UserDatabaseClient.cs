using GrpcHelper.Clients.Interfaces;
using GrpcHelper.Proto.Common;
using GrpcHelper.Proto.Db;

namespace GrpcHelper.Clients.Db;

public class UserDatabaseClient : IUserDatabaseClient
{
    private readonly User.UserClient _userClient;
    
    public UserDatabaseClient(User.UserClient userClient)
    {
        _userClient = userClient;
    }
    
    public async Task<(bool Created, Error Error)> CreateUser(UserModel newUser)
    {
        var result = await _userClient.CreateUserAsync(newUser);
        return (result.Complete, result.Error);
    }

    public async Task<(string Token, DateTime Expired, Error Error)> GetRefreshToken(string userId)
    {
        var result = await _userClient.GetRefreshTokenAsync(new Id
        {
            ObjectId = userId
        });

        return (result.Token, result.Expired.ToDateTime(), result.Error);
    }

    public async Task<(bool Updated, Error Error)> UpdateRefreshToken(string id, string token)
    {
        var result = await _userClient.UpdateRefreshTokenAsync(new UpdateTokenRequest
        {
            Token = token,
            UserId = id,
        });

        return (result.Complete, result.Error);
    }

    public async Task<(UserModel User, Error Error)> GetUserByEmail(string email)
    {
        var result = await _userClient.GetUserByEmailAsync(new EmailRequest
        {
            Email = email
        });

        return (result.User, result.Error);
    }
}