using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EXhibition.Controllers
{
    public class QRCodeAccessController : ApiController
    {
        Models.DBConnector db = new Models.DBConnector();

        public IHttpActionResult PostLogin([FromBody]Models.LoginToken token)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["token"] = token;
            data["status"] = 200;
            return Ok(data);
        }

        public IHttpActionResult GetLoginToken()
        {
            Models.LoginToken data = new Models.LoginToken();
            var s = DateTime.Now.GetHashCode().ToString();
            data.token = Guid.NewGuid().ToString() ;
            //data.token = data.token +""+s;
            db.QRCodeLoginToken.Add(new Models.QRCodeLoginToken() { token = data.token , createTime = DateTime.Now.AddHours(-1)});
            db.SaveChanges();
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            //dict["token"] = data.token;
            //dict["DateTimeNowHash"] = s ;
            //dict["TokenLength"] = data.token.Length;
            return Ok(data) ;
        }
    }
}
