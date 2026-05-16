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
        /// 获取列表的所有元素
        /// </summary>
        Task<List<string>> GetAllAsync(string key);

        /// <summary>
        /// 获取列表的所有元素
        /// </summary>
        List<string> GetAll(string key);

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        string GetByIndex(string key, long index);

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        Task<string> GetByIndexAsync(string key, long index);

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        T GetByIndex<T>(string key, long index) where T : class;

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        Task<T> GetByIndexAsync<T>(string key, long index) where T : class;

        /// <summary>
        /// 获取指定区间的元素
        /// </summary>
        List<string> GetRange(string key, long start, long stop);

        /// <summary>
        /// 获取指定区间的元素
        /// </summary>
        Task<List<string>> GetRangeAsync(string key, long start, long stop);

        /// <summary>
        /// 获取指定区间的元素（泛型）
        /// </summary>
        List<T> GetRange<T>(string key, long start, long stop) where T : class;

        /// <summary>
        /// 获取指定区间的元素（泛型）
        /// </summary>
        Task<List<T>> GetRangeAsync<T>(string key, long start, long stop) where T : class;

        /// <summary>
        /// 在指定元素前插入新元素
        /// </summary>
        long InsertBefore(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素前插入新元素
        /// </summary>
        Task<long> InsertBeforeAsync(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素前插入新元素（泛型）
        /// </summary>
        long InsertBefore<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 在指定元素前插入新元素（泛型）
        /// </summary>
        Task<long> InsertBeforeAsync<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 在指定元素后插入新元素
        /// </summary>
        long InsertAfter(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素后插入新元素
        /// </summary>
        Task<long> InsertAfterAsync(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素后插入新元素（泛型）
        /// </summary>
        long InsertAfter<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 在指定元素后插入新元素（泛型）
        /// </summary>
        Task<long> InsertAfterAsync<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 通过索引设置元素值
        /// </summary>
        bool SetByIndex(string key, long index, string value);

        /// <summary>
        /// 通过索引设置元素值
        /// </summary>
        Task<bool> SetByIndexAsync(string key, long index, string value);

        /// <summary>
        /// 通过索引设置元素值（泛型）
        /// </summary>
        bool SetByIndex<T>(string key, long index, T value) where T : class;

        /// <summary>
        /// 通过索引设置元素值（泛型）
        /// </summary>
        Task<bool> SetByIndexAsync<T>(string key, long index, T value) where T : class;

        /// <summary>
        /// 获取列表长度
        /// </summary>
        long Length(string key);

        /// <summary>
        /// 获取列表长度
        /// </summary>
        Task<long> LengthAsync(string key);

        /// <summary>
        /// 移除指定数量的匹配元素
        /// </summary>
        long Remove(string key, long count, string value);

        /// <summary>
        /// 移除指定数量的匹配元素
        /// </summary>
        Task<long> RemoveAsync(string key, long count, string value);

        /// <summary>
        /// 保留指定区间元素，丢弃其余
        /// </summary>
        bool Trim(string key, long start, long stop);

        /// <summary>
        /// 保留指定区间元素，丢弃其余
        /// </summary>
        Task<bool> TrimAsync(string key, long start, long stop);

        /// <summary>
        /// 从源列表尾部弹出，推入目标列表头部
        /// </summary>
        string PopLastPushFirst(string sourceKey, string targetKey);

        /// <summary>
        /// 从源列表尾部弹出，推入目标列表头部
        /// </summary>
        Task<string> PopLastPushFirstAsync(string sourceKey, string targetKey);
    }
}
