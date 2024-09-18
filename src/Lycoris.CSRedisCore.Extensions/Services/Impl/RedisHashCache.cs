using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class RedisHashCache : IRedisHashCache
	{
		private readonly CSRedisClient CSRedisCore;
		private readonly JsonSerializerSettings JsonSetting;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="CSRedisCore"></param>
		/// <param name="JsonSetting"></param>
		public RedisHashCache(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
		{
			this.CSRedisCore = CSRedisCore;
			this.JsonSetting = JsonSetting;
		}

		/// <summary>
		/// 删除一个或多个哈希表字段
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieIds"></param>
		/// <returns></returns>
		public long Remove(string key, params string[] fieIds) => CSRedisCore.HDel(key, fieIds);

		/// <summary>
		/// 删除一个或多个哈希表字段
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieIds"></param>
		/// <returns></returns>
		public async Task<long> RemoveAsync(string key, params string[] fieIds) => await CSRedisCore.HDelAsync(key, fieIds);

		/// <summary>
		/// 查看哈希表 key 中,指定的字段是否存在。
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <returns></returns>
		public bool Exists(string key, string fieId) => CSRedisCore.HExists(key, fieId);

		/// <summary>
		/// 查看哈希表 key 中,指定的字段是否存在。
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <returns></returns>
		public async Task<bool> ExistsAsync(string key, string fieId) => await CSRedisCore.HExistsAsync(key, fieId);

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <returns></returns>
		public string Get(string key, string fieId) => CSRedisCore.HGet(key, fieId);

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <returns></returns>
		public T Get<T>(string key, string fieId) where T : class
		{
			var cache = CSRedisCore.HGet(key, fieId);
			if (string.IsNullOrEmpty(cache))
				return default;

			return JsonConvert.DeserializeObject<T>(cache);
		}

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <returns></returns>
		public async Task<string> GetAsync(string key, string fieId) => await CSRedisCore.HGetAsync(key, fieId);

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <returns></returns>
		public async Task<T> GetAsync<T>(string key, string fieId) where T : class
		{
			var cache = await CSRedisCore.HGetAsync(key, fieId);
			if (string.IsNullOrEmpty(cache))
				return default;

			return JsonConvert.DeserializeObject<T>(cache);
		}

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieIds"></param>
		/// <returns></returns>
		public List<string> Get(string key, params string[] fieIds)
		{
			var cache = CSRedisCore.HMGet(key, fieIds);
			return cache?.ToList() ?? new List<string>();
		}

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieIds"></param>
		/// <returns></returns>
		public async Task<List<string>> GetAsync(string key, params string[] fieIds)
		{
			var cache = await CSRedisCore.HMGetAsync(key, fieIds);
			return cache?.ToList() ?? new List<string>();
		}

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieIds"></param>
		/// <returns></returns>
		public List<T> Get<T>(string key, params string[] fieIds) where T : class
		{
			var cache = CSRedisCore.HMGet(key, fieIds);
			if (cache == null || cache.Length == 0)
				return null;

			var result = new List<T>();

			foreach (var item in cache)
			{
				if (!string.IsNullOrEmpty(item))
					result.Add(JsonConvert.DeserializeObject<T>(item));
				else
					result.Add(default);
			}

			return result;
		}

		/// <summary>
		/// 获取存储在哈希表中指定字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieIds"></param>
		/// <returns></returns>
		public async Task<List<T>> GetAsync<T>(string key, params string[] fieIds) where T : class
		{
			var cache = await CSRedisCore.HMGetAsync(key, fieIds);
			if (cache == null || cache.Length == 0)
				return null;

			var result = new List<T>();

			foreach (var item in cache)
			{
				if (!string.IsNullOrEmpty(item))
					result.Add(JsonConvert.DeserializeObject<T>(item));
				else
					result.Add(default);
			}

			return result;
		}

		/// <summary>
		/// 获取在哈希表中指定 key 的所有字段和值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Dictionary<string, string> GetAll(string key) => CSRedisCore.HGetAll(key);

		/// <summary>
		/// 获取在哈希表中指定 key 的所有字段和值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Dictionary<string, T> GetAll<T>(string key) where T : class
		{
			var cache = CSRedisCore.HGetAll(key);
			if (cache == null || cache.Count == 0)
				return null;

			var dic = new Dictionary<string, T>();
			foreach (var item in cache)
			{
				if (!string.IsNullOrEmpty(item.Value))
					dic.Add(item.Key, JsonConvert.DeserializeObject<T>(item.Value));
				else
					dic.Add(item.Key, default);

			}

			return dic;
		}

		/// <summary>
		/// 获取在哈希表中指定 key 的所有字段和值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<Dictionary<string, string>> GetAllAsync(string key) => await CSRedisCore.HGetAllAsync(key);

		/// <summary>
		/// 获取在哈希表中指定 key 的所有字段和值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<Dictionary<string, T>> GetAllAsync<T>(string key) where T : class
		{
			var cache = await CSRedisCore.HGetAllAsync(key);
			if (cache == null || cache.Count == 0)
				return null;

			var dic = new Dictionary<string, T>();
			foreach (var item in cache)
			{
				if (!string.IsNullOrEmpty(item.Value))
					dic.Add(item.Key, JsonConvert.DeserializeObject<T>(item.Value));
				else
					dic.Add(item.Key, default);
			}

			return dic;
		}

		/// <summary>
		/// 获取所有哈希表中的字段
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public List<string> FieIds(string key) => CSRedisCore.HKeys(key).ToList();

		/// <summary>
		/// 获取所有哈希表中的字段
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<List<string>> FieIdsAsync(string key) => (await CSRedisCore.HKeysAsync(key)).ToList();

		/// <summary>
		/// 获取哈希表中字段的数量
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public long FieIdsCount(string key) => CSRedisCore.HLen(key);

		/// <summary>
		/// 获取哈希表中字段的数量
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<long> FieIdsCountAsync(string key) => await CSRedisCore.HLenAsync(key);

		/// <summary>
		/// 同时将多个 field-value (域-值)对设置到哈希表 key 中
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Set(string key, params (string, string)[] value)
		{
			var keyValues = new List<object>();

			foreach (var item in value)
			{
				keyValues.Add(item.Item1);
				keyValues.Add(item.Item2);
			}

			return CSRedisCore.HMSet(key, keyValues.ToArray());
		}

		/// <summary>
		/// 同时将多个 field-value (域-值)对设置到哈希表 key 中
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Set<T>(string key, params (string, T)[] value) where T : class
		{
			var keyValues = new List<object>();

			foreach (var item in value)
			{
				keyValues.Add(item.Item1);
				keyValues.Add(JsonConvert.SerializeObject(item.Item2));
			}

			return CSRedisCore.HMSet(key, keyValues.ToArray());
		}

		/// <summary>
		/// 将哈希表 key 中的字段 field 的值设为 value 。
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
		public bool Set(string key, string fieId, string value) 
		{
			return CSRedisCore.HSet(key, fieId, value);
		}

		/// <summary>
		/// 将哈希表 key 中的字段 field 的值设为 value 。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
		public bool Set<T>(string key, string fieId, T value) where T : class
        {
            var json = JsonConvert.SerializeObject(value, JsonSetting);
            return CSRedisCore.HSet(key, fieId, json);
        }

		/// <summary>
		/// 同时将多个 field-value (域-值)对设置到哈希表 key 中
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public async Task<bool> SetAsync(string key, params (string, string)[] value)
		{
			var keyValues = new List<object>();

			foreach (var item in value)
			{
				keyValues.Add(item.Item1);
				keyValues.Add(item.Item2);
			}

			return await CSRedisCore.HMSetAsync(key, keyValues.ToArray());
		}

		/// <summary>
		/// 同时将多个 field-value (域-值)对设置到哈希表 key 中
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public async Task<bool> SetAsync<T>(string key, params (string, T)[] value) where T : class
		{
			var keyValues = new List<object>();

			foreach (var item in value)
			{
				keyValues.Add(item.Item1);
				keyValues.Add(item.Item2 != null ? JsonConvert.SerializeObject(item.Item2) : "");
			}

			return await CSRedisCore.HMSetAsync(key, keyValues.ToArray());
		}

		/// <summary>
		/// 将哈希表 key 中的字段 field 的值设为 value 。
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
		public Task<bool> SetAsync(string key, string fieId, string value) => CSRedisCore.HSetAsync(key, fieId, value);

		/// <summary>
		/// 将哈希表 key 中的字段 field 的值设为 value 。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns>如果字段是哈希表中的一个新建字段,并且值设置成功,返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖,返回false。</returns>
		public async Task<bool> SetAsync<T>(string key, string fieId, T value) where T : class
		{
			var json = JsonConvert.SerializeObject(value, JsonSetting);
			return await CSRedisCore.HSetAsync(key, fieId, json);
		}

		/// <summary>
		/// 只有在字段 field 不存在时,设置哈希表字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SetNx(string key, string fieId, string value)
			=> CSRedisCore.HSetNx(key, fieId, value);

		/// <summary>
		/// 只有在字段 field 不存在时,设置哈希表字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SetNx<T>(string key, string fieId, T value) where T : class
		{
			var json = JsonConvert.SerializeObject(value, JsonSetting);
			return CSRedisCore.HSetNx(key, fieId, json);
		}

		/// <summary>
		/// 只有在字段 field 不存在时,设置哈希表字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public Task<bool> SetNxAsync(string key, string fieId, string value) => CSRedisCore.HSetNxAsync(key, fieId, value);

		/// <summary>
		/// 只有在字段 field 不存在时,设置哈希表字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public async Task<bool> SetNxAsync<T>(string key, string fieId, T value) where T : class
		{
			var json = JsonConvert.SerializeObject(value, JsonSetting);
			return await CSRedisCore.HSetNxAsync(key, fieId, json);
		}

		/// <summary>
		/// 为哈希表 key 中的指定字段的整数值加上增量
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <param name="timeSpan">过期时间</param>
		/// <returns></returns>
		public long Addition(string key, string fieId, long value = 1, TimeSpan? timeSpan = null) => CSRedisCore.HIncrBy(key, fieId, value);

		/// <summary>
		/// 为哈希表 key 中的指定字段的整数值加上增量
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieId"></param>
		/// <param name="value"></param>
		/// <param name="timeSpan">过期时间</param>
		/// <returns></returns>
		public async Task<long> AdditionAsync(string key, string fieId, long value = 1, TimeSpan? timeSpan = null) => await CSRedisCore.HIncrByAsync(key, fieId, value);
	}
}
