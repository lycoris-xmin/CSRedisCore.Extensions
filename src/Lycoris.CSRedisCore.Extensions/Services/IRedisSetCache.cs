using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis Set 类型操作接口，提供集合成员的增删改查、随机获取、集合运算（差集/交集/并集）等操作
    /// </summary>
    public interface IRedisSetCache
    {
        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要添加的成员数组</param>
        /// <returns>成功添加的成员数量</returns>
        long Set(string key, params string[] value);

        /// <summary>
        /// 向集合添加一个或多个成员，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要添加的成员数组</param>
        /// <returns>成功添加的成员数量</returns>
        long Set<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要添加的成员数组</param>
        /// <returns>成功添加的成员数量</returns>
        Task<long> SetAsync(string key, params string[] value);

        /// <summary>
        /// 向集合添加一个或多个成员，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要添加的成员数组</param>
        /// <returns>成功添加的成员数量</returns>
        Task<long> SetAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员数量</returns>
        long Count(string key);

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员数量</returns>
        Task<long> CountAsync(string key);

        /// <summary>
        /// 判断元素是否是集合的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要判断的元素</param>
        /// <returns>是否为成员</returns>
        bool Exists(string key, string value);

        /// <summary>
        /// 判断元素是否是集合的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要判断的元素</param>
        /// <returns>是否为成员</returns>
        Task<bool> ExistsAsync(string key, string value);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机元素值，集合为空则返回 null</returns>
        string GetRandom(string key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>随机元素值，集合为空则返回 null</returns>
        T GetRandom<T>(string key) where T : class;

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机元素值，集合为空则返回 null</returns>
        Task<string> GetRandomAsync(string key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>随机元素值，集合为空则返回 null</returns>
        Task<T> GetRandomAsync<T>(string key) where T : class;

        /// <summary>
        /// 获取集合的所有元素
        /// </summary>
        /// <param name="setKey">集合键</param>
        /// <returns>所有元素的列表</returns>
        Task<List<string>> GetAllAsync(string setKey);

        /// <summary>
        /// 获取集合的所有元素
        /// </summary>
        /// <param name="setKey">集合键</param>
        /// <returns>所有元素的列表</returns>
        List<string> GetAll(string setKey);

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员数组</param>
        /// <returns>成功移除的成员数量</returns>
        long Remove(string key, params string[] value);

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员数组</param>
        /// <returns>成功移除的成员数量</returns>
        Task<long> RemoveAsync(string key, params string[] value);

        /// <summary>
        /// 移除集合中一个或多个成员，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员数组</param>
        /// <returns>成功移除的成员数量</returns>
        long Remove<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 移除集合中一个或多个成员，值自动序列化
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">要移除的成员数组</param>
        /// <returns>成功移除的成员数量</returns>
        Task<long> RemoveAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 随机获取一个成员（不移除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员值，集合为空则返回 null</returns>
        string RandomMember(string key);

        /// <summary>
        /// 随机获取一个成员（不移除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员值，集合为空则返回 null</returns>
        Task<string> RandomMemberAsync(string key);

        /// <summary>
        /// 随机获取一个成员（不移除），并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员值，集合为空则返回 null</returns>
        T RandomMember<T>(string key) where T : class;

        /// <summary>
        /// 随机获取一个成员（不移除），并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>随机成员值，集合为空则返回 null</returns>
        Task<T> RandomMemberAsync<T>(string key) where T : class;

        /// <summary>
        /// 随机获取多个成员（不移除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">获取数量</param>
        /// <returns>随机成员数组</returns>
        string[] RandomMembers(string key, int count);

        /// <summary>
        /// 随机获取多个成员（不移除）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">获取数量</param>
        /// <returns>随机成员数组</returns>
        Task<string[]> RandomMembersAsync(string key, int count);

        /// <summary>
        /// 将成员从源集合移动到目标集合
        /// </summary>
        /// <param name="sourceKey">源集合键</param>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="value">要移动的成员</param>
        /// <returns>是否移动成功</returns>
        bool Move(string sourceKey, string targetKey, string value);

        /// <summary>
        /// 将成员从源集合移动到目标集合
        /// </summary>
        /// <param name="sourceKey">源集合键</param>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="value">要移动的成员</param>
        /// <returns>是否移动成功</returns>
        Task<bool> MoveAsync(string sourceKey, string targetKey, string value);

        /// <summary>
        /// 计算多个集合的差集
        /// </summary>
        /// <param name="keys">集合键数组</param>
        /// <returns>差集成员数组</returns>
        string[] Difference(params string[] keys);

        /// <summary>
        /// 计算多个集合的差集
        /// </summary>
        /// <param name="keys">集合键数组</param>
        /// <returns>差集成员数组</returns>
        Task<string[]> DifferenceAsync(params string[] keys);

        /// <summary>
        /// 计算差集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">源集合键数组</param>
        /// <returns>新集合的成员数</returns>
        long DifferenceStore(string targetKey, params string[] keys);

        /// <summary>
        /// 计算差集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">源集合键数组</param>
        /// <returns>新集合的成员数</returns>
        Task<long> DifferenceStoreAsync(string targetKey, params string[] keys);

        /// <summary>
        /// 计算多个集合的交集
        /// </summary>
        /// <param name="keys">集合键数组</param>
        /// <returns>交集成员数组</returns>
        string[] Intersection(params string[] keys);

        /// <summary>
        /// 计算多个集合的交集
        /// </summary>
        /// <param name="keys">集合键数组</param>
        /// <returns>交集成员数组</returns>
        Task<string[]> IntersectionAsync(params string[] keys);

        /// <summary>
        /// 计算交集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">源集合键数组</param>
        /// <returns>新集合的成员数</returns>
        long IntersectionStore(string targetKey, params string[] keys);

        /// <summary>
        /// 计算交集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">源集合键数组</param>
        /// <returns>新集合的成员数</returns>
        Task<long> IntersectionStoreAsync(string targetKey, params string[] keys);

        /// <summary>
        /// 计算多个集合的并集
        /// </summary>
        /// <param name="keys">集合键数组</param>
        /// <returns>并集成员数组</returns>
        string[] Union(params string[] keys);

        /// <summary>
        /// 计算多个集合的并集
        /// </summary>
        /// <param name="keys">集合键数组</param>
        /// <returns>并集成员数组</returns>
        Task<string[]> UnionAsync(params string[] keys);

        /// <summary>
        /// 计算并集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">源集合键数组</param>
        /// <returns>新集合的成员数</returns>
        long UnionStore(string targetKey, params string[] keys);

        /// <summary>
        /// 计算并集并存储到新集合
        /// </summary>
        /// <param name="targetKey">目标集合键</param>
        /// <param name="keys">源集合键数组</param>
        /// <returns>新集合的成员数</returns>
        Task<long> UnionStoreAsync(string targetKey, params string[] keys);

        /// <summary>
        /// 迭代遍历集合中的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次成员</returns>
        Models.RedisScanResult<string[]> Scan(string key, long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 迭代遍历集合中的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次成员</returns>
        Task<Models.RedisScanResult<string[]>> ScanAsync(string key, long cursor, string pattern = null, long? count = null);
    }
}
