using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers
{
    //[CheckSession.CheckSessionExpireFilter]
    public class BaseController : Controller
    {
        int userid = 0;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            loginLogoutS();
            if (Session["UserDetails"] != null)
            {
                var user = (OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"];
                userid = user.iUserId;
                if (Session["ControllerAction"] == null)
                {
                    Session["ControllerAction"] = OneFineRateBLL.BL_Menu.GetControllerActionByUserId(userid);
                }

                if (filterContext.ActionDescriptor.ActionName == "MappedHotels")
                {
                    Session["MenuType"] = "1";
                }
                if (Session["OldMenuType"] == null)
                {
                    Session["OldMenuType"] = Session["MenuType"].ToString();
                }

                //if (OneFineRateBLL.BL_Menu.ValidateLinkForUser(userid, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName))
                if (OneFineRateBLL.BL_Menu.ValidateUserAccess(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName))
                {
                    if (Session["Menu"] == null || Session["OldMenuType"].ToString() != Session["MenuType"].ToString())
                    {
                        Session["Menu"] = OneFineRateBLL.BL_Menu.GetMenu(userid, Convert.ToInt16(Session["MenuType"].ToString()));
                        Session["OldMenuType"] = Session["MenuType"].ToString();
                    }
                    if (Session["PropertyMenu"] == null)
                    {
                        Session["PropertyMenu"] = OneFineRateBLL.BL_Menu.GetPropertyMenu(userid);
                    }

                    ViewBag.Menu = (MvcHtmlString)Session["Menu"];//OneFineRateBLL.BL_Menu.GetMenu(userid, Convert.ToInt16(Session["MenuType"].ToString()));
                    ViewBag.PropertyMenu = (MvcHtmlString)Session["PropertyMenu"]; //OneFineRateBLL.BL_Menu.GetPropertyMenu(userid);
                    // ViewBag.DeploymentAlert = GetDeploymentAlert();
                    ViewBag.UserName = user.FisrtName + " " + user.LastName;
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    //filterContext.Result = new RedirectResult("/Account/Login", true);
                    filterContext.Result = RedirectToAction("Login", "Account");
                }
            }
            else
            {
                //filterContext.Result = new RedirectResult("/Account/Login", true);
                //base.OnActionExecuting(filterContext);
                var value = filterContext.HttpContext.Request.RawUrl;
                var bookingId = string.Empty;

                if (value.Contains("BookingConfirmation"))
                {
                    var bookingIdEncoded = value.Split('/')[2];
                    bookingId = bookingIdEncoded;
                }

                filterContext.Result = RedirectToAction("Login", "Account", new { bookingId = bookingId });
            }
        }



        public void loginLogoutS()
        {
            try
            {
                //_context = new GP_QportEntities();
                //tbl_user_login_logout_x objl = new tbl_user_login_logout_x();

                //ObjectParameter pErrFlag = new ObjectParameter("pErrFlag", typeof(bool));
                //ObjectParameter pErrDesc = new ObjectParameter("pErrDesc", typeof(string));
                //long bi_userid = Convert.ToInt64(Session["UsrId"]);
                //long bi_logid = Convert.ToInt64(Session["LOGIN_LOGOUT_ID"]);
                //_context.usp_update_user_loginlogout(bi_userid, bi_logid, "S", "N", pErrFlag, pErrDesc);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", Convert.ToString("Error from loginLogoutS-BaseController: " + ex.Message));
            }
        }
    }
}