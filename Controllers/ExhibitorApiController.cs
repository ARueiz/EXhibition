using EXhibition.Models;

using System;
using System.Linq;
using System.Web.Mvc;
using System.Runtime;
using EXhibition.Filters;

namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.Exhibitor)]
    public class ExhibitorApiController : Controller
    {

        Models.DBConnector db = new Models.DBConnector();

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
                        orderby events.startdate,events.enddate
                        select new { events.EVID, events.name, startdate =events.startdate.ToString(), enddate =events.enddate.ToString(), events.venue }).ToList();

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
                        select new ApplyList{ EVID =events.EVID, name = events.name, startdate = events.startdate.ToString(), enddate = events.enddate.ToString(), venue = events.venue, status =exhibitinfo.status, dateout = false }).ToList();

            foreach(var item in list)
            {
                if(item.status == "尚未審核")
                {
                    item.status = "checking";
                }
                else if (item.status == "允許")
                {
                    item.status = "success";
                }
                else if (item.status == "拒絕")
                {
                    item.status = "fail";
                }
                else
                {
                    item.status = "";
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

        //廠商正在審核中的申請--點下tag
        public ActionResult NowApplyingTag(string tag)
        {
            ReturnData rd = new ReturnData();

            if (tag == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var list = (from exhibitinfo in db.exhibitinfo
                        join events in db.events on exhibitinfo.EVID equals events.EVID
                        where exhibitinfo.status == tag
                        select new ApplyList { EVID = events.EVID, name = events.name, startdate = events.startdate.ToString(), enddate = events.enddate.ToString(), venue = events.venue, status = exhibitinfo.status, dateout = false }).ToList();

            foreach (var item in list)
            {
                if (item.status == "尚未審核")
                {
                    item.status = "checking";
                }
                else if (item.status == "允許")
                {
                    item.status = "success";
                }
                else if (item.status == "拒絕")
                {
                    item.status = "fail";
                }
                else
                {
                    item.status = "";
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
            int EID = Convert.ToInt32(Session["AccountID"]);

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