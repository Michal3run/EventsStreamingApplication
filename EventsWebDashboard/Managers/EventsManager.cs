using EventsWebDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsWebDashboard.Managers
{
    public class EventsManager : IEventsManager
    {
        private readonly IMasterDictionaryManager dictionaryManager;

        public EventsManager(IMasterDictionaryManager dictionaryManager)
        {
            this.dictionaryManager = dictionaryManager;
        }

        public List<TopicModel> GetBestEventTopics(int numberToTake)
        {
            var topicCountDictionary = dictionaryManager.GetOrderedTopicCountDictionary();
            return topicCountDictionary.Take(numberToTake).Select(t => new TopicModel { TopicName = t.Key, Count = t.Value }).ToList();
        }

        public string CleanEvents()
        {
            try
            {
                dictionaryManager.CleanDictionary();
            }
            catch (Exception e)
            {
                return $"Error during cleaning! {e}";
            }

            return "Cleaning finished with success";            
        }
    }
}
