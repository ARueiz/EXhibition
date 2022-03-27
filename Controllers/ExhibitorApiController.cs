using System;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class ExhibitorApiController : Controller
    {

        Models.DBConnector db = new Models.DBConnector();

        [HttpPost]
        public ActionResult Register(Models.exhibitors exhibitor)
        {
            Models.ReturnData r = new Models.ReturnData();
            try
            {
                exhibitor.EID = 0;
                var e = db.exhibitors.Add(exhibitor);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                r.status = Models.ReturnStatus.Error;
                r.message = "註冊失敗";
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            r.status = Models.ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/ExhibitiorLogin" };
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login(Models.Login login)
        {
            Models.ReturnData returnData = new Models.ReturnData();
            Session["auth"] = 2;
            returnData.status = "success";
            returnData.data = new { url = "/Exhibitor" };
            return Json(returnData, JsonRequestBehavior.AllowGet);
        }


    }
}