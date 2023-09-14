namespace RedisHelper.Interfaces;

public interface ICacheManager
{
    public Task Set<T>(string key, T data, CancellationToken ct) where T : class;
    public Task<T> Get<T>(string key, CancellationToken ct) where T : class;
    public Task Refresh(string key, CancellationToken ct);
}