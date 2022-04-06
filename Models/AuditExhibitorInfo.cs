using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class AuditExhibitorInfo
    {
        public string name { get; set; }
        public int id { get; set; }
        public string createAt { get; set; }
        public bool? verify { get; set; }

    }
}