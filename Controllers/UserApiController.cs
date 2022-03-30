using EXhibition.Models;
using System;
using System.Linq;
using System.Web.Mvc;
namespace EXhibition.Controllers
{
    public class UserApiController : Controller
    {

        DBConnector db = new DBConnector();

        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            ReturnData r = new ReturnData();
            r.message = "登入成功";
            r.status = ReturnStatus.Success;
            r.data = new { url = "/", mylogin = login };
            Session["auth"] = 1;
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(Models.users user)
        {
            user.UID = 0;
            ReturnData r = new ReturnData();
            users u = db.users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                r.status = ReturnStatus.Error;
                r.message = "註冊失敗";
                r.data = u;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            r.status = ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/UserLogin" };
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        //票卷票表
        public ActionResult ticketList()
        {
            int id = (int)Session["userid"] == null ? 2 : (int)Session["userid"];

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

        public ActionResult ticketdetail(int? eventID = 12)
        {
            //int userId = (int)Session["userid"];
            int userId = 14;
            var data = (from p in db.Tickets
                        join q in db.users on p.UID equals q.UID
                        join k in db.events on p.EVID equals k.EVID
                        where k.EVID == eventID
                        where q.UID == userId
                        select new
                        {
                            name = k.name,
                            start = k.startdate,
                            end = k.enddate,
                            image = k.image,
                            info = k.exhibitinfo,
                            token = p.token

                        }).ToList();



            //return new NewJsonResult() { Data = data };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}