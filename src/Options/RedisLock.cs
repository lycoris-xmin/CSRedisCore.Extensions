using CSRedis;
using System;

namespace Lycoris.CSRedisCore.Extensions.Options
{
    /// <summary>
    /// Redis 分布式锁封装
    /// </summary>
    public class RedisLock
    {
        private readonly CSRedisClientLock redisClientLock;

        /// <summary>
        /// 初始化 RedisLock 实例
        /// </summary>
        /// <param name="redisClientLock">CSRedis 客户端锁对象</param>
        public RedisLock(CSRedisClientLock redisClientLock)
        {
            this.redisClientLock = redisClientLock;
        }

        /// <summary>
        /// 是否已获取锁
        /// </summary>
        public bool IsLock { get => redisClientLock != null; }

        /// <summary>
        /// 延长锁的持有时间
        /// </summary>
        /// <param name="milliseconds">延长时间（毫秒）</param>
        /// <returns>是否成功延长</returns>
        public bool Delay(int milliseconds)
        {
            if (this.redisClientLock == null)
                throw new NullReferenceException($"{typeof(CSRedisClientLock).Name} Object reference not set to an instance of an object.");

            return this.redisClientLock.Delay(milliseconds);
        }

        /// <summary>
        /// 刷新锁的过期时间
        /// </summary>
        /// <param name="milliseconds">刷新时间（毫秒）</param>
        /// <returns>是否成功刷新</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Refresh(int milliseconds)
        {
            if (this.redisClientLock == null)
                throw new NullReferenceException($"{typeof(CSRedisClientLock).Name} Object reference not set to an instance of an object.");

            return this.redisClientLock.Refresh(milliseconds);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns>是否成功释放</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Unlock()
        {
            if (this.redisClientLock == null)
                throw new NullReferenceException($"{typeof(CSRedisClientLock).Name} Object reference not set to an instance of an object.");

            return this.redisClientLock.Unlock();
        }
    }
}
