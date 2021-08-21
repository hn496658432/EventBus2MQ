using RabbitMQ.Client;
using System.Threading.Tasks;

namespace EventBus2MQ
{
    /// <summary>
    /// 客户端配置
    /// </summary>
    public class MqClientConfig
    {
        public static IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "192.168.137.50"
            };
            return factory.CreateConnection();
            
        }
    }
}
