using Facebook;
using FutureSoft.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OneFineRateApp.ViewModels;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OneFineRateApp.Controllers.Account
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("Account")]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string sPropId, string bookingId)
        {
            //var invoideDetail = BL_Invoice.GetInvoiceDetailByBooking(3220);
            //invoideDetail.sInvoiceNumber = invoideDetail.BookingId + "/" + "02" + (invoideDetail.cBookingStatus == "MD" ? "/MOD" : "");

            //invoideDetail.HotelOrGuest = HotelOrGuest.Hotel;

            //invoideDetail.IsGeneratingPdf = true;

            //var html_Customer = OneFineRateApp.App_Helper.EmailTempleteParser.RenderViewToString(this,"~/Views/Bookings/_Invoice.cshtml", invoideDetail);

            // SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            //SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html_Customer);

            //using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            //{
            //    doc.Save(memoryStream);
            //    byte[] bytes = memoryStream.ToArray();
            //    memoryStream.Close();

            //    var attachment = new System.Net.Mail.Attachment(new System.IO.MemoryStream(bytes), "Invoice#" + invoideDetail.BookingId + ".pdf");
            //    MailComponent.SendEmail("deepaka@futuresoftindia.com", "deepaka@futuresoftindia.com", "", "OneFineRate! Invoice# :" + invoideDetail.BookingId, html_Customer, attachment, null, true, new System.IO.MemoryStream(bytes), "Invoice#" + invoideDetail.BookingId + ".pdf");
            //}

            //doc.Close();

            ////////////////////////////////////////
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            eLogin model = new eLogin();
            model.sPropId = sPropId;
            model.sBookingId = bookingId;

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public string ForgotPassword(string emailID)
        {
            return OneFineRateBLL.BL_Login.ChangePasswordUrl(emailID);
            //generate url for change password and send as an email
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(OneFineRateBLL.Entities.eLogin model, string returnUrl)
        {
            TempData["Error"] = null;
            if (ModelState.IsValid)
            {
                OneFineRateBLL.BL_Login.UserDetails eobj = new OneFineRateBLL.BL_Login.UserDetails();
                OneFineRateBLL.BL_Login.UserDetails eobjActiveUser = new OneFineRateBLL.BL_Login.UserDetails();
                OneFineRateAppUtil.clsUtils.ConvertToObject(OneFineRateBLL.BL_Login.ValidateUser(model.UserName, model.Password), eobj);
                OneFineRateAppUtil.clsUtils.ConvertToObject(OneFineRateBLL.BL_Login.ActiveUser(model.UserName), eobjActiveUser);
                if (eobj.iUserId > 0 && eobjActiveUser.cStatus == "A")
                {
                    if (model.RememberMe)
                    {
                        Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                    }
                    else
                    {
                        Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);

                    }
                    Response.Cookies["UserName"].Value = model.UserName.Trim();
                    Response.Cookies["Password"].Value = model.Password.Trim();


                    Session["UserDetails"] = eobj;
                    Session["MenuType"] = "1";
                    //Session["Username"] = eobj.UserName;
                    //Session["FisrtName"] = eobj.FisrtName;
                    //Session["LastName"] = eobj.LastName;

                    string sPath = OneFineRateBLL.BL_Menu.GetHomePageByUserId(eobj.iUserId);
                    if (sPath != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);

                        #region Redirect For Booking Confirmation

                        if (model.sBookingId != null)
                        {
                            var bookingIdDecoded = OneFineRateAppUtil.clsUtils.Decode(model.sBookingId);

                            var iPropId = BL_Booking.GetPropIdByBookingId(bookingIdDecoded);

                            if (iPropId.HasValue)
                            {
                                Session["MenuType"] = "2";
                                Session["PropId"] = iPropId;

                                Session["CurrCode"] = BL_Currency.GetCurrencyByPropId(iPropId.Value);
                                //Session["Symbol"] = BL_Currency.GetSymbolByPropId(PropId);
                                Session["Flag"] = BL_Currency.GetFlagByPropId(iPropId.Value);
                                Session["PropName"] = BL_PropDetails.GetPropertyName(iPropId.Value);

                                return RedirectToRoute("BookingConfirmation", new { bookingId = model.sBookingId });
                            }
                        }

                        #endregion Redirect For Pending Confirmation

                        #region Redirect For Pending Negotiation

                        if (model.sPropId != null)
                        {
                            int iPropId = 0;
                            try
                            {
                                iPropId = int.Parse(clsUtils.Decode(model.sPropId));
                            }
                            catch (Exception)
                            {
                            }

                            if (iPropId > 0)
                            {
                                Session["MenuType"] = "2";
                                Session["PropId"] = iPropId;

                                Session["CurrCode"] = BL_Currency.GetCurrencyByPropId(iPropId);
                                //Session["Symbol"] = BL_Currency.GetSymbolByPropId(PropId);
                                Session["Flag"] = BL_Currency.GetFlagByPropId(iPropId);
                                Session["PropName"] = BL_PropDetails.GetPropertyName(iPropId);

                                return RedirectToAction("Index", "PendingNegotiations");
                            }
                        }

                        #endregion

                        string[] Values = sPath.Split('/');
                        if (Values.Length == 2)
                        {
                            //FormsAuthentication.RedirectFromLoginPage(model.UserName, model.RememberMe);
                            return RedirectToAction("Index", Values[1]);

                        }
                        else
                        {
                            //FormsAuthentication.RedirectFromLoginPage(model.UserName, model.RememberMe);
                            return RedirectToAction(Values[2], Values[1]);
                        }
                    }
                    else
                    {
                        TempData["Error"] = "All the Groups are Inactive for this user.";
                    }


                }

                if (eobj.iUserId == 0)
                {
                    // ModelState.AddModelError("", "Incorrect username and/or password");
                    TempData["Error"] = "Incorrect username and/or password";
                }
                else if (eobjActiveUser.cStatus == "I")
                {
                    TempData["Error"] = "User has been diasbled by Administrator.";
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ChangePassword(string id)
        {
            int userId = 0;

            if (!string.IsNullOrEmpty(id))
            {
                var mailKey = clsUtils.getmainkey();
                userId = int.Parse(Encription.AESEncryption.DecryptData(id, mailKey));
                ViewBag.HideMenu = true;
            }

            OneFineRateBLL.BL_Login.UserDetails userDetail = null;

            userDetail = Session["UserDetails"] as OneFineRateBLL.BL_Login.UserDetails;

            if (userDetail == null)
            {
                userDetail = new BL_Login.UserDetails();
                userDetail.iUserId = userId;
            }
            ViewBag.UserName = (string.IsNullOrEmpty(userDetail.FisrtName) ? "" : userDetail.FisrtName) + " " + (string.IsNullOrEmpty(userDetail.LastName) ? "" : userDetail.LastName);//Session["USER_NAME"];
            if (string.IsNullOrEmpty(Session["MenuType"] as string))
                Session["MenuType"] = 1;
            //TempData["uid"] = OneFineRateBLL.BL_Login.getDecryptedUserId(userDetails.UserName);
            return View(userDetail);

        }

        public string UpdateNewPassword(string NewPass, int uid)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                // var userDetails = Session["UserDetails"] as OneFineRateBLL.BL_Login.UserDetails;

                int i = OneFineRateBLL.BL_Login.UpdateNewPassword(uid, NewPass);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Password updated successfully!." };
                }
                else
                {
                    result = new { st = 0, msg = "Password not updated successfully." };
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
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


        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account", new { });
        }

    }
}
