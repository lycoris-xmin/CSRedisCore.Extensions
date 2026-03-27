using Lycoris.CSRedisCore.Extensions.JsonExtensions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lycoris.CSRedisCore.Extensions
{
    internal static class RedisStore
    {
        /// <summary>
        /// 
        /// </summary>
        internal static JsonSerializerSettings Settings { get; private set; } = new JsonSerializerSettings()
        {
            ContractResolver = new PreserveDictionaryKeysResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss.ffffff",
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>() { new DecimalStringConverter(), new LongStringConverter() },
            MaxDepth = 200
        };

        /// <summary>
        /// 
        /// </summary>
        internal static string PrefixCacheKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        internal static void SetPrefixCacheKey(string value)
        {
            if (string.IsNullOrEmpty(value))
                PrefixCacheKey = "";

            PrefixCacheKey = $"{value.TrimEnd(':')}:";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        internal static void SetJsonSerializerSettings(JsonSerializerSettings value)
        {
            if (value != null)
                Settings = value;
        }
    }
}
