using CSRedis;
using Lycoris.CSRedisCore.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class CSRedisCoreBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configure"></param>
        public static void AddSingleRedisInstance(Action<RedisConiguration> configure)
        {
            var configuretaion = new RedisConiguration();
            configure(configuretaion);
            RedisCache.Client = configuretaion.HasSentinels ? new CSRedisClient(configuretaion.ToString(), configuretaion.Sentinels) : new CSRedisClient(configuretaion.ToString());
            RedisCache.PrefixCacheKey = configuretaion.Prefix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="configure"></param>
        public static void AddMultipleRedisInstance(string instanceName, Action<RedisConiguration> configure)
        {
            var configuretaion = new RedisConiguration();
            configure.Invoke(configuretaion);

            if (RedisCacheFactory.Clients == null)
                RedisCacheFactory.Clients = new Dictionary<string, CSRedisClient>();

            RedisCacheFactory.Clients.Add(instanceName, configuretaion.HasSentinels ? new CSRedisClient(configuretaion.ToString(), configuretaion.Sentinels) : new CSRedisClient(configuretaion.ToString()));

            RedisCacheFactory.PrefixCackeKeys.Add(instanceName, configuretaion.Prefix);

            if (RedisCacheFactory.RedisJsonSerializerSettings == null)
                RedisCacheFactory.RedisJsonSerializerSettings = new Dictionary<string, JsonSerializerSettings>();

            RedisCacheFactory.RedisJsonSerializerSettings.Add(instanceName, configuretaion.NewtonsoftJsonSerializerSettings);
        }
    }
}
