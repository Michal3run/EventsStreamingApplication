using EventAppCommon.Log;
using EventAppCommon.Queue;
using EventTransformer;
using System;
using System.Threading;

namespace EventTransformerConsoleApp
{
    class Program
    {
        private static IQueueMessageConsumer MessageConsumer => new RawEventsQueueMessageConsumer();

        private static ILogger Logger => new ConsoleLogger();

        static void Main(string[] args)
        {
            Console.WriteLine("Consumer started. Waiting for messages.");

            using (var consumer = MessageConsumer)
            {
                var cancellationTokenSource = new CancellationTokenSource();
                consumer.StartConsuming();
                Console.ReadKey();
                cancellationTokenSource.Cancel();
                Console.WriteLine("Streaming was cancelled. Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
