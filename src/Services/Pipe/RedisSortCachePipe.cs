using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 管道 Sorted Set 操作包装器，实现 <see cref="IRedisSortCache"/>。
    /// </summary>
    public class RedisSortCachePipe : IRedisSortCache
    {
        private readonly RedisCachePipe _cachePipe;

        internal RedisSortCachePipe(RedisCachePipe cachePipe)
        {
            _cachePipe = cachePipe;
        }

        #region synchronous — Not Supported

        long IRedisSortCache.Count(string key) => throw NewNotSupportedException();

        long IRedisSortCache.Add(string key, string member, decimal source) => throw NewNotSupportedException();

        long IRedisSortCache.Remove(string key, params string[] member) => throw NewNotSupportedException();

        long IRedisSortCache.RemoveRange(string key, long start, long stop) => throw NewNotSupportedException();

        decimal IRedisSortCache.Addition(string key, string member, decimal value) => throw NewNotSupportedException();

        string[] IRedisSortCache.GetRangeBySource(string key, decimal min, decimal max, long? count, long offset) => throw NewNotSupportedException();

        Dictionary<string, decimal> IRedisSortCache.GetRangeBySourceWithScores(string key, decimal min, decimal max, long? count, long offset) => throw NewNotSupportedException();

        Dictionary<T, decimal> IRedisSortCache.GetRangeBySourceWithScores<T>(string key, decimal min, decimal max, long? count, long offset) where T : class => throw NewNotSupportedException();

        Dictionary<string, decimal> IRedisSortCache.GetRevRangeByScoreWithScores(string key, decimal max, decimal min, long? count, long offset) => throw NewNotSupportedException();

        Dictionary<T, decimal> IRedisSortCache.GetRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? count, long offset) where T : class => throw NewNotSupportedException();

        string[] IRedisSortCache.Max(string key, long count) => throw NewNotSupportedException();

        T[] IRedisSortCache.Max<T>(string key, long count) where T : class => throw NewNotSupportedException();

        string[] IRedisSortCache.Min(string key, long count) => throw NewNotSupportedException();

        T[] IRedisSortCache.Min<T>(string key, long count) where T : class => throw NewNotSupportedException();

        long? IRedisSortCache.Rank(string key, string member) => throw NewNotSupportedException();

        decimal? IRedisSortCache.GetScore(string key, string member) => throw NewNotSupportedException();

        Dictionary<string, decimal> IRedisSortCache.GetAll(string key) => throw NewNotSupportedException();

        long IRedisSortCache.CountByScore(string key, decimal min, decimal max) => throw NewNotSupportedException();

        long IRedisSortCache.SetMultiple(string key, params (decimal, string)[] members) => throw NewNotSupportedException();

        string[] IRedisSortCache.GetRangeByRank(string key, long start, long stop) => throw NewNotSupportedException();

        Dictionary<string, decimal> IRedisSortCache.GetRangeByRankWithScores(string key, long start, long stop) => throw NewNotSupportedException();

        string[] IRedisSortCache.GetRevRangeByRank(string key, long start, long stop) => throw NewNotSupportedException();

        Dictionary<string, decimal> IRedisSortCache.GetRevRangeByRankWithScores(string key, long start, long stop) => throw NewNotSupportedException();

        long IRedisSortCache.RemoveByScore(string key, decimal min, decimal max) => throw NewNotSupportedException();

        long? IRedisSortCache.RankAscending(string key, string member) => throw NewNotSupportedException();

        Models.RedisScanResult<Dictionary<string, decimal>> IRedisSortCache.Scan(string key, long cursor, string pattern, long? count) => throw NewNotSupportedException();

        #endregion

        #region async

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员数量</returns>
        public Task<long> CountAsync(string key)
        {
            _cachePipe.Pipe.ZCard(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="source">分数</param>
        /// <returns>被添加的新成员数量</returns>
        public Task<long> AddAsync(string key, string member, decimal source)
        {
            _cachePipe.Pipe.ZAdd(key, (source, (object)member));
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">要移除的成员</param>
        /// <returns>被移除的成员数量</returns>
        public Task<long> RemoveAsync(string key, params string[] member)
        {
            _cachePipe.Pipe.ZRem(key, member);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>被移除的成员数量</returns>
        public Task<long> RemoveRangeAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRemRangeByRank(key, start, stop);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">增量值</param>
        /// <returns>增量后的分数</returns>
        public Task<decimal> AdditionAsync(string key, string member, decimal value = 1)
        {
            _cachePipe.Pipe.ZIncrBy(key, member, value);
            return _cachePipe.EnqueueResult<decimal>();
        }

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>指定区间内的成员</returns>
        public Task<string[]> GetRangeBySourceAsync(string key, decimal min, decimal max, long? count = null, long offset = 0)
        {
            _cachePipe.Pipe.ZRangeByScore(key, min, max, count, offset);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员及对应分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetRangeBySourceWithScoresAsync(string key, decimal min, decimal max, long? count = null, long offset = 0)
        {
            _cachePipe.Pipe.ZRangeByScoreWithScores(key, min, max, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员及对应分数的字典</returns>
        public async Task<Dictionary<T, decimal>> GetRangeBySourceWithScoresAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class
        {
            _cachePipe.Pipe.ZRangeByScoreWithScores(key, min, max, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractGenericScoreDic<T>(rawResult);
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员及对应分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? count = null, long offset = 0)
        {
            _cachePipe.Pipe.ZRevRangeByScoreWithScores(key, max, min, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员及对应分数的字典</returns>
        public async Task<Dictionary<T, decimal>> GetRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class
        {
            _cachePipe.Pipe.ZRevRangeByScoreWithScores(key, max, min, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractGenericScoreDic<T>(rawResult);
        }

        /// <summary>
        /// 删除并返回有序集合中得分最高的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>得分最高的成员</returns>
        public async Task<string[]> MaxAsync(string key, long count = 1)
        {
            _cachePipe.Pipe.ZPopMax(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractMembers(rawResult);
        }

        /// <summary>
        /// 删除并返回有序集合中得分最高的成员
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>得分最高的成员</returns>
        public async Task<T[]> MaxAsync<T>(string key, long count = 1) where T : class
        {
            _cachePipe.Pipe.ZPopMax(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractTypedMembers<T>(rawResult);
        }

        /// <summary>
        /// 删除并返回有序集合中得分最低的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>得分最低的成员</returns>
        public async Task<string[]> MinAsync(string key, long count = 1)
        {
            _cachePipe.Pipe.ZPopMin(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractMembers(rawResult);
        }

        /// <summary>
        /// 删除并返回有序集合中得分最低的成员
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>得分最低的成员</returns>
        public async Task<T[]> MinAsync<T>(string key, long count = 1) where T : class
        {
            _cachePipe.Pipe.ZPopMin(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractTypedMembers<T>(rawResult);
        }

        /// <summary>
        /// 返回有序集合中指定成员的排名，按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>排名（从0开始），不存在返回 null</returns>
        public Task<long?> RankAsync(string key, string member)
        {
            _cachePipe.Pipe.ZRevRank(key, member);
            return _cachePipe.EnqueueResult<long?>();
        }

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>分数值，不存在返回 null</returns>
        public Task<decimal?> GetScoreAsync(string key, string member)
        {
            _cachePipe.Pipe.ZScore(key, member);
            return _cachePipe.EnqueueResult<decimal?>();
        }

        /// <summary>
        /// 获取有序集合的所有成员及分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>所有成员及分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetAllAsync(string key)
        {
            _cachePipe.Pipe.ZRangeWithScores(key, 0, -1);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        /// <summary>
        /// 统计分数区间内的成员数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>成员数量</returns>
        public Task<long> CountByScoreAsync(string key, decimal min, decimal max)
        {
            _cachePipe.Pipe.ZCount(key, min, max);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 同时向有序集合添加多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="members">分数和成员的元组列表</param>
        /// <returns>被添加的新成员数量</returns>
        public Task<long> SetMultipleAsync(string key, params (decimal, string)[] members)
        {
            var objMembers = members.Select(m => ((decimal, object))(m.Item1, m.Item2)).ToArray();
            _cachePipe.Pipe.ZAdd(key, objMembers);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 按排名区间返回成员（升序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>指定排名区间的成员</returns>
        public Task<string[]> GetRangeByRankAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRange(key, start, stop);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 按排名区间返回成员及分数（升序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员及分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetRangeByRankWithScoresAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRangeWithScores(key, start, stop);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        /// <summary>
        /// 按排名区间返回成员（降序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>指定排名区间的成员</returns>
        public Task<string[]> GetRevRangeByRankAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRevRange(key, start, stop);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 按排名区间返回成员及分数（降序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员及分数的字典</returns>
        public async Task<Dictionary<string, decimal>> GetRevRangeByRankWithScoresAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRevRangeWithScores(key, start, stop);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        /// <summary>
        /// 移除指定分数区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>被移除的成员数量</returns>
        public Task<long> RemoveByScoreAsync(string key, decimal min, decimal max)
        {
            _cachePipe.Pipe.ZRemRangeByScore(key, min, max);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 返回指定成员升序排名（分数从小到大）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>排名（从0开始），不存在返回 null</returns>
        public Task<long?> RankAscendingAsync(string key, string member)
        {
            _cachePipe.Pipe.ZRank(key, member);
            return _cachePipe.EnqueueResult<long?>();
        }

        /// <summary>
        /// 同步方法，管道模式不支持，请使用异步重载
        /// </summary>
        public Task<Models.RedisScanResult<Dictionary<string, decimal>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for sorted set scans. Use PipeExecute/raw pipe access for scan operations.");

        #endregion

        #region Internal helpers

        private Dictionary<string, decimal> ExtractScoreDic(object rawResult)
        {
            var dic = new Dictionary<string, decimal>();
            if (rawResult is (string member, decimal score)[] tupleArr)
            {
                foreach (var item in tupleArr)
                {
                    if (item.member != null) dic[item.member] = item.score;
                }
            }
            return dic;
        }

        private Dictionary<T, decimal> ExtractGenericScoreDic<T>(object rawResult) where T : class
        {
            var dic = new Dictionary<T, decimal>();
            if (rawResult is (string member, decimal score)[] tupleArr)
            {
                foreach (var item in tupleArr)
                {
                    if (!string.IsNullOrEmpty(item.member))
                        dic[JsonConvert.DeserializeObject<T>(item.member, _cachePipe.JsonSetting)] = item.score;
                }
            }
            return dic;
        }

        private string[] ExtractMembers(object rawResult)
        {
            if (rawResult is (string member, decimal score)[] tupleArr)
                return tupleArr.Where(x => x.member != null).Select(x => x.member).ToArray();

            return Array.Empty<string>();
        }

        private T[] ExtractTypedMembers<T>(object rawResult)
        {
            if (rawResult is (string member, decimal score)[] tupleArr)
            {
                return tupleArr
                    .Select(x => string.IsNullOrEmpty(x.member) ? default : JsonConvert.DeserializeObject<T>(x.member, _cachePipe.JsonSetting))
                    .ToArray();
            }

            return Array.Empty<T>();
        }

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
