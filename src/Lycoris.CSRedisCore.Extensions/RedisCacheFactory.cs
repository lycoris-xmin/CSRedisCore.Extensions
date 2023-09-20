using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class RedisCacheFactory
    {
        /// <summary>
        /// 
        /// </summary>
        internal static Dictionary<string, JsonSerializerSettings> RedisJsonSerializerSettings;

        /// <summary>
        /// 
        /// </summary>
        internal static Dictionary<string, CSRedisClient> Clients;

        /// <summary>
        /// 
        /// </summary>
        internal static Dictionary<string, RedisCacheService> Instances;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static RedisCacheService GetInstance(string name)
        {
            if (Clients == null || !Clients.ContainsKey(name))
                throw new ArgumentNullException();

            if (Instances == null)
                Instances = new Dictionary<string, RedisCacheService>();

            if (!Instances.ContainsKey(name))
            {
                Instances.Add(name, new RedisCacheService(Clients[name], RedisJsonSerializerSettings[name]));
            }

            return Instances[name];
        }
    }
}
