using CSRedis;

namespace Lycoris.CSRedisCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisKeyInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public KeyType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? TTL { get; set; }
    }
}
