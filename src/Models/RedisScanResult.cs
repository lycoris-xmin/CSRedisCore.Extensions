namespace Lycoris.CSRedisCore.Extensions.Models
{
    /// <summary>
    /// SCAN 游标迭代结果
    /// </summary>
    /// <typeparam name="T">迭代项类型</typeparam>
    public class RedisScanResult<T>
    {
        /// <summary>
        /// 下次迭代的游标，0 表示迭代结束
        /// </summary>
        public long Cursor { get; set; }

        /// <summary>
        /// 本次迭代返回的项
        /// </summary>
        public T Items { get; set; }
    }
}
