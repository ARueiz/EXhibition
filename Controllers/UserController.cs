using EXhibition.Filters;
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
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //票卷列表
        public ActionResult MyTicketList()
        {
            return View();
        }

        //票卷細節
        public ActionResult MyTicketDetail()
        {
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

    }
}