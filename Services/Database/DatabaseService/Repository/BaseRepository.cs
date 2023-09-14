using ConfigHelper.Configs;
using ConfigHelper.Interfaces;
using DatabaseService.Interfaces;
using DatabaseService.Interfaces.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseService.Repository;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly Serilog.ILogger _logger;
    private readonly IMongoClient _mongoClient;
    private readonly IOptions<DatabaseSettings> _dbConfig;
    private readonly IMongoCollection<TEntity> _collection;

    protected BaseRepository(IMongoFactory factory, IOptions<DatabaseSettings> dbConfig, Serilog.ILogger logger)
    {
        _logger = logger;
        _mongoClient = factory.GetClient();
        _dbConfig = dbConfig;

        _collection = InitCollection();
    }

    public async Task<TEntity> Find(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken ct)
    {
        var result = await _collection
            .Find(filter, options)
            .FirstOrDefaultAsync(ct);

        return result;
    }

    public async Task<bool> Insert(TEntity entity, InsertOneOptions options, CancellationToken ct)
    {
        await _collection.InsertOneAsync(entity, options, ct);
        return true;
    }

    public async Task<UpdateResult> Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions option, CancellationToken ct)
    {
        var result = await _collection.UpdateOneAsync(filter, updateDefinition, option, ct);

        return result;
    }

    public async Task<DeleteResult> Delete(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken ct)
    {
        var result = await _collection.DeleteOneAsync(filter, options, ct);

        return result;
    }

    public async Task<List<TEntity>> FindMany(FilterDefinition<TEntity> filter, FindOptions options, int? limit, CancellationToken ct)
    {
        var result = await _collection
            .Find(filter, options)
            .Limit(limit)
            .ToListAsync(ct);

        return result;
    }
    
    public async Task<bool> InsertMany(IEnumerable<TEntity> entity, InsertManyOptions options, CancellationToken ct)
    {
        await _collection.InsertManyAsync(entity, options, ct);
        return true;
    }
    
    public async Task<UpdateResult> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions option, CancellationToken ct)
    {
        var result = await _collection.UpdateManyAsync(filter, updateDefinition, option, ct);
        return result;
    }

    public async Task<DeleteResult> DeleteMany(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken ct)
    {
        var result = await _collection.DeleteManyAsync(filter, options, ct);
        return result;
    }

    private IMongoCollection<TEntity> InitCollection()
    {
        var collection = _mongoClient
            .GetDatabase(_dbConfig.Value.CollectionName)
            .GetCollection<TEntity>(typeof(TEntity).Name + "s");
        
        return collection;
    }
}