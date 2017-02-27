using DoubanCrawler.Core.DbGroup;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawlerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var durable = true;
                    channel.QueueDeclare("DoubanGroupQueue", durable, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("DoubanGroupQueue", false, consumer);
                     
                    Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        var topic = JsonConvert.DeserializeObject<Topic>(message);

                        Console.WriteLine(" [x] Received {0}", topic.Title);
                          
                        Console.WriteLine(" [x] Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
