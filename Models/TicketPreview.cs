using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class TicketPreview
    {
        public int ticketId { get; set; }
        public string name { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string image { get; set; }
        public string status { get; set; }
        public string purchaseDateTime { get; set; } 
        public int ticketPrice { get; set; }

    }
}