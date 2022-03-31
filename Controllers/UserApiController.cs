using EXhibition.Filters;
using EXhibition.Models;
using System;
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

            int id = (int)(Session["userid"] == null ? 2 : Session["userid"]);

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
                              select new
                              {
                                  name = q.name,
                                  startdate = q.startdate,
                                  enddate = q.enddate,
                                  image = q.image,


                              }).ToList();

            return Json(ticketlist, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ticketdetail(int? eventID = 12) // 改用 ticketId 
        {
            //int userId = (int)Session["userid"];
            int userId = 14;

            // 更新 qr code 產生
            var tk = db.Tickets.Where(e => e.UID == userId).Where(e => e.EVID == eventID).FirstOrDefault();
            tk.token = Guid.NewGuid().ToString();
            tk.tokenExistenceTime = DateTime.Now;
            db.SaveChanges();

            var data = (from t in db.Tickets
                        join u in db.users on t.UID equals u.UID
                        join e in db.events on t.EVID equals e.EVID
                        where e.EVID == eventID
                        where u.UID == userId
                        select new
                        {

                            name = e.name,
                            start = e.startdate,
                            end = e.enddate,
                            image = e.image,
                            info = e.exhibitinfo,
                            token = t.token,
                            TicketEventId = e.EVID,
                            TicketId = t.TID
                        }).FirstOrDefault();



            return Json(tk, JsonRequestBehavior.AllowGet);
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
    }
}