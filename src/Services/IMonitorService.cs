using Lycoris.CSRedisCore.Extensions.Models;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis 监控服务接口，提供服务器信息和运行状态查询
    /// </summary>
    public interface IMonitorService
    {
        /// <summary>
        /// 获取 Redis 服务器运行信息
        /// </summary>
        /// <returns>Redis 服务器信息模型</returns>
        Task<RedisInfoModel> GetInfoAsync();
    }
}
