// See https://aka.ms/new-console-template for more information

using EventBus2MQ;

//MqClient mqClient = new MqClient();

await MqClient.StartupAsync();

Console.ReadLine();
