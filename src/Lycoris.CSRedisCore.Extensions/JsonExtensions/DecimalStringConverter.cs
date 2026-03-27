using Newtonsoft.Json;
using System;

namespace Lycoris.CSRedisCore.Extensions.JsonExtensions
{
    /// <summary>
    /// 
    /// </summary>
    internal class DecimalStringConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(decimal) || objectType == typeof(decimal?);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
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
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => existingValue ?? 0m;

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead => false;
    }
}
