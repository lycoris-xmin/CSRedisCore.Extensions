using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisSortCache
    {
        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long Count(string key);

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> CountAsync(string key);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        long Add(string key, string member, decimal source);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<long> AddAsync(string key, string member, decimal source);

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        long Remove(string key, params string[] member);

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long> RemoveAsync(string key, params string[] member);

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        long RemoveRange(string key, long start, long stop);

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<long> RemoveRangeAsync(string key, long start, long stop);

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        decimal Addition(string key, string member, decimal value = 1);

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<decimal> AdditionAsync(string key, string member, decimal value = 1);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string[] GetRangeBySource(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<string[]> GetRangeBySourceAsync(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Dictionary<string, decimal> GetRangeBySourceWithScores(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Dictionary<T, decimal> GetRangeBySourceWithScores<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<Dictionary<string, decimal>> GetRangeBySourceWithScoresAsync(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<Dictionary<T, decimal>> GetRangeBySourceWithScoresAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Dictionary<string, decimal> GetRevRangeByScoreWithScores(string key, decimal max, decimal min, long? count = null, long offset = 0);

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Dictionary<T, decimal> GetRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<Dictionary<string, decimal>> GetRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? count = null, long offset = 0);

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<Dictionary<T, decimal>> GetRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] Max(string key, long count = 1);

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] Max<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<string[]> MaxAsync(string key, long count = 1);

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        ///  当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<T[]> MaxAsync<T>(string key, long count = 1);

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] Min(string key, long count = 1);

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] Min<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<string[]> MinAsync(string key, long count = 1);

        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。
        /// 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<T[]> MinAsync<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long? Rank(string key, string member);

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long?> RankAsync(string key, string member);

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        decimal? GetScore(string key, string member);

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<decimal?> GetScoreAsync(string key, string member);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, decimal>> GetAllAsync(string key);
    }
}
