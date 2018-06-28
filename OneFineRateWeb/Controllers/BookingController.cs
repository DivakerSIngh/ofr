using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using OneFineRateAppUtil;
using System.Text;
using FutureSoft.Util;
using OneFineRateBLL;
using Newtonsoft.Json;
using OneFineRateBLL.Entities;
using System.Web.Script.Serialization;
using System.Data;
using Newtonsoft.Json.Linq;
using OneFineRateWeb.App_Helper;
using System.Configuration;
using OneFineRateBLL.WebUserEntities;
using System.Web;
using System.Net.Mail;
using System.IO;

namespace OneFineRateWeb.Controllers
{
    public class BookingController : BaseController
    {
        #region Public Methods

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BookingReview(FormCollection formData)
        {
            ViewBag.HeaderBarData = "Booking Review";
            var propDetail = TempData["propDetailsTG"] as PropDetailsM;
            string roomId = formData["roomId"];
            string vendorId = formData["vendorId"];
            string ratePlanCode = formData["RatePlanCode"];

            if (propDetail != null)
            {
                propDetail.sRoomId = roomId ?? propDetail.sRoomId;
                propDetail.sRatePlanCode = ratePlanCode ?? propDetail.sRatePlanCode;

                var roomDetail = propDetail.TG_Hotel.RoomDetails.Where(x => x.RoomId == propDetail.sRoomId).FirstOrDefault();
                if (roomDetail != null)
                {
                    var ratePlan = roomDetail.RatePlans.Where(x => x.RatePlanCode == propDetail.sRatePlanCode).FirstOrDefault();
                    if (ratePlan != null)
                    {
                        if (CurrencyCode == "USD")
                        {
                            etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById(CurrencyCode, "INR");
                            if (objExchange.dRate != 0)
                            {
                                var exchangeRate = 1/objExchange.dRate.Value;
                                propDetail.sRoomId = roomDetail.RoomId;
                                propDetail.sRatePlanCode = ratePlan.RatePlanCode;
                                propDetail.iVendorId = roomDetail.VendorId;
                                propDetail.sRoomName = roomDetail.RoomName;
                                propDetail.dSummaryRoomRate = Math.Round(ratePlan.TotalRoomRate * exchangeRate, 2); ;
                                propDetail.dSummaryTaxes = Math.Round(ratePlan.TotalTax * exchangeRate, 2);
                                propDetail.dSummaryExtraBedCharges = Math.Round(ratePlan.TotalExtraBedCharge * exchangeRate, 2);
                                propDetail.Symbol = "$";
                            }
                        }
                        else
                        {
                            propDetail.sRoomId = roomDetail.RoomId;
                            propDetail.sRatePlanCode = ratePlan.RatePlanCode;
                            propDetail.iVendorId = roomDetail.VendorId;
                            propDetail.sRoomName = roomDetail.RoomName;
                            propDetail.dSummaryRoomRate = ratePlan.TotalRoomRate;
                            propDetail.dSummaryTaxes = ratePlan.TotalTax;
                            propDetail.dSummaryExtraBedCharges = ratePlan.TotalExtraBedCharge;
                            propDetail.Symbol = "₹";
                        }
                    }
                }

                if (User.Identity.IsAuthenticated)
                {
                    var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                    propDetail.sUserTitle = user.Title;
                    propDetail.sUserFirstName = user.FirstName;
                    propDetail.sUserLastName = user.LastName;
                    propDetail.sUserEmail = user.Email;
                    propDetail.sUserMobileNo = user.PhoneNumber;
                }
            }

            TempData["propDetailsTG"] = propDetail;
            ViewBag.Referral = Request.UrlReferrer;
            return View(propDetail);
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

        // [Authorize]
        public ActionResult Modify(string bookingId)
        {
            int bookingId_Decoded = 0;

            string appropriateBooking = string.Empty;

            try
            {
                bookingId_Decoded = Convert.ToInt32(clsUtils.Decode(bookingId));
                appropriateBooking = bookingId;
            }
            catch
            {
                bookingId_Decoded = Convert.ToInt32(bookingId);
                appropriateBooking = clsUtils.Encode(bookingId);
            }

            var bookingModel = BL_Booking.GetBookingModifyDetails(bookingId_Decoded);

            if (bookingModel != null)
            {
                if (bookingModel.cBookingType == "N")
                {
                    if (bookingModel.cBookingStatus == "BP")
                    {
                        return RedirectToRoute("BalancePaymnet", new { bookingId = appropriateBooking });
                    }

                    return RedirectToAction("NegotiationStatus", "Negotiation", new { bookingId = appropriateBooking });
                }
            }

            var roomDataResult = new List<RoomData>();

            #region FetchRoomData

            var bookingDetailList = BL_Booking.GetBookingDetailList(bookingId_Decoded);

            if (bookingDetailList != null)
            {
                int roomId = 1;

                foreach (var bookingDetail in bookingDetailList)
                {
                    var roomData = new RoomData() { adult = bookingDetail.iAdults.Value, child = bookingDetail.iChildren.Value, room = roomId, iBookingDetailId = bookingDetail.iBookingDetailsID };

                    if (!string.IsNullOrEmpty(bookingDetail.sChildrenAge))
                    {
                        foreach (var childAge in bookingDetail.sChildrenAge.Split(','))
                        {
                            roomData.ChildAge = new List<ChildAge>() { new ChildAge() { Age = childAge } };
                        }
                    }
                    else
                    {
                        roomData.ChildAge = new List<ChildAge>() { };
                    }

                    roomDataResult.Add(roomData);
                    roomId++;
                }

                var searchParams = bookingDetailList.Select(x => new { x.dtCheckIn, x.dtCheckOut }).FirstOrDefault();

                if (searchParams.dtCheckIn < DateTime.Today)
                {
                    bookingModel.BookingStatus = "expire";
                    return View(bookingModel);
                }

                if (searchParams.dtCheckIn == null && searchParams.dtCheckOut == null)
                {

                    bookingModel.sCheckIn = DateTime.Today.ToString("dd/MM/yyyy");
                    bookingModel.sCheckOut = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                }
                else
                {
                    bookingModel.sCheckIn = searchParams.dtCheckIn.Value.ToString("dd/MM/yyyy");
                    bookingModel.sCheckOut = searchParams.dtCheckOut.Value.ToString("dd/MM/yyyy");

                }

                bookingModel.sRoomData = clsUtils.ConvertToJson(roomDataResult);

            }

            #endregion
            var guid = Guid.NewGuid().ToString();
            string tempdataOldRooms = "OldRooms" + guid;
            bookingModel.TempDataOldRooms = tempdataOldRooms;

            Session[tempdataOldRooms] = bookingModel;
            // TempData.Keep(tempdataOldRooms);

            return View(bookingModel);
        }

        //[Authorize]
        [ValidateInput(false)]
        public ActionResult GetCanellationCharges(PropDetailsM propDetail, string bookingDetailsId)
        {
            object result = null;
            try
            {

                var lstBookdetails = new List<eBookingRoomM>();
                var lstDeletedBookingdetails = new List<int>();
                var lstAllData_Temp = new eBookingModify();
                var lstAllData = new eBookingModify();
                lstAllData = Session[propDetail.TempDataOldRooms] as eBookingModify;

                lstAllData_Temp = Session[propDetail.TempDataOldRooms] as eBookingModify;
                if (Session[propDetail.TempDataOldRooms] != null)
                {
                    // TempData.Keep(propDetail.TempDataOldRooms);


                    //if (TempData["OldRooms_Temp"] != null) { TempData.Keep(); lstAllData_Temp = TempData["OldRooms"] as eBookingModify; }

                    lstBookdetails = lstAllData_Temp.BookingRoomDetails;

                    var idsArr = bookingDetailsId.Split(',').Select(x => x);

                    //foreach (var item in lstBookdetails)
                    //{
                    //   // item.IsActive = true;
                    //    if (idsArr.Contains(item.iBookingDetailsId))
                    //    {
                    //        item.IsActive = false;
                    //    }
                    //    else
                    //    {
                    //        item.IsActive = true;
                    //    }
                    //}



                    long BookingId = Convert.ToInt64(lstAllData_Temp.BookingId);
                    DataTable dtBookingDetails = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("iBDId", typeof(Int64));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("iRoomId", typeof(String));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("iRPId", typeof(String));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("iOccupancy", typeof(Int16));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("dAvgRoomRateWithExtraBed", typeof(Decimal));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("dAvgTaxes", typeof(Decimal));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("iAdults", typeof(Int16));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("iChildren", typeof(Int16));
                    dtBookingDetails.Columns.Add(col);
                    col = new DataColumn("sChildrenAge", typeof(String));
                    dtBookingDetails.Columns.Add(col);



                    for (int i = 0; i < propDetail.lstetblRooms.Count; i++)
                    {
                        for (int j = 0; j < propDetail.lstetblRooms[i].lstRatePlan.Count; j++)
                        {
                            for (int lstOcc = 0; lstOcc < propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                            {
                                if (propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                                {
                                    for (int k = 0; k < propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                                    {
                                        int Occupancy = propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy;
                                        //var lst = lstBookdetails.Where(u => u.Occupancy == Occupancy.ToString() && u.IsActive == true).ToList();
                                        //var lst = lstBookdetails.Where(u => u.Occupancy == Occupancy.ToString() && u.IsActive == false).ToList();
                                        foreach (var item in lstBookdetails)
                                        {
                                            // item.IsActive = true;
                                            if (idsArr.Contains(item.iBookingDetailsId))
                                            {
                                                item.IsActive = false;
                                                //if (lst.Count > 0)
                                                //{
                                                //  var firstRecord = lst.FirstOrDefault();
                                                //lstBookdetails.Remove(firstRecord);
                                                //Aditya
                                                //   firstRecord.IsActive = false;

                                                //DataRow dr = dtBookingDetails.NewRow();
                                                //dr["iBDId"] = Convert.ToInt32(firstRecord.iBookingDetailsId);
                                                //dr["iRoomId"] = Convert.ToString(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iRoomId);
                                                //dr["iRPId"] = Convert.ToInt32(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].RPID);
                                                //dr["iOccupancy"] = Occupancy;
                                                //dr["dAvgRoomRateWithExtraBed"] = Convert.ToDecimal(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dBasePrice);
                                                //dr["dAvgTaxes"] = Convert.ToDecimal(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes);
                                                //dr["iAdults"] = Convert.ToInt16(firstRecord.Adult);
                                                //dr["iChildren"] = Convert.ToInt16(firstRecord.Children);
                                                //dr["sChildrenAge"] = Convert.ToString(firstRecord.sChildrenAge);
                                                //dtBookingDetails.Rows.Add(dr);

                                                //lstDeletedBookingdetails.Add(Convert.ToInt32(firstRecord.iBookingDetailsId));

                                                DataRow dr = dtBookingDetails.NewRow();
                                                dr["iBDId"] = Convert.ToInt32(item.iBookingDetailsId);
                                                dr["iRoomId"] = Convert.ToString(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iRoomId);
                                                dr["iRPId"] = Convert.ToInt32(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].RPID);
                                                dr["iOccupancy"] = Occupancy;
                                                dr["dAvgRoomRateWithExtraBed"] = Convert.ToDecimal(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dBasePrice);
                                                dr["dAvgTaxes"] = Convert.ToDecimal(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes);
                                                dr["iAdults"] = Convert.ToInt16(item.Adult);
                                                dr["iChildren"] = Convert.ToInt16(item.Children);
                                                dr["sChildrenAge"] = Convert.ToString(item.sChildrenAge);
                                                dtBookingDetails.Rows.Add(dr);
                                                lstDeletedBookingdetails.Add(Convert.ToInt32(item.iBookingDetailsId));
                                            }
                                            else
                                            {
                                                item.IsActive = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    propDetail.lstDeletedBookingDetails = lstDeletedBookingdetails.Distinct().ToList();
                    DataView view = new DataView(dtBookingDetails);
                    dtBookingDetails = view.ToTable(true, "iBDId", "iRoomId", "iRPId", "iOccupancy", "dAvgRoomRateWithExtraBed", "dAvgTaxes", "iAdults", "iChildren", "sChildrenAge");

                    //Get Time Zone for this booking
                    string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                    decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                    decimal? Offset = zone;

                    bool bType = true;
                    if (propDetail.sRequestType.Trim().ToLower() == "modifyrate")
                    {
                        bType = false;
                    }

                    BookingAmedCancelation objCharges = BL_Booking.GetAmendCacellationCharges(BookingId, dtBookingDetails, Convert.ToDecimal(Offset), Convert.ToDateTime(propDetail.sCheckInDate), Convert.ToDateTime(propDetail.sCheckOutDate), bType);
                    if (objCharges.lstRoomCharges.Count > 0)
                    {
                        result = new { st = 1, msg = "Cancellation charges applicable.", lstdata = OneFineRateAppUtil.clsUtils.ConvertToJson(objCharges.lstRoomCharges), Totalcharges = objCharges.TotalCanellationCharges, symbol = lstAllData_Temp.Symbol };
                        propDetail.dTotalCancellationCharges = objCharges.TotalCanellationCharges;
                        Session[propDetail.TempDataRoomCancellationCharges] = objCharges.lstBookingRoomCharges;
                    }
                    else
                    {
                        result = new { st = 1, msg = "No cancellation charges applicable." };
                    }
                }
                else
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            Session[propDetail.TempDataPropDetails] = propDetail;
            // TempData.Keep(propDetail.TempDataPropDetails);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        public ActionResult ModificationReview(string TempDataOldRooms, string TempDataPropDetails, string TempDataRoomCancellationCharges)
        {
            ViewBag.HeaderBarData = "Preview";
            var propDetail = new PropDetailsM();
            var lstAllData = new eBookingModify();
            var lstRoomCancelCharges = new List<eBookingAmendCancelBookingRoomIdWiseCharges>();
            try
            {


                if (Session[TempDataOldRooms] != null)
                {
                    // TempData.Keep(TempDataOldRooms);
                    lstAllData = Session[TempDataOldRooms] as eBookingModify;
                }



                if (Session[TempDataPropDetails] != null)
                {// TempData.Keep(TempDataPropDetails);
                    propDetail = Session[TempDataPropDetails] as PropDetailsM;
                }
                propDetail.IsDataHasSaved = false; // To Check If Already Saved in DB.

                //feteched room cancellation charges booking details id wise
                if (Session[TempDataRoomCancellationCharges] != null)
                {// TempData.Keep(TempDataRoomCancellationCharges);
                    lstRoomCancelCharges = TempData[TempDataRoomCancellationCharges] as List<eBookingAmendCancelBookingRoomIdWiseCharges>;
                }


                propDetail.dOFRServiceCharge = propDetail.TaxCharges.dOFRServiceCharge + propDetail.TaxCharges.TaxOnServiceCharge;

                propDetail.BookingRoomDetails = lstAllData.BookingRoomDetails;
                propDetail.dTotalAmountPaid = Convert.ToDecimal(lstAllData.SubTotal);
                propDetail.RatingImageUrl = lstAllData.RatingImageUrl;
                propDetail.RatingString = lstAllData.RatingString;

                propDetail.scheckIn = String.Format("{0:dddd MMM d yyyy}", propDetail.sCheckInDate);
                propDetail.scheckOut = String.Format("{0:dddd MMM d yyyy}", propDetail.sCheckOutDate);
                propDetail.sExtra1 = lstAllData.sExtra1; //  on modify discount is not applicable.

                decimal dTotal = 0;
                for (int i = 0; i < propDetail.lstetblRooms.Count; i++)
                {

                    for (int j = 0; j < propDetail.lstetblRooms[i].lstRatePlan.Count; j++)
                    {

                        for (int lstOcc = 0; lstOcc < propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                        {
                            if (propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                            {
                                for (int k = 0; k < propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                                {
                                    var ExtraBedNumber = propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds;
                                    var ExtraBedCharges = propDetail.lstetblRooms[i].ExtraBedCharges;
                                    var ExtraCharges = ExtraBedNumber * ExtraBedCharges;
                                    var Price = propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice;
                                    var Days = Convert.ToInt32(propDetail.iTotalDays);
                                    var totalrooms = propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms;
                                    var TotalPrice = (Price + ExtraCharges) * Days;

                                    dTotal += Convert.ToDecimal(TotalPrice);


                                }
                            }
                        }
                    }
                }

                var activeBookingDetailsList = propDetail.BookingRoomDetails.Where(u => u.IsActive == true).ToList();

                if (activeBookingDetailsList.Count > 0)
                {
                    foreach (var item in activeBookingDetailsList)
                    {
                        var Days = Convert.ToInt32(propDetail.iTotalDays);

                        var ExtraBedCharges = item.dExtraBedRate;
                        var ExtraCharges = ExtraBedCharges;
                        var Price = Convert.ToDecimal(item.RoomRate) - Convert.ToDecimal(item.dExtraBedRate);

                        var TotalPrice = (Price + ExtraCharges) * Days;

                        dTotal += Convert.ToDecimal(TotalPrice);
                    }
                }



                //if (dTotal > 0)
                //{
                //    decimal? PromoDiscount = 0;
                //    string PromoCode = lstAllData.sExtra1;
                //    if (!String.IsNullOrEmpty(PromoCode))
                //    {
                //        string[] Arr;
                //        if (PromoCode.Contains("%"))
                //        {
                //            Arr = PromoCode.Split('%');
                //            decimal amt = Convert.ToDecimal(Arr[0]);

                //            PromoDiscount = (dTotal * amt) / 100;
                //        }
                //        else
                //        {
                //            string[] stringSeparators = new string[] { "Val" };
                //            Arr = PromoCode.Split(stringSeparators, StringSplitOptions.None);

                //            decimal amt = Convert.ToDecimal(Arr[0]);

                //            PromoDiscount = (dTotal) - amt;
                //        }

                propDetail.dDiscountedBidPrice = Convert.ToDecimal(lstAllData.dDiscountedBidPrice);
                //    }
                //}
            }
            catch
            {

                return View("Error");
            }
            propDetail.TempDataOldRooms = TempDataOldRooms;
            propDetail.TempDataPropDetails = TempDataPropDetails;
            propDetail.TempDataRoomCancellationCharges = TempDataRoomCancellationCharges;
            return View(propDetail);
        }

        public ActionResult UpdateBooking(string TempDataOldRooms, string TempDataPropDetails, string TempDataRoomCancellationCharges)
        {
            var obj = new PropDetailsM();
            var lstRoomCancelCharges = new List<eBookingAmendCancelBookingRoomIdWiseCharges>();
            var lstAllData = new eBookingModify();

            try
            {

                Decimal? ExchangeRate = 1, dBaseAmount = 0, dCustomerPayable = 0, dTotalAmount = 0, dTotalComm = 0, dTaxes = 0, dTotalExtraBedAmount = 0, dDiscountedBidPrice = 0;
                int? Days = 0;


                //feteched left booking details rooms which were not modified
                if (Session[TempDataOldRooms] != null)
                { //TempData.Keep(TempDataOldRooms);
                    lstAllData = Session[TempDataOldRooms] as eBookingModify;
                }

                //feteched fresh booking details rooms
                if (Session[TempDataPropDetails] != null)
                {// TempData.Keep(TempDataPropDetails);
                    obj = Session[TempDataPropDetails] as PropDetailsM;
                }

                //feteched room cancellation charges booking details id wise
                if (Session[TempDataRoomCancellationCharges] != null)
                { //TempData.Keep(TempDataRoomCancellationCharges); 
                    lstRoomCancelCharges = Session[TempDataRoomCancellationCharges] as List<eBookingAmendCancelBookingRoomIdWiseCharges>;
                }

                //fetch the data of booking and booking object created
                etblBookingTx objBook = BL_Booking.GetBooking(Convert.ToInt64(lstAllData.BookingId));


                //Type of Amendment


                if (obj.sRequestType != "ModifyRate")
                {
                    objBook.dtCheckIn = Convert.ToDateTime(obj.sCheckInDate);
                    objBook.dtChekOut = Convert.ToDateTime(obj.sCheckOutDate);

                }

                decimal OFRServiceTax = obj.TaxCharges.TaxOnServiceCharge + obj.TaxCharges.dOFRServiceCharge;
                //decimal OFRServiceTax = Convert.ToDecimal(BL_Bidding.GetBidMaster(objBook.sCurrencyCode != null ? objBook.sCurrencyCode.ToString() : "INR").dOFRServiceCharge);
                Days = Convert.ToInt32((Convert.ToDateTime(objBook.dtChekOut) - Convert.ToDateTime(objBook.dtCheckIn)).TotalDays);

                if (objBook.sCurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",obj.objBooking.sCurrencyCode );
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/ objExchange.dRate;
                    }
                }

                // tables object created
                etblBookingTrakerTx objTrck = new etblBookingTrakerTx();
                List<etblBookingDetailsTx> lstBookDetails = new List<etblBookingDetailsTx>();
                List<etblBookingCancellationPolicyMap> lstCancelPolicy = new List<etblBookingCancellationPolicyMap>();
                List<etblBookedDayWiseTaxAmountDetails> lstDayTaxes = new List<etblBookedDayWiseTaxAmountDetails>();
                List<etblBookedDayWiseTaxAmountDetailsAll> lstDayTaxesDateWise = new List<etblBookedDayWiseTaxAmountDetailsAll>();
                etblBookingAmedTx objBooking_Amend = new etblBookingAmedTx();


                objBooking_Amend = (etblBookingAmedTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objBook, objBooking_Amend);

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
                                    decimal RoomAmt = Convert.ToDecimal(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice) * Convert.ToDecimal(ExchangeRate);

                                    dTotalAmount += (RoomAmt + total) * Days;
                                    dTaxes += Convert.ToDecimal(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes * ExchangeRate);
                                    dTotalExtraBedAmount = total * Days;

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

                                // datewise 
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
                                        iTaxId = Convert.ToInt32(lstTaxesDateWise[tax].TaxId),
                                        sTaxName = Convert.ToString(lstTaxesDateWise[tax].sTaxName)
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


                for (int i = 0; i < lstAllData.BookingRoomDetails.Count; i++)
                {
                    if (lstAllData.BookingRoomDetails[i].IsActive == true)
                    {
                        decimal extrabedcharges = Convert.ToDecimal(lstAllData.BookingRoomDetails[i].dExtraBedRate);
                        decimal total = extrabedcharges;
                        decimal RoomAmt = Convert.ToDecimal(lstAllData.BookingRoomDetails[i].RoomRate) * Convert.ToDecimal(ExchangeRate);

                        dTotalAmount += (RoomAmt) * Days;
                        dTaxes += Convert.ToDecimal(lstAllData.BookingRoomDetails[i].dTaxes) * ExchangeRate *Days;
                        dTotalExtraBedAmount += total * Days;
                    }

                }

                decimal CustomerTaxes = Convert.ToDecimal(dTaxes) + Convert.ToDecimal(OFRServiceTax);

                decimal AmtPaid = Convert.ToDecimal(obj.dTotalAmountPaid);
                decimal CancelAmt = Convert.ToDecimal(obj.dTotalCancellationCharges);
                decimal Balance = Convert.ToDecimal(dTotalAmount + dTaxes) - (Convert.ToDecimal(AmtPaid) - CancelAmt);
                objBook.dRefundAmount = Balance + Convert.ToDecimal(OFRServiceTax);
                objBook.cRefundStatus = "P";
                if (Balance <= 0)
                {
                    if (obj.sRequestType == "ModifyRate")
                    {
                        objBook.BookingStatus = "MR";
                        objBook.PaymentStatus = "C";
                        objBook.cAmendStatus = "R";
                        objBooking_Amend.cAmendStatus = "R";
                    }
                    else
                    {
                        objBook.BookingStatus = "MD";
                        objBook.PaymentStatus = "C";
                        objBook.cAmendStatus = "D";
                        objBooking_Amend.cAmendStatus = "D";

                    }
                    objBooking_Amend.cRefundStatus = "P";

                }
                else
                {
                    if (obj.sRequestType == "ModifyRate")
                    {
                        objBook.BookingStatus = "MR";
                        objBook.PaymentStatus = "P";
                        objBook.cAmendStatus = "R";
                        objBooking_Amend.cAmendStatus = "R";

                    }
                    else
                    {
                        objBook.BookingStatus = "MD";
                        objBook.PaymentStatus = "P";
                        objBook.cAmendStatus = "D";
                        objBooking_Amend.cAmendStatus = "D";
                    }
                }

                objBook.dTotalAmount = dTotalAmount - dTotalExtraBedAmount;

                objBook.dTaxes = dTaxes;
                objBook.dTaxesOriginal = CustomerTaxes;
                objBook.dTaxesForHotel = dTaxes;
                objBook.dTotalExtraBedAmount = dTotalExtraBedAmount;
                objBook.dCustomerPayable = dTotalAmount;
                //objBook.dDiscountedBidPrice = 0;// ON Modify we are not providing discount so its 0 .// 11/17/2017 /// reverted back
                objBook.sExtra1 = "";

                decimal Rate = obj.dCommissionRate;
                if (Rate != 0)
                {
                    decimal Comm = (Convert.ToDecimal(dTotalAmount) * Rate) / 100;
                    objBook.dTotalComm = Comm * ExchangeRate;
                    objBook.dTotalCommOriginal = Comm * ExchangeRate;
                }
                int result = 0;
                if (!obj.IsDataHasSaved)
                {
                    result = BL_Booking.Amendments_UpdateBooking(objBook, objBooking_Amend, lstBookDetails, lstCancelPolicy, lstDayTaxes, obj.lstDeletedBookingDetails, lstRoomCancelCharges, lstDayTaxesDateWise);
                }
                else
                {
                    result = 1;
                }

                if (result == 1)
                {
                    obj.IsDataHasSaved = true;
                    //string notificationSMS_User = "Your Booking ID: " + objBook.iBookingId + " is now amended";

                    //string extranetUrl = System.Configuration.ConfigurationManager.AppSettings["OFRExtranetBaseUrl"] + "Bookings/ModifyBooking?=" + clsUtils.Encode(objBook.iPropId.ToString());

                    //string notificationSMS_Mngr = "Your Booking Id: " + objBook.iBookingId + " is now amended. Please view the amendment by clicking the link :" + extranetUrl;

                    //// get from tblEmailSetting
                    //var revenueManagerDetail = BL_tblEmailSettingsM.GetRecord("RevenueManager");

                    //// get from propertyEdit page 
                    //var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(objBook.iPropId.Value);

                    //string sPrimaryContactEmail;
                    //string sConfirmationContactEmail;
                    //string sRevenueManagerEmail;
                    //string sRevenueManagerContact;

                    //pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                    //pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                    //pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                    //pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

                    //List<string> mailList = new List<string>();
                    //mailList.Add(sRevenueManagerEmail);
                    //mailList.Add(sConfirmationContactEmail);
                    //mailList.Add(sPrimaryContactEmail);

                    //string emails = string.Join(",", mailList.Distinct());

                    //Task.Run(() => clsUtils.sendSMS(sRevenueManagerContact, notificationSMS_Mngr));

                    //Task.Run(() => clsUtils.sendSMS(objBook.sMobileOFR, notificationSMS_User));


                    //Update Datewise booking rate 
                    Task.Run(() => BL_Booking.UpdateDaywiseBookingRate(Convert.ToInt32(objBook.iBookingId)));

                    //Session[TempDataRoomCancellationCharges] = null;
                    //Session[TempDataOldRooms] = null;
                    //Session[TempDataPropDetails] = null;
                    if (Balance <= 0)
                    {
                        return RedirectToAction("UpdateGuestBookingDetailsAmmend", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(objBook.iBookingId.ToString()) });
                    }
                    else
                    {
                        //var customerModel = new NegotiationEmailTempleteModel();
                        //var html_Customer = "";
                        //var bookingModel = BL_Booking.GetBookingModifyDetails(objBook.iBookingId);
                        //customerModel.BookingModify = bookingModel;
                        //customerModel.Status = "" + bookingModel.Booker + "! Thank you for initiating the payment. Click the link below to continue." + Environment.NewLine + "";
                        //var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                        //customerModel.CallbackUrl = websiteBaseUrl + "UpdateGuestBookingDetailsAmmend/" + clsUtils.Encode(objBook.iBookingId.ToString());
                        //html_Customer = this.RenderViewToString("_EmailTemplete", customerModel);
                        //Task.Run(() => MailComponent.SendEmailAsync(bookingModel.EmailOFR, "", "", "Payment Initialization", html_Customer, null, null, true));


                        return RedirectToAction("PayNow", "Payment", new { bookingId = objBook.iBookingId });

                    }

                }
                else
                {
                    obj.dOFRServiceCharge = Convert.ToDecimal(BL_Bidding.GetBidMaster(lstAllData.sCurrencyCode).dOFRServiceCharge);

                    obj.BookingRoomDetails = lstAllData.BookingRoomDetails;
                    obj.dTotalAmountPaid = Convert.ToDecimal(lstAllData.SubTotal);
                    obj.scheckIn = String.Format("{0:dddd MMM d yyyy}", obj.sCheckInDate);
                    obj.scheckOut = String.Format("{0:dddd MMM d yyyy}", obj.sCheckOutDate);

                }
            }
            catch (Exception ex)
            {
                obj.dOFRServiceCharge = Convert.ToDecimal(BL_Bidding.GetBidMaster(lstAllData.sCurrencyCode).dOFRServiceCharge);

                obj.BookingRoomDetails = lstAllData.BookingRoomDetails;
                obj.dTotalAmountPaid = Convert.ToDecimal(lstAllData.SubTotal);
                obj.scheckIn = String.Format("{0:dddd MMM d yyyy}", obj.sCheckInDate);
                obj.scheckOut = String.Format("{0:dddd MMM d yyyy}", obj.sCheckOutDate);
            }
            return View(obj);
        }

        public ActionResult GetHotelRooms(string propId, string sCheckIn, string sCheckOut, string sRoomData, string sRequestType, string tempdataOldRooms)
        {
            try
            {
                if (!string.IsNullOrEmpty(propId))
                {
                    PropDetailsM dbRequestParamModel = new PropDetailsM();
                    dbRequestParamModel.iPropId = int.Parse(propId);

                    if (string.IsNullOrEmpty(sCheckIn) && string.IsNullOrEmpty(sCheckOut))
                    {
                        dbRequestParamModel.dtCheckIn = DateTime.Now.Date.AddDays(1);
                        dbRequestParamModel.dtCheckOut = DateTime.Now.Date.AddDays(2);

                        sCheckIn = DateTime.Today.ToString("dd/MM/yyyy");
                        sCheckOut = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        dbRequestParamModel.dtCheckIn = DateTime.Parse(sCheckIn);
                        dbRequestParamModel.dtCheckOut = DateTime.Parse(sCheckOut);
                    }

                    dbRequestParamModel.bLogin = User.Identity.IsAuthenticated;
                    dbRequestParamModel.Currency = CurrencyCode;


                    #region FetchRoomData and bind in datatables

                    DataTable dtTblRoomOccupancySearch = new DataTable();
                    dtTblRoomOccupancySearch.Columns.AddRange(new DataColumn[3] {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                    });

                    DataTable dtTblChildrenAgeSearch = new DataTable();
                    dtTblChildrenAgeSearch.Columns.AddRange(new DataColumn[2] {
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
                        roomDataResult.Add(new RoomData() { adult = 2, child = 0, room = 1, ChildAge = new List<ChildAge>() { new ChildAge() { Age = "5" } } });

                        sRoomData = clsUtils.ConvertToJson(roomDataResult);
                    }

                    dbRequestParamModel.sRoomData = sRoomData;

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
                            dtChildrenAge["Age"] = child.Age;
                            dtTblChildrenAgeSearch.Rows.Add(dtChildrenAge);
                        }
                    }

                    dbRequestParamModel.bIsAirConditioning = false;
                    dbRequestParamModel.bIsBathtub = false;
                    dbRequestParamModel.bIsFlatScreenTV = false;
                    dbRequestParamModel.bIsSoundproof = false;
                    dbRequestParamModel.bIsView = false;
                    dbRequestParamModel.bIsInternetFacilities = false;
                    dbRequestParamModel.bIsPrivatePool = false;
                    dbRequestParamModel.bIsRoomService = false;
                    dbRequestParamModel.dMinPrice = 0;
                    dbRequestParamModel.dMaxPrice = 0;
                    dbRequestParamModel.SpecialDeal = true;

                    #endregion


                    // var ofrServiceChargeModel = BL_Bidding.GetBidMaster(CurrencyCode);
                    //ViewData["dOFRServiceCharge"] = Convert.ToDecimal(ofrServiceChargeModel.dOFRServiceCharge);


                    var property_With_RoomList = BL_PropDetails.GetRoomsOfFHotel(dbRequestParamModel, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, false);

                    property_With_RoomList.iTotalDays = (dbRequestParamModel.dtCheckOut - dbRequestParamModel.dtCheckIn).TotalDays.ToString();
                    property_With_RoomList.iPropId = dbRequestParamModel.iPropId;
                    property_With_RoomList.sRequestType = sRequestType;
                    property_With_RoomList.sCheckInDate = sCheckIn;
                    property_With_RoomList.sCheckOutDate = sCheckOut;
                    property_With_RoomList.dtCheckIn = dbRequestParamModel.dtCheckIn;
                    property_With_RoomList.dtCheckOut = dbRequestParamModel.dtCheckOut;
                    ViewData["dOFRServiceCharge"] = (property_With_RoomList.TaxCharges.dOFRServiceCharge + property_With_RoomList.TaxCharges.TaxOnServiceCharge);
                    var propDetailsTempData = "DbRequestModel" + Guid.NewGuid().ToString();
                    var roomCancellationChargesTempData = "RoomCancellationCharges" + Guid.NewGuid().ToString();
                    property_With_RoomList.TempDataPropDetails = propDetailsTempData;
                    property_With_RoomList.TempDataOldRooms = tempdataOldRooms;
                    property_With_RoomList.TempDataRoomCancellationCharges = roomCancellationChargesTempData;
                    Session[propDetailsTempData] = dbRequestParamModel;
                    return PartialView("_HotelRooms", property_With_RoomList);
                }

                return PartialView("Error");
            }

            catch (Exception)
            {
                return PartialView("Error");
            }
        }

        [Route("UpdateGuestBookingDetails/{bookingId}", Name = "UpdateGuestBookingDetails")]
        public ActionResult UpdateGuestBookingDetails(string bookingId)
        {
            ViewBag.HeaderBarData = "Modify Booking";
            int iBookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails obj = new BookingGuestDetails();
            obj = BL_PropDetails.GetBookingDetailsForGuestsRoomsInfo(iBookingId);
            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            obj.objBooking.Birthday = Convert.ToBoolean(obj.bBirthday);
            obj.objBooking.Anniversary = Convert.ToBoolean(obj.bAnniversary);
            obj.objBooking.Honeymoon = Convert.ToBoolean(obj.bHoneymoon);
            if (obj.objBooking.iCounterOffer == null)
                obj.objBooking.iCounterOffer = 0;


            return View(obj);
        }

        [Route("UpdateGuestBookingDetailsAmmend/{bookingId}", Name = "UpdateGuestBookingDetailsAmmend")]
        public ActionResult UpdateGuestBookingDetailsAmmend(string bookingId)
        {
            WebsiteUser userDetails = null;
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Modify Booking";
            int iBookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails obj = new BookingGuestDetails();
            obj = BL_PropDetails.GetBookingDetailsForGuestsRoomsInfo(iBookingId);
            var val = obj.objBooking;
            //var bookingDetail = obj.objBooking;
            //if (bookingDetail.iCustomerId != null && bookingDetail.iCustomerId.HasValue)

            //{
            //    userDetails = BL_WebsiteUser.GetSingleRecordById(bookingDetail.iCustomerId.Value);
            //}
            //else
            //{
            //    userDetails = BL_NegotiationBooking.GetGuestUserDetailById(bookingDetail.iGuestId.Value);
            //}
            Status = obj.objBooking.BookingStatus;
            CCType = obj.objBooking.ccType;

            //Redirecting the pages 
            if (Status == "HR" || Status == "CO" || Status == "FO")
            {

                if (CCType == "CA" || CCType == "BA" || CCType == "RA")
                {
                    //Balance Payment
                    return RedirectToAction("BalancePayment", new { bookingId = bookingId });
                }
                else if (CCType == "CR" || CCType == "RR" || CCType == "FO")
                {
                    //Negotiation Status
                    return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
                else
                {
                    //Negotiation Status
                    return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
            }
            else if (Status == "BP" || Status == "NA" || Status == "CA" || Status == "NS" || Status == "FA" || Status == "BA")
            {
                //Balance Payment
                return RedirectToAction("BalancePayment", new { bookingId = bookingId });
            }
            else if (Status == "RM")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Status == "LT")
            {
                //Booking Confirmation through linked id
                long lbookingid = BL_Booking.GetBookingIdByLinkedId(Convert.ToInt64(obj.objBooking.iLinkedBookingId));
                string id = OneFineRateAppUtil.clsUtils.Encode(lbookingid.ToString());
                return RedirectToRoute("BookingConfirmation", new { bookingId = id });

            }
            else if ((Status.Trim() == "MR" || Status.Trim() == "MD") && obj.objBooking.PaymentStatus.Trim() == "P")
            {
                return RedirectToAction("PayNow", "Payment", new { bookingId = obj.objBooking.iBookingId });
            }


            obj.objBooking.Birthday = Convert.ToBoolean(obj.bBirthday);
            obj.objBooking.Anniversary = Convert.ToBoolean(obj.bAnniversary);
            obj.objBooking.Honeymoon = Convert.ToBoolean(obj.bHoneymoon);
            if (obj.objBooking.iCounterOffer == null)
                obj.objBooking.iCounterOffer = 0;

            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            return View(obj);
        }

        [HttpPost]
        public ActionResult UpdateGuestInformation(BookingGuestDetails obj)
        {
            try
            {
                etblBookingTrakerTx objTrck = new etblBookingTrakerTx();
                List<etblBookingGuestMap> lst = new List<etblBookingGuestMap>();
                List<etblBookingDetailsTx> lsttblBookDetails = new List<etblBookingDetailsTx>();
                string EmailIds = "";
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
                                sGuestName = !string.IsNullOrEmpty(Convert.ToString(item["gname"])) ? Convert.ToString(item["gname"]) : null,
                                sGuestEmail = !string.IsNullOrEmpty(Convert.ToString(item["gemail"])) ? Convert.ToString(item["gemail"]) : null,
                                sGuestMobile = !string.IsNullOrEmpty(Convert.ToString(item["gphone"])) ? Convert.ToString(item["gphone"]) : null,
                                sBedPreference = !string.IsNullOrEmpty(Convert.ToString(item["ptype"])) ? Convert.ToString(item["ptype"]) : null,
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                sCountryPhoneCode = !string.IsNullOrWhiteSpace(Convert.ToString(item["gphone"])) ? Convert.ToString(item["countryPhoneCode"].ToString()) : null,
                                dtActionDate = DateTime.Now
                            });
                            emailIdList.Add(item["gemail"].ToString());
                        }
                    }
                }

                EmailIds = string.Join(",", emailIdList.Distinct());
                var lastIndexEmailIds = EmailIds.LastIndexOf(',');
                EmailIds = lastIndexEmailIds != -1 ? EmailIds.Remove(lastIndexEmailIds, 1) : EmailIds;

                StringBuilder oostr = new StringBuilder();
                if (obj.objBooking.Anniversary)
                    oostr.Append("Anniversary,");

                if (obj.objBooking.Birthday)
                    oostr.Append("Birthday,");

                if (obj.objBooking.Honeymoon)
                    oostr.Append("Honeymoon,");

                string SpecialOccasion = oostr.ToString();
                if (SpecialOccasion != string.Empty)
                    SpecialOccasion = SpecialOccasion.TrimEnd(',');

                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.sSpecialOccasion = SpecialOccasion;

                objTrck.BookingStatus = "P";
                objTrck.dtActionDate = DateTime.Now;
                objTrck.iBookingId = obj.objBooking.iBookingId;

                lsttblBookDetails = obj.lsttblBookDetails;

                int j = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);

                if (j > 0)
                {
                    #region  GST Info Update

                    if (obj.IsGuestBooking)
                    {
                        var guest = BL_WebsiteUser.GetGuestRecordById(obj.CustomerId);
                        guest.GstnEntityName = obj.GstnEntityName;
                        guest.GstnEntityType = obj.GstnEntityType;
                        guest.GstnNumber = obj.GstnNumber;
                        if (obj.GstnState.HasValue)
                            guest.iStateId = obj.GstnState;
                        BL_WebsiteUser.UpdateGuestRecord(guest);
                    }
                    else
                    {
                        var customer = BL_WebsiteUser.GetSingleRecordById(obj.CustomerId);
                        customer.GstnEntityName = obj.GstnEntityName;
                        customer.GstnEntityType = obj.GstnEntityType;
                        customer.GstnNumber = obj.GstnNumber;
                        if (obj.GstnState.HasValue)
                            customer.StateId = obj.GstnState;
                        BL_WebsiteUser.UpdateRecord(customer);
                    }

                    #endregion
                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(obj.objBooking.iPropId.Value);

                    string sPrimaryContactEmail;
                    string sConfirmationContactEmail;
                    string sRevenueManagerEmail;
                    string sRevenueManagerContact;

                    pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                    pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

                    List<string> emailList = new List<string>();
                    emailList.Add(sConfirmationContactEmail);
                    emailList.Add(sRevenueManagerEmail);
                    emailList.Add(sPrimaryContactEmail);

                    string emails = string.Join(",", emailIdList.Distinct().ToList());
                    var lastIndex = emails.LastIndexOf(',');
                    emails = lastIndex != -1 ? emails.Remove(lastIndex, 1) : emails;
                    var customerModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(obj.objBooking.iBookingId));
                    if (obj.objBooking.cBookingType == "C")
                    {
                        customerModel.CompanyName = BL_Booking.GetCompanyNameByUserEmail(User.Identity.GetUserId<long>());
                    }
                    var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                    #region Revenue Manager

                    var paraxisAdmin = BL_tblEmailSettingsM.GetRecord("RevenueManager");
                    var revenuManagerModel = new NegotiationEmailTempleteModel();
                    revenuManagerModel.IsRevenueManagerFormat = true;
                    revenuManagerModel.BookingModify = customerModel;

                    revenuManagerModel.BookingModify.Comm = Math.Round(Convert.ToDecimal(revenuManagerModel.BookingModify.Comm), 2).ToString();
                    revenuManagerModel.BookingModify.sCurrencyCode = "INR";
                    revenuManagerModel.BookingModify.Symbol = "₹";
                    var html_RevenueManager = this.RenderViewToString("_BookingConfirmationTemplateRM", revenuManagerModel.BookingModify);

                    MailComponent.SendEmail(sRevenueManagerEmail, "", paraxisAdmin.sEmail, "OneFineRate- Modification! Confirmation No:" + obj.objBooking.iBookingId, html_RevenueManager, null, null, true, null, null);
                    MailComponent.SendEmail(sConfirmationContactEmail, "", paraxisAdmin.sEmail, "OneFineRate- Modification! Confirmation No:" + obj.objBooking.iBookingId, html_RevenueManager, null, null, true, null, null);

                    string extranetUrl = System.Configuration.ConfigurationManager.AppSettings["OFRExtranetBaseUrl"] + "Bookings/ModifyBooking?=" + clsUtils.Encode(obj.objBooking.iPropId.ToString());

                    string shortExtranetUrl = clsUtils.Shorten(extranetUrl);

                    string notificationSMS_Mngr = "Your Booking Id: " + obj.objBooking.iBookingId + " is now amended. Please view the amendment by clicking the link :" + shortExtranetUrl;

                    var status1 = clsUtils.sendSMS(sRevenueManagerContact, notificationSMS_Mngr);

                    #endregion Revenue Manager


                    #region For Hotel

                    var revenuHotelModel = new NegotiationEmailTempleteModel();
                    customerModel.IsForHotel = true;
                    revenuManagerModel.BookingModify = customerModel;

                    revenuManagerModel.BookingModify = customerModel;

                    revenuManagerModel.BookingModify.sCurrencyCode = "INR";
                    revenuManagerModel.BookingModify.Symbol = "₹";
                    var html_Hotel = this.RenderViewToString("_BookingConfirmationTemplateRM", revenuManagerModel.BookingModify);

                    MailComponent.SendEmail(sPrimaryContactEmail, "", paraxisAdmin.sEmail, "OneFineRate- Modification! Confirmation No:" + obj.objBooking.iBookingId, html_Hotel, null, null, true, null, null);

                    #endregion For Hotel

                    #region User
                    customerModel.Symbol = "₹";
                    customerModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerModel);
                    Task.Run(() => MailComponent.SendEmail(EmailIds, "", "", "OneFineRate- New Booking! Confirmation No:" + obj.objBooking.iBookingId, html_Customer, null, null, true, null, null));

                    string confirmationLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/BookingConfirmation/" + clsUtils.Encode(customerModel.BookingId);
                    string shortComfirmationLink = clsUtils.Shorten(confirmationLink);

                    string message = "New Booking at OFR! Your Booking no. is  " + customerModel.BookingId + " , " + customerModel.CheckIn + " to " + customerModel.ChekOut + " for " + customerModel.NoOfRooms + " rooms." +
                        "A detailed confirmation has been sent to your registered email address. You can also review it on " + shortComfirmationLink +
                        ".You save more on One Fine Rate. Tell your friends about OFR and save even more in your next transaction with us.";
                    Task.Run(() => clsUtils.sendSMS(customerModel.MobileOFR, message));
                    #endregion


                    return RedirectToRoute("BookingConfirmation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(obj.objBooking.iBookingId)) });
                }

            }
            catch (Exception)
            {
                return View("UpdateGuestBookingDetails", obj);
            }

            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            return View("UpdateGuestBookingDetails", obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateGuestInformationAmmend(BookingGuestDetails obj)
        {
            try
            {
                obj.objBooking.BookingStatus = "MD";
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
                                sGuestName = !string.IsNullOrEmpty(Convert.ToString(item["gname"])) ? Convert.ToString(item["gname"]) : null,
                                sGuestEmail = !string.IsNullOrEmpty(Convert.ToString(item["gemail"])) ? Convert.ToString(item["gemail"]) : null,
                                sGuestMobile = !string.IsNullOrEmpty(Convert.ToString(item["gphone"])) ? Convert.ToString(item["gphone"]) : null,
                                sBedPreference = !string.IsNullOrEmpty(Convert.ToString(item["ptype"])) ? Convert.ToString(item["ptype"]) : null,
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                sCountryPhoneCode = !string.IsNullOrWhiteSpace(Convert.ToString(item["gphone"])) ? Convert.ToString(item["countryPhoneCode"].ToString()) : null,

                                dtActionDate = DateTime.Now
                            });
                            emailIdList.Add(item["gemail"].ToString());
                        }
                    }
                }

                guestEmailIds = string.Join(",", emailIdList.Where(x => !string.IsNullOrEmpty(x)).Select(x => x).Distinct().ToList());
                StringBuilder oostr = new StringBuilder();
                if (obj.objBooking.Anniversary)
                    oostr.Append("Anniversary,");

                if (obj.objBooking.Birthday)
                    oostr.Append("Birthday,");

                if (obj.objBooking.Honeymoon)
                    oostr.Append("Honeymoon,");

                string SpecialOccasion = oostr.ToString();
                if (SpecialOccasion != string.Empty)
                    SpecialOccasion = SpecialOccasion.TrimEnd(',');

                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.sSpecialOccasion = SpecialOccasion;


                objTrck.BookingStatus = "P";
                objTrck.dtActionDate = DateTime.Now;
                objTrck.iBookingId = obj.objBooking.iBookingId;

                lsttblBookDetails = obj.lsttblBookDetails;

                int j = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);
                obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

                if (j > 0)
                {

                    var customerModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(obj.objBooking.iBookingId));
                    customerModel.cBookingStatus = (customerModel.cBookingStatus != "MD" && customerModel.sExtra4 == "N" || customerModel.cBookingStatus != "MR" && customerModel.sExtra4 == "N") ? "MD" : customerModel.cBookingStatus;
                    customerModel.BookingStatus = (customerModel.cBookingStatus != "MD" && customerModel.sExtra4 == "N" || customerModel.cBookingStatus != "MR" && customerModel.sExtra4 == "N") ? "Booking Modified" : customerModel.BookingStatus;

                    if (obj.objBooking.cBookingType == "C")
                    {
                        customerModel.CompanyName = BL_Booking.GetCompanyNameByUserEmail(User.Identity.GetUserId<long>());
                    }

                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(obj.objBooking.iPropId.Value);

                    string sPrimaryContactEmail;
                    string sConfirmationContactEmail;
                    string sRevenueManagerEmail;
                    string sRevenueManagerContact;

                    pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                    pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

                    List<string> emailList = new List<string>();
                    emailList.Add(sConfirmationContactEmail);
                    emailList.Add(sRevenueManagerEmail);
                    emailList.Add(sPrimaryContactEmail);

                    string revenuManagerEmails = string.Join(",", emailList.Distinct().ToList());

                    var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                    //string confirmationLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/BookingConfirmation/" + clsUtils.Encode(customerModel.BookingId);


                    #region For Hotel

                    var paraxisAdmin = BL_tblEmailSettingsM.GetRecord("RevenueManager");
                    var revenuManagerModel = new NegotiationEmailTempleteModel();
                    customerModel.IsForHotel = true;
                    revenuManagerModel.BookingModify = customerModel;
                    revenuManagerModel.BookingModify.sCurrencyCode = "INR";
                    revenuManagerModel.BookingModify.Symbol = "₹";
                    var html_Hotel = this.RenderViewToString("_BookingConfirmationTemplateRM", revenuManagerModel.BookingModify);

                    string extranetUrl = System.Configuration.ConfigurationManager.AppSettings["OFRExtranetBaseUrl"] + "Bookings/ModifyBooking?=" + clsUtils.Encode(obj.objBooking.iPropId.ToString());

                    string short_extranetUrl = clsUtils.Shorten(extranetUrl);

                    string notificationSMS_Mngr = "Your Booking Id: " + obj.objBooking.iBookingId + " is now amended. Please view the amendment by clicking the link :" + short_extranetUrl;

                    var status1 = clsUtils.sendSMS(sRevenueManagerContact, notificationSMS_Mngr);

                    MailComponent.SendEmail(revenuManagerEmails, "", paraxisAdmin.sEmail, "OneFineRate-Modification! Confirmation No:" + obj.objBooking.iBookingId, html_Hotel, null, null, true, null, null);

                    #endregion For Hotel

                    #region User

                    if (Convert.ToDecimal(customerModel.RefundAmount) > 0)
                    {
                        var refundModel = new eRefundModel();
                        refundModel.CustomerName = customerModel.Booker;
                        refundModel.ConfirmationNumber = Convert.ToString(obj.objBooking.iBookingId);
                        refundModel.RefundAmount = customerModel.RefundAmount.ToString();
                        var todayDate = DateTime.Now;
                        refundModel.RefundInitiationDate = string.Concat(todayDate.ToString("ddd"), ", ", todayDate.ToString("dd"), " ", todayDate.ToString("MMM"), "'", todayDate.ToString("yy"));
                        var html_Refund_Details = this.RenderViewToString("_RefundEmailTemplate", refundModel);
                        MailComponent.SendEmail(guestEmailIds, "", "", "OneFineRate-Refund! Confirmation No:" + obj.objBooking.iBookingId, html_Refund_Details, null, null, true, null, null);
                        string notificationMsg_User_With_Refund = "Your Booking ID: " + customerModel.BookingId + " is now amended. INR " + customerModel.RefundAmount + " is being refunded to you.";
                        var status = clsUtils.sendSMS(customerModel.MobileOFR, notificationMsg_User_With_Refund);
                    }
                    else
                    {
                        string notificationSMS_User = "Your Booking ID: " + obj.objBooking.iBookingId + " is now amended.";
                        Task.Run(() => clsUtils.sendSMS(obj.objBooking.sMobileOFR, notificationSMS_User));
                    }
                    customerModel.Symbol = "₹";
                    customerModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerModel);
                    MailComponent.SendEmail(guestEmailIds, "", "", "OneFineRate-Modification! Confirmation No: " + obj.objBooking.iBookingId, html_Customer, null, null, true, null, null);

                    //Sending Invoice
                    SendInvoiceAfterGuestInformationUpdate(Convert.ToInt32(obj.objBooking.iBookingId));
                    if (!BL_Booking.UpdateSyncStatusChannelMgr(obj.objBooking.iBookingId, null))
                        throw new Exception();
                    #endregion
                    
                    return RedirectToRoute("ModifyConfirmation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(obj.objBooking.iBookingId)) });
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("UpdateGuestBookingDetails", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(obj.objBooking.iBookingId)) });
            }

            return View("UpdateGuestBookingDetailsAmmend", obj);
        }

        [Route("ModifyConfirmation/{bookingId}", Name = "ModifyConfirmation")]
        public ActionResult ModifyConfirmation(string bookingId)
        {
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Confirmation";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var BookingModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(bookId));
            BookingModel.Symbol = "₹";
            Status = BookingModel.cBookingStatus;
            CCType = BookingModel.ccType;


            if (BookingModel.cBookingType == "B")
            {
                var rateInclusions = BL_Booking.GetBidRateInclusions(bookId);

                foreach (var item in BookingModel.BookingRoomDetails)
                {
                    item.AmenityRatePlan = rateInclusions;
                }
            }

            //Redirecting the pages 
            if (Status == "HR" || Status == "CO" || Status == "FO")
            {

                if (CCType == "CA" || CCType == "BA" || CCType == "RA")
                {
                    //Balance Payment
                    return RedirectToAction("BalancePayment", new { bookingId = bookingId });
                }
                else if (CCType == "CR" || CCType == "RR" || CCType == "FO")
                {
                    //Negotiation Status
                    return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
                else
                {
                    //Negotiation Status
                    return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
            }
            else if (Status == "BP" || Status == "NA" || Status == "CA" || Status == "NS" || Status == "BA")
            {
                //Balance Payment
                return RedirectToAction("BalancePayment", new { bookingId = bookingId });
            }
            else if (Status == "RM")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Status == "LT")
            {
                //Booking Confirmation through linked id
                long lbookingid = BL_Booking.GetBookingIdByLinkedId(BookingModel.iLinkedBookingId);
                string id = OneFineRateAppUtil.clsUtils.Encode(lbookingid.ToString());
                //return RedirectToRoute("BookingConfirmation", new { bookingId = id });
                BookingModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(lbookingid));
            }

            return View(BookingModel);
        }
        public ActionResult UpdateBookingDetails(string bookingId)
        {
            ViewBag.HeaderBarData = "Modify Booking";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            return View();
        }

        /// <summary>
        /// Cancellation of existing booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="bookingDetailsIds"></param>
        /// <returns></returns>
        public ActionResult CancelBooking(string bookingId, string bookingDetailsIds)
        {
            try
            {
                int bookingId_decoded = 0;

                try
                {
                    bookingId_decoded = Convert.ToInt32(clsUtils.Decode(bookingId));
                }
                catch (Exception)
                {
                    bookingId_decoded = Convert.ToInt32(bookingId);
                }

                string sTimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(sTimeZone.Replace(":", ".").Replace("+", ""));

                DataTable dtTblBookingDetailsIds = new DataTable();
                dtTblBookingDetailsIds.Columns.Add(new DataColumn("bookingDetailsId", typeof(int)));


                if (!string.IsNullOrEmpty(bookingDetailsIds))
                {
                    string[] bookingDetailIdArr = bookingDetailsIds.Split(',');

                    for (int i = 0; i < bookingDetailIdArr.Length; i++)
                    {
                        DataRow dtTblBookingDetailRow = dtTblBookingDetailsIds.NewRow();
                        dtTblBookingDetailRow["bookingDetailsId"] = bookingDetailIdArr[i];
                        dtTblBookingDetailsIds.Rows.Add(dtTblBookingDetailRow);
                    }
                }


                var cancelModel = BL_Booking.CancelBooking(Convert.ToInt64(bookingId_decoded), zone, dtTblBookingDetailsIds);
                cancelModel.Refund = System.Math.Abs(cancelModel.Refund);

                //TO DO
                var bookingForNotification = BL_Booking.GetBookingModifyDetails(bookingId_decoded);

                if (cancelModel != null)
                {
                    // get from propertyEdit page 
                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(bookingForNotification.PropId);

                    string sPrimaryContactEmail;
                    string sConfirmationContactEmail;
                    string sRevenueManagerEmail;
                    string sRevenueManagerContact;

                    pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                    pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

                    List<string> emailList = new List<string>();
                    emailList.Add(sRevenueManagerEmail);
                    emailList.Add(sConfirmationContactEmail);
                    emailList.Add(sPrimaryContactEmail);
                    var paraxisAdmin = BL_tblEmailSettingsM.GetRecord("RevenueManager");
                    string hotelEmails = string.Join(",", emailList.Distinct().ToList());

                    if (cancelModel.Refund > 0)
                    {
                        string notificationMsg_User_With_Refund = "Your Booking ID: " + bookingForNotification.BookingId + " is now cancelled. INR " + cancelModel.Refund + " is being refunded to you.";
                        clsUtils.sendSMS(bookingForNotification.MobileOFR, notificationMsg_User_With_Refund);
                    }
                    else
                    {
                        string notificationMsg_User = "Your Booking ID: " + bookingForNotification.BookingId + " is now cancelled.";
                        clsUtils.sendSMS(bookingForNotification.MobileOFR, notificationMsg_User);
                    }

                    #region OFR Admin
                    string notificationMsg_RevenueMgr = "Cancellation! Booking id : " + bookingForNotification.BookingId + "  from " + bookingForNotification.CheckIn + " to " + bookingForNotification.ChekOut + " for " + cancelModel.RoomsCancelled + (cancelModel.RoomsCancelled > 1 ? " rooms." : " room.");

                    clsUtils.sendSMS(sRevenueManagerContact, notificationMsg_RevenueMgr);
                    cancelModel.IsFormatForRevenueMgr = true;

                    var html_CancellationDetails_RevenueMgr = this.RenderViewToString("_CancellationEmailTemplate", cancelModel);
                    MailComponent.SendEmail(sRevenueManagerEmail, "", paraxisAdmin.sEmail, "OneFineRate-Cancellation! Confirmation No:" + bookingId_decoded, html_CancellationDetails_RevenueMgr, null, null, true, null, null);

                    #endregion OFR Admin

                    #region For Hotel

                    cancelModel.IsFormatForRevenueMgr = false;
                    cancelModel.IsFormatForHotel = true;
                    cancelModel.CancelChargesHotel = Math.Abs(cancelModel.CancellationCharges);
                    var html_CancellationDetails_Hotel = this.RenderViewToString("_CancellationEmailTemplate", cancelModel);
                    Task.Run(() => MailComponent.SendEmail(sConfirmationContactEmail, "", sRevenueManagerEmail, "OneFineRate-Cancellation! Confirmation No:" + bookingId_decoded, html_CancellationDetails_Hotel, null, null, true, null, null));

                    #endregion For Hotel

                    #region For User

                    bookingForNotification.CancellationCharge = cancelModel.CancellationCharges.ToString();
                    bookingForNotification.RefundAmount = cancelModel.Refund.ToString();

                    // Send confirmation mail to user (New Changes June)
                    bookingForNotification.sCurrencyCode = "INR";
                    bookingForNotification.Symbol = "₹";
                    var html_CancellationDetails_User = this.RenderViewToString("_CancelConfirmationTemplate_User", bookingForNotification);
                    MailComponent.SendEmail(bookingForNotification.EmailOFR, "", "", "OneFineRate-Cancellation! Confirmation No:" + bookingId_decoded, html_CancellationDetails_User, null, null, true, null, null);

                    //Added to send seperate email in case of Refund (New Changes June)
                    if (cancelModel.Refund != 0)
                    {
                        var refundModel = new eRefundModel();
                        refundModel.CustomerName = cancelModel.CustName;
                        refundModel.ConfirmationNumber = cancelModel.BookingId;
                        refundModel.RefundAmount = cancelModel.Refund.ToString();
                        var todayDate = DateTime.Now;
                        refundModel.RefundInitiationDate = string.Concat(todayDate.ToString("ddd"), ", ", todayDate.ToString("dd"), " ", todayDate.ToString("MMM"), "'", todayDate.ToString("yy"));
                        var html_Refund_Details = this.RenderViewToString("_RefundEmailTemplate", refundModel);
                        MailComponent.SendEmail(bookingForNotification.EmailOFR, "", "", "OneFineRate-Refund! Confirmation No:" + bookingId_decoded, html_Refund_Details, null, null, true, null, null);
                    }

                    #endregion

                    //Send Invoice if Partial cancellation

                    if (cancelModel.Status.ToLower() == "md" || cancelModel.Status.ToLower() == "mc")
                    {
                        SendInvoiceAfterGuestInformationUpdate(Convert.ToInt32(bookingForNotification.BookingId));
                    }

                    TempData["msg"] = "The Booking has been cancelled !";
                }

                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Booking", "Manage");

                //return RedirectToAction("Modify", new { bookingId = bookingId });
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult GetCancellationCharge(long bookingId, string bookingDetailsIds)
        {
            try
            {
                DataTable dtTblBookingDetailsIds = new DataTable();
                dtTblBookingDetailsIds.Columns.Add(new DataColumn("bookingDetailsId", typeof(int)));

                if (!string.IsNullOrEmpty(bookingDetailsIds))
                {
                    string[] bookingDetailIdArr = bookingDetailsIds.Split(',');

                    for (int i = 0; i < bookingDetailIdArr.Length; i++)
                    {
                        DataRow dtTblBookingDetailRow = dtTblBookingDetailsIds.NewRow();
                        dtTblBookingDetailRow["bookingDetailsId"] = bookingDetailIdArr[i];
                        dtTblBookingDetailsIds.Rows.Add(dtTblBookingDetailRow);
                    }
                }

                string sTimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(sTimeZone.Replace(":", ".").Replace("+", ""));
                eCancelBookingResponse cancellationResponse = BL_Booking.GetCancellationCharge(bookingId, zone, dtTblBookingDetailsIds);
                return Json(new { cancellationCharge = clsUtils.ConvertNumberToCommaSeprated(cancellationResponse.CancellationCharges), policyName = cancellationResponse.CancellationPolicy, refund = cancellationResponse.Refund }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return PartialView("Error");
            }
        }

        #endregion Public Methods

        #region Helper Method Non Action

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

        private int SaveBooking(PropDetailsM propDetail)
        {
            int bookingIdFromDbIdentity = 0;
            try
            {
                Decimal? exchangeRate = 1;
                etblBookingTrakerTx bookingTracker = new etblBookingTrakerTx();
                List<etblBookingDetailsTx> lstBookingDetails = new List<etblBookingDetailsTx>();
                List<etblBookingCancellationPolicyMap> lstCancellationPolicy = new List<etblBookingCancellationPolicyMap>();
                List<etblBookedDayWiseTaxAmountDetails> lstDayWiseTaxes = new List<etblBookedDayWiseTaxAmountDetails>();

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

                if (propDetail.objBooking.sCurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",propDetail.objBooking.sCurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        exchangeRate = 1/objExchange.dRate;
                    }
                }

                propDetail.objBooking.dTotalAmount = propDetail.dSummaryRoomRate * exchangeRate;
                propDetail.objBooking.dTaxes = propDetail.dSummaryTaxes * exchangeRate;
                propDetail.objBooking.dTaxesForHotel = propDetail.dSummaryTaxes * exchangeRate;
                propDetail.objBooking.dTotalExtraBedAmount = propDetail.dSummaryExtraBedCharges * exchangeRate;


                string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                propDetail.objBooking.iCountryOffset = zone;


                decimal Rate = propDetail.dCommissionRate;
                if (Rate != 0)
                {
                    decimal Comm = (propDetail.dSummaryRoomRate + propDetail.dSummaryExtraBedCharges) * Rate / 100;
                    propDetail.objBooking.dTotalComm = Comm * exchangeRate;
                }

                propDetail.objBooking.cBookingType = "R";

                bookingTracker.BookingStatus = "PP";
                bookingTracker.dtActionDate = DateTime.Now;

                var roomDataResult = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<OneFineRateBLL.Entities.RoomData>>(propDetail.sRoomData);

                var roomDetail = propDetail.TG_Hotel.RoomDetails.Where(x => x.RoomId == propDetail.sRoomId).FirstOrDefault();

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
                        bookingDetail.dRoomRate = ratePlan.TotalRoomRate;
                        bookingDetail.dExtraBedRate = ratePlan.TotalExtraBedCharge;
                        bookingDetail.sAmenityRatePlan = ratePlan.RateInclusions;
                        bookingDetail.iAdults = (short)roomdata.adult;
                        bookingDetail.iChildren = (short)roomdata.child;
                        bookingDetail.iExtraBeds = (short)roomDetail.ExtraBedCont;
                        bookingDetail.sChildrenAge = string.Join(",", roomdata.ChildAge);
                        bookingDetail.dTaxes = ratePlan.TotalTax;
                        bookingDetail.dtActionDate = DateTime.Now;

                        lstBookingDetails.Add(bookingDetail);

                        var bookingCancellation = new etblBookingCancellationPolicyMap();

                        bookingCancellation.sPolicyName = ratePlan.CancellationPolicyDescription;
                        bookingCancellation.dtActionDate = DateTime.Now;
                        bookingCancellation.iRPId = ratePlan.RatePlanCode;

                        lstCancellationPolicy.Add(bookingCancellation);
                    }
                }

                bookingIdFromDbIdentity = BL_Booking.AddBooking(propDetail, null, bookingTracker, lstBookingDetails, null, lstCancellationPolicy, lstDayWiseTaxes, null);
            }
            catch (Exception)
            {
                bookingIdFromDbIdentity = 0;
            }

            return bookingIdFromDbIdentity;
        }

        #endregion Helper Method Non Action

        public void SendInvoiceAfterGuestInformationUpdate(long bookingId)
        {
            //TO DO
            var ccMail = string.Empty;
            ccMail = "himanshuS@futuresoftindia.com";

            var bookingDetails = BL_Invoice.GetInvoiceDetailByBooking(bookingId);

            bookingDetails.sInvoiceNumber = bookingDetails.iBookingId + "/" + "01" + ((bookingDetails.cBookingStatus.ToLower() == "md" || bookingDetails.cBookingStatus.ToLower() == "mc") ? "/MOD" : "");
            bookingDetails.HotelOrGuest = HotelOrGuest.Guest;

            var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(bookingDetails.iPropId);

            string sPrimaryContactEmail;
            string sConfirmationContactEmail;
            string sRevenueManagerEmail;
            string sRevenueManagerContact;

            pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
            pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
            pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
            pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

            List<string> emailList = new List<string>();
            emailList.Add(sRevenueManagerEmail);
            emailList.Add(sConfirmationContactEmail);
            emailList.Add(sPrimaryContactEmail);

            string emails = string.Join(",", emailList.Distinct().ToList());

            var html_Customer = this.RenderViewToString("_Invoice", bookingDetails);

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(html_Customer);
            var attachment_Customer = new Attachment(new MemoryStream(pdfBytes), "Invoice#" + bookingDetails.iBookingId + ".pdf");

            bookingDetails.sInvoiceNumber = bookingDetails.iBookingId + "/" + "02" + ((bookingDetails.cBookingStatus.ToLower() == "md" || bookingDetails.cBookingStatus.ToLower() == "mc") ? "/MOD" : "");
            bookingDetails.HotelOrGuest = HotelOrGuest.Hotel;

            var html_Hotel = this.RenderViewToString("_Invoice", bookingDetails);
            var htmlToPdf_Hotel = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes_Hotel = htmlToPdf.GeneratePdf(html_Hotel);
            var attachment_Hotel = new Attachment(new MemoryStream(pdfBytes_Hotel), "Invoice#" + bookingDetails.iBookingId + ".pdf");

            Task.Run(() =>
            {
                MailComponent.SendEmail(sRevenueManagerEmail, ccMail, "", "OneFineRate! Invoice# :" + bookingDetails.iBookingId, html_Hotel, attachment_Hotel, null, true, new MemoryStream(pdfBytes_Hotel), "Invoice#" + bookingDetails.iBookingId + ".pdf");
                MailComponent.SendEmail(bookingDetails.sEmailOFR, ccMail, "", "OneFineRate! Invoice# :" + bookingDetails.iBookingId, html_Customer, attachment_Customer, null, true, new MemoryStream(pdfBytes), "Invoice#" + bookingDetails.iBookingId + ".pdf");

            });
        }
    }
}