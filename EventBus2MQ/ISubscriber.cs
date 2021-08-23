using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    // 订阅者接口
    public interface ISubscriber
    {

        string QueueName { get; }
        string EventName { get; }

        /// <summary>
        /// 处理订阅消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        Task ProcessData(string Message);
    }
}
