using EventsWebDashboard.Models;
using EventTransformer.Models;
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
            var test = new Dictionary<string, EventDictValue>
            {
                { "test1", new EventDictValue{ Count = 2} },
                { "test2", new EventDictValue{ Count = 5} },
                { "test3", new EventDictValue{ Count = 4} }
            };
            var test2 = new Dictionary<string, EventDictValue>
            {
                { "test1", new EventDictValue{ Count = 200} },
                { "test2", new EventDictValue{ Count = 500} },
                { "test4", new EventDictValue{ Count = 400} }
            };

            UpdateDictionary(test);
            UpdateDictionary(test2);

            var topicCountDictionary = dictionaryManager.GetOrderedTopicCountDictionary();
            return topicCountDictionary.Take(numberToTake).Select(t => new TopicModel { TopicName = t.Key, Count = t.Value }).ToList();
        }

        public void UpdateDictionary(Dictionary<string, EventDictValue> partialDict)
        {
            dictionaryManager.UpdateDictionary(partialDict);
        }

        //TODO queue wrapper

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
