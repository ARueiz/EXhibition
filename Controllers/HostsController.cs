using EXhibition.Models;
using System;
using System.Linq;
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

        public ActionResult DetailsHost(int? id)
        {

            if (id == null)
            {
                id = 0;
            }
            ViewBag.id = id;
            return View();
        }

        //馬誠遠
        public ActionResult EditEvent()
        {
            return View();
        }

        public ActionResult exhibirtor_AllowOrRefuse()
        {
            var rd = new ReturnData();
            var alldata = db.exhibitinfo.ToList();

            if (alldata == null)
            {
                rd.message = "no data";
                rd.status = "error";
            }

            return Json(alldata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllowOrRefuse(int? index)
        {
            var rd = new ReturnData();
            if (index == null)
            {
                rd.message = "no data";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            int x = (int)index;
            var allow = db.exhibitors.Find(x);

            if (allow.verify == false)
            {
                allow.verify = true;

                rd.message = "modified";
                rd.status = "success";
                db.SaveChanges();
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            rd.message = "no data";
            rd.status = "error";
            return Json(rd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult allow_all()
        {
            var rd = new ReturnData();


            var allow = db.exhibitors.Where(i => i.verify == false);

            if (allow == null)
            {
                rd.message = "no data";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            foreach (var i in allow)
            {
                i.verify = true;
            }

            rd.message = "modified success";
            rd.status = "success";
            db.SaveChanges();

            return Json(rd, JsonRequestBehavior.AllowGet);

        }

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
            catch(Exception e)
            {
                rd.message = "no data";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

        }

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

        public ActionResult show_verified_false_exhibitor()
        {
            var data = db.exhibitors.Where(a => a.verify == false);


            return Json(data, JsonRequestBehavior.AllowGet);
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