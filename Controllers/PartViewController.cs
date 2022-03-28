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
            else if ((int)Session["auth"] == 3)
                return PartialView("_NavbarHost");
            else if ((int)Session["auth"] == 2)
                return PartialView("_NavbarExhibitor");
            else if ((int)Session["auth"] == 1)
                return PartialView("_NavbarUser");
            else
                return PartialView("_NavbarVisitor");

        }

        public ActionResult SideBarBtn()
        {
            int authId = Session["auth"] == null ? 0 : (int)Session["auth"];
            if (authId == 1)
            {
                return PartialView("_SideBarUserBtn");
            }
            else if (authId == 2)
            {
                return PartialView("_SideBarExhibitorBtn");
            }
            else
            {
                return PartialView("_SideBarHostBtn");
            }
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
                return PartialView("_SideBarExhibitor");
            }
            else
            {
                return PartialView("_SideBarHost");
            }
        }

        public ActionResult Footer()
        {
            return PartialView("_Footer");
        }

    }
}