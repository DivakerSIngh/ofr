using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;

namespace OneFineRateApp.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            return View();

        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        public string UpdatePassword(string OldPass, string NewPass)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = OneFineRateBLL.BL_Login.UpdatePassword(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, OldPass, NewPass);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Password updated successfully!." };
                }
                else
                {
                    result = new { st = 0, msg = "Old password incorrect!" };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

    }
}