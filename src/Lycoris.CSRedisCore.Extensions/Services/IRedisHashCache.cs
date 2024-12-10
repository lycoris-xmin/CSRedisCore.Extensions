using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisHashCache
    {
        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieIds"></param>
        /// <returns></returns>
        long Remove(string key, params string[] fieIds);

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieIds"></param>
        /// <returns></returns>
        Task<long> RemoveAsync(string key, params string[] fieIds);

        /// <summary>
        /// 查看哈希表 key 中,指定的字段是否存在。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        bool Exists(string key, string fieId);

        /// <summary>
        /// 查看哈希表 key 中,指定的字段是否存在。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key, string fieId);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        string Get(string key, string fieId);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        T Get<T>(string key, string fieId) where T : class;

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key, string fieId);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, string fieId) where T : class;

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieIds"></param>
        /// <returns></returns>
        List<string> Get(string key, params string[] fieIds);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieIds"></param>
        /// <returns></returns>
        Task<List<string>> GetAsync(string key, params string[] fieIds);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieIds"></param>
        /// <returns></returns>
        List<T> Get<T>(string key, params string[] fieIds) where T : class;

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieIds"></param>
        /// <returns></returns>
        Task<List<T>> GetAsync<T>(string key, params string[] fieIds) where T : class;

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> GetAll(string key);

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, T> GetAll<T>(string key) where T : class;

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetAllAsync(string key);

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, T>> GetAllAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<string> FieIds(string key);

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<string>> FieIdsAsync(string key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long FieIdsCount(string key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> FieIdsCountAsync(string key);

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool Set(string key, params (string, string)[] value);

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool Set<T>(string key, params (string, T)[] value) where T : class;

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, params (string, string)[] value);

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetAsync<T>(string key, params (string, T)[] value) where T : class;

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value 。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
        bool Set(string key, string fieId, string value);

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value 。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
        bool Set<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value 。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
        Task<bool> SetAsync(string key, string fieId, string value);

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value 。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
        Task<bool> SetAsync<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 只有在字段 field 不存在时,设置哈希表字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetNx(string key, string fieId, string value);

        /// <summary>
        /// 只有在字段 field 不存在时,设置哈希表字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetNx<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 只有在字段 field 不存在时,设置哈希表字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetNxAsync(string key, string fieId, string value);

        /// <summary>
        /// 只有在字段 field 不存在时,设置哈希表字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetNxAsync<T>(string key, string fieId, T value) where T : class;

        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        long Addition(string key, string fieId, long value = 1, TimeSpan? timeSpan = null);

        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        Task<long> AdditionAsync(string key, string fieId, long value = 1, TimeSpan? timeSpan = null);
    }
}
