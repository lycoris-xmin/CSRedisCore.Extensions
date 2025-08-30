namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisKeyspaceInfoModel
    {
        /// <summary>
        /// 当前数据库中的键数量
        /// </summary>
        public int Keys { get; set; }

        /// <summary>
        /// 设置了过期时间的键数量
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 平均存活时间（毫秒）
        /// </summary>
        public long AvgTtl { get; set; }
    }
}
