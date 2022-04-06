using System;
using PayPalCheckoutSdk.Core;
using PayPalHttp;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using PayPalCheckoutSdk.Orders;

namespace EXhibition.Repo
{
    public class PayPalClient
    {

        static public string Currency = "TWD";

        static public ApplicationContext appContext = new ApplicationContext
        {
            BrandName = "展覽館-Exhibition Inc.",
            LandingPage = "BILLING",
            CancelUrl = Models.GlobalVariables.ServerHost,
            ReturnUrl = Models.GlobalVariables.ServerHost + "/shop/CheckoutSuccess",
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

    }
}