using Newtonsoft.Json;
using System;

namespace Lycoris.CSRedisCore.Extensions.JsonExtensions
{
    /// <summary>
    /// long/long? → string，仅影响输出（防止前端精度丢失）
    /// </summary>
    internal class LongStringConverter : JsonConverter
    {
        /// <summary>
        /// 判断是否可转换 long / long?
        /// </summary>
        public override bool CanConvert(Type objectType) => objectType == typeof(long) || objectType == typeof(long?);

        /// <summary>
        /// 序列化（输出时 long 转 string）
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }

        /// <summary>
        /// 禁止参与反序列化，保持默认行为（即不影响接口入参）
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => existingValue ?? 0L;

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead => false;
    }
}
