using CSRedis;
using Lycoris.CSRedisCore.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisCacheUtils
    {
        /// <summary>
        /// 尝试开启分布式锁，若失败立刻返回null
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        RedisLock Lock(string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// 尝试开启分布式锁，若失败立刻返回null
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        Task<RedisLock> LockAsync(string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="getTimeout">获取锁超时间</param>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        RedisLock TryLock(int getTimeout, string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="getTimeout">获取锁超时间</param>
        /// <param name="key"></param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns><see cref="RedisLock"/></returns>
        Task<RedisLock> TryLockAsync(int getTimeout, string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Enqueue(string key, string value);

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task EnqueueAsync(string key, string value);

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Enqueue<T>(string key, T value) where T : class;

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task EnqueueAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Dequeue(string key);

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> DequeueAsync(string key);

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Dequeue<T>(string key) where T : class;

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> DequeueAsync<T>(string key) where T : class;

        /// <summary>
        /// 将指定值移除队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void RemoveValueFromQueue(string key, string value);

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        void RemoveValueFromQueue(string key, string value, int count);

        /// <summary>
        /// 将指定值移除队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void RemoveValueFromQueue<T>(string key, T value) where T : class;

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        void RemoveValueFromQueue<T>(string key, T value, int count) where T : class;

        /// <summary>
        /// 将指定值移除队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task RemoveValueFromQueueAsync(string key, string value);

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task RemoveValueFromQueueAsync(string key, string value, int count);

        /// <summary>
        /// 将指定值移除队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task RemoveValueFromQueueAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task RemoveValueFromQueueAsync<T>(string key, T value, int count) where T : class;

        /// <summary>
        /// 检测队列中是否存在某个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CheckValueExitsFromQueue(string key, string value);

        /// <summary>
        /// 检测队列中是否存在某个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CheckValueExitsFromQueue<T>(string key, T value) where T : class;

        /// <summary>
        /// 检测队列中是否存在某个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> CheckValueExitsFromQueueAsync(string key, string value);

        /// <summary>
        /// 检测队列中是否存在某个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> CheckValueExitsFromQueueAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// 执行 Lua 脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        object RunLuaScript(string script, string key, params object[] args);

        /// <summary>
        /// 执行 Lua 脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        Task<object> RunLuaScriptAsync(string script, string key, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        void FlushLuaScriptCache();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task FlushLuaScriptCacheAsync();

        /// <summary>
        /// 
        /// </summary>
        void KillLuaScript();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task KillLuaScriptAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        List<(string node, string value)> RedisInfo(InfoSection? section = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<List<(string node, string value)>> RedisInfoAsync(InfoSection? section = null);
    }
}
