using CSRedis;
using System;

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
        /// <param name="func"></param>
        /// <returns></returns>
        object[] Shell(Action<CSRedisClientPipe<string>> func);
    }
}
