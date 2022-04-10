using System.Linq;
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

        public async System.Threading.Tasks.Task<ActionResult> CheckoutSuccess(string token)
        {
            if (Session[Models.GlobalVariables.AccountID] == null || token == null )
            {
                return RedirectToAction("Index");
            }
            Models.DBConnector db = new Models.DBConnector();
            var authorizeOrderResponse = await Repo.PayPalClient.AuthorizeOrder(token);
            var authorizeOrderResult = authorizeOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();
            var authorizationId = authorizeOrderResult.PurchaseUnits[0].Payments.Authorizations[0].Id;
            var captureOrderResponse = await Repo.PayPalClient.CaptureOrder(authorizationId);
            var order = db.orders.Where(e=>e.paypal_Id == token).FirstOrDefault();
            if(order == null) return RedirectToAction("Index");
            order.isPay = true;
            db.SaveChanges();
            return View();
        }

      
    }
}