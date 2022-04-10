using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXhibition.Repo
{
    public class BuildPayPalOrder
    {

        //Below function can be used to build the create order request body with complete payload.
        private static OrderRequest BuildRequestBody(List<Models.events> ticketList, int totalPrice)
        {

            var itemList = new List<Item>();
            string currency = PayPalClient.Currency;            

            // 製作購買列表
            foreach (var item in ticketList)
            {                
                var i = new Item();
                i.Name = item.name;
                i.UnitAmount = new Money { CurrencyCode = currency, Value = Decimal.ToInt32(item.ticketprice).ToString()};
                i.Quantity = "1";                                
                itemList.Add(i);
            }

            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "AUTHORIZE",
                ApplicationContext = PayPalClient.appContext,
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{
                        ReferenceId =  "PUHF",
                        Description = "購買票券",
                        //CustomId = "1000",
                        SoftDescriptor = "HighFashions",
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = PayPalClient.Currency,
                            Value = totalPrice.ToString(),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = currency,
                                    Value = totalPrice.ToString(),
                                }
                            }
                        },
                        Items = itemList
                    }
                }
            };
            return orderRequest;
        }

        //Below function can be used to create an order with complete payload.
        public async static Task<HttpResponse> CreateOrder(List<Models.events> ticketList, int totalPrice)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(BuildRequestBody(ticketList, totalPrice));
            var response = await PayPalClient.client().Execute(request);
            return response;
        }

    }
}