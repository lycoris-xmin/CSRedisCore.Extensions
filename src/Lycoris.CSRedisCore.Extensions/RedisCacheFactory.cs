using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// Redis 多实例缓存工厂，通过实例名称获取对应的 RedisCacheService
    /// </summary>
    public static class RedisCacheFactory
    {
        /// <summary>
        /// 各实例的 JSON 序列化设置
        /// </summary>
        internal static Dictionary<string, JsonSerializerSettings> RedisJsonSerializerSettings;

        /// <summary>
        /// 各实例的 Redis 客户端
        /// </summary>
        internal static Dictionary<string, CSRedisClient> Clients;

        /// <summary>
        /// 各实例的缓存键前缀
        /// </summary>
        internal static Dictionary<string, string> PrefixCackeKeys = new Dictionary<string, string>();

        /// <summary>
        /// 各实例的缓存服务实例缓存
        /// </summary>
        internal static Dictionary<string, RedisCacheService> Instances = new Dictionary<string, RedisCacheService>();

        /// <summary>
        /// 根据实例名称获取对应的 Redis 缓存服务实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>Redis 缓存服务实例</returns>
        /// <exception cref="ArgumentNullException">实例不存在时抛出</exception>
        public static RedisCacheService GetInstance(string name)
        {
            if (Clients == null || !Clients.ContainsKey(name))
                throw new ArgumentNullException();

            if (Instances == null)
                Instances = new Dictionary<string, RedisCacheService>();

            if (!Instances.ContainsKey(name))
            {
                Instances.Add(name, new RedisCacheService(Clients[name], RedisJsonSerializerSettings[name], PrefixCackeKeys[name]));
            }

            return Instances[name];
        }
    }
}
