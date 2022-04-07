using EXhibition.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class HomeApiController : Controller
    {
        DBConnector db = new DBConnector();

        //-----------------使用者登入註冊------------------//
        [HttpPost]
        public ActionResult UserLogin(Models.Login login)
        {
            ReturnData r = new ReturnData();

            if (string.IsNullOrEmpty(login.account) || string.IsNullOrEmpty(login.password))
            {
                r.message = "登入失敗";
                r.status = ReturnStatus.Error;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    string hashPwd = SHA_256.ComputeSha256Hash(login.password);

                    var loginData = (from user in db.users
                                     where user.email == login.account && user.password == hashPwd && user.verify == true
                                     select new
                                     {
                                         UID = user.UID,
                                     }).ToList();
                    if (loginData.Any())
                    {
                        r.message = "登入成功";
                        r.status = ReturnStatus.Success;
                        Session["UserRole"] = "User";
                        Session["AccountID"] = loginData[0].UID;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        r.message = "帳號或密碼錯誤，查無此人";
                        r.status = ReturnStatus.Error;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    r.message = "帳號或密碼錯誤";
                    r.status = ReturnStatus.Error;
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult UserRegister(Models.users user)
        {
            ReturnData r = new ReturnData();
            user.password = SHA_256.ComputeSha256Hash(user.password);
            user.verify = true;

            users u = db.users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                r.status = ReturnStatus.Error;
                r.message = "註冊失敗";
                r.data = u;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            r.status = ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/UserLogin" };
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        //-----------------參展商登入註冊------------------//
        public ActionResult ExhibitorLogin(Models.Login login)
        {
            ReturnData r = new ReturnData();

            if (string.IsNullOrEmpty(login.account) || string.IsNullOrEmpty(login.password))
            {
                r.message = "登入失敗";
                r.status = ReturnStatus.Error;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    string hashPwd = SHA_256.ComputeSha256Hash(login.password);

                    var loginData = (from exhibitor in db.exhibitors
                                     where exhibitor.email == login.account && exhibitor.password == hashPwd && exhibitor.verify == true
                                     select new
                                     {
                                         EID = exhibitor.EID,
                                     }).ToList();
                    if (loginData.Any())
                    {
                        r.message = "登入成功";
                        r.status = ReturnStatus.Success;
                        Session["UserRole"] = "Exhibitor";
                        Session["AccountID"] = loginData[0].EID;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        r.message = "帳號或密碼錯誤，查無此人";
                        r.status = ReturnStatus.Error;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    r.message = "帳號或密碼錯誤";
                    r.status = ReturnStatus.Error;
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult ExhibitorRegister(Models.exhibitors exhibitor)
        {
            Models.ReturnData r = new Models.ReturnData();
            try
            {
                exhibitor.password = SHA_256.ComputeSha256Hash(exhibitor.password);
                exhibitor.verify = true;

                var e = db.exhibitors.Add(exhibitor);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                r.status = Models.ReturnStatus.Error;
                r.message = "註冊失敗";
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            r.status = Models.ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/ExhibitorLogin" };
            return Json(r, JsonRequestBehavior.AllowGet);
        }




        //-----------------主辦方登入註冊------------------//
        public ActionResult HostLogin(Models.Login login)
        {
            ReturnData r = new ReturnData();

            if (string.IsNullOrEmpty(login.account) || string.IsNullOrEmpty(login.password))
            {
                r.message = "登入失敗";
                r.status = ReturnStatus.Error;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    string hashPwd = SHA_256.ComputeSha256Hash(login.password);

                    var loginData = (from host in db.hosts
                                     where host.email == login.account && host.password == hashPwd && host.verify == true
                                     select new
                                     {
                                         HID = host.HID,
                                     }).ToList();
                    if (loginData.Any())
                    {
                        r.message = "登入成功";
                        r.status = ReturnStatus.Success;
                        r.data = loginData;
                        Session["UserRole"] = "Host";
                        Session["AccountID"] = loginData[0].HID;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        r.message = "帳號或密碼錯誤，查無此人";
                        r.status = ReturnStatus.Error;
                        r.data = loginData;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    r.message = "帳號或密碼錯誤";
                    r.status = ReturnStatus.Error;
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult HostRegister(Models.hosts host)
        {
            Models.ReturnData r = new ReturnData();

            host.password = SHA_256.ComputeSha256Hash(host.password);
            host.image = "EditHost.png";
            host.verify = true;

            db.hosts.Add(host);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                r.status = Models.ReturnStatus.Error;
                r.message = "註冊失敗";
                return Json(r, JsonRequestBehavior.AllowGet);
            }

            r.status = Models.ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/HostLogin" };
            return Json(r);
        }

        //顯示展覽細節
        public ActionResult EventDetail(int? id)
        {
            ReturnData rd = new ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var list = (from host in db.hosts
                        join events in db.events on host.HID equals events.HID
                        where events.EVID == id
                        select new
                        {
                            organizer = host.name,
                            events.EVID,
                            title = events.name,
                            start = events.startdate.ToString(),
                            end = events.enddate.ToString(),
                            location = events.venue,
                            price = events.ticketprice 
                        }).ToList();
        
            if (!list.Any())
            {
                rd.message = "找不到資料";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            rd.message = "成功";
            rd.status = ReturnStatus.Success;
            rd.data = list;
            return Json(rd, JsonRequestBehavior.AllowGet);
        }
    }
}