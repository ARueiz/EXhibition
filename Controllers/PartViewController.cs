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

        public ActionResult SideBar()
        {
            int authId = Session["auth"] == null ? 1 : (int)Session["auth"];
            if (authId == 1)
            {
                return PartialView("_SideBarUser");
            }
            else if (authId == 2)
            {
                return PartialView("_SideBarHost");
            }
            else
            {
                return PartialView("_SideBarExhibitor");
            }
        }

    }
}