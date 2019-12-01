using System.Collections.Generic;

namespace EventsTransformer.Models
{
    public class TransformedEventModel
    {
        public string EventName { get; set; }
        public long EventTime { get; set; }
        public List<string> GroupTopicNames { get; set; }
        //public DateTime EventTime { get; set; }

    }
}
