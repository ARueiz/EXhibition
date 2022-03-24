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

        public ActionResult Login(Models.Login login)
        {
            ReturnData r = new ReturnData();
            r.message = "登入成功";
            r.status = "success";
            r.data = new { url = "/" , mylogin = login };
            Session["auth"] = 1;
            return Json(r, JsonRequestBehavior.AllowGet);
        }
    }
}