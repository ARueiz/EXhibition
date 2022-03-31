
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class CheckTicket
    {
        public int TicketId { get; set; }
        public string TicketToken { get; set; }

        public int TicketEventId { get; set; }  
    }
}