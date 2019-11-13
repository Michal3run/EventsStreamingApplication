using EventTransformer.Models;
using System.Collections.Generic;

namespace EventsWebDashboard.Models
{
    public interface IMasterDictionaryManager
    {
        void UpdateDictionary(Dictionary<string, EventDictValue> partialDict);

        Dictionary<string, int> GetOrderedTopicCountDictionary();

        void CleanDictionary();
    }
}
