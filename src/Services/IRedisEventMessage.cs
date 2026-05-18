using System;
using static CSRedis.CSRedisClient;

namespace Lycoris.CSRedisCore.Extensions.Services
{
    /// <summary>
    /// Redis 发布/订阅接口，提供消息发布、频道订阅、模式订阅（通配符）等功能
    /// </summary>
    public interface IRedisEventMessage
    {
        /// <summary>
        /// 向指定频道发布消息。
        /// 无论是集群还是普通模式，都能正常通信。
        /// </summary>
        /// <typeparam name="T">消息的类型</typeparam>
        /// <param name="channel">频道名</param>
        /// <param name="data">消息数据</param>
        void Publish<T>(string channel, T data);

        /// <summary>
        /// 订阅指定频道，收到消息时触发回调
        /// </summary>
        /// <param name="channel">频道名</param>
        /// <param name="action">消息处理回调</param>
        /// <returns>订阅对象，可用于取消订阅</returns>
        SubscribeObject SubscribeEvent(string channel, Action<SubscribeMessageEventArgs> action);

        /// <summary>
        /// 订阅指定频道，收到原始字符串消息时触发回调
        /// </summary>
        /// <param name="channel">频道名</param>
        /// <param name="action">消息处理回调</param>
        /// <returns>订阅对象，可用于取消订阅</returns>
        SubscribeObject Subscribe(string channel, Action<string> action);

        /// <summary>
        /// 订阅指定频道，收到消息时自动反序列化并触发回调
        /// </summary>
        /// <typeparam name="T">消息的目标类型</typeparam>
        /// <param name="channel">频道名</param>
        /// <param name="action">消息处理回调</param>
        /// <returns>订阅对象，可用于取消订阅</returns>
        SubscribeObject Subscribe<T>(string channel, Action<T> action);

        /// <summary>
        /// 模式订阅（支持通配符），匹配的频道消息都会触发回调。
        /// 已解决集群节点的匹配规则问题，同一条消息不会重复执行多次。
        /// </summary>
        /// <param name="action">消息处理回调</param>
        /// <param name="channels">频道模式数组</param>
        /// <returns>模式订阅对象，可用于取消订阅</returns>
        PSubscribeObject PSubscribe(Action<PSubscribePMessageEventArgs> action, params string[] channels);

    }
}
