using Newtonsoft.Json;
using System;

namespace Lycoris.CSRedisCore.Extensions.JsonExtensions
{
    /// <summary>
    /// decimal/decimal? 转字符串，仅影响输出（保留两位小数）
    /// </summary>
    internal class DecimalStringConverter : JsonConverter
    {
        /// <summary>
        /// 判断是否可转换 decimal / decimal?
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否可转换</returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(decimal) || objectType == typeof(decimal?);

        /// <summary>
        /// 序列化 decimal 为两位小数字符串
        /// </summary>
        /// <param name="writer">JSON 写入器</param>
        /// <param name="value">待序列化的值</param>
        /// <param name="serializer">序列化器</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            if (value is decimal dec)
                writer.WriteValue(dec.ToString("0.00"));
            else
                writer.WriteValue(value.ToString());
        }

        /// <summary>
        /// 禁止参与反序列化，返回默认值 0
        /// </summary>
        /// <param name="reader">JSON 读取器</param>
        /// <param name="objectType">目标类型</param>
        /// <param name="existingValue">现有值</param>
        /// <param name="serializer">序列化器</param>
        /// <returns>反序列化结果</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => existingValue ?? 0m;

        /// <summary>
        /// 是否支持反序列化读取
        /// </summary>
        public override bool CanRead => false;
    }
}
