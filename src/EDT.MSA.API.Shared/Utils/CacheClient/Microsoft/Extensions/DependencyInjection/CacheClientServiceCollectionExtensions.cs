using EDT.MSA.API.Shared.Utils;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CacheClientServiceCollectionExtensions
    {
        public static void AddRedisClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRedisClientSettings(configuration);
            services.AddSingleton<IRedisCacheClient, CSRedisCoreClient>();
        }

        public static void AddRedisClientSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = new RedisSettings();
            configuration.GetSection(nameof(RedisSettings)).Bind(redisSettings);

            // redis serveraddresses adapter
            if (string.IsNullOrWhiteSpace(redisSettings.Hosts) && redisSettings.SentinelHosts == null)
            {
                var serverAddressesStr = configuration["RedisClientOptions:SentinelHosts"];
                var serverAddresses = serverAddressesStr?.Split(',');
                redisSettings.SentinelHosts = serverAddresses ??
                    throw new ArgumentNullException("Redis SentinelHosts为空！");
            }

            services.AddSingleton<IRedisSettings>(redisSettings);
        }
    }
}
