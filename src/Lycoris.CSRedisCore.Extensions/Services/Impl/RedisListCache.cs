using CSRedis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// Redis List 缓存操作实现，提供列表的双端入队出队、索引访问、范围查询、插入删除等功能
    /// </summary>
    public sealed class RedisListCache : IRedisListCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;
        private readonly string PrefixCacheKey;

        /// <summary>
        /// 初始化 RedisListCache 实例
        /// </summary>
        /// <param name="CSRedisCore">CSRedis 客户端实例</param>
        /// <param name="JsonSetting">JSON 序列化配置</param>
        /// <param name="prefixCacheKey">缓存键前缀</param>
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

        /// <summary>
        /// 获取列表中的所有元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表中所有元素的列表</returns>
        public async Task<List<string>> GetAllAsync(string key) => (await CSRedisCore.LRangeAsync(key, 0, -1)).ToList();

        /// <summary>
        /// 获取列表中的所有元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表中所有元素的列表</returns>
        public List<string> GetAll(string key) => CSRedisCore.LRange(key, 0, -1).ToList();

        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>指定索引处的元素</returns>
        public string GetByIndex(string key, long index) => CSRedisCore.LIndex(key, index);

        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>指定索引处的元素</returns>
        public async Task<string> GetByIndexAsync(string key, long index) => await CSRedisCore.LIndexAsync(key, index);

        /// <summary>
        /// 通过索引获取列表中的元素，并反序列化为指定类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>指定索引处的元素，反序列化为泛型对象</returns>
        public T GetByIndex<T>(string key, long index) where T : class
        {
            var str = GetByIndex(key, index);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 通过索引获取列表中的元素，并反序列化为指定类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>指定索引处的元素，反序列化为泛型对象</returns>
        public async Task<T> GetByIndexAsync<T>(string key, long index) where T : class
        {
            var str = await GetByIndexAsync(key, index);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns>指定范围内的元素列表</returns>
        public List<string> GetRange(string key, long start, long stop) => CSRedisCore.LRange(key, start, stop).ToList();

        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns>指定范围内的元素列表</returns>
        public async Task<List<string>> GetRangeAsync(string key, long start, long stop) => (await CSRedisCore.LRangeAsync(key, start, stop)).ToList();

        /// <summary>
        /// 获取列表指定范围内的元素，并反序列化为指定类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns>指定范围内的泛型对象列表</returns>
        public List<T> GetRange<T>(string key, long start, long stop) where T : class
        {
            var items = GetRange(key, start, stop);
            var result = new List<T>();
            foreach (var item in items)
                result.Add(string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item));
            return result;
        }

        /// <summary>
        /// 获取列表指定范围内的元素，并反序列化为指定类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns>指定范围内的泛型对象列表</returns>
        public async Task<List<T>> GetRangeAsync<T>(string key, long start, long stop) where T : class
        {
            var items = await GetRangeAsync(key, start, stop);
            var result = new List<T>();
            foreach (var item in items)
                result.Add(string.IsNullOrEmpty(item) ? default : JsonConvert.DeserializeObject<T>(item));
            return result;
        }

        /// <summary>
        /// 将值插入到列表指定元素之前
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public long InsertBefore(string key, string pivot, string value) => CSRedisCore.LInsertBefore(key, pivot, value);

        /// <summary>
        /// 将值插入到列表指定元素之前
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public async Task<long> InsertBeforeAsync(string key, string pivot, string value) => await CSRedisCore.LInsertBeforeAsync(key, pivot, value);

        /// <summary>
        /// 将泛型对象序列化后插入到列表指定元素之前
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的泛型对象</param>
        /// <returns>插入后列表的长度</returns>
        public long InsertBefore<T>(string key, string pivot, T value) where T : class => InsertBefore(key, pivot, JsonConvert.SerializeObject(value));

        /// <summary>
        /// 将泛型对象序列化后插入到列表指定元素之前
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的泛型对象</param>
        /// <returns>插入后列表的长度</returns>
        public async Task<long> InsertBeforeAsync<T>(string key, string pivot, T value) where T : class => await InsertBeforeAsync(key, pivot, JsonConvert.SerializeObject(value));

        /// <summary>
        /// 将值插入到列表指定元素之后
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public long InsertAfter(string key, string pivot, string value) => CSRedisCore.LInsertAfter(key, pivot, value);

        /// <summary>
        /// 将值插入到列表指定元素之后
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        public async Task<long> InsertAfterAsync(string key, string pivot, string value) => await CSRedisCore.LInsertAfterAsync(key, pivot, value);

        /// <summary>
        /// 将泛型对象序列化后插入到列表指定元素之后
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的泛型对象</param>
        /// <returns>插入后列表的长度</returns>
        public long InsertAfter<T>(string key, string pivot, T value) where T : class => InsertAfter(key, pivot, JsonConvert.SerializeObject(value));

        /// <summary>
        /// 将泛型对象序列化后插入到列表指定元素之后
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">参考元素</param>
        /// <param name="value">要插入的泛型对象</param>
        /// <returns>插入后列表的长度</returns>
        public async Task<long> InsertAfterAsync<T>(string key, string pivot, T value) where T : class => await InsertAfterAsync(key, pivot, JsonConvert.SerializeObject(value));

        /// <summary>
        /// 通过索引设置列表元素的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">新值</param>
        /// <returns>是否设置成功</returns>
        public bool SetByIndex(string key, long index, string value) => CSRedisCore.LSet(key, index, value);

        /// <summary>
        /// 通过索引设置列表元素的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">新值</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetByIndexAsync(string key, long index, string value) => await CSRedisCore.LSetAsync(key, index, value);

        /// <summary>
        /// 通过索引设置列表元素的值（泛型对象自动序列化）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">泛型对象</param>
        /// <returns>是否设置成功</returns>
        public bool SetByIndex<T>(string key, long index, T value) where T : class => SetByIndex(key, index, JsonConvert.SerializeObject(value));

        /// <summary>
        /// 通过索引设置列表元素的值（泛型对象自动序列化）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">泛型对象</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetByIndexAsync<T>(string key, long index, T value) where T : class => await SetByIndexAsync(key, index, JsonConvert.SerializeObject(value));

        /// <summary>
        /// 获取列表的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表长度</returns>
        public long Length(string key) => CSRedisCore.LLen(key);

        /// <summary>
        /// 获取列表的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表长度</returns>
        public async Task<long> LengthAsync(string key) => await CSRedisCore.LLenAsync(key);

        /// <summary>
        /// 移除列表中指定数量的匹配元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">移除数量（正数从头开始，负数从尾开始，0 移除全部）</param>
        /// <param name="value">要移除的值</param>
        /// <returns>实际移除的元素数量</returns>
        public long Remove(string key, long count, string value) => CSRedisCore.LRem(key, count, value);

        /// <summary>
        /// 移除列表中指定数量的匹配元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">移除数量（正数从头开始，负数从尾开始，0 移除全部）</param>
        /// <param name="value">要移除的值</param>
        /// <returns>实际移除的元素数量</returns>
        public async Task<long> RemoveAsync(string key, long count, string value) => await CSRedisCore.LRemAsync(key, count, value);

        /// <summary>
        /// 修剪列表，只保留指定范围内的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns>是否修剪成功</returns>
        public bool Trim(string key, long start, long stop) => CSRedisCore.LTrim(key, start, stop);

        /// <summary>
        /// 修剪列表，只保留指定范围内的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns>是否修剪成功</returns>
        public async Task<bool> TrimAsync(string key, long start, long stop) => await CSRedisCore.LTrimAsync(key, start, stop);

        /// <summary>
        /// 将源列表的最后一个元素弹出并插入到目标列表的头部
        /// </summary>
        /// <param name="sourceKey">源列表键</param>
        /// <param name="targetKey">目标列表键</param>
        /// <returns>被移动的元素</returns>
        public string PopLastPushFirst(string sourceKey, string targetKey) => CSRedisCore.RPopLPush(sourceKey, targetKey);

        /// <summary>
        /// 将源列表的最后一个元素弹出并插入到目标列表的头部
        /// </summary>
        /// <param name="sourceKey">源列表键</param>
        /// <param name="targetKey">目标列表键</param>
        /// <returns>被移动的元素</returns>
        public async Task<string> PopLastPushFirstAsync(string sourceKey, string targetKey) => await CSRedisCore.RPopLPushAsync(sourceKey, targetKey);
    }
}
