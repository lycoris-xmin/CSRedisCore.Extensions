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
        /// 获取集合的所有元素
        /// </summary>
        Task<List<string>> GetAllAsync(string setKey);

        /// <summary>
        /// 获取集合的所有元素
        /// </summary>
        List<string> GetAll(string setKey);

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        long Remove(string key, params string[] value);

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        Task<long> RemoveAsync(string key, params string[] value);

        /// <summary>
        /// 移除集合中一个或多个成员（泛型）
        /// </summary>
        long Remove<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 移除集合中一个或多个成员（泛型）
        /// </summary>
        Task<long> RemoveAsync<T>(string key, params T[] value) where T : class;

        /// <summary>
        /// 随机获取成员（不移除）
        /// </summary>
        string RandomMember(string key);

        /// <summary>
        /// 随机获取成员（不移除）
        /// </summary>
        Task<string> RandomMemberAsync(string key);

        /// <summary>
        /// 随机获取成员（不移除）
        /// </summary>
        T RandomMember<T>(string key) where T : class;

        /// <summary>
        /// 随机获取成员（不移除）
        /// </summary>
        Task<T> RandomMemberAsync<T>(string key) where T : class;

        /// <summary>
        /// 随机获取多个成员（不移除）
        /// </summary>
        string[] RandomMembers(string key, int count);

        /// <summary>
        /// 随机获取多个成员（不移除）
        /// </summary>
        Task<string[]> RandomMembersAsync(string key, int count);

        /// <summary>
        /// 将成员从源集合移动到目标集合
        /// </summary>
        bool Move(string sourceKey, string targetKey, string value);

        /// <summary>
        /// 将成员从源集合移动到目标集合
        /// </summary>
        Task<bool> MoveAsync(string sourceKey, string targetKey, string value);

        /// <summary>
        /// 多个集合的差集
        /// </summary>
        string[] Difference(params string[] keys);

        /// <summary>
        /// 多个集合的差集
        /// </summary>
        Task<string[]> DifferenceAsync(params string[] keys);

        /// <summary>
        /// 差集并存储到新集合
        /// </summary>
        long DifferenceStore(string targetKey, params string[] keys);

        /// <summary>
        /// 差集并存储到新集合
        /// </summary>
        Task<long> DifferenceStoreAsync(string targetKey, params string[] keys);

        /// <summary>
        /// 多个集合的交集
        /// </summary>
        string[] Intersection(params string[] keys);

        /// <summary>
        /// 多个集合的交集
        /// </summary>
        Task<string[]> IntersectionAsync(params string[] keys);

        /// <summary>
        /// 交集并存储到新集合
        /// </summary>
        long IntersectionStore(string targetKey, params string[] keys);

        /// <summary>
        /// 交集并存储到新集合
        /// </summary>
        Task<long> IntersectionStoreAsync(string targetKey, params string[] keys);

        /// <summary>
        /// 多个集合的并集
        /// </summary>
        string[] Union(params string[] keys);

        /// <summary>
        /// 多个集合的并集
        /// </summary>
        Task<string[]> UnionAsync(params string[] keys);

        /// <summary>
        /// 并集并存储到新集合
        /// </summary>
        long UnionStore(string targetKey, params string[] keys);

        /// <summary>
        /// 并集并存储到新集合
        /// </summary>
        Task<long> UnionStoreAsync(string targetKey, params string[] keys);

        /// <summary>
        /// 迭代遍历集合中的成员
        /// </summary>
        Models.RedisScanResult<string[]> Scan(string key, long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 迭代遍历集合中的成员
        /// </summary>
        Task<Models.RedisScanResult<string[]>> ScanAsync(string key, long cursor, string pattern = null, long? count = null);
    }
}
