using Microsoft.Extensions.Caching.Distributed;
using RedisHelper.Interfaces;
using Serilog;
using SpanJson;

namespace RedisHelper.Services;

public class CacheManager : ICacheManager
{
    private readonly IDistributedCache _redisCache;
    private readonly ILogger _logger;

    public CacheManager(IDistributedCache redisCache, ILogger logger)
    {
        _redisCache = redisCache;
        _logger = logger;
    }

    public async Task Set<T>(string key, T data, CancellationToken ct) where T : class
    {
        try
        {
            var serializedData = JsonSerializer.Generic.Utf16.Serialize(data);
            await _redisCache.SetStringAsync(key, serializedData, ct);
        }
        catch (Exception err)
        {
            _logger.Error(err, "CacheManager.Set: Problem with setup cache");
        }
    }

    public async Task<T> Get<T>(string key, CancellationToken ct) where T : class
    {
        try
        {
            var data = await _redisCache.GetStringAsync(key, ct);
            var result = JsonSerializer.Generic.Utf16.Deserialize<T>(data);
            return result;
        }
        catch (Exception err)
        {
            _logger.Error(err, "CacheManager.Get: Problem with getting cache");
        }

        return null;
    }

    public async Task Refresh(string key, CancellationToken ct)
    {
        await _redisCache.RefreshAsync(key, ct);
    }
}