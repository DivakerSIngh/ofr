using Microsoft.AspNet.Identity;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OneFineRateWeb.Controllers
{
    [Authorize]
    public class RedeemPointsController : BaseController
    {
        public ActionResult Index(string propId, string checkIn, string checkOut, string sRoomData)
        {
            PropDetailsM propDetailRequestModel = new PropDetailsM();
            PropDetailsM propDetails = new PropDetailsM();

            if (!string.IsNullOrEmpty(propId))
            {
                if (!string.IsNullOrEmpty(checkIn) && !string.IsNullOrEmpty(checkOut))
                {
                    propDetailRequestModel.dtCheckIn = DateTime.Parse(checkIn);
                    propDetailRequestModel.dtCheckOut = DateTime.Parse(checkOut);
                }
                else
                {
                    propDetailRequestModel.dtCheckIn = DateTime.Now.Date.AddDays(1);
                    propDetailRequestModel.dtCheckOut = DateTime.Now.Date.AddDays(2);

                    checkIn = DateTime.Today.ToString("dd/MM/yyyy");
                    checkOut = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                }

                var decodedPropId = Convert.ToInt32(clsUtils.Decode(propId));

                propDetailRequestModel.iPropId = decodedPropId;
                propDetailRequestModel.iUserId = User.Identity.GetUserId<int>();
                propDetailRequestModel.Currency = CurrencyCode;

                #region RoomOccupancySearch

                DataTable dtRoomOccupancySearch = new DataTable();
                dtRoomOccupancySearch.Columns.AddRange(new DataColumn[3]
                {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                });

                DataTable dtChildrenAgeSearch = new DataTable();
                dtChildrenAgeSearch.Columns.AddRange(new DataColumn[2]
                {
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))
                });

                var roomDataResult = new List<RoomData>();

                if (!string.IsNullOrEmpty(sRoomData))
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);
                }
                else
                {
                    roomDataResult = new List<RoomData> { new RoomData { adult = 2, room = 1, child = 0, ChildAge = new List<ChildAge>() } };
                }

                foreach (var room in roomDataResult)
                {
                    DataRow roomOccupancy = dtRoomOccupancySearch.NewRow();
                    roomOccupancy["ID"] = room.room;
                    roomOccupancy["iAdults"] = room.adult;
                    roomOccupancy["children"] = room.child;
                    dtRoomOccupancySearch.Rows.Add(roomOccupancy);

                    foreach (var child in room.ChildAge)
                    {
                        DataRow dtChildrenAge = dtChildrenAgeSearch.NewRow();
                        dtChildrenAge["ID"] = room.room;
                        dtChildrenAge["Age"] = child.Age;
                        dtChildrenAgeSearch.Rows.Add(dtChildrenAge);
                    }
                }

                #endregion

                #region OfferReviewTracker

                var tracker = new etblOfferReviewTrackerTx();
                tracker.iPropId = decodedPropId;
                tracker.dtActionDate = DateTime.Now;
                tracker.sIPAddress = OneFineRateAppUtil.clsUtils.getIpAddress();
                BL_tblOfferReviewTrackerTx.AddRecord(tracker);

                #endregion

                propDetails = BL_PropDetails.GetPropertyDetailsToRedeemPoints(decodedPropId, propDetailRequestModel, dtRoomOccupancySearch, dtChildrenAgeSearch);
                propDetails.bRoomAvailable = true;
                propDetails.cStatus = "A";
                propDetails.dtCheckIn = propDetailRequestModel.dtCheckIn;
                propDetails.dtCheckOut = propDetailRequestModel.dtCheckOut;
                propDetails.sCheckInDate = checkIn;
                propDetails.sCheckOutDate = checkOut;
                propDetails.sRoomData = sRoomData;
                propDetails.hdnJsonData = sRoomData;
                ViewBag.GuestDetailString = roomDataResult.FirstOrDefault().adult + roomDataResult.FirstOrDefault().child + " Guests in 1 Room";

                var uniqueSessionId = Guid.NewGuid().ToString();
                propDetails.TempDataPropDetails = uniqueSessionId;
            }

            return View(propDetails);
        }

        public ActionResult RoomInfo(string propid, string roomid)
        {
            TempData.Keep();
            RoomDetails objRoomDetails = BL_PropDetails.GetRoomDetails(Convert.ToInt32(propid), Convert.ToInt32(roomid), CurrencyCode);
            return PartialView("pvRoomInfo", objRoomDetails);
        }

        public ActionResult Review(string propId, int roomId, string checkIn, string checkOut, int ratePlanId, string sRoomData, bool isPromo, string uniqueSessionId)
        {
            try
            {
                ViewBag.HeaderBarData = "Preview";

                var decodedPropId = Convert.ToInt32(clsUtils.Decode(propId));
                var customerId = User.Identity.GetUserId<long>();

                DateTime dtCheckIn = DateTime.ParseExact(checkIn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dtCheckOut = DateTime.ParseExact(checkOut, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                #region RoomOccupancySearch

                DataTable dtRoomOccupancySearch = new DataTable();
                dtRoomOccupancySearch.Columns.AddRange(new DataColumn[3]
                {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                });

                DataTable dtChildrenAgeSearch = new DataTable();
                dtChildrenAgeSearch.Columns.AddRange(new DataColumn[2]
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
                        DataRow roomOccupancy = dtRoomOccupancySearch.NewRow();
                        roomOccupancy["ID"] = room.room;
                        roomOccupancy["iAdults"] = room.adult;
                        roomOccupancy["children"] = room.child;
                        dtRoomOccupancySearch.Rows.Add(roomOccupancy);

                        foreach (var child in room.ChildAge)
                        {
                            DataRow dtChildrenAge = dtChildrenAgeSearch.NewRow();
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

                            dtChildrenAgeSearch.Rows.Add(dtChildrenAge);
                        }
                    }
                }

                #endregion

                #region Hotel Details

                var propDetails = BL_PropDetails.GetRoomReviewDetailsToRedeemPoints(decodedPropId,
                    customerId,
                    dtCheckIn.ToString("yyyy-MM-dd"),
                    dtCheckOut.ToString("yyyy-MM-dd"),
                                   CurrencyCode,
                                   roomId,
                                   ratePlanId,
                                   isPromo,
                                   dtRoomOccupancySearch,
                                   dtChildrenAgeSearch);

                if (propDetails != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                        propDetails.sUserTitle = user.Title;
                        propDetails.sUserFirstName = user.FirstName;
                        propDetails.sUserLastName = user.LastName;
                        propDetails.sUserEmail = user.Email;
                        propDetails.sUserMobileNo = user.PhoneNumber;
                        propDetails.sCountryPhoneCode = user.sCountryPhoneCode;
                        propDetails.iStateId = user.StateId.HasValue ? user.StateId.Value : 0;
                        propDetails.iUserId = user.Id;
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

                    //Used to Remember when Modify Search
                    ViewBag.sPropId = propId;
                    ViewBag.scheckIn = checkIn;
                    ViewBag.scheckOut = checkOut;
                    ViewBag.sRoomData = sRoomData;
                    ViewBag.iRoomId = roomId;
                    ViewBag.iRatePlanId = ratePlanId;
                    ViewBag.IsPromo = isPromo;

                    propDetails.iPropId = decodedPropId;
                    propDetails.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);
                    propDetails.sRoomData = sRoomData;
                    propDetails.dtCheckIn = dtCheckIn;
                    propDetails.dtCheckOut = dtCheckOut;
                    propDetails.iTotalDays = (dtCheckOut - dtCheckIn).TotalDays.ToString();
                    propDetails.Currency = CurrencyCode;

                    int RatePlan = 0, iNoRoom = 0, iDays = 0, iCount = 0;
                    decimal RoomPrice = 0;
                    decimal DTotal = 0;
                    decimal ExtraCharges = 0;
                    decimal totalTax = 0;
                    int ExtraBed = 0;
                    int totalPoints = 0;

                    for (int i = 0; i < propDetails.lstetblRooms.Count; i++)
                    {
                        for (int j = 0; j < propDetails.lstetblRooms[i].lstRatePlan.Count; j++)
                        {
                            for (int lstOcc = 0; lstOcc < propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                            {
                                if (propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                                {
                                    for (int k = 0; k < propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                                    {
                                        ExtraBed = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds;
                                        ExtraCharges = propDetails.lstetblRooms[i].ExtraBedCharges;
                                        totalTax += propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxesForHotel;
                                        RatePlan = propDetails.lstetblRooms[i].lstRatePlan[j].RPID;
                                        RoomPrice = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice;
                                        isPromo = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].blsPromo;
                                        totalPoints += propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iPoints;
                                        iNoRoom = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms;
                                        iDays = Convert.ToInt32(propDetails.iTotalDays);
                                        iCount = iNoRoom * iDays;

                                        RoomPrice = RoomPrice * (iDays);
                                        ExtraCharges = (ExtraBed * ExtraCharges) * iDays;
                                        DTotal += RoomPrice + ExtraCharges;
                                        iNoRoom++;
                                    }
                                }
                            }
                        }
                    }

                    propDetails.dSummaryRoomRate = RoomPrice;
                    propDetails.sSummaryRoomRate = clsUtils.ConvertNumberToCommaSeprated(RoomPrice);

                    propDetails.dSummaryExtraBedCharges = ExtraCharges;
                    propDetails.sSummaryExtraBedCharges = clsUtils.ConvertNumberToCommaSeprated(ExtraCharges);

                    propDetails.dSummaryTaxes = totalTax;
                    propDetails.sSummaryTaxes = clsUtils.ConvertNumberToCommaSeprated(totalTax);

                    propDetails.dSummaryTotal = DTotal;
                    propDetails.sSummaryTotal = clsUtils.ConvertNumberToCommaSeprated(DTotal);

                    propDetails.sSummaryTaxes_display = clsUtils.ConvertNumberToCommaSeprated(totalTax + propDetails.TaxCharges.TotalServiceCharge);
                    propDetails.sTotalPoints = clsUtils.ConvertNumberToCommaSeprated(totalPoints * Convert.ToInt32(propDetails.iTotalDays));
                    
                    propDetails.TempDataPropDetails = uniqueSessionId;

                    Session[uniqueSessionId] = propDetails;
                }

                #endregion Hotel Details

                return View(propDetails);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="bookingId">It will be available after cancel previous progress booking i.e CancelRewardPointProgressBooking()</param>
        /// <param name="uniqueSessionId"></param>
        /// <returns></returns>
        public ActionResult ChargeCard(int? stateId, int? bookingId, string uniqueSessionId)
        {
            var propDetails = new PropDetailsM();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                    if (!user.StateId.HasValue)
                    {
                        user.StateId = stateId;
                        Task.Run(() => BL_WebsiteUser.UpdateRecord(user));
                    }

                    if (Session[uniqueSessionId] != null)
                    {
                        int bookingIdForPayament = 0;

                        propDetails = Session[uniqueSessionId] as PropDetailsM;

                        if (bookingId.HasValue)
                            bookingIdForPayament = bookingId.Value;
                        else
                            bookingIdForPayament = SaveBookingAndGetBookingId(propDetails);

                        string message = string.Empty;
                        long oldBookingId = 0L;

                        var eligibleToMakePayament = BL_WebsiteUser.CheckIfEligibleToMakePayamentUsingRewardPoint(bookingIdForPayament, user.Id, out message, out oldBookingId);

                        if (!eligibleToMakePayament)
                            return Json(new { status = true, bookingInProgress = true, oldBookingId = oldBookingId, bookingIdForPayament = bookingIdForPayament, message = message }, JsonRequestBehavior.AllowGet);

                        return Json(new { status = true, bookingIdForPayament = bookingIdForPayament }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Sorry! An error occurred while processing your request, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, message = "Sorry! An error occurred while processing your booking, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CancelRewardPointProgressBooking(int bookingId)
        {
            var userId = User.Identity.GetUserId<long>();

            var status = BL_WebsiteUser.CancelLoyalityPointProgressBooking(bookingId, userId);
            if (status > 0)
                return Json(new { status = true, bookingId = bookingId, message = "Your pending booking with booking Id : " + bookingId + " has been successfully cancelled. " }, JsonRequestBehavior.AllowGet);

            return Json(new { status = false, message = "Sorry! An error occurred while processing your request, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
        }

        //Save booking data in Database
        private int SaveBookingAndGetBookingId(PropDetailsM obj)
        {
            int result = 0;
            try
            {
                Decimal? ExchangeRate = 1;
                Decimal? OriginalTotalAmt = 0;
                int? Days = 0;
                etblOriginalBookingPrice objOrgBook = new etblOriginalBookingPrice();
                etblBookingNegotiationTx objNego = new etblBookingNegotiationTx();
                etblBookingTrakerTx objTrck = new etblBookingTrakerTx();
                List<etblBookingGuestMap> lst = new List<etblBookingGuestMap>();
                List<etblBookingDetailsTx> lstBookDetails = new List<etblBookingDetailsTx>();
                List<etblBookingCancellationPolicyMap> lstCancelPolicy = new List<etblBookingCancellationPolicyMap>();
                List<etblBookedDayWiseTaxAmountDetails> lstDayTaxes = new List<etblBookedDayWiseTaxAmountDetails>();
                List<etblBookedDayWiseTaxAmountDetailsAll> lstDayTaxesDateWise = new List<etblBookedDayWiseTaxAmountDetailsAll>();

                obj.objBooking.iPropId = obj.iPropId;
                obj.objBooking.iCustomerId = obj.iUserId;
                obj.objBooking.iGuestId = obj.iGuestId;
                if (obj.iUserId == 0)
                {
                    obj.objBooking.iCustomerId = null;
                }
                if (obj.iGuestId == 0)
                {
                    obj.objBooking.iGuestId = null;
                }
                obj.objBooking.dtCheckIn = obj.dtCheckIn;
                obj.objBooking.dtChekOut = obj.dtCheckOut;
                obj.objBooking.dtReservationDate = DateTime.Now;
                obj.objBooking.sTitleOFR = obj.sUserTitle;
                obj.objBooking.sFirstNameOFR = obj.sUserFirstName;
                obj.objBooking.sLastNameOFR = obj.sUserLastName;
                obj.objBooking.sEmailOFR = obj.sUserEmail;
                obj.objBooking.sMobileOFR = obj.sUserMobileNo;
                obj.objBooking.sCountryPhoneCode = obj.sCountryPhoneCode;
                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.BookingStatus = "PP";
                obj.objBooking.PaymentStatus = "P";
                obj.objBooking.sExtra3 = "Redeem";
                obj.objBooking.sExtra4 = obj.sTotalPoints;
                obj.objBooking.sCurrencyCode = obj.Currency;
                obj.objBooking.dServiceCharge = obj.TaxCharges.dOFRServiceCharge;
                obj.objBooking.dGSTOnServiceCharge = obj.TaxCharges.TaxOnServiceCharge;
                obj.objBooking.dGSTValue = obj.TaxCharges.dGstValue;
                obj.objBooking.dGSTServiceType = obj.TaxCharges.cGstValueType;


                if (obj.objBooking.sCurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",obj.objBooking.sCurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }

                Days = Convert.ToInt32((Convert.ToDateTime(obj.objBooking.dtChekOut) - Convert.ToDateTime(obj.objBooking.dtCheckIn)).TotalDays);

                obj.objBooking.dTotalAmount = obj.dSummaryRoomRate * ExchangeRate;


                decimal OFRServiceTax = obj.objBooking.dServiceCharge + obj.objBooking.dGSTOnServiceCharge;

                if (!String.IsNullOrEmpty(obj.objBooking.sExtra2))
                {
                    obj.objBooking.sExtra2 = (Convert.ToDecimal(obj.objBooking.sExtra2) * Convert.ToDecimal(ExchangeRate)).ToString();
                }
                obj.objBooking.dTaxes = obj.dSummaryTaxes * ExchangeRate;
                obj.objBooking.dTaxesForHotel = obj.dSummaryTaxes * ExchangeRate;
                obj.objBooking.dTotalExtraBedAmount = obj.dSummaryExtraBedCharges * ExchangeRate;

                obj.objBooking.dTaxesOriginal = obj.dSummaryTaxes * ExchangeRate;

                string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                obj.objBooking.iCountryOffset = zone;


                if (obj.objBooking.PromoCodeApplied == true)
                {
                    obj.objBooking.dDiscountedBidPrice = obj.objBooking.PromoCodeValue * ExchangeRate;
                }

                decimal Rate = obj.dCommissionRate;
                if (Rate != 0)
                {
                    decimal Comm = (obj.dSummaryRoomRate + obj.dSummaryExtraBedCharges) * Rate / 100;
                    obj.objBooking.dTotalComm = Comm * ExchangeRate;
                    obj.objBooking.dTotalCommOriginal = Comm * ExchangeRate;
                }

                obj.objBooking.cBookingType = "R";

                objTrck.BookingStatus = "PP";
                objTrck.dtActionDate = DateTime.Now;

                for (int i = 0; i < obj.lstetblRooms.Count; i++)
                {
                    for (int j = 0; j < obj.lstetblRooms[i].lstRatePlan.Count; j++)
                    {
                        for (int lstOcc = 0; lstOcc < obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                        {
                            if (obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                            {
                                for (int k = 0; k < obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                                {
                                    int extrabed = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds;
                                    decimal extrabedcharges = Convert.ToDecimal(obj.lstetblRooms[i].ExtraBedCharges) * Convert.ToDecimal(ExchangeRate);
                                    decimal total = extrabedcharges * extrabed;

                                    decimal? dPriceRP = Convert.ToDecimal(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPriceRP) * Convert.ToDecimal(ExchangeRate);
                                    decimal? dBasePrice = Convert.ToDecimal(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dBasePrice) * Convert.ToDecimal(ExchangeRate);
                                    if (dPriceRP > dBasePrice)
                                    {
                                        OriginalTotalAmt += dPriceRP * Days;
                                    }
                                    else
                                    {
                                        OriginalTotalAmt += dBasePrice * Days;
                                    }

                                    lstBookDetails.Add(new etblBookingDetailsTx()
                                    {
                                        iRoomId = Convert.ToString(obj.lstetblRooms[i].iRoomId),
                                        iRPId = Convert.ToString(obj.lstetblRooms[i].lstRatePlan[j].RPID),
                                        iRooms = 1,
                                        sRoomName = obj.lstetblRooms[i].sRoomName,
                                        sRPName = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].RatePlan,
                                        iOccupancy = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy),
                                        dRoomRate = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice * ExchangeRate,
                                        dExtraBedRate = total,
                                        sAmenityRatePlan = obj.lstetblRooms[i].lstRatePlan[j].RateInclusion,
                                        iAdults = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iAdults),
                                        iChildren = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iChildrens),
                                        iExtraBeds = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds),
                                        sChildrenAge = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ChildrenAge,
                                        dTaxes = (obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes * ExchangeRate) / Days,
                                        dTaxesForHotel = ((obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes) * ExchangeRate) / Days,
                                        dtActionDate = DateTime.Now,
                                        iPromoType = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iPromoType)
                                    });
                                }

                                var lstTaxes = obj.lstTaxesDateWise_OfferReview.Where(u => u.RPID == obj.lstetblRooms[i].lstRatePlan[j].RPID && u.RoomID == obj.lstetblRooms[i].iRoomId && u.iOccupancy == obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy).ToList();

                                for (int tax = 0; tax < lstTaxes.Count; tax++)
                                {
                                    lstDayTaxes.Add(new etblBookedDayWiseTaxAmountDetails()
                                    {
                                        dtStayDay = Convert.ToDateTime(lstTaxes[tax].dtDate),
                                        dAmount = Convert.ToDecimal(lstTaxes[tax].dBasePrice) * ExchangeRate,
                                        dTaxPerc = Convert.ToDecimal(lstTaxes[tax].TaxPer),
                                        dTaxVal = Convert.ToDecimal(lstTaxes[tax].TaxVal) * ExchangeRate,
                                        RoomID = Convert.ToInt32(lstTaxes[tax].RoomID),
                                        RPID = Convert.ToInt32(lstTaxes[tax].RPID),
                                        iOccupancy = Convert.ToInt32(lstTaxes[tax].iOccupancy),
                                        bIsPromo = Convert.ToBoolean(lstTaxes[tax].bIsPromo)
                                    });
                                }
                                var lstTaxesDateWise = obj.lstTaxesDateWiseAll_OfferReview.Where(u => u.RPID == obj.lstetblRooms[i].lstRatePlan[j].RPID && u.iRoomId == obj.lstetblRooms[i].iRoomId && u.iOccupancy == obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy).ToList();

                                for (int tax = 0; tax < lstTaxesDateWise.Count; tax++)
                                {
                                    lstDayTaxesDateWise.Add(new etblBookedDayWiseTaxAmountDetailsAll()
                                    {
                                        dtStayDay = Convert.ToDateTime(lstTaxesDateWise[tax].dtStay),
                                        dAmount = Convert.ToDecimal(lstTaxesDateWise[tax].dPrice) * ExchangeRate,
                                        dTaxPerc = Convert.ToDecimal(lstTaxesDateWise[tax].MaxTaxPer),
                                        dTaxVal = Convert.ToDecimal(lstTaxesDateWise[tax].MaxTaxVal) * ExchangeRate,
                                        RoomID = Convert.ToInt32(lstTaxesDateWise[tax].iRoomId),
                                        RPID = Convert.ToInt32(lstTaxesDateWise[tax].RPID),
                                        iOccupancy = Convert.ToInt32(lstTaxesDateWise[tax].iOccupancy),
                                        iTaxId = Convert.ToInt32(lstTaxesDateWise[tax].TaxId)
                                    });
                                }

                                for (int lstCancellation = 0; lstCancellation < obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy.Count; lstCancellation++)
                                {
                                    DateTime StarDate = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].ValidFrom;
                                    DateTime EndDate = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].ValidTo;
                                    var res = new List<string>();
                                    for (var date = StarDate; date <= EndDate; date = date.AddDays(1))
                                        res.Add(date.ToString());
                                    foreach (var ddate in res)
                                    {
                                        lstCancelPolicy.Add(new etblBookingCancellationPolicyMap()
                                        {
                                            sPolicyName = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].PolicyName,
                                            dtDate = Convert.ToDateTime(ddate),
                                            dtActionDate = DateTime.Now,
                                            iRPId = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].iRPId.ToString()
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                objOrgBook.dOriginalTotalAmount = OriginalTotalAmt;

                result = BL_Booking.AddBooking(obj, objNego, objTrck, lstBookDetails, lst, lstCancelPolicy, lstDayTaxes, objOrgBook, lstDayTaxesDateWise);

            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }
    }
}