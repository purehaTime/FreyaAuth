using ConfigHelper.Configs;
using ConfigHelper.Interfaces;
using DatabaseService.Interfaces;
using Microsoft.Extensions.Options;
using Models.DbModels;
using MongoDB.Driver;

namespace DatabaseService.Repository;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(IMongoFactory factory, IOptions<DatabaseSettings> dbConfig, Serilog.ILogger logger) : base(factory, dbConfig, logger)
    {
    }
}