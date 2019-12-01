using EventsWebDashboard.Models;
using EventsTransformer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EventsWebDashboard.Managers
{
    public class MasterDictionaryManager : IMasterDictionaryManager
    {
        private readonly object _locker = new object();

        private readonly ConcurrentDictionary<string, EventDictValue> _groupTopicDictionary = new ConcurrentDictionary<string, EventDictValue>(); //ConcurrentDictionary? only one thread per application
        
        public void UpdateDictionary(Dictionary<string, EventDictValue> partialDict)
        {
            if (partialDict == null)
                throw new Exception("PartialDict is null!");

            foreach(var topicItem in partialDict)
            {
                var eventDictValue = _groupTopicDictionary.GetOrAdd(topicItem.Key, new EventDictValue());
                Interlocked.Add(ref eventDictValue.Count, topicItem.Value.Count);
            }
        }

        public Dictionary<string, int> GetOrderedTopicCountDictionary()
        {
            var clonedDict = new Dictionary<string, EventDictValue>(_groupTopicDictionary);
            var orderedDictionary = clonedDict.OrderByDescending(t => t.Value.Count).ToDictionary(k => k.Key, k => k.Value.Count);
            return orderedDictionary;
            
        }

        public void CleanDictionary() => _groupTopicDictionary.Clear();
    }
}
