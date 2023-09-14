using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisHelper.Interfaces;
using RedisHelper.Services;
using StackExchange.Redis;

namespace RedisHelper;

public static class RedisHelperExtension
{
    public static void UseRedis(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL");
            var redisPass = Environment.GetEnvironmentVariable("REDIS_PASS");
            
            options.Configuration = redisUrl;
            options.ConfigurationOptions = new ConfigurationOptions()
            {
                Password = redisPass,
            };
        });

        builder.Services.AddSingleton<ICacheManager, CacheManager>();
    }
}