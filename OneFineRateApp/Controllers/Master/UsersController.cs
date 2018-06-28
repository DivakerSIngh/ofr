using FutureSoft.ClassFiles;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class UsersController : BaseController
    {
        // GET: Users
        [Route("Users")]
        [Route("Users/Index")]
        public ActionResult Index()
        {
            return View();
        }

        public string AddUsers(string firstname, string lastname, string emailID, string contact, bool Active, string Groups)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eUsers eObj = new eUsers();
                eObj.sUserName = emailID;
                byte[] b = new byte[1];
                string Password = emailID.Substring(0, emailID.IndexOf("@")).ToLower();
                Password = SimpleHash.ComputeHash(Password, "SHA1", b);

                eObj.sPassword = Password;
                eObj.sFirstName = firstname;
                eObj.sLastName = lastname;
                eObj.sEmail = emailID;
                eObj.sContact = contact;
                eObj.dtCreatedOn = DateTime.Now;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = Active ? "A" : "D";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                int i = BL_Users.AddRecord(eObj, Groups.Substring(0, Groups.Length - 1));
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("User", 1) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("User", 0) };
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;

        }
        public string DeleteUsers(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_Users.DeleteRecord(id);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Deleted successfully." };
                }
                else
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string UpdateUsers(string Usersname, string firstname, string lastname, string emailID, string contact, bool Active, int id, string Groups)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eUsers Obj = new eUsers();
                Obj = BL_Users.GetSingleRecordById(id);
                Obj.sUserName = emailID;
                Obj.sFirstName = firstname;
                Obj.sLastName = lastname;
                Obj.sEmail = emailID;
                Obj.sContact = contact;
                Obj.dtCreatedOn = DateTime.Now;
                Obj.dtActionDate = DateTime.Now;
                Obj.cStatus = Active ? "A" : "I";
                int a = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                Obj.iActionBy = a;
                int i = BL_Users.UpdateRecord(Obj, Groups.Substring(0, Groups.Length - 1));
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("User", 2) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("User", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public bool ResetPasswordDefault(int userId, string email)
        {
            bool status = false;
            string newPassword = email.Substring(0, email.IndexOf("@"));
            int i = OneFineRateBLL.BL_Login.UpdateNewPassword(userId, newPassword);
            if (i == 1)
            {
                status = true;
            }
            else
            {
                TempData["ERROR"] = "Opps , Something went wrong, Please try after some time !";
            }
            return status;
        }

        public ActionResult EditHotelMapping(int id)
        {
            TempData["id"] = id;

            return RedirectToAction("index", "UserHotelMap");

        }
    }
}