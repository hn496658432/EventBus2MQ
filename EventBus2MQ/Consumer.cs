using System;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class Consumer
    {
        /// <summary>
        /// 接收的事件
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 消费者名称
        /// </summary>
        public string ConsumerName { get; set; }
        /// <summary>
        /// 处理线程
        /// </summary>
        public string EventProcesser { get; set; }
        /// <summary>
        /// 在服务器重启时，能够存活
        /// </summary>
        public bool? Durable { get; set; }
        /// <summary>
        /// 是否为当前连接的专用队列，在连接断开后，会自动删除该队列
        /// </summary>
        public bool? Exclusive { get; set; }
        /// <summary>
        /// 当没有任何消费者使用时，自动删除该队列
        /// </summary>
        public bool? AutoDelete { get; set; }

    }
}
