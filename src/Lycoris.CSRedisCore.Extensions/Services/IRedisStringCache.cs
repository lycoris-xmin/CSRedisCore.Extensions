using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisStringCache
    {
        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 获取指定 key 的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<string> MultipleGet(string key);

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<string>> MultipleGetAsync(string key);

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> MultipleGet<T>(string key) where T : class;

        /// <summary>
        /// 获取该目录下所有的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> MultipleGetAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<string> MultipleGet(params string[] key);

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> MultipleGet<T>(params string[] key) where T : class;

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<string>> MultipleGetAsync(params string[] key);

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> MultipleGetAsync<T>(params string[] key) where T : class;

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetSet(string key, string value);

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> GetSetAsync(string key, string value);

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetSet<T>(string key, T value) where T : class;

        /// <summary>
        /// 将给定 key 的值设为 value ,并返回 key 的旧值(old value)。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> GetSetAsync<T>(string key, T value) where T : class;

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool Set(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, string value, TimeSpan? expire = null);

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expire = null) where T : class;

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        bool MultipleSet(params object[] keyValues);

        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<bool> MultipleSetAsync(params object[] keyValues);

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        long Addition(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 将 key 所储存的值加上给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        Task<long> AdditionAsync(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 将 key 所储存的值减去给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值为正整数</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        long Subtraction(string key, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 将 key 所储存的值减去给定的增量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值为正整数</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        Task<long> SubtractionAsync(string key, long value = 1, TimeSpan? timeSpan = null);
    }
}
