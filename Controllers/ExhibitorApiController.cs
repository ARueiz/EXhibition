using EXhibition.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Runtime;
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

        //廠商參展的歷史紀錄
        public ActionResult EventHistory(int? id) 
        {
            ReturnData rd = new ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var list = (from exhibitinfo in db.exhibitinfo
                        join events in db.events on exhibitinfo.EVID equals events.EVID
                        where exhibitinfo.EID == id
                        select new { events.EVID, events.name, events.startdate, events.enddate, events.venue }).ToList();

            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        //廠商審核中申請
        public ActionResult NowApplying(int? id)
        {
            ReturnData rd = new ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var list = (from info in db.exhibitinfo
                        join even in db.events on info.EVID equals even.EVID
                        where info.EID == id && info.status == "尚未審核"
                        select new { even.EVID, even.name, even.startdate, even.enddate, even.venue, info.status }).ToList();

            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //廠商可申請的展覽列表
        public ActionResult CanApplyList()
        {
            ReturnData rd = new ReturnData();

            DateTime applydate = DateTime.Now.AddDays(+5);
            
            var list = (from even in db.events
                        where even.startdate>applydate
                        select new { even.EVID, even.name,even.startdate , even.enddate, even.venue }).ToString();
            
            if (!list.Any())
            {
                rd.message = "近期無可申請之展覽";
                rd.status = "no list";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}