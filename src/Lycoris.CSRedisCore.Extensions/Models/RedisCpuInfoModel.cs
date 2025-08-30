namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisCpuInfoModel
    {
        /// <summary>
        /// Redis 系统级 CPU 使用时间（秒）
        /// </summary>
        public double UsedCpuSys { get; set; }

        /// <summary>
        /// Redis 用户级 CPU 使用时间（秒）
        /// </summary>
        public double UsedCpuUser { get; set; }

        /// <summary>
        /// 子进程的系统级 CPU 使用时间（秒）
        /// </summary>
        public double UsedCpuSysChildren { get; set; }

        /// <summary>
        /// 子进程的用户级 CPU 使用时间（秒）
        /// </summary>
        public double UsedCpuUserChildren { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LoadDescription
        {
            get
            {
                var totalCpu = UsedCpuSys + UsedCpuUser;
                var totalSystemCpu = (int)(UsedCpuSys + UsedCpuUser) * 100;

                var cpuUsage = totalCpu / totalSystemCpu * 100;

                if (cpuUsage < 50)
                    return "低负载";
                else if (cpuUsage < 80)
                    return "中负载";
                else
                    return "高负载";
            }
        }
    }
}
