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

        public long Count(string key) => throw NewNotSupportedException();
        public long Add(string key, string member, decimal source) => throw NewNotSupportedException();
        public long Remove(string key, params string[] member) => throw NewNotSupportedException();
        public long RemoveRange(string key, long start, long stop) => throw NewNotSupportedException();
        public decimal Addition(string key, string member, decimal value = 1) => throw NewNotSupportedException();
        public string[] GetRangeBySource(string key, decimal min, decimal max, long? count = null, long offset = 0) => throw NewNotSupportedException();
        public Dictionary<string, decimal> GetRangeBySourceWithScores(string key, decimal min, decimal max, long? count = null, long offset = 0) => throw NewNotSupportedException();
        public Dictionary<T, decimal> GetRangeBySourceWithScores<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class => throw NewNotSupportedException();
        public Dictionary<string, decimal> GetRevRangeByScoreWithScores(string key, decimal max, decimal min, long? count = null, long offset = 0) => throw NewNotSupportedException();
        public Dictionary<T, decimal> GetRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class => throw NewNotSupportedException();
        public string[] Max(string key, long count = 1) => throw NewNotSupportedException();
        public T[] Max<T>(string key, long count = 1) where T : class => throw NewNotSupportedException();
        public string[] Min(string key, long count = 1) => throw NewNotSupportedException();
        public T[] Min<T>(string key, long count = 1) where T : class => throw NewNotSupportedException();
        public long? Rank(string key, string member) => throw NewNotSupportedException();
        public decimal? GetScore(string key, string member) => throw NewNotSupportedException();
        public Dictionary<string, decimal> GetAll(string key) => throw NewNotSupportedException();
        public long CountByScore(string key, decimal min, decimal max) => throw NewNotSupportedException();
        public long SetMultiple(string key, params (decimal, string)[] members) => throw NewNotSupportedException();
        public string[] GetRangeByRank(string key, long start, long stop) => throw NewNotSupportedException();
        public Dictionary<string, decimal> GetRangeByRankWithScores(string key, long start, long stop) => throw NewNotSupportedException();
        public string[] GetRevRangeByRank(string key, long start, long stop) => throw NewNotSupportedException();
        public Dictionary<string, decimal> GetRevRangeByRankWithScores(string key, long start, long stop) => throw NewNotSupportedException();
        public long RemoveByScore(string key, decimal min, decimal max) => throw NewNotSupportedException();
        public long? RankAscending(string key, string member) => throw NewNotSupportedException();
        public Models.RedisScanResult<Dictionary<string, decimal>> Scan(string key, long cursor, string pattern = null, long? count = null) => throw NewNotSupportedException();

        #endregion

        #region async

        public Task<long> CountAsync(string key)
        {
            _cachePipe.Pipe.ZCard(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> AddAsync(string key, string member, decimal source)
        {
            _cachePipe.Pipe.ZAdd(key, (source, (object)member));
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> RemoveAsync(string key, params string[] member)
        {
            _cachePipe.Pipe.ZRem(key, member);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> RemoveRangeAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRemRangeByRank(key, start, stop);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<decimal> AdditionAsync(string key, string member, decimal value = 1)
        {
            _cachePipe.Pipe.ZIncrBy(key, member, value);
            return _cachePipe.EnqueueResult<decimal>();
        }

        public Task<string[]> GetRangeBySourceAsync(string key, decimal min, decimal max, long? count = null, long offset = 0)
        {
            _cachePipe.Pipe.ZRangeByScore(key, min, max, count, offset);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public async Task<Dictionary<string, decimal>> GetRangeBySourceWithScoresAsync(string key, decimal min, decimal max, long? count = null, long offset = 0)
        {
            _cachePipe.Pipe.ZRangeByScoreWithScores(key, min, max, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        public async Task<Dictionary<T, decimal>> GetRangeBySourceWithScoresAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class
        {
            _cachePipe.Pipe.ZRangeByScoreWithScores(key, min, max, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractGenericScoreDic<T>(rawResult);
        }

        public async Task<Dictionary<string, decimal>> GetRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? count = null, long offset = 0)
        {
            _cachePipe.Pipe.ZRevRangeByScoreWithScores(key, max, min, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        public async Task<Dictionary<T, decimal>> GetRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class
        {
            _cachePipe.Pipe.ZRevRangeByScoreWithScores(key, max, min, count, offset);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractGenericScoreDic<T>(rawResult);
        }

        public async Task<string[]> MaxAsync(string key, long count = 1)
        {
            _cachePipe.Pipe.ZPopMax(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractMembers(rawResult);
        }

        public async Task<T[]> MaxAsync<T>(string key, long count = 1)
        {
            _cachePipe.Pipe.ZPopMax(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractTypedMembers<T>(rawResult);
        }

        public async Task<string[]> MinAsync(string key, long count = 1)
        {
            _cachePipe.Pipe.ZPopMin(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractMembers(rawResult);
        }

        public async Task<T[]> MinAsync<T>(string key, long count = 1) where T : class
        {
            _cachePipe.Pipe.ZPopMin(key, count);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractTypedMembers<T>(rawResult);
        }

        public Task<long?> RankAsync(string key, string member)
        {
            _cachePipe.Pipe.ZRevRank(key, member);
            return _cachePipe.EnqueueResult<long?>();
        }

        public Task<decimal?> GetScoreAsync(string key, string member)
        {
            _cachePipe.Pipe.ZScore(key, member);
            return _cachePipe.EnqueueResult<decimal?>();
        }

        public async Task<Dictionary<string, decimal>> GetAllAsync(string key)
        {
            _cachePipe.Pipe.ZRangeWithScores(key, 0, -1);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        public Task<long> CountByScoreAsync(string key, decimal min, decimal max)
        {
            _cachePipe.Pipe.ZCount(key, min, max);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> SetMultipleAsync(string key, params (decimal, string)[] members)
        {
            var objMembers = members.Select(m => ((decimal, object))(m.Item1, m.Item2)).ToArray();
            _cachePipe.Pipe.ZAdd(key, objMembers);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<string[]> GetRangeByRankAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRange(key, start, stop);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public async Task<Dictionary<string, decimal>> GetRangeByRankWithScoresAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRangeWithScores(key, start, stop);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        public Task<string[]> GetRevRangeByRankAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRevRange(key, start, stop);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public async Task<Dictionary<string, decimal>> GetRevRangeByRankWithScoresAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.ZRevRangeWithScores(key, start, stop);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            return ExtractScoreDic(rawResult);
        }

        public Task<long> RemoveByScoreAsync(string key, decimal min, decimal max)
        {
            _cachePipe.Pipe.ZRemRangeByScore(key, min, max);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long?> RankAscendingAsync(string key, string member)
        {
            _cachePipe.Pipe.ZRank(key, member);
            return _cachePipe.EnqueueResult<long?>();
        }

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
