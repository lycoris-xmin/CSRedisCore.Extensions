using CSRedis;
using Newtonsoft.Json;
using System;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisTransaction : IRedisTransaction
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisTransaction(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
        }

        /// <summary>
        /// 获取 Redis 事务 管道流
        /// </summary>
        /// <returns></returns>
        public CSRedisClientPipe<string> GetTransaction() => CSRedisCore.StartPipe();

        /// <summary>
        /// Redis 事务
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public object[] Shell(Action<CSRedisClientPipe<string>> func)
        {
            try
            {
                return CSRedisCore.StartPipe(func);
            }
            catch
            {
                throw;
            }
        }
    }
}
