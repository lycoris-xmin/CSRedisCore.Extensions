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

        long IRedisHashCache.Remove(string key, params string[] fieIds) => throw NewNotSupportedException();

        bool IRedisHashCache.Exists(string key, string fieId) => throw NewNotSupportedException();

        string IRedisHashCache.Get(string key, string fieId) => throw NewNotSupportedException();

        T IRedisHashCache.Get<T>(string key, string fieId) where T : class => throw NewNotSupportedException();

        List<string> IRedisHashCache.Get(string key, params string[] fieIds) => throw NewNotSupportedException();

        List<T> IRedisHashCache.Get<T>(string key, params string[] fieIds) where T : class => throw NewNotSupportedException();

        Dictionary<string, string> IRedisHashCache.GetAll(string key) => throw NewNotSupportedException();

        Dictionary<string, T> IRedisHashCache.GetAll<T>(string key) where T : class => throw NewNotSupportedException();

        List<string> IRedisHashCache.FieIds(string key) => throw NewNotSupportedException();

        long IRedisHashCache.FieIdsCount(string key) => throw NewNotSupportedException();

        bool IRedisHashCache.Set(string key, params (string, string)[] value) => throw NewNotSupportedException();

        bool IRedisHashCache.Set<T>(string key, params (string, T)[] value) where T : class => throw NewNotSupportedException();

        bool IRedisHashCache.Set(string key, string fieId, string value) => throw NewNotSupportedException();

        bool IRedisHashCache.Set<T>(string key, string fieId, T value) where T : class => throw NewNotSupportedException();

        bool IRedisHashCache.SetNx(string key, string fieId, string value) => throw NewNotSupportedException();

        bool IRedisHashCache.SetNx<T>(string key, string fieId, T value) where T : class => throw NewNotSupportedException();

        long IRedisHashCache.Addition(string key, string fieId, long value, TimeSpan? timeSpan) => throw NewNotSupportedException();

        decimal IRedisHashCache.IncrByFloat(string key, string fieId, decimal value) => throw NewNotSupportedException();

        List<string> IRedisHashCache.Values(string key) => throw NewNotSupportedException();

        long IRedisHashCache.FieldStringLength(string key, string fieId) => throw NewNotSupportedException();

        Models.RedisScanResult<Dictionary<string, string>> IRedisHashCache.Scan(string key, long cursor, string pattern, long? count) => throw NewNotSupportedException();

        #endregion

        #region async

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">要删除的字段</param>
        /// <returns>被删除的字段数量</returns>
        public Task<long> RemoveAsync(string key, params string[] fieIds)
        {
            _cachePipe.Pipe.HDel(key, fieIds);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>是否存在</returns>
        public Task<bool> ExistsAsync(string key, string fieId)
        {
            _cachePipe.Pipe.HExists(key, fieId);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段值</returns>
        public Task<string> GetAsync(string key, string fieId)
        {
            _cachePipe.Pipe.HGet(key, fieId);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段值</returns>
        public async Task<T> GetAsync<T>(string key, string fieId) where T : class
        {
            _cachePipe.Pipe.HGet(key, fieId);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">要获取的字段名</param>
        /// <returns>字段值列表</returns>
        public Task<List<string>> GetAsync(string key, params string[] fieIds)
        {
            _cachePipe.Pipe.HMGet(key, fieIds);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">要获取的字段名</param>
        /// <returns>字段值列表</returns>
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

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>所有字段和值的字典</returns>
        public Task<Dictionary<string, string>> GetAllAsync(string key)
        {
            _cachePipe.Pipe.HGetAll(key);
            return _cachePipe.EnqueueResult<Dictionary<string, string>>();
        }

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>所有字段和值的字典</returns>
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

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段名列表</returns>
        public Task<List<string>> FieIdsAsync(string key)
        {
            _cachePipe.Pipe.HKeys(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段数量</returns>
        public Task<long> FieIdsCountAsync(string key)
        {
            _cachePipe.Pipe.HLen(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">字段-值对</param>
        /// <returns>是否设置成功</returns>
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

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">字段-值对</param>
        /// <returns>是否设置成功</returns>
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

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是哈希表中的一个新建字段且值设置成功返回 true，如果字段已存在且旧值被覆盖返回 false</returns>
        public Task<bool> SetAsync(string key, string fieId, string value)
        {
            _cachePipe.Pipe.HSet(key, fieId, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是哈希表中的一个新建字段且值设置成功返回 true，如果字段已存在且旧值被覆盖返回 false</returns>
        public Task<bool> SetAsync<T>(string key, string fieId, T value) where T : class
        {
            var json = JsonConvert.SerializeObject(value, _cachePipe.JsonSetting);
            _cachePipe.Pipe.HSet(key, fieId, json);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetNxAsync(string key, string fieId, string value)
        {
            _cachePipe.Pipe.HSetNx(key, fieId, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> SetNxAsync<T>(string key, string fieId, T value) where T : class
        {
            var json = JsonConvert.SerializeObject(value, _cachePipe.JsonSetting);
            _cachePipe.Pipe.HSetNx(key, fieId, json);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">增量值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>增量后的值</returns>
        public Task<long> AdditionAsync(string key, string fieId, long value = 1, TimeSpan? timeSpan = null)
        {
            _cachePipe.Pipe.HIncrBy(key, fieId, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 为哈希表 key 中的指定字段加上浮点增量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">增量值</param>
        /// <returns>增量后的值</returns>
        public Task<decimal> IncrByFloatAsync(string key, string fieId, decimal value)
        {
            _cachePipe.Pipe.HIncrByFloat(key, fieId, value);
            return _cachePipe.EnqueueResult<decimal>();
        }

        /// <summary>
        /// 获取哈希表中所有字段的值（不返回键名）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段值列表</returns>
        public Task<List<string>> ValuesAsync(string key)
        {
            _cachePipe.Pipe.HVals(key);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 获取哈希表指定字段值的字符串长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字符串长度</returns>
        public Task<long> FieldStringLengthAsync(string key, string fieId)
        {
            _cachePipe.Pipe.HStrLen(key, fieId);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 同步方法，管道模式不支持，请使用异步重载
        /// </summary>
        public Task<Models.RedisScanResult<Dictionary<string, string>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for hash scans. Use PipeExecute/raw pipe access for scan operations.");

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
