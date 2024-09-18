using CSRedis;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisListCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
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
    }
}
