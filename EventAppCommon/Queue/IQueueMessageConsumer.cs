using System;

namespace EventAppCommon.Queue
{
    public interface IQueueMessageConsumer : IDisposable
    {
        void StartConsuming();
    }
}
