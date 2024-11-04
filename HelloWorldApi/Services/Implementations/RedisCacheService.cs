using System;
using System.Threading.Tasks;
using HelloWorldApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace HelloWorldApi.Services.Implementations;

public class RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger): ICacheService
{
    public async Task<string?> GetAsync(string key)
    {
         var currentValue = await cache.GetStringAsync(key);
         if (currentValue is not null) return currentValue;
         currentValue = "http";
         await SetAsync(key, currentValue);
         return currentValue;
    }

    public async Task SetAsync(string key, string value)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        };
        await cache.SetStringAsync(key, value, options);
        logger.LogInformation($"Set {key} to {value}");
    }

    public async Task SwapAsync(string key)
    {
        var currentValue = await GetAsync(key);
        if (currentValue is null or "http")
        {
            await SetAsync(key, "kafka");
        }
        else
        {
            await SetAsync(key, "http");
        }
        
        
    }
}