using System;
using System.Threading.Tasks;

namespace EventBus2MQ.SubscriberTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MqClient mqClient = new();

            TestSubscriberA testSubscriberA = new();
            TestSubscriberB testSubscriberB = new();
            await mqClient.AddSubscriber(testSubscriberA,testSubscriberB);
           
            mqClient.Startup();

            Console.ReadLine();
        }
    }
}
