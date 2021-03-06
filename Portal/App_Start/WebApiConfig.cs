using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Portal
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
            config.Formatters.Clear();
            //config.Formatters.Add(new XmlMediaTypeFormatter());
            config.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}
