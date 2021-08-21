using System;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    public interface IConsumer<T>
    {
        /// <summary>
        /// 错误的数据是否丢弃
        /// </summary>
        bool IsDiscardErrorData { get; }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="jsondata"></param>
        Task ProcessData(T data);

        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="jsondata">正在处理的数据，有可能为空</param>
        Task ErrorHandler(Exception ex, T data);
    }
}
