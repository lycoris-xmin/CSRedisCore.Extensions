using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis Sorted Set 类型操作接口，提供有序集合成员的增删改查、分数增减、排名查询、区间遍历等操作
    /// </summary>
    public interface IRedisSortCache
    {
        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员数量</returns>
        long Count(string key);

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员数量</returns>
        Task<long> CountAsync(string key);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="source">分数</param>
        /// <returns>成功添加的新成员数量（不含更新的已有成员）</returns>
        long Add(string key, string member, decimal source);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="source">分数</param>
        /// <returns>成功添加的新成员数量（不含更新的已有成员）</returns>
        Task<long> AddAsync(string key, string member, decimal source);

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">要移除的成员数组</param>
        /// <returns>成功移除的成员数量</returns>
        long Remove(string key, params string[] member);

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">要移除的成员数组</param>
        /// <returns>成功移除的成员数量</returns>
        Task<long> RemoveAsync(string key, params string[] member);

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成功移除的成员数量</returns>
        long RemoveRange(string key, long start, long stop);

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成功移除的成员数量</returns>
        Task<long> RemoveRangeAsync(string key, long start, long stop);

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">增量值</param>
        /// <returns>增加后的分数值</returns>
        decimal Addition(string key, string member, decimal value = 1);

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">增量值</param>
        /// <returns>增加后的分数值</returns>
        Task<decimal> AdditionAsync(string key, string member, decimal value = 1);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员数组</returns>
        string[] GetRangeBySource(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员数组</returns>
        Task<string[]> GetRangeBySourceAsync(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员及分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<string, decimal> GetRangeBySourceWithScores(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员及分数，成员反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<T, decimal> GetRangeBySourceWithScores<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员及分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<string, decimal>> GetRangeBySourceWithScoresAsync(string key, decimal min, decimal max, long? count = null, long offset = 0);

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员及分数，并反序列化成员为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<T, decimal>> GetRangeBySourceWithScoresAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<string, decimal> GetRevRangeByScoreWithScores(string key, decimal max, decimal min, long? count = null, long offset = 0);

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序，成员反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<T, decimal> GetRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<string, decimal>> GetRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? count = null, long offset = 0);

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序，成员反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="count">返回数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<T, decimal>> GetRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) where T : class;

        /// <summary>
        /// 删除并返回有序集合中得分最高的成员（redis-server 5.0.0+）。
        /// 返回多个元素时，得分最高的为第一个，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        string[] Max(string key, long count = 1);

        /// <summary>
        /// 删除并返回有序集合中得分最高的成员（redis-server 5.0.0+），并反序列化为指定类型。
        /// 返回多个元素时，得分最高的为第一个，然后是分数较低的元素。
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        T[] Max<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// 删除并返回有序集合中得分最高的成员（redis-server 5.0.0+）。
        /// 返回多个元素时，得分最高的为第一个，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        Task<string[]> MaxAsync(string key, long count = 1);

        /// <summary>
        /// 删除并返回有序集合中得分最高的成员（redis-server 5.0.0+），并反序列化为指定类型。
        /// 返回多个元素时，得分最高的为第一个，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        Task<T[]> MaxAsync<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// 删除并返回有序集合中得分最低的成员（redis-server 5.0.0+）。
        /// 返回多个元素时，得分最低的为第一个，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        string[] Min(string key, long count = 1);

        /// <summary>
        /// 删除并返回有序集合中得分最低的成员（redis-server 5.0.0+），并反序列化为指定类型。
        /// 返回多个元素时，得分最低的为第一个，然后是分数较高的元素。
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        T[] Min<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// 删除并返回有序集合中得分最低的成员（redis-server 5.0.0+）。
        /// 返回多个元素时，得分最低的为第一个，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        Task<string[]> MinAsync(string key, long count = 1);

        /// <summary>
        /// 删除并返回有序集合中得分最低的成员（redis-server 5.0.0+），并反序列化为指定类型。
        /// 返回多个元素时，得分最低的为第一个，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="count">返回数量</param>
        /// <returns>成员数组</returns>
        Task<T[]> MinAsync<T>(string key, long count = 1) where T : class;

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>排名，不存在则返回 null</returns>
        long? Rank(string key, string member);

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>排名，不存在则返回 null</returns>
        Task<long?> RankAsync(string key, string member);

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>分数值，不存在则返回 null</returns>
        decimal? GetScore(string key, string member);

        /// <summary>
        /// 返回指定成员的分数值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>分数值，不存在则返回 null</returns>
        Task<decimal?> GetScoreAsync(string key, string member);

        /// <summary>
        /// 获取有序集合的所有成员及分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<string, decimal>> GetAllAsync(string key);

        /// <summary>
        /// 获取有序集合的所有成员及分数
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<string, decimal> GetAll(string key);

        /// <summary>
        /// 统计分数区间内的成员数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>成员数量</returns>
        long CountByScore(string key, decimal min, decimal max);

        /// <summary>
        /// 统计分数区间内的成员数量
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>成员数量</returns>
        Task<long> CountByScoreAsync(string key, decimal min, decimal max);

        /// <summary>
        /// 同时向有序集合添加多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="members">分数与成员的元组数组</param>
        /// <returns>成功添加的新成员数量</returns>
        long SetMultiple(string key, params (decimal, string)[] members);

        /// <summary>
        /// 同时向有序集合添加多个成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="members">分数与成员的元组数组</param>
        /// <returns>成功添加的新成员数量</returns>
        Task<long> SetMultipleAsync(string key, params (decimal, string)[] members);

        /// <summary>
        /// 按排名区间返回成员（升序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员数组</returns>
        string[] GetRangeByRank(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员（升序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员数组</returns>
        Task<string[]> GetRangeByRankAsync(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（升序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<string, decimal> GetRangeByRankWithScores(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（升序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<string, decimal>> GetRangeByRankWithScoresAsync(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员（降序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员数组</returns>
        string[] GetRevRangeByRank(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员（降序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员数组</returns>
        Task<string[]> GetRevRangeByRankAsync(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（降序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        Dictionary<string, decimal> GetRevRangeByRankWithScores(string key, long start, long stop);

        /// <summary>
        /// 按排名区间返回成员及分数（降序）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始排名</param>
        /// <param name="stop">结束排名</param>
        /// <returns>成员与分数的字典</returns>
        Task<Dictionary<string, decimal>> GetRevRangeByRankWithScoresAsync(string key, long start, long stop);

        /// <summary>
        /// 移除指定分数区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>成功移除的成员数量</returns>
        long RemoveByScore(string key, decimal min, decimal max);

        /// <summary>
        /// 移除指定分数区间的所有成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <returns>成功移除的成员数量</returns>
        Task<long> RemoveByScoreAsync(string key, decimal min, decimal max);

        /// <summary>
        /// 返回指定成员升序排名（分数从小到大）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>排名，不存在则返回 null</returns>
        long? RankAscending(string key, string member);

        /// <summary>
        /// 返回指定成员升序排名（分数从小到大）
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <returns>排名，不存在则返回 null</returns>
        Task<long?> RankAscendingAsync(string key, string member);

        /// <summary>
        /// 迭代遍历有序集合成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次数据</returns>
        Models.RedisScanResult<Dictionary<string, decimal>> Scan(string key, long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 迭代遍历有序集合成员
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cursor">游标</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="count">每次返回数量</param>
        /// <returns>扫描结果，包含下一游标和当前批次数据</returns>
        Task<Models.RedisScanResult<Dictionary<string, decimal>>> ScanAsync(string key, long cursor, string pattern = null, long? count = null);
    }
}
