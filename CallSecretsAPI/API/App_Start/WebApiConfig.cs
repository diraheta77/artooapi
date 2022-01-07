using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using API.Interface;
using API.Repository;
using API.Security;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Configuración y servicios de API web
            ////config.Filters.Add(new API.Security.AuthorizationFilter());

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
