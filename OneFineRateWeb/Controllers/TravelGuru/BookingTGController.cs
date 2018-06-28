using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using System.Text;
using OneFineRateAppUtil;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OneFineRateWeb.App_Helper;
using FutureSoft.Util;
using OneFineRateWeb.Handlers;
using System.Configuration;
using System.Globalization;

namespace OneFineRateWeb.Controllers.TravelGuru
{
    public class BookingTGController : BaseController
    {
        #region Public Methods

        public ActionResult Index()
        {
            return View();
        }

        //[OutputCache(Duration = 30, VaryByParam = "iVendorId;sRoomId;sCheckIn;sCheckOut;sRatePlanCode;sRoomData", VaryByCustom = "User")] // Cached for 30 second
        /// <summary>
        /// After selection of the Room of the travel guru 
        /// </summary>
        /// <param name="iVendorId"></param>
        /// <param name="sRoomId"></param>
        /// <param name="sCheckIn"></param>
        /// <param name="sCheckOut"></param>
        /// <param name="sRatePlanCode"></param>
        /// <param name="sRoomData"></param>
        /// <returns></returns>
        public ActionResult Review(string iVendorId, string sRoomId, string sCheckIn, string sCheckOut, string sRatePlanCode, string sRoomData)
        {
            try
            {
                ViewBag.HeaderBarData = "Preview";

                PropDetailsM propDetail = BL_PropDetailsTG.GetPropertyFacility(iVendorId);

                DateTime dtCheckIn = DateTime.ParseExact(sCheckIn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dtCheckOut = DateTime.ParseExact(sCheckOut, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                #region RoomOccupancySearch

                DataTable dtTblRoomOccupancySearch = new DataTable();
                dtTblRoomOccupancySearch.Columns.AddRange(new DataColumn[3]
                {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                });

                DataTable dtTblChildrenAgeSearch = new DataTable();
                dtTblChildrenAgeSearch.Columns.AddRange(new DataColumn[2]
                {
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))
                });

                var roomDataResult = new List<RoomData>();

                if (!string.IsNullOrEmpty(sRoomData))
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);

                    foreach (var room in roomDataResult)
                    {
                        DataRow roomOccupancy = dtTblRoomOccupancySearch.NewRow();
                        roomOccupancy["ID"] = room.room;
                        roomOccupancy["iAdults"] = room.adult;
                        roomOccupancy["children"] = room.child;
                        dtTblRoomOccupancySearch.Rows.Add(roomOccupancy);

                        foreach (var child in room.ChildAge)
                        {
                            DataRow dtChildrenAge = dtTblChildrenAgeSearch.NewRow();
                            dtChildrenAge["ID"] = room.room;
                            if (child.Age == "<1")
                            {
                                dtChildrenAge["Age"] = "0";
                                child.Age = "0";
                            }
                            else
                            {
                                dtChildrenAge["Age"] = child.Age;
                            }
                            dtChildrenAge["Age"] = child.Age == "<1" ? "0" : child.Age;

                            dtTblChildrenAgeSearch.Rows.Add(dtChildrenAge);
                        }
                    }
                }

                #endregion

                #region Hotel Details

                var TG_Hotel_With_Rooms = clsSearchHotel.FetchHotelsDetailsByVendorId(
                                   false,
                                   iVendorId, dtCheckIn.ToString("yyyy-MM-dd"),
                                   dtCheckOut.ToString("yyyy-MM-dd"),
                                   dtTblRoomOccupancySearch,
                                   dtTblChildrenAgeSearch,
                                   roomDataResult
                                   );

                if (TG_Hotel_With_Rooms != null)
                {

                    var ofrServiceCharge = BL_PropDetailsTG.GetOfrServiceCharge(dtCheckIn, dtCheckOut);

                    propDetail.TG_Hotel = TG_Hotel_With_Rooms;

                    propDetail.iVendorId = iVendorId;
                    propDetail.sRoomId = sRoomId;
                    propDetail.sRatePlanCode = sRatePlanCode;
                    propDetail.scheckOut = sCheckOut;
                    propDetail.scheckIn = sCheckIn;
                    propDetail.sRoomData = sRoomData;
                    propDetail.iStarCategory = Convert.ToInt16(TG_Hotel_With_Rooms.Rating);

                    //TO DO
                    //Its Giving wrong star category '2', that's why manually assigning star as verified from travelguru website
                    if (iVendorId == "00006584")
                    {
                        propDetail.iStarCategory = 3;
                    }
                    propDetail.dtCheckIn = dtCheckIn;
                    propDetail.dtCheckOut = dtCheckOut;
                    propDetail.ServiceChargeTG = ofrServiceCharge;

                    propDetail.sCheckInHH = propDetail.TG_Hotel.CheckInTime;
                    propDetail.sCheckoutHH = propDetail.TG_Hotel.CheckOutTime;

                    var roomDetail = propDetail.TG_Hotel.RoomDetails.Where(x => x.RoomId == propDetail.sRoomId).FirstOrDefault();

                    if (roomDetail != null)
                    {
                        roomDetail.RoomAmenities = BL_PropDetailsTG.GetRoomAmenities(iVendorId, sRoomId);

                        if (roomDetail.RoomImages != null)
                        {
                            if (roomDetail.RoomImages.Count > 1)
                            {
                                propDetail.TG_Hotel.RoomImageUrl = roomDetail.RoomImages[1];

                            }
                            else
                            {
                                propDetail.TG_Hotel.RoomImageUrl = roomDetail.RoomImages.FirstOrDefault();
                            }
                        }


                        var ratePlan = roomDetail.RatePlans.Where(x => x.RatePlanCode == propDetail.sRatePlanCode).FirstOrDefault();

                        if (ratePlan != null)
                        {
                            propDetail.sRoomId = roomDetail.RoomId;
                            propDetail.sRatePlanCode = ratePlan.RatePlanCode;
                            propDetail.iVendorId = roomDetail.VendorId;
                            propDetail.sRoomName = roomDetail.RoomName;
                            propDetail.dSummaryRoomRate = ratePlan.TotalRoomRate;
                            propDetail.dSummaryTaxes = ratePlan.TotalTax;
                            propDetail.dSummaryExtraBedCharges = ratePlan.TotalExtraBedCharge;
                        }

                        //TO DO
                        //Most of the cases we dont't have valid room image url from Travelgur Dump Scheduler, So Manually updating same to the database
                        Task.Run(() =>
                        {
                            BL_PropDetailsTG.AddUpdateRoomDetails(propDetail.iVendorId, propDetail.sRoomId, int.Parse(roomDetail.MaxAdult), int.Parse(roomDetail.MaxChild), propDetail.TG_Hotel.RoomImageUrl);

                        });

                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                        propDetail.sUserTitle = user.Title;
                        propDetail.sUserFirstName = user.FirstName;
                        propDetail.sUserLastName = user.LastName;
                        propDetail.sUserEmail = user.Email;
                        propDetail.sUserMobileNo = user.PhoneNumber;
                        propDetail.sCountryPhoneCode = user.sCountryPhoneCode;
                    }

                    ViewBag.dExchangeRate = 1;

                    if (CurrencyCode != "INR")
                    {
                        etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);

                        if (objExchange.dRate.HasValue)
                        {
                            ViewBag.dExchangeRate = 1/objExchange.dRate.Value;
                        }
                    }

                    propDetail.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);
                }

                #endregion Hotel Details

                ViewBag.Referral = Request.UrlReferrer;
                TempData["propDetailsTG"] = propDetail;
                return View(propDetail);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ChargeCard(FormCollection collection)
        {
            var propDetail = new PropDetailsM();

            var mobile = collection["Mobile"];
            var otpCode = collection["OTPCode"];
            var verificationCode = collection["VerificationCode"];
            string title = collection["Title"];
            string firstName = collection["FirstName"];
            string lastName = collection["LastName"];
            string emailID = collection["EmailID"];
            string counryPhoneCode = collection["sCountryPhoneCode"];

            try
            {
                if (TempData["propDetailsTG"] != null)
                {
                    propDetail = TempData["propDetailsTG"] as PropDetailsM;
                    TempData.Keep();
                }

                propDetail.Currency = "INR";

                if (User.Identity.IsAuthenticated)
                {
                    var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                    propDetail.sUserTitle = user.Title;
                    propDetail.sUserFirstName = user.FirstName;
                    propDetail.sUserLastName = user.LastName;
                    propDetail.sUserEmail = user.Email;
                    propDetail.iUserId = user.Id;

                    if (user.PhoneNumber == mobile)
                    {
                        return ProvisionalBooking(propDetail);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(otpCode))
                        {
                            if (DecodeVC(verificationCode) == otpCode)
                            {
                                propDetail.sUserMobileNo = mobile;

                                return ProvisionalBooking(propDetail);
                            }
                            else
                            {
                                return Json(new { status = false, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            string VerificationCode = ResendVerification(mobile);
                            propDetail.sVerificationCode = VerificationCode;
                        }
                    }
                }
                else
                {
                    propDetail.sUserTitle = title;
                    propDetail.sUserFirstName = firstName;
                    propDetail.sUserLastName = lastName;
                    propDetail.sUserEmail = emailID;
                    propDetail.sUserMobileNo = mobile;
                    propDetail.sCountryPhoneCode = counryPhoneCode;

                    if (!string.IsNullOrEmpty(otpCode))
                    {
                        if (DecodeVC(verificationCode) == otpCode)
                        {
                            int i = BL_NegotiationBooking.AddRecord(propDetail);
                            if (i > 0)
                            {
                                propDetail.iGuestId = i;

                                return ProvisionalBooking(propDetail);
                            }
                            else
                            {
                                return Json(new { status = false, msg = "An error occured, kindly try after some time." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { status = false, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string VerificationCode = ResendVerification(mobile);
                        if (VerificationCode == "error")
                        {
                            return Json(new { status = false, msg = "An error occured, kindly try after some time." }, JsonRequestBehavior.AllowGet);
                        }

                        propDetail.sVerificationCode = VerificationCode;
                    }

                }
            }
            catch (Exception)
            {
                return Json(new { status = false, msg = "An error occured, kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = true, verificationRequired = true, verificationCode = propDetail.sVerificationCode }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReSendOTPCode(string Mobile, string VC)
        {
            var encodedCode = "";
            try
            {
                int phoneVerificationCode;
                if (VC != string.Empty)
                {
                    phoneVerificationCode = Convert.ToInt32(DecodeVC(VC));
                }
                else
                {
                    phoneVerificationCode = clsUtils.GetVerificationCode();
                }

                encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());

                string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                clsUtils.sendSMS(Mobile, message);

                //var stronglObject = new PropDetailsM();
                //if (TempData["propDetails"] != null) { TempData.Keep(); stronglObject = TempData["propDetails"] as PropDetailsM; }

            }
            catch (Exception)
            {
                return Json(new { st = "0" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { st = "1", VC = encodedCode }, JsonRequestBehavior.AllowGet);
        }

        // Called from Charge Card Internally
        [ChildActionOnly]
        public ActionResult ProvisionalBooking(PropDetailsM propDetail)
        {

            //To Generate OFR booikng Detail

            var bookingId = SaveAndGetBookingId(propDetail);
            propDetail.objBooking.iBookingId = bookingId;

            var requestModel = InitializeProvisionalBooking(propDetail);

            //To Update Travel guru about the booking 
            var response = TGBookingManager.ProvisionalBooking(requestModel);

            if (response.IsSuccedded)
            {
                propDetail.objBooking.sProvisionalBookingIdTG = response.UniqueId;
                propDetail.objBooking.sProvisionalTrxnTypeTG = response.UniqueIdType;

                //TODO
                propDetail.objBooking.BookingStatus = "PP";
                propDetail.objBooking.dtActionDate = DateTime.Now;

                etblBookingTrakerTx dbBookingTracker = new etblBookingTrakerTx();
                dbBookingTracker.iBookingId = bookingId;
                dbBookingTracker.BookingStatus = "PP";
                dbBookingTracker.dtActionDate = DateTime.Now;

                int updateResult = BL_Booking.UpdateBookingStatus(propDetail.objBooking, dbBookingTracker);

                if (updateResult == 1)
                {
                    return Json(new { status = true, bookingId = bookingId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                if (response.ErrorCode == "081")
                {
                    response.ErrorMessage = "Sorry! The room you have selected is not available,Please try modifying search";
                }
                else if (response.ErrorCode == "030")
                {
                    response.ErrorMessage = "Sorry! We are unable to process the booking, Please try after some time!";
                }
                else
                {
                    response.ErrorMessage = "Sorry! We are unable to process the booking, Please try after some time!";
                }

                Task.Run(() =>
                {
                    etblBookingTx dbBooking = BL_Booking.GetBooking(bookingId);
                    dbBooking.BookingStatus = "TX";
                    dbBooking.dtActionDate = DateTime.Now;

                    etblBookingTrakerTx dbBookingTracker = new etblBookingTrakerTx();
                    dbBookingTracker.iBookingId = bookingId;
                    dbBookingTracker.BookingStatus = "TX";
                    dbBookingTracker.dtActionDate = DateTime.Now;

                    BL_Booking.UpdateBookingStatus(dbBooking, dbBookingTracker);
                });
            }

            return Json(new { status = false, msg = response.ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        // Automatically Called after successful payment
        public ActionResult FinalBooking(etblBookingTx booking)
        {
            try
            {
                var requestModel = InitializeFinalBooking(booking);

                //Final Booking after Provisional and Successful payment 
                var response = TGBookingManager.FinalBooking(requestModel);

                if (response.IsSuccedded)
                {
                    booking.sFinalBookingIdTG = response.UniqueId;
                    booking.sFinalTrxnTypeTG = response.UniqueIdType;
                    var status = BL_Booking.UpdateBooking(booking);
                    if (status == 1)
                    {
                        return RedirectToAction("GuestInformation", "BookingTG", new { bookingId = clsUtils.Encode(booking.iBookingId.ToString()) });
                    }
                }
                else
                {
                    return Json(new { status = false, msg = response.ErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

            }

            return Json(new { status = false, msg = "An Error occurred !" }, JsonRequestBehavior.AllowGet);
        }

        // Called from/After Final Booking
        public ActionResult GuestInformation(string bookingId)
        {
            ViewBag.HeaderBarData = "Guest Information";
            int BookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails bookingDetails = new BookingGuestDetails();
            bookingDetails = BL_PropDetails.GetBookingDetailsForGuestsRoomsInfo(BookingId);

            var roomDetails = BL_PropDetailsTG.GetRoomOccupancyDetails(bookingDetails.iVendorId, bookingDetails.lsttblBookDetails.FirstOrDefault().iRoomId);

            bookingDetails.sRoomImageUrl = roomDetails != null ? roomDetails.sDefaultImage : bookingDetails.sMainImgUrl;

            bookingDetails.lstetblHotelFacilities = BL_PropDetailsTG.GetFirstFourHotelFacilities(long.Parse(bookingDetails.iPropId));
            ViewBag.dExchangeRate = 1;

            if (CurrencyCode != "INR")
            {
                etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                if (objExchange.dRate.HasValue)
                {
                    ViewBag.dExchangeRate = 1/objExchange.dRate.Value;
                }
            }

            bookingDetails.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            bookingDetails.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);            

            return View(bookingDetails);
        }

        [HttpPost]
        public ActionResult GuestInformation(BookingGuestDetails obj)
        {
            int result;
            try
            {
                etblBookingTrakerTx objTrck = new etblBookingTrakerTx();
                List<etblBookingGuestMap> lst = new List<etblBookingGuestMap>();
                List<etblBookingDetailsTx> lsttblBookDetails = new List<etblBookingDetailsTx>();
                string guestEmailIds = "";
                List<string> emailIdList = new List<string>();
                if (obj.GuestData != null)
                {
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(obj.GuestData.Replace("\\", "\""));
                    if (jArray != null)
                    {

                        foreach (var item in jArray)
                        {
                            lst.Add(new etblBookingGuestMap()
                            {
                                sGuestName = Convert.ToString(item["gname"]),
                                sGuestEmail = Convert.ToString(item["gemail"]),
                                sGuestMobile = Convert.ToString(item["gphone"]),
                                sBedPreference = Convert.ToString(item["ptype"]),
                                sCountryPhoneCode = Convert.ToString(obj.sCountryPhoneCode),
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                dtActionDate = DateTime.Now
                            });

                            emailIdList.Add(item["gemail"].ToString());
                        }
                    }
                }

                guestEmailIds = string.Join(",", emailIdList.Distinct());


                obj.objBooking.dtActionDate = DateTime.Now;

                objTrck.BookingStatus = "PC";
                objTrck.dtActionDate = DateTime.Now;
                objTrck.iBookingId = obj.objBooking.iBookingId;

                lsttblBookDetails = obj.lsttblBookDetails;

                result = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);
                if (result > 0)
                {
                    var customerModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(obj.objBooking.iBookingId));

                    customerModel.IsTG = true;

                    //Send SMS
                    string confirmationLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/BookingTG/Confirmation?bookingId=" + clsUtils.Encode(customerModel.BookingId);
                    string shortConfirmationLink = clsUtils.Shorten(confirmationLink);

                    string messageRevenueManager = "New Booking! Your Booking no. is  " + customerModel.BookingId + " , " + customerModel.CheckIn + " to " + customerModel.ChekOut + " for " + customerModel.NoOfRooms + " rooms." +
                        " A detailed confirmation has been sent to your registered email address. You can also review it on " + shortConfirmationLink;

                    string messageUser = messageRevenueManager + ". You save more on One Fine Rate. Tell your friends about OFR and save even more in your next transaction with us.";

                    // get from propertyEdit page 
                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(customerModel.PropId);

                    string sPrimaryContactEmail;
                    string sConfirmationContactEmail;
                    string sRevenueManagerEmail;
                    string sRevenueManagerContact;

                    pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                    pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

                    string hotelEmails = sRevenueManagerEmail + "," + sConfirmationContactEmail + "," + sPrimaryContactEmail;

                    Task.Run(() => clsUtils.sendSMS(customerModel.MobileOFR, messageUser));
                    //Task.Run(() => clsUtils.sendSMS(sRevenueManagerContact, messageRevenueManager));
                    customerModel.Symbol = "₹";
                    customerModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerModel);

                    Task.Run(() => MailComponent.SendEmail(customerModel.EmailOFR, "", "", "OneFineRate- New Booking! Confirmation No : " + obj.objBooking.iBookingId, html_Customer, null, null, true, null, null));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("ShareBooking", new { bookingId = clsUtils.Encode(obj.objBooking.iBookingId.ToString()) });
        }

        public ActionResult Confirmation(string bookingId)
        {
            ViewBag.HeaderBarData = "Confirmation";

            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var bookingModel = BL_Booking.GetBookingModifyDetails(bookId);

            var defaultRoomDetail = bookingModel.BookingRoomDetails.FirstOrDefault();

            var roomAmenities = BL_PropDetailsTG.GetRoomAmenities(bookingModel.sVendorId, defaultRoomDetail.RoomId);

            var roomDetails = BL_PropDetailsTG.GetRoomOccupancyDetails(bookingModel.sVendorId, defaultRoomDetail.RoomId);

            if (roomDetails != null)
            {
                defaultRoomDetail.MaxChildOccupancy = roomDetails.MaxChildOccupancy;
                defaultRoomDetail.MaxAdultOccupancy = roomDetails.MaxAdultOccupancy;
                defaultRoomDetail.sRoomImageUrl = roomDetails.sDefaultImage;
            }
            else
            {
                defaultRoomDetail.sRoomImageUrl = bookingModel.sImgUrl;
            }

            defaultRoomDetail.RoomAmenities = roomAmenities;

            bookingModel.DefaultRoomDetail = defaultRoomDetail;

            ViewBag.dExchangeRate = 1;

            if (CurrencyCode != "INR")
            {
                etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                if (objExchange.dRate.HasValue)
                {
                    ViewBag.dExchangeRate = 1/objExchange.dRate.Value;
                }
            }

            bookingModel.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);
            //TODO
            if (bookingModel.cBookingStatus == "RM")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(bookingModel);
        }

        [Authorize]
        public ActionResult Modify(string bookingId)
        {
            int bookingId_Decoded = Convert.ToInt32(clsUtils.Decode(bookingId));

            var bookingModel = BL_Booking.GetBookingModifyDetails(bookingId_Decoded);

            var defaultRoomDetail = bookingModel.BookingRoomDetails.FirstOrDefault();

            var roomAmenities = BL_PropDetailsTG.GetRoomAmenities(bookingModel.sVendorId, defaultRoomDetail.RoomId);

            //defaultRoomDetail.RoomAmenities = roomAmenities;
            defaultRoomDetail.sRoomImageUrl = bookingModel.sImgUrl;

            bookingModel.DefaultRoomDetail = defaultRoomDetail;

            ViewBag.dExchangeRate = 1;

            if (CurrencyCode != "INR")
            {
                etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                if (objExchange.dRate.HasValue)
                {
                    ViewBag.dExchangeRate = 1/objExchange.dRate.Value;
                }
            }

            bookingModel.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);

            return View(bookingModel);
        }

        [HttpGet]
        public ActionResult GetCancellationCharge(string bookingId, string bookingDetailsIds)
        {
            try
            {
                etblBookingTx outResult = null;

                long bookingIdParsed = long.Parse(bookingId);

                var bookingDetailsList = BL_Booking.GetBookingWithBookingDetails(bookingIdParsed, out outResult);

                var cancelModel = InitializeCancelBooking(outResult);

                var cancellationConfirmResult = TGBookingManager.InitiateCancelBooking(cancelModel);

                if (cancellationConfirmResult.IsSuccedded)
                {
                    return Json(new { status = true, msg = cancellationConfirmResult.Comment }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Task.Run(() =>
                    {
                        etblBookingTx dbBooking = BL_Booking.GetBooking(bookingIdParsed);
                        dbBooking.BookingStatus = "TX";
                        dbBooking.dtActionDate = DateTime.Now;

                        etblBookingTrakerTx dbBookingTracker = new etblBookingTrakerTx();
                        dbBookingTracker.iBookingId = bookingIdParsed;
                        dbBookingTracker.BookingStatus = "TX";
                        dbBookingTracker.dtActionDate = DateTime.Now;

                        BL_Booking.UpdateBookingStatus(dbBooking, dbBookingTracker);
                    });
                }

                return Json(new { status = false, msg = "Sorry! We are unable to process your request, Please try after some time!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, msg = "Sorry! We are unable to process your request, Please try after some time!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CancelBooking(string bookingId, string provisionalBookingId)
        {
            try
            {
                etblBookingTx bookingModel = null;

                if (bookingModel != null)
                {
                    var html_CancellationDetails = this.RenderViewToString("_CancellationEmailTemplate", bookingModel);

                    var bookingDetails = BL_Booking.GetBooking(long.Parse(bookingId));
                    
                    string notificationMsg_User = "Your Booking ID: " + bookingId + " is now cancelled.";
                    clsUtils.sendSMS(bookingDetails.sMobileOFR, notificationMsg_User);


                    if (bookingDetails != null)
                    {
                        Task.Run(() => MailComponent.SendEmail(bookingDetails.sEmailOFR, "", "", "OneFineRate –Cancellation! Confirmation No: " + bookingId, html_CancellationDetails, null, null, true, null, null));
                        TempData["msg"] = "The Booking has been cancelled !";
                    }
                }

                return RedirectToAction("Modify", new { bookingId = bookingId });
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult ShareBooking(string bookingId)
        {
            eBookingShareInfo obj = new eBookingShareInfo();
            try
            {
                int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));
                ViewBag.HeaderBarData = "Share booking information";
                obj = BL_Booking.GetBookingDetailsForSharing(Convert.ToInt32(bookId));
                obj.PropertyId = OneFineRateAppUtil.clsUtils.Encode(obj.iPropId.ToString());
                obj.BookingId = bookingId;
                string socialShareMessage = BL_Booking.GetSocialShareMessage(bookId);
                obj.sDescription = socialShareMessage;
            }
            catch (Exception)
            {

            }
            return View(obj);
        }

        public ActionResult ShareInformationViaEmail(eBookingShareInfo model)
        {
            if (model != null)
            {
                Task.Run(() => MailComponent.SendEmail(model.sMailTo, "", "", "Share Booking Information", model.sDescription, null, null, false, null, null));
                return RedirectToAction("Confirmation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(model.iBookingId)) });
            }
            return View("Error");
        }

        public ActionResult UpdateGuestInformation(string bookingId)
        {
            ViewBag.HeaderBarData = "Update Information";

            int BookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails bookingDetails = new BookingGuestDetails();
            bookingDetails = BL_PropDetails.GetBookingDetailsForGuestsRoomsInfo(BookingId);
            bookingDetails.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();


            var roomDetails = BL_PropDetailsTG.GetRoomOccupancyDetails(bookingDetails.iVendorId, bookingDetails.lsttblBookDetails.FirstOrDefault().iRoomId);

            bookingDetails.sRoomImageUrl = roomDetails != null ? roomDetails.sDefaultImage : bookingDetails.sMainImgUrl;

            ViewBag.dExchangeRate = 1;

            if (CurrencyCode != "INR")
            {
                etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                if (objExchange.dRate.HasValue)
                {
                    ViewBag.dExchangeRate = 1/objExchange.dRate.Value;
                }
            }

            bookingDetails.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);

            //if (bookingDetails.objBooking.bSelfTravelling != null)
            //{
            //    return RedirectToAction("Confirmation", new { bookingId = bookingId });
            //}

            return View(bookingDetails);
        }

        [HttpPost]
        public ActionResult UpdateGuestInformation(BookingGuestDetails obj)
        {
            int result;
            try
            {
                etblBookingTrakerTx objTrck = new etblBookingTrakerTx();
                List<etblBookingGuestMap> lst = new List<etblBookingGuestMap>();
                List<etblBookingDetailsTx> lsttblBookDetails = new List<etblBookingDetailsTx>();
                string EmailIds = "";

                if (obj.GuestData != null)
                {
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(obj.GuestData.Replace("\\", "\""));
                    if (jArray != null)
                    {

                        foreach (var item in jArray)
                        {
                            lst.Add(new etblBookingGuestMap()
                            {
                                sGuestName = Convert.ToString(item["gname"]),
                                sGuestEmail = Convert.ToString(item["gemail"]),
                                sGuestMobile = Convert.ToString(item["gphone"]),
                                sBedPreference = Convert.ToString(item["ptype"]),
                                sCountryPhoneCode = Convert.ToString(obj.sCountryPhoneCode),
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                // iRPId = Convert.ToInt32(item["planid"]),
                                dtActionDate = DateTime.Now
                            });
                            EmailIds += Convert.ToString(item["gemail"]) + ",";
                        }
                    }
                }

                EmailIds += obj.objBooking.sEmailOFR + ",";
                if (EmailIds != "")
                {
                    EmailIds = EmailIds.Replace(",,", ",");
                    EmailIds = EmailIds.TrimEnd(',');
                }

                //Object created for tblBookingTx
                obj.objBooking.dtActionDate = DateTime.Now;

                //Object created for tblBookingTrakerTx
                objTrck.BookingStatus = "PC";
                objTrck.dtActionDate = DateTime.Now;
                objTrck.iBookingId = obj.objBooking.iBookingId;

                lsttblBookDetails = obj.lsttblBookDetails;

                result = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);
                if (result > 0)
                {
                    var customerModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(obj.objBooking.iBookingId));
                    customerModel.Symbol = "₹";
                    customerModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerModel);
                    Task.Run(() => MailComponent.SendEmail(EmailIds, "", "", "OneFineRate- Modification! Confirmation No: " + obj.objBooking.iBookingId, html_Customer, null, null, true,null,null));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Confirmation", new { bookingId = clsUtils.Encode(obj.objBooking.iBookingId.ToString()) });
        }

        #endregion Public Methods

        #region Helper Method Non Action

        private TG_ProvisionalBookingRequestModel InitializeProvisionalBooking(PropDetailsM propDetail)
        {
            var roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(propDetail.sRoomData);

            TG_ProvisionalBookingRequestModel requestModel = new TG_ProvisionalBookingRequestModel();
            requestModel.CheckIn = propDetail.dtCheckIn.ToString("yyyy-MM-dd");
            requestModel.CheckOut = propDetail.dtCheckOut.ToString("yyyy-MM-dd");
            requestModel.Currency = propDetail.Currency;
            requestModel.CustomerNamePrefix = propDetail.sUserTitle;
            requestModel.CustomerEmail = propDetail.sUserEmail;
            requestModel.CustomerGivenName = propDetail.sUserFirstName;
            requestModel.CustomerSurname = propDetail.sUserLastName;
            requestModel.CustomerComment = "Provisional Hotel Booking";
            requestModel.AmountBeforeTax = propDetail.dSummaryRoomRate.ToString();
            requestModel.ExtraBedCharge = propDetail.dSummaryExtraBedCharges.ToString();
            requestModel.TaxAmount = propDetail.dSummaryTaxes.ToString();
            requestModel.PhoneNumber = propDetail.sUserMobileNo;
            requestModel.RatePlanCode = propDetail.sRatePlanCode;
            requestModel.RoomTypeCode = propDetail.sRoomId;
            requestModel.RoomData = roomDataResult;
            requestModel.NumberOfRooms = roomDataResult.Count.ToString();
            requestModel.HotelCode = propDetail.iVendorId;
            requestModel.CorrelationID = propDetail.objBooking.iBookingId.ToString();
            requestModel.UniqueId = "";
            requestModel.UniqueIdType = "0";

            requestModel.AddressLine = propDetail.sPropertyAddress.Split(',').ToList<string>();
            requestModel.CityName = propDetail.sCity;
            requestModel.PostalCode = "400064";
            requestModel.StateName = "MH";
            requestModel.CountryCode = "IN";
            requestModel.CountryName = "India";

            requestModel.PhoneTechType = "1";
            requestModel.GuaranteeType = "PrePay";
            requestModel.ProfileType = "1";
            requestModel.CountryAccessCode = "91";

            return requestModel;

        }

        private TG_FinalBookingRequestModel InitializeFinalBooking(etblBookingTx booking)
        {
            TG_FinalBookingRequestModel requestModel = new TG_FinalBookingRequestModel();
            requestModel.CorrelationID = booking.iBookingId.ToString();
            requestModel.UniqueId = booking.sProvisionalBookingIdTG;
            requestModel.UniqueIdType = booking.sProvisionalTrxnTypeTG;
            return requestModel;
        }

        private TG_CancelBookingRequestModel InitializeCancelBooking(etblBookingTx booking)
        {
            var datesStringArr = new List<string>();
            //datesStringArr.Add(booking.dtCheckIn.Value.ToString("yyyy-MM-dd"));
            datesStringArr.Add(booking.dtChekOut.Value.ToString("yyyy/dd/MM"));

            //for (var dt = booking.dtCheckIn; dt <= booking.dtChekOut; dt = dt.Value.AddDays(1))
            //{
            //    datesStringArr.Add(dt.Value.ToString("yyyy-MM-dd"));
            //}

            TG_CancelBookingRequestModel requestModel = new TG_CancelBookingRequestModel();
            requestModel.CancelDateArr = datesStringArr.ToArray();
            requestModel.CustomerNamePrefixArr = new string[] { booking.sTitleOFR };
            requestModel.CustomerEmail = booking.sEmailOFR;
            requestModel.CustomerGivenNameArr = new string[] { booking.sFirstNameOFR };
            requestModel.CustomerSurname = booking.sLastNameOFR;
            requestModel.UniqueId = booking.sFinalBookingIdTG;

            return requestModel;
        }

        private string ResendVerification(string phone)
        {
            try
            {
                var phoneVerificationCode = clsUtils.GetVerificationCode();

                var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());

                string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                clsUtils.sendSMS(phone, message);

                return encodedCode;
            }
            catch (Exception)
            {
                return "error";
                throw;
            }
        }

        private string DecodeVC(string VerificationCode)
        {
            try
            {
                var decode = clsUtils.Decode(VerificationCode);
                return decode;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        private long SaveAndGetBookingId(PropDetailsM propDetail)
        {
            long bookingIdFromDbIdentity = 0;

            try
            {
                Decimal? exchangeRate = 1;
                etblBookingTrakerTx bookingTracker = new etblBookingTrakerTx();
                List<etblBookingDetailsTx> lstBookingDetails = new List<etblBookingDetailsTx>();
                List<etblBookingCancellationPolicyMap> lstCancellationPolicy = new List<etblBookingCancellationPolicyMap>();
                List<etblBookingGuestMap> lstBookingGuestMap = new List<etblBookingGuestMap>();

                propDetail.objBooking.iPropId = propDetail.iPropId;
                propDetail.objBooking.iCustomerId = propDetail.iUserId;
                propDetail.objBooking.iGuestId = propDetail.iGuestId;
                if (propDetail.iUserId == 0)
                {
                    propDetail.objBooking.iCustomerId = null;
                }
                if (propDetail.iGuestId == 0)
                {
                    propDetail.objBooking.iGuestId = null;
                }
                propDetail.objBooking.dtCheckIn = propDetail.dtCheckIn;
                propDetail.objBooking.dtChekOut = propDetail.dtCheckOut;
                propDetail.objBooking.dtReservationDate = DateTime.Now;
                propDetail.objBooking.sTitleOFR = propDetail.sUserTitle;
                propDetail.objBooking.sFirstNameOFR = propDetail.sUserFirstName;
                propDetail.objBooking.sLastNameOFR = propDetail.sUserLastName;
                propDetail.objBooking.sEmailOFR = propDetail.sUserEmail;
                propDetail.objBooking.sMobileOFR = propDetail.sUserMobileNo;
                propDetail.objBooking.dtActionDate = DateTime.Now;
                propDetail.objBooking.BookingStatus = "PP";
                propDetail.objBooking.PaymentStatus = "P";
                propDetail.objBooking.sCurrencyCode = CurrencyCode;

                //if (CurrencyCode != "INR")
                //{
                //    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                //    if (objExchange.dRate.HasValue)
                //    {
                //        exchangeRate = objExchange.dRate.Value;
                //    }
                //}

                propDetail.objBooking.dTotalAmount = propDetail.dSummaryRoomRate * exchangeRate;

                propDetail.objBooking.dTaxes = propDetail.dSummaryTaxes * exchangeRate;
                propDetail.objBooking.dTaxesForHotel = propDetail.dSummaryTaxes * exchangeRate;
                propDetail.objBooking.dTotalExtraBedAmount = propDetail.dSummaryExtraBedCharges ;                
                propDetail.objBooking.dTaxesOriginal = propDetail.objBooking.dTaxes;

                propDetail.objBooking.dServiceCharge = propDetail.ServiceChargeTG.ServiceChargeTG;
                propDetail.objBooking.dGSTOnServiceCharge = propDetail.ServiceChargeTG.GstOnServiceChargeTG;
                propDetail.objBooking.dGSTServiceType = propDetail.ServiceChargeTG.GstValueType;
                propDetail.objBooking.dGSTValue = propDetail.ServiceChargeTG.GstValue.ToString();


                string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                propDetail.objBooking.iCountryOffset = zone;

                decimal commissionRate = propDetail.dCommissionRate;
                if (commissionRate != 0)
                {
                    decimal totalCommission = (propDetail.dSummaryRoomRate + propDetail.dSummaryExtraBedCharges) * commissionRate / 100;
                    propDetail.objBooking.dTotalComm = totalCommission * exchangeRate;
                }

                propDetail.objBooking.cBookingType = "R";

                bookingTracker.BookingStatus = "PP";
                bookingTracker.dtActionDate = DateTime.Now;

                var roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(propDetail.sRoomData);

                var roomDetail = propDetail.TG_Hotel.RoomDetails.Where(x => x.RoomId == propDetail.sRoomId).FirstOrDefault();

                var ePropertyPolicyMap = new etblPropertyPolicyMap();

                if (roomDetail != null)
                {
                    var ratePlan = roomDetail.RatePlans.Where(x => x.RatePlanCode == propDetail.sRatePlanCode).FirstOrDefault();

                    foreach (var roomdata in roomDataResult)
                    {
                        var bookingDetail = new etblBookingDetailsTx();

                        bookingDetail.iRoomId = roomDetail.RoomId;
                        bookingDetail.iRPId = ratePlan.RatePlanCode;
                        bookingDetail.iRooms = 1;
                        bookingDetail.sRoomName = roomDetail.RoomName;
                        bookingDetail.sRPName = ratePlan.RatePlanName;
                        bookingDetail.iOccupancy = (short)roomdata.adult;
                        bookingDetail.dRoomRate = ratePlan.RoomRates.First().RatePerRoomPerDay * exchangeRate;

                        bookingDetail.dExtraBedRate = ratePlan.TotalExtraBedCharge * exchangeRate;
                        bookingDetail.sAmenityRatePlan = ratePlan.RateInclusions;
                        bookingDetail.iAdults = (short)roomdata.adult;
                        bookingDetail.iChildren = (short)roomdata.child;
                        bookingDetail.iExtraBeds = (short)roomDetail.ExtraBedCont;
                        bookingDetail.sChildrenAge = string.Join(",", roomdata.ChildAge.Select(x => x.Age));
                       
                        bookingDetail.dTaxes = ratePlan.RoomRates.First().TaxPerDay  * exchangeRate;
                        bookingDetail.dTaxesForHotel = ratePlan.RoomRates.First().TaxPerDay * exchangeRate;

                        bookingDetail.dtActionDate = DateTime.Now;

                        lstBookingDetails.Add(bookingDetail);

                        var bookingCancellation = new etblBookingCancellationPolicyMap();

                        string sPolicyName = string.Empty;

                        if (ratePlan.IsNonRefundable)
                        {
                            sPolicyName = "Non-Refundable";
                        }
                        else
                        {
                            string offsetMultiplier = "0";

                            if (ratePlan.OffsetTimeUnit == "Hour")
                            {
                                System.TimeSpan result = System.TimeSpan.FromHours(ratePlan.OffsetUnitMultiplier);
                                offsetMultiplier = result.TotalDays.ToString();
                            }

                            sPolicyName = ratePlan.NumberOfNights.ToString() + "Nights-" + offsetMultiplier + "Days";
                        }

                        bookingCancellation.sPolicyName = sPolicyName;
                        bookingCancellation.dtDate = propDetail.dtCheckOut - System.TimeSpan.FromHours(ratePlan.OffsetUnitMultiplier);

                        bookingCancellation.dtActionDate = DateTime.Now;
                        bookingCancellation.iRPId = ratePlan.RatePlanCode;
                        bookingCancellation.bTgTaxInclusive = ratePlan.IsTaxInclusive;

                        lstCancellationPolicy.Add(bookingCancellation);
                    }
                }

                bookingIdFromDbIdentity = BL_Booking.AddTGBooking(propDetail, bookingTracker, lstBookingDetails, lstCancellationPolicy, lstBookingGuestMap);

                ePropertyPolicyMap.iPropId = propDetail.iPropId;
                ePropertyPolicyMap.sCheckInHH = propDetail.sCheckInHH.Substring(0, 2);
                ePropertyPolicyMap.sCheckoutHH = propDetail.sCheckoutHH.Substring(0, 2);
                ePropertyPolicyMap.sCheckInMM = propDetail.sCheckInHH.Substring(2);
                ePropertyPolicyMap.sCheckoutMM = propDetail.sCheckInHH.Substring(2);

                int j = BL_tblPropertyPolicyMap.UpdateRecord(ePropertyPolicyMap);
            }
            catch (Exception)
            {
                bookingIdFromDbIdentity = 0;
            }

            return bookingIdFromDbIdentity;
        }

        #endregion Helper Method Non Action
    }
}