namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisClientsInfoModel
    {
        /// <summary>
        /// 当前连接的客户端数量
        /// </summary>
        public int ConnectedClients { get; set; }

        /// <summary>
        /// 连接负载
        /// </summary>
        public string LoadDescription
        {
            get
            {
                if (ConnectedClients > 0.8 * 10000) // 假设 maxclients 为 10000
                    return "高负载";
                else if (ConnectedClients > 0.5 * 10000)
                    return "中负载";
                else
                    return "低负载";
            }
        }
    }
}
