using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RedisSetCache : IRedisSetCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisSetCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
        }

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long Set(string key, params string[] value)
        {
            if (value == null || value.Length == 0)
                throw new ArgumentNullException(nameof(value), "");

            return CSRedisCore.SAdd(key, value);
        }

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long Set<T>(string key, params T[] value) where T : class
        {
            if (value == null || value.Length == 0)
                throw new ArgumentNullException(nameof(value), "");

            var list = new List<string>();
            foreach (var item in value)
            {
                list.Add(JsonConvert.SerializeObject(item, JsonSetting));
            }
            return CSRedisCore.SAdd(key, list.ToArray());
        }

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> SetAsync(string key, params string[] value)
        {
            if (value == null || value.Length == 0)
                throw new ArgumentNullException(nameof(value), "");

            return await CSRedisCore.SAddAsync(key, value);
        }

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> SetAsync<T>(string key, params T[] value) where T : class
        {
            var list = new List<string>();
            foreach (var item in value)
            {
                list.Add(JsonConvert.SerializeObject(item, JsonSetting));
            }

            return await CSRedisCore.SAddAsync(key, list.ToArray());
        }

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Count(string key) => CSRedisCore.SCard(key);

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> CountAsync(string key) => await CSRedisCore.SCardAsync(key);

        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Exists(string key, string value) => CSRedisCore.SIsMember(key, value);

        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key, string value) => await CSRedisCore.SIsMemberAsync(key, value);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        public string GetRandom(string key) => CSRedisCore.SPop(key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        public T GetRandom<T>(string key) where T : class
        {
            var cache = CSRedisCore.SPop(key);
            if (string.IsNullOrEmpty(cache))
                return default;

            return JsonConvert.DeserializeObject<T>(cache, JsonSetting);
        }

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        public async Task<string> GetRandomAsync(string key) => await CSRedisCore.SPopAsync(key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        public async Task<T> GetRandomAsync<T>(string key) where T : class
        {
            var cache = await CSRedisCore.SPopAsync(key);
            if (string.IsNullOrEmpty(cache))
                return default;

            return JsonConvert.DeserializeObject<T>(cache, JsonSetting);
        }

        /// <summary>
        /// 获取集合的所有元素（用于展示）
        /// </summary>
        /// <param name="setKey">集合的 Redis 键</param>
        /// <returns>集合中的所有元素</returns>
        public async Task<List<string>> GetAllAsync(string setKey)
        {
            // 使用 SMEMBERS 获取集合所有元素
            var items = await CSRedisCore.SMembersAsync(setKey);
            return items.ToList();
        }
    }
}
