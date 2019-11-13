using RabbitMQ.Client;
using System;
using System.Text;

namespace ApiConnector.RabbitMQTests
{
    public class ExchangeProducer
    {
        public static void ProduceExchange()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
                //channel.BasicQos(0, 2, false); //only one message delivered at once                

                for (int i = 1; i <= 50; ++i)
                {
                    string message = $"Message{i}!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "direct_logs",
                                         routingKey: i % 2 == 0 ? "low" : "high",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}

