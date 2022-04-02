using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class PartViewController : Controller
    {
        // GET: PartView
        public ActionResult Navbar()
        {

            if (Session["UserRole"] == null)
                return PartialView("_NavbarVisitor");
            else if (Session["UserRole"].ToString() == "Host")
                return PartialView("_NavbarHost");
            else if (Session["UserRole"].ToString() == "Exhibitor")
                return PartialView("_NavbarExhibitor");
            else if (Session["UserRole"].ToString() == "User")
                return PartialView("_NavbarUser");
            else
                return PartialView("_NavbarVisitor");

        }

        public ActionResult SideBarBtn()
        {
            if (Session["UserRole"] == null)
            {
                Session["UserRole"] = "Visitor";
                Session["AccountID"] = 2;
            }

            if (Session["UserRole"].ToString() == "User")
            {
                return PartialView("_SideBarUserBtn");
            }
            else if (Session["UserRole"].ToString() == "Exhibitor")
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
            if (Session["UserRole"].ToString() == "User")
            {
                return PartialView("_SideBarUser");
            }
            else if (Session["UserRole"].ToString() == "Exhibitor")
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