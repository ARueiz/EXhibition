using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class HomeController : Controller
    {
        private DBConnector db = new DBConnector();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetEnv()
        {
            var a = Environment.GetEnvironmentVariable("myValue");
            ViewBag.data = a;
            return View();
        }

        public ActionResult dbTest()
        {
            var exists = db.users.Any(m => m.UID == 2);

            //string tmp = Environment.GetEnvironmentVariable("SQL_PASSWORD");

            return Content(exists.ToString(), "text/plain", Encoding.UTF8);
        }
        public ActionResult LoginExample()
        {
            return View();
        }

        public ActionResult test1()
        {

            var b = from userTable in db.users
                    join ticketTable in db.tickets on userTable.UID equals ticketTable.UID
                    select new
                    {
                        name = userTable.name,
                        phone = userTable.phone,

                    };
            return Json(new { code = 200, data = b }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult test2()
        {

            var b = from userTable in db.users
                    join ticketTable in db.tickets on userTable.UID equals ticketTable.UID 
                    select new { 
                        name = userTable.name , phone = userTable.phone ,

                    } ;
            return Json( new { code = 200 , data = b }, JsonRequestBehavior.AllowGet);
        }
    }
}