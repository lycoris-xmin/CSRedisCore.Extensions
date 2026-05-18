namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 统计信息
    /// </summary>
    public class RedisStatsInfoModel
    {
        /// <summary>
        /// 接收到的总连接数
        /// </summary>
        public long TotalConnectionsReceived { get; set; }

        /// <summary>
        /// 已处理的总命令数
        /// </summary>
        public long TotalCommandsProcessed { get; set; }

        /// <summary>
        /// 键命中次数
        /// </summary>
        public long KeyspaceHits { get; set; }

        /// <summary>
        /// 键未命中次数
        /// </summary>
        public long KeyspaceMisses { get; set; }

        /// <summary>
        /// 被拒绝的连接总数（达到 maxclients 限制时）
        /// </summary>
        public long RejectedConnections { get; set; }

        /// <summary>
        /// 被驱逐的键总数（触发内存淘汰策略时）
        /// </summary>
        public long EvictedKeys { get; set; }

        /// <summary>
        /// 过期的键总数（因设置了过期时间）
        /// </summary>
        public long ExpiredKeys { get; set; }

        /// <summary>
        /// 当前每秒处理命令数（QPS）
        /// </summary>
        public long InstantaneousOpsPerSec { get; set; }

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public string LoadDescription
        {
            get
            {
                var total = KeyspaceHits + KeyspaceMisses;
                if (total == 0) return "未知";
                var hitRate = (double)KeyspaceHits / total;
                if (hitRate < 0.85)
                    return "低";
                else if (hitRate < 0.95)
                    return "正常";
                else
                    return "高";
            }
        }
    }
}
