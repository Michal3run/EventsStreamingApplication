using EventAppCommon.Queue;
using EventTransformer.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace EventTransformer
{
    public class SingleRawEventMessageConsumer : ISingleMessageConsumer
    {
        public void ConsumeMessage(string message)
        {
            var rawEventModel = JsonConvert.DeserializeObject<RawEventModel>(message);

            Console.WriteLine($"Consumed Message. Id: {rawEventModel.member.member_id},  Name: {rawEventModel.member.member_name}");

            //TODO TRANSFORM AND PASS TO NEW QUEUE?

            var transformedEventModel = GetTransformedEventModel(rawEventModel);

            transformedEventModel.GroupTopicNames.ForEach(topicName =>
                EventsCounterManager.Instance.AddToDictionary(topicName)
            );
        }

        private TransformedEventModel GetTransformedEventModel(RawEventModel rawEventModel)
        {
            return new TransformedEventModel
            {
                EventName = rawEventModel.@event.event_name,
                EventTime = rawEventModel.@event.time,
                GroupTopicNames = rawEventModel.group.group_topics.Select(t => t.topic_name).ToList()
            };
        }

        private DateTime GetDateTime(int unixTime)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            return dateTimeOffset.DateTime;
        }
    }
}
