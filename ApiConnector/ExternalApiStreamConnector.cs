using EventsAppCommon.Log;
using EventsAppCommon.Message;
using EventsAppCommon.Queue;
using System.IO;
using System.Net;
using System.Threading;

namespace ApiConnector
{
    public class ExternalApiStreamConnector
    {
        private readonly string apiUrl;
        private readonly IQueueMessageProducer producer;
        private readonly ILogger logger;
        private readonly CancellationToken cancellationToken;

        public ExternalApiStreamConnector(string apiUrl, IQueueMessageProducer producer, ILogger logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.apiUrl = apiUrl;
            this.producer = producer;
            this.logger = logger;
            this.cancellationToken = cancellationToken;
        }
        
        public void StartStreamingFromApi()
        {
            var request = WebRequest.Create(apiUrl);
            request.Method = "GET";

            request.BeginGetResponse(ar =>
            {
                var req = (WebRequest)ar.AsyncState;
                using (var response = req.EndGetResponse(ar))
                using (var reader = new StreamReader(response.GetResponseStream()))
                {                    
                    ReadFromApi(reader);
                }

            }, request);
        }

        private void ReadFromApi(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var apiMessage = reader.ReadLine();

                PerformActionsOnClientResponse(apiMessage);

                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }

        private void PerformActionsOnClientResponse(string apiMessage)
        {
            logger.LogInfo(apiMessage);
            var message = CreateMessage(apiMessage);
            producer.ProduceMessage(message);
        }

        private ProducerMessage CreateMessage(string content)
        {
            return new ProducerMessage
            {
                Content = content
            };
        }
    }
}
