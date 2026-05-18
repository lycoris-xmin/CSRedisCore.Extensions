using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis List 类型操作接口，提供列表的头尾插入/弹出、索引访问、区间查询、裁剪等操作
    /// </summary>
    public interface IRedisListCache
    {
        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>第一个元素的值，不存在则返回 null</returns>
        string GetAndRemoveFirst(string key);

        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>第一个元素的值，不存在则返回 null</returns>
        Task<string> GetAndRemoveFirstAsync(string key);

        /// <summary>
        /// 移出并获取列表的第一个元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>第一个元素的值，不存在则返回 null</returns>
        T GetAndRemoveFirst<T>(string key) where T : class;

        /// <summary>
        /// 移出并获取列表的第一个元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>第一个元素的值，不存在则返回 null</returns>
        Task<T> GetAndRemoveFirstAsync<T>(string key) where T : class;

        /// <summary>
        /// 移出并获取列表的最后一个元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>最后一个元素的值，不存在则返回 null</returns>
        string GetAndRemoveLast(string key);

        /// <summary>
        /// 移出并获取列表的最后一个元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>最后一个元素的值，不存在则返回 null</returns>
        Task<string> GetAndRemoveLastAsync(string key);

        /// <summary>
        /// 移出并获取列表的最后一个元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>最后一个元素的值，不存在则返回 null</returns>
        T GetAndRemoveLast<T>(string key) where T : class;

        /// <summary>
        /// 移出并获取列表的最后一个元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>最后一个元素的值，不存在则返回 null</returns>
        Task<T> GetAndRemoveLastAsync<T>(string key) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        void SetFirst(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表首位
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        /// <returns>异步操作</returns>
        Task SetFirstAsync(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表首位，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        void SetFirst<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表首位，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        /// <returns>异步操作</returns>
        Task SetFirstAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        void SetLast(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表末尾
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        /// <returns>异步操作</returns>
        Task SetLastAsync(string key, params string[] value);

        /// <summary>
        /// 将一个或多个值插入到列表末尾，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        void SetLast<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 将一个或多个值插入到列表末尾，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要插入的值数组</param>
        /// <returns>异步操作</returns>
        Task SetLastAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 获取列表的所有元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>所有元素的列表</returns>
        Task<List<string>> GetAllAsync(string key);

        /// <summary>
        /// 获取列表的所有元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>所有元素的列表</returns>
        List<string> GetAll(string key);

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>元素值，不存在则返回 null</returns>
        string GetByIndex(string key, long index);

        /// <summary>
        /// 通过索引获取元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>元素值，不存在则返回 null</returns>
        Task<string> GetByIndexAsync(string key, long index);

        /// <summary>
        /// 通过索引获取元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>元素值，不存在则返回 null</returns>
        T GetByIndex<T>(string key, long index) where T : class;

        /// <summary>
        /// 通过索引获取元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <returns>元素值，不存在则返回 null</returns>
        Task<T> GetByIndexAsync<T>(string key, long index) where T : class;

        /// <summary>
        /// 获取指定区间的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>元素列表</returns>
        List<string> GetRange(string key, long start, long stop);

        /// <summary>
        /// 获取指定区间的元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>元素列表</returns>
        Task<List<string>> GetRangeAsync(string key, long start, long stop);

        /// <summary>
        /// 获取指定区间的元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>元素列表</returns>
        List<T> GetRange<T>(string key, long start, long stop) where T : class;

        /// <summary>
        /// 获取指定区间的元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>元素列表</returns>
        Task<List<T>> GetRangeAsync<T>(string key, long start, long stop) where T : class;

        /// <summary>
        /// 在指定元素前插入新元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        long InsertBefore(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素前插入新元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        Task<long> InsertBeforeAsync(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素前插入新元素，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        long InsertBefore<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 在指定元素前插入新元素，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        Task<long> InsertBeforeAsync<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 在指定元素后插入新元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        long InsertAfter(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素后插入新元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        Task<long> InsertAfterAsync(string key, string pivot, string value);

        /// <summary>
        /// 在指定元素后插入新元素，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        long InsertAfter<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 在指定元素后插入新元素，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="pivot">基准元素</param>
        /// <param name="value">要插入的值</param>
        /// <returns>插入后列表的长度</returns>
        Task<long> InsertAfterAsync<T>(string key, string pivot, T value) where T : class;

        /// <summary>
        /// 通过索引设置元素值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        bool SetByIndex(string key, long index, string value);

        /// <summary>
        /// 通过索引设置元素值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetByIndexAsync(string key, long index, string value);

        /// <summary>
        /// 通过索引设置元素值，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        bool SetByIndex<T>(string key, long index, T value) where T : class;

        /// <summary>
        /// 通过索引设置元素值，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="index">索引位置</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetByIndexAsync<T>(string key, long index, T value) where T : class;

        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表长度</returns>
        long Length(string key);

        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>列表长度</returns>
        Task<long> LengthAsync(string key);

        /// <summary>
        /// 移除指定数量的匹配元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">移除数量，正数从头移除，负数从尾移除，0 移除所有</param>
        /// <param name="value">要移除的值</param>
        /// <returns>实际移除的元素数量</returns>
        long Remove(string key, long count, string value);

        /// <summary>
        /// 移除指定数量的匹配元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">移除数量，正数从头移除，负数从尾移除，0 移除所有</param>
        /// <param name="value">要移除的值</param>
        /// <returns>实际移除的元素数量</returns>
        Task<long> RemoveAsync(string key, long count, string value);

        /// <summary>
        /// 保留指定区间元素，丢弃其余
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>是否操作成功</returns>
        bool Trim(string key, long start, long stop);

        /// <summary>
        /// 保留指定区间元素，丢弃其余
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>是否操作成功</returns>
        Task<bool> TrimAsync(string key, long start, long stop);

        /// <summary>
        /// 从源列表尾部弹出，推入目标列表头部（阻塞）
        /// </summary>
        /// <param name="sourceKey">源列表键</param>
        /// <param name="targetKey">目标列表键</param>
        /// <returns>弹出的元素值</returns>
        string PopLastPushFirst(string sourceKey, string targetKey);

        /// <summary>
        /// 从源列表尾部弹出，推入目标列表头部（阻塞）
        /// </summary>
        /// <param name="sourceKey">源列表键</param>
        /// <param name="targetKey">目标列表键</param>
        /// <returns>弹出的元素值</returns>
        Task<string> PopLastPushFirstAsync(string sourceKey, string targetKey);
    }
}
