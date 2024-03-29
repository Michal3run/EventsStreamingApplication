﻿using EventsAppCommon;
using EventsAppCommon.Queue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EventsWebDashboard.Managers
{
    public class AggregatedEventsMessageConsumer : IQueueMessageConsumer
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly IBasicConsumer basicConsumer;

        public AggregatedEventsMessageConsumer(ISingleMessageConsumer singleMessageConsumer)
        {
            var factory = new ConnectionFactory { HostName = QueueConfiguration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            DeclareQueue();
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
            channel.QueueDeclare(QueueConfiguration.AggregatedTopicsQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void StartConsuming()
        {
            channel.BasicConsume(queue: QueueConfiguration.AggregatedTopicsQueueName,
                autoAck: true, //false if we want to BasicAck manually after processing message
                consumer: basicConsumer);
        }

        public void Dispose()
        {
            connection.Close();
        }       
    }
}
