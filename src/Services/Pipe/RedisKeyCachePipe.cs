using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 管道 Key 操作包装器，实现 <see cref="IRedisKeyCache"/>。
    /// </summary>
    public class RedisKeyCachePipe : IRedisKeyCache
    {
        private readonly RedisCachePipe _cachePipe;

        internal RedisKeyCachePipe(RedisCachePipe cachePipe)
        {
            _cachePipe = cachePipe;
        }

        #region synchronous — Not Supported

        string[] IRedisKeyCache.GetKeys(string pattern) => throw NewNotSupportedException();

        bool IRedisKeyCache.Exists(string key) => throw NewNotSupportedException();

        bool IRedisKeyCache.Expire(string key, TimeSpan expire) => throw NewNotSupportedException();

        bool IRedisKeyCache.Persist(string key) => throw NewNotSupportedException();

        long IRedisKeyCache.TTL(string key) => throw NewNotSupportedException();

        bool IRedisKeyCache.Rename(string oldkey, string key) => throw NewNotSupportedException();

        bool IRedisKeyCache.RenameIfNoExists(string oldkey, string key) => throw NewNotSupportedException();

        bool IRedisKeyCache.Remove(params string[] key) => throw NewNotSupportedException();

        long IRedisKeyCache.PTTL(string key) => throw NewNotSupportedException();

        bool IRedisKeyCache.PExpire(string key, int milliseconds) => throw NewNotSupportedException();

        KeyType IRedisKeyCache.Type(string key) => throw NewNotSupportedException();

        Models.RedisScanResult<string[]> IRedisKeyCache.Scan(long cursor, string pattern, long? count) => throw NewNotSupportedException();

        string IRedisKeyCache.RandomKey() => throw NewNotSupportedException();

        bool IRedisKeyCache.ExpireAt(string key, DateTime expire) => throw NewNotSupportedException();

        #endregion

        #region async

        /// <summary>
        /// 获取当前目录下的所有键
        /// </summary>
        /// <param name="pattern">匹配模式</param>
        /// <returns>匹配的键列表</returns>
        public async Task<string[]> GetKeysAsync(string pattern)
        {
            var patternWithPrefix = $"{_cachePipe.PrefixCacheKey}{(pattern.EndsWith("*") ? pattern : $"{pattern}*")}";
            _cachePipe.Pipe.Keys(patternWithPrefix);
            var tcs = _cachePipe.RegisterPending();
            var rawResult = await tcs.Task;
            var keys = rawResult as string[] ?? Array.Empty<string>();
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].StartsWith(_cachePipe.PrefixCacheKey))
                    keys[i] = keys[i].Substring(_cachePipe.PrefixCacheKey.Length);
                keys[i] = keys[i].TrimStart(':');
            }
            return keys;
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        public Task<bool> ExistsAsync(string key)
        {
            _cachePipe.Pipe.Exists(key);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 为给定 key 设置过期时间，以秒计
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            _cachePipe.Pipe.Expire(key, expire);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 移除 key 的过期时间，key 将持久保持
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        public Task<bool> PersistAsync(string key)
        {
            _cachePipe.Pipe.Persist(key);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 以秒为单位，返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余秒数</returns>
        public Task<long> TTLAsync(string key)
        {
            _cachePipe.Pipe.Ttl(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="oldkey">旧键名</param>
        /// <param name="key">新键名</param>
        /// <returns>是否重命名成功</returns>
        public Task<bool> RenameAsync(string oldkey, string key)
        {
            _cachePipe.Pipe.Rename(oldkey, key);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 仅当 newkey 不存在时，将 key 改名为 newkey
        /// </summary>
        /// <param name="oldkey">旧键名</param>
        /// <param name="key">新键名</param>
        /// <returns>是否重命名成功</returns>
        public Task<bool> RenameIfNoExistsAsync(string oldkey, string key)
        {
            _cachePipe.Pipe.RenameNx(oldkey, key);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 删除指定 Key
        /// </summary>
        /// <param name="key">要删除的键</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> RemoveAsync(params string[] key)
        {
            foreach (var item in key)
            {
                _cachePipe.Pipe.Del(item);
            }
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 获取所有 key 详细信息（需要迭代 SCAN+TYPE+TTL，不支持管道上下文）。
        /// </summary>
        public Task<List<RedisKeyInfo>> GetAllRedisKeysInfoAsync()
            => throw new NotSupportedException("GetAllRedisKeysInfoAsync requires per-key iteration and cannot be used in a pipe context. Use the non-pipe version instead.");

        /// <summary>
        /// 以毫秒为单位返回 key 的剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余毫秒数</returns>
        public Task<long> PTTLAsync(string key)
        {
            _cachePipe.Pipe.PTtl(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 以毫秒为单位设置 key 过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="milliseconds">过期毫秒数</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> PExpireAsync(string key, int milliseconds)
        {
            _cachePipe.Pipe.PExpire(key, milliseconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 获取 key 的数据类型
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>数据类型</returns>
        public Task<KeyType> TypeAsync(string key)
        {
            _cachePipe.Pipe.Type(key);
            return _cachePipe.EnqueueResult<KeyType>();
        }

        /// <summary>
        /// 同步方法，管道模式不支持，请使用异步重载
        /// </summary>
        public Task<Models.RedisScanResult<string[]>> ScanAsync(long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for key scans. Use PipeExecute/raw pipe access for scan operations.");

        /// <summary>
        /// 随机返回一个 key
        /// </summary>
        /// <returns>随机的键名</returns>
        public Task<string> RandomKeyAsync()
        {
            _cachePipe.Pipe.RandomKey();
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 以 Unix 时间戳（秒）设置 key 过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expire">过期时间点</param>
        /// <returns>是否设置成功</returns>
        public Task<bool> ExpireAtAsync(string key, DateTime expire)
        {
            _cachePipe.Pipe.ExpireAt(key, expire);
            return _cachePipe.EnqueueResult<bool>();
        }

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
