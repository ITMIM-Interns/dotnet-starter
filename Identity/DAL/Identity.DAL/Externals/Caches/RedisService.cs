using Identity.BLL.Abstractions.Externals;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Identity.DAL.Externals.Caches
{
    public sealed class RedisService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)=>_cache = cache;
        

        public async Task<T?> GetAsync<T>(string key)
        {
           string? data=await _cache.GetStringAsync(key);
            return data is null ? default : JsonSerializer.Deserialize<T>(data);
        }
        public async Task RemoveAsync(string key)=> await _cache.RemoveAsync(key);
        public async Task SetAsync<T>(string key, T value, TimeSpan time)=>
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = time });
    }
}
