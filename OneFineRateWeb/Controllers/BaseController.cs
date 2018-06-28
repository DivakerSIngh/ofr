using OneFineRateWeb.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;

namespace OneFineRateWeb.Controllers
{
    [ErrorHandler]
    public class BaseController : Controller
    {
        public string CurrencyCode { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["ParaxisStateId"] ==null)
            {
                Session["ParaxisStateId"] = BL_WebsiteHomePage.GetParaxisStateId();
            }
            if (Session["CurrencyCode"] == null)
            {
                eLocationDetails obj = BL_WebsiteHomePage.GetUserCountryDetails();
                var ip2 = Request.UserHostAddress;
                if (obj.CurrencyCode != null)
                {
                    Session["CurrencyCode"] = obj.CurrencyCode;
                    Session["TimeZone"] = obj.TimeZone;
                    CurrencyCode = obj.CurrencyCode;
                    List<eCurrencyFlags> lst = new List<eCurrencyFlags>();
                    try
                    {
                        lst = BL_WebsiteHomePage.GetAllCurrenyFlags(obj.CountryCode);
                        if (lst.Count > 0)
                        {
                            Session["CountryList"] = OneFineRateAppUtil.clsUtils.ConvertToJson(lst);
                        }
                        else
                        {
                            Session["CountryList"] = string.Empty;
                        }
                    }
                    catch (Exception)
                    {


                    }
                }
            }
            else
            {
                CurrencyCode = Session["CurrencyCode"].ToString();
            }

        }
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;
        //    var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error",
        //        ViewData = new ViewDataDictionary(model)
        //    };
        //    base.OnException(filterContext);
        //}
    }
}