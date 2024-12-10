using CSRedis;
using Lycoris.CSRedisCore.Extensions.Services;
using Lycoris.CSRedisCore.Extensions.Services.Impl;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        private string PrefixCacheKey = "";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="JsonSerializerSetting"></param>
        public RedisCacheService(CSRedisClient Command, JsonSerializerSettings JsonSerializerSetting, string prefixCackeKey)
        {
            this.Command = Command;
            JsonSetting = JsonSerializerSetting ?? new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss.ffffff",
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                NullValueHandling = NullValueHandling.Ignore,
                MaxDepth = 200
            };
            PrefixCacheKey = $"{prefixCackeKey}:";
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

                _Key = new RedisKeyCache(Command, this.PrefixCacheKey);
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

                _String = new RedisStringCache(Command, JsonSetting);
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

                _Hash = new RedisHashCache(Command, JsonSetting);
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

                _List = new RedisListCache(Command, JsonSetting);
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

                _Set = new RedisSetCache(Command, JsonSetting);
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

                _Sort = new RedisSortCache(Command, JsonSetting);
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


        private IRedisTransaction _Transaction = null;
        /// <summary>
        /// 事务管道
        /// </summary>
        public IRedisTransaction Transaction
        {
            get
            {
                if (_Transaction != null)
                    return _Transaction;

                _Transaction = new RedisTransaction(Command, JsonSetting);
                return _Transaction;
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

                _Utils = new RedisCacheUtils(Command, JsonSetting);
                return _Utils;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span">超时时间</param>
        /// <param name="func"></param>
        /// <returns></returns>
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
