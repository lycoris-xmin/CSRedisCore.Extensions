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

        long IRedisSetCache.Set(string key, params string[] value) => throw NewNotSupportedException();

        long IRedisSetCache.Set<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();

        long IRedisSetCache.Count(string key) => throw NewNotSupportedException();

        bool IRedisSetCache.Exists(string key, string value) => throw NewNotSupportedException();

        string IRedisSetCache.GetRandom(string key) => throw NewNotSupportedException();

        T IRedisSetCache.GetRandom<T>(string key) where T : class => throw NewNotSupportedException();

        List<string> IRedisSetCache.GetAll(string setKey) => throw NewNotSupportedException();

        long IRedisSetCache.Remove(string key, params string[] value) => throw NewNotSupportedException();

        long IRedisSetCache.Remove<T>(string key, params T[] value) where T : class => throw NewNotSupportedException();

        string IRedisSetCache.RandomMember(string key) => throw NewNotSupportedException();

        T IRedisSetCache.RandomMember<T>(string key) where T : class => throw NewNotSupportedException();

        string[] IRedisSetCache.RandomMembers(string key, int count) => throw NewNotSupportedException();

        bool IRedisSetCache.Move(string sourceKey, string targetKey, string value) => throw NewNotSupportedException();

        string[] IRedisSetCache.Difference(params string[] keys) => throw NewNotSupportedException();

        long IRedisSetCache.DifferenceStore(string targetKey, params string[] keys) => throw NewNotSupportedException();

        string[] IRedisSetCache.Intersection(params string[] keys) => throw NewNotSupportedException();

        long IRedisSetCache.IntersectionStore(string targetKey, params string[] keys) => throw NewNotSupportedException();

        string[] IRedisSetCache.Union(params string[] keys) => throw NewNotSupportedException();

        long IRedisSetCache.UnionStore(string targetKey, params string[] keys) => throw NewNotSupportedException();

        Models.RedisScanResult<string[]> IRedisSetCache.Scan(string key, long cursor, string pattern, long? count) => throw NewNotSupportedException();

        #endregion

        #region async

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要添加的成员</param>
        /// <returns>被添加到集合中的新成员的数量</returns>
        public Task<long> SetAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.SAdd(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要添加的成员</param>
        /// <returns>被添加到集合中的新成员的数量</returns>
        public Task<long> SetAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.SAdd(key, list);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员数量</returns>
        public Task<long> CountAsync(string key)
        {
            _cachePipe.Pipe.SCard(key);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要判断的值</param>
        /// <returns>是否存在</returns>
        public Task<bool> ExistsAsync(string key, string value)
        {
            _cachePipe.Pipe.SIsMember(key, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>被移除的随机元素</returns>
        public Task<string> GetRandomAsync(string key)
        {
            _cachePipe.Pipe.SPop(key);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>被移除的随机元素</returns>
        public async Task<T> GetRandomAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.SPop(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 获取集合的所有元素
        /// </summary>
        /// <param name="setKey">缓存键</param>
        /// <returns>集合的所有元素</returns>
        public Task<List<string>> GetAllAsync(string setKey)
        {
            _cachePipe.Pipe.SMembers(setKey);
            return _cachePipe.EnqueueResult<List<string>>();
        }

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员</param>
        /// <returns>被移除的成员数量</returns>
        public Task<long> RemoveAsync(string key, params string[] value)
        {
            _cachePipe.Pipe.SRem(key, value);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 移除集合中一个或多个成员（泛型）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员</param>
        /// <returns>被移除的成员数量</returns>
        public Task<long> RemoveAsync<T>(string key, params T[] value) where T : class
        {
            var list = value.Select(v => JsonConvert.SerializeObject(v, _cachePipe.JsonSetting)).ToArray();
            _cachePipe.Pipe.SRem(key, list);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 随机获取成员（不移除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机的成员值</returns>
        public Task<string> RandomMemberAsync(string key)
        {
            _cachePipe.Pipe.SRandMember(key);
            return _cachePipe.EnqueueResult<string>();
        }

        /// <summary>
        /// 随机获取成员（不移除）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>随机的成员值</returns>
        public async Task<T> RandomMemberAsync<T>(string key) where T : class
        {
            _cachePipe.Pipe.SRandMember(key);
            var tcs = _cachePipe.RegisterPending();
            var val = await tcs.Task;
            return string.IsNullOrEmpty(val?.ToString()) ? default : JsonConvert.DeserializeObject<T>(val.ToString(), _cachePipe.JsonSetting);
        }

        /// <summary>
        /// 随机获取多个成员（不移除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">获取数量</param>
        /// <returns>随机的成员数组</returns>
        public Task<string[]> RandomMembersAsync(string key, int count)
        {
            _cachePipe.Pipe.SRandMembers(key, (int)count);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 将成员从源集合移动到目标集合
        /// </summary>
        /// <param name="sourceKey">源缓存键</param>
        /// <param name="targetKey">目标缓存键</param>
        /// <param name="value">要移动的成员</param>
        /// <returns>是否移动成功</returns>
        public Task<bool> MoveAsync(string sourceKey, string targetKey, string value)
        {
            _cachePipe.Pipe.SMove(sourceKey, targetKey, value);
            return _cachePipe.EnqueueResult<bool>();
        }

        /// <summary>
        /// 多个集合的差集
        /// </summary>
        /// <param name="keys">集合的键列表</param>
        /// <returns>差集结果</returns>
        public Task<string[]> DifferenceAsync(params string[] keys)
        {
            _cachePipe.Pipe.SDiff(keys);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 差集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标存储键</param>
        /// <param name="keys">集合的键列表</param>
        /// <returns>结果集合的成员数</returns>
        public Task<long> DifferenceStoreAsync(string targetKey, params string[] keys)
        {
            _cachePipe.Pipe.SDiffStore(targetKey, keys);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 多个集合的交集
        /// </summary>
        /// <param name="keys">集合的键列表</param>
        /// <returns>交集结果</returns>
        public Task<string[]> IntersectionAsync(params string[] keys)
        {
            _cachePipe.Pipe.SInter(keys);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 交集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标存储键</param>
        /// <param name="keys">集合的键列表</param>
        /// <returns>结果集合的成员数</returns>
        public Task<long> IntersectionStoreAsync(string targetKey, params string[] keys)
        {
            _cachePipe.Pipe.SInterStore(targetKey, keys);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 多个集合的并集
        /// </summary>
        /// <param name="keys">集合的键列表</param>
        /// <returns>并集结果</returns>
        public Task<string[]> UnionAsync(params string[] keys)
        {
            _cachePipe.Pipe.SUnion(keys);
            return _cachePipe.EnqueueResult<string[]>();
        }

        /// <summary>
        /// 并集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标存储键</param>
        /// <param name="keys">集合的键列表</param>
        /// <returns>结果集合的成员数</returns>
        public Task<long> UnionStoreAsync(string targetKey, params string[] keys)
        {
            _cachePipe.Pipe.SUnionStore(targetKey, keys);
            return _cachePipe.EnqueueResult<long>();
        }

        /// <summary>
        /// 同步方法，管道模式不支持，请使用异步重载
        /// </summary>
        public Task<Models.RedisScanResult<string[]>> ScanAsync(string key, long cursor, string pattern = null, long? count = null)
            => throw new NotSupportedException("Pipe ScanAsync is not supported for set scans. Use PipeExecute/raw pipe access for scan operations.");

        #endregion

        private static NotSupportedException NewNotSupportedException()
            => new NotSupportedException("Synchronous methods are not supported in pipe context. Use the async overload.");
    }
}
