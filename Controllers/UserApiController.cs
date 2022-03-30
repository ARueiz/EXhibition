using EXhibition.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class UserApiController : Controller
    {

        DBConnector db = new DBConnector();

        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            ReturnData r = new ReturnData();
            r.message = "登入成功";
            r.status = ReturnStatus.Success;
            r.data = new { url = "/", mylogin = login };
            Session["auth"] = 1;
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(Models.users user)
        {
            user.UID = 0;
            ReturnData r = new ReturnData();
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

        public ActionResult GetUserInfo(int? id)
        {
            Session["UID"] = 2;

            int UID = Convert.ToInt32(Session["UID"]);

            var list = (from user in db.users
                        where user.UID == UID && user.verify == true
                        select new
                        {
                            UID = user.UID,
                            name = user.name,
                            phone = user.phone,
                            email = user.email,
                        }).ToList();

            return new NewJsonResult() { Data = list[0] };
        }

        public ActionResult DoUpdateUserInfo(users user)
        {
            users updateUser = db.users.FirstOrDefault(u => u.UID == user.UID);
            ReturnData data = new ReturnData();

            if (updateUser != null)
            {
                updateUser.name = user.name;
                updateUser.phone = user.phone;
                updateUser.email = user.email;

                try
                {
                    db.SaveChanges();
                    data.status = ReturnStatus.Success;
                    data.message = "更新成功!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    data.status = ReturnStatus.Error;
                    data.message = "更新失敗!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.status = ReturnStatus.Error;
                data.message = "更新失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
    }
}