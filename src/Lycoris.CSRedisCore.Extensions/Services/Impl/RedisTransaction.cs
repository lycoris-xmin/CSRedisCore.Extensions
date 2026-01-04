using CSRedis;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

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
        /// 同步事务执行，接收同步操作委托
        /// </summary>
        /// <param name="action"></param>
        /// <returns>事务命令返回结果数组</returns>
        public object[] Execute(Action<CSRedisClientPipe<string>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var pipe = CSRedisCore.StartPipe();

            try
            {
                action.Invoke(pipe);
                return pipe.EndPipe();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 异步事务执行，接收异步操作委托
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<object[]> ExecuteAsync(Func<CSRedisClientPipe<string>, Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var pipe = CSRedisCore.StartPipe();

            try
            {
                await func.Invoke(pipe);
                return pipe.EndPipe();
            }
            catch
            {
                throw;
            }
        }
    }
}
