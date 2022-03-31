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

            int id = Session["userid"] == null ? 2 : (int)Session["userid"];

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
                else if (DateTime.Now >= DateTime.Parse(item.startdate) || DateTime.Now <= DateTime.Parse(item.enddate)) // 現在
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
            Session["UID"] = 2;

            int UID = Convert.ToInt32(Session["UID"]);

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