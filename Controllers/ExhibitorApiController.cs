using EXhibition.Models;

using System;
using System.Linq;
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
                        orderby events.startdate, events.enddate
                        select new { events.EVID, events.name, startdate = events.startdate.ToString(), enddate = events.enddate.ToString(), events.venue }).ToList();

            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        //廠商活動資訊修改
        public ActionResult edit__exhibition(Models.exhibitinfo exhibitor)
        {

            var rd = new ReturnData();


            if (exhibitor.EID == 0)
            {
                rd.message = "no data";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            try
            {
                int i = (int)exhibitor.EID;
                var data = db.exhibitinfo.Find(i);


                data.link = exhibitor.link;
                change_image_link(data.image, exhibitor.image);
                data.boothnumber = exhibitor.boothnumber;
                data.productinfo = exhibitor.productinfo;

                rd.message = "modified success";
                rd.status = "success";


                db.SaveChanges();

                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                rd.message = "no data";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

        }
        //改變圖片
        public ActionResult change_image_link(string link, string newlink)
        {
            if (System.IO.File.Exists(link))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {

                    System.IO.File.Delete(link);
                    link = newlink;
                }
                catch (System.IO.IOException e)
                {
                    ReturnData rd = new ReturnData();
                    rd.message = "no data";
                    rd.status = "error";
                    return Json(rd, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
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

        //廠商正在審核中的申請
        public ActionResult NowApplying(int? id)
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
                        select new { EVID = events.EVID,
                                     name = events.name, 
                                     startdate = events.startdate.ToString(), 
                                     enddate = events.enddate.ToString(), 
                                     venue = events.venue, 
                                     verify = exhibitinfo.verify,
                                     createAt = exhibitinfo.createAt,
                                     reason = exhibitinfo.reason,
                                     dateout = false }).ToList();

  
            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            return new NewJsonResult() { Data = list};
        }

        public string Convert(DateTime nTime)
        {
            DateTime time = (DateTime)(nTime == null ? DateTime.Parse("2000-01-01") : nTime);
            return time.ToString("yyyy-MM-dd");
        }

        //廠商正在審核中的申請--點下tag
        public ActionResult NowApplyingTag(int? EID, bool? tag)
        {
            ReturnData rd = new ReturnData();


            var list = (from exhibitinfo in db.exhibitinfo
                        join events in db.events on exhibitinfo.EVID equals events.EVID
                        where exhibitinfo.verify == tag && exhibitinfo.EID == EID
                        select new ApplyList
                        {
                            EVID = events.EVID,
                            name = events.name,
                            startdate = events.startdate.ToString(),
                            enddate = events.enddate.ToString(),
                            venue = events.venue,
                            verify = exhibitinfo.verify,
                            createAt = exhibitinfo.createAt.ToString(),
                            reson = exhibitinfo.reason,
                            dateout = false
                        }).ToList();

            foreach (var item in list)
            {
                if (item.createAt.Length > 0 )
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd");
                } else
                {
                    item.createAt = "沒有資料";
                }
                
            }

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
                        where even.startdate > applydate
                        select new { even.EVID, even.name, startdate = even.startdate.ToString(), enddate = even.enddate.ToString(), even.venue }).ToList();

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