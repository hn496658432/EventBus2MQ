// See https://aka.ms/new-console-template for more information

using EventBus2MQ;
using System;
using System.Threading.Tasks;

//MqClient mqClient = new MqClient();
class Program
{
    static async Task Main(string[] args)
    {

        await MqClient.StartupAsync();

        Console.ReadLine();
    }

}

