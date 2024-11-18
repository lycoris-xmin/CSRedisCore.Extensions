using Newtonsoft.Json;
using System.Text;

namespace Lycoris.CSRedisCore.Extensions.Options
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisConiguration
    {
        /// <summary>
        /// redis 服务地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// redis 服务端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// redis 服务地址
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// redis服务密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// redis 库配置 默认：<see langword="0"/>
        /// </summary>
        public int UseDatabase { get; set; } = 0;

        /// <summary>
        /// 是否异步方式自动使用pipeline 默认：<see langword="false"/>
        /// </summary>
        public bool AsyncPipeline { get; set; } = false;

        /// <summary>
        /// 连接池
        /// </summary>
        public int Poolsize { get; set; } = 50;

        /// <summary>
        /// 连接池（MS）元素空闲时间(单位：毫秒) 默认：<see langword="20000"/>
        /// </summary>
        public int IdleTimeout { get; set; } = 20000;

        /// <summary>
        /// 连接超时时间(单位：毫秒) 默认：<see langword="5000"/>
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;

        /// <summary>
        /// 发送/接收超时时间(单位：毫秒) 默认：<see langword="10000"/>
        /// </summary>
        public int SyncTimeout { get; set; } = 10000;

        /// <summary>
        /// 是否跟随系统退出事件自动释放 默认：<see langword="true"/>
        /// </summary>
        public bool AutoDispose { get; set; } = true;

        /// <summary>
        /// 是否启用加密传输 默认：<see langword="false"/>
        /// </summary>
        public bool SSL { get; set; } = false;

        /// <summary>
        /// 是否尝试集群模式，阿里云、腾讯云集群需要设置此选项为 false  默认：<see langword="false"/>
        /// </summary>
        public bool TestCluster { get; set; } = false;

        /// <summary>
        /// 缓存键前缀 所有方法都会附带此前辍
        /// </summary>
        public string Prefix { get; set; } = null;

        /// <summary>
        /// 失败重试次数 默认：<see langword="0"/>
        /// </summary>
        public int RetryOnFailure { get; set; } = 0;

        /// <summary>
        /// 连接池预热连接数 默认：<see langword="0"/>
        /// </summary>
        public int Preheat { get; set; } = 0;

        /// <summary>
        /// Json序列化配置项
        /// </summary>
        public JsonSerializerSettings NewtonsoftJsonSerializerSettings { get; set; } = null;

        /// <summary>
        /// 使用哨兵模式
        /// </summary>
        /// <param name="sentinels"></param>
        public void UseSentinels(params string[] sentinels)
        {
            if (sentinels == null || sentinels.Length == 0)
                return;
            Sentinels = sentinels;
        }

        /// <summary>
        /// 哨兵
        /// </summary>
        internal string[] Sentinels { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        internal bool HasSentinels { get => Sentinels != null && Sentinels.Length > 0; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (HasSentinels)
                sb.Append("mymaster,");
            else
                sb.AppendFormat("{0}:{1},", Host, Port);

            if (!string.IsNullOrEmpty(UserName))
                sb.AppendFormat("user={0},", UserName);

            sb.AppendFormat("password={0},", Password);
            sb.AppendFormat("defaultDatabase={0},", UseDatabase);
            sb.AppendFormat("asyncPipeline={0},", AsyncPipeline);
            sb.AppendFormat("poolsize={0},", Poolsize);
            sb.AppendFormat("idleTimeout={0},", IdleTimeout);
            sb.AppendFormat("connectTimeout={0},", ConnectTimeout);
            sb.AppendFormat("syncTimeout={0}", SyncTimeout);
            sb.AppendFormat("autoDispose={0}", AutoDispose);
            sb.AppendFormat("ssl={0},", SSL);
            sb.AppendFormat("testcluster={0},", TestCluster);
            sb.AppendFormat("tryit={0},", RetryOnFailure);

            if (!string.IsNullOrEmpty(Prefix))
                sb.AppendFormat("prefix={0},", Prefix);

            sb.AppendFormat("preheat={0}", Preheat > 0 ? Preheat.ToString() : "false");

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string ToString(int database)
        {
            var sb = new StringBuilder();

            if (HasSentinels)
                sb.Append("mymaster,");
            else
                sb.AppendFormat("{0}:{1},", Host, Port);

            if (!string.IsNullOrEmpty(UserName))
                sb.AppendFormat("user={0},", UserName);

            sb.AppendFormat("password={0},", Password);
            sb.AppendFormat("defaultDatabase={0},", database);
            sb.AppendFormat("asyncPipeline={0},", AsyncPipeline);
            sb.AppendFormat("poolsize={0},", Poolsize);
            sb.AppendFormat("idleTimeout={0},", IdleTimeout);
            sb.AppendFormat("connectTimeout={0},", ConnectTimeout);
            sb.AppendFormat("syncTimeout={0},", SyncTimeout);
            sb.AppendFormat("autoDispose={0},", AutoDispose);
            sb.AppendFormat("ssl={0},", SSL);
            sb.AppendFormat("testcluster={0},", TestCluster);
            sb.AppendFormat("tryit={0},", RetryOnFailure);

            if (!string.IsNullOrEmpty(Prefix))
                sb.AppendFormat("prefix={0},", Prefix);

            sb.AppendFormat("preheat={0}", Preheat > 0 ? Preheat.ToString() : "false");

            return sb.ToString();
        }
    }
}
