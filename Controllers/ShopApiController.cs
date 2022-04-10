using EXhibition.Models;
using EXhibition.Repo;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EXhibition.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShopApiController : ApiController
    {

        Models.DBConnector db = new Models.DBConnector();


        // 如果 在 webapiconfig 的 routeTemplate: "api/{controller}/{action}/{id}" 有加上 /{id} 則 id 會自動帶入
        // 如果 沒有則需要加上 [FromBody] 去取得 帶進來的 json 檔案        
        public IHttpActionResult GetTicketList(int? id)
        {
            if (id == null || id <= 1) { id = 1; }
            int num = (int)id;
            num = (num - 1) * 12;
            DateTime now = DateTime.Now;
            var list = (from eve in db.events where eve.startdate > now orderby eve.startdate descending select eve).Skip(num).Take(12).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].image = "/image/Host/" + list[i].image;
            }
            return Json(list);
        }

        public IHttpActionResult GetNewTicketList()
        {
            var b = (from eve in db.events orderby eve.createAt descending select eve).Take(4).ToList();
            foreach (var item in b)
            {
                item.image = "/image/host/" + item.image;
            }
            return Ok(b);
        }

        // 查詢熱門票券
        public IHttpActionResult GetHotTicketList()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select top(4) count(A.TID) , B.EVID from Tickets as A inner join events as B on A.EVID = B.EVID where B.startdate > GETDATE() group by B.EVID , B.name order by 1 desc";

            // 先將 id 撈成 陣列後 用 entity framework 去找資料

            List<events> eventlist = new List<events>();
            List<int> eventIdList = new List<int>();

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
                        eventIdList.Add((int)reader[1]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            eventlist = db.events.Where(item => eventIdList.Contains(item.EVID)).ToList();
            foreach (var item in eventlist)
            {
                item.image = "/image/host/" + item.image;
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

            return Ok(new ReturnData() { message = "加入成功", status = ReturnStatus.Success });
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

        public IHttpActionResult GetCartList()
        {
            // 區域變數
            string cartItem = GlobalVariables.CartItems;
            List<CartItem> list = new List<CartItem>();
            list = (List<CartItem>)HttpContext.Current.Session[cartItem];

            if (list == null) list = new List<CartItem>();

            return Ok(list);
        }

        // 建立訂單
        public IHttpActionResult PostCreateOrder2()
        {
            List<CartItem> eventList = (List<CartItem>)HttpContext.Current.Session[GlobalVariables.CartItems];
            if (eventList == null || eventList.Count <= 0)
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "購物車為空" });

            List<int> eventIdList = new List<int>();
            foreach (var item in eventList) { eventIdList.Add(item.EVID); };
            orders order = new CheckOutRepo(eventIdList, 2).getOrder();

            HttpContext.Current.Session[GlobalVariables.CartItems] = null; // 清空 session 購物清單

            return Ok(new { status = ReturnStatus.Success, order = order, data = new { url = "/shop/CheckoutSuccess" } });
        }

        public async Task<IHttpActionResult> PostCreateOrder()
        {
            int userid = (int)HttpContext.Current.Session[GlobalVariables.AccountID];


            List<CartItem> cartList = (List<CartItem>)HttpContext.Current.Session[GlobalVariables.CartItems];
            if (cartList == null || cartList.Count <= 0)
                return Ok(new ReturnData() { status = ReturnStatus.Error, message = "購物車為空" });

            // 將購物車的展覽 id 轉成 id 陣列
            List<int> eventIdList = new List<int>();
            foreach (var item in cartList) { eventIdList.Add(item.EVID); };

            // 進入結帳資料庫
            orders order = new CheckOutRepo(eventIdList, userid).getOrder();

            HttpContext.Current.Session[GlobalVariables.CartItems] = null; // 清空 session 購物清單

            // 將 event.EVID 陣列轉成 event 陣列
            List<events> ticketList = db.events.Where(i => eventIdList.Contains(i.EVID)).ToList();

            PayPalHttp.HttpResponse s = await Repo.BuildPayPalOrder.CreateOrder(ticketList, order.totalPrice);
            Order orderResult = s.Result<Order>();

            order = db.orders.Find(order.id);
            order.paypal_Id = orderResult.Id;
            db.SaveChanges();

            string url = orderResult.Links.Where(i => i.Rel == "approve").First().Href;

            return Ok(new Models.ReturnData() { status = ReturnStatus.Success, message = "成功", data = new { url = url } });
        }


        // 展覽資訊
        public IHttpActionResult GetEventDetail(int? id = 1)
        {
            var mEventDetail = (from e in db.events
                                join h in db.hosts on e.HID equals h.HID
                                where e.EVID == id
                                select new Models.EventDetail
                                {
                                    EVID = e.EVID,
                                    organizer = e.name,
                                    hostName = h.name,
                                    start = e.startdate.ToString(),
                                    end = e.enddate.ToString(),
                                    location = e.venue,
                                    image = "/image/host/" + e.image,
                                    price = e.ticketprice.ToString(),
                                    eventinfo = e.eventinfo
                                }).FirstOrDefault();

            if (mEventDetail == null) return Ok(new Models.ReturnData() { status = ReturnStatus.Error, message = "查無資料" });

            var idList = db.exhibitinfo.Where(e => e.EVID == id).Where(e => e.verify == true).Select(e => e.EID).ToList();

            mEventDetail.exhibitorList = (from eInfo in db.exhibitinfo
                                          join eData in db.exhibitors on eInfo.EID equals eData.EID
                                          where eInfo.EVID == id where eInfo.verify == true
                                          select new Models.ExhibitorsInfo
                                          {
                                              EID = eInfo.EID,
                                              name = eData.name,
                                              image = eInfo.image
                                          }).ToList();

            mEventDetail.tagList = (from tg in db.eventTags join tn in db.TagsName on tg.tagID equals tn.id where tg.EVID == id select tn.tagName ).ToList();

            foreach (var item in mEventDetail.exhibitorList)
            {
                item.image = "/image/exhibitor/" + item.image;
            }

            return Ok(mEventDetail);
        }


        public IHttpActionResult Get()
        {
            return Ok("hello");
        }

        public class tName
        {
            public string tagName { get; set; }
        }

        //tag選擇後跳出所有活動詳細資料
        public IHttpActionResult PostTest([FromBody] tName tg)
        {
            string tagName = tg.tagName;
            var tag = (from q in db.eventTags
                       join p in db.TagsName
                       on q.tagID equals p.id
                       join k in db.events
                       on q.EVID equals k.EVID
                       where p.tagName == tagName
                       select new
                       {
                           EVID = k.EVID,
                           name = k.name,
                           startdate = k.startdate,
                           enddate = k.enddate,
                           eventinfo = k.eventinfo,
                           image = "/image/host/" + k.image

                       }).ToList();

            return Ok(tag);

            //return Ok();
        }

        public IHttpActionResult Gettop5Tag()
        {
            //var data = from p in db.TagsName
            //           join q in db.eventTags
            //           on p.id equals q.tagID
            //           orderby q.tagID.

            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select TOP(5) count(q.tagName) ,q.tagName from eventTags as p join TagsName as q on p.tagID = q.id group by q.tagName";


            List<string> eList = new List<string>();

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

                        eList.Add((string)reader[1]);

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Ok(eList);
        }




        public IHttpActionResult GetSearchTicketList(string searchStr, int? page)
        {
            int num = 1;
            if (page != null && page >= 1)
            {
                num = ((int)page - 1) * 12;
            }
            var list = (from evn in db.events where evn.name.Contains(searchStr) select evn).OrderBy(e => e.createAt).Skip(num).Take(12);
            foreach (var item in list)
            {
                item.image = "/image/host/" + item.image;
            }
            return Ok(list);
        }

        public IHttpActionResult GetDefaultTag()
        {
            var tag = db.TagsName.OrderBy(fer => fer.id).Take(10).ToList();
            return Ok(tag);
        }

        public IHttpActionResult PostSelectSearch([FromBody] Models.SearchSelect data, int page = 1)
        {

            if (data == null || (data.StartDate == null && data.EndDate == null && data.CheckTag == null))
            {
                return Ok(new List<string>());
            }

            IQueryable<events> query = (from eh in db.events select eh);

            // var eventList = (from tg in db.TagsName join eTag in db.eventTags on tg.id equals eTag.tagID select eTag.EVID).ToList().Distinct();

            if (page - 1 >= 0)
            {
                page = (page - 1) * 12;
            }

            if (data.CheckTag != null && data.CheckTag.Length > 0)
            {
                var tgList = (from tg in db.TagsName where data.CheckTag.Contains(tg.tagName) select tg.id).ToList().Distinct();
                var eList2 = (from eventTg in db.eventTags where tgList.Contains(eventTg.tagID) select eventTg.EVID).ToList().Distinct();
                query = (from oEvent in db.events
                         where eList2.Contains(oEvent.EVID)
                         select oEvent);
            }

            if (data.StartDate == null && data.EndDate == null)
            {
                var searchDate = query.OrderByDescending(e => e.startdate).Skip(page).Take(12).ToList();
                for (int i = 0; i < searchDate.Count; i++)
                {
                    searchDate[i].image = "/image/Host/" + searchDate[i].image;
                }
                return Ok(searchDate);
            }

            if (data.StartDate != null)
            {
                query = from q in query where q.startdate >= data.StartDate select q;
            }

            if (data.EndDate != null)
            {
                query = from q in query where q.enddate <= data.EndDate select q;
            }

            var b = query.OrderByDescending(e => e.startdate).Skip(page).Take(12).ToList();

            for (int i = 0; i < b.Count; i++)
            {
                b[i].image = "/image/Host/" + b[i].image;
            }
            return Ok(b);
        }

        public IHttpActionResult GetConsumingRecord()
        {
            int userId = (int)(HttpContext.Current.Session[Models.GlobalVariables.AccountId] == null ? 2 : HttpContext.Current.Session[Models.GlobalVariables.AccountId]);
            var data = (from tk in db.Tickets
                        join ex in db.events on tk.EVID equals ex.EVID
                        where tk.UID == userId
                        select new TicketPreview
                        {
                            name = ex.name,
                            startdate = ex.startdate.ToString(),
                            enddate = ex.enddate.ToString(),
                            purchaseDateTime = tk.createAt.ToString(),
                            ticketPrice = (int)ex.ticketprice
                        }).ToList();

            foreach (var item in data)
            {
                if (DateTime.Now < DateTime.Parse(item.startdate)) // 未來
                {
                    item.status = "籌備中";
                }
                else if (DateTime.Now >= DateTime.Parse(item.startdate) || DateTime.Now <= DateTime.Parse(item.enddate)) // 現在
                {
                    item.status = "舉辦中";
                }
                else if (DateTime.Now > DateTime.Parse(item.enddate))  //過去
                {
                    item.status = "已逾期";
                }

                try
                {
                    item.purchaseDateTime = DateTime.Parse(item.purchaseDateTime).ToString();
                }
                catch (Exception) { item.purchaseDateTime = ""; }

            }



            return Ok(data);
        }

    }
}
