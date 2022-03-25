using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EXhibition.Controllers
{
    public class ShopApiController : ApiController
    {

        Models.DBConnector db = new Models.DBConnector();

        public IHttpActionResult GetTicketList()
        {
            var b = (from eve in db.events orderby eve.startdate descending select eve ).Take(12).ToList();
            return Json(b);
        }
    }
}
