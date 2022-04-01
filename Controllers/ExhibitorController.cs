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

        // showHostList -> 綠色(展覽細節)按鈕
        public ActionResult showEventDetail(int? EVID)
        {
            ViewBag.EVID = EVID;

            
            return View();
        }


        //廠商 可申請展覽
        public ActionResult CanApplyList(int? id=10)
        {
            
            ViewBag.EID = id;
            return View();
        }

        //廠商 正進行審核的活動列表
        public ActionResult NowApplying(int? id=10)
        {
            ViewBag.EID = id;
            return View();
        }
        //廠商 編輯個人資料
        public ActionResult EditExhibitor()
        {

            return View();
        }

    }
}