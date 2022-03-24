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
        
        public ActionResult Index(int? id)
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
        public ActionResult Edit(Models.hosts host)
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
            returnData.status = "success";
            returnData.data = new { url = "/Host" };
            return Json(returnData,JsonRequestBehavior.AllowGet);
        }

    }
}