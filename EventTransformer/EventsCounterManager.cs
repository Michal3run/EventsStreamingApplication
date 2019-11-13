using EventTransformer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventTransformer
{
    public class EventsCounterManager
    {
        private readonly Dictionary<string, EventDictValue> _groupTopicDictionary = new Dictionary<string, EventDictValue>(); //ConcurrentDictionary? only one thread per application

        #region Singleton

        private static EventsCounterManager _instance;
        private static object _lock = new object();

        public static EventsCounterManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EventsCounterManager();
                        }
                    }
                }

                return _instance;
            }
        }

        private EventsCounterManager()
        {
        }

        #endregion;

        public void AddToDictionary(string groupTopic)
        {
            if (groupTopic == null)
                throw new Exception("GroupTopic is null!");

            EventDictValue groupTopicValue;
            if (_groupTopicDictionary.TryGetValue(groupTopic, out groupTopicValue) == false)
            {
                groupTopicValue = new EventDictValue();
                _groupTopicDictionary.Add(groupTopic, groupTopicValue);
            }

            groupTopicValue.Count++;
        }

        public Dictionary<string, EventDictValue> GetOrderedTopicCountDictionary()
            => _groupTopicDictionary.OrderByDescending(t => t.Value.Count).ToDictionary(t => t.Key, t => t.Value);

        public void CleanDictionary() => _groupTopicDictionary.Clear();
    }
}
