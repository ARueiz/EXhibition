using EXhibition.Filters;
using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    [AuthorizeFilter(UserRole.Host)]
    public class HostApiController : Controller
    {

        DBConnector db = new DBConnector();


        //顯示主辦單位
        public ActionResult show_Host(int? id)
        {

            Models.ReturnData rd = new Models.ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            int index = (int)id;

            var host = db.hosts.Find(index);

            if (host == null || host.HID == 0)
            {
                rd.message = "找不到資料";
                rd.status = "error";
                rd.data = host;
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return Json(host, JsonRequestBehavior.AllowGet);
        }


        //編輯主辦單位
        [HttpPost]
        public ActionResult Edit_Host(Models.hosts host)
        {
            Models.ReturnData rd = new Models.ReturnData();

            if (host.HID == 0)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";
                rd.data = host;
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            var data = db.hosts.Find(host.HID);

            if (data == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";
                rd.data = host;
                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            else
            {

                data.link = host.link;
                data.name = host.name;
                data.phone = host.phone;
                data.email = host.email;


                db.SaveChanges();
            }

            rd.message = "成功";
            rd.status = "success";
            return Json(rd, JsonRequestBehavior.AllowGet);
        }

        //顯示展覽列表
        public ActionResult show_host_list(int? num = 1)
        {
            int x;
            List<Models.events> info = new List<Models.events>();
            if (num < 0 || num == 1)
            {
                num = 1;
                x = 0;
                info = db.events.OrderBy(y => y.EVID).Skip(x).Take(5).ToList();
            }
            else
            {
                x = (int)num * 5;
                info = db.events.OrderBy(y => y.EVID).Skip(x).Take(5).ToList();
            }
            foreach (var i in info)
            {
                i.image = @"/image/host/" + i.image;
            }
            return Json(info, JsonRequestBehavior.AllowGet);
        }


        //顯示唯一展覽詳細
        public ActionResult showEventDetail(int? index)
        {
            Models.ReturnData rd = new ReturnData();
            if (index == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            int i = (int)index;

            var data = db.events.Find(i);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        //顯示廠商詳細

        public ActionResult showexhibitor(int? index)
        {
            Models.ReturnData rd = new Models.ReturnData();

            if (index == null)
            {
                rd.message = "Id 錯誤";
                rd.status = "error";

                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            int i = (int)index;
            Models.exhibitors data = db.exhibitors.Find(i);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //顯示verefied 是 false 的參展商
        public ActionResult show_verified_false_exhibitor()
        {
            var data = db.exhibitors.Where(a => a.verify == false);


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //顯示所有廠商活動
        public ActionResult exhibitInfo_AllowOrRefuse(int? EVID)
        {
            var rd = new ReturnData();
            DateTime today = DateTime.Now;
            int eidd = (int)EVID;
            var alldata = (from q in db.exhibitinfo
                           join p in db.exhibitors
                           on q.EID equals p.EID
                           join k in db.events
                           on q.EVID equals k.EVID
                           where k.startdate >= today && q.verify == null && k.EVID == eidd
                           select new AuditExhibitorInfo
                           {
                               name = p.name,
                               id = q.id,
                               createAt = q.createAt.ToString(),
                               verify = q.verify
                           }
                           ).ToList();

            foreach (var item in alldata)
            {
                if(item.createAt != "")
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            if (alldata == null)
            {
                rd.message = "no data";
                rd.status = "error";
            }

            return Json(alldata, JsonRequestBehavior.AllowGet);
        }


        //允許或拒絕廠商
        public ActionResult AllowOrRefuse(int? index, bool verified, string reason, string status)
        {
            var rd = new ReturnData();
            if (index == null)
            {
                rd.message = "no data";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            int x = (int)index;
            var allow = db.exhibitinfo.Find(x);
            allow.verify = verified;
            allow.reason = reason;
            allow.status = status;

            try
            {
                db.SaveChanges();
                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Success;
                data.message = "新增成功!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Error;
                data.message = "新增失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        //允許全部
        public ActionResult allow_all(int? EVID)
        {
            var rd = new ReturnData();
            DateTime today = DateTime.Now;
            int eidd = (int)EVID;
            var alldata = (from q in db.exhibitinfo
                           join p in db.exhibitors
                           on q.EID equals p.EID
                           join k in db.events
                           on q.EVID equals k.EVID
                           where k.startdate < today && q.verify == false && k.EVID == EVID
                           select new AuditExhibitorInfo
                           {
                               name = p.name,
                               id = q.id,
                               createAt = q.createAt.ToString(),
                               verify = q.verify

                           }
                           ).ToList();
            if (alldata == null)
            {
                rd.message = "no data";
                rd.status = "error";
            }

            return Json(alldata, JsonRequestBehavior.AllowGet);

        }

        public ActionResult selectorAllStatusList(int? EVID)
        {
            var rd = new ReturnData();
            DateTime today = DateTime.Now;
            int eidd = (int)EVID;
            var alldata = (from q in db.exhibitinfo
                           join p in db.exhibitors
                           on q.EID equals p.EID
                           join k in db.events
                           on q.EVID equals k.EVID
                           where k.startdate >= today && k.EVID == EVID
                           select new AuditExhibitorInfo
                           {
                               name = p.name,
                               id = q.id,
                               createAt = q.createAt.ToString(),
                               verify = q.verify

                           }
                           ).ToList();

            foreach (var item in alldata)
            {
                if(item.createAt != "")
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd HH:mm:ss");
                }
                
            }

            if (alldata == null)
            {
                rd.message = "no data";
                rd.status = "error";
            }

            return Json(alldata, JsonRequestBehavior.AllowGet);
        }




        public ActionResult selectorAllow(int? EVID)
        {
            var rd = new ReturnData();
            DateTime today = DateTime.Now;
            if (EVID == null)
            {

            }

            int eidd = (int)EVID;
            var alldata = (from q in db.exhibitinfo
                           join p in db.exhibitors
                           on q.EID equals p.EID
                           join k in db.events
                           on q.EVID equals k.EVID
                           where k.startdate >= today && q.verify == true && k.EVID == EVID
                           select new AuditExhibitorInfo
                           {
                               name = p.name,
                               id = q.id,
                               createAt = q.createAt.ToString(),
                               verify = q.verify

                           }
                           ).ToList();

            foreach (var item in alldata)
            {
                if(item.createAt != "")
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            if (alldata == null)
            {
                rd.message = "no data";
                rd.status = "error";
            }

            return Json(alldata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectorReject(int? EVID)
        {
            var rd = new ReturnData();
            DateTime today = DateTime.Now;
            int eidd = (int)EVID;
            var alldata = (from q in db.exhibitinfo
                           join p in db.exhibitors
                           on q.EID equals p.EID
                           join k in db.events
                           on q.EVID equals k.EVID
                           where k.startdate >= today && q.verify == false && k.EVID == EVID
                           select new AuditExhibitorInfo
                           {
                               name = p.name,
                               id = q.id,
                               createAt = q.createAt.ToString(),
                               verify = q.verify

                           }
                           ).ToList();

            foreach (var item in alldata)
            {
                if(item.createAt != "")
                {
                    item.createAt = DateTime.Parse(item.createAt).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            if (alldata == null)
            {
                rd.message = "no data";
                rd.status = "error";
            }

            //var c = db.exhibitinfo.Where(i=>i.EVID == evid).Where(i => i.verify == null).Count();

            return Json(alldata, JsonRequestBehavior.AllowGet);
        }


        //tag顯示器只顯示前十筆
        public ActionResult tagselector()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select top(10) count(A.tagId) , A.TagId , B.tagName from eventTags as A inner join TagsName as B on A.tagID = B.id group by A.tagId ,B.tagName order by 1 desc";

            // 先將 id 撈成 陣列後 用 entity framework 去找資料

            List<TagsName> eventlist = new List<TagsName>();
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

            var list = db.TagsName.Where(item => eventIdList.Contains(item.id)).Select(item => item.tagName).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult howManyLeft()
        {
            var amount = db.exhibitinfo.Where(i => i.verify == null);
            return Json(amount, JsonRequestBehavior.AllowGet);
        }


        public ActionResult List(int? id)
        {

            int va;

            if (id == null || (int)id <= 0)
            {
                va = 1;
            }
            else
            {
                va = (int)id;
            }

            va = (int)(va - 1) * 10;

            int HID = (int)Session["AccountID"];

            var data = (from hostsTable in db.hosts
                        join eventsTable in db.events on hostsTable.HID equals eventsTable.HID
                        where eventsTable.HID == HID
                        orderby eventsTable.startdate descending 
                        select new Models.HostEventInfo

                        {
                            name = hostsTable.name,
                            phone = hostsTable.phone,
                            startdate = eventsTable.startdate.ToString(),
                            enddate = eventsTable.enddate.ToString(),
                            exhibitionname = eventsTable.name,
                            evid = eventsTable.EVID,
                            ticketPrice = eventsTable.ticketprice,
                            hid = hostsTable.HID
                        }).Skip(va).Take(10).ToList();

            for (int i = 0; i < data.Count(); i++)
            {
                int evid = data[i].evid;
                var count = (db.exhibitinfo.Where(e => e.EVID == evid).Where(e => e.verify == null)).ToArray().Count();
                data[i].waitingCount = count;
            }



            foreach (var item in data)
            {
                if (DateTime.Now >= DateTime.Parse(item.startdate))
                {
                    item.isOver = false;
                }
                else if (DateTime.Now < DateTime.Parse(item.startdate))
                {
                    item.isOver = true;
                }

            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostList(int? id)
        {

            Models.ReturnData rd = new Models.ReturnData();

            if (id == null)
            {
                rd.message = "找不到資料 Id 為 null";
                rd.status = "error";
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            int index = (int)id;

            var a = db.events.Find(index);

            if (a == null || a.EVID == 0)
            {
                rd.message = "找不到資料";
                rd.status = "error";
                rd.data = a;
                return Json(rd, JsonRequestBehavior.AllowGet);
            }

            return new Models.NewJsonResult() { Data = a };
        }

        //新增展覽
        public ActionResult DoCreateEvent(HttpPostedFileBase image, HttpPostedFileBase floorplanimg, Models.events events, List<string> tagList)
        {
            string strPath = "";

            if (image != null)
            {
                //儲存 封面圖 to Image/Host
                strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.image;
                image.SaveAs(strPath);
            }

            if (floorplanimg != null)
            {
                //儲存 平面圖 to Image/Host
                strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.floorplanimg;
                floorplanimg.SaveAs(strPath);
            }

            //儲存資料到DB
            events.HID = (int)Session["AccountID"];
            events.createAt = DateTime.Now;
            db.events.Add(events);
            int result = db.SaveChanges();

            // 加入 tag
            Repo.TagRepo insert = new Repo.TagRepo();
            insert.TagsInsert(tagList, events.EVID);

            try
            {
                db.SaveChanges();
                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Success;
                data.message = "新增成功!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                ReturnData data = new ReturnData();
                data.status = ReturnStatus.Error;
                data.message = "新增失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Login(Models.Login login)
        {
            ReturnData r = new ReturnData();

            if (string.IsNullOrEmpty(login.account) || string.IsNullOrEmpty(login.password))
            {
                r.message = "登入失敗";
                r.status = ReturnStatus.Error;
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    string hashPwd = SHA_256.ComputeSha256Hash(login.password);

                    var loginData = (from host in db.hosts
                                     where host.email == login.account && host.password == hashPwd && host.verify == true
                                     select new
                                     {
                                         HID = host.HID,
                                     }).ToList();
                    if (loginData.Any())
                    {
                        r.message = "登入成功";
                        r.status = ReturnStatus.Success;
                        Session["UserRole"] = "Host";
                        Session["AccountID"] = loginData[0].HID;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        r.message = "帳號或密碼錯誤，查無此人";
                        r.status = ReturnStatus.Error;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    r.message = "帳號或密碼錯誤";
                    r.status = ReturnStatus.Error;
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Register(Models.hosts host)
        {
            Models.ReturnData r = new ReturnData();

            host.password = SHA_256.ComputeSha256Hash(host.password);
            host.image = "EditHost.png";
            host.verify = true;

            db.hosts.Add(host);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                r.status = Models.ReturnStatus.Error;
                r.message = "註冊失敗";
                return Json(r, JsonRequestBehavior.AllowGet);
            }

            r.status = Models.ReturnStatus.Success;
            r.message = "註冊成功";
            r.data = new { url = "/Home/HostLogin" };
            return Json(r);
        }


        public ActionResult editExhibitorJoinStatus(Models.exhibitinfo e, string reason, bool isAllow)
        {
            if (isAllow) // 允許加入
            {

            }
            else  // 拒絕加入，加上拒絕原因
            {

            }
            return Json(new { id = e.EID, resaon = reason, isAllow = isAllow }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEventList(int? id)
        {
            int HID = Convert.ToInt32(Session["AccountID"]);

            if (id == null) { id = 0; }
            int num = (int)id;
            var list = (from eve in db.events
                        where eve.HID == HID
                        orderby eve.startdate descending
                        select eve)
                        .Skip(num).Take(12).ToList();

            for (int i = 0; i < list.Count; i++)
            {

                list[i].image = "/image/Host/" + list[i].image;
            }

            return new NewJsonResult() { Data = list };
        }

        public ActionResult GetHostInfo(int? id)
        {
            int HID = Convert.ToInt32(Session["AccountID"]);

            var list = (from host in db.hosts
                        where host.HID == HID && host.verify == true
                        select new
                        {
                            HID = host.HID,
                            name = host.name,
                            phone = host.phone,
                            email = host.email,
                            link = host.link,
                            image = host.image,
                            verify = host.verify
                        }).ToList();

            return new NewJsonResult() { Data = list[0] };
        }

        public ActionResult DoUpdateHostInfo(HttpPostedFileBase imagefile, hosts host)
        {
            string strPath = "";

            hosts updateHost = db.hosts.FirstOrDefault(h => h.HID == host.HID);
            ReturnData data = new ReturnData();

            if (updateHost != null)
            {
                if (imagefile != null)
                {
                    //儲存 封面圖 to Image/Host
                    strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + host.image;
                    updateHost.image = host.image;
                    imagefile.SaveAs(strPath);
                }

                updateHost.name = host.name;
                updateHost.phone = host.phone;
                updateHost.email = host.email;
                updateHost.link = host.link;

                try
                {
                    db.SaveChanges();
                    data.status = ReturnStatus.Success;
                    data.message = "更新成功!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    data.status = ReturnStatus.Error;
                    data.message = "更新失敗!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.status = ReturnStatus.Error;
                data.message = "更新失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult test(string name)
        {
            var x = "pokemon";
            var tag = (from q in db.eventTags
                       join p in db.TagsName
                       on q.tagID equals p.id
                       join k in db.events
                       on q.EVID equals k.EVID
                       where p.tagName == name
                       select new
                       {
                           name = k.name,
                           start = k.startdate,
                           end = k.enddate,
                           info = k.eventinfo,
                           image = k.image

                       }).ToList();
            return Json(tag, JsonRequestBehavior.AllowGet);

            //return Ok();
        }


        //更新展覽
        public ActionResult DoUpdateEvent(HttpPostedFileBase image, HttpPostedFileBase floorplanimg, Models.events events, List<string> tagList)
        {

            string strPath = "";
            events updateEvent = db.events.FirstOrDefault(e => e.EVID == events.EVID);
            ReturnData data = new ReturnData();

            if (updateEvent != null)
            {
                if (image != null)
                {
                    //儲存 封面圖 to Image/Host
                    strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.image;
                    updateEvent.image = events.image;
                    image.SaveAs(strPath);
                }

                if (floorplanimg != null)
                {
                    //儲存 平面圖 to Image/Host
                    strPath = Request.PhysicalApplicationPath + "Image\\Host\\" + events.floorplanimg;
                    updateEvent.floorplanimg = events.floorplanimg;
                    floorplanimg.SaveAs(strPath);
                }

                updateEvent.name = events.name;
                updateEvent.startdate = events.startdate;
                updateEvent.enddate = events.enddate;
                updateEvent.venue = events.venue;
                updateEvent.ticketprice = events.ticketprice;
                updateEvent.eventinfo = HttpUtility.UrlDecode(events.eventinfo);

                //刪除所有Tag
                db.eventTags.RemoveRange(db.eventTags.Where(e => e.EVID == events.EVID));

                // 加入 tag
                Repo.TagRepo insert = new Repo.TagRepo();
                insert.TagsInsert(tagList, events.EVID);

                try
                {
                    db.SaveChanges();
                    data.status = ReturnStatus.Success;
                    data.message = "更新成功!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    data.status = ReturnStatus.Error;
                    data.message = "更新失敗!";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.status = ReturnStatus.Error;
                data.message = "更新失敗!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult getExhibitInfo(int? id)
        {
            int evinfoid = (int)id;
            
            var evinfo = db.exhibitinfo.Where(e=>e.id == evinfoid).FirstOrDefault();

            return new NewJsonResult(){ Data = evinfo};
        }
    }
}