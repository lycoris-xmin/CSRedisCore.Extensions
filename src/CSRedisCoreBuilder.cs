using CSRedis;
using Lycoris.CSRedisCore.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// Redis 客户端构建器，用于注册单实例或多实例 Redis 连接
    /// </summary>
    public class CSRedisCoreBuilder
    {
        /// <summary>
        /// 注册单个 Redis 实例，配置连接到 RedisCache.Client 静态入口
        /// </summary>
        /// <param name="configure">Redis 配置委托</param>
        public static void AddSingleRedisInstance(Action<RedisConiguration> configure)
        {
            var configuretaion = new RedisConiguration();
            configure(configuretaion);
            RedisCache.Client = configuretaion.HasSentinels ? new CSRedisClient(configuretaion.ToString(), configuretaion.Sentinels) : new CSRedisClient(configuretaion.ToString());
            RedisStore.SetPrefixCacheKey(configuretaion.Prefix);
            RedisStore.SetJsonSerializerSettings(configuretaion.NewtonsoftJsonSerializerSettings);
        }

        /// <summary>
        /// 注册多个 Redis 实例，通过 RedisCacheFactory.GetInstance(instanceName) 获取对应实例
        /// </summary>
        /// <param name="instanceName">实例名称，用于后续获取实例</param>
        /// <param name="configure">Redis 配置委托</param>
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

            RedisCacheFactory.RedisJsonSerializerSettings.Add(instanceName, configuretaion.NewtonsoftJsonSerializerSettings ?? RedisStore.Settings);
        }
    }
}
