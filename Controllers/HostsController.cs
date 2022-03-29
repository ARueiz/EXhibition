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

        public ActionResult DetailsHost(int? id)
        {
            //Session["EVID"] = 1;
            //int EVID = (int)Session["EVID"];

            //var info = db.events.Where(h => h.EVID == EVID).Select(a => new
            //{ 
            //    a.name, 
            //    a.startdate, 
            //    a.enddate, 
            //    a.ticketprice, 
            //    a.verify
            //}).ToList();
            //ViewBag.hostname = info[0].name;
            //ViewBag.startdate = info[0].startdate;
            //ViewBag.enddate = info[0].enddate;
            //ViewBag.ticketprice = info[0].ticketprice;
            //ViewBag.verify = info[0].verify;
            //ViewBag.sessionEVID = Session["EVID"];
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