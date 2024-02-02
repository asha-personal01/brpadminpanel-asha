using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
namespace D1WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://13.126.26.188,http://localhost:4200,http://localhost:4300," +
                "http://localhost:4201,http://localhost,http://portal.distone.com,https://portal.distone.com," +
                "http://portal2.distone.com,https://portal2.distone.com," +
                "https://paymentportal.distone.com,http://paymentportal.distone.com", "*", "*");

            config.EnableCors(cors);
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiByName",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
