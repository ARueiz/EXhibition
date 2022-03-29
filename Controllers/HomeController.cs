﻿using EXhibition.Models;
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

        // 使用者登入
        public ActionResult UserLogin()
        {
            return View();
        }

        // 廠商登入
        public ActionResult ExhibitiorLogin()
        {
            return View();
        }

        // 主辦單位登入
        public ActionResult HostLogin()
        {
            return View();
        }

        // 展覽列表
        public ActionResult ExhibitionList()
        {
            return View();
        }

        // 註冊 主辦單位
        public ActionResult RegisterHost()
        {
            return View();
        }

        // 註冊 廠商
        public ActionResult RegisterExhibitor()
        {
            return View();
        }

        // 註冊 用戶
        public ActionResult RegisterUser()
        {
            return View();
        }

        // 登出
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
                    join ticketTable in db.Tickets on userTable.UID equals ticketTable.UID
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
                    join ticketTable in db.Tickets on userTable.UID equals ticketTable.UID
                    select new
                    {
                        name = userTable.name,
                        phone = userTable.phone,

                    };
            return Json(new { code = 200, data = b }, JsonRequestBehavior.AllowGet);
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
            var a = db.users.Select(e => new
            {
                編號 = e.UID,
                電子郵件 = e.email,
                price = 2500
            }).First();
            //a.price = a.price * 0.7;
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public ActionResult test5()
        {
            var a = db.users.Join(db.Tickets, ticketTable => ticketTable.UID,
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
            var a = db.users.Join(db.Tickets, ticketTable => ticketTable.UID,
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

            return Json(user, JsonRequestBehavior.AllowGet);
        }
        public ActionResult test8(Models.users user)
        {
            user.password = "11111111";
            user.verify = true;

            db.users.Add(user);
            var a = db.SaveChanges();

            return Json(user, JsonRequestBehavior.DenyGet);
        }

        public class GetData
        {
            public int EVID { get; set; }
            public int EID { get; set; }
            public bool isVerified { get; set; }
        }

        public ActionResult test11(GetData d)
        {
            var ExhibitiorItem = db.exhibitinfo.Where(t => t.EID == d.EID).Where(t => t.EVID == d.EVID).First();

            if (d.isVerified == true)
            {
                ExhibitiorItem.status = "允許";
            }
            else if(d.isVerified == false)
            {
                ExhibitiorItem.status = "拒絕";
            }

            db.SaveChanges();


            return Json(null, JsonRequestBehavior.DenyGet);
        }

        public ActionResult test12()
        {
            var d = DateTime.Now;
            d = d.AddDays(-5);
            var a = db.events.Where(e => e.startdate < d).ToList();
            return Json(a,JsonRequestBehavior.AllowGet);
        }


    }
}