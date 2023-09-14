using Models.Common;
using Models.Models;
using MongoDB.Bson;
using User = Models.DbModels.User;

namespace DatabaseService.Interfaces.Services;

public interface IUserService
{
    public Task<(bool Created, Error Error)> CreateUser(User newUser);
    public Task<(bool Created, Error Error)> VerifyUser(ObjectId userId);
    public Task<(User User, Error Error)> GetUser(ObjectId id);
    public Task<(User User, Error Error)> FindUser(FindUser findUser);
    public Task<(List<User> Users, Error Error)> FindUsers(string searchData);
    public Task<(bool Banned, Error Error)> BanUser(ObjectId id, string reason);
    public Task<(bool Unbanned, Error Error)> UnbanUser(ObjectId id);
    public Task<(bool Locked, Error Error)> LockUser(ObjectId id);
    public Task<(bool Updated, Error Error)> UpdatePassword(ObjectId id, string hash);
    public Task<(bool Updated, Error Error)> UpdateToken(ObjectId id, string refreshToken, DateTime expired);
}