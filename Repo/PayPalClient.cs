using System;
using PayPalCheckoutSdk.Core;
using PayPalHttp;

using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;


namespace EXhibition.Repo
{
    public class PayPalClient
    {

        static public string Currency = "TWD";


        //public static PayPalEnvironment environment()
        //{
        //    return new SandboxEnvironment(
        //         System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID"),
        //         System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET")
        //    );
        //}

        public static PayPalEnvironment environment()
        {
            return new SandboxEnvironment(
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") != null ?
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") : "AR0o52R3Xd6eSerXouyA-EeTqVkMH43fmU-nrxRZw-a_25YWVIVVBivwTzabDLzbuaOe01JF7KMgaCHR",
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") != null ?
                 System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") : "EOoy_vAyvhDa_lt5leDCxrSWGB95E_SW4mYpaK0AarglWdjkHHTgOGxxH00zDFd-JYdYh3t5LGYYapgv");
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