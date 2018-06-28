using FutureSoft.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OneFineRateApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs s)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, PUT, DELETE, GET");

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }
        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();

            MailComponent.SendEmailTest("deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com", "", "", "Error", exception.StackTrace + "Message:: " + exception.ToString(), null, null, true);
            Response.Clear();
            HttpException httpException = exception as HttpException;

            //if (httpException != null)
            //{
            //    string action;

            //    switch (httpException.GetHttpCode())
            //    {
            //        case 404:
            //            // page not found
            //            action = "HttpError404";
            //            break;
            //        case 500:
            //            // server error
            //            action = "HttpError500";
            //            break;
            //        default:
            //            action = "Index";
            //            break;
            //    }
            //    // clear error on server
            //    Server.ClearError();

            //    Response.Redirect(String.Format("~/Error/{0}?message={0}", action, HttpUtility.UrlEncode(exception.StackTrace)));

            //Exception error = Server.GetLastError();
            //var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
            //if (code == 404)
            //{
            //    Response.Redirect("/Account/Login");
            //}
            //Response.Clear();
            //Server.ClearError();
            //if (ex is ThreadAbortException)
            //    return;
            //Logger.Error(LoggerType.Global, ex, "Exception");

            Response.Redirect("/Account/Login");

            //}
            //else
            //{
            //    Server.ClearError();

            //    Response.Redirect("~/Error");
            //}
        }
    }
}

