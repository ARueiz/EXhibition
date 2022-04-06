using System.Collections.Generic;
using System.Linq;

namespace EXhibition.Repo
{
    public class TagRepo
    {
        public void TagsInsert(List<string> list, int eventId)
        {
            Models.DBConnector db = new Models.DBConnector();

            List<Models.TagsName> tags = new List<Models.TagsName>();

            // db.eventTags.Add(new Models.eventTags() { EVID = eventId , tagID = 1 });

            list.ForEach(item =>
            {
                var hasTag = db.TagsName.Where(e => e.tagName == item).FirstOrDefault();
                if (hasTag != null)
                {
                    db.eventTags.Add(new Models.eventTags() { EVID = eventId, tagID = hasTag.id });
                    db.SaveChanges();
                }
                else
                {
                    var tg = db.TagsName.Add(new Models.TagsName() { tagName = item });
                    db.SaveChanges();
                    db.eventTags.Add(new Models.eventTags() { EVID = eventId, tagID = tg.id });
                    db.SaveChanges();
                }

            });
        }
    }
}