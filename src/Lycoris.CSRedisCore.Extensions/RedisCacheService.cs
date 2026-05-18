using CSRedis;
using Lycoris.CSRedisCore.Extensions.Services;
using Lycoris.CSRedisCore.Extensions.Services.Impl;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// Redis 缓存服务实例，封装 String/Hash/List/Set/Sort/Key/Message/Utils/Monitor 等操作
    /// </summary>
    public sealed class RedisCacheService
    {
        /// <summary>
        /// redis实例
        /// </summary>
        private readonly CSRedisClient Command;

        /// <summary>
        /// Json序列化配置
        /// </summary>
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        private string PrefixCacheKey = "";

        /// <summary>
        /// 初始化 Redis 缓存服务实例
        /// </summary>
        /// <param name="Command">Redis 客户端</param>
        /// <param name="JsonSerializerSetting">JSON 序列化设置</param>
        /// <param name="prefixCackeKey">缓存键前缀</param>
        public RedisCacheService(CSRedisClient Command, JsonSerializerSettings JsonSerializerSetting, string prefixCackeKey)
        {
            this.Command = Command;
            JsonSetting = JsonSerializerSetting ?? RedisStore.Settings;
            PrefixCacheKey = $"{prefixCackeKey.TrimEnd(':')}:";
        }


        private IRedisKeyCache _Key = null;
        /// <summary>
        /// 键操作
        /// </summary>
        public IRedisKeyCache Key
        {
            get
            {
                if (_Key != null)
                    return _Key;

                _Key = new RedisKeyCache(Command, RedisStore.PrefixCacheKey);
                return _Key;
            }
        }


        private IRedisStringCache _String = null;
        /// <summary>
        /// String类型
        /// </summary>
        public IRedisStringCache String
        {
            get
            {
                if (_String != null)
                    return _String;

                _String = new RedisStringCache(Command, JsonSetting, PrefixCacheKey);
                return _String;
            }
        }


        private IRedisHashCache _Hash = null;
        /// <summary>
        /// Hash类型
        /// </summary>
        public IRedisHashCache Hash
        {
            get
            {
                if (_Hash != null)
                    return _Hash;

                _Hash = new RedisHashCache(Command, JsonSetting, PrefixCacheKey);
                return _Hash;
            }
        }


        private IRedisListCache _List = null;
        /// <summary>
        /// List类型
        /// </summary>
        public IRedisListCache List
        {
            get
            {
                if (_List != null)
                    return _List;

                _List = new RedisListCache(Command, JsonSetting, PrefixCacheKey);
                return _List;
            }
        }


        private IRedisSetCache _Set = null;
        /// <summary>
        /// Set类型
        /// </summary>
        public IRedisSetCache Set
        {
            get
            {
                if (_Set != null)
                    return _Set;

                _Set = new RedisSetCache(Command, JsonSetting, PrefixCacheKey);
                return _Set;
            }
        }


        private IRedisSortCache _Sort = null;
        /// <summary>
        /// Sort类型
        /// </summary>
        public IRedisSortCache Sort
        {
            get
            {
                if (_Sort != null)
                    return _Sort;

                _Sort = new RedisSortCache(Command, JsonSetting, PrefixCacheKey);
                return _Sort;
            }
        }


        private IRedisEventMessage _Message;
        /// <summary>
        /// 发布/订阅
        /// </summary>
        public IRedisEventMessage Message
        {
            get
            {
                if (_Message != null)
                    return _Message;

                _Message = new RedisEventMessage(Command, JsonSetting);
                return _Message;
            }
        }


        private IRedisCacheUtils _Utils = null;
        /// <summary>
        /// 封装工具
        /// </summary>
        public IRedisCacheUtils Utils
        {
            get
            {
                if (_Utils != null)
                    return _Utils;

                _Utils = new RedisCacheUtils(Command, JsonSetting, PrefixCacheKey);
                return _Utils;
            }
        }

        private IMonitorService _Monitor = null;
        /// <summary>
        /// 监控服务
        /// </summary>
        public IMonitorService Monitor
        {
            get
            {
                if (_Monitor != null)
                    return _Monitor;

                _Monitor = new MonitorService(Command, JsonSetting);
                return _Monitor;
            }
        }

        /// <summary>
        /// 缓存穿透保护，在缓存不存在时执行指定函数获取数据并自动缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="span">超时时间</param>
        /// <param name="func">获取数据的异步函数</param>
        /// <returns>缓存值或函数返回的数据</returns>
        public async Task<string> CacheShell(string key, TimeSpan span, Func<Task<string>> func) => await Command.CacheShellAsync(key, (int)span.TotalSeconds, func);

        /// <summary>
        /// CSRedis 原本的Client
        /// </summary>
        public CSRedisClient CSRedisClient => this.Command;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (Command != null && !Command.Nodes.IsEmpty)
                    Command.Dispose();
            }
            catch
            {
            }
        }
    }
}
