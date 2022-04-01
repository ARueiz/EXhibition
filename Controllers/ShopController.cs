using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Product(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult CheckoutSuccess()
        {
            return View();
        }

      
    }
}