using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EXhibition.Models;

namespace EXhibition.Controllers
{    
    public class UserApiController : Controller
    {

        DBConnector db = new DBConnector();

        public ActionResult Login(Models.Login login)
        {
            ReturnData r = new ReturnData();
            r.message = "登入成功";
            r.status = "success";
            r.data = new { url = "/" , mylogin = login };
            Session["auth"] = 1;
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register(Models.users user)
        {
            user.UID = 0; 
            ReturnData r = new ReturnData();
            //user.UID = 1;
            users u = db.users.Add(user);
            try
            {
                db.SaveChanges();
            }catch(Exception ex)
            {
                r.message = "註冊失敗";
                r.data = u;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            r.message = "註冊成功";
            r.data = u ;            
            return Json(r, JsonRequestBehavior.AllowGet);
        }
    }
}