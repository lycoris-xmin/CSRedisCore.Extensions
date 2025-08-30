namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisServerInfoModel
    {
        /// <summary>
        /// Redis 服务器版本号
        /// </summary>
        public string RedisVersion { get; set; }

        /// <summary>
        /// 运行 Redis 的操作系统信息
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// Redis 服务器运行时间（秒）
        /// </summary>
        public long UptimeInSeconds { get; set; }

        /// <summary>
        /// Redis 服务器运行时间（天）
        /// </summary>
        public long UptimeInDays { get; set; }
    }
}
