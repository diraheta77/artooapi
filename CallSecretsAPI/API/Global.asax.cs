using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using API.Interface;
using API.Repository;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ///BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Aprovecho el final de la ejecucion de la llamada al api y asi 
        /// remuevo el header con la informacion del Server.
        /// </summary>
        protected void Application_EndRequest()
        {
            Response.Headers.Remove("Server");
        }

    }
}
