using ConfigHelper.Configs;
using DatabaseService.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseService.Services;

public class MongoFactory : IMongoFactory
{
    private readonly DatabaseSettings _dbSettings;

    public MongoFactory(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }
    
    public IMongoClient GetClient()
    {
        var settings = MongoClientSettings.FromConnectionString($"{_dbSettings.Server}:{_dbSettings.Port}");
        var client = new MongoClient(settings);
        return client;
    }
}