﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
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

        public ActionResult MyTicketDetail()
        {
            return View();
        }

        public ActionResult ScanLoginQRCode()
        {
            return View();
        }

    }
}