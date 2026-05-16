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
        Task<decimal?> GetScoreAsync(string key, string member);

        /// <summary>
        /// 获取有序集合的所有成员及分数
        /// </summary>
        Task<Dictionary<string, decimal>> GetAllAsync(string key);

        /// <summary>
        /// 获取有序集合的所有成员及分数
        /// </summary>
        Dictionary<string, decimal> GetAll(string key);

        /// <summary>
        /// 统计分数区间内的成员数量
        /// </summary>
        long CountByScore(string key, decimal min, decimal max);

        /// <summary>
        /// 统计分数区间内的成员数量
        /// </summary>
        Task<long> CountByScoreAsync(string key, decimal min, decimal max);

        /// <summary>
        /// 同时向有序集合添加多个成员
        /// </summary>
        long SetMultiple(string key, params (decimal, string)[] members);

        /// <summary>
        /// 同时向有序集合添加多个成员
        /// </summary>
        Task<long> SetMultipleAsync(string key, params (decimal, string)[] members);

        /// <summary>
        /// 按排名区间返回成员（升序）
        /// </summary>
        string[] GetRangeByRank(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员（升序）
        /// </summary>
        Task<string[]> GetRangeByRankAsync(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（升序）
        /// </summary>
        Dictionary<string, decimal> GetRangeByRankWithScores(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（升序）
        /// </summary>
        Task<Dictionary<string, decimal>> GetRangeByRankWithScoresAsync(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员（降序）
        /// </summary>
        string[] GetRevRangeByRank(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员（降序）
        /// </summary>
        Task<string[]> GetRevRangeByRankAsync(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（降序）
        /// </summary>
        Dictionary<string, decimal> GetRevRangeByRankWithScores(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（降序）
        /// </summary>
        Task<Dictionary<string, decimal>> GetRevRangeByRankWithScoresAsync(string key, long start, long stop);

        /// <summary>
        /// 移除指定分数区间的所有成员
        /// </summary>
        long RemoveByScore(string key, decimal min, decimal max);

        /// <summary>
        /// 移除指定分数区间的所有成员
        /// </summary>
        Task<long> RemoveByScoreAsync(string key, decimal min, decimal max);

        /// <summary>
        /// 返回指定成员升序排名（分数从小到大）
        /// </summary>
        long? RankAscending(string key, string member);

        /// <summary>
        /// 返回指定成员升序排名（分数从小到大）
        /// </summary>
        Task<long?> RankAscendingAsync(string key, string member);

        /// <summary>
        /// 迭代遍历有序集合成员
        /// </summary>
        Models.RedisScanResult<Dictionary<string, decimal>> Scan(string key, long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 迭代遍历有序集合成员
        /// </summary>
        Task<Models.RedisScanResult<Dictionary<string, decimal>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null);
    }
}
