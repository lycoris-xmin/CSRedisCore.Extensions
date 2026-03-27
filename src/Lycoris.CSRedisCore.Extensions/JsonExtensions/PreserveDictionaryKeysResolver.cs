using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lycoris.CSRedisCore.Extensions.JsonExtensions
{
    internal class PreserveDictionaryKeysResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            // 如果是 Dictionary 类型，保持 key 原样
            if (typeof(System.Collections.IDictionary).IsAssignableFrom(prop.PropertyType))
            {
                prop.PropertyName = prop.UnderlyingName; // 保持原始名字
            }
            else
            {
                // 其它类属性可以 camelCase
                prop.PropertyName = !string.IsNullOrEmpty(prop.PropertyName)
                    ? char.ToLowerInvariant(prop.PropertyName[0]) + prop.PropertyName.Substring(1)
                    : prop.PropertyName; // 如果为 null 或空，保持原样
            }

            return prop;
        }
    }
}
