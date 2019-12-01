using System;

namespace EventsAppCommon.Queue
{
    public interface IQueueMessageConsumer : IDisposable
    {
        void StartConsuming();
    }
}
