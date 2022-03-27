using System;

namespace EXhibition.Models
{

    static public class ReturnStatus
    {
        public static string Success = "success";
        public static string Error = "error";
    }

    public class ReturnData
    {
        public string message { get; set; }
        public string status { get; set; }
        public Object data { get; set; }

        public ReturnData() { }

        public ReturnData(string message, string status, object data)
        {
            this.message = message;
            this.status = status;
            this.data = data;
        }
    }
}