using EXhibition.Models;
using System.Linq;
using System.Web.Http;

namespace EXhibition.Controllers
{
    public class QRCodeAccessController : ApiController
    {
        DBConnector db = new DBConnector();

        // 沒有登入，瀏覽器尋找 token
        public IHttpActionResult PostLogin([FromBody] LoginToken data)
        {

            if (data == null || data.token == null)
            {
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "token 為 null" });
            }

            var info = db.QRCodeLoginToken.Where(x => x.token == data.token).FirstOrDefault();

            if (info == null)
            {
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "找不到 token" });
            }
            else
            {
                db.QRCodeLoginToken.Remove(info);
                db.SaveChanges();
            }

            //---- 去資料庫依照用戶 Type 去對應table尋找用戶 Id
            //if (info.accountType.ToUpper().Equals(GlobalVariables.User.ToUpper()))
            //{
            //    var u = db.users.Where(e => e.UID == info.accountId).FirstOrDefault();
            //    if (u == null) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "錯誤找不到用戶" });
            //}
            //else if (info.accountType.ToUpper().Equals(GlobalVariables.Exhibitor.ToUpper()))
            //{
            //    var h = db.hosts.Where(e => e.HID == info.accountId).FirstOrDefault();
            //    if (h == null) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "錯誤找不到用戶" });
            //}
            //else if (info.accountType.ToUpper().Equals(GlobalVariables.Host.ToUpper()))
            //{
            //    var ex = db.exhibitors.Where(e => e.EID == info.accountId).FirstOrDefault();
            //    if (ex == null) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "錯誤找不到用戶" });
            //}


            ReturnData returnData = new ReturnData()
            {
                status = ReturnStatus.Success,
                message = "成功登入",
                data = new { url = "/user" , info = info }  // 給予對應的後台畫面
            };

            return Ok(returnData);
        }

        // 沒有登入，瀏覽器取得辨識用的 token
        public IHttpActionResult GetLoginToken()
        {
            LoginToken data = LoginToken.getToken();
            //HttpContext.Current.Session[GlobalVariables.QRcodeToken] = data.token;
            //data.token = data.token +""+s;
            //db.QRCodeLoginToken.Add(new Models.QRCodeLoginToken() { token = data.token , createAt = DateTime.Now.AddHours(-1)});
            //db.SaveChanges();
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            //dict["token"] = data.token;
            //dict["DateTimeNowHash"] = s ;
            //dict["TokenLength"] = data.token.Length;
            return Ok(data);
        }

        // 以登入，用登入帳戶去資料庫儲存 token 和 使用者資訊
        public IHttpActionResult PostSaveToken([FromBody] LoginToken token)
        {
            //if (HttpContext.Current.Session[GlobalVariables.AccountId] == null
            //    || token == null || token.token == null )
            //{
            //    return Ok(new ReturnData() { status = RetrunStatus.Error,data= token, message = "沒有登入資訊、token 錯誤" });
            //}
            var account = new Account() { Id = 1, accountType = "user" };
            //account.Id = (int)HttpContext.Current.Session[GlobalVariables.AccountId];
            //account.accountType = (string)HttpContext.Current.Session[GlobalVariables.AccountType];

            db.QRCodeLoginToken.Add(new QRCodeLoginToken()
            {
                token = token.token,
                accountId = account.Id,
                accountType = account.accountType
            });
            db.SaveChanges();

            return Ok(new ReturnData() { status = ReturnStatus.Success, message = "成功" });
        }

        public IHttpActionResult PostCheckTicket(Models.CheckTicket ticket)
        {
            
            if (ticket == null) return Ok(new ReturnData() { status = ReturnStatus.Error });



            return Ok("1234");
        }

    }
}
