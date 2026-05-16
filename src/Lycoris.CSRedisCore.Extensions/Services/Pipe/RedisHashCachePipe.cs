using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 管道 Hash 操作包装器，实现 <see cref="IRedisHashCache"/>。
    /// </summary>
    public class RedisHashCachePipe : IRedisHashCache
    {
        private readonly RedisCachePipe _cachePipe;

        internal RedisHashCachePipe(RedisCachePipe cachePipe)
        {
            _cachePipe = cachePipe;
        }

        #region synchronous — Not Supported

        public long Remove(string key, params string[] fieIds) => throw NewNotSupportedException();
        public bool Exists(string key, string fieId) => throw NewNotSupportedException();
        public string Get(string key, string fieId) => throw NewNotSupportedException();
        public T Get<T>(string key, string fieId) where T : class => throw NewNotSupportedException();
        public List<string> Get(string key, params string[] fieIds) => throw NewNotSupportedException();
        public List<T> Get<T>(string key, params string[] fieIds) where T : class => throw NewNotSupportedException();
        public Dictionary<string, string> GetAll(string key) => throw NewNotSupportedException();
        public Dictionary<string, T> GetAll<T>(string key) where T : class => throw NewNotSupportedException();
        public List<string> FieIds(string key) => throw NewNotSupportedException();
        public long FieIdsCount(string key) => throw NewNotSupportedException();
        public bool Set(string key, params (string, string)[] value) => throw NewNotSupportedException();
        public bool Set<T>(string key, params (string, T)[] value) where T : class => throw NewNotSupportedException();
        public bool Set(string key, string fieId, string value) => throw NewNotSupportedException();
        public bool Set<T>(string key, string fieId, T value) where T : class => throw NewNotSupportedException();
        public bool SetNx(string key, string fieId, string value) => throw NewNotSupportedException();
        public bool SetNx<T>(string key, string fieId, T value) where T : class => throw NewNotSupportedException();
        public long Addition(string key, string fieId, long value = 1, TimeSpan? timeSpan = null) => throw NewNotSupportedException();
        public decimal IncrByFloat(string key, string fieId, decimal value) => throw NewNotSupportedException();
        public List<string> Values(string key) => throw NewNotSupportedException();
        public long FieldStringLength(string key, string fieId) => throw NewNotSupportedException();
        public Models.RedisScanResult<Dictionary<string, string>> Scan(string key, long cursor, string pattern = null, long? count = null) => throw NewNotSupportedException();

        #endregion

        #region async

        public Task<long> RemoveAsync(string key, params string[] fieIds)
        {
            _cachePipe.Pipe.HDel(key, fieIds);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> ExistsAsync(string key, string fieId)
        {
            _cachePipe.Pipe.HExists(key, fieId);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<string> GetAsync(string key, string fieId)
        {
            _cachePipe.Pipe.HGet(key, fieId);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> GetAsync<T>(string key, string fieId) where T : class
        {
            _cachePipe.Pipe.HGet(key, fieId);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        public Task<List<string>> GetAsync(string key, params string[] fieIds)
        {
            _cachePipe.Pipe.HMGet(key, fieIds);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public async Task<List<T>> GetAsync<T>(string key, params string[] fieIds) where T : class
        {
            _cachePipe.Pipe.HMGet(key, fieIds);
            var tcs = _cachePipe.RegisterPending();
            var arr = await tcs.Task;
            if (arr is string[] items)
            {
                return items.Select(item => string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item, _cachePipe.JsonSetting)).ToList();
            }
            return new List<T>();
        }

        public Task<Dictionary<string, string>> GetAllAsync(string key)
        {
            _cachePipe.Pipe.HGetAll(key);
            return _cachePipe.EnqueueResult<Dictionary<string, string>>();
        }

        public async Task<Dictionary<string, T>> GetAllAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.HGetAll(key);
            var tcs = _cachePipe.RegisterPending();
            var result = await tcs.Task;
            if (result is Dictionary<string, string> dic)
            {
                var typedDic = new Dictionary<string, T>();
                foreach (var kv in dic)
                    typedDic[kv.Key] = string.IsNullOrEmpty(kv.Value) ? default : JsonConvert.DeserializeObject<T>(kv.Value, _cachePipe.JsonSetting);
                return typedDic;
            }
            return new Dictionary<string, T>();
        }

        public Task<List<string>> FieIdsAsync(string key)
        {
            _cachePipe.Pipe.HKeys(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public Task<long> FieIdsCountAsync(string key)
        {
            _cachePipe.Pipe.HLen(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public async Task<bool> SetAsync(string key, params (string, string)[] value)
        {
            var keyValues = new List<object>();
            foreach (var item in value)
            {
                keyValues.Add(item.Item1);
                keyValues.Add(item.Item2);
            }
            _cachePipe.Pipe.HMSet(key, keyValues.ToArray());
            var tcs = _cachePipe.RegisterPending();
            return (bool)(await tcs.Task);
        }

        public async Task<bool> SetAsync<T>(string key, params (string, T)[] value) where T : class
        {
            var keyValues = new List<object>();
            foreach (var item in value)
            {
                keyValues.Add(item.Item1);
                keyValues.Add(item.Item2 != null ? JsonConvert.SerializeObject(item.Item2, _cachePipe.JsonSetting) : "");
            }
            _cachePipe.Pipe.HMSet(key, keyValues.ToArray());
            var tcs = _cachePipe.RegisterPending();
            return (bool)(await tcs.Task);
        }

        public Task<bool> SetAsync(string key, string fieId, string value)
        {
            _cachePipe.Pipe.HSet(key, fieId, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetAsync<T>(string key, string fieId, T value) where T : class
        {
            var json = JsonConvert.SerializeObject(value, _cachePipe.JsonSetting);
            _cachePipe.Pipe.HSet(key, fieId, json);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetNxAsync(string key, string fieId, string value)
        {
            _cachePipe.Pipe.HSetNx(key, fieId, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetNxAsync<T>(string key, string fieId, T value) where T : class
        {
            var json = JsonConvert.SerializeObject(value, _cachePipe.JsonSetting);
            _cachePipe.Pipe.HSetNx(key, fieId, json);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<long> AdditionAsync(string key, string fieId, long value = 1, TimeSpan? timeSpan = null)
        {
            _cachePipe.Pipe.HIncrBy(key, fieId, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<decimal> IncrByFloatAsync(string key, string fieId, decimal value)
        {
            _cachePipe.Pipe.HIncrByFloat(key, fieId, value);
            return _cachePipe.EnqueueResult<decimal>();
        }

        public Task<List<string>> ValuesAsync(string key)
        {
            _cachePipe.Pipe.HVals(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public Task<long> FieldStringLengthAsync(string key, string fieId)
        {
            _cachePipe.Pipe.HStrLen(key, fieId);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<Models.RedisScanResult<Dictionary<string, string>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for hash scans. Use PipeExecute/raw pipe access for scan operations.");

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
