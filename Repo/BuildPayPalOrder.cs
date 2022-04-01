using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXhibition.Repo
{
    public class BuildPayPalOrder
    {



        //Below function can be used to build the create order request body with minimum payload.
        private static OrderRequest BuildRequestBodyWithMinimumFields(int totalPirce = 200)
        {
            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "AUTHORIZE",
                ApplicationContext = new ApplicationContext
                {
                    CancelUrl = "https://www.example.com",
                    ReturnUrl = "https://localhost:44378/User"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "TWD",
                            Value = totalPirce.ToString()
                        }
                    }
                }
            };

            return orderRequest;
        }

        //Below function can be used to create an order with minimum payload.
        public static async Task<HttpResponse> CreateOrderWithMinimumFieldsAsync(int totalPirce = 200)
        {
            // Console.WriteLine("Create Order with minimum payload..");
            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            request.RequestBody(BuildRequestBodyWithMinimumFields());
            var response = await PayPalClient.client().Execute(request);
            Order order = response.Result<Order>();
            var db = new Models.DBConnector();
            db.orders.Add(new Models.orders() { createDateTime = DateTime.Now, paypalId = order.Id, totalPrice = totalPirce, finalPrice = totalPirce });
            db.SaveChanges();
            return response;
        }


    }
}