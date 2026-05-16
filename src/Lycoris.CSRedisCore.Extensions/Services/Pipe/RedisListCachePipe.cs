using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 管道 List 操作包装器，实现 <see cref="IRedisListCache"/>。
    /// </summary>
    public class RedisListCachePipe : IRedisListCache
    {
        private readonly RedisCachePipe _cachePipe;

        internal RedisListCachePipe(RedisCachePipe cachePipe)
        {
            _cachePipe = cachePipe;
        }

        #region synchronous — Not Supported

        public string GetAndRemoveFirst(string key) => throw NewNotSupportedException();
        public T GetAndRemoveFirst<T>(string key) where T : class => throw NewNotSupportedException();
        public string GetAndRemoveLast(string key) => throw NewNotSupportedException();
        public T GetAndRemoveLast<T>(string key) where T : class => throw NewNotSupportedException();
        public void SetFirst(string key, params string[] value) => throw NewNotSupportedException();
        public void SetFirst<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();
        public void SetLast(string key, params string[] value) => throw NewNotSupportedException();
        public void SetLast<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();
        public List<string> GetAll(string key) => throw NewNotSupportedException();
        public string GetByIndex(string key, long index) => throw NewNotSupportedException();
        public T GetByIndex<T>(string key, long index) where T : class => throw NewNotSupportedException();
        public List<string> GetRange(string key, long start, long stop) => throw NewNotSupportedException();
        public List<T> GetRange<T>(string key, long start, long stop) where T : class => throw NewNotSupportedException();
        public long InsertBefore(string key, string pivot, string value) => throw NewNotSupportedException();
        public long InsertBefore<T>(string key, string pivot, T value) where T : class => throw NewNotSupportedException();
        public long InsertAfter(string key, string pivot, string value) => throw NewNotSupportedException();
        public long InsertAfter<T>(string key, string pivot, T value) where T : class => throw NewNotSupportedException();
        public bool SetByIndex(string key, long index, string value) => throw NewNotSupportedException();
        public bool SetByIndex<T>(string key, long index, T value) where T : class => throw NewNotSupportedException();
        public long Length(string key) => throw NewNotSupportedException();
        public long Remove(string key, long count, string value) => throw NewNotSupportedException();
        public bool Trim(string key, long start, long stop) => throw NewNotSupportedException();
        public string PopLastPushFirst(string sourceKey, string targetKey) => throw NewNotSupportedException();

        #endregion

        #region async

        public Task<string> GetAndRemoveFirstAsync(string key)
        {
            _cachePipe.Pipe.LPop(key);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> GetAndRemoveFirstAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.LPop(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        public Task<string> GetAndRemoveLastAsync(string key)
        {
            _cachePipe.Pipe.RPop(key);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> GetAndRemoveLastAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.RPop(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        public Task SetFirstAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.LPush(key, value);
            return _cachePipe.EnqueueVoidResult();
        }

        public Task SetFirstAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.LPush(key, list);
            return _cachePipe.EnqueueVoidResult();
        }

        public Task SetLastAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.RPush(key, value);
            return _cachePipe.EnqueueVoidResult();
        }

        public Task SetLastAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.RPush(key, list);
            return _cachePipe.EnqueueVoidResult();
        }

        public Task<List<string>> GetAllAsync(string key)
        {
            _cachePipe.Pipe.LRange(key, 0, -1);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public Task<string> GetByIndexAsync(string key, long index)
        {
            _cachePipe.Pipe.LIndex(key, index);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> GetByIndexAsync<T>(string key, long index) where T : class
        {
            _cachePipe.Pipe.LIndex(key, index);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        public Task<List<string>> GetRangeAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.LRange(key, start, stop);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public async Task<List<T>> GetRangeAsync<T>(string key, long start, long stop) where T : class
        {
            _cachePipe.Pipe.LRange(key, start, stop);
            var tcs = _cachePipe.RegisterPending();
            var arr = await tcs.Task;
            if (arr is string[] items)
            {
                return items.Select(item => string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item, _cachePipe.JsonSetting)).ToList();
            }
            return new List<T>();
        }

        public Task<long> InsertBeforeAsync(string key, string pivot, string value)
        {
            _cachePipe.Pipe.LInsertBefore(key, pivot, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> InsertBeforeAsync<T>(string key, string pivot, T value) where T : class
        {
            _cachePipe.Pipe.LInsertBefore(key, pivot, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> InsertAfterAsync(string key, string pivot, string value)
        {
            _cachePipe.Pipe.LInsertAfter(key, pivot, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> InsertAfterAsync<T>(string key, string pivot, T value) where T : class
        {
            _cachePipe.Pipe.LInsertAfter(key, pivot, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> SetByIndexAsync(string key, long index, string value)
        {
            _cachePipe.Pipe.LSet(key, index, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetByIndexAsync<T>(string key, long index, T value) where T : class
        {
            _cachePipe.Pipe.LSet(key, index, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<long> LengthAsync(string key)
        {
            _cachePipe.Pipe.LLen(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> RemoveAsync(string key, long count, string value)
        {
            _cachePipe.Pipe.LRem(key, count, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> TrimAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.LTrim(key, start, stop);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<string> PopLastPushFirstAsync(string sourceKey, string targetKey)
        {
            _cachePipe.Pipe.RPopLPush(sourceKey, targetKey);
            return _cachePipe.EnqueueResult<string>();
        }

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
