using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawler.Core.Mq
{
    public interface IMqService
    {
        void Produce(string message);
    }

    public class MqService : IMqService
    {
        public void Produce(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    bool durable = true;
                    channel.QueueDeclare("DoubanGroupQueue", durable, false, false, null);

                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(true); //持久化，

                    channel.BasicPublish("", "DoubanGroupQueue", null, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

    }
}
