using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisSetCache
    {
        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        long Set(string key, params string[] value);

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        long Set<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> SetAsync(string key, params string[] value);

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> SetAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long Count(string key);

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> CountAsync(string key);

        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Exists(string key, string value);

        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key, string value);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        string GetRandom(string key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        T GetRandom<T>(string key) where T : class;

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        Task<string> GetRandomAsync(string key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        Task<T> GetRandomAsync<T>(string key) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setKey"></param>
        /// <returns></returns>
        Task<List<string>> GetAllAsync(string setKey);
    }
}
