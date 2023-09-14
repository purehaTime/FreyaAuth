using DatabaseService.Interfaces.Repository;
using DatabaseService.Interfaces.Services;
using GrpcHelper.Proto.Db;
using Models.Common;
using Models.DbModels;
using Models.Enums;
using Models.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;
using User = Models.DbModels.User;

namespace DatabaseService.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly ILogger _logger;
    
    public UserService(IRepository<User> userRepository, ILogger logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    public async Task<(bool Created, Error Error)> CreateUser(User newUser)
    {
        try
        {
            var result = await _userRepository.Insert(newUser, null, CancellationToken.None);
            _logger.Information("Created user {NewUserId}", newUser.Id);
            return (result, null);
        }
        catch (MongoException err)
        {
            _logger.Error(err, "Can\'t insert user {NewUserId}", newUser.Id);
        }

        return (false, new Error { Level = ErrorLevel.Medium, Message = "User can't be created" });
    }

    public async Task<(bool Created, Error Error)> VerifyUser(ObjectId userId)
    {
        var update = Builders<User>.Update.Set(s => s.Verified, true);
        var filter = Builders<User>.Filter.Eq(f => f.Id, userId);
        var result = await _userRepository.Update(filter, update, null, CancellationToken.None);

        return result.MatchedCount == 1 
            ? (true, null) 
            : (false, new Error() { Level = ErrorLevel.Medium, Message = "Can't Verify User" });
    }

    public async Task<(User User, Error Error)> GetUser(ObjectId id)
    {
        throw new NotImplementedException();
    }

    public async Task<(User User, Error Error)> FindUser(FindUser findUser)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(e => e.Email, findUser.Email);
            var result = await _userRepository.Find(filter, null, CancellationToken.None);
            return (result, null);
        }
        catch (MongoException err)
        {
            _logger.Error(err, "Can\'t find user");
        }

        return (new User(), new Error { Level = ErrorLevel.Medium, Message = "User can't be created" });
    }

    public async Task<(List<User> Users, Error Error)> FindUsers(string searchData)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool Banned, Error Error)> BanUser(ObjectId id, string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool Unbanned, Error Error)> UnbanUser(ObjectId id)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool Locked, Error Error)> LockUser(ObjectId id)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool Updated, Error Error)> UpdatePassword(ObjectId id, string hash)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool Updated, Error Error)> UpdateToken(ObjectId id, string refreshToken, DateTime expired)
    {
        var updateToken = Builders<User>.Update.Set(s => s.RefreshToken, refreshToken);
        var updateDate = Builders<User>.Update.Set(s => s.RefreshTokenExpire, expired);
        var updates = Builders<User>.Update.Combine(updateToken, updateDate);
        
        var filter = Builders<User>.Filter.Eq(f => f.Id, id);
        var result = await _userRepository.Update(filter, updates, null, CancellationToken.None);
        
        return result.MatchedCount == 1 
            ? (true, null) 
            : (false, new Error() { Level = ErrorLevel.Medium, Message = $"Can't Update token for user {id}" });
    }
}