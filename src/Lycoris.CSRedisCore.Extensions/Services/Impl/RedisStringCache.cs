using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RedisStringCache : IRedisStringCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisStringCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
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
    }
}
