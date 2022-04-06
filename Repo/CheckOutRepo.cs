using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EXhibition.Repo
{
    public class CheckOutRepo
    {

        DBConnector db = new DBConnector();

        private Models.orders order = null;

        public CheckOutRepo(List<int> eventIdList, int userId)
        {
            // 先建立訂單，之後用迴圈計算價錢
            this.order = db.orders.Add(new orders() { createDateTime = DateTime.Now, userId = userId });
            db.SaveChanges();

            // 將 event.EVID 陣列轉乘 event 陣列
            List<events> ticketList = db.events.Where(i => eventIdList.Contains(i.EVID)).ToList();

            // 金額統計
            int totalPrice = 0;

            foreach (var ticket in ticketList)
            {
                // 建立票券
                var t = db.Tickets.Add(new Tickets() { UID = userId, createAt = DateTime.Now, EVID = ticket.EVID, paid = true});
                db.SaveChanges();

                // 建立訂單資料
                db.orderDetail.Add(new orderDetail() { orderId = this.order.id, ticketId = t.TID, price = (int?)ticket.ticketprice });
                db.SaveChanges();

                totalPrice = (int)(totalPrice + ticket.ticketprice);
            }

            // 金額總結
            this.order = db.orders.Find(this.order.id);
            this.order.finalPrice = totalPrice;
            this.order.totalPrice = totalPrice;
            this.order.isPay = false;
            db.SaveChanges();

        }

        public orders getOrder()
        {
            return this.order;
        }

    }
}