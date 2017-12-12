using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FirstREST
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Financial", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Filter",
                url: "Sales/{action}/{period1}/{period2}",
                defaults: new { controller = "Sales", action = "Filter", period1 = 1, period2 = 12 }
            );

            routes.MapRoute(
               name: "Financial",
               url: "Financial/{action}/{period1}/{period2}",
               defaults: new { controller = "Financial", action = "Filter", period1 = 1, period2 = 12 }
           );
        }
    }
}