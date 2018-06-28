using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OneFineRateWeb.Models;
using OneFineRateBLL.WebUserBL;
using OneFineRateBLL.WebUserEntities;
using System.Threading;
using System.IO;
using OneFineRateAppUtil;
using OneFineRateBLL;
using System.Configuration;
using System.Collections.Generic;
using OneFineRateWeb.ViewModels;
using OneFineRateWeb.Handlers;
using System.Text;
using OneFineRateBLL.Entities;

namespace OneFineRateWeb.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        public ManageController()
        {
        }

        public ManageController(WebsiteUserManager userManager)
        {
            UserManager = userManager;
        }

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

        public ActionResult Index(ManageMessageId? message)
        {

            var user = GetUser();
            var allBookings = BL_WebsiteUser.GetBookings(user.Id);
            var model = new IndexViewModel
            {
                HasPassword = user.PasswordHash != null,
                OFRPoints = user.OFRPoints,
                ProfileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + user.ProfileImageUrl
                //PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId<long>()),
                //TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId<long>()),
                //Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<long>()),
                //BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId<long>().ToString())
            };

            List<eCustomerViewedItems> relatedViewItems;

            model.RecentlyViewedItems  = BL_WebsiteUser.GetRecentlyViewedItems(user.Id,out relatedViewItems);
            model.UserMayLikeItems = relatedViewItems;

            return View(model);
        }


        //[HttpGet]
        //public IEnumerable<etblBookingTx> GetCurrentNegotiations(bool? filterActive, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        //{
        //    int TotalCount;
        //    long userId = User.Identity.GetUserId<long>();
        //    IEnumerable<etblBookingTx> query = BL_WebsiteUser.GetCurrentNegotialions(out TotalCount, limitOffset, limitRowCount, userId).ToList();

        //    if (!String.IsNullOrWhiteSpace(orderBy))
        //    {
        //        switch (orderBy.ToLower())
        //        {
        //            case "iBookingId":
        //                if (!desc)
        //                    query = query.OrderBy(p => p.iBookingId);
        //                else
        //                    query = query.OrderByDescending(p => p.iBookingId);
        //                break;
        //            case "sHotelName":
        //                if (!desc)
        //                    query = query.OrderBy(p => p.sHotelName);
        //                else
        //                    query = query.OrderByDescending(p => p.sHotelName);
        //                break;
        //            case "dTotalAmount":
        //                if (!desc)
        //                    query = query.OrderBy(p => p.dTotalAmount);
        //                else
        //                    query = query.OrderByDescending(p => p.dTotalAmount);
        //                break;
        //            case "dTotalNegotiateAmount":
        //                if (!desc)
        //                    query = query.OrderBy(p => p.dTotalNegotiateAmount);
        //                else
        //                    query = query.OrderByDescending(p => p.dTotalNegotiateAmount);
        //                break;
        //            case "dtReservationDate":
        //                if (!desc)
        //                    query = query.OrderBy(p => p.dtReservationDate);
        //                else
        //                    query = query.OrderByDescending(p => p.dtReservationDate);
        //                break;
        //            default:
        //                query = query.OrderBy(p => p.iBookingId);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        query = query.OrderBy(p => p.iBookingId);
        //    }

        //    if (limitOffset.HasValue)
        //    {
        //        query = query.Skip(limitOffset.Value).Take(limitRowCount.Value);
        //    }

        //    return query.ToList();
        //}

    
        [HttpGet]
        public ActionResult MyAccount()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<long>());
            var model = new MyAccountViewModel
            {
                FullName = user.FirstName + " " + user.LastName,
                ProfileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + user.ProfileImageUrl,
                HasPassword = user.PasswordHash != null
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult WishList()
        {
            var userId = User.Identity.GetUserId<long>();
            var allBookings = BL_WebsiteUser.GetBookings(userId);
            return View();
        }

        [HttpGet]
        public ActionResult Booking()
        {
            var userId = User.Identity.GetUserId<long>();

            var model = new CustomerBookingModel();
            model.PendingNegotiations = BL_WebsiteUser.GetPendingNegotialions(userId);
            // if (allBookings != null)
            // {

            //model.FutureBookings = allBookings.Where(x => x.dtCheckIn > DateTime.Today).ToList();
            //model.PastBookings = allBookings.Where(x => x.dtCheckIn < DateTime.Today).ToList();
            //model.PendingNegotiations = allBookings.Where(x => x.cBookingType.ToUpper() == "N" && x.BookingStatus.ToUpper() == "P").ToList();
            //model.UnSuccessfullNegotiations = allBookings.Where(x => x.cBookingType.ToUpper() == "N" && x.BookingStatus.ToUpper() == "R").ToList();
            // }
            return View(model);
        }


        [ChildActionOnly]
        public PartialViewResult CustomerDetails()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<long>());
            var model = new CutomerDetailModel();
            if (user != null)
            {
                model.ProfileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + user.ProfileImageUrl;
                model.Name = user.FirstName + " " + user.LastName;
                model.Phone = user.PhoneNumber;
                model.Email = user.Email;
                model.BirthDate = user.DateOfBirth.ToShortDateString();
            }

            return PartialView("_CustomerDetails", model);
        }

        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId<long>());
            ViewBag.ShowRemoveButton = GetUser().PasswordHash != null || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<long>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        public ActionResult AddPhoneNumber()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<long>(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost]
        public ActionResult RememberBrowser()
        {
            var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId<long>().ToString());
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, rememberBrowserIdentity);
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult ForgetBrowser()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public async Task<ActionResult> EnableTFA()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<long>(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public async Task<ActionResult> DisableTFA()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<long>(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            // This code allows you exercise the flow without actually sending codes
            // For production use please register a SMS provider in IdentityConfig and generate a code here.
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<long>(), phoneNumber);
            ViewBag.Status = "For DEMO purposes only, the current code is " + code;
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<long>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }


        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId<long>(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        public ActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                JsonResult jsonData = null;

                if (ModelState.IsValid)
                {
                    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<long>(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
                        if (user != null)
                        {
                            await SignInAsync(user, isPersistent: false);
                            jsonData = Json(new { status = true, message = "Password changed successfully !" });
                        }
                    }
                    else
                    {
                        string message = string.Empty;

                        foreach (var error in result.Errors)
                        {
                            message += error;
                        }
                        jsonData = Json(new { status = false, message = message });
                    }
                }
                else
                {
                    jsonData = Json(new { status = true, message = "Validation failed ! Please correct the errors and try again." });
                }

                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
            var profileViewModel = new ProfileViewModel();
            profileViewModel.Email = existingUser.Email;
            profileViewModel.PhoneNumber = existingUser.PhoneNumber;

            if (!string.IsNullOrEmpty(existingUser.Title))
                profileViewModel.Title = (TitleType)Enum.Parse(typeof(TitleType), existingUser.Title, true);
            profileViewModel.PinCode = existingUser.PinCode;
            profileViewModel.FirstName = existingUser.FirstName;
            profileViewModel.LastName = existingUser.LastName;
            profileViewModel.DisplayName = existingUser.DisplayName;
            profileViewModel.DateOfBirth = existingUser.DateOfBirth.ToShortDateString();
            if (!string.IsNullOrEmpty(existingUser.MartialStatus))
                profileViewModel.MartialStatus = (MartialStatusType)Enum.Parse(typeof(MartialStatusType), existingUser.MartialStatus, true);
            profileViewModel.AnniversaryDate = existingUser.AnniversaryDate.ToShortDateString();
            profileViewModel.SpouseName = existingUser.SpouseName;
            profileViewModel.ProfileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + existingUser.ProfileImageUrl;
            profileViewModel.CurrentAddressLine1 = existingUser.CurrentAddressLine1;
            profileViewModel.CurrentAddressLine2 = existingUser.CurrentAddressLine2;
            profileViewModel.CurrentAddressLine3 = existingUser.CurrentAddressLine3;
            profileViewModel.CountryId = existingUser.CountryId;
            profileViewModel.StateId = existingUser.StateId;
            profileViewModel.City = existingUser.City;

            return PartialView("_UserProfile", profileViewModel);
        }

        [HttpPost]
        public ActionResult UserProfile(ProfileViewModel profile)
        {
            try
            {
                JsonResult jsonData = null;

                if (ModelState.IsValid)
                {
                    var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
                    if (existingUser != null)
                    {
                        if (profile.ProfileImagePostedFile != null && IsValideImage(profile.ProfileImagePostedFile))
                        {
                            if (!string.IsNullOrEmpty(existingUser.ProfileImageUrl))
                            {
                                string photoUrl = existingUser.ProfileImageUrl;

                                var uniqueFileName = photoUrl.Substring(photoUrl.LastIndexOf('/') + 1);

                                clsUtils.DeleteAzureUploadedFile(PropertyConstant.CustomerProfileImages, uniqueFileName);
                            }
                            using (MemoryStream target = new MemoryStream())
                            {
                                profile.ProfileImagePostedFile.InputStream.CopyTo(target);
                                byte[] data = target.ToArray();

                                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profile.ProfileImagePostedFile.FileName);

                                var currentUploadedFileUrl = clsUtils.fnUploadFileINBlobStorage(PropertyConstant.CustomerProfileImages, uniqueFileName, data, generateThumbnail: false);

                                //System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));

                                existingUser.ProfileImageUrl = PropertyConstant.CustomerProfileImages + "/" + uniqueFileName;

                            }
                        }

                        //existingUser.Email = profile.Email;
                        existingUser.PhoneNumber = profile.PhoneNumber;
                        existingUser.Title = profile.Title.ToString();
                        existingUser.PinCode = profile.PinCode;
                        existingUser.FirstName = profile.FirstName;
                        existingUser.LastName = profile.LastName;
                        existingUser.DisplayName = profile.DisplayName;
                        existingUser.DateOfBirth = DateTime.Parse(profile.DateOfBirth);
                        existingUser.MartialStatus = profile.MartialStatus.ToString();
                        existingUser.AnniversaryDate = DateTime.Parse(profile.AnniversaryDate);
                        existingUser.SpouseName = profile.SpouseName;

                        existingUser.CurrentAddressLine1 = profile.CurrentAddressLine1;
                        existingUser.CurrentAddressLine2 = profile.CurrentAddressLine2;
                        existingUser.CurrentAddressLine3 = profile.CurrentAddressLine3;
                        existingUser.CountryId = profile.CountryId;
                        existingUser.StateId = profile.StateId;
                        existingUser.City = profile.City;

                        UserManager.Update(existingUser);

                        jsonData = Json(new { status = true, message = " Profile updated successfully ! " });
                    }
                    else
                    {
                        jsonData = Json(new { status = true, message = "Opps ! Something went wrong. Please try again. " });
                    }
                }
                else
                {
                    jsonData = Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
                }

                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult UserPreference()
        {
            var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
            var preferenceViewModel = new PreferenceViewModel();
            preferenceViewModel._24_Hour_Front_Desk = existingUser._24_Hour_Front_Desk;
            preferenceViewModel.Airport_Suttle = existingUser.Airport_Suttle;
            preferenceViewModel.Bar = existingUser.Bar;
            preferenceViewModel.Facilities_For_Disabled_Guest = existingUser.Facilities_For_Disabled_Guest;
            preferenceViewModel.Family_Rooms = existingUser.Family_Rooms;
            preferenceViewModel.Fitness_Center = existingUser.Fitness_Center;
            preferenceViewModel.Internet_Services = existingUser.Internet_Services;
            preferenceViewModel.Luggage_Storage = existingUser.Luggage_Storage;
            preferenceViewModel.NewsNotifications = existingUser.NewsNotifications;
            preferenceViewModel.Parking = existingUser.Parking;
            preferenceViewModel.Pets_Allowed = existingUser.Pets_Allowed;

            preferenceViewModel.Pool = existingUser.Pool;
            if (!string.IsNullOrEmpty(existingUser.PreferedStarRating))
                preferenceViewModel.PreferedStarRating = (PreferedStarRatingType)Enum.Parse(typeof(PreferedStarRatingType), existingUser.PreferedStarRating.ToString(), true);
            preferenceViewModel.Restaurant = existingUser.Restaurant;
            if (!string.IsNullOrEmpty(existingUser.SmokingRoom))
                preferenceViewModel.SmokingRoom = (PreferedSmokingRoomType)Enum.Parse(typeof(PreferedSmokingRoomType), existingUser.SmokingRoom, true);
            preferenceViewModel.Spa_And_Wellness_Center = existingUser.Spa_And_Wellness_Center;
            preferenceViewModel.SpecialRequest = existingUser.SpecialRequest;
            return PartialView("_UserPreference", preferenceViewModel);
        }

        [HttpPost]
        public ActionResult UserPreference(PreferenceViewModel preference)
        {
            try
            {
                JsonResult jsonData = null;

                if (ModelState.IsValid)
                {
                    var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
                    if (existingUser != null)
                    {
                        existingUser.SmokingRoom = preference.SmokingRoom.ToString();
                        existingUser.PreferedStarRating = preference.PreferedStarRating.ToString();
                        existingUser.SpecialRequest = preference.SpecialRequest;
                        existingUser.NewsNotifications = preference.NewsNotifications;
                        existingUser.Facilities_For_Disabled_Guest = preference.Facilities_For_Disabled_Guest;
                        existingUser.Family_Rooms = preference.Family_Rooms;
                        existingUser.Fitness_Center = preference.Fitness_Center;
                        existingUser.Internet_Services = preference.Internet_Services;
                        existingUser.Pets_Allowed = preference.Pets_Allowed;
                        existingUser.Luggage_Storage = preference.Luggage_Storage;
                        existingUser.Pool = preference.Pool;
                        existingUser._24_Hour_Front_Desk = preference._24_Hour_Front_Desk;
                        existingUser.Restaurant = preference.Restaurant;
                        existingUser.Spa_And_Wellness_Center = preference.Spa_And_Wellness_Center;
                        existingUser.Bar = preference.Bar;
                        existingUser.Family_Rooms = preference.Family_Rooms;
                        existingUser.Airport_Suttle = preference.Airport_Suttle;
                        existingUser.Parking = preference.Parking;

                        UserManager.Update(existingUser);
                        jsonData = Json(new { status = true, message = " Preferences updated successfully ! " });
                    }
                    else
                    {
                        jsonData = Json(new { status = true, message = "Opps ! Something went wrong. Please try again." });
                    }
                }
                else
                {
                    jsonData = Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
                }

                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult GovernmentApprovedId()
        {
            var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
            var govApprovedViewModel = new GovernMentApprovedIdViewModel();
            govApprovedViewModel.ID_Number = existingUser.ID_Number;
            govApprovedViewModel.UploadedPhotoUrl = ConfigurationManager.AppSettings["BlobUrl"] + existingUser.GovApprovedPhotoIdUrl;
            if (!string.IsNullOrEmpty(existingUser.ID_Type))
                govApprovedViewModel.ID_Type = (Id_Type)Enum.Parse(typeof(Id_Type), existingUser.ID_Type, true);
            govApprovedViewModel.ExpirationDate = existingUser.ExpirationDate.ToShortDateString();
            return PartialView("_GovernmentApprovedId", govApprovedViewModel);
        }

        [HttpPost]
        public ActionResult GovernmentApprovedId(GovernMentApprovedIdViewModel governmentApprovedModel)
        {
            try
            {
                JsonResult jsonData = null;

                if (ModelState.IsValid)
                {
                    var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
                    if (existingUser != null)
                    {
                        if (governmentApprovedModel.IdImagePostedFile != null && IsValideImage(governmentApprovedModel.IdImagePostedFile))
                        {
                            if (!string.IsNullOrEmpty(existingUser.GovApprovedPhotoIdUrl))
                            {
                                string photoUrl = existingUser.GovApprovedPhotoIdUrl;

                                var uniqueFileName = photoUrl.Substring(photoUrl.LastIndexOf('/') + 1);

                                clsUtils.DeleteAzureUploadedFile(PropertyConstant.CustomerProfileImages, uniqueFileName);
                            }
                            using (MemoryStream target = new MemoryStream())
                            {
                                governmentApprovedModel.IdImagePostedFile.InputStream.CopyTo(target);
                                byte[] data = target.ToArray();

                                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(governmentApprovedModel.IdImagePostedFile.FileName);

                                var currentUploadedFileUrl = clsUtils.fnUploadFileINBlobStorage(PropertyConstant.CustomerProfileImages, uniqueFileName, data, generateThumbnail: false);

                                existingUser.GovApprovedPhotoIdUrl = PropertyConstant.CustomerProfileImages + "/" + uniqueFileName;
                            }
                        }
                        existingUser.ID_Number = governmentApprovedModel.ID_Number;
                        existingUser.ID_Type = governmentApprovedModel.ID_Type.ToString();
                        if (governmentApprovedModel.ExpirationDate != null)
                        {
                            existingUser.ExpirationDate = DateTime.Parse(governmentApprovedModel.ExpirationDate);
                        }
                        UserManager.Update(existingUser);
                    }
                    jsonData = Json(new { status = true, message = " Government Approved Details updated successfully ! " });
                }
                else
                {
                    jsonData = Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
                }

                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult SetPassword()
        {
            return PartialView("_SetPassword");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            try
            {
                JsonResult jsonData = null;

                if (ModelState.IsValid)
                {
                    var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<long>(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
                        if (user != null)
                        {
                            await SignInAsync(user, isPersistent: false);
                        }
                        jsonData = Json(new { status = true, message = "Password set successfully ! " });
                    }
                    else
                    {
                        string message = string.Empty;

                        foreach (var error in result.Errors)
                        {
                            message += error;
                        }

                        jsonData = Json(new { status = false, message = message });
                    }
                }
                else
                {
                    jsonData = Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
                }

                return jsonData;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<long>());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId<long>().ToString());
        }


        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId<long>().ToString());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<long>(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        public ActionResult IsValidAnniversaryDate(DateTime? AnniversaryDate, DateTime? DateOfBirth)
        {
            try
            {
                if (DateOfBirth.HasValue && AnniversaryDate.HasValue)
                {
                    if (DateOfBirth.Value > AnniversaryDate.Value)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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

        #region Helpers

        [HttpGet]
        public JsonResult BindStates(int countryId)
        {
            var results = BL_tblStateM.GetAllRecordsById(countryId).Select(x => new { x.iStateId, x.sState });
            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult BindCity(int stateId)
        {
            var results = BL_tblCityM.GetAllRecordsById(stateId).Select(x => new { x.iCityId, x.sCity });
            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);
        }


        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(WebsiteUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private WebsiteUser GetUser()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<long>());
            if (user != null)
            {
                return user;
            }
            return new WebsiteUser();
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<long>());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        private bool IsValideImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentType.Contains("image"))
                {
                    return true;
                }

                string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };

                return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            UpdatePreferencesSuccess,
            UpdateProfileSuccess,
            ApprovedGovernmentIdSuccess,
            Error
        }

        #endregion
    }
}