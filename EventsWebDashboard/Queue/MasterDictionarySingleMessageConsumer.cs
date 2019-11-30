using EventAppCommon.Queue;
using EventsWebDashboard.Models;
using EventTransformer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventsWebDashboard.Queue
{
    public class MasterDictionarySingleMessageConsumer : ISingleMessageConsumer
    {
        private readonly IMasterDictionaryManager _dictionaryManager;

        public MasterDictionarySingleMessageConsumer(IMasterDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }
        
        public void ConsumeMessage(string message)
        {
            var deserializedDict = GetDeserializedMessage(message);
            _dictionaryManager.UpdateDictionary(deserializedDict);
        }

        private Dictionary<string, EventDictValue> GetDeserializedMessage(string content) 
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, EventDictValue>>(content);
            }
            catch (Exception e)
            {
                throw new Exception($"Can't deserializer content: {content}. Error: {e}");
            }
        }
    }
}
