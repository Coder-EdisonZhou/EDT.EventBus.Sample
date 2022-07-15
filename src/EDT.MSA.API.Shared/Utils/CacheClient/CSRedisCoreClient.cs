using CSRedis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.API.Shared.Utils
{
    /// <summary>
    /// 请将CSRedisCoreClient注册为单例
    /// </summary>
    public class CSRedisCoreClient : IRedisCacheClient
    {
        protected readonly CSRedisClient _redisClient;
        protected readonly IRedisSettings _redisSettings;
        protected readonly ILogger<IRedisCacheClient> _logger;

        private readonly Func<object, string> _jsonSerializer = (value) =>
        {
            var serializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
            };

            return value != null ?
                JsonConvert.SerializeObject(value, serializerSettings) : null;
        };

        public CSRedisCoreClient(IRedisSettings settings, ILogger<IRedisCacheClient> logger)
        {
            _redisSettings = settings;
            _logger = logger;

            if (_redisSettings.SentinelHosts == null)
            {
                // 单机模式
                _redisClient = new CSRedisClient(_redisSettings.GetConnectionString());
            }
            else
            {
                // 哨兵集群模式
                _redisClient = new CSRedisClient(_redisSettings.GetConnectionString(), _redisSettings.SentinelHosts);
            }

            // 使用自定义方法序列化：避免自循环
            CSRedisClient.Serialize = _jsonSerializer;
        }

        public async Task<bool> SetAsync(string key, object value, int? expireSeconds = null)
        {
            try
            {
                return (expireSeconds == null) ?
                    await _redisClient.SetAsync(key, value, _redisSettings.ExpireSeconds) :
                    await _redisClient.SetAsync(key, value, expireSeconds.Value);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> GetAsync(string key)
        {
            try
            {
                return await _redisClient.GetAsync(key);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return string.Empty;
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                return await _redisClient.GetAsync<T>(key);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return default(T);
            }
        }

        public async Task<IList<T>> GetFilteredListAsync<T>(string pattern)
        {
            try
            {
                var keys = await _redisClient.KeysAsync(pattern);
                var filteredList = await _redisClient.MGetAsync<T>(keys);

                return filteredList;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return default(IList<T>);
            }
        }

        public async Task<bool> DeleteAsync(params string[] keys)
        {
            try
            {
                return await _redisClient.DelAsync(keys) > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return false;
            }
        }

        public async Task<long> IncByAsync(string key, long value = 1)
        {
            try
            {
                return await _redisClient.IncrByAsync(key, value);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return default(long);
            }
        }

        public async Task<T> Lock<T>(string key, Func<Task<T>> func, int expire = 60)
        {
            try
            {
                using (_redisClient.Lock(key, expire))
                {
                    return await func();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return default(T);
            }
        }

        public async Task<bool> Unlock(object redisClientLock)
        {
            try
            {
                var csRedisClientLock = redisClientLock as CSRedisClientLock;
                return csRedisClientLock.Unlock();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return false;
            }
        }
    }
}
