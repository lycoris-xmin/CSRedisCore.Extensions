using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisKeyCache : IRedisKeyCache
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly string PrefixCacheKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="PrefixCacheKey"></param>
        public RedisKeyCache(CSRedisClient CSRedisCore, string PrefixCacheKey)
        {
            this.CSRedisCore = CSRedisCore;
            this.PrefixCacheKey = string.IsNullOrEmpty(PrefixCacheKey) ? "" : $"{PrefixCacheKey}:";
        }

        /// <summary>
        /// 模糊查找所有键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string[] GetKeys(string pattern)
        {
            var keys = CSRedisCore.Keys($"{this.PrefixCacheKey}{(pattern.EndsWith("*") ? pattern : $"{pattern}*")}");
            if (keys == null || keys.Length == 0)
                return Array.Empty<string>();

            // 移除每个键的前缀部分
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].StartsWith(this.PrefixCacheKey))
                    keys[i] = keys[i].Substring(this.PrefixCacheKey.Length);

                keys[i] = keys[i].TrimStart(':');
            }

            return keys;
        }

        /// <summary>
        /// 模糊查找所有键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<string[]> GetKeysAsync(string pattern)
        {
            var keys = await CSRedisCore.KeysAsync($"{this.PrefixCacheKey}{(pattern.EndsWith("*") ? pattern : $"{pattern}*")}");
            if (keys == null || keys.Length == 0)
                return Array.Empty<string>();

            // 移除每个键的前缀部分
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].StartsWith(this.PrefixCacheKey))
                    keys[i] = keys[i].Substring(this.PrefixCacheKey.Length);

                keys[i] = keys[i].TrimStart(':');
            }

            return keys;
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key) => !string.IsNullOrEmpty(key) && CSRedisCore.Exists(key);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key) => !string.IsNullOrEmpty(key) && await CSRedisCore.ExistsAsync(key);

        /// <summary>
        /// 为给定 key 设置过期时间,以秒计
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public bool Expire(string key, TimeSpan expire) => !string.IsNullOrEmpty(key) && CSRedisCore.Expire(key, expire);

        /// <summary>
        /// 为给定 key 设置过期时间,以秒计
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> ExpireAsync(string key, TimeSpan expire)
            => !string.IsNullOrEmpty(key) && await CSRedisCore.ExpireAsync(key, expire);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Persist(string key)
            => !string.IsNullOrEmpty(key) && CSRedisCore.Persist(key);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> PersistAsync(string key)
            => !string.IsNullOrEmpty(key) && await CSRedisCore.PersistAsync(key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long TTL(string key)
            => string.IsNullOrEmpty(key) ? 0 : CSRedisCore.Ttl(key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> TTLAsync(string key)
            => string.IsNullOrEmpty(key) ? 0 : await CSRedisCore.TtlAsync(key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Rename(string oldkey, string key)
            => !string.IsNullOrEmpty(key) && CSRedisCore.Rename(oldkey, key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RenameAsync(string oldkey, string key)
            => !string.IsNullOrEmpty(key) && await CSRedisCore.RenameAsync(oldkey, key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RenameIfNoExists(string oldkey, string key)
            => !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(key) && CSRedisCore.RenameNx(oldkey, key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RenameIfNoExistsAsync(string oldkey, string key)
            => !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(key) && await CSRedisCore.RenameNxAsync(oldkey, key);

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(params string[] key)
        {
            if (key != null && key.Length > 0)
            {
                var result = CSRedisCore.Del(key) > 0;
                if (!result)
                {
                    var keys = new string[key.Length];
                    for (int i = 0; i < key.Length; i++)
                    {
                        keys[i] = $"{this.PrefixCacheKey}{key}";
                    }

                    return CSRedisCore.Del(keys) > 0;
                }
            }

            return true;
        }

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(params string[] key)
        {
            if (key != null && key.Length > 0)
            {
                var result = await CSRedisCore.DelAsync(key) > 0;
                if (!result)
                {
                    var keys = new string[key.Length];
                    for (int i = 0; i < key.Length; i++)
                    {
                        keys[i] = $"{this.PrefixCacheKey}{key}";
                    }

                    return await CSRedisCore.DelAsync(keys) > 0;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取所有key
        /// </summary>
        /// <returns></returns>
        public async Task<List<RedisKeyInfo>> GetAllRedisKeysInfoAsync()
        {
            var result = new List<RedisKeyInfo>();

            // 使用 SCAN 代替 KEYS 防止阻塞
            var keys = new List<string>();
            long cursor = 0;
            do
            {
                var scanResult = await CSRedisCore.ScanAsync(cursor, $"{this.PrefixCacheKey}*", 100);

                cursor = scanResult.Cursor;

                keys.AddRange(scanResult.Items);
            }
            while (cursor != 0);

            // 遍历每个 key，获取类型和剩余过期时间
            foreach (var key in keys)
            {
                var _key = key;

                if (_key.StartsWith(this.PrefixCacheKey))
                    _key = _key.Substring(this.PrefixCacheKey.Length);

                _key = _key.TrimStart(':');

                var type = await CSRedisCore.TypeAsync(_key); // 获取类型
                if (type == KeyType.None)
                    continue;

                var ttl = await CSRedisCore.TtlAsync(_key); // 获取剩余过期时间

                result.Add(new RedisKeyInfo
                {
                    Key = _key,
                    Type = type,
                    TTL = ttl >= 0 ? ttl : -1
                });
            }

            return result;
        }
    }
}
