using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EXhibition.Models;

namespace EXhibition.Controllers
{
    public class HostApiController : Controller
    {

        DBConnector db = new DBConnector();
        
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
                i.image = Request.Url.Authority + @"/image/host/" + i.image;
            }
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showOnlyExhibition(int? index)
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

        public ActionResult editexhibitor(Models.exhibitors host)
        {

            Models.ReturnData rd = new Models.ReturnData();
            if (host.EID == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var data = db.exhibitors.Find(host.EID);

            if (data == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            data.name = host.name;
            data.email = host.email;
            data.password = host.password;
            data.phone = host.phone;
            data.link = host.link;

            db.SaveChanges();

            rd.message = "成功";
            rd.status = "success";
            return Json(rd, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DoCreateEvent(HttpPostedFileBase image, HttpPostedFileBase floorplanimg, Models.events events){

            //儲存 封面圖 to Image/Host
            string strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.image;
            image.SaveAs(strPath);

            //儲存 平面圖 to Image/Host
            strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.floorplanimg;
            floorplanimg.SaveAs(strPath);

            //儲存資料到DB
            events.HID = (int)Session["HID"];
            db.events.Add(events);
            int result = db.SaveChanges();
            
            if(result > 0)
            {
                ReturnData data = new ReturnData();
                data.status = "success";
                data.message = "新增成功!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ReturnData data = new ReturnData();
                data.status = "failed";
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
            var list = (from eve in db.events where eve.HID == HID orderby eve.startdate descending select eve).Skip(num).Take(12).ToList();

            for (int i = 0; i < list.Count; i++)
            {

                list[i].image = "/image/Host/" + list[i].image;
            }

            return new NewJsonResult() { Data = list};
        }
    }
}