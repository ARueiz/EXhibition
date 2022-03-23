using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class PartViewController : Controller
    {
        // GET: PartView
        public ActionResult Navbar()
        {

            if (Session["auth"] == null)
                return PartialView("_NavbarVisitor");
            else
                return PartialView("_NavbarUser");
        }
    }
}