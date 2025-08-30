using Lycoris.CSRedisCore.Extensions.Models;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMonitorService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<RedisInfoModel> GetInfoAsync();
    }
}
