using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis String 类型操作接口，提供键值对的增删改查、计数器、子字符串等操作
    /// </summary>
    public interface IRedisStringCache
    {
        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值，不存在则返回 null</returns>
        string Get(string key);

        /// <summary>
        /// 获取指定 key 的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值，不存在则返回 null</returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值，不存在则返回 null</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 获取指定 key 的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值，不存在则返回 null</returns>
        Task<T> GetAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取该目录下所有的键值（模糊匹配）
        /// </summary>
        /// <param name="key">缓存键模式</param>
        /// <returns>匹配到的值列表</returns>
        List<string> MultipleGet(string key);

        /// <summary>
        /// 获取该目录下所有的键值（模糊匹配）
        /// </summary>
        /// <param name="key">缓存键模式</param>
        /// <returns>匹配到的值列表</returns>
        Task<List<string>> MultipleGetAsync(string key);

        /// <summary>
        /// 获取该目录下所有的键值（模糊匹配），并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键模式</param>
        /// <returns>匹配到的值列表</returns>
        List<T> MultipleGet<T>(string key) where T : class;

        /// <summary>
        /// 获取该目录下所有的键值（模糊匹配），并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键模式</param>
        /// <returns>匹配到的值列表</returns>
        Task<List<T>> MultipleGetAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取多个 key 的值
        /// </summary>
        /// <param name="key">缓存键数组</param>
        /// <returns>值列表</returns>
        List<string> MultipleGet(params string[] key);

        /// <summary>
        /// 获取多个 key 的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键数组</param>
        /// <returns>值列表</returns>
        List<T> MultipleGet<T>(params string[] key) where T : class;

        /// <summary>
        /// 获取多个 key 的值
        /// </summary>
        /// <param name="key">缓存键数组</param>
        /// <returns>值列表</returns>
        Task<List<string>> MultipleGetAsync(params string[] key);

        /// <summary>
        /// 获取多个 key 的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键数组</param>
        /// <returns>值列表</returns>
        Task<List<T>> MultipleGetAsync<T>(params string[] key) where T : class;

        /// <summary>
        /// 将给定 key 的值设为 value，并返回 key 的旧值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">新值</param>
        /// <returns>旧值</returns>
        string GetSet(string key, string value);

        /// <summary>
        /// 将给定 key 的值设为 value，并返回 key 的旧值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">新值</param>
        /// <returns>旧值</returns>
        Task<string> GetSetAsync(string key, string value);

        /// <summary>
        /// 将给定 key 的值设为 value，并返回 key 的旧值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">新值</param>
        /// <returns>旧值</returns>
        string GetSet<T>(string key, T value) where T : class;

        /// <summary>
        /// 将给定 key 的值设为 value，并返回 key 的旧值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">新值</param>
        /// <returns>旧值</returns>
        Task<string> GetSetAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool Set(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetAsync(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 设置指定 key 的值，自动序列化对象
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool Set<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 设置指定 key 的值，自动序列化对象
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 仅当 key 不存在时才设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetIfNotExistsAsync(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 仅当 key 不存在时才设置值，自动序列化对象
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetIfNotExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">键值对数组，按 key1, value1, key2, value2 顺序</param>
        /// <returns>是否设置成功</returns>
        bool MultipleSet(params object[] keyValues);

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">键值对数组，按 key1, value1, key2, value2 顺序</param>
        /// <returns>是否设置成功</returns>
        Task<bool> MultipleSetAsync(params object[] keyValues);

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">增量值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>增加后的值</returns>
        long Addition(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">增量值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>增加后的值</returns>
        Task<long> AdditionAsync(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 将 key 所储存的值减去给定的减量值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">减量值（正整数）</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>减去后的值</returns>
        long Subtraction(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 将 key 所储存的值减去给定的减量值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">减量值（正整数）</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>减去后的值</returns>
        Task<long> SubtractionAsync(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 仅当 key 已存在时才设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool SetIfExists(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 仅当 key 已存在时才设置值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetIfExistsAsync(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 仅当 key 已存在时才设置值，自动序列化对象
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool SetIfExists<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 仅当 key 已存在时才设置值，自动序列化对象
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetIfExistsAsync<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 获取字符串值的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字符串长度</returns>
        long StringLength(string key);

        /// <summary>
        /// 获取字符串值的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字符串长度</returns>
        Task<long> StringLengthAsync(string key);

        /// <summary>
        /// 追加内容到已有字符串末尾，返回新长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要追加的值</param>
        /// <returns>追加后的字符串长度</returns>
        long Append(string key, string value);

        /// <summary>
        /// 追加内容到已有字符串末尾，返回新长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要追加的值</param>
        /// <returns>追加后的字符串长度</returns>
        Task<long> AppendAsync(string key, string value);

        /// <summary>
        /// 获取子字符串
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>子字符串</returns>
        string GetRange(string key, long start, long end);

        /// <summary>
        /// 获取子字符串
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>子字符串</returns>
        Task<string> GetRangeAsync(string key, long start, long end);

        /// <summary>
        /// 覆盖指定偏移位置的子字符串，返回新长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="value">要写入的值</param>
        /// <returns>修改后的字符串长度</returns>
        long SetRange(string key, uint offset, string value);

        /// <summary>
        /// 覆盖指定偏移位置的子字符串，返回新长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="value">要写入的值</param>
        /// <returns>修改后的字符串长度</returns>
        Task<long> SetRangeAsync(string key, uint offset, string value);

        /// <summary>
        /// 仅当 key 不存在时才设置值（同步版本）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool SetIfNotExists(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 仅当 key 不存在时才设置值（同步版本），自动序列化对象
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool SetIfNotExists<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 获取指定 key 的原始字节值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字节数组，不存在则返回 null</returns>
        byte[] GetBytes(string key);

        /// <summary>
        /// 获取指定 key 的原始字节值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字节数组，不存在则返回 null</returns>
        Task<byte[]> GetBytesAsync(string key);

        /// <summary>
        /// 设置指定 key 的原始字节值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">字节数组</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool SetBytes(string key, byte[] value, TimeSpan? expire = null);

        /// <summary>
        /// 设置指定 key 的原始字节值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">字节数组</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetBytesAsync(string key, byte[] value, TimeSpan? expire = null);
    }
}
