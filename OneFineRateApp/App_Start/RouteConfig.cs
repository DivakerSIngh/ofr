
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OneFineRateApp
{

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.MapMvcAttributeRoutes();
            //   routes.MapRoute(
            //name: "PropertyDining",
            //url: "{controller}/{action}/{id}",
            //defaults: new { controller = "PropertyDining", action = "Index", id = UrlParameter.Optional }
            //);
           

            routes.MapRoute(
           name: "FitnessRecreation",
           url: "FitnessRecreation/{action}/{id}",
           defaults: new { controller = "FitnessRecreation", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
            name: "Rooms",
            url: "Rooms/{action}/{id}",
            defaults: new { controller = "Rooms", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Spa",
            url: "Spa/{action}/{id}",
            defaults: new { controller = "Spa", action = "Index" }
            );

            routes.MapRoute(
            name: "Property",
            url: "Property/{action}/{id}",
            defaults: new { controller = "Property", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "BulkUpdate",
            url: "BulkUpdate/{action}/{id}",
            defaults: new { controller = "BulkUpdate", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "BulkBid",
            url: "BulkBid/{action}/{id}",
            defaults: new { controller = "BulkBid", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
          name: "ServiceCharge",
          url: "ServiceCharge/{action}/{id}",
          defaults: new { controller = "ServiceCharge", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
            name: "PropertyAmenities",
            url: "PropertyAmenities/{action}/{id}",
            defaults: new { controller = "PropertyAmenities", action = "Index" }
            );


            routes.MapRoute(
           name: "PromotionManagement",
           url: "PromotionManagement/{action}/{id}",
           defaults: new { controller = "PromotionManagement", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
          name: "default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
          );
        }
    }
}
