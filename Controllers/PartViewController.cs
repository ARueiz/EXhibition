using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class PartViewController : Controller
    {
        // GET: PartView
        public ActionResult Navbar()
        {
            int x = 1;
            if (x == 1)
                return PartialView("_NavbarUser");
            else
                return PartialView("_NavbarVisitor");
        }
    }
}