namespace EventAppCommon.Queue
{
    /// <summary>
    /// Performs actions on received message
    /// </summary>
    public interface ISingleMessageConsumer
    {
        void ConsumeMessage(string message);
    }
}
