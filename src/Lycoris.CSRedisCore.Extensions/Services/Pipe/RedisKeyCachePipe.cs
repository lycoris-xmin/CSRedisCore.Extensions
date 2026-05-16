using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string[] GetKeys(string pattern) => throw NewNotSupportedException();
        public bool Exists(string key) => throw NewNotSupportedException();
        public bool Expire(string key, TimeSpan expire) => throw NewNotSupportedException();
        public bool Persist(string key) => throw NewNotSupportedException();
        public long TTL(string key) => throw NewNotSupportedException();
        public bool Rename(string oldkey, string key) => throw NewNotSupportedException();
        public bool RenameIfNoExists(string oldkey, string key) => throw NewNotSupportedException();
        public bool Remove(params string[] key) => throw NewNotSupportedException();
        public long PTTL(string key) => throw NewNotSupportedException();
        public bool PExpire(string key, int milliseconds) => throw NewNotSupportedException();
        public KeyType Type(string key) => throw NewNotSupportedException();
        public Models.RedisScanResult<string[]> Scan(long cursor, string pattern = null, long? count = null) => throw NewNotSupportedException();
        public string RandomKey() => throw NewNotSupportedException();
        public bool ExpireAt(string key, DateTime expire) => throw NewNotSupportedException();

        #endregion

        #region async

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

        public Task<bool> ExistsAsync(string key)
        {
            _cachePipe.Pipe.Exists(key);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            _cachePipe.Pipe.Expire(key, expire);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> PersistAsync(string key)
        {
            _cachePipe.Pipe.Persist(key);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<long> TTLAsync(string key)
        {
            _cachePipe.Pipe.Ttl(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> RenameAsync(string oldkey, string key)
        {
            _cachePipe.Pipe.Rename(oldkey, key);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<bool> RenameIfNoExistsAsync(string oldkey, string key)
        {
            _cachePipe.Pipe.RenameNx(oldkey, key);
            return _cachePipe.EnqueueResult<bool>();
        }

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

        public Task<long> PTTLAsync(string key)
        {
            _cachePipe.Pipe.PTtl(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> PExpireAsync(string key, int milliseconds)
        {
            _cachePipe.Pipe.PExpire(key, milliseconds);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<KeyType> TypeAsync(string key)
        {
            _cachePipe.Pipe.Type(key);
            return _cachePipe.EnqueueResult<KeyType>();
        }

        public Task<Models.RedisScanResult<string[]>> ScanAsync(long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for key scans. Use PipeExecute/raw pipe access for scan operations.");

        public Task<string> RandomKeyAsync()
        {
            _cachePipe.Pipe.RandomKey();
            return _cachePipe.EnqueueResult<string>();
        }

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
