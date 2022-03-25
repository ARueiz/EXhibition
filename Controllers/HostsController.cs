using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EXhibition.Controllers
{
    public class HostsController : Controller
    {
        private DBConnector db = new DBConnector();
        // GET: Hosts
        public ActionResult publicVersion()
        {
            return View();
        }


        //黃亭愷
        public ActionResult Index()
        {
            return View();
        }

        //林昶廷
        public ActionResult EditHost()
        {
            return View();
        }

        //馬誠遠
        public ActionResult EditEvent()
        {
            return View();
        }

       public ActionResult showexhibitor(int? index)
        {
            Models.ReturnData rd = new Models.ReturnData();

            if(index == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";
                
                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            int i = (int)index;
            Models.exhibitors data = db.exhibitors.Find(i);


            

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult editexhibitor(Models.exhibitors host)
        {

            Models.ReturnData rd = new Models.ReturnData();
            if(host.EID == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";               

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var data = db.exhibitors.Find(host.EID);

            if(data == null)
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








        //邱品叡
        public ActionResult CreateEvent()
        {
            Session["HID"] = 2;

            int HID = (int)Session["HID"];

            var info = db.hosts.Where(h => h.HID == HID).Select(h => new
            {
                h.name
            }).ToList();

            ViewBag.hostname = info[0].name;
            ViewBag.sessionHID = Session["HID"];

            return View();
        }
    }
}