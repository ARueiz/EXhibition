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
        public ActionResult Index(int id)
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

            return View();
        }
    }
}