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
        /// Redis队列入队列，支持去重（长队列场景，基于辅助Set实现）
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">入队值</param>
        /// <param name="checkDuplicate">是否检测重复，默认true</param>
        /// <returns>是否成功入队（重复时返回false）</returns>
        public bool Enqueue(string key, string value, bool checkDuplicate = true)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            if (!checkDuplicate)
            {
                CSRedisCore.RPush(key, value);
                return true;
            }

            // 额外的辅助 Set key，约定为原队列key后缀 ":set"
            var setKey = key + ":set";

            var script = @"
                          if redis.call('SISMEMBER', KEYS[2], ARGV[1]) == 1 then
                              return 0
                          else
                              redis.call('RPUSH', KEYS[1], ARGV[1])
                              redis.call('SADD', KEYS[2], ARGV[1])
                              return 1
                          end
                          ";

            var result = CSRedisCore.Eval(script, key, setKey, value);

            return (long)result == 1;
        }

        /// <summary>
        /// Redis队列入队列，异步版本，支持去重（长队列场景，基于辅助Set实现）
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">入队值</param>
        /// <param name="checkDuplicate">是否检测重复，默认true</param>
        /// <returns>是否成功入队（重复时返回false）</returns>
        public async Task<bool> EnqueueAsync(string key, string value, bool checkDuplicate = true)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            if (!checkDuplicate)
            {
                await CSRedisCore.RPushAsync(key, value);
                return true;
            }

            var setKey = key + ":set";

            var script = @"
                             if redis.call('SISMEMBER', KEYS[2], ARGV[1]) == 1 then
                                 return 0
                             else
                                 redis.call('RPUSH', KEYS[1], ARGV[1])
                                 redis.call('SADD', KEYS[2], ARGV[1])
                                 return 1
                             end
                         ";

            var result = await CSRedisCore.EvalAsync(script, key, setKey, value);
            return (long)result == 1;
        }

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="checkDuplicate"></param>
        public bool Enqueue<T>(string key, T value, bool checkDuplicate = true) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return false;

            if (value == null)
                return false;

            return this.Enqueue(key, JsonConvert.SerializeObject(value, JsonSetting), checkDuplicate);
        }

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="checkDuplicate"></param>
        public async Task<bool> EnqueueAsync<T>(string key, T value, bool checkDuplicate = true) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return false;

            if (value == null)
                return false;

            return await this.EnqueueAsync(key, JsonConvert.SerializeObject(value, JsonSetting), checkDuplicate);
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

            var setKey = key + ":set";

            var script = @"
                local val = redis.call('LPOP', KEYS[1])
                if val ~= false then
                    redis.call('SREM', KEYS[2], val)
                    return val
                else
                    return nil
                end
            ";

            var result = CSRedisCore.Eval(script, key, setKey);
            return result?.ToString();
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

            var setKey = key + ":set";

            var script = @"
                local val = redis.call('LPOP', KEYS[1])
                if val ~= false then
                    redis.call('SREM', KEYS[2], val)
                    return val
                else
                    return nil
                end
            ";

            var result = await CSRedisCore.EvalAsync(script, key, setKey);
            return result?.ToString();
        }

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Dequeue<T>(string key) where T : class
        {
            var str = Dequeue(key);

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
            var str = await DequeueAsync(key);

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
        /// 从队列移除指定值（所有匹配项），同步移除辅助Set对应元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void RemoveValueFromQueue(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = key + ":set";

            var script = @"
                              redis.call('LREM', KEYS[1], 0, ARGV[1])
                              redis.call('SREM', KEYS[2], ARGV[1])
                              return 1
                          ";

            CSRedisCore.Eval(script, key, setKey, value);
        }

        /// <summary>
        /// 从队列移除指定值，指定数量（正数从头部开始，负数从尾部开始），同步移除辅助Set对应元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public void RemoveValueFromQueue(string key, string value, int count)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = key + ":set";

            var script = @"
                             redis.call('LREM', KEYS[1], ARGV[2], ARGV[1])
                             redis.call('SREM', KEYS[2], ARGV[1])
                             return 1
                         ";

            CSRedisCore.Eval(script, key, setKey, value, count);
        }

        /// <summary>
        /// 泛型版本，序列化后移除指定值，移除所有匹配项，同步维护辅助Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void RemoveValueFromQueue<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            RemoveValueFromQueue(key, json);
        }

        /// <summary>
        /// 泛型版本，序列化后移除指定值，指定数量，同步维护辅助Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public void RemoveValueFromQueue<T>(string key, T value, int count) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            RemoveValueFromQueue(key, json, count);
        }

        /// <summary>
        /// 异步版本，移除指定值（所有匹配项），同步移除辅助Set对应元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task RemoveValueFromQueueAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = key + ":set";

            var script = @"
                             redis.call('LREM', KEYS[1], 0, ARGV[1])
                             redis.call('SREM', KEYS[2], ARGV[1])
                             return 1
                         ";

            await CSRedisCore.EvalAsync(script, key, setKey, value);
        }

        /// <summary>
        /// 异步版本，移除指定值，指定数量，同步维护辅助Set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task RemoveValueFromQueueAsync(string key, string value, int count)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = key + ":set";

            var script = @"
                             redis.call('LREM', KEYS[1], ARGV[2], ARGV[1])
                             redis.call('SREM', KEYS[2], ARGV[1])
                             return 1
                         ";

            await CSRedisCore.EvalAsync(script, key, setKey, value, count);
        }

        /// <summary>
        /// 异步泛型版本，序列化后移除指定值，所有匹配项，同步维护辅助Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task RemoveValueFromQueueAsync<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            await RemoveValueFromQueueAsync(key, json);
        }

        /// <summary>
        /// 异步泛型版本，序列化后移除指定值，指定数量，同步维护辅助Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task RemoveValueFromQueueAsync<T>(string key, T value, int count) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            await RemoveValueFromQueueAsync(key, json, count);
        }

        /// <summary>
        /// 检测队列中是否存在指定值（使用辅助Set检测，效率高）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckValueExitsFromQueue(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            var setKey = key + ":set";
            return CSRedisCore.SIsMember(setKey, value);
        }

        /// <summary>
        /// 泛型版本，检测队列中是否存在指定值（序列化后，使用辅助Set检测）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckValueExitsFromQueue<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                throw new ArgumentNullException(nameof(value));

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            return CheckValueExitsFromQueue(key, json);
        }

        /// <summary>
        /// 异步版本，检测队列中是否存在指定值（使用辅助Set检测）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> CheckValueExitsFromQueueAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            var setKey = key + ":set";
            return await CSRedisCore.SIsMemberAsync(setKey, value);
        }

        /// <summary>
        /// 异步泛型版本，检测队列中是否存在指定值（序列化后，使用辅助Set检测）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> CheckValueExitsFromQueueAsync<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                throw new ArgumentNullException(nameof(value));

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            return await CheckValueExitsFromQueueAsync(key, json);
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
    }
}
