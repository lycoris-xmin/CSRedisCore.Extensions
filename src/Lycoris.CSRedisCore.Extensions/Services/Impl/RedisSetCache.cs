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
        private readonly string PrefixCacheKey;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        /// <param name="prefixCacheKey"></param>
        public RedisSetCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting, string prefixCacheKey)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
            PrefixCacheKey = prefixCacheKey;
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

        public async Task<List<string>> GetAllAsync(string setKey) => (await CSRedisCore.SMembersAsync(setKey)).ToList();

        public List<string> GetAll(string setKey) => CSRedisCore.SMembers(setKey).ToList();

        public long Remove(string key, params string[] value) => CSRedisCore.SRem(key, value);

        public async Task<long> RemoveAsync(string key, params string[] value) => await CSRedisCore.SRemAsync(key, value);

        public long Remove<T>(string key, params T[] value) where T : class
        {
            var list = new List<string>();
            foreach (var item in value)
                list.Add(JsonConvert.SerializeObject(item, JsonSetting));
            return CSRedisCore.SRem(key, list.ToArray());
        }

        public async Task<long> RemoveAsync<T>(string key, params T[] value) where T : class
        {
            var list = new List<string>();
            foreach (var item in value)
                list.Add(JsonConvert.SerializeObject(item, JsonSetting));
            return await CSRedisCore.SRemAsync(key, list.ToArray());
        }

        public string RandomMember(string key) => CSRedisCore.SRandMember(key);

        public async Task<string> RandomMemberAsync(string key) => await CSRedisCore.SRandMemberAsync(key);

        public T RandomMember<T>(string key) where T : class
        {
            var str = CSRedisCore.SRandMember(key);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        public async Task<T> RandomMemberAsync<T>(string key) where T : class
        {
            var str = await CSRedisCore.SRandMemberAsync(key);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        public string[] RandomMembers(string key, int count) => CSRedisCore.SRandMembers(key, count);

        public async Task<string[]> RandomMembersAsync(string key, int count) => await CSRedisCore.SRandMembersAsync(key, count);

        public bool Move(string sourceKey, string targetKey, string value) => CSRedisCore.SMove(sourceKey, targetKey, value);

        public async Task<bool> MoveAsync(string sourceKey, string targetKey, string value) => await CSRedisCore.SMoveAsync(sourceKey, targetKey, value);

        public string[] Difference(params string[] keys) => CSRedisCore.SDiff(keys);

        public async Task<string[]> DifferenceAsync(params string[] keys) => await CSRedisCore.SDiffAsync(keys);

        public long DifferenceStore(string targetKey, params string[] keys) => CSRedisCore.SDiffStore(targetKey, keys);

        public async Task<long> DifferenceStoreAsync(string targetKey, params string[] keys) => await CSRedisCore.SDiffStoreAsync(targetKey, keys);

        public string[] Intersection(params string[] keys) => CSRedisCore.SInter(keys);

        public async Task<string[]> IntersectionAsync(params string[] keys) => await CSRedisCore.SInterAsync(keys);

        public long IntersectionStore(string targetKey, params string[] keys) => CSRedisCore.SInterStore(targetKey, keys);

        public async Task<long> IntersectionStoreAsync(string targetKey, params string[] keys) => await CSRedisCore.SInterStoreAsync(targetKey, keys);

        public string[] Union(params string[] keys) => CSRedisCore.SUnion(keys);

        public async Task<string[]> UnionAsync(params string[] keys) => await CSRedisCore.SUnionAsync(keys);

        public long UnionStore(string targetKey, params string[] keys) => CSRedisCore.SUnionStore(targetKey, keys);

        public async Task<long> UnionStoreAsync(string targetKey, params string[] keys) => await CSRedisCore.SUnionStoreAsync(targetKey, keys);

        public Models.RedisScanResult<string[]> Scan(string key, long cursor, string pattern = null, long? count = null)
        {
            var result = CSRedisCore.SScan(key, cursor, pattern, count ?? 100);
            return new Models.RedisScanResult<string[]> { Cursor = result.Cursor, Items = result.Items };
        }

        public async Task<Models.RedisScanResult<string[]>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            var result = await CSRedisCore.SScanAsync(key, cursor, pattern, count ?? 100);
            return new Models.RedisScanResult<string[]> { Cursor = result.Cursor, Items = result.Items };
        }
    }
}
