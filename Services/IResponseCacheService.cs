using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
        Task RemoveCacheAsync(string requestPath);
        Task RemoveAllCacheAsync();
    }

    public class ResponseCacheService : IResponseCacheService
    {
        
        private readonly IConnectionMultiplexer _connectionMutiplexer;

        public ResponseCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            
            _connectionMutiplexer = connectionMultiplexer;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive)
        {
            if (response == null) return;

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serializerSetting = new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            };
            var serializeResponse = JsonConvert.SerializeObject(response,serializerSetting);
            var db = _connectionMutiplexer.GetDatabase();
            await db.StringSetAsync(cacheKey, serializeResponse);
            await db.KeyExpireAsync(cacheKey, timeTimeLive);
            //await _distributedCache.SetStringAsync(cacheKey, serializeResponse, new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = timeTimeLive
            //});
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var db = _connectionMutiplexer.GetDatabase();
            string stringVal = await db.StringGetAsync(cacheKey);
            //var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            return !string.IsNullOrEmpty(stringVal) ? stringVal : null;
        }

        public async Task RemoveCacheAsync(string requestPath)
        {
            var db = _connectionMutiplexer.GetDatabase();
            var server = _connectionMutiplexer.GetServer("beanlancer.redis.cache.windows.net:6380");
            var keys = server.Keys(pattern: $"*{requestPath}*").ToArray();
            await db.KeyDeleteAsync(keys);
        }

        public async Task RemoveAllCacheAsync()
        {
            var db = _connectionMutiplexer.GetDatabase();
            var server = _connectionMutiplexer.GetServer("beanlancer.redis.cache.windows.net:6380");
            var keys = server.Keys().ToArray();
            await db.KeyDeleteAsync(keys);
        }
    }
}
