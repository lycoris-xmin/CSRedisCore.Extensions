using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Pipe
{
    /// <summary>
    /// 类型化管道上下文，提供与 <see cref="RedisCacheService"/> 一致的 API 风格进行事务操作。
    /// 所有方法排队到 <see cref="CSRedisClientPipe{T}"/>，结果在 <see cref="DistributeResults"/> 后可用。
    /// </summary>
    public class RedisCachePipe
    {
        internal readonly CSRedisClientPipe<string> Pipe;
        internal readonly JsonSerializerSettings JsonSetting;
        internal readonly string PrefixCacheKey;
        private readonly List<TaskCompletionSource<object>> _pending = new List<TaskCompletionSource<object>>();

        internal RedisCachePipe(CSRedisClientPipe<string> pipe, JsonSerializerSettings jsonSetting, string prefixCacheKey)
        {
            Pipe = pipe;
            JsonSetting = jsonSetting;
            PrefixCacheKey = prefixCacheKey;
        }

        private IRedisStringCache _String;
        /// <summary>
        /// String 类型操作
        /// </summary>
        public IRedisStringCache String
        {
            get
            {
                if (_String != null) return _String;
                _String = new RedisStringCachePipe(this);
                return _String;
            }
        }

        private IRedisHashCache _Hash;
        /// <summary>
        /// Hash 类型操作
        /// </summary>
        public IRedisHashCache Hash
        {
            get
            {
                if (_Hash != null) return _Hash;
                _Hash = new RedisHashCachePipe(this);
                return _Hash;
            }
        }

        private IRedisListCache _List;
        /// <summary>
        /// List 类型操作
        /// </summary>
        public IRedisListCache List
        {
            get
            {
                if (_List != null) return _List;
                _List = new RedisListCachePipe(this);
                return _List;
            }
        }

        private IRedisSetCache _Set;
        /// <summary>
        /// Set 类型操作
        /// </summary>
        public IRedisSetCache Set
        {
            get
            {
                if (_Set != null) return _Set;
                _Set = new RedisSetCachePipe(this);
                return _Set;
            }
        }

        private IRedisSortCache _Sort;
        /// <summary>
        /// Sorted Set 类型操作
        /// </summary>
        public IRedisSortCache Sort
        {
            get
            {
                if (_Sort != null) return _Sort;
                _Sort = new RedisSortCachePipe(this);
                return _Sort;
            }
        }

        private IRedisKeyCache _Key;
        /// <summary>
        /// 键管理操作
        /// </summary>
        public IRedisKeyCache Key
        {
            get
            {
                if (_Key != null) return _Key;
                _Key = new RedisKeyCachePipe(this);
                return _Key;
            }
        }

        /// <summary>
        /// 底层的 CSRedisClientPipe，用于高级场景
        /// </summary>
        public CSRedisClientPipe<string> CSRedisPipe => Pipe;

        #region Internal Result Management

        internal TaskCompletionSource<object> RegisterPending()
        {
            var tcs = new TaskCompletionSource<object>();
            _pending.Add(tcs);
            return tcs;
        }

        internal void DistributeResults(object[] results)
        {
            for (int i = 0; i < results.Length && i < _pending.Count; i++)
                _pending[i].TrySetResult(results[i]);

            for (int i = results.Length; i < _pending.Count; i++)
                _pending[i].TrySetException(
                    new InvalidOperationException("EndPipe returned fewer results than queued commands."));
        }

        internal void FailPending(Exception ex)
        {
            foreach (var tcs in _pending)
                tcs.TrySetException(ex);
        }

        internal Task<T> EnqueueResult<T>()
        {
            var tcs = RegisterPending();
            return tcs.Task.ContinueWith(t =>
            {
                if (t.IsFaulted) throw t.Exception.InnerException;
                var result = t.Result;
                if (result is T typed) return typed;
                if (result == null) return default;
                return (T)Convert.ChangeType(result, typeof(T));
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        internal Task EnqueueVoidResult()
        {
            var tcs = RegisterPending();
            return tcs.Task;
        }

        /// <summary>
        /// 注册一个需要自定义结果提取的待处理槽位。
        /// 返回的 Task 将在 <see cref="DistributeResults"/> 后完成，
        /// 其 Result 包含原始 object 值。
        /// </summary>
        internal Task<object> RegisterPendingWithTask()
        {
            var tcs = new TaskCompletionSource<object>();
            _pending.Add(tcs);
            return tcs.Task;
        }

        #endregion
    }
}
