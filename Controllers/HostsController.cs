using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Linq;
using System.Web.Mvc;


namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.Host)]
    public class HostsController : Controller
    {
        private DBConnector db = new DBConnector();
        // GET: Hosts
        public ActionResult publicVersion()
        {
            return View();
        }

        public ActionResult test1()
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
                 return Redirect("Index");
            }
            ViewBag.id = id;
            return View();
        }

        //馬誠遠
        public ActionResult EditEvent()
        {
            return View();
        }

     

       

        public ActionResult allow_all()
        {
            var rd = new ReturnData();


            var allow = db.exhibitors.Where(i => i.verify == false).ToList() ;

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