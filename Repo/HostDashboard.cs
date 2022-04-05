using System;
using System.Linq;

namespace EXhibition.Repo
{
    public class HostDashboard
    {



        public HostDashboard() { }

        static public int GetMonthRevenue(int hostId)
        {
            Models.DBConnector db = new Models.DBConnector();
            DateTime? time = DateTime.Now.AddMonths(-1);
            var ticketList = (from t in db.Tickets
                              join ex in db.events on t.EVID equals ex.EVID
                              where ex.HID == hostId
                              where t.createAt > time
                              select new { id = t.TID, price = ex.ticketprice }
                     ).ToList();

            int totalPrice = 0;
            ticketList.ForEach(ticket =>
            {
                totalPrice = totalPrice + decimal.ToInt32(ticket.price);
            });

            return totalPrice;
        }

    }
}