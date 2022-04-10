using System;
using PayPalCheckoutSdk.Core;
using PayPalHttp;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using PayPalCheckoutSdk.Orders;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Payments;

namespace EXhibition.Repo
{
    public class PayPalClient
    {

        static public string Currency = "TWD";

        static public ApplicationContext appContext = new ApplicationContext
        {
            BrandName = "展覽館-E展鴻圖- Exhibition Inc.",
            LandingPage = "BILLING",
            CancelUrl = Models.GlobalVariables.OnlinePayPalUrl,
            ReturnUrl = Models.GlobalVariables.OnlinePayPalUrl + "shop/CheckoutSuccess/",
            UserAction = "CONTINUE",
            ShippingPreference = "NO_SHIPPING"
        };

        public static PayPalEnvironment environment()
        {
            return new SandboxEnvironment(
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID"),
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET")
            );
        }


        public static HttpClient client()
        {
            return new PayPalHttpClient(environment());
        }

        public static HttpClient client(string refreshToken)
        {
            return new PayPalHttpClient(environment(), refreshToken);
        }

        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(
                        memoryStream, Encoding.UTF8, true, true, "  ");
            DataContractJsonSerializer ser = new DataContractJsonSerializer(serializableObject.GetType(), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            ser.WriteObject(writer, serializableObject);
            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }


        public async static Task<HttpResponse> AuthorizeOrder(string OrderId, bool debug = false)
        {
            var request = new OrdersAuthorizeRequest(OrderId);
            request.Prefer("return=representation");
            request.RequestBody(new AuthorizeRequest());
            var response = await PayPalClient.client().Execute(request);
            return response;
        }

        public async static Task<HttpResponse> CaptureOrder(string AuthorizationId, bool debug = false)
        {
            var request = new AuthorizationsCaptureRequest(AuthorizationId);
            request.Prefer("return=representation");
            request.RequestBody(new CaptureRequest());
            var response = await PayPalClient.client().Execute(request);
            return response;
        }

    }
}