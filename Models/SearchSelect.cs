using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class SearchSelect
    {
        public string[] CheckTag { get; set; }
        public DateTime? StartDate { get; set; }    
        public DateTime? EndDate { get; set; } 
    }
}