using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateApp.Result;

namespace OneFineRateApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //stuff before
            filters.Add(new ExceptionFilters());
            //stuff after
        }
    }
}