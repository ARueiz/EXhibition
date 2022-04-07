using System.Linq;
using System.Web.Mvc;
using EXhibition.Filters;
using EXhibition.Models;

namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.Exhibitor)]
    public class ExhibitorController : Controller
    {
        DBConnector db = new DBConnector();

        // 廠商 首頁
        public ActionResult Index()
        {
            var id = (int) Session[Models.GlobalVariables.AccountID];
            ViewBag.AccountName = db.exhibitors.Find(id).name ;
            return View();
            //return RedirectToAction("showHostList", "Exhibitor", null);
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

        // showHostList -> 綠色(展覽細節)按鈕
        public ActionResult showEventDetail(int? EVID)
        {
            ViewBag.EVID = EVID;

            
            return View();
        }

        // showHostList -> 紅色(申請展覽)按鈕
        public ActionResult createEventInfo(int? EVID)
        {
            int EID = (int)Session["AccountID"];
            ViewBag.EVID = EVID;
            ViewBag.EID = EID;

            var events = db.events.Where(ev => ev.EVID == EVID).FirstOrDefault();

            var exhibitor = db.exhibitors.Where(e => e.EID == EID).FirstOrDefault();

            ViewBag.eventname = events.name;
            ViewBag.name = exhibitor.name;
            
            return View();
        }


        //廠商 可申請展覽
        public ActionResult CanApplyList(int? id)
        {
            
            ViewBag.EID = Session["AccountID"];
            return View();
        }

        //廠商 正進行審核的活動列表
        public ActionResult NowApplying(int? id)
        {
            ViewBag.id = Session["AccountID"];
            return View();
        }
        //廠商 編輯個人資料
        public ActionResult EditExhibitor()
        {

            return View();
        }

        //廠商申請參展歷史紀錄
        public ActionResult ApplyHistory()
        {
            int EID = (int)Session["AccountID"];
            ViewBag.id = EID;
            return View();
        }

    }
}