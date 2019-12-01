using ApiConnector;
using EventsAppCommon.Log;
using EventsAppCommon.Queue;
using System;
using System.Threading;

namespace ApiConnectorConsoleApp
{
    class Program
    {
        private const string ApiUrl = "http://stream.meetup.com/2/rsvps";

        private static IQueueMessageProducer MessageProducer => new RawEventsQueueMessageProducer();

        private static ILogger Logger => new ConsoleLogger();

        static void Main(string[] args)
        {
            using (var producer = MessageProducer)
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var connector = new ExternalApiStreamConnector(ApiUrl, producer, Logger, cancellationTokenSource.Token);
                connector.StartStreamingFromApi();
                Console.ReadKey();
                cancellationTokenSource.Cancel();
                Console.WriteLine("Streaming was cancelled. Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
