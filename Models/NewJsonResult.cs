using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Models
{
    public class NewJsonResult: JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public NewJsonResult()
        {
            Settings = new JsonSerializerSettings {

                //1. 忽略循環引用
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                //2. 日期格式化
                DateFormatString = "yyyy/MM/dd HH:mm:ss",

                //3. 設置屬性開頭為小寫
                ContractResolver  = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()

            };

        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentNullException("GET is not Allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if(this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if(this.Data == null)
            {
                return;
            }

            var scriptSerializer = JsonSerializer.Create(this.Settings);
            scriptSerializer.Serialize(response.Output, this.Data);
        }

    }
}