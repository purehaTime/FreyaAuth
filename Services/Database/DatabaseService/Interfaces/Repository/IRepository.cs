using MongoDB.Driver;

namespace DatabaseService.Interfaces.Repository;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity> Find (FilterDefinition<TEntity> filter, FindOptions options, CancellationToken ct);
    Task<bool> Insert(TEntity entity, InsertOneOptions options, CancellationToken ct);
    Task<UpdateResult> Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions option, CancellationToken ct);
    Task<DeleteResult> Delete(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken ct);
    Task<List<TEntity>> FindMany (FilterDefinition<TEntity> filter, FindOptions options, int? limit, CancellationToken ct);
    Task<bool> InsertMany(IEnumerable<TEntity> entity, InsertManyOptions options, CancellationToken ct);
    Task<UpdateResult> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition,
        UpdateOptions option, CancellationToken ct);
    Task<DeleteResult> DeleteMany (FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken ct);
}