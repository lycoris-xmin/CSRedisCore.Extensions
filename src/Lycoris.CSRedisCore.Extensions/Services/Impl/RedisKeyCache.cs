using CSRedis;
using System;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisKeyCache : IRedisKeyCache
    {
        private readonly CSRedisClient CSRedisCore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CSRedisCore"></param>
        public RedisKeyCache(CSRedisClient CSRedisCore)
        {
            this.CSRedisCore = CSRedisCore;
        }

        /// <summary>
        /// 模糊查找所有键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string[] GetKeys(string pattern) => CSRedisCore.Keys(pattern.EndsWith("*") ? pattern : $"{pattern}*");

        /// <summary>
        /// 模糊查找所有键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<string[]> GetKeysAsync(string pattern) => await CSRedisCore.KeysAsync(pattern.EndsWith("*") ? pattern : $"{pattern}*");

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
        public async Task<bool> ExpireAsync(string key, TimeSpan expire) => !string.IsNullOrEmpty(key) && await CSRedisCore.ExpireAsync(key, expire);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Persist(string key) => !string.IsNullOrEmpty(key) && CSRedisCore.Persist(key);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> PersistAsync(string key) => !string.IsNullOrEmpty(key) && await CSRedisCore.PersistAsync(key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long TTL(string key) => string.IsNullOrEmpty(key) ? 0 : CSRedisCore.Ttl(key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> TTLAsync(string key) => string.IsNullOrEmpty(key) ? 0 : await CSRedisCore.TtlAsync(key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Rename(string oldkey, string key) => !string.IsNullOrEmpty(key) && CSRedisCore.Rename(oldkey, key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RenameAsync(string oldkey, string key) => !string.IsNullOrEmpty(key) && await CSRedisCore.RenameAsync(oldkey, key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RenameIfNoExists(string oldkey, string key) => !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(key) && CSRedisCore.RenameNx(oldkey, key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RenameIfNoExistsAsync(string oldkey, string key) => !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(key) && await CSRedisCore.RenameNxAsync(oldkey, key);

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(params string[] key) => key != null && key.Length > 0 && CSRedisCore.Del(key) > 0;

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(params string[] key) => key != null && key.Length > 0 && await CSRedisCore.DelAsync(key) > 0;
    }
}
