using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    /***************************************************************************
     *                      RabbitMq 发布、订阅 Demo
     * -------------------------------------------------------------------------
     * 
     * 参照官方文档建议使用 Connection 和 Channel，尽量复用这些资源
     * 
     * 
     * Auth:kl
     * Date:2021-08-23
     ***************************************************************************/
    public class MqClient
    {
        // 连接创建效率不高，为更好的复用，减少资源消耗，在全局声明连接
        IConnection? _connection;
        ICollection<ISubscriber>? _subscribers;

        /// <summary>
        /// 启动客户端
        /// </summary>
        public void Startup()
        {
            Console.WriteLine("MQ客户端已启动.");
            _connection = GetConnection();
            // 程序退出时调用关闭连接
            Console.CancelKeyPress += (sender, e) => Shutdown();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => Shutdown();

            // 将所有订阅者激活
            _subscribers?.ToList().ForEach(SubscriberActive);
        }

        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void Shutdown()
        {
            if (_connection != null)
            {
                Console.WriteLine("客户端即将退出...");
                lock (_connection)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
        }

        /// <summary>
        /// 添加订阅者（多个）
        /// </summary>
        /// <param name="subscribers"></param>
        /// <returns></returns>
        public Task AddSubscriber(params ISubscriber[] subscribers)
        {
            _subscribers ??= new List<ISubscriber>();
            subscribers.ToList().ForEach(_subscribers.Add);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 添加订阅者（单个）
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public Task AddSubscriber(ISubscriber subscriber)
        {
            _subscribers ??= new List<ISubscriber>();
            _subscribers.Add(subscriber);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 为订阅者添加接收消息事件
        /// </summary>
        /// <param name="subscriber"></param>
        private void SubscriberActive(ISubscriber subscriber)
        {
            IModel? channel;
            try
            {
                channel = _connection?.CreateModel() ?? throw new ArgumentNullException(nameof(channel));
                channel.BasicQos(0, 5, false);
                Console.WriteLine("{0}开始订阅：{1}", subscriber.QueueName, subscriber.EventName);
                channel.QueueDeclare(subscriber.QueueName, true, false, false);
                channel.QueueBind(subscriber.QueueName, subscriber.EventName, "");
                var listener = new AsyncEventingBasicConsumer(channel);
                listener.Received += (sender, args) => ListenerReceived(channel, subscriber, sender, args);
                channel.BasicConsume(subscriber.QueueName, false, listener);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}订阅{1}失败:{2}\r\n{3}",
                    subscriber.QueueName,
                    subscriber.EventName,
                    ex.Message,
                    ex.StackTrace);
            }
            //return Task.CompletedTask;
        }

        /// <summary>
        /// [响应订阅者接受消息事件]
        /// 调用订阅者处理业务方法，在拦截业务方法错误时，调用错误处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public static async Task ListenerReceived(
            IModel? channel,
            ISubscriber subscriber,
            object sender,
            BasicDeliverEventArgs eventArgs)
        {
            string _message = string.Empty;
            try
            {
                _message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
#pragma warning disable CS4014
                subscriber.ProcessData(_message).ContinueWith(async task =>
                {
                    if (channel == null)
                    {
                        throw new ArgumentNullException(nameof(channel));
                    }

                    if (task.IsCompleted && task.Exception == null)
                    {
                        Console.WriteLine("{0}业务处理完成！", subscriber.QueueName);
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    else if (task.Exception != null)
                    {

                        if (subscriber.IsDiscardErrorData)
                        {
                            Console.WriteLine("{0}业务处理失败！消息已丢弃！", subscriber.QueueName);
                            channel.BasicAck(eventArgs.DeliveryTag, false);
                        }
                        else
                        {
                            // 业务处理发生错误，并设置了不丢弃错误消息，延迟重试处理！
                            for (int i = 0; i < subscriber.TrySize; i++)
                            {
                                await Task.Delay(1000 * 1 * i);
                                Console.WriteLine("{0}业务处理失败！消息将尝试重新处理[{1}]！", subscriber.QueueName, i + 1);
                                var status = await subscriber.ErrorHandler(task.Exception, _message);
                                if (status)
                                {
                                    channel.BasicAck(eventArgs.DeliveryTag, false);
                                    break;
                                }
                            }
                            channel.BasicAck(eventArgs.DeliveryTag, false);
                            Console.WriteLine("业务处理重试了{0}次,消息已被丢弃",subscriber.TrySize);
                        }
                    }

                });
#pragma warning restore

            }
            catch (Exception)
            {
                // 事件回调异常
            }
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "192.168.137.50",
                VirtualHost = "/",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                DispatchConsumersAsync = true       // 启用异步订阅支持
            };
            return factory.CreateConnection();
        }
    }
}
