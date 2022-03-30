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

        //編輯修改廠商
        public ActionResult editexhibitor(Models.exhibitors exhibitor)
        {

            Models.ReturnData rd = new Models.ReturnData();
            if (exhibitor.EID == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var data = db.exhibitors.Find(exhibitor.EID);

            if (data == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            data.name = exhibitor.name;
            data.email = exhibitor.email;
            data.password = exhibitor.password;
            data.phone = exhibitor.phone;
            data.link = exhibitor.link;

            db.SaveChanges();

            rd.message = "成功";
            rd.status = "success";
            return Json(rd, JsonRequestBehavior.AllowGet);
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
                        select new { even.EVID, even.name,startdate = even.startdate.ToString() , enddate =even.enddate.ToString(), even.venue }).ToList();
            
            if (!list.Any())
            {
                rd.message = "近期無可申請之展覽";
                rd.status = "no list";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExhibitorInfo(int? id)
        {
            Session["EID"] = 2;

            int EID = Convert.ToInt32(Session["EID"]);

            var list = (from exhibitor in db.exhibitors
                        where exhibitor.EID == EID && exhibitor.verify == true
                        select new
                        {
                            EID = exhibitor.EID,
                            name = exhibitor.name,
                            phone = exhibitor.phone,
                            email = exhibitor.email,
                            link = exhibitor.link,
                        }).ToList();

            return new NewJsonResult() { Data = list[0] };
        }

        public ActionResult DoUpdateExhibitorInfo(exhibitors exhibitor)
        {
            exhibitors updateExhibitor = db.exhibitors.FirstOrDefault(e => e.EID == exhibitor.EID);
            ReturnData data = new ReturnData();

            if (updateExhibitor != null)
            {
                updateExhibitor.name = exhibitor.name;
                updateExhibitor.phone = exhibitor.phone;
                updateExhibitor.email = exhibitor.email;
                updateExhibitor.link = exhibitor.link;

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