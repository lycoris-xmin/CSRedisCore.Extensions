namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisInfoModel
    {
        /// <summary>
        /// 服务器相关信息
        /// </summary>
        public RedisServerInfoModel Server { get; set; }

        /// <summary>
        /// 客户端相关信息
        /// </summary>
        public RedisClientsInfoModel Clients { get; set; }

        /// <summary>
        /// 内存使用相关信息
        /// </summary>
        public RedisMemoryInfoModel Memory { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public RedisStatsInfoModel Stats { get; set; }

        /// <summary>
        /// 键空间相关信息
        /// </summary>
        public RedisKeyspaceInfoModel Keyspace { get; set; }

        /// <summary>
        /// CPU相关信息
        /// </summary>
        public RedisCpuInfoModel Cpu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RedisInfoModel()
        {
            Server = new RedisServerInfoModel();
            Clients = new RedisClientsInfoModel();
            Memory = new RedisMemoryInfoModel();
            Stats = new RedisStatsInfoModel();
            Keyspace = new RedisKeyspaceInfoModel();
            Cpu = new RedisCpuInfoModel();
        }
    }
}
