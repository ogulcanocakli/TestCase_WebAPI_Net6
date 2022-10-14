using Application.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheSettings.RedisURL;
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    EndPoints = { cacheSettings.RedisURL }
                };
            });

            services.AddTransient<ICacheService, RedisService>();
        }
    }
}
