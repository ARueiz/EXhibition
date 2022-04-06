using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class ApplyList
    {
        public int EVID { get; set; }
        public int EID { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public DateTime DTstartdate { get; set; }
        public DateTime DTenddate { get; set; }
        public string venue { get; set; }
        public bool? verify { get; set; }
        public bool? dateout { get; set; }
        public string reason { get; set; }
        public string createAt { get; set; }
        public string img { get; set; }
    }
}