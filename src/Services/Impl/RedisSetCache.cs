using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// Redis Set 缓存操作实现，提供集合成员的增删查、随机操作、集合运算（差集/交集/并集）等功能
    /// </summary>
    public sealed class RedisSetCache : IRedisSetCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;
        private readonly string PrefixCacheKey;

        /// <summary>
        /// 初始化 RedisSetCache 实例
        /// </summary>
        /// <param name="CSRedisCore">CSRedis 客户端实例</param>
        /// <param name="JsonSetting">JSON 序列化配置</param>
        /// <param name="prefixCacheKey">缓存键前缀</param>
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

        /// <summary>
        /// 获取集合中的所有成员
        /// </summary>
        /// <param name="setKey">集合键</param>
        /// <returns>集合中所有成员的列表</returns>
        public async Task<List<string>> GetAllAsync(string setKey) => (await CSRedisCore.SMembersAsync(setKey)).ToList();

        /// <summary>
        /// 获取集合中的所有成员
        /// </summary>
        /// <param name="setKey">集合键</param>
        /// <returns>集合中所有成员的列表</returns>
        public List<string> GetAll(string setKey) => CSRedisCore.SMembers(setKey).ToList();

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员</param>
        /// <returns>被移除的成员数量</returns>
        public long Remove(string key, params string[] value) => CSRedisCore.SRem(key, value);

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员</param>
        /// <returns>被移除的成员数量</returns>
        public async Task<long> RemoveAsync(string key, params string[] value) => await CSRedisCore.SRemAsync(key, value);

        /// <summary>
        /// 移除集合中一个或多个成员（泛型对象自动序列化）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的泛型对象</param>
        /// <returns>被移除的成员数量</returns>
        public long Remove<T>(string key, params T[] value) where T : class
        {
            var list = new List<string>();
            foreach (var item in value)
                list.Add(JsonConvert.SerializeObject(item, JsonSetting));
            return CSRedisCore.SRem(key, list.ToArray());
        }

        /// <summary>
        /// 移除集合中一个或多个成员（泛型对象自动序列化）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的泛型对象</param>
        /// <returns>被移除的成员数量</returns>
        public async Task<long> RemoveAsync<T>(string key, params T[] value) where T : class
        {
            var list = new List<string>();
            foreach (var item in value)
                list.Add(JsonConvert.SerializeObject(item, JsonSetting));
            return await CSRedisCore.SRemAsync(key, list.ToArray());
        }

        /// <summary>
        /// 从集合中返回一个随机成员（不删除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员</returns>
        public string RandomMember(string key) => CSRedisCore.SRandMember(key);

        /// <summary>
        /// 从集合中返回一个随机成员（不删除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员</returns>
        public async Task<string> RandomMemberAsync(string key) => await CSRedisCore.SRandMemberAsync(key);

        /// <summary>
        /// 从集合中返回一个随机成员并反序列化为指定类型（不删除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员，反序列化为泛型对象</returns>
        public T RandomMember<T>(string key) where T : class
        {
            var str = CSRedisCore.SRandMember(key);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 从集合中返回一个随机成员并反序列化为指定类型（不删除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员，反序列化为泛型对象</returns>
        public async Task<T> RandomMemberAsync<T>(string key) where T : class
        {
            var str = await CSRedisCore.SRandMemberAsync(key);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 从集合中返回多个随机成员（不删除，可能重复）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>随机成员数组</returns>
        public string[] RandomMembers(string key, int count) => CSRedisCore.SRandMembers(key, count);

        /// <summary>
        /// 从集合中返回多个随机成员（不删除，可能重复）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>随机成员数组</returns>
        public async Task<string[]> RandomMembersAsync(string key, int count) => await CSRedisCore.SRandMembersAsync(key, count);

        /// <summary>
        /// 将指定成员从源集合移动到目标集合
        /// </summary>
        /// <param name="sourceKey">源集合键</param>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="value">要移动的成员</param>
        /// <returns>是否移动成功</returns>
        public bool Move(string sourceKey, string targetKey, string value) => CSRedisCore.SMove(sourceKey, targetKey, value);

        /// <summary>
        /// 将指定成员从源集合移动到目标集合
        /// </summary>
        /// <param name="sourceKey">源集合键</param>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="value">要移动的成员</param>
        /// <returns>是否移动成功</returns>
        public async Task<bool> MoveAsync(string sourceKey, string targetKey, string value) => await CSRedisCore.SMoveAsync(sourceKey, targetKey, value);

        /// <summary>
        /// 返回给定集合之间的差集
        /// </summary>
        /// <param name="keys">集合键列表</param>
        /// <returns>差集结果</returns>
        public string[] Difference(params string[] keys) => CSRedisCore.SDiff(keys);

        /// <summary>
        /// 返回给定集合之间的差集
        /// </summary>
        /// <param name="keys">集合键列表</param>
        /// <returns>差集结果</returns>
        public async Task<string[]> DifferenceAsync(params string[] keys) => await CSRedisCore.SDiffAsync(keys);

        /// <summary>
        /// 将给定集合的差集存储到目标集合中
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">集合键列表</param>
        /// <returns>差集中的成员数量</returns>
        public long DifferenceStore(string targetKey, params string[] keys) => CSRedisCore.SDiffStore(targetKey, keys);

        /// <summary>
        /// 将给定集合的差集存储到目标集合中
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">集合键列表</param>
        /// <returns>差集中的成员数量</returns>
        public async Task<long> DifferenceStoreAsync(string targetKey, params string[] keys) => await CSRedisCore.SDiffStoreAsync(targetKey, keys);

        /// <summary>
        /// 返回给定集合之间的交集
        /// </summary>
        /// <param name="keys">集合键列表</param>
        /// <returns>交集结果</returns>
        public string[] Intersection(params string[] keys) => CSRedisCore.SInter(keys);

        /// <summary>
        /// 返回给定集合之间的交集
        /// </summary>
        /// <param name="keys">集合键列表</param>
        /// <returns>交集结果</returns>
        public async Task<string[]> IntersectionAsync(params string[] keys) => await CSRedisCore.SInterAsync(keys);

        /// <summary>
        /// 将给定集合的交集存储到目标集合中
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">集合键列表</param>
        /// <returns>交集中的成员数量</returns>
        public long IntersectionStore(string targetKey, params string[] keys) => CSRedisCore.SInterStore(targetKey, keys);

        /// <summary>
        /// 将给定集合的交集存储到目标集合中
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">集合键列表</param>
        /// <returns>交集中的成员数量</returns>
        public async Task<long> IntersectionStoreAsync(string targetKey, params string[] keys) => await CSRedisCore.SInterStoreAsync(targetKey, keys);

        /// <summary>
        /// 返回给定集合之间的并集
        /// </summary>
        /// <param name="keys">集合键列表</param>
        /// <returns>并集结果</returns>
        public string[] Union(params string[] keys) => CSRedisCore.SUnion(keys);

        /// <summary>
        /// 返回给定集合之间的并集
        /// </summary>
        /// <param name="keys">集合键列表</param>
        /// <returns>并集结果</returns>
        public async Task<string[]> UnionAsync(params string[] keys) => await CSRedisCore.SUnionAsync(keys);

        /// <summary>
        /// 将给定集合的并集存储到目标集合中
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">集合键列表</param>
        /// <returns>并集中的成员数量</returns>
        public long UnionStore(string targetKey, params string[] keys) => CSRedisCore.SUnionStore(targetKey, keys);

        /// <summary>
        /// 将给定集合的并集存储到目标集合中
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">集合键列表</param>
        /// <returns>并集中的成员数量</returns>
        public async Task<long> UnionStoreAsync(string targetKey, params string[] keys) => await CSRedisCore.SUnionStoreAsync(targetKey, keys);

        /// <summary>
        /// 迭代集合中的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次迭代返回的数量</param>
        /// <returns>扫描结果，包含游标和成员数组</returns>
        public Models.RedisScanResult<string[]> Scan(string key, long cursor, string pattern = null, long? count = null)
        {
            var result = CSRedisCore.SScan(key, cursor, pattern, count ?? 100);
            return new Models.RedisScanResult<string[]> { Cursor = result.Cursor, Items = result.Items };
        }

        /// <summary>
        /// 迭代集合中的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次迭代返回的数量</param>
        /// <returns>扫描结果，包含游标和成员数组</returns>
        public async Task<Models.RedisScanResult<string[]>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            var result = await CSRedisCore.SScanAsync(key, cursor, pattern, count ?? 100);
            return new Models.RedisScanResult<string[]> { Cursor = result.Cursor, Items = result.Items };
        }
    }
}
