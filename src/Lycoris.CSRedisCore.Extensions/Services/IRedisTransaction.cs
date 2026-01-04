using CSRedis;
using System;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisTransaction
    {
        /// <summary>
        /// 获取 Redis 事务 管道流
        /// </summary>
        /// <returns></returns>
        CSRedisClientPipe<string> GetTransaction();

        /// <summary>
        /// Redis 事务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        object[] Execute(Action<CSRedisClientPipe<string>> action);

        /// <summary>
        /// 异步事务执行，接收异步操作委托
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<object[]> ExecuteAsync(Func<CSRedisClientPipe<string>, Task> func);
    }
}
