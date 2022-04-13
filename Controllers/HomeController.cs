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

            var twtzinfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime localdt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, twtzinfo);

            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ip == null) ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                      
            db.request_log.Add(new Models.request_log() { access_time = localdt , client_ip = ip  });
            db.SaveChanges();

            if (Session["UserRole"] == null)
            {
                Session["UserRole"] = "Visitor";
            }
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
        public ActionResult ExhibitorLogin()
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
            Session["UserRole"] = null;
            return RedirectToAction("index");
        }

        public ActionResult EventDetail(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult GetEnv()
        {
            var a = Environment.GetEnvironmentVariable("myValue");
            ViewBag.data = a;
            return View();
        }

        public ActionResult DenyAuthorize()
        {
            ViewBag.sessionUserRole = Session["UserRole"];

            return PartialView();
        }

        public ActionResult HostForgetPassword()
        {
            return View();
        }

        public ActionResult ExhibitorForgetPassword()
        {
            return View();
        }

        public ActionResult UserForgetPassword()
        {
            return View();
        }

        public ActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HostForgetPassword(string email)
        {
            string uuid = Guid.NewGuid().ToString();
            db.forgetPassword.Add(new forgetPassword() { email = email, uuid = uuid, userType = "Host" });
            db.SaveChanges();

            // 送信 
            Repo.SendResetEmailRepo.SendResetEmail(email,uuid);

            return RedirectToAction("SendEmail");
        }

        [HttpPost]
        public ActionResult ExhibitorForgetPassword(string email)
        {
            string uuid = Guid.NewGuid().ToString();
            db.forgetPassword.Add(new forgetPassword() { email = email, uuid = uuid, userType = "Exhibitor" });
            db.SaveChanges();

            // 送信 
            Repo.SendResetEmailRepo.SendResetEmail(email, uuid);

            return RedirectToAction("SendEmail");
        }

        [HttpPost]
        public ActionResult UserForgetPassword(string email)
        {
            string uuid = Guid.NewGuid().ToString();
            db.forgetPassword.Add(new forgetPassword() { email = email, uuid = uuid, userType = "User" });
            db.SaveChanges();

            // 送信 
            Repo.SendResetEmailRepo.SendResetEmail(email, uuid);

            return RedirectToAction("SendEmail");
        }

        public ActionResult ResetPassword(string uuid = "sv1b-f7sa-bb15-54dn")
        {
            ViewBag.uuid = uuid;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string uuid, string password)
        {
            var a = db.forgetPassword.Where(e => e.uuid == uuid).FirstOrDefault();
            if (a == null) return Redirect("/");
            if (a.userType == "User")
            {
                var user = db.users.Where(u=>u.email == a.email ).FirstOrDefault();
                if (user == null) { return Redirect("/"); }
                user.password = Models.SHA_256.ComputeSha256Hash(password);
                db.SaveChanges();
                var b = db.forgetPassword.Where(e => e.uuid == uuid).FirstOrDefault();
                db.forgetPassword.Remove(b);
                db.SaveChanges();
            }
            else if (a.userType == "Host")
            {
                var host = db.hosts.Where(u => u.email == a.email).FirstOrDefault();
                if (host == null) { return Redirect("/"); }
                host.password = Models.SHA_256.ComputeSha256Hash(password);
                db.SaveChanges();
                var b = db.forgetPassword.Where(e => e.uuid == uuid).FirstOrDefault();
                db.forgetPassword.Remove(b);
                db.SaveChanges();
            }
            else if (a.userType == "Exhibitor")
            {
                var exhibitor = db.exhibitors.Where(u => u.email == a.email).FirstOrDefault();
                if (exhibitor == null) { return Redirect("/"); }
                exhibitor.password = Models.SHA_256.ComputeSha256Hash(password);
                db.SaveChanges();
                var b = db.forgetPassword.Where(e => e.uuid == uuid).FirstOrDefault();
                db.forgetPassword.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("ResetPasswordSuccess");
        }

        public ActionResult ResetPasswordSuccess()
        {
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
            var ExhibitorItem = db.exhibitinfo.Where(t => t.EID == d.EID).Where(t => t.EVID == d.EVID).First();

            if (d.isVerified == true)
            {
                ExhibitorItem.status = "允許";
            }
            else if (d.isVerified == false)
            {
                ExhibitorItem.status = "拒絕";
            }

            db.SaveChanges();


            return Json(null, JsonRequestBehavior.DenyGet);
        }

        public ActionResult test12()
        {
            var d = DateTime.Now;
            d = d.AddDays(-5);
            var a = db.events.Where(e => e.startdate < d).ToList();
            return Json(a, JsonRequestBehavior.AllowGet);
        }

    }
}