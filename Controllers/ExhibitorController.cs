using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class ExhibitorController : Controller
    {
        // 廠商 首頁
        public ActionResult Index()
        {
            return View();
        }

        // 廠商 參加展覽
        public ActionResult JoinExhibition()
        {
            return View();
        }

    }
}