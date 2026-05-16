using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 管道 String 操作包装器，实现 <see cref="IRedisStringCache"/>。
    /// 所有异步方法排队到 <see cref="RedisCachePipe"/>，同步方法抛出 <see cref="NotSupportedException"/>。
    /// </summary>
    public class RedisStringCachePipe : IRedisStringCache
    {
        private readonly RedisCachePipe _cachePipe;

        internal RedisStringCachePipe(RedisCachePipe cachePipe)
        {
            _cachePipe = cachePipe;
        }

        #region synchronous — Not Supported

        public string Get(string key) => throw NewNotSupportedException();
        public T Get<T>(string key) where T : class => throw NewNotSupportedException();
        public List<string> MultipleGet(string key) => throw NewNotSupportedException();
        public List<T> MultipleGet<T>(string key) where T : class => throw NewNotSupportedException();
        public List<string> MultipleGet(params string[] key) => throw NewNotSupportedException();
        public List<T> MultipleGet<T>(params string[] key) where T : class => throw NewNotSupportedException();
        public string GetSet(string key, string value) => throw NewNotSupportedException();
        public string GetSet<T>(string key, T value) where T : class => throw NewNotSupportedException();
        public bool Set(string key, string value, TimeSpan? expire = null) => throw NewNotSupportedException();
        public bool Set<T>(string key, T value, TimeSpan? expire = null) where T : class => throw NewNotSupportedException();
        public bool MultipleSet(params object[] keyValues) => throw NewNotSupportedException();
        public long Addition(string key, long value = 1, TimeSpan? timeSpan = null) => throw NewNotSupportedException();
        public long Subtraction(string key, long value = 1, TimeSpan? timeSpan = null) => throw NewNotSupportedException();
        public bool SetIfExists(string key, string value, TimeSpan? expire = null) => throw NewNotSupportedException();
        public bool SetIfExists<T>(string key, T value, TimeSpan? expire = null) where T : class => throw NewNotSupportedException();
        public long StringLength(string key) => throw NewNotSupportedException();
        public long Append(string key, string value) => throw NewNotSupportedException();
        public string GetRange(string key, long start, long end) => throw NewNotSupportedException();
        public long SetRange(string key, uint offset, string value) => throw NewNotSupportedException();
        public bool SetIfNotExists(string key, string value, TimeSpan? expire = null) => throw NewNotSupportedException();
        public bool SetIfNotExists<T>(string key, T value, TimeSpan? expire = null) where T : class => throw NewNotSupportedException();

        #endregion

        #region async

        public Task<string> GetAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.Get(key);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.Get(key);
            var tcs = _cachePipe.RegisterPending();
            var str = await tcs.Task;
            return string.IsNullOrEmpty(str?.ToString()) ? default : JsonConvert.DeserializeObject<T>(str.ToString(), _cachePipe.JsonSetting);
        }

        public Task<List<string>> MultipleGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.MGet(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public async Task<List<T>> MultipleGetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.MGet(key);
            var tcs = _cachePipe.RegisterPending();
            var arr = await tcs.Task;
            if (arr is string[] items)
            {
                return items.Select(item => string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item, _cachePipe.JsonSetting)).ToList();
            }
            return new List<T>();
        }

        public Task<List<string>> MultipleGetAsync(params string[] key)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (key.Length == 0) throw new ArgumentNullException(nameof(key), "is all empty");
            _cachePipe.Pipe.MGet(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public async Task<List<T>> MultipleGetAsync<T>(params string[] key) where T : class
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (key.Length == 0) throw new ArgumentNullException(nameof(key), "is all empty");
            _cachePipe.Pipe.MGet(key);
            var tcs = _cachePipe.RegisterPending();
            var arr = await tcs.Task;
            if (arr is string[] items)
            {
                return items.Select(item => string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item, _cachePipe.JsonSetting)).ToList();
            }
            return new List<T>();
        }

        public Task<string> GetSetAsync(string key, string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.GetSet(key, value);
            return _cachePipe.EnqueueResult<string>();
        }

        public Task<string> GetSetAsync<T>(string key, T value) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.GetSet(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<string>();
        }

        public Task<bool> SetAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, value, seconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting), seconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetIfNotExistsAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, value, seconds, RedisExistence.Nx);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetIfNotExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting), seconds, RedisExistence.Nx);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> MultipleSetAsync(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Length == 0) throw new ArgumentNullException(nameof(keyValues));
            if (keyValues.Length % 2 != 0) throw new ArgumentException("keyValues must contain an even number of items.", nameof(keyValues));

            for (var i = 0; i < keyValues.Length; i += 2)
            {
                _cachePipe.Pipe.Set(keyValues[i]?.ToString(), keyValues[i + 1]);
            }
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<long> AdditionAsync(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            _cachePipe.Pipe.IncrBy(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> SubtractionAsync(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            _cachePipe.Pipe.IncrBy(key, -value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> SetIfExistsAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, value, seconds, RedisExistence.Xx);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> SetIfExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting), seconds, RedisExistence.Xx);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<long> StringLengthAsync(string key)
        {
            _cachePipe.Pipe.StrLen(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> AppendAsync(string key, string value)
        {
            _cachePipe.Pipe.Append(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<string> GetRangeAsync(string key, long start, long end)
        {
            _cachePipe.Pipe.GetRange(key, start, end);
            return _cachePipe.EnqueueResult<string>();
        }

        public Task<long> SetRangeAsync(string key, uint offset, string value)
        {
            _cachePipe.Pipe.SetRange(key, offset, value);
            return _cachePipe.EnqueueResult<long>();
        }

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
