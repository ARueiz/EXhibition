using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Shopcart()
        {
            return View();
        }

        public ActionResult Product(int? id)
        {
            ViewBag.Id = id;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult CheckoutSuccess()
        {
            return View();
        }

      
    }
}