using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBus2MQ
{
    public static class MqClientExtend
    {
        public static void AddQueueBind(this IModel channel, ConsumerDefualt consumer)
        {
            var queue = channel.QueueDeclare(
                    consumer.ConsumerName,
                    consumer.Durable.GetValueOrDefault(true),
                    consumer.Exclusive.GetValueOrDefault(false),
                    consumer.AutoDelete.GetValueOrDefault(false));
            channel.QueueBind(queue, consumer.EventName, "");
        }
    }
}