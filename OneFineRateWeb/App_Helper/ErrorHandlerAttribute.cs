using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;
using OneFineRateBLL;
using System.Threading.Tasks;
using FutureSoft.Util;

namespace OneFineRateWeb.Handlers
{
    public class ErrorHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {
                Exception ex = filterContext.Exception;
                filterContext.ExceptionHandled = true;
                var model = new HandleErrorInfo(new Exception(), "Controller", "Action");
                try
                {
                    var controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
                    var actionName = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
                    model = new HandleErrorInfo(ex.GetBaseException(), controllerName, actionName);

                    string headerMessage = "<h1>" + ex.Message + "</h1> <br/><br/>";
                  
                    Task.Run(() => MailComponent.SendEmailTest("deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com", "", "", "Error", headerMessage + ex.StackTrace + "<br/><br/><br/> Message:: " + ex.GetBaseException(), null, null, true));
                }
                catch (Exception)
                {
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { status = false, message = filterContext.Exception.GetBaseException().Message },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary(model)
                    };

                }
                // throw new Exception();
                base.OnException(filterContext);
            }
        }
    }


    public class CorporateAuthrizationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId<long>();

            var respectiveUser = BL_WebsiteUser.CheckCorporateCustomerById(userId);

            if (respectiveUser != null)
            {
                if (respectiveUser.CorporateCustomerStatus == CorporateEmailStatus.ActiveUser)
                {

                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Home/UnAuthrizedCorporate", false);
            }

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectResult("/Home/UnAuthrizedCorporate", false);
            }
        }
    }
}