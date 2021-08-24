using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ.SubscriberTest
{
    public class TestSubscriberB : ISubscriber
    {
        /// <summary>
        /// 消费队列
        /// </summary>
        public string QueueName => "test2";
        /// <summary>
        /// 订阅事件
        /// </summary>
        public string EventName => "amq.direct";

        public bool IsDiscardErrorData => false;

        public int TrySize { get; set; } = 3;

        public async Task<bool> ErrorHandler(Exception ex, string Message)
        {
            Console.WriteLine("Error:{0}\r\nBody:{1}",ex.Message,Message);

            return true;
        }

        /// <summary>
        /// 订阅者业务处理
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public Task ProcessData(string Message)
        {
            Console.WriteLine("{0}收到消息：{1}", QueueName, Message);
//#if DEBUG
//            throw new Exception("预设业务处理错误！");
//#endif
            return Task.CompletedTask;
        }
    }
}
