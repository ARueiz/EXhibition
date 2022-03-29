using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EXhibition.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShopApiController : ApiController
    {

        Models.DBConnector db = new Models.DBConnector();


        // 如果 在 webapiconfig 的 routeTemplate: "api/{controller}/{action}/{id}" 有加上 /{id} 則 id 會自動帶入
        // 如果 沒有則需要加上 [FromBody] 去取得 帶進來的 json 檔案        
        public IHttpActionResult GetTicketList(int? id)
        {
            if (id == null) { id = 0; }
            int num = (int)id;
            var list = (from eve in db.events orderby eve.startdate descending select eve).Skip(num).Take(12).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].image = "/image/Host/" + list[i].image;
            }
            return Json(list);
        }

        public IHttpActionResult GetNewTicketList()
        {
            var b = (from eve in db.events orderby eve.startdate descending select eve).Take(4).ToList();
            return Json(b);
        }

        public IHttpActionResult GetHotTicketList()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select top(5) count(A.TID) , B.name " +
                "from Tickets as A inner join events as B on A.EVID = B.EVID " +
                "group by B.EVID , B.name order by 1 desc";

            List<events> eventlist = new List<events>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        eventlist.Add(new Models.events()
                        {

                        });
                        Console.WriteLine("\t{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Json(eventlist);
        }

        // 取得存在 session 內的購物車資訊 (此 id 為產品的 id)
        public IHttpActionResult PostAddCartItem(int? id)
        {

            if (id == null) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "null id", data = id });

            // 區域變數
            List<CartItem> list;
            string cartItem = GlobalVariables.CartItems;

            // 查詢是否有資料
            var oEvent = db.events.Where(i => i.EVID == id).FirstOrDefault();

            // null 則 報錯
            if (oEvent == null) return Ok(new ReturnData(ReturnStatus.Error, "id not find", id));

            if (HttpContext.Current.Session[cartItem] == null)
            {
                CartItem item = new CartItem(oEvent);
                item.cartId = 0;
                list = new List<CartItem>();
                list.Add(item);
                HttpContext.Current.Session[cartItem] = list;
            }
            else
            {
                list = (List<CartItem>)HttpContext.Current.Session[cartItem];
                CartItem item = new CartItem(oEvent);
                item.cartId = list.Count;
                list.Add(item);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].cartId = i;
                }
                HttpContext.Current.Session[cartItem] = list;
            }

            return Ok(list);
        }

        // 移除 購物車內的產品 (此 id 為 session 陣列中排行的編號，請帶入 cartId)
        public IHttpActionResult PostRemoveCartItem(int? id)
        {
            if (id == null) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "null id" });

            // 區域變數
            string cartItem = GlobalVariables.CartItems;
            List<CartItem> list = new List<CartItem>();

            if (HttpContext.Current.Session[cartItem] == null)
            {
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "刪除 id 錯誤" });
            }
            else
            {
                list = (List<CartItem>)HttpContext.Current.Session[cartItem];

                // 防止超過陣列長度
                if ((int)id > list.Count) return Ok(new ReturnData() { status = ReturnStatus.Error, message = "null id" });

                list.RemoveAt((int)id);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].cartId = i;
                }
                HttpContext.Current.Session[cartItem] = list;
            }

            return Ok(list);
        }

    }
}
