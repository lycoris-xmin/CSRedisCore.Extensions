using CSRedis;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// Redis 键信息
    /// </summary>
    public class RedisKeyInfo
    {
        /// <summary>
        /// 键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 键类型
        /// </summary>
        public KeyType Type { get; set; }

        /// <summary>
        /// 剩余生存时间（毫秒），null 表示永不过期
        /// </summary>
        public long? TTL { get; set; }
    }
}
