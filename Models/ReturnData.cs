using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EXhibition.Models
{

    static public class RetrunStatus
    {
        public static string Success = "success";
        public static string Error = "error";
    }

    public class ReturnData
    {
        public string message { get; set; }
        public string status { get; set; }
        public Object data { get; set; }
    }
}