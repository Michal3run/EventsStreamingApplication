using EventAppCommon.Message;
using System;

namespace EventAppCommon.Queue
{
    public interface IQueueMessageProducer : IDisposable
    {
        void ProduceMessage(ProducerMessage message);
    }
}
