using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EXhibition.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OtherController : ApiController
    {

        DBConnector db = new DBConnector();

        public IHttpActionResult GetTagList()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select top(10) count(A.tagId) , A.TagId , B.tagName from eventTags as A inner join TagsName as B on A.tagID = B.id group by A.tagId ,B.tagName order by 1 desc";

            // 先將 id 撈成 陣列後 用 entity framework 去找資料

            List<TagsName> tagList = new List<TagsName>();
            List<int> eventIdList = new List<int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        eventIdList.Add((int)reader[1]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            List<string> tList = db.TagsName.Where(item => eventIdList.Contains(item.id)).Select(e => e.tagName).ToList();

            return Ok(tList);
        }

        //展覽已有的Tag
        public IHttpActionResult GetEventTagsList(int? EVID)
        {
            int evid = (int)EVID;

            var tList = (from evtag in db.eventTags
                         join tagname in db.TagsName
                         on evtag.tagID equals tagname.id
                         where evtag.EVID == evid
                         select tagname.tagName).ToList();

            return Ok(tList);
        }

        public async Task<IHttpActionResult> GetHostDashBoard()

        {
            var hostInfo = new Repo.HostDashboard();
            int accountId = (int)(HttpContext.Current.Session["AccountID"] == null ? 2 : HttpContext.Current.Session["AccountID"]);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["revenue"] = hostInfo.GetMonthlyRevenue(accountId);
            data["numPeople"] = hostInfo.GetMonthlyPerson(accountId);
            data["myHotExhibition"] = await hostInfo.GetSellingHotExhibition(accountId);
            data["holdCount"] = hostInfo.GetHoldCount(accountId);
            data["myHotEventList"] = await hostInfo.GetMyHotEventList(accountId);
            data["myHotTagList"] = await hostInfo.GetMyHotTagList(accountId);
            data["allHotEventList"] = await hostInfo.GetAllHotEventList(accountId);
            data["allHotTagList"] = await hostInfo.GetAllHotTagList(accountId);
            return Ok(data);
        }

        public async Task<IHttpActionResult> GetExhibitorDashBoard()
        {
            var hostInfo = new Repo.HostDashboard();
            int accountId = (int)(HttpContext.Current.Session["AccountID"] == null ? 2 : HttpContext.Current.Session["AccountID"]);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["hotUserTag"] = hostInfo.GetHotUserTag(accountId);
            data["hotTag"] = hostInfo.GetHotEventTag();
            data["hotEvent"] = hostInfo.GetHotEvent();
            data["joinEventCount"] = hostInfo.GetJoinCount(accountId);           
            return Ok(data);
        }


    }
}
