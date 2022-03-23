using EXhibition.Models;
using System;
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
            return View();
        }

        public ActionResult UserLogin()
        {
            return View();
        }

        public ActionResult ExhibtiorLogin()
        {
            return View();
        }

        public ActionResult HostLogin()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["auth"] = null;
            return RedirectToAction("index");
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

        public ActionResult test3(Models.hosts hosts)
        {

            var returnData = new Models.ReturnData();

            if (string.IsNullOrEmpty(hosts.name))
            {
                returnData.status = "error";
                returnData.message = "姓名不可為空";
                return Json(returnData, JsonRequestBehavior.AllowGet);
            }
            returnData.status = "success";
            returnData.message = "註冊成功";
            var h = new hosts();
            h.email = "1234234";
            returnData.data = h;
            return Json(returnData, JsonRequestBehavior.AllowGet);
        }

        public class ttt
        {
            public int 編號 { get; set; }
            public string 電子郵件 { get; set; }
            public double price { get; set; }
        }

        public ActionResult test4(int? id = 1)
        {
            DateTime t = DateTime.Parse("2022/02/01");
            var a = db.users.Select(e => new {
                編號 = e.UID,
                電子郵件 = e.email,
                price = 2500
            }).First();
            //a.price = a.price * 0.7;
            return Json(a, JsonRequestBehavior.AllowGet);
        }


        public ActionResult test5()
        {
            var a = db.users.Join(db.tickets, ticketTable => ticketTable.UID,
                userTable => userTable.UID,
                (userTable, ticketTable) => new
                {
                    ticketId = ticketTable.UID,
                    UserName = userTable.name,
                    isPay = ticketTable.paid
                });
            return Json(a, JsonRequestBehavior.AllowGet);
        }


        public string test6()
        {
            var a = db.users.Join(db.tickets, ticketTable => ticketTable.UID,
                userTable => userTable.UID,
                (userTable, ticketTable) => new
                {
                    ticketId = ticketTable.UID,
                    UserName = userTable.name,
                    isPay = ticketTable.paid
                }).ToString();
            return a;
        }

        public ActionResult test7(Models.users user)
        {
            user.password = "11111111";
            user.verify = true;

            db.users.Add(user);
            var a = db.SaveChanges();

            return Json(user,JsonRequestBehavior.AllowGet);
        }

        public ActionResult test8(Models.users user)
        {
            user.password = "11111111";
            user.verify = true;

            db.users.Add(user);
            var a = db.SaveChanges();

            return Json(user,JsonRequestBehavior.DenyGet);
        }



    }
}