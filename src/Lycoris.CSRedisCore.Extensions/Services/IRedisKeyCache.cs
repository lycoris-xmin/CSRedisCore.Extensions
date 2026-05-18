using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis Key 管理操作接口，提供键的增删改查、过期时间管理、重命名、类型查询等操作
    /// </summary>
    public interface IRedisKeyCache
    {
        /// <summary>
        /// 获取当前目录下的所有键
        /// </summary>
        /// <param name="pattern">匹配模式</param>
        /// <returns>键名数组</returns>
        string[] GetKeys(string pattern);

        /// <summary>
        /// 获取当前目录下的所有键
        /// </summary>
        /// <param name="pattern">匹配模式</param>
        /// <returns>键名数组</returns>
        Task<string[]> GetKeysAsync(string pattern);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        bool Exists(string key);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 为给定 key 设置过期时间,以秒计
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool Expire(string key, TimeSpan expire);

        /// <summary>
        /// 为给定 key 设置过期时间,以秒计
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> ExpireAsync(string key, TimeSpan expire);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        bool Persist(string key);

        /// <summary>
        /// 移除 key 的过期时间,key 将持久保持
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        Task<bool> PersistAsync(string key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余秒数</returns>
        long TTL(string key);

        /// <summary>
        /// 以秒为单位,返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余秒数</returns>
        Task<long> TTLAsync(string key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey">旧键名</param>
        /// <param name="key">新键名</param>
        /// <returns>是否修改成功</returns>
        bool Rename(string oldkey, string key);

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey">旧键名</param>
        /// <param name="key">新键名</param>
        /// <returns>是否修改成功</returns>
        Task<bool> RenameAsync(string oldkey, string key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey">旧键名</param>
        /// <param name="key">新键名</param>
        /// <returns>是否修改成功</returns>
        bool RenameIfNoExists(string oldkey, string key);

        /// <summary>
        /// 仅当 newkey 不存在时,将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey">旧键名</param>
        /// <param name="key">新键名</param>
        /// <returns>是否修改成功</returns>
        Task<bool> RenameIfNoExistsAsync(string oldkey, string key);

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key">要删除的键名数组</param>
        /// <returns>是否删除成功</returns>
        bool Remove(params string[] key);

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key">要删除的键名数组</param>
        /// <returns>是否删除成功</returns>
        Task<bool> RemoveAsync(params string[] key);

        /// <summary>
        /// 获取所有 key 的详细信息（类型、TTL等）
        /// </summary>
        /// <returns>Redis 键信息列表</returns>
        Task<List<RedisKeyInfo>> GetAllRedisKeysInfoAsync();

        /// <summary>
        /// 以毫秒返回 key 的剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余毫秒数</returns>
        long PTTL(string key);

        /// <summary>
        /// 以毫秒返回 key 的剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余毫秒数</returns>
        Task<long> PTTLAsync(string key);

        /// <summary>
        /// 以毫秒为单位设置 key 过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="milliseconds">过期毫秒数</param>
        /// <returns>是否设置成功</returns>
        bool PExpire(string key, int milliseconds);

        /// <summary>
        /// 以毫秒为单位设置 key 过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="milliseconds">过期毫秒数</param>
        /// <returns>是否设置成功</returns>
        Task<bool> PExpireAsync(string key, int milliseconds);

        /// <summary>
        /// 获取 key 的数据类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>键的数据类型</returns>
        KeyType Type(string key);

        /// <summary>
        /// 获取 key 的数据类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>键的数据类型</returns>
        Task<KeyType> TypeAsync(string key);

        /// <summary>
        /// 游标迭代所有 key
        /// </summary>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次键名</returns>
        Models.RedisScanResult<string[]> Scan(long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 游标迭代所有 key
        /// </summary>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次键名</returns>
        Task<Models.RedisScanResult<string[]>> ScanAsync(long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 随机返回一个 key
        /// </summary>
        /// <returns>随机键名</returns>
        string RandomKey();

        /// <summary>
        /// 随机返回一个 key
        /// </summary>
        /// <returns>随机键名</returns>
        Task<string> RandomKeyAsync();

        /// <summary>
        /// 以 Unix 时间戳（秒）设置 key 过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expire">过期时间点</param>
        /// <returns>是否设置成功</returns>
        bool ExpireAt(string key, DateTime expire);

        /// <summary>
        /// 以 Unix 时间戳（秒）设置 key 过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expire">过期时间点</param>
        /// <returns>是否设置成功</returns>
        Task<bool> ExpireAtAsync(string key, DateTime expire);
    }
}
