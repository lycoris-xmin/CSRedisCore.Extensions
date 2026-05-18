using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// Redis SortedSet 缓存操作实现，提供有序集合的成员增删查、分数操作、排名查询、范围扫描等功能
    /// </summary>
    public sealed class RedisSortCache : IRedisSortCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;
        private readonly string PrefixCacheKey;

        /// <summary>
        /// 初始化 RedisSortCache 实例
        /// </summary>
        /// <param name="CSRedisCore">CSRedis 客户端实例</param>
        /// <param name="JsonSetting">JSON 序列化配置</param>
        /// <param name="prefixCacheKey">缓存键前缀</param>
        public RedisSortCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting, string prefixCacheKey)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
            PrefixCacheKey = prefixCacheKey;
        }

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Count(string key) => CSRedisCore.ZCard(key);

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> CountAsync(string key) => await CSRedisCore.ZCardAsync(key);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public long Add(string key, string member, decimal source) => CSRedisCore.ZAdd(key, (source, (object)member));

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(string key, string member, decimal source) => await CSRedisCore.ZAddAsync(key, (source, (object)member));

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public long Remove(string key, params string[] member) => CSRedisCore.ZRem(key, member);

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<long> RemoveAsync(string key, params string[] member) => await CSRedisCore.ZRemAsync(key, member);

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public long RemoveRange(string key, long start, long stop) => CSRedisCore.ZRemRangeByRank(key, start, stop);

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<long> RemoveRangeAsync(string key, long start, long stop) => await CSRedisCore.ZRemRangeByRankAsync(key, start, stop);

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal Addition(string key, string member, decimal value = 1) => CSRedisCore.ZIncrBy(key, member, value);

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<decimal> AdditionAsync(string key, string member, decimal value = 1) => await CSRedisCore.ZIncrByAsync(key, member, value);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string[] GetRangeBySource(string key, decimal min, decimal max, long? count = null, long offset = 0) => CSRedisCore.ZRangeByScore(key, min, max, count, offset);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<string[]> GetRangeBySourceAsync(string key, decimal min, decimal max, long? count = null, long offset = 0) => await CSRedisCore.ZRangeByScoreAsync(key, min, max, count, offset);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetRangeBySourceWithScores(string key, decimal min, decimal max, long? count = null, long offset = 0)
        {
            var cache = CSRedisCore.ZRangeByScoreWithScores(key, min, max, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<string, decimal>();

            var dic = new Dictionary<string, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<T, decimal> GetRangeBySourceWithScores<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class
        {
            var cache = CSRedisCore.ZRangeByScoreWithScores<T>(key, min, max, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<T, decimal>();

            var dic = new Dictionary<T, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, decimal>> GetRangeBySourceWithScoresAsync(string key, decimal min, decimal max, long? count = null, long offset = 0)
        {
            var cache = await CSRedisCore.ZRangeByScoreWithScoresAsync(key, min, max, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<string, decimal>();

            var dic = new Dictionary<string, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<Dictionary<T, decimal>> GetRangeBySourceWithScoresAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class
        {
            var cache = await CSRedisCore.ZRangeByScoreWithScoresAsync<T>(key, min, max, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<T, decimal>();

            var dic = new Dictionary<T, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetRevRangeByScoreWithScores(string key, decimal max, decimal min, long? count = null, long offset = 0)
        {
            var cache = CSRedisCore.ZRevRangeByScoreWithScores(key, max, min, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<string, decimal>();

            var dic = new Dictionary<string, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<T, decimal> GetRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class
        {
            var cache = CSRedisCore.ZRevRangeByScoreWithScores<T>(key, max, min, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<T, decimal>();

            var dic = new Dictionary<T, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, decimal>> GetRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? count = null, long offset = 0)
        {
            var cache = await CSRedisCore.ZRevRangeByScoreWithScoresAsync(key, max, min, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<string, decimal>();

            var dic = new Dictionary<string, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<Dictionary<T, decimal>> GetRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class
        {
            var cache = await CSRedisCore.ZRevRangeByScoreWithScoresAsync<T>(key, max, min, count, offset);
            if (cache == null || cache.Length == 0)
                return new Dictionary<T, decimal>();

            var dic = new Dictionary<T, decimal>();
            foreach (var (member, score) in cache)
                dic.Add(member, score);

            return dic.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string[] Max(string key, long count = 1)
        {
            var cache = CSRedisCore.ZPopMax(key, count);
            if (cache == null || !cache.Any())
                return Array.Empty<string>();

            return cache.Select(x => x.member).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public T[] Max<T>(string key, long count = 1) where T : class
        {
            var cache = CSRedisCore.ZPopMax(key, count);
            return cache?.Select(x => string.IsNullOrEmpty(x.member) ? default : JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]> MaxAsync(string key, long count = 1)
        {
            var cache = await CSRedisCore.ZPopMaxAsync(key, count);
            return cache?.Select(x => x.member).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<T[]> MaxAsync<T>(string key, long count = 1) where T : class
        {
            var cache = await CSRedisCore.ZPopMaxAsync(key, count);
            return cache?.Select(x => string.IsNullOrEmpty(x.member) ? default : JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string[] Min(string key, long count = 1)
        {
            var cache = CSRedisCore.ZPopMin(key, count);
            return cache.Select(x => x.member).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public T[] Min<T>(string key, long count = 1) where T : class
        {
            var cache = CSRedisCore.ZPopMin(key, count);
            return cache?.Select(x => string.IsNullOrEmpty(x.member) ? default : JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。   
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]> MinAsync(string key, long count = 1)
        {
            var cache = await CSRedisCore.ZPopMinAsync(key, count);
            return cache?.Select(x => x.member).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<T[]> MinAsync<T>(string key, long count = 1) where T : class
        {
            var cache = await CSRedisCore.ZPopMinAsync(key, count);
            return cache?.Select(x => string.IsNullOrEmpty(x.member) ? default : JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public long? Rank(string key, string member) => CSRedisCore.ZRevRank(key, member);

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public Task<long?> RankAsync(string key, string member) => CSRedisCore.ZRevRankAsync(key, member);

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public decimal? GetScore(string key, string member) => CSRedisCore.ZScore(key, member);

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public Task<decimal?> GetScoreAsync(string key, string member) => CSRedisCore.ZScoreAsync(key, member);

        /// <summary>
        /// 获取有序集合中的所有成员及其分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员与分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetAllAsync(string key)
        {
            var items = await CSRedisCore.ZRangeWithScoresAsync(key, 0, -1);
            return items.ToDictionary(x => x.member, x => x.score);
        }

        /// <summary>
        /// 获取有序集合中的所有成员及其分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员与分数的字典</returns>
        public Dictionary<string, decimal> GetAll(string key)
        {
            var items = CSRedisCore.ZRangeWithScores(key, 0, -1);
            return items.ToDictionary(x => x.member, x => x.score);
        }

        /// <summary>
        /// 计算有序集合中指定分数区间的成员数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>指定分数区间的成员数量</returns>
        public long CountByScore(string key, decimal min, decimal max) => CSRedisCore.ZCount(key, min, max);

        /// <summary>
        /// 计算有序集合中指定分数区间的成员数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>指定分数区间的成员数量</returns>
        public async Task<long> CountByScoreAsync(string key, decimal min, decimal max) => await CSRedisCore.ZCountAsync(key, min, max);

        /// <summary>
        /// 向有序集合批量添加多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="members">分数与成员元组数组</param>
        /// <returns>新增成员的数量</returns>
        public long SetMultiple(string key, params (decimal, string)[] members) => CSRedisCore.ZAdd(key, members.Select(x => (x.Item1, (object)x.Item2)).ToArray());

        /// <summary>
        /// 向有序集合批量添加多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="members">分数与成员元组数组</param>
        /// <returns>新增成员的数量</returns>
        public async Task<long> SetMultipleAsync(string key, params (decimal, string)[] members) => await CSRedisCore.ZAddAsync(key, members.Select(x => (x.Item1, (object)x.Item2)).ToArray());

        /// <summary>
        /// 返回有序集合中指定排名区间的成员（分数从低到高）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>指定排名区间的成员数组</returns>
        public string[] GetRangeByRank(string key, long start, long stop) => CSRedisCore.ZRange(key, start, stop);

        /// <summary>
        /// 返回有序集合中指定排名区间的成员（分数从低到高）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>指定排名区间的成员数组</returns>
        public async Task<string[]> GetRangeByRankAsync(string key, long start, long stop) => await CSRedisCore.ZRangeAsync(key, start, stop);

        /// <summary>
        /// 返回有序集合中指定排名区间的成员及其分数（分数从低到高）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        public Dictionary<string, decimal> GetRangeByRankWithScores(string key, long start, long stop)
        {
            var cache = CSRedisCore.ZRangeWithScores(key, start, stop);
            return cache.ToDictionary(x => x.member, x => x.score);
        }

        /// <summary>
        /// 返回有序集合中指定排名区间的成员及其分数（分数从低到高）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetRangeByRankWithScoresAsync(string key, long start, long stop)
        {
            var cache = await CSRedisCore.ZRangeWithScoresAsync(key, start, stop);
            return cache.ToDictionary(x => x.member, x => x.score);
        }

        /// <summary>
        /// 返回有序集合中指定排名区间的成员（分数从高到低）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>指定排名区间的成员数组</returns>
        public string[] GetRevRangeByRank(string key, long start, long stop) => CSRedisCore.ZRevRange(key, start, stop);

        /// <summary>
        /// 返回有序集合中指定排名区间的成员（分数从高到低）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>指定排名区间的成员数组</returns>
        public async Task<string[]> GetRevRangeByRankAsync(string key, long start, long stop) => await CSRedisCore.ZRevRangeAsync(key, start, stop);

        /// <summary>
        /// 返回有序集合中指定排名区间的成员及其分数（分数从高到低）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        public Dictionary<string, decimal> GetRevRangeByRankWithScores(string key, long start, long stop)
        {
            var cache = CSRedisCore.ZRevRangeWithScores(key, start, stop);
            return cache.ToDictionary(x => x.member, x => x.score);
        }

        /// <summary>
        /// 返回有序集合中指定排名区间的成员及其分数（分数从高到低）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetRevRangeByRankWithScoresAsync(string key, long start, long stop)
        {
            var cache = await CSRedisCore.ZRevRangeWithScoresAsync(key, start, stop);
            return cache.ToDictionary(x => x.member, x => x.score);
        }

        /// <summary>
        /// 移除有序集合中指定分数区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>被移除的成员数量</returns>
        public long RemoveByScore(string key, decimal min, decimal max) => CSRedisCore.ZRemRangeByScore(key, min, max);

        /// <summary>
        /// 移除有序集合中指定分数区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>被移除的成员数量</returns>
        public async Task<long> RemoveByScoreAsync(string key, decimal min, decimal max) => await CSRedisCore.ZRemRangeByScoreAsync(key, min, max);

        /// <summary>
        /// 返回有序集合中指定成员的排名（分数从低到高）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>成员的排名</returns>
        public long? RankAscending(string key, string member) => CSRedisCore.ZRank(key, member);

        /// <summary>
        /// 返回有序集合中指定成员的排名（分数从低到高）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>成员的排名</returns>
        public async Task<long?> RankAscendingAsync(string key, string member) => await CSRedisCore.ZRankAsync(key, member);

        /// <summary>
        /// 迭代有序集合中的成员和分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次迭代返回的数量</param>
        /// <returns>扫描结果，包含游标以及成员与分数的字典</returns>
        public Models.RedisScanResult<Dictionary<string, decimal>> Scan(string key, long cursor, string pattern = null, long? count = null)
        {
            var result = CSRedisCore.ZScan(key, cursor, pattern, count ?? 100);
            var dic = new Dictionary<string, decimal>();
            foreach (var (member, score) in result.Items)
                dic[member] = score;
            return new Models.RedisScanResult<Dictionary<string, decimal>> { Cursor = result.Cursor, Items = dic };
        }

        /// <summary>
        /// 迭代有序集合中的成员和分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次迭代返回的数量</param>
        /// <returns>扫描结果，包含游标以及成员与分数的字典</returns>
        public async Task<Models.RedisScanResult<Dictionary<string, decimal>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            var result = await CSRedisCore.ZScanAsync(key, cursor, pattern, count ?? 100);
            var dic = new Dictionary<string, decimal>();
            foreach (var (member, score) in result.Items)
                dic[member] = score;
            return new Models.RedisScanResult<Dictionary<string, decimal>> { Cursor = result.Cursor, Items = dic };
        }
    }
}
