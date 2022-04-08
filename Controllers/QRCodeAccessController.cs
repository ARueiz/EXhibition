using EXhibition.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

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

            HttpContext.Current.Session[Models.GlobalVariables.AccountId] = info.accountId;
            HttpContext.Current.Session["UserRole"] = info.accountType.ToString();

            ReturnData returnData = new ReturnData()
            {
                status = ReturnStatus.Success,
                message = "成功登入",
                data = new { url = "/user", info = info }  // 給予對應的後台畫面
            };

            return Ok(returnData);
        }

        // 沒有登入，瀏覽器取得辨識用的 token
        public IHttpActionResult GetLoginToken()
        {
            LoginToken data = LoginToken.getToken();
            return Ok(data);
        }

        // 以登入，用登入帳戶去資料庫儲存 token 和 使用者資訊
        public IHttpActionResult PostSaveToken([FromBody] LoginToken token)
        {
            if (HttpContext.Current.Session[GlobalVariables.AccountId] == null
                || token == null || token.token == null)
            {
                return Ok(new ReturnData() { status = ReturnStatus.Error, data = token, message = "沒有登入資訊、token 錯誤" });
            }
            var account = new Account();
            account.Id = (int)HttpContext.Current.Session[GlobalVariables.AccountId];
            account.accountType = (string)HttpContext.Current.Session["UserRole"];

            db.QRCodeLoginToken.Add(new QRCodeLoginToken()
            {
                token = token.token,
                accountId = account.Id,
                accountType = account.accountType,
                createAt = DateTime.Now 
            });
            db.SaveChanges();

            return Ok(new ReturnData() { status = ReturnStatus.Success, message = "成功" });
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostCheckTicket([FromBody] Models.CheckTicket getTicketData)
        {

            if (getTicketData == null) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "錯誤" });

            var t = db.Tickets.Find(getTicketData.Id);

            if (t == null) // 找不到票券
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "找不到票券" });

            //else if (t.EVID != getTicketData.TicketEventId) // 票券 id 與 該場次 id 不符
            //    return Ok(new ReturnData() { status = ReturnStatus.Error, message = "票券 id 與 該場次 id 不符" });

            else if (t.token.Equals(getTicketData.Token) == false) // token 不相符
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "驗證碼不符" });

            else if (t.EVID.Equals(getTicketData.EVID) == false) // token 不相符
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "場次不符" });

            //else if (t.createAt < DateTime.Now)
            //    return Ok(new ReturnData() { status = ReturnStatus.Error, message = "驗證逾期" });


            return Ok(new { ticket = getTicketData, status = "success", message = "票券驗證成功" });
        }

    }
}
