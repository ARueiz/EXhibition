
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class CheckTicket
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int TicketEventId { get; set; }
        public int EVID { get; set; }
    }
}