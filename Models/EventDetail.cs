using System.Collections.Generic;

namespace EXhibition.Models
{
    public class EventDetail
    {
        public int EVID { get; set; }
        public string organizer { get; set; }
        public string hostName { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string location { get; set; }
        public string image { get; set; }
        public string price { get; set; }

        public string eventinfo { get; set; }

        public List<ExhibitorsInfo> exhibitorList = new List<ExhibitorsInfo>();

        public List<string> tagList = new List<string>();
    }

    public class ExhibitorsInfo
    {
        public string url { get; set; }
        public int EID { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }

}