using CSRedis;
using Newtonsoft.Json;
using System;
using static CSRedis.CSRedisClient;

namespace Lycoris.CSRedisCore.Extensions.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisEventMessage : IRedisEventMessage
    {
        private readonly CSRedisClient CSRedisCore;
        private readonly JsonSerializerSettings JsonSetting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CSRedisCore"></param>
        /// <param name="JsonSetting"></param>
        public RedisEventMessage(CSRedisClient CSRedisCore, JsonSerializerSettings JsonSetting)
        {
            this.CSRedisCore = CSRedisCore;
            this.JsonSetting = JsonSetting;
        }

        /// <summary>
        /// 发布
        /// 无论是集群或普通模式,client.Publish 都能正常通信
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="data"></param>
        public void Publish<T>(string channel, T data) => CSRedisCore.Publish(channel, JsonConvert.SerializeObject(data, JsonSetting));

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        /// <returns>可停止的订阅对象</returns>
        public SubscribeObject SubscribeEvent(string channel, Action<SubscribeMessageEventArgs> action) => CSRedisCore.Subscribe((channel, msg => action(msg)));

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        /// <returns>可停止的订阅对象</returns>
        public SubscribeObject Subscribe(string channel, Action<string> action) => CSRedisCore.Subscribe((channel, msg => action(msg.Body)));

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        /// <returns>可停止的订阅对象</returns>
        public SubscribeObject Subscribe<T>(string channel, Action<T> action)
        {
            void handler(SubscribeMessageEventArgs msg)
            {
                T obj = default;
                if (!string.IsNullOrEmpty(msg.Body))
                    obj = JsonConvert.DeserializeObject<T>(msg.Body, JsonSetting);

                action(obj);
            }

            return CSRedisCore.Subscribe((channel, handler));
        }

        /// <summary>
        /// 模糊订阅（通配符）
        /// 模糊订阅已经解决的难题：
        /// 1、集群的节点匹配规则,导致通配符最大可能匹配全部节点,所以全部节点都要订阅
        /// 2、本组 "test*", "*test001", "test*002" 订阅全部节点时,需要解决同一条消息不可执行多次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        public PSubscribeObject PSubscribe(Action<PSubscribePMessageEventArgs> action, params string[] channels) => CSRedisCore.PSubscribe(channels, msg => action(msg));
    }
}
