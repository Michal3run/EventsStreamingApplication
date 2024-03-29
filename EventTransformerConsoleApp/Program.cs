﻿using EventsAppCommon.Log;
using EventsAppCommon.Queue;
using EventsTransformer.Queue;
using EventsTransformerWorker.Managers;
using System;
using System.Threading;

namespace EventsTransformerConsoleApp
{
    class Program
    {
        private static IQueueMessageConsumer MessageConsumer => new RawEventsQueueMessageConsumer();

        private static AggregatedEventsPublishingManager AggregatedMessageProducer => new AggregatedEventsPublishingManager();

        private static ILogger Logger => new ConsoleLogger();

        static void Main(string[] args)
        {
            Console.WriteLine("Consumer started. Waiting for messages.");

            using (var consumer = MessageConsumer)
            using (var producer = AggregatedMessageProducer)
            {
                var cancellationTokenSource = new CancellationTokenSource();
                consumer.StartConsuming();
                producer.StartPublishingAggregatedEvents();
                Console.ReadKey();
                cancellationTokenSource.Cancel();
                Console.WriteLine("Streaming was cancelled. Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
