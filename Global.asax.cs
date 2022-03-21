using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EXhibition
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            if (HttpContext.Current.IsDebuggingEnabled)
            {
                System.Diagnostics.Debug.WriteLine("is debug mode");
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
