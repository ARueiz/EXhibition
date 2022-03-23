using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class HostApiController : Controller
    {

        Models.DBConnector db = new Models.DBConnector();
        // GET: HostApi
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

        public ActionResult List()
        {
            var host = db.hosts.ToList();
            return Json(host, JsonRequestBehavior.AllowGet);
        }

    }
}