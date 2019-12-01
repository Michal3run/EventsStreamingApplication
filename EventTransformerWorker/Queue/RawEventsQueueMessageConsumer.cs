using EventsAppCommon;
using EventsAppCommon.Queue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EventsTransformer.Queue
{
    public class RawEventsQueueMessageConsumer : IQueueMessageConsumer
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly ISingleMessageConsumer singleMessageConsumer;
        private readonly IBasicConsumer basicConsumer;

        public RawEventsQueueMessageConsumer()
        {
            var factory = new ConnectionFactory { HostName = QueueConfiguration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            DeclareQueue();
            singleMessageConsumer = new SingleRawEventMessageConsumer();
            basicConsumer = GetBasicConsumer(singleMessageConsumer);
        }

        private IBasicConsumer GetBasicConsumer(ISingleMessageConsumer singleMessageConsumer)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                singleMessageConsumer.ConsumeMessage(message);

                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //manual ack, after processing message
            };

            return consumer;
        }

        private void DeclareQueue()
        {
            channel.QueueDeclare(QueueConfiguration.RawEventsQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void StartConsuming()
        {
            channel.BasicConsume(queue: QueueConfiguration.RawEventsQueueName,
                autoAck: true, //false if we want to BasicAck manually after processing message
                consumer: basicConsumer);
        }

        public void Dispose()
        {
            connection.Close();
        }       
    }
}
