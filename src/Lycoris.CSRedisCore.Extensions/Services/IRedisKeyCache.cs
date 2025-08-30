using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisKeyCache
    {
        /// <summary>
        /// 获取当前目录下的所有键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        string[] GetKeys(string pattern);

        /// <summary>
        /// 获取当前目录下的所有键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        Task<string[]> GetKeysAsync(string pattern);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 为给定 key 设置过期时间,以秒计
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool Expire(string key, TimeSpan expire);

        /// <summary>
        /// 为给定 key 设置过期时间,以秒计
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, TimeSpan expire);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Persist(string key);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> PersistAsync(string key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long TTL(string key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> TTLAsync(string key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Rename(string oldkey, string key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> RenameAsync(string oldkey, string key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RenameIfNoExists(string oldkey, string key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> RenameIfNoExistsAsync(string oldkey, string key);

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key"></param>
        bool Remove(params string[] key);

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(params string[] key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<RedisKeyInfo>> GetAllRedisKeysInfoAsync();
    }
}
