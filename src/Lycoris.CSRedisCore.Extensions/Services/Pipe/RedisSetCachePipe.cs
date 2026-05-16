using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 管道 Set 操作包装器，实现 <see cref="IRedisSetCache"/>。
    /// </summary>
    public class RedisSetCachePipe : IRedisSetCache
    {
        private readonly RedisCachePipe _cachePipe;

        internal RedisSetCachePipe(RedisCachePipe cachePipe)
        {
            _cachePipe = cachePipe;
        }

        #region synchronous — Not Supported

        public long Set(string key, params string[] value) => throw NewNotSupportedException();
        public long Set<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();
        public long Count(string key) => throw NewNotSupportedException();
        public bool Exists(string key, string value) => throw NewNotSupportedException();
        public string GetRandom(string key) => throw NewNotSupportedException();
        public T GetRandom<T>(string key) where T : class => throw NewNotSupportedException();
        public List<string> GetAll(string setKey) => throw NewNotSupportedException();
        public long Remove(string key, params string[] value) => throw NewNotSupportedException();
        public long Remove<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();
        public string RandomMember(string key) => throw NewNotSupportedException();
        public T RandomMember<T>(string key) where T : class => throw NewNotSupportedException();
        public string[] RandomMembers(string key, int count) => throw NewNotSupportedException();
        public bool Move(string sourceKey, string targetKey, string value) => throw NewNotSupportedException();
        public string[] Difference(params string[] keys) => throw NewNotSupportedException();
        public long DifferenceStore(string targetKey, params string[] keys) => throw NewNotSupportedException();
        public string[] Intersection(params string[] keys) => throw NewNotSupportedException();
        public long IntersectionStore(string targetKey, params string[] keys) => throw NewNotSupportedException();
        public string[] Union(params string[] keys) => throw NewNotSupportedException();
        public long UnionStore(string targetKey, params string[] keys) => throw NewNotSupportedException();
        public Models.RedisScanResult<string[]> Scan(string key, long cursor, string pattern = null, long? count = null) => throw NewNotSupportedException();

        #endregion

        #region async

        public Task<long> SetAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.SAdd(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> SetAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.SAdd(key, list);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> CountAsync(string key)
        {
            _cachePipe.Pipe.SCard(key);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<bool> ExistsAsync(string key, string value)
        {
            _cachePipe.Pipe.SIsMember(key, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<string> GetRandomAsync(string key)
        {
            _cachePipe.Pipe.SPop(key);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> GetRandomAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.SPop(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        public Task<List<string>> GetAllAsync(string setKey)
        {
            _cachePipe.Pipe.SMembers(setKey);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        public Task<long> RemoveAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.SRem(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<long> RemoveAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.SRem(key, list);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<string> RandomMemberAsync(string key)
        {
            _cachePipe.Pipe.SRandMember(key);
            return _cachePipe.EnqueueResult<string>();
        }

        public async Task<T> RandomMemberAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.SRandMember(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        public Task<string[]> RandomMembersAsync(string key, int count)
        {
            _cachePipe.Pipe.SRandMembers(key, (int)count);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public Task<bool> MoveAsync(string sourceKey, string targetKey, string value)
        {
            _cachePipe.Pipe.SMove(sourceKey, targetKey, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        public Task<string[]> DifferenceAsync(params string[] keys)
        {
            _cachePipe.Pipe.SDiff(keys);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public Task<long> DifferenceStoreAsync(string targetKey, params string[] keys)
        {
            _cachePipe.Pipe.SDiffStore(targetKey, keys);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<string[]> IntersectionAsync(params string[] keys)
        {
            _cachePipe.Pipe.SInter(keys);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public Task<long> IntersectionStoreAsync(string targetKey, params string[] keys)
        {
            _cachePipe.Pipe.SInterStore(targetKey, keys);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<string[]> UnionAsync(params string[] keys)
        {
            _cachePipe.Pipe.SUnion(keys);
            return _cachePipe.EnqueueResult<string[]>();
        }

        public Task<long> UnionStoreAsync(string targetKey, params string[] keys)
        {
            _cachePipe.Pipe.SUnionStore(targetKey, keys);
            return _cachePipe.EnqueueResult<long>();
        }

        public Task<Models.RedisScanResult<string[]>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for set scans. Use PipeExecute/raw pipe access for scan operations.");

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
