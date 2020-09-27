using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.DependencyResolvers.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolvers.Concrete
{
    public class CoreModule: IDependencyResolverModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddSingleton<ICacheManager, RedisMemoryCacheManager>();
            services.AddDistributedRedisCache(t =>
            {
                t.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    EndPoints = { "redis:6379" },
                    Password = "1234",
                    Ssl = false
                };
            }
            );
        }
    }
}