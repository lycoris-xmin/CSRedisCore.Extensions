using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisListCache
    {
        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetAndRemoveFirst(string key);

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAndRemoveFirstAsync(string key);

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetAndRemoveFirst<T>(string key) where T : class;

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAndRemoveFirstAsync<T>(string key) where T : class;

        /// <summary>
        /// 移出并获取列表的最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetAndRemoveLast(string key);

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAndRemoveLastAsync(string key);

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetAndRemoveLast<T>(string key) where T : class;

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAndRemoveLastAsync<T>(string key) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetFirst(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetFirstAsync(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetFirst<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetFirstAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetLast(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetLastAsync(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetLast<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetLastAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<string>> GetAllAsync(string key);
    }
}
