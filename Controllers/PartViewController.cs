using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class PartViewController : Controller
    {
        // GET: PartView
        public ActionResult Navbar()
        {
            return PartialView();
        }
    }
}