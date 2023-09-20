using System;
using static CSRedis.CSRedisClient;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisEventMessage
    {
        /// <summary>
        /// 发布
        /// 无论是集群或普通模式,client.Publish 都能正常通信
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="data"></param>
        void Publish<T>(string channel, T data);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        SubscribeObject SubscribeEvent(string channel, Action<SubscribeMessageEventArgs> action);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        SubscribeObject Subscribe(string channel, Action<string> action);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        SubscribeObject Subscribe<T>(string channel, Action<T> action);

        /// <summary>
        /// 模式订阅（通配符）
        /// 模式订阅已经解决的难题：
        /// 1、集群的节点匹配规则,导致通配符最大可能匹配全部节点,所以全部节点都要订阅
        /// 2、本组 "test*", "*test001", "test*002" 订阅全部节点时,需要解决同一条消息不可执行多次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        PSubscribeObject PSubscribe(Action<PSubscribePMessageEventArgs> action, params string[] channels);
    }
}
