using CSRedis;
using Lycoris.CSRedisCore.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

                    Task.Delay(200).RunSynchronously();
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

                    await Task.Delay(200);
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

            if (string.IsNullOrEmpty(str))
                return default;

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

            if (string.IsNullOrEmpty(str))
                return default;

            return JsonConvert.DeserializeObject<T>(str, JsonSetting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> QueueCountAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return 0;

            return await CSRedisCore.LLenAsync(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void RemoveValueFromQueue(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            CSRedisCore.LRem(key, 0, value);
        }

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public void RemoveValueFromQueue(string key, string value, int count)
        {
            if (string.IsNullOrEmpty(key))
                return;

            CSRedisCore.LRem(key, count, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void RemoveValueFromQueue<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            var _value = JsonConvert.SerializeObject(value, JsonSetting);

            CSRedisCore.LRem(key, 0, _value);
        }

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public void RemoveValueFromQueue<T>(string key, T value, int count) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            var _value = JsonConvert.SerializeObject(value, JsonSetting);

            CSRedisCore.LRem(key, count, _value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task RemoveValueFromQueueAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            await CSRedisCore.LRemAsync(key, 0, value);
        }

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public async Task RemoveValueFromQueueAsync(string key, string value, int count)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            await CSRedisCore.LRemAsync(key, count, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task RemoveValueFromQueueAsync<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            var _value = JsonConvert.SerializeObject(value, JsonSetting);

            await CSRedisCore.LRemAsync(key, 0, _value);
        }

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public async Task RemoveValueFromQueueAsync<T>(string key, T value, int count) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (value == null)
                return;

            var _value = JsonConvert.SerializeObject(value, JsonSetting);

            await CSRedisCore.LRemAsync(key, count, _value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckValueExitsFromQueue(string key, string value)
        {
            var script = CreateCheckValueExitsFromQueueLuaScript();
            var result = RunLuaScript(script, key, value);
            return result != null && (long)result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckValueExitsFromQueue<T>(string key, T value) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(T));

            var _value = JsonConvert.SerializeObject(value);

            var script = CreateCheckValueExitsFromQueueLuaScript();
            var result = RunLuaScript(script, key, _value);
            return result != null && (long)result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> CheckValueExitsFromQueueAsync(string key, string value)
        {
            var script = CreateCheckValueExitsFromQueueLuaScript();
            var result = await RunLuaScriptAsync(script, key, value);
            return result != null && (long)result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> CheckValueExitsFromQueueAsync<T>(string key, T value) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(T));

            var _value = JsonConvert.SerializeObject(value);

            var script = CreateCheckValueExitsFromQueueLuaScript();
            var result = await RunLuaScriptAsync(script, key, _value);
            return result != null && (long)result == 1;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public List<(string node, string value)> RedisInfo(InfoSection? section = null)
        {
            var result = CSRedisCore.NodesServerManager.Info(section);
            return result.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public async Task<List<(string node, string value)>> RedisInfoAsync(InfoSection? section = null)
        {
            var result = await CSRedisCore.NodesServerManager.InfoAsync(section);
            return result.ToList();
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
        public object[] PipeExecute(Action<CSRedisClientPipe<string>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var pipe = CSRedisCore.StartPipe())
            {
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
        }

        /// <summary>
        /// 异步事务执行，接收异步操作委托
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<object[]> PipeExecuteAsync(Func<CSRedisClientPipe<string>, Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            using (var pipe = CSRedisCore.StartPipe())
            {
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string CreateCheckValueExitsFromQueueLuaScript()
        {
            return @"
                local key = KEYS[1]  
                local value = ARGV[1]  
                local found = 0
                  
                for i, v in ipairs(redis.call('LRANGE', key, 0, -1)) do  
                    if v == value then  
                        found = 1  
                        break  
                    end  
                end  
                  
                return found
            ";
        }
    }
}
