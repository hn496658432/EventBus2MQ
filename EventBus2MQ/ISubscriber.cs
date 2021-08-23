using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    // 订阅者接口
    public interface ISubscriber
    {
        /// <summary>
        /// 订阅者队列
        /// </summary>
        string QueueName { get; }
        /// <summary>
        /// 订阅事件名称
        /// </summary>
        string EventName { get; }
        /// <summary>
        /// 是否丢弃处理错误消息
        /// </summary>
        bool IsDiscardErrorData { get; }
        /// <summary>
        /// 处理订阅消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        Task ProcessData(string Message);
        /// <summary>
        /// 业务发生错误处理
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        Task ErrorHandler(Exception ex, string Message);
    }
}
