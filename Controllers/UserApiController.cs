using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            
            int id = (int)Session["userid"];

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