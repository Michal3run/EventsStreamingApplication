using System.Collections.Generic;

namespace EventTransformer.Models
{
    public class RawEventModel
    {
        public Venue venue { get; set; }
        public string visibility { get; set; }
        public string response { get; set; }
        public int guests { get; set; }
        public Member member { get; set; }
        public int rsvp_id { get; set; }
        public long mtime { get; set; }
        public Event @event { get; set; }
        public Group group { get; set; }
    }

    public class Venue
    {
        public string venue_name { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public int venue_id { get; set; }
    }

    public class Member
    {
        public int member_id { get; set; }
        public string photo { get; set; }
        public string member_name { get; set; }
    }

    public class Event
    {
        public string event_name { get; set; }
        public string event_id { get; set; }
        public long time { get; set; }
        public string event_url { get; set; }
    }

    public class GroupTopic
    {
        public string urlkey { get; set; }
        public string topic_name { get; set; }
    }

    public class Group
    {
        public List<GroupTopic> group_topics { get; set; }
        public string group_city { get; set; }
        public string group_country { get; set; }
        public int group_id { get; set; }
        public string group_name { get; set; }
        public double group_lon { get; set; }
        public string group_urlname { get; set; }
        public double group_lat { get; set; }
    }
}
