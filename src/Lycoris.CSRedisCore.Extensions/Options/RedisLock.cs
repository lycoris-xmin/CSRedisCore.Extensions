using CSRedis;
using System;

namespace Lycoris.CSRedisCore.Extensions.Options
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisLock
    {
        private readonly CSRedisClientLock redisClientLock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisClientLock"></param>
        public RedisLock(CSRedisClientLock redisClientLock)
        {
            this.redisClientLock = redisClientLock;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLock { get => redisClientLock != null; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool Delay(int milliseconds)
        {
            if (this.redisClientLock == null)
                throw new NullReferenceException($"{typeof(CSRedisClientLock).Name} Object reference not set to an instance of an object.");

            return this.redisClientLock.Delay(milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Refresh(int milliseconds)
        {
            if (this.redisClientLock == null)
                throw new NullReferenceException($"{typeof(CSRedisClientLock).Name} Object reference not set to an instance of an object.");

            return this.redisClientLock.Refresh(milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Unlock()
        {
            if (this.redisClientLock == null)
                throw new NullReferenceException($"{typeof(CSRedisClientLock).Name} Object reference not set to an instance of an object.");

            return this.redisClientLock.Unlock();
        }
    }
}
