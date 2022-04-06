using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{
    public class GlobalVariables
    {
        static public string QRcodeToken = "QRcodeToken";
        static public string AccountId = "AccountId";
        static public string AccountID = "AccountID";        
        static public string AccountType = "AccountType";
        static public string User = "User";
        static public string Host = "Host";
        static public string Exhibitor = "Exhibitor";
        static public string CartItems = "CartItems";
        static public string HostImageUrl = @"/image/host/";
        static readonly public string LocalPayPalUrl = "https://localhost:44378/";
        static readonly public string OnlinePayPalUrl = "https://exhibition.azurewebsites.net/";
        static public string PayPalCancelUrl = "";
        static public string PayPalReturnUrl = "";
        static public string ServerHost = "https://localhost:44378/";
    }
}