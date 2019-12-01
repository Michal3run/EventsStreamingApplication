using EventsAppCommon;
using EventsAppCommon.Message;
using EventsAppCommon.Queue;
using RabbitMQ.Client;
using System.Text;

namespace ApiConnector
{
    public class RawEventsQueueMessageProducer : IQueueMessageProducer
    {
        private readonly IConnection connection;
        private readonly IModel channel;

        public RawEventsQueueMessageProducer()
        {
            var factory = new ConnectionFactory { HostName = QueueConfiguration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            DeclareQueue();
        }

        private void DeclareQueue()
        {
            channel.QueueDeclare(QueueConfiguration.RawEventsQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        
        public void ProduceMessage(ProducerMessage eventMessage)
        {
            var body = Encoding.UTF8.GetBytes(eventMessage.Content);

            channel.BasicPublish(exchange: "",
                                 routingKey: QueueConfiguration.RawEventsQueueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            connection.Close();
        }        
    }
}
