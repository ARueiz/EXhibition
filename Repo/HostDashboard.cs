using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EXhibition.Repo
{
    public class HostDashboard
    {

        public struct ItemInfo
        {
            public string name { get; set; }
            public int number { get; set; }
        }

        Models.DBConnector db = new Models.DBConnector();

        public HostDashboard() { }

        public int GetMonthlyRevenue(int hostId)
        {

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

            //return totalPrice;
            return totalPrice;
        }

        public int GetMonthlyPerson(int hostId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var a = from r in db.Tickets
                    join ev in db.events on r.EVID equals ev.EVID
                    where ev.HID == hostId
                    where r.createAt > date
                    select r;
            var count = a.Count();
            return count;
        }

        public Task<string> GetSellingHotExhibition(int hostId)
        {
            var findHostTicket = (from ticket in db.Tickets
                                  join even in db.events on ticket.EVID equals even.EVID
                                  where even.HID == hostId
                                  select ticket);

            var findMaxPerson = findHostTicket.GroupBy(e => e.EVID).
                Select(group => new { id = group.Key, Count = group.Count() }).
                OrderByDescending(x => x.Count).FirstOrDefault();

            if (findMaxPerson == null) return Task.FromResult("");

            var myEvent = db.events.Find(findMaxPerson.id);

            return Task.FromResult(myEvent.name);
        }

        public int GetHoldCount(int hostId)
        {
            var count = db.events.Where(e => e.HID == hostId).Count();
            return count;
        }

        public Task<List<ItemInfo>> GetMyHotEventList(int hostId)
        {
            var a = (from t in db.Tickets
                     join ex in db.events on t.EVID equals ex.EVID
                     where ex.HID == hostId
                     group ex by ex.EVID into grp
                     select new { id = grp.Key, count = grp.Count() }).OrderByDescending(e => e.count).Take(3).ToList();

            List<ItemInfo> list = (from p in a join ex in db.events on p.id equals ex.EVID select new ItemInfo { name = ex.name, number = p.count }).OrderByDescending(e => e.number).ToList();

            return Task.FromResult(list);
        }

        public Task<List<ItemInfo>> GetMyHotTagList(int hostId)
        {

            var tagCountList = (from e in db.events
                                join tg in db.eventTags on e.EVID equals tg.EVID
                                where e.HID == hostId
                                group tg by tg.tagID into grp
                                select new { id = grp.Key, count = grp.Count() }).OrderByDescending(e => e.count).Take(3).ToList();
            var list = (from p in tagCountList
                        join tg in db.TagsName on p.id equals tg.id
                        select new ItemInfo { name = tg.tagName, number = p.count }).OrderByDescending(e => e.number).ToList();
            return Task.FromResult(list);
        }

        public Task<List<ItemInfo>> GetAllHotEventList(int hostId)
        {
            var a = (from t in db.Tickets
                     join ex in db.events on t.EVID equals ex.EVID
                     group ex by ex.EVID into grp
                     select new { id = grp.Key, count = grp.Count() }).OrderByDescending(e => e.count).Take(3).ToList();

            List<ItemInfo> list = (
                from p in a
                join ex in db.events on p.id equals ex.EVID
                select new ItemInfo { name = ex.name, number = p.count })
                .OrderByDescending(e => e.number).ToList();

            return Task.FromResult(list);
        }

        public Task<List<ItemInfo>> GetAllHotTagList(int hostId)
        {

            var tagCountList = (from e in db.events
                                join tg in db.eventTags on e.EVID equals tg.EVID
                                group tg by tg.tagID into grp
                                select new { id = grp.Key, count = grp.Count() }).OrderByDescending(e => e.id).Take(3).ToList();
            var list = (from p in tagCountList
                        join tg in db.TagsName on p.id equals tg.id
                        select new ItemInfo { name = tg.tagName, number = p.count })
                        .OrderByDescending(e => e.number)
                        .ToList();
            return Task.FromResult(list);
        }

        public String GetHotEvent()
        {

            var now = DateTime.Now;

            var query = (from tk in db.Tickets
                         join evnt in db.events on tk.EVID equals evnt.EVID
                         where evnt.enddate > now
                         select evnt);

            var eventId = query.GroupBy(e => e.EVID)
                .Select(e => new { id = e.Key, count = e.Count() })
                .OrderByDescending(e => e.count).Select(e => e.id).FirstOrDefault();

            return db.events.Find(eventId).name;
        }

        public int GetJoinCount(int eid)
        {
            int count = db.exhibitinfo.Where(e => e.verify == true).Where(e => e.EID == eid).Count();
            return count;
        }

        public string GetHotEventTag()
        {
            var tagId = (from evtg in db.eventTags
                         group evtg by evtg.tagID into grp
                         select new { id = grp.Key, count = grp.Count() })
                       .OrderByDescending(e => e.count).FirstOrDefault();
            string tagName = db.TagsName.Find(tagId.id).tagName;
            return tagName;
        }
        public string GetHotUserTag(int accountId)
        {

            var now = DateTime.Now;

            var query = (from tk in db.Tickets
                         join evnt in db.events on tk.EVID equals evnt.EVID
                         where evnt.enddate > now
                         select evnt);

            var eventIdArray = query.GroupBy(e => e.EVID)
                .Select(e => new { id = e.Key, count = e.Count() })
                .OrderByDescending(e => e.count).Select(e => e.id).Take(10).ToArray();

            var tagId = db.eventTags.
                Where(e => eventIdArray.Contains(e.EVID)).
                GroupBy(e => e.tagID).
                Select(e => new { id = e.Key, count = e.Count() }).
                OrderByDescending(e => e.count).Select(e=>e.id).FirstOrDefault();

            return db.TagsName.Find(tagId).tagName;
        }

    }
}