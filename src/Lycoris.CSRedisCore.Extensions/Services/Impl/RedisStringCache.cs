using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// Redis String 缓存操作实现，提供字符串值的增删改查、批量操作、原子增减等功能
    /// </summary>
    public sealed class RedisStringCache : IRedisStringCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;
        private readonly string PrefixCacheKey;

        /// <summary>
        /// 初始化 RedisStringCache 实例
        /// </summary>
        /// <param name="CSRedisCore">CSRedis 客户端实例</param>
        /// <param name="JsonSetting">JSON 序列化配置</param>
        /// <param name="prefixCacheKey">缓存键前缀</param>
        public RedisStringCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting, string prefixCacheKey)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
            PrefixCacheKey = prefixCacheKey;
        }

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return CSRedisCore.Get(key);
        }

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var val = CSRedisCore.Get(key);
            return string.IsNullOrEmpty(val) ? default : JsonConvert.DeserializeObject<T>(val, JsonSetting);
        }

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return await CSRedisCore.GetAsync(key);
        }

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var val = await CSRedisCore.GetAsync(key);
            return string.IsNullOrEmpty(val) ? default : JsonConvert.DeserializeObject<T>(val, JsonSetting);
        }

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> MultipleGet(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var keys = CSRedisCore.Keys(key);

            if (keys == null || keys.Length == 0)
                return null;

            return CSRedisCore.MGet(key)?.ToList();
        }

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<string>> MultipleGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var keys = CSRedisCore.Keys(key);

            if (keys == null || keys.Length == 0)
                return null;

            return (await CSRedisCore.MGetAsync(key))?.ToList();
        }

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> MultipleGet<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var keys = CSRedisCore.Keys(key);

            if (keys == null || keys.Length == 0)
                return null;

            var cache = CSRedisCore.MGet(key)?.ToList();
            if (cache == null || !cache.Any())
                return null;

            var list = new List<T>();
            foreach (var item in cache)
            {
                if (string.IsNullOrEmpty(item))
                    list.Add(default);
                else
                    list.Add(JsonConvert.DeserializeObject<T>(item));
            }

            return list;
        }

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> MultipleGetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var keys = CSRedisCore.Keys(key);

            if (keys == null || keys.Length == 0)
                return null;

            var cache = (await CSRedisCore.MGetAsync(key))?.ToList();

            if (cache == null || !cache.Any())
                return null;

            var list = new List<T>();
            foreach (var item in cache)
            {
                if (string.IsNullOrEmpty(item))
                    list.Add(default);
                else
                    list.Add(JsonConvert.DeserializeObject<T>(item));
            }

            return list;
        }

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> MultipleGet(params string[] key)
        {
            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key));

            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (key == null || key.Length == 0)
                return null;

            return CSRedisCore.MGet(key)?.ToList();
        }

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> MultipleGet<T>(params string[] key) where T : class
        {
            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key));

            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (key == null || key.Length == 0)
                return null;

            var val = CSRedisCore.MGet(key)?.ToList();
            if (val == null || val.Count == 0 || !val.Any(x => x != null))
                return null;

            var list = new List<T>();

            foreach (var item in val)
            {
                if (!string.IsNullOrEmpty(item))
                    list.Add(JsonConvert.DeserializeObject<T>(item, JsonSetting));
                else
                    list.Add(default);
            }

            return list;
        }

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<string>> MultipleGetAsync(params string[] key)
        {
            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key));

            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (key == null || key.Length == 0)
                return null;

            return (await CSRedisCore.MGetAsync(key))?.ToList();
        }

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> MultipleGetAsync<T>(params string[] key) where T : class
        {
            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key));

            key = key.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key), "is all empty");

            var val = (await CSRedisCore.MGetAsync(key))?.ToList();
            if (val == null || val.Count == 0 || !val.Any(x => x != null))
                return null;

            var list = new List<T>();

            foreach (var item in val)
            {
                if (!string.IsNullOrEmpty(item))
                    list.Add(JsonConvert.DeserializeObject<T>(item, JsonSetting));
                else
                    list.Add(default);
            }

            return list;
        }

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetSet(string key, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return CSRedisCore.GetSet(key, value);
        }

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> GetSetAsync(string key, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return await CSRedisCore.GetSetAsync(key, value);
        }

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetSet<T>(string key, T value) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return CSRedisCore.GetSet(key, JsonConvert.SerializeObject(value, JsonSetting));
        }

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> GetSetAsync<T>(string key, T value) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return await CSRedisCore.GetSetAsync(key, JsonConvert.SerializeObject(value, JsonSetting));
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public bool Set(string key, string value, TimeSpan? expire = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (expire != null)
                return CSRedisCore.Set(key, value, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));
            else
                return CSRedisCore.Set(key, value);
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (expire != null)
                return await CSRedisCore.SetAsync(key, value, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));
            else
                return await CSRedisCore.SetAsync(key, value);
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (expire != null)
                return CSRedisCore.Set(key, JsonConvert.SerializeObject(value, JsonSetting), expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));
            else
                return CSRedisCore.Set(key, JsonConvert.SerializeObject(value, JsonSetting));
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (expire != null)
                return await CSRedisCore.SetAsync(key, JsonConvert.SerializeObject(value, JsonSetting), expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));
            else
                return await CSRedisCore.SetAsync(key, JsonConvert.SerializeObject(value, JsonSetting));
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> SetIfNotExistsAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var result = await CSRedisCore.SetNxAsync(key, value);
            if (result && expire != null)
                await CSRedisCore.ExpireAsync(key, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));

            return result;
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> SetIfNotExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var result = await CSRedisCore.SetNxAsync(key, JsonConvert.SerializeObject(value, JsonSetting));
            if (result && expire != null)
                await CSRedisCore.ExpireAsync(key, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));

            return result;
        }

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public bool MultipleSet(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Length == 0)
                throw new ArgumentNullException(nameof(keyValues));

            var temp = keyValues.Where(x => x != null).ToArray();

            if (temp == null || temp.Length == 0)
                throw new ArgumentNullException(nameof(keyValues), "is all empty");

            return CSRedisCore.MSet(keyValues);
        }

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public async Task<bool> MultipleSetAsync(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Length == 0)
                throw new ArgumentNullException(nameof(keyValues));

            var temp = keyValues.Where(x => x != null).ToArray();

            if (temp == null || temp.Length == 0)
                throw new ArgumentNullException(nameof(keyValues), "is all empty");

            return await CSRedisCore.MSetAsync(keyValues);
        }

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        public long Addition(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            long? ttl = null;
            if (timeSpan == null)
                ttl = CSRedisCore.Ttl(key);

            var cache = CSRedisCore.IncrBy(key, value);

            if (timeSpan.HasValue || ttl.HasValue)
            {
                if (!timeSpan.HasValue && ttl.HasValue)
                    timeSpan = TimeSpan.FromSeconds(ttl.Value);

                CSRedisCore.Expire(key, (int)Math.Ceiling(timeSpan?.TotalSeconds ?? -1));
            }

            return cache;
        }

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        public async Task<long> AdditionAsync(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            long? ttl = null;
            if (timeSpan == null)
                ttl = await CSRedisCore.TtlAsync(key);

            var cache = await CSRedisCore.IncrByAsync(key, value);

            if (timeSpan.HasValue || ttl.HasValue)
            {
                if (!timeSpan.HasValue && ttl.HasValue)
                    timeSpan = TimeSpan.FromSeconds(ttl.Value);

                await CSRedisCore.ExpireAsync(key, (int)Math.Ceiling(timeSpan?.TotalSeconds ?? -1));
            }

            return cache;
        }

        /// <summary>
        /// 将 key 所储存的值减去给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值为正整数</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        public long Subtraction(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            var cache = CSRedisCore.IncrBy(key, 0 - value);
            if (timeSpan != null)
                CSRedisCore.Expire(key, timeSpan.Value);

            return cache;
        }

        /// <summary>
        /// 将 key 所储存的值减去给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值为正整数</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        public async Task<long> SubtractionAsync(string key, long value = 1, TimeSpan? timeSpan = null)
        {
            var cache = await CSRedisCore.IncrByAsync(key, 0 - value);
            if (timeSpan != null)
                CSRedisCore.Expire(key, timeSpan.Value);

            return cache;
        }

        /// <summary>
        /// 仅当 key 已存在时设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public bool SetIfExists(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : 30;
            if (expire != null)
                return CSRedisCore.Set(key, value, seconds, RedisExistence.Xx);
            else
                return CSRedisCore.Set(key, value, 30, RedisExistence.Xx);
        }

        /// <summary>
        /// 仅当 key 已存在时设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetIfExistsAsync(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var seconds = expire.HasValue ? (int)expire.Value.TotalSeconds : 30;
            if (expire != null)
                return await CSRedisCore.SetAsync(key, value, seconds, RedisExistence.Xx);
            else
                return await CSRedisCore.SetAsync(key, value, 30, RedisExistence.Xx);
        }

        /// <summary>
        /// 仅当 key 已存在时设置泛型对象值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public bool SetIfExists<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return SetIfExists(key, JsonConvert.SerializeObject(value, JsonSetting), expire);
        }

        /// <summary>
        /// 仅当 key 已存在时设置泛型对象值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetIfExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return await SetIfExistsAsync(key, JsonConvert.SerializeObject(value, JsonSetting), expire);
        }

        /// <summary>
        /// 获取指定 key 所储存的字符串值的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字符串长度</returns>
        public long StringLength(string key) => CSRedisCore.StrLen(key);

        /// <summary>
        /// 获取指定 key 所储存的字符串值的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字符串长度</returns>
        public async Task<long> StringLengthAsync(string key) => await CSRedisCore.StrLenAsync(key);

        /// <summary>
        /// 将指定的 value 追加到 key 原值的末尾
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要追加的值</param>
        /// <returns>追加后字符串的长度</returns>
        public long Append(string key, string value) => CSRedisCore.Append(key, value);

        /// <summary>
        /// 将指定的 value 追加到 key 原值的末尾
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要追加的值</param>
        /// <returns>追加后字符串的长度</returns>
        public async Task<long> AppendAsync(string key, string value) => await CSRedisCore.AppendAsync(key, value);

        /// <summary>
        /// 获取指定 key 的子字符串（按字节位置截取）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>截取的子字符串</returns>
        public string GetRange(string key, long start, long end) => CSRedisCore.GetRange(key, start, end);

        /// <summary>
        /// 获取指定 key 的子字符串（按字节位置截取）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>截取的子字符串</returns>
        public async Task<string> GetRangeAsync(string key, long start, long end) => await CSRedisCore.GetRangeAsync(key, start, end);

        /// <summary>
        /// 用指定的 value 覆盖 key 所储存的字符串值从偏移量 offset 开始的部分
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">要写入的值</param>
        /// <returns>修改后的字符串长度</returns>
        public long SetRange(string key, uint offset, string value) => CSRedisCore.SetRange(key, offset, value);

        /// <summary>
        /// 用指定的 value 覆盖 key 所储存的字符串值从偏移量 offset 开始的部分
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">要写入的值</param>
        /// <returns>修改后的字符串长度</returns>
        public async Task<long> SetRangeAsync(string key, uint offset, string value) => await CSRedisCore.SetRangeAsync(key, offset, value);

        /// <summary>
        /// 仅当 key 不存在时设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public bool SetIfNotExists(string key, string value, TimeSpan? expire = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var result = CSRedisCore.SetNx(key, value);
            if (result && expire != null)
                CSRedisCore.Expire(key, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));

            return result;
        }

        /// <summary>
        /// 仅当 key 不存在时设置泛型对象值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public bool SetIfNotExists<T>(string key, T value, TimeSpan? expire = null) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var result = CSRedisCore.SetNx(key, JsonConvert.SerializeObject(value, JsonSetting));
            if (result && expire != null)
                CSRedisCore.Expire(key, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));

            return result;
        }

        /// <summary>
        /// 获取指定 key 的原始字节值
        /// </summary>
        public byte[] GetBytes(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var b64 = CSRedisCore.Get(key);
            return string.IsNullOrEmpty(b64) ? null : Convert.FromBase64String(b64);
        }

        /// <summary>
        /// 获取指定 key 的原始字节值
        /// </summary>
        public async Task<byte[]> GetBytesAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var b64 = await CSRedisCore.GetAsync(key);
            return string.IsNullOrEmpty(b64) ? null : Convert.FromBase64String(b64);
        }

        /// <summary>
        /// 设置指定 key 的原始字节值
        /// </summary>
        public bool SetBytes(string key, byte[] value, TimeSpan? expire = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var b64 = Convert.ToBase64String(value);

            if (expire != null)
                return CSRedisCore.Set(key, b64, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));
            else
                return CSRedisCore.Set(key, b64);
        }

        /// <summary>
        /// 设置指定 key 的原始字节值
        /// </summary>
        public async Task<bool> SetBytesAsync(string key, byte[] value, TimeSpan? expire = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var b64 = Convert.ToBase64String(value);

            if (expire != null)
                return await CSRedisCore.SetAsync(key, b64, expire.Value.Add(TimeSpan.FromSeconds(new Random().Next(1, 30))));
            else
                return await CSRedisCore.SetAsync(key, b64);
        }
    }
}
