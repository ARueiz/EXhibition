﻿using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class HostApiController : Controller
    {

        DBConnector db = new DBConnector();


        //顯示主辦單位
        public ActionResult show_Host(int? id)
        {

            Models.ReturnData rd = new Models.ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";                
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            int index = (int) id;

            var host = db.hosts.Find(index);
            
            if ( host == null || host.HID == 0)
            {
                rd.message = "找不到資料";
                rd.status = "error";
                rd.data = host; 
                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            
            return Json(host,JsonRequestBehavior.AllowGet);
        }


        //編輯主辦單位
        [HttpPost]
        public ActionResult Edit_Host(Models.hosts host)
        {
            Models.ReturnData rd = new Models.ReturnData();

            if (host.HID == 0 )
            {
                rd.message = "Id 錯誤";
                rd.status = "error";
                rd.data = host;
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var data = db.hosts.Find(host.HID);

            if (data == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";
                rd.data = host;
                return Json(rd, JsonRequestBehavior.AllowGet);
            } else
            {

                data.link = host.link;
                data.name = host.name;
                data.phone = host.phone;
                data.email = host.email;
              

                db.SaveChanges();
            }

            rd.message = "成功";
            rd.status = "success";
            return Json(rd, JsonRequestBehavior.AllowGet);
        }

        //顯示展覽列表
        public ActionResult show_host_list(int? num = 1)
        {
            int x;
            List<Models.events> info = new List<Models.events>();
            if (num < 0 || num == 1)
            {
                num = 1;
                x = 0;
                info = db.events.OrderBy(y => y.EVID).Skip(x).Take(5).ToList();
            }
            else
            {
                x = (int)num * 5;
                info = db.events.OrderBy(y => y.EVID).Skip(x).Take(5).ToList();
            }
            foreach (var i in info)
            {
                i.image =  @"/image/host/" + i.image;
            }
            return Json(info, JsonRequestBehavior.AllowGet);
        }


        //顯示唯一展覽詳細
        public ActionResult showEventDetail(int? index)
        {
            Models.ReturnData rd = new ReturnData();
            if(index == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            int i = (int)index;

            var data = db.events.Find(i);
            return Json(data,JsonRequestBehavior.AllowGet);
        }



        //顯示廠商詳細

        public ActionResult showexhibitor(int? index)
        {
            Models.ReturnData rd = new Models.ReturnData();

            if (index == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            int i = (int)index;
            Models.exhibitors data = db.exhibitors.Find(i);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

       





        public ActionResult List()
        {


            var b = from hostsTable in db.hosts
                    join eventsTable in db.events on hostsTable.HID equals eventsTable.HID
                    select new
                    {
                        name = hostsTable.name,
                        phone = hostsTable.phone,
                        startdate = eventsTable.startdate.ToString(),
                        enddate = eventsTable.enddate.ToString(),
                        exhibitionname = eventsTable.name,
                        evid = eventsTable.EVID,
                        ticketPrice = eventsTable.ticketprice,
                    };

            return Json( b, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult PostList(int? id )
        {

            Models.ReturnData rd = new Models.ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            int index = (int)id;

            var a = db.events.Find(index);

            if (a == null || a.EVID == 0)
            {
                rd.message = "找不到資料";
                rd.status = "error";
                rd.data = a;
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(a, JsonRequestBehavior.AllowGet);
        }





        public ActionResult DoCreateEvent(HttpPostedFileBase image, HttpPostedFileBase floorplanimg, Models.events events)
        {
        

            string strPath = "";

            if (image != null)
            {
                //儲存 封面圖 to Image/Host
                strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.image;
                image.SaveAs(strPath);
            }

            if(floorplanimg != null)
            {
                //儲存 平面圖 to Image/Host
                strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.floorplanimg;
                floorplanimg.SaveAs(strPath);
            }

            var userInputTags = new List<Models.TagsName>();
            userInputTags.Add(new TagsName() { id = 1, tagName = "美食" });
            userInputTags.Add(new TagsName() { id = 2, tagName = "美九" });
            userInputTags.Add(new TagsName() { id = 3, tagName = "美立" });

            foreach (var item in userInputTags)
            {
                var a = db.TagsName.Where(e => e.tagName == item.tagName).FirstOrDefault();
                if (a==null)
                {
                    db.TagsName.Add(new TagsName() { tagName = item.tagName });
                    db.SaveChanges();
                }

                //db.eventTags.
            }            


            //儲存資料到DB
            events.HID = (int)Session["HID"];
            db.events.Add(events);
            int result = db.SaveChanges();
            try
            {
                db.SaveChanges();
                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Success;
                data.message = "新增成功!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Error;
                data.message = "新增失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Login(Models.Login login)
        {
            Models.ReturnData returnData = new Models.ReturnData();
            Session["auth"] = 3;
            returnData.status = "success";
            returnData.data = new { url = "/Host" };
            return Json(returnData,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(Models.hosts host)
        {
            Models.ReturnData r = new ReturnData();
            db.hosts.Add(host);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                r.status = Models.ReturnStatus.Error;
                r.message = "註冊失敗";
                return Json(r, JsonRequestBehavior.AllowGet);
            }

            r.status = Models.ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/HostLogin" };
            return Json(r);
        }


        public ActionResult editExhibitorJoinStatus(Models.exhibitinfo e,string reason,bool isAllow)
        {
            if (isAllow) // 允許加入
            {
               
            }
            else  // 拒絕加入，加上拒絕原因
            {

            }
            return Json(new { id = e.EID , resaon = reason , isAllow = isAllow } ,JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEventList(int? id)
        {
            Session["HID"] = 2;

            int HID = Convert.ToInt32(Session["HID"]);

            if (id == null) { id = 0; }
            int num = (int)id;
            var list = (from eve in db.events
                        where eve.HID == HID 
                        orderby eve.startdate descending select eve)
                        .Skip(num).Take(12).ToList();

            for (int i = 0; i < list.Count; i++)
            {

                list[i].image = "/image/Host/" + list[i].image;
            }

            return new NewJsonResult() { Data = list};
        }

        public ActionResult GetHostInfo(int? id)
        {
            Session["HID"] = 2;

            int HID = Convert.ToInt32(Session["HID"]);

            var list = (from host in db.hosts where host.HID == HID && host.verify == true
                        select new {
                            HID = host.HID,
                            name = host.name,
                            phone = host.phone,
                            email = host.email,
                            link = host.link,
                            image = host.image,
                            verify = host.verify
                        }).ToList();

            return new NewJsonResult() { Data = list[0] };
        }

        public ActionResult DoUpdateHostInfo(HttpPostedFileBase imagefile, hosts host)
        {
            string strPath = "";

            hosts updateHost = db.hosts.FirstOrDefault(h => h.HID == host.HID);
            ReturnData data = new ReturnData();

            if (updateHost != null)
            {
                if (imagefile != null)
                {
                    //儲存 封面圖 to Image/Host
                    strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + host.image;
                    updateHost.image = host.image;
                    imagefile.SaveAs(strPath);
                }

                updateHost.name = host.name;
                updateHost.phone = host.phone;
                updateHost.email = host.email;
                updateHost.link = host.link;

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