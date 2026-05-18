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

        string IRedisListCache.GetAndRemoveFirst(string key) => throw NewNotSupportedException();

        T IRedisListCache.GetAndRemoveFirst<T>(string key) where T : class => throw NewNotSupportedException();

        string IRedisListCache.GetAndRemoveLast(string key) => throw NewNotSupportedException();

        T IRedisListCache.GetAndRemoveLast<T>(string key) where T : class => throw NewNotSupportedException();

        void IRedisListCache.SetFirst(string key, params string[] value) => throw NewNotSupportedException();

        void IRedisListCache.SetFirst<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();

        void IRedisListCache.SetLast(string key, params string[] value) => throw NewNotSupportedException();

        void IRedisListCache.SetLast<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();

        List<string> IRedisListCache.GetAll(string key) => throw NewNotSupportedException();

        string IRedisListCache.GetByIndex(string key, long index) => throw NewNotSupportedException();

        T IRedisListCache.GetByIndex<T>(string key, long index) where T : class => throw NewNotSupportedException();

        List<string> IRedisListCache.GetRange(string key, long start, long stop) => throw NewNotSupportedException();

        List<T> IRedisListCache.GetRange<T>(string key, long start, long stop) where T : class => throw NewNotSupportedException();

        long IRedisListCache.InsertBefore(string key, string pivot, string value) => throw NewNotSupportedException();

        long IRedisListCache.InsertBefore<T>(string key, string pivot, T value) where T : class => throw NewNotSupportedException();

        long IRedisListCache.InsertAfter(string key, string pivot, string value) => throw NewNotSupportedException();

        long IRedisListCache.InsertAfter<T>(string key, string pivot, T value) where T : class => throw NewNotSupportedException();

        bool IRedisListCache.SetByIndex(string key, long index, string value) => throw NewNotSupportedException();

        bool IRedisListCache.SetByIndex<T>(string key, long index, T value) where T : class => throw NewNotSupportedException();

        long IRedisListCache.Length(string key) => throw NewNotSupportedException();

        long IRedisListCache.Remove(string key, long count, string value) => throw NewNotSupportedException();

        bool IRedisListCache.Trim(string key, long start, long stop) => throw NewNotSupportedException();

        string IRedisListCache.PopLastPushFirst(string sourceKey, string targetKey) => throw NewNotSupportedException();

        #endregion

        #region async

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>第一个元素的值</returns>
        public Task<string> GetAndRemoveFirstAsync(string key)
        {
            _cachePipe.Pipe.LPop(key);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>第一个元素的值</returns>
        public async Task<T> GetAndRemoveFirstAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.LPop(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 移出并获取列表的最后一个元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>最后一个元素的值</returns>
        public Task<string> GetAndRemoveLastAsync(string key)
        {
            _cachePipe.Pipe.RPop(key);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 移出并获取列表的最后一个元素
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>最后一个元素的值</returns>
        public async Task<T> GetAndRemoveLastAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.RPop(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值</param>
        public Task SetFirstAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.LPush(key, value);
            return _cachePipe.EnqueueVoidResult();
        }

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值</param>
        public Task SetFirstAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.LPush(key, list);
            return _cachePipe.EnqueueVoidResult();
        }

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值</param>
        public Task SetLastAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.RPush(key, value);
            return _cachePipe.EnqueueVoidResult();
        }

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值</param>
        public Task SetLastAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.RPush(key, list);
            return _cachePipe.EnqueueVoidResult();
        }

        /// <summary>
        /// 获取列表的所有元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表的所有元素</returns>
        public Task<List<string>> GetAllAsync(string key)
        {
            _cachePipe.Pipe.LRange(key, 0, -1);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>元素的值</returns>
        public Task<string> GetByIndexAsync(string key, long index)
        {
            _cachePipe.Pipe.LIndex(key, index);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>元素的值</returns>
        public async Task<T> GetByIndexAsync<T>(string key, long index) where T : class
        {
            _cachePipe.Pipe.LIndex(key, index);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 获取指定区间的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>指定区间的元素列表</returns>
        public Task<List<string>> GetRangeAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.LRange(key, start, stop);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 获取指定区间的元素（泛型）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>指定区间的元素列表</returns>
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

        /// <summary>
        /// 在指定元素前插入新元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public Task<long> InsertBeforeAsync(string key, string pivot, string value)
        {
            _cachePipe.Pipe.LInsertBefore(key, pivot, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 在指定元素前插入新元素（泛型）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public Task<long> InsertBeforeAsync<T>(string key, string pivot, T value) where T : class
        {
            _cachePipe.Pipe.LInsertBefore(key, pivot, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 在指定元素后插入新元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public Task<long> InsertAfterAsync(string key, string pivot, string value)
        {
            _cachePipe.Pipe.LInsertAfter(key, pivot, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 在指定元素后插入新元素（泛型）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public Task<long> InsertAfterAsync<T>(string key, string pivot, T value) where T : class
        {
            _cachePipe.Pipe.LInsertAfter(key, pivot, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 通过索引设置元素值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">新值</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetByIndexAsync(string key, long index, string value)
        {
            _cachePipe.Pipe.LSet(key, index, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 通过索引设置元素值（泛型）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">新值</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetByIndexAsync<T>(string key, long index, T value) where T : class
        {
            _cachePipe.Pipe.LSet(key, index, JsonConvert.SerializeObject(value, _cachePipe.JsonSetting));
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表长度</returns>
        public Task<long> LengthAsync(string key)
        {
            _cachePipe.Pipe.LLen(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 移除指定数量的匹配元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">移除数量（正数从头部移除，负数从尾部移除）</param>
        /// <param name="value">要匹配的值</param>
        /// <returns>实际移除的元素数量</returns>
        public Task<long> RemoveAsync(string key, long count, string value)
        {
            _cachePipe.Pipe.LRem(key, count, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 保留指定区间元素，丢弃其余
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>是否操作成功</returns>
        public Task<bool> TrimAsync(string key, long start, long stop)
        {
            _cachePipe.Pipe.LTrim(key, start, stop);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 从源列表尾部弹出，推入目标列表头部
        /// </summary>
        /// <param name="sourceKey">源缓存键</param>
        /// <param name="targetKey">目标缓存键</param>
        /// <returns>被移动的元素值</returns>
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
