using CSRedis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RedisListCache : IRedisListCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;
        private readonly string PrefixCacheKey;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        /// <param name="prefixCacheKey"></param>
        public RedisListCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting, string prefixCacheKey)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
            PrefixCacheKey = prefixCacheKey;
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAndRemoveFirst(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return CSRedisCore.LPop(key);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAndRemoveFirstAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return await CSRedisCore.LPopAsync(key);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetAndRemoveFirst<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return default;

            var str = CSRedisCore.LPop(key);

            if (string.IsNullOrEmpty(str))
                return default;

            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAndRemoveFirstAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return default;

            var str = await CSRedisCore.LPopAsync(key);

            if (string.IsNullOrEmpty(str))
                return default;

            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 移出并获取列表的最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAndRemoveLast(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return CSRedisCore.RPop(key);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAndRemoveLastAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return await CSRedisCore.RPopAsync(key);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetAndRemoveLast<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return default;

            var str = CSRedisCore.RPop(key);

            if (string.IsNullOrEmpty(str))
                return default;

            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAndRemoveLastAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return default;

            var str = await CSRedisCore.RPopAsync(key);

            if (string.IsNullOrEmpty(str))
                return default;

            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetFirst(string key, params string[] value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            CSRedisCore.LPush(key, value);
        }

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetFirstAsync(string key, params string[] value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            await CSRedisCore.LPushAsync(key, value);
        }

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetFirst<T>(string key, params T[] value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            var list = new List<string>();
            foreach (var item in value)
            {
                var json = JsonConvert.SerializeObject(item, JsonSetting);
                list.Add(json);
            }

            CSRedisCore.LPush(key, list.ToArray());
        }

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetFirstAsync<T>(string key, params T[] value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            var list = new List<string>();
            foreach (var item in value)
            {
                var json = JsonConvert.SerializeObject(item, JsonSetting);
                list.Add(json);
            }

            await CSRedisCore.LPushAsync(key, list.ToArray());
        }

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetLast(string key, params string[] value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            CSRedisCore.RPush(key, value);
        }

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetLastAsync(string key, params string[] value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            await CSRedisCore.RPushAsync(key, value);
        }

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetLast<T>(string key, params T[] value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            var list = new List<string>();
            foreach (var item in value)
            {
                var json = JsonConvert.SerializeObject(item, JsonSetting);
                list.Add(json);
            }

            CSRedisCore.RPush(key, list.ToArray());
        }

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetLastAsync<T>(string key, params T[] value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            var list = new List<string>();
            foreach (var item in value)
            {
                var json = JsonConvert.SerializeObject(item, JsonSetting);
                list.Add(json);
            }

            await CSRedisCore.RPushAsync(key, list.ToArray());
        }

        public async Task<List<string>> GetAllAsync(string key) => (await CSRedisCore.LRangeAsync(key, 0, -1)).ToList();

        public List<string> GetAll(string key) => CSRedisCore.LRange(key, 0, -1).ToList();

        public string GetByIndex(string key, long index) => CSRedisCore.LIndex(key, index);

        public async Task<string> GetByIndexAsync(string key, long index) => await CSRedisCore.LIndexAsync(key, index);

        public T GetByIndex<T>(string key, long index) where T : class
        {
            var str = GetByIndex(key, index);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        public async Task<T> GetByIndexAsync<T>(string key, long index) where T : class
        {
            var str = await GetByIndexAsync(key, index);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        public List<string> GetRange(string key, long start, long stop) => CSRedisCore.LRange(key, start, stop).ToList();

        public async Task<List<string>> GetRangeAsync(string key, long start, long stop) => (await CSRedisCore.LRangeAsync(key, start, stop)).ToList();

        public List<T> GetRange<T>(string key, long start, long stop) where T : class
        {
            var items = GetRange(key, start, stop);
            var result = new List<T>();
            foreach (var item in items)
                result.Add(string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item));
            return result;
        }

        public async Task<List<T>> GetRangeAsync<T>(string key, long start, long stop) where T : class
        {
            var items = await GetRangeAsync(key, start, stop);
            var result = new List<T>();
            foreach (var item in items)
                result.Add(string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item));
            return result;
        }

        public long InsertBefore(string key, string pivot, string value) => CSRedisCore.LInsertBefore(key, pivot, value);

        public async Task<long> InsertBeforeAsync(string key, string pivot, string value) => await CSRedisCore.LInsertBeforeAsync(key, pivot, value);

        public long InsertBefore<T>(string key, string pivot, T value) where T : class => InsertBefore(key, pivot, JsonConvert.SerializeObject(value));

        public async Task<long> InsertBeforeAsync<T>(string key, string pivot, T value) where T : class => await InsertBeforeAsync(key, pivot, JsonConvert.SerializeObject(value));

        public long InsertAfter(string key, string pivot, string value) => CSRedisCore.LInsertAfter(key, pivot, value);

        public async Task<long> InsertAfterAsync(string key, string pivot, string value) => await CSRedisCore.LInsertAfterAsync(key, pivot, value);

        public long InsertAfter<T>(string key, string pivot, T value) where T : class => InsertAfter(key, pivot, JsonConvert.SerializeObject(value));

        public async Task<long> InsertAfterAsync<T>(string key, string pivot, T value) where T : class => await InsertAfterAsync(key, pivot, JsonConvert.SerializeObject(value));

        public bool SetByIndex(string key, long index, string value) => CSRedisCore.LSet(key, index, value);

        public async Task<bool> SetByIndexAsync(string key, long index, string value) => await CSRedisCore.LSetAsync(key, index, value);

        public bool SetByIndex<T>(string key, long index, T value) where T : class => SetByIndex(key, index, JsonConvert.SerializeObject(value));

        public async Task<bool> SetByIndexAsync<T>(string key, long index, T value) where T : class => await SetByIndexAsync(key, index, JsonConvert.SerializeObject(value));

        public long Length(string key) => CSRedisCore.LLen(key);

        public async Task<long> LengthAsync(string key) => await CSRedisCore.LLenAsync(key);

        public long Remove(string key, long count, string value) => CSRedisCore.LRem(key, count, value);

        public async Task<long> RemoveAsync(string key, long count, string value) => await CSRedisCore.LRemAsync(key, count, value);

        public bool Trim(string key, long start, long stop) => CSRedisCore.LTrim(key, start, stop);

        public async Task<bool> TrimAsync(string key, long start, long stop) => await CSRedisCore.LTrimAsync(key, start, stop);

        public string PopLastPushFirst(string sourceKey, string targetKey) => CSRedisCore.RPopLPush(sourceKey, targetKey);

        public async Task<string> PopLastPushFirstAsync(string sourceKey, string targetKey) => await CSRedisCore.RPopLPushAsync(sourceKey, targetKey);
    }
}
