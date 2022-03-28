using EXhibition.Models;
using System;
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
    }
}