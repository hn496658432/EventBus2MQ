using EventBus2MQ.Receivers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    public class MqClient
    {
        static readonly IConnection _conn = MqClientConfig.GetConnection();
        public static Task StartupAsync()
        {            
            IModel channel = _conn.CreateModel();

            // 控制台 "Ctrl + C" 和程序退出调用 Shutdown
            // Console.CancelKeyPress += async (s, e) => await ShutdownAsync();

            AppDomain.CurrentDomain.ProcessExit += async (s, e) => await ShutdownAsync();


            List<ConsumerDefualt> consumers = new List<ConsumerDefualt>()
            {
                new TestReceiverA("amq.direct","test1"),
                new TestReceiverB("amq.fanout","test2")
            };
            

            // 创建队列绑定
            consumers.ForEach(channel.AddQueueBind);

            // 为消费者加订阅事件
            consumers.ForEach(c =>
            {
               // using var channel_x = _conn.CreateModel();
                Console.WriteLine("开始监听:{0}" ,c.EventName);
                channel.BasicQos(0,5,false);
                var driver = new EventingBasicConsumer(channel);
                driver.Received += new EventHandler<BasicDeliverEventArgs>(async (sender, args) =>
                    await ConsumerWorking(channel, c, sender, args)
                );
            });



            return Task.CompletedTask;
        }

        public async static Task ConsumerWorking(IModel channel, IConsumer<string> consumer, object sender, BasicDeliverEventArgs args)
        {
            string message = string.Empty;

            try
            {
                var body = args.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
                await consumer.ProcessData(message);
                channel.BasicAck(args.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                if (args != null)
                {
                    if (consumer.IsDiscardErrorData)
                    {
                        channel.BasicAck(args.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(args.DeliveryTag, false, true);
                    }
                }
                await consumer.ErrorHandler(ex, message);
            }

        }

        public static Task ShutdownAsync()
        {
            Console.WriteLine("RabbitMq 进程退出.");
            lock (_conn)
            {
                if (_conn.IsOpen)
                {
                    _conn.Close();
                }
            }

            return Task.CompletedTask;
        }
    }
}
