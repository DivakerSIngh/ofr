#region Usings
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
using FutureSoft.Util;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Web.Routing;

#endregion

namespace OneFineRateWeb.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        #region Initialization

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

        #endregion Initialization

        #region Public Action Methods

        //[OutputCache(Duration = 30, VaryByParam = "*")]
        public async Task<ActionResult> Index()
        {
            TempData["msg"] = null;
            TempData["ERROR"] = null;
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
            var loyalityPoints = await BL_CustomerPointsMap.GetLoyalityPointDetails();
            var customerPointData = BL_CustomerPointsMap.GetCustomerPointData(user.Id);
            var model = new IndexViewModel
            {
                HasPassword = user.PasswordHash != null,
                OFRPoints = customerPointData.TotalRemainingPoints,
                EarnMoney = loyalityPoints != null ? loyalityPoints.iEarnMoney : 0,
                EarnPoint = loyalityPoints != null ? loyalityPoints.iEarnPoints : 0
                //PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId<long>()),
                //TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId<long>()),
                //Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<long>()),
                //BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId<long>().ToString())
            };

            List<eCustomerViewedItems> relatedViewItems;

            model.RecentlyViewedItems = BL_WebsiteUser.GetRecentlyViewedItems(user.Id, out relatedViewItems);
            model.UserMayLikeItems = relatedViewItems;

            string currencySymbol = "₹";
            decimal exchangeRate = 1;

            var recentViews = model.RecentlyViewedItems.Where(x => x.bIsTG == true).ToList();
            var userMayLike = model.UserMayLikeItems.Where(x => x.bIsTG == true).ToList();

            model.RecentlyViewedItems.RemoveAll(x => x.bIsTG == true);
            model.UserMayLikeItems.RemoveAll(x => x.bIsTG == true);

            if (CurrencyCode != "INR")
            {
                currencySymbol = "$";

                var objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                if (objExchange.dRate.HasValue && objExchange.dRate != 0)
                {
                    exchangeRate = objExchange.dRate.Value;
                }
            }

            model.RecentlyViewedItems.AddRange(clsSearchHotel.FetchTodayHotelPrice<eCustomerViewedItems>(recentViews, exchangeRate));
            model.UserMayLikeItems.AddRange(clsSearchHotel.FetchTodayHotelPrice<eCustomerViewedItems>(userMayLike, exchangeRate));


            ViewBag.CurrencySymbol = currencySymbol;

            return View(model);
        }


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
        [AllowAnonymous]
        public ActionResult Booking(string BookingId, string email, string mobile)
        {
            var EmailMobile = email;
            if (string.IsNullOrEmpty(email))
            {
                if (!string.IsNullOrEmpty(mobile))
                    EmailMobile = mobile.Replace("-", "");
            }

            var model = new CustomerBookingModel();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId<long>();
                int pendingNegotiationsCount;
                int futureBookingsCount;
                int pastBookingsCount;
                int unSuccessfullNegotiationsCount;

                model.PendingNegotiations = BL_WebsiteUser.GetPendingNegotialions(out pendingNegotiationsCount, userId, 1, BookingId, EmailMobile, CurrencyCode);
                model.FutureBookings = BL_WebsiteUser.GetFutureBookings(out futureBookingsCount, userId, 1, BookingId, EmailMobile, CurrencyCode);
                model.PastBookings = BL_WebsiteUser.GetPastBookings(out pastBookingsCount, userId, 1, BookingId, EmailMobile, CurrencyCode);
                model.UnSuccessfullNegotiations = BL_WebsiteUser.GetUnSuccessfulNegotiations(out unSuccessfullNegotiationsCount, userId, 1, BookingId, EmailMobile, CurrencyCode);

                model.PendingNegotiationsPageCount = pendingNegotiationsCount;
                model.FutureBookingsPageCount = futureBookingsCount;
                model.PastBookingsPageCount = pastBookingsCount;
                model.UnSuccessfullNegotiationsPageCount = unSuccessfullNegotiationsCount;

                model.BookingId = BookingId;
                model.Email = email;
                model.Mobile = mobile;
            }
            else
            {
                if(string.IsNullOrEmpty(BookingId))
                {
                    TempData["ERROR"] = "Booking not found!";
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToRoute("BookingStatus", new { bookingId = clsUtils.Encode(BookingId), phone = mobile, email = email });
            }

            return View(model);
        }


        public async Task<ActionResult> WishList()
        {
            long userId = User.Identity.GetUserId<long>();
            string currencySymbol = string.Empty;
            decimal exchangeRate = 1;
            var wishList = BL_WebsiteUser.GetCustomerWishList(userId, CurrencyCode, out currencySymbol);

            if (wishList.Count > 0)
            {
                var travelGuruHotels = wishList.Where(x => x.bIsTG == true).ToList();

                if (CurrencyCode != "INR")
                {
                    var objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                    if (objExchange.dRate.HasValue && objExchange.dRate != 0)
                    {
                        exchangeRate = objExchange.dRate.Value;
                    }
                }

                travelGuruHotels = clsSearchHotel.FetchTodayHotelPrice<PropSearchResponseModel>(travelGuruHotels, exchangeRate);
            }

            ViewBag.CurrencySymbol = currencySymbol;
            return View(wishList);
        }


        public ActionResult ReferralCode()
        {
            return View(new ReferralCodeViewModel());
        }

        [HttpPost]
        public ActionResult ReferralCode(ReferralCodeViewModel model, string submitButton)
        {
            if (ModelState.IsValid)
            {
                var referralCode = User.ReferralCode();

                if (submitButton.Equals("email"))
                {

                    string[] emails = new string[10];
                    emails[0] = model.Email1;
                    emails[1] = model.Email2;
                    emails[2] = model.Email3;
                    emails[3] = model.Email4;
                    emails[4] = model.Email5;
                    emails[5] = model.Email6;
                    emails[6] = model.Email7;
                    emails[7] = model.Email8;
                    emails[8] = model.Email9;
                    emails[9] = model.Email10;

                    var emailString = string.Join(",", emails.Where(s => !string.IsNullOrEmpty(s)));

                    MailComponent.SendEmail(emailString, "", "", "Referral Code", "Use My referral code <strong>" + referralCode + "</strong> for booking.", null, null, true, null, null);

                    TempData["msg"] = "Email successfully sent to " + emailString;
                }
                else if (submitButton.Equals("message"))
                {
                    string[] mobileNumbers = new string[10];
                    mobileNumbers[0] = model.Phone1;
                    mobileNumbers[1] = model.Phone2;
                    mobileNumbers[2] = model.Phone3;
                    mobileNumbers[3] = model.Phone4;
                    mobileNumbers[4] = model.Phone5;
                    mobileNumbers[5] = model.Phone6;
                    mobileNumbers[6] = model.Phone7;
                    mobileNumbers[7] = model.Phone8;
                    mobileNumbers[8] = model.Phone9;
                    mobileNumbers[9] = model.Phone10;

                    var mobileNumberstring = string.Join(",", mobileNumbers.Where(s => !string.IsNullOrEmpty(s)));

                    clsUtils.sendSMS(mobileNumberstring, "Use my referral code " + referralCode + " for booking and get discount.");

                    TempData["msg"] = "SMS successfully sent to " + mobileNumberstring;

                    return RedirectToAction("ReferralCode");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult RewardPoints()
        {
            var customerPointData = BL_CustomerPointsMap.GetCustomerPointData(User.Identity.GetUserId<long>());
            return View(customerPointData);
        }

        [HttpPost]
        public JsonResult LoadCurrentNegotationData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            int PageNo = skip / pageSize + 1;

            var userId = User.Identity.GetUserId<long>();

            var currentNegotiations = BL_WebsiteUser.GetCurrentNegotialions(out recordsTotal, userId, Convert.ToInt32(PageNo), "", "", CurrencyCode);

            var data = currentNegotiations.ToList();//.Skip(skip).Take(pageSize)

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult LoadPastSavingData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            int PageNo = skip / pageSize + 1;

            var userId = User.Identity.GetUserId<long>();

            var pastSavings = BL_WebsiteUser.GetPastSavings(out recordsTotal, userId, Convert.ToInt32(PageNo), CurrencyCode);

            //var data = pastSavings.ToList();//.Skip(skip).Take(pageSize)

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = pastSavings }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult LoadRewardPointData()
        {
            var startDateRewardPoint = Request.Form.GetValues("startDateRewardPoint").FirstOrDefault();
            var endDateRewardPoint = Request.Form.GetValues("endDateRewardPoint").FirstOrDefault();
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var list = BL_tblCustomerPointsHistoryTx.GetAllRecord(User.Identity.GetUserId<long>(), startDateRewardPoint, endDateRewardPoint);

            recordsTotal = list.Count;
            var data = list.Skip(skip).Take(pageSize).ToList();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "ActivityDate":
                        {
                            if (sortColumnDir == "desc")
                            {
                               data = data.OrderBy(x => x.dtAction).ToList();
                            }

                            else
                            {
                                data = data.OrderByDescending(x => x.dtAction).ToList();
                            }
                            break;
                        }
                    case "TransactionType":
                        {
                            if (sortColumnDir == "desc")
                            {
                                data = data.OrderByDescending(x => x.Type).ToList();
                            }

                            else
                            {
                                list = list.OrderBy(x => x.Type).ToList();
                            }
                            break;
                        }
                    case "Points":
                        {
                            if (sortColumnDir == "desc")
                            {
                                data = data.OrderByDescending(x => x.iTotalPoints).ToList();
                            }

                            else
                            {
                                data = data.OrderBy(x => x.iTotalPoints).ToList();
                            }
                            break;
                        }
                    case "TransactionDate":
                        {
                            if (sortColumnDir == "desc")
                            {
                                data = data.OrderByDescending(x => x.dtDate).ToList();
                            }

                            else
                            {
                                data = data.OrderBy(x => x.dtDate).ToList();
                            }
                            break;
                        }
                }
            }

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Partial View Methos

        #region Bookings

        [HttpGet]
        public PartialViewResult GetPendingNegotiations(int pageNumber, string BookingId, string EmailMobile)
        {
            var userId = User.Identity.GetUserId<long>();
            int totalPageCount = 0;
            var pendingNegotiations = BL_WebsiteUser.GetPendingNegotialions(out totalPageCount, userId, pageNumber, BookingId, EmailMobile, CurrencyCode);
            return PartialView("_PendingNegotiations", pendingNegotiations);
        }

        [HttpGet]
        public PartialViewResult GetFutureBookings(int pageNumber, string BookingId, string EmailMobile)
        {
            var userId = User.Identity.GetUserId<long>();
            int totalPageCount = 0;
            var futureBookings = BL_WebsiteUser.GetFutureBookings(out totalPageCount, userId, pageNumber, BookingId, EmailMobile, CurrencyCode);

            return PartialView("_FutureBookings", futureBookings);
        }

        [HttpGet]
        public PartialViewResult GetPastBookings(int pageNumber, bool? isDashboard, string BookingId, string EmailMobile)
        {
            var userId = User.Identity.GetUserId<long>();
            int totalPageCount = 0;
            var pastSavings = BL_WebsiteUser.GetPastBookings(out totalPageCount, userId, pageNumber, BookingId, EmailMobile, CurrencyCode);
            if (isDashboard.HasValue && isDashboard.Value)
            {
                this.ViewData.Add(new KeyValuePair<string, object>("IsCalledFromDashboard", true));
            }
            else
            {
                this.ViewData.Add(new KeyValuePair<string, object>("IsCalledFromDashboard", false));
            }
            return PartialView("_PastBookings", pastSavings);
        }

        [HttpGet]
        public PartialViewResult GetUnSuccessfulNegotiations(int pageNumber, string BookingId, string EmailMobile)
        {
            var userId = User.Identity.GetUserId<long>();
            int totalPageCount = 0;
            var unSuccessfulNegotiations = BL_WebsiteUser.GetUnSuccessfulNegotiations(out totalPageCount, userId, pageNumber, BookingId, EmailMobile, CurrencyCode);
            return PartialView("_UnSuccessfulNegotiations", unSuccessfulNegotiations);
        }

        #endregion Bookings

        #region MyAccount

        public ActionResult UserProfile()
        {
            var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
            var profileViewModel = new ProfileViewModel();
            profileViewModel.Email = existingUser.Email;
            profileViewModel.PhoneNumber = existingUser.PhoneNumber;

            //if (!string.IsNullOrEmpty(existingUser.Title))
            //    profileViewModel.Title = (TitleType)Enum.Parse(typeof(TitleType), existingUser.Title, true);
            profileViewModel.Title = existingUser.Title;
            profileViewModel.PinCode = existingUser.PinCode;
            profileViewModel.FirstName = existingUser.FirstName;
            profileViewModel.LastName = existingUser.LastName;
            profileViewModel.DisplayName = string.IsNullOrEmpty(existingUser.DisplayName) ? existingUser.FirstName + " " + existingUser.LastName : existingUser.DisplayName;

            if (existingUser.DateOfBirth.HasValue)
                profileViewModel.DateOfBirth = existingUser.DateOfBirth.Value.ToShortDateString();
            if (!string.IsNullOrEmpty(existingUser.MartialStatus))
                profileViewModel.MartialStatus = (MartialStatusType)Enum.Parse(typeof(MartialStatusType), existingUser.MartialStatus, true);

            if (existingUser.AnniversaryDate.HasValue)
                profileViewModel.AnniversaryDate = existingUser.AnniversaryDate.Value.ToShortDateString();
            profileViewModel.SpouseName = existingUser.SpouseName;
            profileViewModel.ProfileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + existingUser.ProfileImageUrl;
            profileViewModel.CurrentAddressLine1 = existingUser.CurrentAddressLine1;
            profileViewModel.CurrentAddressLine2 = existingUser.CurrentAddressLine2;
            profileViewModel.CurrentAddressLine3 = existingUser.CurrentAddressLine3;
            profileViewModel.CountryId = existingUser.CountryId;
            profileViewModel.StateId = existingUser.StateId;
            profileViewModel.City = existingUser.City;
            profileViewModel.NewsNotifications = existingUser.NewsNotifications;
            profileViewModel.CorporateEmail = existingUser.CorporateEmail;

            profileViewModel.PhoneNumberConfirmed = existingUser.PhoneNumberConfirmed;
            profileViewModel.EmailConfirmed = existingUser.EmailConfirmed;
            profileViewModel.CorporateEmailConfirmed = existingUser.CorporateEmailConfirmed;

            profileViewModel.GstnEntityType = existingUser.GstnEntityType;
            profileViewModel.GstnEntityName = existingUser.GstnEntityName;
            profileViewModel.GstnNumber = existingUser.GstnNumber;

            var countryCodeList = OneFineRateBLL.BL_Country.GetAllCountryPhoneCodes();
            profileViewModel.CountryCodePhoneList = countryCodeList;
           
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
                        if (profile.PhoneNumber != existingUser.PhoneNumber)
                        {
                            var isPhoneNumberExists = BL_WebsiteUser.IsPhoneNumberOrUserNameExists(profile.PhoneNumber);

                            if (isPhoneNumberExists)
                            {
                                return Json(new { status = false, message = "The Phone number " + profile.PhoneNumber + " is already exits for another user" });
                            }

                            return Json(new { status = false, phoneVerify = true, message = " You have updated your Phone number , We have sent OTP to " + profile.PhoneNumber + " " });
                        }

                        if (profile.CorporateEmail != existingUser.CorporateEmail)
                        {
                            var corporateEmailExistsForAnotherUser = BL_WebsiteUser.IsAnyEmailExistsV2(profile.CorporateEmail, existingUser.Id);

                            if (corporateEmailExistsForAnotherUser)
                            {
                                return Json(new { status = false, message = "The Corporate Email " + profile.CorporateEmail + " is already associated with another account." });
                            }
                        }

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
                        existingUser.UserName = profile.PhoneNumber;
                        existingUser.Title = profile.Title; // == TitleType.Mr ? "Mr." : "Ms.";

                        existingUser.PinCode = profile.PinCode;
                        existingUser.FirstName = profile.FirstName;
                        existingUser.LastName = profile.LastName;
                        existingUser.DisplayName = profile.DisplayName;

                        if (!String.IsNullOrEmpty(profile.DateOfBirth))
                            existingUser.DateOfBirth = DateTime.Parse(profile.DateOfBirth);
                        else existingUser.DateOfBirth = null;
                        existingUser.MartialStatus = profile.MartialStatus.ToString();

                        if (!String.IsNullOrEmpty(profile.AnniversaryDate))
                            existingUser.AnniversaryDate = DateTime.Parse(profile.AnniversaryDate);
                        else existingUser.AnniversaryDate = null;
                        existingUser.SpouseName = profile.SpouseName;

                        existingUser.CurrentAddressLine1 = profile.CurrentAddressLine1;
                        existingUser.CurrentAddressLine2 = profile.CurrentAddressLine2;
                        existingUser.CurrentAddressLine3 = profile.CurrentAddressLine3;
                        existingUser.CountryId = profile.CountryId;
                        existingUser.StateId = profile.StateId;
                        existingUser.City = profile.City;
                        existingUser.NewsNotifications = profile.NewsNotifications;
                        existingUser.sCountryPhoneCode = profile.sCountryPhoneCode;
                        existingUser.CorporateEmail = profile.CorporateEmail;
                        existingUser.GstnEntityName = profile.GstnEntityName;
                        existingUser.GstnNumber = profile.GstnNumber;
                        existingUser.GstnEntityType = profile.GstnEntityType;

                        UserManager.Update(existingUser);

                        jsonData = Json(new { status = true, profileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + existingUser.ProfileImageUrl, message = " Profile updated successfully ! " });
                    }
                    else
                    {
                        jsonData = Json(new { status = false, message = "Opps ! Something went wrong. Please try again. " });
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


        public ActionResult UserPreference()
        {
            var existingUser = UserManager.FindById(User.Identity.GetUserId<long>());
            var preferenceViewModel = new PreferenceViewModel();
            preferenceViewModel.Front_Desk_24_Hour = existingUser.Front_Desk_24_Hour;
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
                        existingUser.Front_Desk_24_Hour = preference.Front_Desk_24_Hour;
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
            {
                govApprovedViewModel.ID_Type = (Id_Type)Enum.Parse(typeof(Id_Type), existingUser.ID_Type, true);
            }
            if (existingUser.ExpirationDate != DateTime.MinValue)
            {
                govApprovedViewModel.ExpirationDate = existingUser.ExpirationDate.ToShortDateString();
            }

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
                        jsonData = Json(new { status = false, message = "Incorrect old password." });
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
            //ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return PartialView("_ManageLogins", new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpGet]
        public ActionResult RemoveLogin()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<long>());
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId<long>());
            ViewBag.ShowRemoveButton = user.PasswordHash != null || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<long>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<long>());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
            }
            else
            {
            }
            return RedirectToAction("MyAccount");
        }

        #endregion MyAccount

        [HttpGet]
        public ActionResult GetHotelSearchData(string hotelSearchTxt)
        {
            var hotelList = BL_tblPropertyM.GetAllPropertyListExTG(hotelSearchTxt);

            return Json(new { hotelList }, JsonRequestBehavior.AllowGet);
        }

        #endregion Partial View Methos

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

        [HttpGet]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<long>(), phoneNumber);

            string message = code + " is your One Time Mobile verification code for OneFineRate";
            clsUtils.sendSMS(phoneNumber, message);

            return PartialView("_VerifyPhoneNumber", new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }
        [HttpGet]
        public async Task<ActionResult> VerifyEmail(string email)
        {
            //var code = await UserManager.GenerateEmailConfirmationTokenAsync(User.Identity.GetUserId<long>());
            var code = clsUtils.GetVerificationCode();
            string message = code + " is your One Time Email verification code for OneFineRate";
            MailComponent.SendEmail(email, "", "", "Email Verification OTP", message, null, null, true, null, null);
            return PartialView("_VerifyEmail", new VerifyEmailViewModel { EmailId = email, HiddenCode = clsUtils.Encode(code.ToString()) });
        }

        [HttpGet]
        public async Task<ActionResult> ResendVerificationCode(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<long>(), phoneNumber);
            string message = code + " is your One Time Mobile verification code for OneFineRate";
            clsUtils.sendSMS(phoneNumber, message);

            return Json(new { message = "An OTP sent to " + phoneNumber, code = code }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> ResendEmailVerificationCode(string email)
        {
            //var code = await UserManager.GenerateEmailConfirmationTokenAsync(User.Identity.GetUserId<long>());
            var code = clsUtils.GetVerificationCode();
            string message = code + " is your One Time Email verification code for OneFineRate";
            MailComponent.SendEmail(email, "", "", "Email verification OTP", message, null, null, true, null, null);
            return Json(new { message = "An OTP sent to " + email, hiddenCode = clsUtils.Encode(code.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
            }

            var code = model.Code;
            if (code != clsUtils.Decode(model.HiddenCode))
            {
                ModelState.AddModelError("", "OTP is not valid !");
                return Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
            }

            //   var result = await UserManager.ConfirmEmailAsync(User.Identity.GetUserId<long>(), code);
            // var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<long>(), model.PhoneNumber, model.Code);
            var result = BL_Users.UpdateEmailVerified(User.Identity.GetUserId<long>());
            if (result)
            {
                return Json(new { status = true, message = "Email Address updated Successfully !" });
            }

            ModelState.AddModelError("", "OTP is not valid !");
            return Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
            }
            //var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<long>(), model.PhoneNumber);
            var code = model.Code;

            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<long>(), model.PhoneNumber, code);
            // var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<long>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                return Json(new { status = true, message = "Phone number updated Successfully !" });
            }

            ModelState.AddModelError("", "OTP is not valid !");
            return Json(new { status = false, message = GetErrorsFromModelState(ModelState.Values.ToList()) });
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
            if (!result.Succeeded)
            {
                TempData["ERROR"] = "Sorry! Cant't Associate Your Social Login.";
            }
            else
            {
                TempData["msg"] = "Your Social login associated successfully !";
            }

            return RedirectToAction("MyAccount");
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