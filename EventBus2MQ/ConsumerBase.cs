using System;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    public abstract class ConsumerDefualt : ConsumerBase<string>
    {
        protected ConsumerDefualt(
            string eventName, 
            string consumerName , 
            bool? durable = null,
            bool? exclusive= null, 
            bool? autoDelete = null)
        {
            EventName = eventName;
            ConsumerName = consumerName;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
        }


        /// <summary>
        /// 接收的事件
        /// </summary>
        public string EventName { get; set; }    
        /// <summary>
        /// 消费者名称
        /// </summary>
        public string ConsumerName { get; set; }
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

    public abstract class ConsumerBase<T> : IConsumer<T>
    {
        public virtual bool IsDiscardErrorData => false;

        public virtual Task ErrorHandler(Exception ex, T data)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(data);

            return Task.CompletedTask;
        }

        public abstract Task ProcessData(T data);
    }
}