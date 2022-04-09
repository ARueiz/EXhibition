using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.User)]
    public class UserController : Controller
    {
        DBConnector db = new DBConnector();

        // GET: User
        public ActionResult Index()
        {
            int UID = (int)Session[GlobalVariables.AccountID];

            var user = db.users.Find(UID);

            ViewBag.username = user.name;
            
            return View();
            
            
            //return RedirectToAction("MyTicketList", "User", null);
        }

        //票卷列表
        public ActionResult MyTicketList()
        {
            return View();
        }

        //票卷細節
        public ActionResult MyTicketDetail(int? TID)
        {
            ViewBag.TID = (int)TID;

            return View();
        }

        public ActionResult EditUser()
        {
            return View();
        }

        public ActionResult ScanLoginQRCode()
        {
            return View();
        }

        //展覽詳細內容
        public ActionResult EventDetail(int? id=16)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult ConsumingRecords()
        {
            return View();
        }

    }
}