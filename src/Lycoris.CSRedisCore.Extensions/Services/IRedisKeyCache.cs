using CSRedis;
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
        /// 获取所有 key 的详细信息（类型、TTL等）
        /// </summary>
        Task<List<RedisKeyInfo>> GetAllRedisKeysInfoAsync();

        /// <summary>
        /// 以毫秒返回 key 的剩余生存时间
        /// </summary>
        long PTTL(string key);

        /// <summary>
        /// 以毫秒返回 key 的剩余生存时间
        /// </summary>
        Task<long> PTTLAsync(string key);

        /// <summary>
        /// 以毫秒为单位设置 key 过期时间
        /// </summary>
        bool PExpire(string key, int milliseconds);

        /// <summary>
        /// 以毫秒为单位设置 key 过期时间
        /// </summary>
        Task<bool> PExpireAsync(string key, int milliseconds);

        /// <summary>
        /// 获取 key 的数据类型
        /// </summary>
        KeyType Type(string key);

        /// <summary>
        /// 获取 key 的数据类型
        /// </summary>
        Task<KeyType> TypeAsync(string key);

        /// <summary>
        /// 游标迭代所有 key
        /// </summary>
        Models.RedisScanResult<string[]> Scan(long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 游标迭代所有 key
        /// </summary>
        Task<Models.RedisScanResult<string[]>> ScanAsync(long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 随机返回一个 key
        /// </summary>
        string RandomKey();

        /// <summary>
        /// 随机返回一个 key
        /// </summary>
        Task<string> RandomKeyAsync();

        /// <summary>
        /// 以 Unix 时间戳（秒）设置 key 过期时间
        /// </summary>
        bool ExpireAt(string key, DateTime expire);

        /// <summary>
        /// 以 Unix 时间戳（秒）设置 key 过期时间
        /// </summary>
        Task<bool> ExpireAtAsync(string key, DateTime expire);
    }
}
