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
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string venue { get; set; }
        public string status { get; set; }
        public bool dateout { get; set; }
    }
}