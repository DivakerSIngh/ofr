using FutureSoft.Util;
using OneFineRateWeb.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace OneFineRateWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var instanceModeList = DisplayModeProvider.Instance.Modes;

            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Tablet")
            {
                ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf("Tablet", StringComparison.OrdinalIgnoreCase) >= 0)
            });

            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            string action = "Index";

            if (exception != null)
                MailComponent.SendEmailTest("deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com", "", "", "From Application Error", exception.StackTrace + "<br/><br/> Message:: " + exception.GetBaseException() + DateTime.Now, null, null, true);

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case (int)HttpStatusCode.NotFound:
                        action = "NotFound";
                        break;
                    case (int)HttpStatusCode.Forbidden:
                        action = "Forbidden";
                        break;
                    case (int)HttpStatusCode.InternalServerError:
                        action = "InternalServerError";
                        break;
                    case (int)HttpStatusCode.Unauthorized:
                        action = "Unauthorized";
                        break;
                    default: break;
                }
            }

            Response.Clear();
            Server.ClearError();
            Server.TransferRequest(string.Format("~/Error/{0}", action));
        }
        protected void Application_BeginRequest(object sender, EventArgs s)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //localhost
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, PUT, DELETE");

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg == "User")
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    return "User=" + context.User.Identity.Name;
                }
                return "User=" + null;
            }

            return base.GetVaryByCustomString(context, arg);
        }
    }
}