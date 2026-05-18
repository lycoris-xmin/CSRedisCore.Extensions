using CSRedis;
using Lycoris.CSRedisCore.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis 工具操作接口，提供分布式锁、队列、Lua 脚本执行、事务管道、Redis 信息查询等操作
    /// </summary>
    public interface IRedisCacheUtils
    {
        /// <summary>
        /// 尝试开启分布式锁，若失败立刻返回null
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns>分布式锁对象，获取失败则返回 null</returns>
        RedisLock Lock(string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// 尝试开启分布式锁，若失败立刻返回null
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns>分布式锁对象，获取失败则返回 null</returns>
        Task<RedisLock> LockAsync(string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="getTimeout">获取锁超时间</param>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns>分布式锁对象，获取失败则返回 null</returns>
        RedisLock TryLock(int getTimeout, string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="getTimeout">获取锁超时间</param>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns>分布式锁对象，获取失败则返回 null</returns>
        Task<RedisLock> TryLockAsync(int getTimeout, string key, int timeout, bool autoDelay = true);

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">值</param>
        /// <param name="checkDuplicate">是否检查重复</param>
        /// <returns>是否入队成功</returns>
        bool Enqueue(string key, string value, bool checkDuplicate = true);

        /// <summary>
        /// Redis队列入队列
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">值</param>
        /// <param name="checkDuplicate">是否检查重复</param>
        /// <returns>是否入队成功</returns>
        Task<bool> EnqueueAsync(string key, string value, bool checkDuplicate = true);

        /// <summary>
        /// Redis队列入队列，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">值</param>
        /// <param name="checkDuplicate">是否检查重复</param>
        /// <returns>是否入队成功</returns>
        bool Enqueue<T>(string key, T value, bool checkDuplicate = true) where T : class;

        /// <summary>
        /// Redis队列入队列，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">值</param>
        /// <param name="checkDuplicate">是否检查重复</param>
        /// <returns>是否入队成功</returns>
        Task<bool> EnqueueAsync<T>(string key, T value, bool checkDuplicate = true) where T : class;

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key">队列键</param>
        /// <returns>出队的值，队列为空则返回 null</returns>
        string Dequeue(string key);

        /// <summary>
        /// Redis队列出队列
        /// </summary>
        /// <param name="key">队列键</param>
        /// <returns>出队的值，队列为空则返回 null</returns>
        Task<string> DequeueAsync(string key);

        /// <summary>
        /// Redis队列出队列，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">队列键</param>
        /// <returns>出队的值，队列为空则返回 null</returns>
        T Dequeue<T>(string key) where T : class;

        /// <summary>
        /// Redis队列出队列，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">队列键</param>
        /// <returns>出队的值，队列为空则返回 null</returns>
        Task<T> DequeueAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取队列中元素的数量
        /// </summary>
        /// <param name="key">队列键</param>
        /// <returns>队列长度</returns>
        Task<long> QueueCountAsync(string key);

        /// <summary>
        /// 将指定值移除队列
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        void RemoveValueFromQueue(string key, string value);

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        /// <param name="count">移除数量，正数从头、负数从尾、0为全部</param>
        void RemoveValueFromQueue(string key, string value, int count);

        /// <summary>
        /// 将指定值移除队列，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        void RemoveValueFromQueue<T>(string key, T value) where T : class;

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        /// <param name="count">移除数量，正数从头、负数从尾、0为全部</param>
        void RemoveValueFromQueue<T>(string key, T value, int count) where T : class;

        /// <summary>
        /// 将指定值移除队列
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        /// <returns>异步操作</returns>
        Task RemoveValueFromQueueAsync(string key, string value);

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        /// <param name="count">移除数量，正数从头、负数从尾、0为全部</param>
        /// <returns>异步操作</returns>
        Task RemoveValueFromQueueAsync(string key, string value, int count);

        /// <summary>
        /// 将指定值移除队列，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        /// <returns>异步操作</returns>
        Task RemoveValueFromQueueAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// 如果 count 大于 0，从头部开始移除 count 个等于 value 的元素。
        /// 如果 count 小于 0，从尾部开始移除 count 个等于 value 的元素。
        /// 如果 count 等于 0，移除列表中所有等于 value 的元素。
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">要移除的值</param>
        /// <param name="count">移除数量，正数从头、负数从尾、0为全部</param>
        /// <returns>异步操作</returns>
        Task RemoveValueFromQueueAsync<T>(string key, T value, int count) where T : class;

        /// <summary>
        /// 检测队列中是否存在某个元素
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">要检测的值</param>
        /// <returns>是否存在</returns>
        bool CheckValueExitsFromQueue(string key, string value);

        /// <summary>
        /// 检测队列中是否存在某个元素，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">要检测的值</param>
        /// <returns>是否存在</returns>
        bool CheckValueExitsFromQueue<T>(string key, T value) where T : class;

        /// <summary>
        /// 检测队列中是否存在某个元素
        /// </summary>
        /// <param name="key">队列键</param>
        /// <param name="value">要检测的值</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckValueExitsFromQueueAsync(string key, string value);

        /// <summary>
        /// 检测队列中是否存在某个元素，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">队列键</param>
        /// <param name="value">要检测的值</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckValueExitsFromQueueAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// 执行 Lua 脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        /// <returns>脚本执行结果</returns>
        object RunLuaScript(string script, string key, params object[] args);

        /// <summary>
        /// 执行 Lua 脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        /// <returns>脚本执行结果</returns>
        Task<object> RunLuaScriptAsync(string script, string key, params object[] args);

        /// <summary>
        /// 清空 Lua 脚本缓存
        /// </summary>
        void FlushLuaScriptCache();

        /// <summary>
        /// 清空 Lua 脚本缓存
        /// </summary>
        /// <returns>异步操作</returns>
        Task FlushLuaScriptCacheAsync();

        /// <summary>
        /// 杀死正在运行的 Lua 脚本
        /// </summary>
        void KillLuaScript();

        /// <summary>
        /// 杀死正在运行的 Lua 脚本
        /// </summary>
        /// <returns>异步操作</returns>
        Task KillLuaScriptAsync();

        /// <summary>
        /// 获取 Redis 服务器的运行信息
        /// </summary>
        /// <param name="section">信息分区，为 null 时返回所有信息</param>
        /// <returns>节点名与值的列表</returns>
        List<(string node, string value)> RedisInfo(InfoSection? section = null);

        /// <summary>
        /// 获取 Redis 服务器的运行信息
        /// </summary>
        /// <param name="section">信息分区，为 null 时返回所有信息</param>
        /// <returns>节点名与值的列表</returns>
        Task<List<(string node, string value)>> RedisInfoAsync(InfoSection? section = null);

        /// <summary>
        /// 获取 Redis 事务管道流
        /// </summary>
        /// <returns>事务管道对象</returns>
        CSRedisClientPipe<string> GetTransaction();

        /// <summary>
        /// Redis 同步事务执行
        /// </summary>
        /// <param name="action">同步事务操作委托</param>
        /// <returns>事务命令返回结果数组</returns>
        object[] PipeExecute(Action<CSRedisClientPipe<string>> action);

        /// <summary>
        /// 异步事务执行，接收异步操作委托
        /// </summary>
        /// <param name="func">异步事务操作委托</param>
        /// <returns>事务命令返回结果数组</returns>
        Task<object[]> PipeExecuteAsync(Func<CSRedisClientPipe<string>, Task> func);

        /// <summary>
        /// 使用类型化管道上下文执行 Redis 事务。
        /// 回调内通过 cache.String/cache.Hash 等排队命令，单次操作不可使用 await。
        /// </summary>
        /// <param name="action">同步事务操作委托</param>
        /// <returns>事务命令返回结果数组</returns>
        object[] PipeExecute(Action<Pipe.RedisCachePipe> action);

        /// <summary>
        /// 使用类型化管道上下文异步执行 Redis 事务。
        /// 回调内通过 cache.String/cache.Hash 等排队命令，单次操作不可使用 await。
        /// </summary>
        /// <param name="func">异步事务操作委托</param>
        /// <returns>事务命令返回结果数组</returns>
        Task<object[]> PipeExecuteAsync(Func<Pipe.RedisCachePipe, Task> func);
    }
}
