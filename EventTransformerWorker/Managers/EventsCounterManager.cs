using EventTransformer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventTransformer.Managers
{
    /// <summary>
    /// Stores 
    /// </summary>
    public class EventsCounterManager
    {
        private readonly Dictionary<string, EventDictValue> _groupTopicDictionary = new Dictionary<string, EventDictValue>(); //ConcurrentDictionary?

        private readonly object _locker = new object();

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

            lock (_locker)
            {
                EventDictValue groupTopicValue;
                if (_groupTopicDictionary.TryGetValue(groupTopic, out groupTopicValue) == false)
                {
                    groupTopicValue = new EventDictValue();
                    _groupTopicDictionary.Add(groupTopic, groupTopicValue);
                }

                groupTopicValue.Count++;
            }
        }

        /// <summary>
        /// GetOrderedTopicCountDictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, EventDictValue> GetSummaryAndCleanSourceDict()
        {
            lock (_locker) //lock because, when reading from dictionary, and cleaning it, new items cannot be added
            {
                var result =  _groupTopicDictionary.OrderByDescending(t => t.Value.Count).ToDictionary(t => t.Key, t => t.Value);
                _groupTopicDictionary.Clear();
                return result;
            }
        }
    }
}
