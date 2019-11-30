using EventAppCommon;
using EventAppCommon.Message;
using EventAppCommon.Queue;
using RabbitMQ.Client;
using System.Text;

namespace EventTransformerWorker.Queue
{
    public class AggregatedEventsMessageProducer : IQueueMessageProducer
    {
        private readonly IConnection connection;
        private readonly IModel channel;

        public AggregatedEventsMessageProducer()
        {
            var factory = new ConnectionFactory { HostName = QueueConfiguration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            DeclareQueue();
        }

        private void DeclareQueue()
        {
            channel.QueueDeclare(QueueConfiguration.AggregatedTopicsQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        
        public void ProduceMessage(ProducerMessage eventMessage)
        {
            var body = Encoding.UTF8.GetBytes(eventMessage.Content);

            channel.BasicPublish(exchange: "",
                                 routingKey: QueueConfiguration.AggregatedTopicsQueueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            connection.Close();
        }        
    }
}
