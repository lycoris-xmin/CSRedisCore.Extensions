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
    public sealed class RedisSortCache : IRedisSortCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisSortCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
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
        public long Add(string key, string member, decimal source) => CSRedisCore.ZAdd(key, (source, member));

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(string key, string member, decimal source) => await CSRedisCore.ZAddAsync(key, (source, member));

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
            return cache?.Select(x => JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
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
        public async Task<T[]> MaxAsync<T>(string key, long count = 1)
        {
            var cache = await CSRedisCore.ZPopMaxAsync(key, count);
            return cache?.Select(x => JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string[] Min(string key, long count)
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
        public T[] Min<T>(string key, long count) where T : class
        {
            var cache = CSRedisCore.ZPopMin(key, count);
            return cache?.Select(x => JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]> MinAsync(string key, long count)
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
        public async Task<T[]> MinAsync<T>(string key, long count) where T : class
        {
            var cache = await CSRedisCore.ZPopMinAsync(key, count);
            return cache?.Select(x => JsonConvert.DeserializeObject<T>(x.member, JsonSetting)).ToArray();
        }
    }
}
