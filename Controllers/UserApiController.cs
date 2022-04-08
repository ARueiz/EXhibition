using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.User)]
    public class UserApiController : Controller
    {
        DBConnector db = new DBConnector();

        //票卷票表
        public ActionResult ticketList()
        {

            int id = (int)(Session["AccountID"] == null ? 2 : Session["AccountID"]);

            var mes = new Models.ReturnData();
            if (id == -1)
            {
                mes.status = ReturnStatus.Error;
                mes.message = "404";

                return Json(mes, JsonRequestBehavior.AllowGet);
            }

            var ticketlist = (from p in db.Tickets
                              join q in db.events on p.EVID equals q.EVID
                              join k in db.users on p.UID equals k.UID
                              where k.UID == id
                              orderby p.createAt descending
                              select new TicketPreview()
                              {
                                  ticketId = p.TID,
                                  name = q.name,
                                  startdate = q.startdate.ToString(),
                                  enddate = q.enddate.ToString(),
                                  image = "/image/host/" + q.image
                              }).ToList();

            foreach (var item in ticketlist)
            {
                if (DateTime.Now < DateTime.Parse(item.startdate)) // 未來
                {
                    item.status = "presale";
                }
                else if (DateTime.Now >= DateTime.Parse(item.startdate) && DateTime.Now <= DateTime.Parse(item.enddate)) // 現在
                {
                    item.status = "now";
                }
                else if (DateTime.Now > DateTime.Parse(item.enddate))  //過去
                {
                    item.status = "over";
                }
            }

            return Json(ticketlist, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ticketdetail(int? ticketId) // 改用 ticketId 
        {
            int userId = (int)(Session["AccountID"] == null ? 2 : Session["AccountID"]);

            // 更新 qr code 產生
            var tk = db.Tickets.Where(t => t.TID == ticketId).FirstOrDefault();
            tk.token = Guid.NewGuid().ToString();
            tk.tokenExistenceTime = DateTime.Now;
            db.SaveChanges();

            var data = (from t in db.Tickets
                        join u in db.users on t.UID equals u.UID
                        join e in db.events on t.EVID equals e.EVID
                        where t.TID == ticketId
                        select new
                        {
                            content = e.eventinfo,
                            name = e.name,
                            start = e.startdate.ToString(),
                            end = e.enddate.ToString(),
                            image = e.image,
                            info = e.exhibitinfo,
                            token = t.token,
                            eventId = e.EVID,
                            ticketId = t.TID
                        }).FirstOrDefault();



            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserInfo(int? id)
        {
            int UID = Convert.ToInt32(Session["AccountID"]);

            var list = (from user in db.users
                        where user.UID == UID && user.verify == true
                        select new
                        {
                            UID = user.UID,
                            name = user.name,
                            phone = user.phone,
                            email = user.email,
                        }).ToList();

            return new NewJsonResult() { Data = list[0] };
        }

        public ActionResult DoUpdateUserInfo(users user)
        {
            users updateUser = db.users.FirstOrDefault(u => u.UID == user.UID);
            ReturnData data = new ReturnData();

            if (updateUser != null)
            {
                updateUser.name = user.name;
                updateUser.phone = user.phone;
                updateUser.email = user.email;

                try
                {
                    db.SaveChanges();
                    data.status = ReturnStatus.Success;
                    data.message = "更新成功!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    data.status = ReturnStatus.Error;
                    data.message = "更新失敗!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.status = ReturnStatus.Error;
                data.message = "更新失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Gettop5Tag()
        {
            //var data = from p in db.TagsName
            //           join q in db.eventTags
            //           on p.id equals q.tagID
            //           orderby q.tagID.

            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select TOP(5) count(q.tagName) ,q.tagName from eventTags as p join TagsName as q on p.tagID = q.id group by q.tagName";

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
                        var A = (int)reader[1];
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}