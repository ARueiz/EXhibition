using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.Exhibitor)]
    public class ExhibitorApiController : Controller
    {

        DBConnector db = new DBConnector();

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
                        orderby events.startdate
                        where exhibitinfo.EID == id
                        select new ApplyList
                        {
                            EVID = events.EVID,
                            name = events.name,
                            startdate = events.startdate.ToString(),
                            enddate = events.enddate.ToString(),
                            venue = events.venue,
                            verify = exhibitinfo.verify,
                            createAt = exhibitinfo.createAt.ToString(),
                            reason = exhibitinfo.reason,
                            dateout = false
                        }).ToList();


            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in list)
            {
                if (item.createAt.Length > 0)
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd");
                }
                else
                {
                    item.createAt = "沒有資料";
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public string ConvertTime(DateTime nTime)
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
                        orderby events.startdate
                        select new ApplyList
                        {
                            EVID = events.EVID,
                            name = events.name,
                            startdate = events.startdate.ToString(),
                            enddate = events.enddate.ToString(),
                            venue = events.venue,
                            verify = exhibitinfo.verify,
                            createAt = exhibitinfo.createAt.ToString(),
                            reason = exhibitinfo.reason,
                            dateout = false
                        }).ToList();

            foreach (var item in list)
            {
                if (item.createAt.Length > 0)
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd");
                }
                else
                {
                    item.createAt = "沒有資料";
                }

            }

            if (!list.Any())
            {
                return Json(new List<string>(), JsonRequestBehavior.AllowGet);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //廠商可申請的展覽列表
        public ActionResult CanApplyList()
        {
            ReturnData rd = new ReturnData();

            DateTime applydate = DateTime.Now.AddDays(+5);

            int exhibitorId = (int)(Session[Models.GlobalVariables.AccountId] == null ? 2 : Session[Models.GlobalVariables.AccountId]);

            var joinList = (from exhib in db.exhibitinfo where exhib.EID == exhibitorId select exhib.EVID).ToList();

            var list = (from even in db.events
                        where even.startdate > applydate
                        where !joinList.Contains(even.EVID)
                        select new { even.EVID, even.name, startdate = even.startdate.ToString(), enddate = even.enddate.ToString(), even.venue }).ToList();

            if (!list.Any())
            {
                rd.message = "近期無可申請之展覽";
                rd.status = ReturnStatus.Error;

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            rd.message = "查詢展覽列表成功";
            rd.status = ReturnStatus.Success;
            rd.data = list;
            return Json(rd, JsonRequestBehavior.AllowGet);
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

        public ActionResult getEventInfo(int? EVID)
        {
            ReturnData rd = new ReturnData();

            if (EVID == null)
            {
                rd.message = "查無此展覽";
                rd.status = ReturnStatus.Error;

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var list = (from ev in db.events
                        join host in db.hosts on ev.HID equals host.HID
                        where ev.EVID == EVID
                        select new
                        {
                            EVID = ev.EVID,
                            hostname = host.name,
                            eventname = ev.name,
                            startdate = ev.startdate.ToString(),
                            enddate = ev.enddate.ToString(),
                            eventinfo = ev.eventinfo,
                            image = ev.image,
                            floorplanimg = ev.floorplanimg,
                            venue = ev.venue,

                        }).ToList();

            if (!list.Any())
            {
                rd.message = "查無此展覽";
                rd.status = ReturnStatus.Error;

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            rd.message = "查詢此展覽成功";
            rd.status = ReturnStatus.Success;
            rd.data = list[0];

            return Json(rd, JsonRequestBehavior.AllowGet);
        }

        //廠商參展歷史
        public ActionResult ApplyHistory(int? id)
        {
            ReturnData rd = new ReturnData();
            if (id == null)
            {
                rd.message = "查無資料";
                rd.status = ReturnStatus.Error;

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            DateTime now = DateTime.Now;
            var list = (from host in db.hosts
                        join events in db.events on host.HID equals events.HID
                        join exhinfo in db.exhibitinfo on events.EVID equals exhinfo.EVID
                        where exhinfo.EID == (int)id && exhinfo.verify == true
                        select new ApplyList
                        {
                            EID = (int)id,
                            EVID = events.EVID,
                            name = host.name,
                            name2 = events.name,
                            startdate = events.startdate.ToString(),
                            enddate = events.enddate.ToString(),
                            DTstartdate = events.startdate,
                            DTenddate = events.enddate,
                            img = "/Image/Host/" + events.image
                            //dateout = null
                        }).ToList();

            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in list)
            {
                if (item.DTstartdate > now)
                {
                    item.dateout = false;
                }
                else if (item.DTenddate < now)
                {
                    item.dateout = true;
                }
            }

            rd.message = "搜尋到資料";
            rd.status = ReturnStatus.Success;
            rd.data = list;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoCreateEventInfo(HttpPostedFileBase image, exhibitinfo exhibitinfo)
        {
            ReturnData rd = new ReturnData();
            string strPath = "";

            if (image != null)
            {
                //儲存 封面圖 to Image/Host
                strPath = Request.PhysicalApplicationPath + "Image\\Exhibitor\\" + exhibitinfo.image;
                image.SaveAs(strPath);
            }

            exhibitinfo.createAt = DateTime.Now;
            exhibitinfo.status = "尚未審核";
            exhibitinfo.verify = null;

            db.exhibitinfo.Add(exhibitinfo);

            try
            {
                db.SaveChanges();
                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Success;
                data.message = "申請成功!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Error;
                data.message = "申請失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}