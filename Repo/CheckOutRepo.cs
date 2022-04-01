using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Repo
{
    public class CheckOutRepo
    {

        DBConnector db = new DBConnector();

        public CheckOutRepo() { }

        // 建立訂單
        public orders CreateOrder(List<int> eventIdList, int userId)
        {

            // 先建立訂單，之後用迴圈計算價錢
            var order = db.orders.Add(new orders() { createDateTime = DateTime.Now, userId = userId });
            db.SaveChanges();

            // 將 event.EVID 陣列轉乘 event 陣列
            List<events> ticketList = db.events.Where(i => eventIdList.Contains(i.EVID)).ToList();

            // 金額統計
            int totalPrice = 0;

            foreach (var ticket in ticketList)
            {
                // 建立票券
                var t = db.Tickets.Add(new Tickets() { UID = userId, createAt = DateTime.Now, EVID = ticket.EVID });
                db.SaveChanges();

                // 建立訂單資料
                db.orderDetail.Add(new orderDetail() { orderId = order.id, ticketId = t.TID, price = (int?)ticket.ticketprice });
                db.SaveChanges();

                totalPrice = (int)(totalPrice + ticket.ticketprice);
            }

            // 金額總結
            order = db.orders.Find(order.id);
            order.finalPrice = totalPrice;
            order.totalPrice = totalPrice;            
            db.SaveChanges();

            return order;
        }

    }
}