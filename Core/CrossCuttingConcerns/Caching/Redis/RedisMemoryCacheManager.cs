using Core.Utilities.IoC.ServiceTools;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisMemoryCacheManager : ICacheManager
    {
        private ConfigurationOptions options = new ConfigurationOptions
        {
            EndPoints = { "redis:6379" },
            Password = "1234",
            Ssl = false,
        };

        private readonly IDistributedCache _distributedCache;

        public RedisMemoryCacheManager()
        {
            _distributedCache = ServiceTool.ServiceProvider.GetService<IDistributedCache>();
        }

        public async Task Add(string key, object data, int durationMinutes)
        {
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data), new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddMinutes(durationMinutes) });
        }

        public async Task<T> Get<T>(string key)
        {
            var response = await _distributedCache.GetStringAsync(key) ?? "";
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<object> Get(string key)
        {
            return JsonConvert.DeserializeObject<object>(await _distributedCache.GetStringAsync(key));
        }

        public async Task<bool> IsAdded(string key)
        {
            using (var redisConnection = ConnectionMultiplexer.Connect(options))
            {
                var server = redisConnection.GetServer(options.EndPoints.FirstOrDefault());
                return await redisConnection.GetDatabase().KeyExistsAsync(key);
            }
        }

        public async Task Remove(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task RemoveByPattern(string pattern)
        {
            using (var redisConnection = ConnectionMultiplexer.Connect(options))
            {
                var server = redisConnection.GetServer(options.EndPoints.FirstOrDefault());
                foreach (var key in server.Keys(pattern: pattern))
                {
                    await redisConnection.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }
    }
}