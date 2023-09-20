using CSRedis;
using Lycoris.CSRedisCore.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisCacheUtils : IRedisCacheUtils
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisCacheUtils(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
        }

        /// <summary>
        /// 尝试开启分布式锁，若失败立刻返回null
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="CSRedisClientLock"/></returns>
        public RedisLock Lock(string key, int timeout, bool autoDelay = true)
        {
            try
            {
                var value = Guid.NewGuid().ToString();
                var client = CSRedisCore.Set(key, value, timeout, RedisExistence.Nx) ? new CSRedisClientLock(CSRedisCore, key, value, timeout, timeout / 2.0, autoDelay) : null;
                return new RedisLock(client);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{this.GetType().FullName} - {ex.Message}");
                Console.ResetColor();
                return new RedisLock(null);
            }
        }

        /// <summary>
        /// 尝试开启分布式锁，若失败立刻返回null
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        public async Task<RedisLock> LockAsync(string key, int timeout, bool autoDelay = true)
        {
            try
            {
                var value = Guid.NewGuid().ToString();
                var client = await CSRedisCore.SetAsync(key, value, timeout, RedisExistence.Nx) ? new CSRedisClientLock(CSRedisCore, key, value, timeout, timeout / 2.0, autoDelay) : null;
                return new RedisLock(client);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{this.GetType().FullName} - {ex.Message}");
                Console.ResetColor();
                return new RedisLock(null);
            }
        }

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="getTimeout">获取锁超时间</param>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        public RedisLock TryLock(int getTimeout, string key, int timeout, bool autoDelay = true)
        {
            try
            {
                var startTime = DateTime.Now;
                CSRedisClientLock client = null;
                while (DateTime.Now.Subtract(startTime).TotalSeconds < getTimeout)
                {
                    var value = Guid.NewGuid().ToString();
                    if (CSRedisCore.Set(key, value, timeout, RedisExistence.Nx))
                    {
                        client = new CSRedisClientLock(CSRedisCore, key, value, timeout, timeout / 2.0, autoDelay);
                        break;
                    }

                    Thread.CurrentThread.Join(100);
                }
                return new RedisLock(client);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{this.GetType().FullName} - {ex.Message}");
                Console.ResetColor();
                return new RedisLock(null);
            }
        }

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="getTimeout">获取锁超时间</param>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        public async Task<RedisLock> TryLockAsync(int getTimeout, string key, int timeout, bool autoDelay = true)
        {
            try
            {
                var startTime = DateTime.Now;
                CSRedisClientLock client = null;
                while (DateTime.Now.Subtract(startTime).TotalSeconds < getTimeout)
                {
                    var value = Guid.NewGuid().ToString();
                    if (await CSRedisCore.SetAsync(key, value, timeout, RedisExistence.Nx))
                    {
                        client = new CSRedisClientLock(CSRedisCore, key, value, timeout, timeout / 2.0, autoDelay);
                        break;
                    }

                    Thread.CurrentThread.Join(100);
                }
                return new RedisLock(client);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{this.GetType().FullName} - {ex.Message}");
                Console.ResetColor();
                return new RedisLock(null);
            }
        }

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Enqueue(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            CSRedisCore.RPush(key, value);
        }

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task EnqueueAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (string.IsNullOrEmpty(key))
                return;

            if (value == null || value.Length < 0)
                return;

            await CSRedisCore.RPushAsync(key, value);
        }

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Enqueue<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            CSRedisCore.RPush(key, JsonConvert.SerializeObject(value, JsonSetting));
        }

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task EnqueueAsync<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            await CSRedisCore.RPushAsync(key, JsonConvert.SerializeObject(value, JsonSetting));
        }

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Dequeue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return CSRedisCore.LPop(key);
        }

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> DequeueAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return await CSRedisCore.LPopAsync(key);
        }

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Dequeue<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return default;

            var str = CSRedisCore.LPop(key);
            return JsonConvert.DeserializeObject<T>(str, JsonSetting);
        }

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> DequeueAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return default;

            var str = await CSRedisCore.LPopAsync(key);
            return JsonConvert.DeserializeObject<T>(str, JsonSetting);
        }

        /// <summary>
        /// 执行 Lua 脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        public object RunLuaScript(string script, string key, params object[] args) => CSRedisCore.Eval(script, key, args);

        /// <summary>
        /// 执行 Lua 脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        public async Task<object> RunLuaScriptAsync(string script, string key, params object[] args) => await CSRedisCore.EvalAsync(script, key, args);

        /// <summary>
        /// 清除所有分区节点中，所有 Lua 脚本缓存
        /// </summary>
        public void FlushLuaScriptCache() => CSRedisCore.ScriptFlush();

        /// <summary>
        /// 清除所有分区节点中，所有 Lua 脚本缓存
        /// </summary>
        public async Task FlushLuaScriptCacheAsync() => await CSRedisCore.ScriptFlushAsync();

        /// <summary>
        /// 杀死所有分区节点中，当前正在运行的 Lua 脚本
        /// </summary>
        public void KillLuaScript() => CSRedisCore.ScriptKill();

        /// <summary>
        /// 杀死所有分区节点中，当前正在运行的 Lua 脚本
        /// </summary>
        public async Task KillLuaScriptAsync() => await CSRedisCore.ScriptKillAsync();
    }
}
