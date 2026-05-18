using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis Hash 类型操作接口，提供哈希表字段的增删改查、计数器、遍历等操作
    /// </summary>
    public interface IRedisHashCache
    {
        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">要删除的字段名数组</param>
        /// <returns>被成功删除的字段数量</returns>
        long Remove(string key, params string[] fieIds);

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">要删除的字段名数组</param>
        /// <returns>被成功删除的字段数量</returns>
        Task<long> RemoveAsync(string key, params string[] fieIds);

        /// <summary>
        /// 查看哈希表 key 中指定的字段是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段是否存在</returns>
        bool Exists(string key, string fieId);

        /// <summary>
        /// 查看哈希表 key 中指定的字段是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段是否存在</returns>
        Task<bool> ExistsAsync(string key, string fieId);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段值，不存在则返回 null</returns>
        string Get(string key, string fieId);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段值，不存在则返回 null</returns>
        T Get<T>(string key, string fieId) where T : class;

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段值，不存在则返回 null</returns>
        Task<string> GetAsync(string key, string fieId);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字段值，不存在则返回 null</returns>
        Task<T> GetAsync<T>(string key, string fieId) where T : class;

        /// <summary>
        /// 批量获取存储在哈希表中多个指定字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">字段名数组</param>
        /// <returns>字段值列表，不存在的字段对应 null</returns>
        List<string> Get(string key, params string[] fieIds);

        /// <summary>
        /// 批量获取存储在哈希表中多个指定字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">字段名数组</param>
        /// <returns>字段值列表，不存在的字段对应 null</returns>
        Task<List<string>> GetAsync(string key, params string[] fieIds);

        /// <summary>
        /// 批量获取存储在哈希表中多个指定字段的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">字段名数组</param>
        /// <returns>字段值列表，不存在的字段对应 null</returns>
        List<T> Get<T>(string key, params string[] fieIds) where T : class;

        /// <summary>
        /// 批量获取存储在哈希表中多个指定字段的值，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieIds">字段名数组</param>
        /// <returns>字段值列表，不存在的字段对应 null</returns>
        Task<List<T>> GetAsync<T>(string key, params string[] fieIds) where T : class;

        /// <summary>
        /// 获取哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段名与值的字典</returns>
        Dictionary<string, string> GetAll(string key);

        /// <summary>
        /// 获取哈希表中指定 key 的所有字段和值，值反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>字段名与值的字典</returns>
        Dictionary<string, T> GetAll<T>(string key) where T : class;

        /// <summary>
        /// 获取哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段名与值的字典</returns>
        Task<Dictionary<string, string>> GetAllAsync(string key);

        /// <summary>
        /// 获取哈希表中指定 key 的所有字段和值，值反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>字段名与值的字典</returns>
        Task<Dictionary<string, T>> GetAllAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取所有哈希表中的字段名
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段名列表</returns>
        List<string> FieIds(string key);

        /// <summary>
        /// 获取所有哈希表中的字段名
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段名列表</returns>
        Task<List<string>> FieIdsAsync(string key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段数量</returns>
        long FieIdsCount(string key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>字段数量</returns>
        Task<long> FieIdsCountAsync(string key);

        /// <summary>
        /// 同时将多个 field-value 对设置到哈希表中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">字段名与值的元组数组</param>
        /// <returns>是否设置成功</returns>
        bool Set(string key, params (string, string)[] value);

        /// <summary>
        /// 同时将多个 field-value 对设置到哈希表中，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">字段名与值的元组数组</param>
        /// <returns>是否设置成功</returns>
        bool Set<T>(string key, params (string, T)[] value) where T : class;

        /// <summary>
        /// 同时将多个 field-value 对设置到哈希表中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">字段名与值的元组数组</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetAsync(string key, params (string, string)[] value);

        /// <summary>
        /// 同时将多个 field-value 对设置到哈希表中，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">字段名与值的元组数组</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetAsync<T>(string key, params (string, T)[] value) where T : class;

        /// <summary>
        /// 将哈希表中指定字段的值设为 value
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是新建字段且设置成功返回 true，如果字段已存在且旧值被覆盖返回 false</returns>
        bool Set(string key, string fieId, string value);

        /// <summary>
        /// 将哈希表中指定字段的值设为 value，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是新建字段且设置成功返回 true，如果字段已存在且旧值被覆盖返回 false</returns>
        bool Set<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 将哈希表中指定字段的值设为 value
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是新建字段且设置成功返回 true，如果字段已存在且旧值被覆盖返回 false</returns>
        Task<bool> SetAsync(string key, string fieId, string value);

        /// <summary>
        /// 将哈希表中指定字段的值设为 value，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是新建字段且设置成功返回 true，如果字段已存在且旧值被覆盖返回 false</returns>
        Task<bool> SetAsync<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 只有在字段不存在时设置哈希表字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        bool SetNx(string key, string fieId, string value);

        /// <summary>
        /// 只有在字段不存在时设置哈希表字段的值，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        bool SetNx<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 只有在字段不存在时设置哈希表字段的值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetNxAsync(string key, string fieId, string value);

        /// <summary>
        /// 只有在字段不存在时设置哈希表字段的值，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetNxAsync<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 为哈希表指定字段的整数值加上增量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">增量值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>增加后的值</returns>
        long Addition(string key, string fieId, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 为哈希表指定字段的整数值加上增量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">增量值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>增加后的值</returns>
        Task<long> AdditionAsync(string key, string fieId, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 为哈希表指定字段加上浮点增量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">浮点增量值</param>
        /// <returns>增加后的值</returns>
        decimal IncrByFloat(string key, string fieId, decimal value);

        /// <summary>
        /// 为哈希表指定字段加上浮点增量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <param name="value">浮点增量值</param>
        /// <returns>增加后的值</returns>
        Task<decimal> IncrByFloatAsync(string key, string fieId, decimal value);

        /// <summary>
        /// 获取哈希表中所有字段的值（不返回键名）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>值列表</returns>
        List<string> Values(string key);

        /// <summary>
        /// 获取哈希表中所有字段的值（不返回键名）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>值列表</returns>
        Task<List<string>> ValuesAsync(string key);

        /// <summary>
        /// 获取哈希表指定字段值的字符串长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字符串长度</returns>
        long FieldStringLength(string key, string fieId);

        /// <summary>
        /// 获取哈希表指定字段值的字符串长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fieId">字段名</param>
        /// <returns>字符串长度</returns>
        Task<long> FieldStringLengthAsync(string key, string fieId);

        /// <summary>
        /// 迭代遍历哈希表中的键值对
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次数据</returns>
        Models.RedisScanResult<Dictionary<string, string>> Scan(string key, long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 迭代遍历哈希表中的键值对
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次数据</returns>
        Task<Models.RedisScanResult<Dictionary<string, string>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null);
    }
}
