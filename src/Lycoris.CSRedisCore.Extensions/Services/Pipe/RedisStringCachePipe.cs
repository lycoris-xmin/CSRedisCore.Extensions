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

        string IRedisStringCache.Get(string key) => throw NewNotSupportedException();

        T IRedisStringCache.Get<T>(string key) where T : class => throw NewNotSupportedException();

        List<string> IRedisStringCache.MultipleGet(string key) => throw NewNotSupportedException();

        List<T> IRedisStringCache.MultipleGet<T>(string key) where T : class => throw NewNotSupportedException();

        List<string> IRedisStringCache.MultipleGet(params string[] key) => throw NewNotSupportedException();

        List<T> IRedisStringCache.MultipleGet<T>(params string[] key) where T : class => throw NewNotSupportedException();

        string IRedisStringCache.GetSet(string key, string value) => throw NewNotSupportedException();

        string IRedisStringCache.GetSet<T>(string key, T value) where T : class => throw NewNotSupportedException();

        bool IRedisStringCache.Set(string key, string value, TimeSpan? expire) => throw NewNotSupportedException();

        bool IRedisStringCache.Set<T>(string key, T value, TimeSpan? expire) where T : class => throw NewNotSupportedException();

        bool IRedisStringCache.MultipleSet(params object[] keyValues) => throw NewNotSupportedException();

        long IRedisStringCache.Addition(string key, long value, TimeSpan? timeSpan) => throw NewNotSupportedException();

        long IRedisStringCache.Subtraction(string key, long value, TimeSpan? timeSpan) => throw NewNotSupportedException();

        bool IRedisStringCache.SetIfExists(string key, string value, TimeSpan? expire) => throw NewNotSupportedException();

        bool IRedisStringCache.SetIfExists<T>(string key, T value, TimeSpan? expire) where T : class => throw NewNotSupportedException();

        long IRedisStringCache.StringLength(string key) => throw NewNotSupportedException();

        long IRedisStringCache.Append(string key, string value) => throw NewNotSupportedException();

        string IRedisStringCache.GetRange(string key, long start, long end) => throw NewNotSupportedException();

        long IRedisStringCache.SetRange(string key, uint offset, string value) => throw NewNotSupportedException();

        bool IRedisStringCache.SetIfNotExists(string key, string value, TimeSpan? expire) => throw NewNotSupportedException();

        bool IRedisStringCache.SetIfNotExists<T>(string key, T value, TimeSpan? expire) where T : class => throw NewNotSupportedException();

        byte[] IRedisStringCache.GetBytes(string key) => throw NewNotSupportedException();

        bool IRedisStringCache.SetBytes(string key, byte[] value, TimeSpan? expire) => throw NewNotSupportedException();

        #endregion

        #region async

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public Task<string> GetAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.Get(key);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.Get(key);
            var tcs = _cachePipe.RegisterPending();
            var str = await tcs.Task;
            return string.IsNullOrEmpty(str?.ToString()) ? default : JsonConvert.DeserializeObject<T>(str.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <param name="key">缓存键模式</param>
        /// <returns>匹配的键值列表</returns>
        public Task<List<string>> MultipleGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.MGet(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键模式</param>
        /// <returns>匹配的键值列表</returns>
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

        /// <summary>
        /// 获取多个 key 的值
        /// </summary>
        /// <param name="key">缓存键列表</param>
        /// <returns>对应 key 的值列表</returns>
        public Task<List<string>> MultipleGetAsync(params string[] key)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (key.Length == 0) throw new ArgumentNullException(nameof(key), "is all empty");
            _cachePipe.Pipe.MGet(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 获取多个 key 的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键列表</param>
        /// <returns>对应 key 的值列表</returns>
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

        /// <summary>
        /// 将给定 key 的值设为 value，并返回 key 的旧值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">新值</param>
        /// <returns>旧值</returns>
        public Task<string> GetSetAsync(string key, string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.GetSet(key, value);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 将给定 key 的值设为 value，并返回 key 的旧值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">新值</param>
        /// <returns>旧值</returns>
        public Task<string> GetSetAsync<T>(string key, T value) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.GetSet(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, value, seconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting), seconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 仅当 key 不存在时才设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetIfNotExistsAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, value, seconds, RedisExistence.Nx);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 仅当 key 不存在时才设置值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetIfNotExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting), seconds, RedisExistence.Nx);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">键值对（偶数个参数，奇数为键，偶数为值）</param>
        /// <returns>是否设置成功</returns>
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

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">增量值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>增量后的值</returns>
        public Task<long> AdditionAsync(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            _cachePipe.Pipe.IncrBy(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 将 key 所储存的值减去给定的增量值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">减量值（正整数）</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>减量后的值</returns>
        public Task<long> SubtractionAsync(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            _cachePipe.Pipe.IncrBy(key, -value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 仅当 key 已存在时才设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetIfExistsAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, value, seconds, RedisExistence.Xx);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 仅当 key 已存在时才设置值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetIfExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting), seconds, RedisExistence.Xx);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 获取字符串值的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字符串长度</returns>
        public Task<long> StringLengthAsync(string key)
        {
            _cachePipe.Pipe.StrLen(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 追加内容到已有字符串末尾，返回新长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要追加的内容</param>
        /// <returns>追加后的字符串长度</returns>
        public Task<long> AppendAsync(string key, string value)
        {
            _cachePipe.Pipe.Append(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 获取子字符串
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>子字符串</returns>
        public Task<string> GetRangeAsync(string key, long start, long end)
        {
            _cachePipe.Pipe.GetRange(key, start, end);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 覆盖指定偏移位置的子字符串，返回新长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="value">要写入的值</param>
        /// <returns>修改后的字符串长度</returns>
        public Task<long> SetRangeAsync(string key, uint offset, string value)
        {
            _cachePipe.Pipe.SetRange(key, offset, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 获取指定 key 的原始字节值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字节数组</returns>
        public async Task<byte[]> GetBytesAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            _cachePipe.Pipe.Get(key);
            var tcs = _cachePipe.RegisterPending();
            var b64 = await tcs.Task;
            return string.IsNullOrEmpty(b64?.ToString()) ? null : Convert.FromBase64String(b64.ToString());
        }

        /// <summary>
        /// 设置指定 key 的原始字节值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">字节数组</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetBytesAsync(string key, byte[] value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : -1;
            _cachePipe.Pipe.Set(key, Convert.ToBase64String(value), seconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
