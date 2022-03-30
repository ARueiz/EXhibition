using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EXhibition.Models;

namespace EXhibition.Controllers
{
    public class ExhibitorController : Controller
    {
        DBConnector db = new DBConnector();

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

        //展覽列表展示
        public ActionResult showHostList()
        {
            return View();
        }

        //廠商 可申請展覽
        public ActionResult CanApplyList(int? id=2)
        {
            
            ViewBag.EID = id;
            return View();
        }

        //廠商 正進行審核的活動列表
        public ActionResult NowApplying(int? id=2)
        {
            ViewBag.EID = id;
            return View();
        }
    }
}