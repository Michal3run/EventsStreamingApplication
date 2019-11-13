using System.Collections.Generic;

namespace EventsWebDashboard.Models
{
    public interface IEventsManager
    {
        List<TopicModel> GetBestEventTopics(int numberToTake);

        /// <summary>
        /// Returns message with result
        /// </summary>
        /// <returns></returns>
        string CleanEvents();
    }
}
