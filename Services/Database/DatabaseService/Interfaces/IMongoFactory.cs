using MongoDB.Driver;

namespace DatabaseService.Interfaces;

public interface IMongoFactory
{
    IMongoClient GetClient();
}