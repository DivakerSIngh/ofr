#region usings
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OneFineRateWeb.Models;
using OneFineRateBLL.WebUserBL;
using OneFineRateBLL.WebUserEntities;
using System.Collections.Generic;
using Facebook;
using OneFineRateAppUtil;
using OneFineRateWeb.ViewModels.Models;
using System.Configuration;
using System.Net;
using System.IO;
using System.Security.Principal;
using System.Text;
using FutureSoft.Util;
using OneFineRateBLL;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using OneFineRateWeb.Handlers;
using OneFineRateBLL.Entities;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Net.Mail;
#endregion

namespace OneFineRateWeb.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {

        #region Public Property

        private WebsiteUserManager _userManager;
        public WebsiteUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<WebsiteUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        private WebsiteUserSignInManager _signInManager;

        public WebsiteUserSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<WebsiteUserSignInManager>();
            }
            private set { _signInManager = value; }
        }


        #endregion

        #region Public Methods

        [AllowAnonymous]
        [HttpGet]
        //[OutputCache(Duration = 100, VaryByParam = "*")]
        public PartialViewResult Register()
        {
            var model = new RegisterViewModel();
            var countryCodeList = OneFineRateBLL.BL_Country.GetAllCountryPhoneCodes();
            model.CountryCodePhoneList = countryCodeList;
            return PartialView("_Register", model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!string.IsNullOrEmpty(model.CorporateEmail) && string.IsNullOrEmpty(model.Email))
            {
                model.Email = model.CorporateEmail;
                if (ModelState.ContainsKey("Email"))
                {
                    ModelState["Email"].Errors.Clear();
                    ModelState.Remove("Email");
                }
            }

            if (ModelState.IsValid)
            {
                bool isPhoneExists = BL_WebsiteUser.IsPhoneNumberOrUserNameExists(model.Phone);
                bool isEmailExists = BL_WebsiteUser.IsEmailExists(model.Email);

                if (!string.IsNullOrEmpty(model.CorporateEmail))
                {
                    bool isCorporateEmailExists = BL_WebsiteUser.IsCorporateEmailExists(model.CorporateEmail);
                    if (isCorporateEmailExists)
                    {
                        return Json(new { status = false, message = "Corporate Email already Associated with an account." });
                    }
                }

                if (isPhoneExists && isEmailExists)
                {
                    return Json(new { status = false, message = "Phone number and Email already Associated with an account." });
                }
                else if (isPhoneExists)
                {
                    return Json(new { status = false, message = "Phone number already Associated with an account." });
                }
                else if (isEmailExists)
                {
                    return Json(new { status = false, message = "Email already Associated with an account." });
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.RefereeCode))
                    {
                        var refreeUserId = BL_WebsiteUser.GetUserIdByReferralCode(model.RefereeCode);

                        if (refreeUserId == default(long))
                        {
                            return Json(new { status = false, message = "Referral code didn't match any account." });
                        }
                    }

                    if (model.IsCorporateRequested)
                    {
                        VerifyPhoneViewModel verifyModel = new VerifyPhoneViewModel()
                        {
                            CompanyName = model.CompanyName,
                            CorporateEmail = model.CorporateEmail,
                            Password = model.Password,
                            Phone = model.Phone,
                            RefereeCode = model.RefereeCode,
                            sCountryCode = model.sCountryCode,
                            Title = model.Title,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsCorporateRequested = model.IsCorporateRequested,

                        };

                        return await VerifyPhoneAndRegister(verifyModel);
                    }

                    var phoneVerificationCode = clsUtils.GetVerificationCode();
                    var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                    string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                    clsUtils.sendSMS(model.Phone, message);
                    return PartialView("_VerifyPhoneNumber", new VerifyPhoneViewModel
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = model.Password,
                        CorporateEmail = model.CorporateEmail,
                        Email = model.Email,
                        Phone = model.Phone,
                        ConfirmPassword = model.ConfirmPassword,
                        PhoneVerificationEncoded = encodedCode,
                        RefereeCode = model.RefereeCode,
                        IsCorporateRequested = model.IsCorporateRequested
                    });
                }
            }
            return Json(new { status = false, errors = GetErrorsFromModelState(ModelState.Values.ToList()) });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyPhoneAndRegister(VerifyPhoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var signUpUser = new WebsiteUser
                {
                    Title = model.Title.ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Phone,
                    PhoneNumber = model.Phone,
                    PhoneNumberConfirmed = false,
                    Email = model.Email,
                    CorporateEmail = model.CorporateEmail,
                    CompanyName = model.CompanyName
                };

                if (model.IsCorporateRequested)
                {
                    if (string.IsNullOrEmpty(model.Email))
                    {
                        signUpUser.Email = model.CorporateEmail;
                        signUpUser.CorporateEmail = string.Empty;
                    }
                }
                else
                {
                    var machineGeneratedCodeForVerification = clsUtils.Decode(model.PhoneVerificationEncoded);

                    if (model.PhoneVerificationCode != machineGeneratedCodeForVerification)
                    {
                        return Json(new { status = false, message = "Verification code didn't match!" });
                    }

                    signUpUser.PhoneNumberConfirmed = true;
                }

                var result = await UserManager.CreateAsync(signUpUser, model.Password);

                if (result.Succeeded)
                {
                    signUpUser = BL_WebsiteUser.GetSingleRecordByUserName(signUpUser.UserName);
                    signUpUser.sReferralCode = clsUtils.GenerateReferralCode(signUpUser.Id, codeLength: 8);
                    BL_WebsiteUser.UpdateRecord(signUpUser);

                    var refreeUserId = BL_WebsiteUser.GetUserIdByReferralCode(model.RefereeCode);

                    if (!string.IsNullOrEmpty(model.RefereeCode) && refreeUserId == 0)
                    {
                        return Json(new { status = false, message = "Referral code doesn't match any account!" });
                    }

                    var resultStatus = BL_WebsiteUser.UpdateCustomerLoyalityPoints(refreeUserId, signUpUser.Id);

                    var signInResult = await SignInManager.PasswordSignInAsync(signUpUser.Email, model.Password, true, shouldLockout: false);

                    switch (signInResult)
                    {
                        case SignInStatus.Success:
                            {
                                string customerLoyalityPointLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/Manage/RewardPoints";
                                string message = "Dear " + signUpUser.Title + " " + signUpUser.FirstName + " " + signUpUser.LastName + ", Thank you for your registeration in OFR. Your User name is " + signUpUser.Email + ". Review your bookings and stay updated  on Loyalty points earned by clicking " + customerLoyalityPointLink;
                                clsUtils.sendSMS(signUpUser.PhoneNumber, message);

                                if (model.IsCorporateRequested)
                                {
                                    var code = clsUtils.GetVerificationCode();
                                    var verificationCode = code.ToString();
                                    var verificationCodeEncoded = clsUtils.Encode(verificationCode);
                                    MailComponent.SendEmail(model.CorporateEmail, "", "", string.Format("OFR E-mail verification  Code-{0}", verificationCode), "Dear Subscriber,<br/><br/>Thanks for visiting  OFR . Your unique OFR email verification code is <b>" + verificationCode + "</b>.<br/>This code will be used to verify your E-mail address.<br/><br/><br/>Regards<br/>OFR Team. ", null, null, true, null, null);

                                    return Json(new { status = true, isCorporateRequested = model.IsCorporateRequested, message = "An OTP code has been sent on " + model.CorporateEmail });
                                }
                                else
                                {
                                    return Json(new { status = true, message = "Your registration was successful. Signing In..." });
                                }
                            }
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "User name or password incorrect !");
                            return Json(new { status = false, errors = GetErrorsFromModelState(ModelState.Values.ToList()) });
                    }

                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    //ViewBag.Link = callbackUrl;
                    //return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return Json(new { status = false, errors = GetErrorsFromModelState(ModelState.Values.ToList()) });
        }


        [AllowAnonymous]
        [HttpGet]
        //[OutputCache(Duration = 100, VaryByParam = "*")]
        public PartialViewResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();

            var persistentCookie = Request.Cookies.Get("ofrLogin");

            if (persistentCookie != null)
            {
                var email = persistentCookie.Values.Get("ofrEmailId");
                var password = persistentCookie.Values.Get("ofrPassword");
                var rememberMe = persistentCookie.Values.Get("ofrRememberMe");
                model.EmailOrPhone = email;
                model.Password = password;
                model.RememberMe = !String.IsNullOrEmpty(rememberMe) ? Convert.ToBoolean(rememberMe) : false;
            }

            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_Login", model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            bool isRedirectedToHomePage = returnUrl.Contains("Account/ResetPassword");

            if (!ModelState.IsValid)
            {
                return Json(new { status = false, errors = GetErrorsFromModelState(ModelState.Values.ToList()) });
            }

            // This doesn't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.EmailOrPhone, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        if (model.RememberMe)
                        {
                            HttpCookie cookie = new HttpCookie("ofrLogin");
                            cookie.Values.Add("ofrEmailId", model.EmailOrPhone);
                            cookie.Values.Add("ofrPassword", model.Password);
                            cookie.Values.Add("ofrRememberMe", model.RememberMe.ToString());
                            cookie.Expires = DateTime.Now.AddDays(15);
                            Response.Cookies.Add(cookie);
                        }
                        else
                        {
                            var persistentCookie = Request.Cookies.Get("ofrLogin");

                            if (persistentCookie != null)
                            {
                                persistentCookie.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(persistentCookie);
                            }
                        }

                        if (!string.IsNullOrEmpty(model.CorporateEmail))
                        {
                            var loggedInUser = UserManager.FindByEmail(model.EmailOrPhone);

                            var corporateEmailExistsForAnotherUser = BL_WebsiteUser.IsAnyEmailExistsV2(model.CorporateEmail, loggedInUser.Id);

                            if (corporateEmailExistsForAnotherUser)
                            {
                                TempData["ERROR"] = string.Format("The corporate email ({0}) you have provided already associated with another account.", model.CorporateEmail);
                            }
                            else
                            {
                                //TO DO Aditya
                                loggedInUser.CorporateEmail = model.CorporateEmail;
                                loggedInUser.CorporateEmailConfirmed = false;
                                BL_WebsiteUser.UpdateRecord(loggedInUser);
                            }
                        }

                        //var loggedInUser = UserManager.FindByEmail(model.EmailOrPhone);
                        //string customerLoyalityPointLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/Manage/RewardPoints";
                        //string message = "Dear " + loggedInUser.Title + " " + loggedInUser.FirstName + " " + loggedInUser.LastName + ", Thank you for your registeration in OFR.  Your User name is " + loggedInUser.Email + ". Review your bookings and stay updated  on Loyalty points earned by clicking <a href=" + customerLoyalityPointLink + "> here.</a>";
                        //clsUtils.sendSMS(loggedInUser.PhoneNumber, message);
                        if(isRedirectedToHomePage)
                            return Json(new
                            {
                                returnUrl = new Uri(returnUrl).GetLeftPart(System.UriPartial.Authority),
                                isCorporateRequested = model.IsCorporateRequested,
                                status = true
                            });
                        return Json(new
                        {
                            returnUrl = returnUrl,
                            isCorporateRequested = model.IsCorporateRequested,
                            status = true
                        });
                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "User name or password incorrect !");
                    return Json(new { status = false, errors = GetErrorsFromModelState(ModelState.Values.ToList()) });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult ResendVerification(string phone)
        {
            try
            {
                var phoneVerificationCode = clsUtils.GetVerificationCode();
                var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                clsUtils.sendSMS(phone, message);
                return Json(new { encodedCode });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
                throw;
            }
        }

        /// <summary>
        ///Called when user is logged-In and click on Corporate button
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckIfCorporateUser()
        {
            var userId = User.Identity.GetUserId<long>();
            var respectiveUser = BL_WebsiteUser.CheckCorporateCustomerById(userId);

            bool status = false;
            string message = string.Empty;
            string verificationCode = string.Empty;
            string verificationCodeEncoded = string.Empty;
            bool emailOfDomainNotFound = false;

            if (respectiveUser != null)
            {
                switch (respectiveUser.CorporateCustomerStatus)
                {
                    case CorporateEmailStatus.BlackListed:
                        {
                            message = "Sorry! Your email is not active. You can still get highly discounted rates by Bidding or Negotiaion.";
                            emailOfDomainNotFound = true;
                            break;
                        }
                    case CorporateEmailStatus.CorporateEmailNotRegistered:
                        {
                            message = "To know your Corporate rates, Please provide your Company Email Address and a one time verification password will be sent.";
                            break;
                        }
                    case CorporateEmailStatus.EmailOrDomainNotFound:
                        {
                            message = "Sorry! Your organisation is currently not part of OFR's Corporate Circle. You can still avail highly discounted rates by Bidding or Negotiation.  Please do register with us and we will get in touch with you soon.";
                            emailOfDomainNotFound = true;
                            break;
                        }
                    case CorporateEmailStatus.ActiveUser:
                        {
                            if (string.IsNullOrEmpty(respectiveUser.CorporateEmail))
                            {
                                respectiveUser.CorporateEmail = respectiveUser.Email;
                            }

                            var code = clsUtils.GetVerificationCode();
                            verificationCode = code.ToString();
                            verificationCodeEncoded = clsUtils.Encode(verificationCode);

                            MailComponent.SendEmail(respectiveUser.CorporateEmail, "", "", string.Format("OFR E-mail verification  Code-{0}", verificationCode), "Dear Subscriber,<br/><br/>Thanks for visiting  OFR . Your unique OFR email verification code is <b>" + verificationCode + "</b>.<br/>This code will be used to verify your E-mail address.<br/><br/><br/>Regards<br/>OFR Team. ", null, null, true, null, null);

                            message = "Please enter the One Time Verification Password sent on your Company's Email Address";
                            status = true;
                            break;
                        }
                    default:
                        {
                            message = "An unknown error occured!";
                            break;
                        }
                }
            }

            return Json(new
            {
                status = status,
                message = message,
                corporateEmail = respectiveUser.CorporateEmail,
                code = verificationCode,
                codeEncoded = verificationCodeEncoded,
                emailOfDomainNotFound = emailOfDomainNotFound

            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult VerifyCorporateEmail()
        {
            return PartialView("_VerifyCorporateEmail");
        }
        /// <summary>
        /// Verify Corporate email after otp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCorporateEmail(VerifyCorporateEmailViewModel model)
        {
            bool status = false;
            bool emailOfDomainNotFound = false;
            string message = string.Empty;
            string corporateEmail = string.Empty;
            string corporateEmailHidden = model.HiddenCorporateEmail;
            string verificationCode = string.Empty;
            string verificationCodeEncoded = string.Empty;
            bool showOTPMessage = false;
            bool loginRequired = false;
            bool emailAlreadyAssociated = false;
            string companyName = "Company";

            bool isEmailChanges = false;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.HiddenCorporateEmail) && model.CorporateEmail.ToLower() != model.HiddenCorporateEmail.ToLower())
                {
                    isEmailChanges = true;
                }

                if ((!string.IsNullOrEmpty(model.VerificationEncoded) || !string.IsNullOrEmpty(model.EmailVerificationCode)) && !isEmailChanges)
                {
                    var userCodeEncode = clsUtils.Encode(model.EmailVerificationCode);

                    if (userCodeEncode == model.VerificationEncoded)
                    {
                        try
                        {
                            //Check the user is registered or not
                            var websiteUser = BL_WebsiteUser.GetWebisteUserByEmail(model.CorporateEmail);

                            if (websiteUser == null)
                            {
                                websiteUser = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                            }

                            //Registered user
                            if (User.Identity.IsAuthenticated)
                            {
                                if (!string.IsNullOrEmpty(model.CorporateEmail) && !string.IsNullOrEmpty(websiteUser.CorporateEmail))
                                {
                                    if (model.CorporateEmail.ToLower() != websiteUser.CorporateEmail.ToLower())
                                    {
                                        AuthenticationManager.SignOut();
                                    }
                                }
                                //TO DO Aditya

                                //Once user is confirmed as Corporate 
                                websiteUser.CorporateEmail = model.CorporateEmail;
                                websiteUser.CorporateEmailConfirmed = true;
                                BL_WebsiteUser.UpdateRecord(websiteUser);
                                status = true;
                            }
                            else
                            {
                                //TO DO Aditya
                                websiteUser.CorporateEmail = model.CorporateEmail;
                                websiteUser.CorporateEmailConfirmed = true;
                                BL_WebsiteUser.UpdateRecord(websiteUser);
                                status = true;
                            }

                            //Auto sing in in case user is corporate and not signed in
                            await SignInManager.SignInAsync(websiteUser, isPersistent: false, rememberBrowser: false);
                        }
                        catch (Exception)
                        {
                            status = false;
                            message = "An unknown error had happen, kindly try after some time.";
                        }
                    }
                    else
                    {
                        verificationCodeEncoded = model.VerificationEncoded;
                        message = "OTP is not correct!";
                        showOTPMessage = true;
                    }
                }
                else
                {

                    var emailAvailableStatus = BL_WebsiteUser.IsCorporateEmailExits(model.CorporateEmail);

                    if (emailAvailableStatus == CorporateEmailStatus.BlackListed || emailAvailableStatus == CorporateEmailStatus.EmailOrDomainNotFound)
                    {
                        message = "Welcome to OFR Corporate Club! Please provide your contact details and our executive will assist you with the registration. In the mean time, please feel free to use our other offering such as Bid/ Bargain or Book to get the best deal.";
                        emailOfDomainNotFound = true;
                    }
                    else
                    {
                        var corporateWebisterUser = BL_WebsiteUser.CheckIfCustomerRegisteredWithCorporate(model.CorporateEmail);
                        //New Corporate user regsitartion 
                        if (corporateWebisterUser == null && User.Identity.IsAuthenticated && isEmailChanges)
                        {
                            var code = clsUtils.GetVerificationCode();
                            verificationCode = code.ToString();
                            verificationCodeEncoded = clsUtils.Encode(verificationCode);
                            MailComponent.SendEmail(model.CorporateEmail, "", "", string.Format("OFR E-mail verification  Code-{0}", verificationCode), "Dear Subscriber,<br/><br/>Thanks for visiting  OFR . Your unique OFR email verification code is <b>" + verificationCode + "</b>.<br/>This code will be used to verify your E-mail address.<br/><br/><br/>Regards<br/>OFR Team. ", null, null, true, null, null);
                            message = "An OTP sent to " + model.CorporateEmail;

                        }

                        else if (corporateWebisterUser == null && !User.Identity.IsAuthenticated)
                        {
                            companyName = BL_WebsiteUser.GetCompanyNameByCorporateEmail(model.CorporateEmail);
                            message = "<b>Congratulation! " + companyName + "</b> is an OFR Corporate Club member, however we do not have your official details. Updating this is simple! Regular OFR members can avail of Corporate Club rates by logging in with their personal ID and OFR will update their official email id automatically. New members will be required to fill in the details as requested.";
                            loginRequired = true;
                        }
                        else
                        {

                            if (isEmailChanges)
                            {
                                var emailAlreadyExists = BL_WebsiteUser.IsAnyEmailExistsV2(model.CorporateEmail, User.Identity.GetUserId<long>());

                                if (emailAlreadyExists)
                                {
                                    status = false;
                                    emailAlreadyAssociated = true;
                                    message = "Email already associated with another account.";
                                }
                            }
                            else
                            {
                                var code = clsUtils.GetVerificationCode();
                                verificationCode = code.ToString();
                                verificationCodeEncoded = clsUtils.Encode(verificationCode);

                                MailComponent.SendEmail(model.CorporateEmail, "", "", string.Format("OFR E-mail verification  Code-{0}",verificationCode), "Dear Subscriber,<br/><br/>Thanks for visiting  OFR . Your unique OFR email verification code is <b>" + verificationCode + "</b>.<br/>This code will be used to verify your E-mail address.<br/><br/><br/>Regards<br/>OFR Team. ", null, null, true, null, null);
                                message = "One Time Verification Password (OTP) sent to " + model.CorporateEmail;
                            }
                        }
                    }
                }
            }

            return Json(new
            {
                status = status,
                message = message,
                emailOfDomainNotFound = emailOfDomainNotFound,
                code = verificationCode,
                codeEncoded = verificationCodeEncoded,
                showOTPMessage = showOTPMessage,
                corporateEmail = model.CorporateEmail,
                loginRequired = loginRequired,
                companyName = companyName,
                emailAlreadyAssociated = emailAlreadyAssociated
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///  send otp on coprorate email
        /// </summary>
        /// <param name="corporateEmail"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult ResendCorporateOTP(string corporateEmail)
        {

            bool status = false;
            string message = string.Empty;
            string verificationCode = string.Empty;
            string verificationCodeEncoded = string.Empty;

            if (string.IsNullOrEmpty(corporateEmail))
            {
                return Json(new
                {
                    status = status,
                    message = "Please provide corporate email.",

                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var code = clsUtils.GetVerificationCode();
                verificationCode = code.ToString();
                verificationCodeEncoded = clsUtils.Encode(verificationCode);
                MailComponent.SendEmail(corporateEmail, "", "", string.Format("OFR E-mail verification  Code-{0}",verificationCode), "Dear Subscriber,<br/><br/>Thanks for visiting  OFR . Your unique OFR email verification code is <b>" + verificationCode + "</b>.<br/>This code will be used to verify your E-mail address.<br/><br/><br/>Regards<br/>OFR Team. ", null, null, true, null, null);
                message = "OTP sent to " + corporateEmail;
                status = true;
            }
            catch (Exception)
            {
                message = "Error in sending OTP! Kindly try after some time.";
            }

            return Json(new
            {
                status = status,
                message = message,
                code = verificationCode,
                codeEncoded = verificationCodeEncoded
            },
           JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId > 0 || code == null)
            {
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            return View("Error");
        }

        [AllowAnonymous]
        public ActionResult GetLoginRegisterForm()
        {
            return PartialView("_LoginRegister");
        }

        [AllowAnonymous]
        public ActionResult GetCorporateLoginRegisterForm()
        {
            return PartialView("_CorporateLoginRegister");
        }

        [AllowAnonymous]
        public PartialViewResult ManageBookingPopup()
        {
            return PartialView("_ManageBookingPopup");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView("_ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return Json(new { status = false, message = "We couldn't find an OFR user with that email address. Please check for any misspellings." });
                    }
                    //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    //{
                    //    // Don't reveal that the user does not exist or is not confirmed
                    //    return View("ForgotPasswordConfirmation");
                    //}
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, protocol: Request.Url.Scheme);


                    string body = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/_ResetPasswordTemplete.html"));

                    body = body.Replace("{{Name}}", user.FirstName);
                    body = body.Replace("{{CompanyName}}", clsUtils.ApplicationName());
                    body = body.Replace("{{CallbackUrl}}", callbackUrl);
                    body = body.Replace("{{browser_name}}", Request.Browser.Type);
                    body = body.Replace("{{operating_system}}", Request.UserAgent);
                    body = body.Replace("{{Date}}", DateTime.Now.Year.ToString());

                    //StringBuilder stringBuilder = new StringBuilder();

                    //stringBuilder.Append("Dear " + user.FirstName + ",</br>");
                    //stringBuilder.Append("Click <a href = " + callbackUrl + ">here</a> to reset your password for " + clsUtils.ApplicationName() + " application.");
                    //stringBuilder.Append("</br>In case the above link doesn't work then please copy the url mentioned below and paste it on browser.");
                    //stringBuilder.Append("</br>" + callbackUrl);
                    //stringBuilder.Append("</br></br>This is system generated email, Please do not reply.");
                    //stringBuilder.Append("</br></br>Regards");
                    //stringBuilder.Append("</br>Administrator");

                    //await UserManager.SendEmailAsync(user.Id, model.Email, stringBuilder.ToString());
                    await UserManager.SendEmailAsync(user.Id, model.Email, body);
                    //return PartialView("ForgotPasswordConfirmation");
                    return Json(new { status = true, message = "A link has been sent your email. Please check your email to reset your password." });
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                    throw;
                }
            }

            // If we got this far, something failed, redisplay form
            return Json(new { status = false, message = "We couldn't find a OFR user with that email address. Please check for any misspellings." });
        }


        [AllowAnonymous]
        public ActionResult ResetPassword(int? userId, string token)
        {
            AuthenticationManager.SignOut();
            var resetPasswordViewModel = new ResetPasswordViewModel();
            if (userId != null)
            {
                var validUser = UserManager.FindById(userId.Value);
                resetPasswordViewModel.Email = validUser.Email;
                resetPasswordViewModel.Token = token;
            }
            return token == null ? View("Error") : View(resetPasswordViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) }, JsonRequestBehavior.AllowGet);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return Json(new { status = false, message = "No user found with " + model.Email }, JsonRequestBehavior.AllowGet);
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);

                //var signResult = await SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, shouldLockout: false);
                //switch (signResult)
                //{

                //    case SignInStatus.Success:
                //        return Json(new { status = true, message = "Your password has been reset." }, JsonRequestBehavior.AllowGet);
                //    case SignInStatus.Failure:
                //        return Json(new { status = false, message = "Something went wrong." }, JsonRequestBehavior.AllowGet);
                //}
            }
            AddErrors(result);
            return Json(new { status = false, message = "Sorry! This link has expired, please request a new one" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl, bool? isCorporateRequested, string corporateEmail)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl, isCorporate = isCorporateRequested, corporateEmail = corporateEmail }));
        }


        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, bool? isCorporate, string corporateEmail)
        {
            TempData["ERROR"] = null;

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                //THIS SHOULD FIX OF THE ISSUE OF SECOND BROWSER LOGIN
                loginInfo = await AuthenticationManager_GetExternalLoginInfoAsync_Workaround();
                if (loginInfo == null)
                {
                    TempData["ERROR"] = "Something went wrong !";
                    return RedirectToAction("Index", "Home");
                }
            }


            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        if (isCorporate.HasValue && isCorporate.Value)
                        {
                            HttpCookie cookie = new HttpCookie("ofr.corporate.required.refresh.click");
                            cookie.Value = "true";
                            this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                            var user = BL_WebsiteUser.GetWebisteUserByEmail(loginInfo.Email);

                            var corporateEmailExistForAnotherUser = BL_WebsiteUser.IsAnyEmailExistsV2(email: corporateEmail, userId: user.Id);

                            if (corporateEmailExistForAnotherUser)
                            {
                                TempData["ERROR"] = string.Format("Sorry! The Email Address {0} is already associated with an account.", corporateEmail);
                            }
                            else
                            {
                                user.CorporateEmail = corporateEmail;
                                user.CorporateEmailConfirmed = false;
                                BL_WebsiteUser.UpdateRecord(user);
                            }

                            return RedirectToAction("Index", "Home");

                            //ExternalLoginConfirmationViewModel roundTripModel = new ExternalLoginConfirmationViewModel();
                            //roundTripModel.Title = user.Title;
                            //roundTripModel.FirstName = user.FirstName;
                            //roundTripModel.LastName = user.LastName;
                            //roundTripModel.DisplayName = user.FirstName + " " + user.LastName;
                            //roundTripModel.Email = user.Email;
                            //roundTripModel.PhoneNumber = user.PhoneNumber;
                            //roundTripModel.CorporateCustomerEmail = user.CorporateEmail ?? corporateEmail;
                            //roundTripModel.CorporateEmailVerificationEncoded = clsUtils.Encode(verificationCode.ToString());
                            //MailComponent.SendEmail(roundTripModel.CorporateCustomerEmail, "", "", "Email Verification", "Use the code : <b>" + verificationCode + "</b> to validate your email address", null, null, true);
                            //ModelState.AddModelError("CustomModelInfo", "An OTP sent to " + roundTripModel.CorporateCustomerEmail);
                            //ModelState.AddModelError("OTPRequired", "Please enter the otp");

                            //return View("ExternalCorporateRoundTrip", roundTripModel);
                        }

                        return RedirectToLocal(returnUrl ?? "/");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:

                    //// Uncomment the following code if user should allow to login from gmail and facebook with same email Id
                    //// View - > MyAccount.cshtml - > Uncomment ManageLogin partial view
                    //var existingUser = await UserManager.FindByEmailAsync(loginInfo.Email);
                    //if (existingUser != null)
                    //{
                    //    var guess_result = await UserManager.AddLoginAsync(existingUser.Id, loginInfo.Login);
                    //    if (guess_result.Succeeded)
                    //    {
                    //        await SignInManager.SignInAsync(existingUser, isPersistent: false, rememberBrowser: false);
                    //        return Redirect(returnUrl);
                    //    }
                    //}

                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    var externalLoginViewModel = new ExternalLoginConfirmationViewModel();

                    externalLoginViewModel.IsCorporateCustomer = isCorporate.HasValue ? isCorporate.Value : false;
                    externalLoginViewModel.CorporateCustomerEmail = corporateEmail;
                    externalLoginViewModel.CompanyName = BL_WebsiteUser.GetCompanyNameByCorporateEmail(corporateEmail);

                    var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);

                    if (loginInfo.Login.LoginProvider.Equals(PropertyConstant.Facebook, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var facebookingUserJsonString = identity.FindFirstValue(PropertyConstant.FacebookUser);
                        FacebookUserViewModel facebookUser = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookUserViewModel>(facebookingUserJsonString);
                        externalLoginViewModel.DisplayName = facebookUser.first_name + " " + facebookUser.last_name;

                        externalLoginViewModel.Email = facebookUser.email;
                        externalLoginViewModel.LastName = facebookUser.last_name;
                        externalLoginViewModel.FirstName = facebookUser.first_name;
                        externalLoginViewModel.Gender = facebookUser.gender;
                        externalLoginViewModel.DateOfBirth = facebookUser.birthday;

                        if (facebookUser.gender == "male")
                        {
                            externalLoginViewModel.Title = "Mr.";
                        }
                        else if (facebookUser.gender == "female")
                        {
                            externalLoginViewModel.Title = "Ms.";
                        }

                        externalLoginViewModel.ProfilePicUrl = facebookUser.picture.data.url;
                    }
                    else if (loginInfo.Login.LoginProvider.Equals(PropertyConstant.Google, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var googleUserJsonString = identity.FindFirstValue(PropertyConstant.GoogleUser);

                        GoogleUserViewModel googleUser = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleUserViewModel>(googleUserJsonString);
                        externalLoginViewModel.DisplayName = googleUser.displayName;
                        externalLoginViewModel.Email = googleUser.emails[0].value;
                        externalLoginViewModel.LastName = googleUser.name.familyName;
                        externalLoginViewModel.FirstName = googleUser.name.givenName;

                        //TO DO
                        ////externalLoginViewModel.Gender = "";
                        if (googleUser.name.givenName == "male")
                        {
                            externalLoginViewModel.Title = "Mr.";
                        }
                        else if (googleUser.name.givenName == "female")
                        {
                            externalLoginViewModel.Title = "Ms.";
                        }
                        externalLoginViewModel.DateOfBirth = googleUser.birthday;
                        externalLoginViewModel.ProfilePicUrl = googleUser.image.url.Replace("?sz=50", "?sz=300");
                    }
                    externalLoginViewModel.CountryCodePhoneList = OneFineRateBLL.BL_Country.GetAllCountryPhoneCodes();
                    if (externalLoginViewModel.IsCorporateCustomer)
                    {
                        return View("ExternalCorporateLoginConfirmation", externalLoginViewModel);
                    }

                    return View("ExternalLoginConfirmation", externalLoginViewModel);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Json(new { status = false, message = "Sorry, An error has been occurred, Login Failed !" }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(model.PhoneNumber) && string.IsNullOrEmpty(model.PhoneVerificationCode))
                {
                    var isPhoneNumberExists = BL_WebsiteUser.IsPhoneNumberOrUserNameExists(model.PhoneNumber);

                    //var isEmailExists = BL_WebsiteUser.IsEmailExists(model.Email);

                    //if(isPhoneNumberExists && isEmailExists)
                    //{
                    //    return Json(new { status = false, message= "The Phone number and Email you have provided is already registered." }, JsonRequestBehavior.AllowGet);
                    //}
                    //else if(isEmailExists)
                    //{
                    //    return Json(new { status = false, message= "The Email you have provided is already registered." }, JsonRequestBehavior.AllowGet);
                    //}
                    //else 

                    if (isPhoneNumberExists)
                    {
                        return Json(new { status = false, message = "The Phone number you have provided is already registered." }, JsonRequestBehavior.AllowGet);
                    }

                    var phoneVerificationCode = clsUtils.GetVerificationCode();
                    var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                    string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                    //clsUtils.sendSMS(model.PhoneNumber, message);
                    return Json(new { status = true, phoneVerificationCode = encodedCode, returnUrl = returnUrl, message = "Verification code sent to " + model.PhoneNumber }, JsonRequestBehavior.AllowGet);
                }

                var decodedPhoneVerificationCode = clsUtils.Decode(model.PhoneVerificationEncoded);
                if (model.PhoneVerificationCode == decodedPhoneVerificationCode)
                {
                    long refreeUserId = 0L;

                    if (!string.IsNullOrEmpty(model.RefereeCode))
                    {
                        refreeUserId = BL_WebsiteUser.GetUserIdByReferralCode(model.RefereeCode);

                        if (refreeUserId == default(long))
                        {
                            return Json(new { status = false, message = "The Referral code you have provided didn't match any account. !" });
                        }
                    }

                    var signUpUser = new WebsiteUser { Title = model.Title.ToString(), DisplayName = model.DisplayName, FirstName = model.FirstName, LastName = model.LastName, UserName = model.PhoneNumber, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true, Email = model.Email };
                    var result = await UserManager.CreateAsync(signUpUser);
                    if (result.Succeeded)
                    {
                        signUpUser = UserManager.FindByEmail(signUpUser.Email);
                        var referralCode = clsUtils.GenerateReferralCode(signUpUser.Id, codeLength: 8);
                        signUpUser.sReferralCode = referralCode;

                        await Task.Run(() => UploadCustomerProfileImage(signUpUser.Id, model.ProfilePicUrl));

                        signUpUser.ProfileImageUrl = string.Format("customerprofileimages/profile{0}.png", signUpUser.Id);

                        UserManager.Update(signUpUser);

                        var resultStatus = BL_WebsiteUser.UpdateCustomerLoyalityPoints(refreeUserId, signUpUser.Id);

                        result = await UserManager.AddLoginAsync(signUpUser.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(signUpUser, isPersistent: false, rememberBrowser: false);
                            return Json(new { status = true, returnUrl = returnUrl, isCorporateRequested = model.IsCorporateRequested }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //else
                    //{
                    //    var existingUser = await UserManager.FindByEmailAsync(signUpUser.Email);
                    //    if (existingUser != null)
                    //    {
                    //        result = await UserManager.AddLoginAsync(existingUser.Id, info.Login);
                    //        if (result.Succeeded)
                    //        {
                    //            await SignInManager.SignInAsync(existingUser, isPersistent: false, rememberBrowser: false);
                    //            return Json(new { status = true, returnUrl = returnUrl }, JsonRequestBehavior.AllowGet);
                    //        }
                    //    }
                    //}

                    ModelState.AddModelError("", "Its seems you have already register with email <strong>" + model.Email + " </strong> by another provider.");

                }
                else
                {
                    ModelState.AddModelError("", "Verification Code didn't match !");
                }
            }

            return Json(new { status = false, returnUrl = returnUrl, message = GetErrorsFromModelState(ModelState.Values.ToList()) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalCorporateLoginConfirmation(ExternalLoginConfirmationViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.CountryCodePhoneList = OneFineRateBLL.BL_Country.GetAllCountryPhoneCodes();
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    ModelState.AddModelError("CustomModelError", "Sorry, An error has been occurred, Login Failed !");
                    return View(model);
                }

                ViewBag.LoginProvider = info.Login.LoginProvider;

                if (!string.IsNullOrEmpty(model.PhoneNumber) && !string.IsNullOrEmpty(model.CorporateCustomerEmail))
                {
                    var isPhoneNumberExists = BL_WebsiteUser.IsPhoneNumberOrUserNameExists(model.PhoneNumber);

                    var isEmailExists = BL_WebsiteUser.IsAnyEmailExists(model.CorporateCustomerEmail);

                    if (isPhoneNumberExists && isEmailExists)
                    {
                        ModelState.AddModelError("CustomModelError", "The Phone number and Corporate Email you have provided is already registered.");
                        return View(model);
                    }
                    else if (isEmailExists)
                    {
                        ModelState.AddModelError("CustomModelError", "The Corporate Email (" + model.CorporateCustomerEmail + ") you have provided is already registered.");
                    }
                    else if (isPhoneNumberExists)
                    {
                        ModelState.AddModelError("CustomModelError", "The Phone number you have provided is already registered.");
                    }
                    else
                    {
                        var isEmailExists_Personal = BL_WebsiteUser.IsAnyEmailExists(model.Email);
                        if (isEmailExists_Personal)
                        {
                            ModelState.AddModelError("CustomModelError", string.Format("Account already exists with email - {0}, please try with another social login with same id.", model.Email));
                        }
                    }

                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }
                }


                long refreeUserId = 0L;

                if (!string.IsNullOrEmpty(model.RefereeCode))
                {
                    refreeUserId = BL_WebsiteUser.GetUserIdByReferralCode(model.RefereeCode);

                    if (refreeUserId == default(long))
                    {
                        ModelState.AddModelError("CustomModelError", "The Referral code you have provided didn't match any account. !");
                        return View(model);
                    }
                }

                var signUpUser = new WebsiteUser
                {
                    Title = model.Title.ToString(),
                    DisplayName = model.DisplayName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = model.Email,
                    CorporateEmail = model.CorporateCustomerEmail
                };

                var result = await UserManager.CreateAsync(signUpUser);
                if (result.Succeeded)
                {
                    signUpUser = UserManager.FindByEmail(signUpUser.Email);
                    var referralCode = clsUtils.GenerateReferralCode(signUpUser.Id, codeLength: 8);
                    signUpUser.sReferralCode = referralCode;

                    await Task.Run(() => UploadCustomerProfileImage(signUpUser.Id, model.ProfilePicUrl));

                    signUpUser.ProfileImageUrl = string.Format("customerprofileimages/profile{0}.png", signUpUser.Id);

                    UserManager.Update(signUpUser);

                    var resultStatus = BL_WebsiteUser.UpdateCustomerLoyalityPoints(refreeUserId, signUpUser.Id);

                    result = await UserManager.AddLoginAsync(signUpUser.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(signUpUser, isPersistent: false, rememberBrowser: false);

                        HttpCookie cookie = new HttpCookie("ofr.corporate.required.refresh.click");
                        cookie.Value = "true";
                        this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomModelError", "An unknown error had happen, kindly try after some time.");
                    }
                }
                else
                {
                    ModelState.AddModelError("CustomModelError", string.Format("Account already exists with email - {0}, please try with another social login with same id.", signUpUser.Email));
                }



                //if (!string.IsNullOrEmpty(model.CorporateCustomerEmail) && string.IsNullOrEmpty(model.CorporateEmailVerificationCode))
                //{
                //    try
                //    {
                //        var code = clsUtils.GetVerificationCode();
                //        model.CorporateEmailVerificationEncoded = clsUtils.Encode(code.ToString());
                //        //MailComponent.SendEmail(model.CorporateCustomerEmail, "", "", "Email Verification", "Use the code : <b>" + verificationCode + "</b> to validate your email address", null, null, true);
                //        ModelState.AddModelError("CustomModelInfo", "An OTP sent to " + model.CorporateCustomerEmail);
                //        ModelState.AddModelError("OTPRequired", "Please enter the otp");
                //    }
                //    catch (Exception)
                //    {
                //        ModelState.AddModelError("CustomModelError", "Error in sending OTP! Kindly try after some time.");
                //    }

                //    return View(model);
                //}

                //if (!string.IsNullOrEmpty(model.CorporateEmailVerificationCode))
                //{
                //    var userCodeEncode = clsUtils.Encode(model.CorporateEmailVerificationCode);

                //    if (userCodeEncode == model.CorporateEmailVerificationEncoded)
                //    {
                //        long refreeUserId = 0L;

                //        if (!string.IsNullOrEmpty(model.RefereeCode))
                //        {
                //            refreeUserId = BL_WebsiteUser.GetUserIdByReferralCode(model.RefereeCode);

                //            if (refreeUserId == default(long))
                //            {
                //                ModelState.AddModelError("OTPRequired", "Please enter the otp");
                //                ModelState.AddModelError("CustomModelError", "The Referral code you have provided didn't match any account. !");
                //                return View(model);
                //            }
                //        }

                //        var signUpUser = new WebsiteUser
                //        {
                //            Title = model.Title.ToString(),
                //            DisplayName = model.DisplayName,
                //            FirstName = model.FirstName,
                //            LastName = model.LastName,
                //            UserName = model.PhoneNumber,
                //            PhoneNumber = model.PhoneNumber,
                //            PhoneNumberConfirmed = true,
                //            Email = model.Email,
                //            CorporateEmail = model.CorporateCustomerEmail
                //        };

                //        var result = await UserManager.CreateAsync(signUpUser);
                //        if (result.Succeeded)
                //        {
                //            signUpUser = UserManager.FindByEmail(signUpUser.Email);
                //            var referralCode = clsUtils.GenerateReferralCode(signUpUser.Id, codeLength: 8);
                //            signUpUser.sReferralCode = referralCode;

                //            await Task.Run(() => UploadCustomerProfileImage(signUpUser.Id, model.ProfilePicUrl));

                //            signUpUser.ProfileImageUrl = string.Format("customerprofileimages/profile{0}.png", signUpUser.Id);

                //            UserManager.Update(signUpUser);

                //            var resultStatus = BL_WebsiteUser.UpdateCustomerLoyalityPoints(refreeUserId, signUpUser.Id);

                //            result = await UserManager.AddLoginAsync(signUpUser.Id, info.Login);
                //            if (result.Succeeded)
                //            {
                //                await SignInManager.SignInAsync(signUpUser, isPersistent: false, rememberBrowser: false);


                //                return RedirectToAction("Index", "Home");

                //                //ModelState.AddModelError("AllDone", "Please wait...");
                //                //return View(model);
                //            }
                //            else
                //            {
                //                ModelState.AddModelError("CustomModelError", "An unknown error had happen, kindly try after some time.");
                //            }
                //        }
                //        else
                //        {
                //            ModelState.AddModelError("CustomModelError", string.Format("Account already exists with email - {0}, please try with another social login with same id.", signUpUser.Email));
                //        }
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("OTPRequired", "Please enter the otp");
                //        ModelState.AddModelError("CustomModelError", "OTP is incorrect!");
                //    }
                //}
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult ExternalCorporateRoundTrip(ExternalLoginConfirmationViewModel model)
        {
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    //DoSomethingWith(error);
                }
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.CorporateEmailVerificationCode))
                {
                    var userCodeEncode = clsUtils.Encode(model.CorporateEmailVerificationCode);

                    if (userCodeEncode == model.CorporateEmailVerificationEncoded)
                    {
                        ModelState.AddModelError("AllDone", "Please wait...");
                    }
                    else
                    {
                        ModelState.AddModelError("OTPRequired", "Please enter the otp");
                        ModelState.AddModelError("CustomModelError", "OTP is incorrect!");
                    }
                }
            }

            return View(model);
        }


        [NonAction]
        private async Task UploadCustomerProfileImage(long userId, string profileImageUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(profileImageUrl);

                string fileName = "profile" + userId + ".png";

                await Task.Run(() => clsUtils.fnUploadFileINBlobStorage(PropertyConstant.CustomerProfileImages, fileName, data, generateThumbnail: false));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        #endregion Public Methods


        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                var NameRemovedError = Regex.Replace(error, "Name", "");
                ModelState.AddModelError("", NameRemovedError);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(returnUrl);
            // return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task<ExternalLoginInfo> AuthenticationManager_GetExternalLoginInfoAsync_Workaround()
        {
            ExternalLoginInfo loginInfo = null;

            var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (result != null && result.Identity != null)
            {
                var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null)
                {
                    loginInfo = new ExternalLoginInfo()
                    {
                        DefaultUserName = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", ""),
                        Login = new UserLoginInfo(idClaim.Issuer, idClaim.Value)
                    };
                }
            }
            return loginInfo;
        }


        #endregion Helpers


        #region UtilityMethod


        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId > 0)
            {
                var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
            }
            return View("Error");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }



        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        private string GetErrorsFromModelState(List<ModelState> modelStates)
        {
            string message = string.Empty;

            foreach (var state in modelStates)
            {
                if (state.Errors.Count > 0)
                {
                    foreach (var error in state.Errors)
                    {
                        message += error.ErrorMessage + Environment.NewLine;
                    }
                }
            }
            return message;
        }

        #endregion UtilityMethod
    }

}