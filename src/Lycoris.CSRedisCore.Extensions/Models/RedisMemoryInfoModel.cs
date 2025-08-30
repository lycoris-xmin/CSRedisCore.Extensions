namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisMemoryInfoModel
    {
        /// <summary>
        /// Redis 使用的内存量（字节）
        /// </summary>
        public long UsedMemory { get; set; }

        /// <summary>
        /// Redis 使用的内存量（人类可读格式）
        /// </summary>
        public string UsedMemoryHuman { get; set; }

        /// <summary>
        /// 内存碎片率
        /// </summary>
        public double MemFragmentationRatio { get; set; }

        /// <summary>
        /// 最大内存
        /// </summary>
        public long MaxMemory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaxMemoryHuman { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LoadDescription
        {
            get
            {
                if (MaxMemory <= 0)
                    return "-";

                if (UsedMemory > 0.8 * MaxMemory)
                    return "高负载";
                else if (UsedMemory > 0.5 * MaxMemory)
                    return "中负载";
                else
                    return "低负载";
            }
        }
    }
}
