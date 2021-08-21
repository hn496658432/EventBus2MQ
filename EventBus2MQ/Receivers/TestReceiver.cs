using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ.Receivers
{
    class TestReceiverA : ConsumerDefualt, IConsumer<string>
    {
        public TestReceiverA(
            string eventName,
            string consumerName,
            bool? durable = null,
            bool? exclusive = null,
            bool? autoDelete = null) : base(eventName, consumerName, durable, exclusive, autoDelete)
        {
        }

        public override Task ProcessData(string data)
        {

            Console.WriteLine("消费者A接受到消息：{0}", data);

            return Task.CompletedTask;
        }
    }

    class TestReceiverB : ConsumerDefualt, IConsumer<string>
    {
        public TestReceiverB(
            string eventName,
            string consumerName,
            bool? durable = null,
            bool? exclusive = null,
            bool? autoDelete = null) : base(eventName, consumerName, durable, exclusive, autoDelete)
        {
        }

        public override Task ProcessData(string data)
        {
            Console.WriteLine("消费者B接受到消息：{0}", data);

            return Task.CompletedTask;
        }
    }
}
