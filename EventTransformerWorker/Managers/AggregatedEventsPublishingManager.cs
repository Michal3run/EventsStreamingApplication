using EventsAppCommon.Message;
using EventsAppCommon.Queue;
using EventsTransformer.Managers;
using EventsTransformerWorker.Queue;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventsTransformerWorker.Managers
{
    public class AggregatedEventsPublishingManager : IDisposable
    {
        private bool _stopPublishing;
        private IQueueMessageProducer _messageProducer = new AggregatedEventsMessageProducer();
        private readonly int _publishingIntervalInMiliseconds;

        public AggregatedEventsPublishingManager(int publishingIntervalInMiliseconds = 5000)
        {
            _publishingIntervalInMiliseconds = publishingIntervalInMiliseconds;
        }

        /// <summary>
        /// Publishes summary message (aggregated info about topic) every interval
        /// </summary>
        public void StartPublishingAggregatedEvents()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(_publishingIntervalInMiliseconds);
                    if (_stopPublishing)
                    {
                        _messageProducer = null;
                        return;
                    }

                    var producerMessage = GetSummaryMessage();
                    _messageProducer.ProduceMessage(producerMessage);                    
                }
            });
        }

        private ProducerMessage GetSummaryMessage()
        {
            var summaryDict = EventsCounterManager.Instance.GetSummaryAndCleanSourceDict();
            var serializedDict = JsonConvert.SerializeObject(summaryDict);
            var producerMessage = CreateMessage(serializedDict);
            return producerMessage;
        }

        private ProducerMessage CreateMessage(string content)
        {
            return new ProducerMessage
            {
                Content = content
            };
        }

        public void Dispose()
        {
            _stopPublishing = true;            
        }
    }
}
