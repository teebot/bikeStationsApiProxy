using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BikeProxy.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var corsOriginsAllowed = ConfigurationManager.AppSettings["CorsOriginsAllowed"];
            var cors = new EnableCorsAttribute(corsOriginsAllowed, "*", "*");
            config.EnableCors(cors);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
