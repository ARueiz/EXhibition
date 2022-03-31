using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class HostEventData
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string exhibitionname { get; set; }
        public int evid { get; set; }
        public string ticketPrice { get; set; }
        public bool isOver { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
    }

}
