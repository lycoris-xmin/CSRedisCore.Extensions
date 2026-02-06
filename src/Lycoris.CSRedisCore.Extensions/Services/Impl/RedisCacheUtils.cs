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
        private readonly string PrefixCacheKey = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisCacheUtils(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting, string prefixCacheKey)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
            PrefixCacheKey = prefixCacheKey;
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

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                    local queueKey = KEYS[1]
                    local countHashKey = ARGV[2]
                    local val = ARGV[1]
                    local checkDuplicate = tonumber(ARGV[3])
                    
                    if checkDuplicate == 1 then
                        local count = tonumber(redis.call('HGET', countHashKey, val) or '0')
                        if count > 0 then
                            return 0 -- 重复且不入队
                        end
                    end
                    
                    redis.call('RPUSH', queueKey, val)
                    redis.call('HINCRBY', countHashKey, val, 1)
                    return 1
                  ";

            var result = CSRedisCore.Eval(script, key, value, setKey, checkDuplicate ? "1" : "0");
            return (long)result == 1;
        }

        /// <summary>
        /// Redis队列入队列异步版本，支持去重（长队列场景，基于辅助Set实现）
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">入队值</param>
        /// <param name="checkDuplicate">是否检测重复，默认true</param>
        /// <returns>是否成功入队（重复时返回false）</returns>
        public async Task<bool> EnqueueAsync(string key, string value, bool checkDuplicate = true)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                    local queueKey = KEYS[1]
                    local countHashKey = ARGV[2]
                    local val = ARGV[1]
                    local checkDuplicate = tonumber(ARGV[3])
                    
                    if checkDuplicate == 1 then
                        local count = tonumber(redis.call('HGET', countHashKey, val) or '0')
                        if count > 0 then
                            return 0 -- 重复且不入队
                        end
                    end
                    
                    redis.call('RPUSH', queueKey, val)
                    redis.call('HINCRBY', countHashKey, val, 1)
                    return 1
                  ";

            var result = await CSRedisCore.EvalAsync(script, key, value, setKey, checkDuplicate ? "1" : "0");
            return (long)result == 1;
        }

        /// <summary>
        /// Redis队列入队列，泛型版本，自动序列化对象为JSON字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">入队对象</param>
        /// <param name="checkDuplicate">是否检测重复，默认true</param>
        /// <returns>是否成功入队</returns>
        public bool Enqueue<T>(string key, T value, bool checkDuplicate = true) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return false;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            return Enqueue(key, json, checkDuplicate);
        }

        /// <summary>
        /// Redis队列入队列异步泛型版本，自动序列化对象为JSON字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">入队对象</param>
        /// <param name="checkDuplicate">是否检测重复，默认true</param>
        /// <returns>是否成功入队</returns>
        public async Task<bool> EnqueueAsync<T>(string key, T value, bool checkDuplicate = true) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return false;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            return await EnqueueAsync(key, json, checkDuplicate);
        }

        /// <summary>
        /// Redis队列出队列，返回字符串
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <returns>出队元素字符串，如果队列为空返回null</returns>
        public string Dequeue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                            local queueKey = KEYS[1]
                            local countHashKey = ARGV[1]
                            
                            local val = redis.call('LPOP', queueKey)
                            if val then
                                local count = tonumber(redis.call('HGET', countHashKey, val) or ""0"")
                                if count > 1 then
                                    redis.call('HINCRBY', countHashKey, val, -1)
                                else
                                    redis.call('HDEL', countHashKey, val)
                                end
                                return val
                            else
                                return nil
                            end
                          ";

            var result = CSRedisCore.Eval(script, key, setKey);
            return result?.ToString();
        }

        /// <summary>
        /// Redis队列异步出队列，返回字符串
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <returns>出队元素字符串，如果队列为空返回null</returns>
        public async Task<string> DequeueAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                            local queueKey = KEYS[1]
                            local countHashKey = ARGV[1]
                            
                            local val = redis.call('LPOP', queueKey)
                            if val then
                                local count = tonumber(redis.call('HGET', countHashKey, val) or ""0"")
                                if count > 1 then
                                    redis.call('HINCRBY', countHashKey, val, -1)
                                else
                                    redis.call('HDEL', countHashKey, val)
                                end
                                return val
                            else
                                return nil
                            end
                          ";

            var result = await CSRedisCore.EvalAsync(script, key, setKey);
            return result?.ToString();
        }

        /// <summary>
        /// Redis队列出队列，泛型版本，自动反序列化JSON字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <returns>出队对象，如果队列为空返回默认值</returns>
        public T Dequeue<T>(string key) where T : class
        {
            var str = Dequeue(key);
            if (string.IsNullOrEmpty(str))
                return default;
            return JsonConvert.DeserializeObject<T>(str, JsonSetting);
        }

        /// <summary>
        /// Redis队列异步出队列，泛型版本，自动反序列化JSON字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <returns>出队对象，如果队列为空返回默认值</returns>
        public async Task<T> DequeueAsync<T>(string key) where T : class
        {
            var str = await DequeueAsync(key);
            if (string.IsNullOrEmpty(str))
                return default;
            return JsonConvert.DeserializeObject<T>(str, JsonSetting);
        }

        /// <summary>
        /// 获取Redis队列长度
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <returns>队列长度，如果Key为空返回0</returns>
        public async Task<long> QueueCountAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return 0;

            return await CSRedisCore.LLenAsync(key);
        }

        /// <summary>
        /// 从队列移除指定值的所有匹配项（同步），同时维护辅助Set一致性
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的值</param>
        public void RemoveValueFromQueue(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                            redis.call('LREM', KEYS[1], 0, ARGV[1])
                            redis.call('HDEL', ARGV[2], ARGV[1])
                            return 1
                          ";

            CSRedisCore.Eval(script, key, value, setKey);
        }

        /// <summary>
        /// 从队列移除指定值的指定数量匹配项（同步），正数从头部开始移除，负数从尾部开始移除，同时维护辅助Set
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的值</param>
        /// <param name="count">移除数量（正数从头部开始，负数从尾部开始，0移除所有匹配）</param>
        public void RemoveValueFromQueue(string key, string value, int count)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                            local removedCount = redis.call('LREM', KEYS[1], tonumber(ARGV[2]), ARGV[1])
                            if removedCount > 0 then
                                local currentCount = tonumber(redis.call('HGET', ARGV[3], ARGV[1]) or ""0"")
                                local newCount = currentCount - removedCount
                                if newCount > 0 then
                                    redis.call('HSET', ARGV[3], ARGV[1], newCount)
                                else
                                    redis.call('HDEL', ARGV[3], ARGV[1])
                                end
                            end
                            return 1
                          ";

            CSRedisCore.Eval(script, key, value, count, setKey);
        }

        /// <summary>
        /// 泛型版本，序列化后移除队列中指定值的所有匹配项，同步维护辅助Set
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的对象</param>
        public void RemoveValueFromQueue<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            RemoveValueFromQueue(key, json);
        }

        /// <summary>
        /// 泛型版本，序列化后移除队列中指定值的指定数量匹配项，同步维护辅助Set
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的对象</param>
        /// <param name="count">移除数量（正数从头部开始，负数从尾部开始，0移除所有匹配）</param>
        public void RemoveValueFromQueue<T>(string key, T value, int count) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            RemoveValueFromQueue(key, json, count);
        }

        /// <summary>
        /// 异步版本，从队列移除指定值的所有匹配项，同时维护辅助Set
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的值</param>
        /// <returns>任务</returns>
        public async Task RemoveValueFromQueueAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                            redis.call('LREM', KEYS[1], 0, ARGV[1])
                            redis.call('HDEL', ARGV[2], ARGV[1])
                            return 1
                          ";

            await CSRedisCore.EvalAsync(script, key, value, setKey);
        }

        /// <summary>
        /// 异步版本，从队列移除指定值的指定数量匹配项，同时维护辅助Set
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的值</param>
        /// <param name="count">移除数量（正数从头部开始，负数从尾部开始，0移除所有匹配）</param>
        /// <returns>任务</returns>
        public async Task RemoveValueFromQueueAsync(string key, string value, int count)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return;

            var setKey = string.IsNullOrEmpty(this.PrefixCacheKey) ? $"{key}:QueueHashSet" : $"{this.PrefixCacheKey.TrimEnd(':')}:{key}:QueueHashSet";

            var script = @"
                            local removedCount = redis.call('LREM', KEYS[1], tonumber(ARGV[2]), ARGV[1])
                            if removedCount > 0 then
                                local currentCount = tonumber(redis.call('HGET', ARGV[3], ARGV[1]) or ""0"")
                                local newCount = currentCount - removedCount
                                if newCount > 0 then
                                    redis.call('HSET', ARGV[3], ARGV[1], newCount)
                                else
                                    redis.call('HDEL', ARGV[3], ARGV[1])
                                end
                            end
                            return 1
                          ";

            await CSRedisCore.EvalAsync(script, key, value, count, setKey);
        }

        /// <summary>
        /// 异步泛型版本，序列化后移除队列中指定值的所有匹配项，同时维护辅助Set
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的对象</param>
        /// <returns>任务</returns>
        public async Task RemoveValueFromQueueAsync<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            await RemoveValueFromQueueAsync(key, json);
        }

        /// <summary>
        /// 异步泛型版本，序列化后移除队列中指定值的指定数量匹配项，同时维护辅助Set
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">要移除的对象</param>
        /// <param name="count">移除数量（正数从头部开始，负数从尾部开始，0移除所有匹配）</param>
        /// <returns>任务</returns>
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
        /// <param name="key">队列Key</param>
        /// <param name="value">待检测值</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool CheckValueExitsFromQueue(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            // 用 HGET 判断是否存在
            var countStr = CSRedisCore.HGet($"{key}:QueueHashSet", value);

            if (string.IsNullOrEmpty(countStr))
                return false;

            if (int.TryParse(countStr, out int count))
                return count > 0;

            return false;
        }

        /// <summary>
        /// 泛型版本，检测队列中是否存在指定值（序列化后，使用辅助Set检测）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">待检测对象</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool CheckValueExitsFromQueue<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                throw new ArgumentNullException(nameof(value));

            var json = JsonConvert.SerializeObject(value, JsonSetting);
            return CheckValueExitsFromQueue(key, json);
        }

        /// <summary>
        /// 异步版本，检测队列中是否存在指定值（使用辅助Set检测，效率高）
        /// </summary>
        /// <param name="key">队列Key</param>
        /// <param name="value">待检测值</param>
        /// <returns>任务，结果存在返回true，不存在返回false</returns>
        public async Task<bool> CheckValueExitsFromQueueAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            // 用 HGET 判断是否存在
            var countStr = await CSRedisCore.HGetAsync($"{key}:QueueHashSet", value);

            if (string.IsNullOrEmpty(countStr))
                return false;

            if (int.TryParse(countStr, out int count))
                return count > 0;

            return false;
        }

        /// <summary>
        /// 异步泛型版本，检测队列中是否存在指定值（序列化后，使用辅助Set检测）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">队列Key</param>
        /// <param name="value">待检测对象</param>
        /// <returns>任务，结果存在返回true，不存在返回false</returns>
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
