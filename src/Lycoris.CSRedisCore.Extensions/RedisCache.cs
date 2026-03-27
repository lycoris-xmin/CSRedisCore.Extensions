using CSRedis;
using Lycoris.CSRedisCore.Extensions.Services;
using Lycoris.CSRedisCore.Extensions.Services.Impl;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class RedisCache
    {
        /// <summary>
        /// redis 客户端
        /// </summary>
        internal static CSRedisClient Client;

        /// <summary>
        /// 
        /// </summary>
        private static IRedisKeyCache _Key = null;
        /// <summary>
        /// 键操作
        /// </summary>
        public static IRedisKeyCache Key
        {
            get
            {
                if (_Key != null)
                    return _Key;

                _Key = new RedisKeyCache(Client, RedisStore.PrefixCacheKey);

                return _Key;
            }
        }


        private static IRedisStringCache _String = null;
        /// <summary>
        /// String类型
        /// </summary>
        public static IRedisStringCache String
        {
            get
            {
                if (_String != null)
                    return _String;

                _String = new RedisStringCache(Client, RedisStore.Settings, RedisStore.PrefixCacheKey);
                return _String;
            }
        }


        private static IRedisHashCache _Hash = null;
        /// <summary>
        /// Hash类型
        /// </summary>
        public static IRedisHashCache Hash
        {
            get
            {
                if (_Hash != null)
                    return _Hash;

                _Hash = new RedisHashCache(Client, RedisStore.Settings, RedisStore.PrefixCacheKey);
                return _Hash;
            }
        }


        private static IRedisListCache _List = null;
        /// <summary>
        /// List类型
        /// </summary>
        public static IRedisListCache List
        {
            get
            {
                if (_List != null)
                    return _List;

                _List = new RedisListCache(Client, RedisStore.Settings, RedisStore.PrefixCacheKey);
                return _List;
            }
        }


        private static IRedisSetCache _Set = null;
        /// <summary>
        /// Set类型
        /// </summary>
        public static IRedisSetCache Set
        {
            get
            {
                if (_Set != null)
                    return _Set;

                _Set = new RedisSetCache(Client, RedisStore.Settings, RedisStore.PrefixCacheKey);
                return _Set;
            }
        }


        private static IRedisSortCache _Sort = null;
        /// <summary>
        /// Sort类型
        /// </summary>
        public static IRedisSortCache Sort
        {
            get
            {
                if (_Sort != null)
                    return _Sort;

                _Sort = new RedisSortCache(Client, RedisStore.Settings, RedisStore.PrefixCacheKey);
                return _Sort;
            }
        }


        private static IRedisEventMessage _Message;
        /// <summary>
        /// 发布/订阅
        /// </summary>
        public static IRedisEventMessage Message
        {
            get
            {
                if (_Message != null)
                    return _Message;

                _Message = new RedisEventMessage(Client, RedisStore.Settings);
                return _Message;
            }
        }


        private static IRedisCacheUtils _Utils = null;
        /// <summary>
        /// 封装工具
        /// </summary>
        public static IRedisCacheUtils Utils
        {
            get
            {
                if (_Utils != null)
                    return _Utils;

                _Utils = new RedisCacheUtils(Client, RedisStore.Settings, RedisStore.PrefixCacheKey);
                return _Utils;
            }
        }


        private static IMonitorService _Monitor = null;
        /// <summary>
        /// 
        /// </summary>
        public static IMonitorService Monitor
        {
            get
            {
                if (_Monitor != null)
                    return _Monitor;

                _Monitor = new MonitorService(Client, RedisStore.Settings);
                return _Monitor;
            }
        }

        /// <summary>
        /// CSRedis 原本的Client
        /// </summary>
        public static CSRedisClient CSRedisClient => Client;
    }
}
