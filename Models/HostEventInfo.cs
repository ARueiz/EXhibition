using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class HostEventInfo
    {
        internal decimal ticketPrice;

        public string name { get; internal set; }
        public string phone { get; internal set; }
        public string startdate { get; internal set; }
        public string enddate { get; internal set; }
        public string exhibitionname { get; internal set; }
        public int evid { get; internal set; }
        public int waitingCount { get; set; }
        public bool isOver { get; set; }
        public int hid { get; set; }
    }
}