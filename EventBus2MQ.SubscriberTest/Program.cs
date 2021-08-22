using System;
using System.Threading.Tasks;

namespace EventBus2MQ.SubscriberTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MqClient mqClient = new MqClient();

            TestSubscriberA testSubscriberA = new TestSubscriberA();

            await mqClient.AddSubscriber(testSubscriberA);
           
            mqClient.Startup();

            Console.ReadLine();
        }
    }
}
