using EventsAppCommon.Message;
using System;

namespace EventsAppCommon.Queue
{
    public interface IQueueMessageProducer : IDisposable
    {
        void ProduceMessage(ProducerMessage message);
    }
}
