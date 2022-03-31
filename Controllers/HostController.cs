using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.Host)]
    public class HostController : Controller
    {
        private DBConnector db = new DBConnector();
        // GET: Hosts
        public ActionResult publicVersion()
        {
            return View();
        }

        //黃亭愷
        public ActionResult Index()
        {
            return View();
        }

        //林昶廷  主辦單位>顯示此主辦方所有的展覽列表
        public ActionResult ShowHostEvent()
        {
            return View();
        }

        public ActionResult DetailsHost(int? id)
        {

            if (id == null)
            {
                id = 0;
            }
            ViewBag.id = id;
            return View();
        }

        //馬誠遠
        public ActionResult EditEvent()
        {
            return View();
        }

        public ActionResult searchTag()
        {
            var tag = db.TagsName.OrderBy(fer=>fer.id).Take(10).ToList();

            return Json(tag,JsonRequestBehavior.AllowGet);
        }

        public ActionResult searchengine(string searchstring)
        {
            var tag = db.TagsName.OrderBy(fer => fer.id).Where(fernando=>fernando.tagName.Contains(searchstring)).Take(10).ToList();



            return Json(tag,JsonRequestBehavior.AllowGet);
        }
       

        //邱品叡
        public ActionResult CreateEvent()
        {
            Session["HID"] = 2;

            int HID = (int)Session["HID"];

            var info = db.hosts.Where(h => h.HID == HID).Select(h => new
            {
                h.name
            }).ToList();

            ViewBag.hostname = info[0].name;
            ViewBag.sessionHID = Session["HID"];

            return View();
        }

        // 邱品叡 主辦單位>修改個人資料
        public ActionResult EditHostInfo()
        {

            return View();
        }


        // 洪奕生 主辦單位>展覽列表>查看資訊
        public ActionResult EventDetail()
        {
            Session["HID"] = 2;

            int HID = (int)Session["HID"];

            var info = db.hosts.Where(h => h.HID == HID).ToList();

            ViewBag.name = info[0].name;

            return View();
        }

    }
}