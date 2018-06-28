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
using OneFineRateApp.App_Helper;
using System.Configuration;
using SelectPdf;
using System.IO;
using System.Net.Mail;

namespace OneFineRateApp.Controllers.Bookings
{
    public class BookingsController : BaseController
    {
        [Route("Bookings")]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            eBookingSearch objebookingsearch = new eBookingSearch();
            return View(objebookingsearch);
        }

        public ActionResult ModifyBooking(string Booking)
        {
            int bookingId_Decoded = Convert.ToInt32(Booking);

            var bookingModel = BL_Booking.GetBookingModifyDetails(bookingId_Decoded);

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

            TempData["OldRooms"] = bookingModel;
            TempData.Keep();

            return View("BookingModify", bookingModel);
        }

        public ActionResult BookingsView()
        {
            eBookingSearch objebookingsearch = new eBookingSearch();
            return View("BookingsView", objebookingsearch);
        }

        [Authorize]
        [ValidateInput(false)]
        public ActionResult GetCanellationCharges(PropDetailsM propDetail)
        {
            object result = null;
            try
            {
                var lstBookdetails = new List<eBookingRoomM>();
                var lstDeletedBookingdetails = new List<int>();
                var lstAllData_Temp = new eBookingModify();
                var lstAllData = new eBookingModify();
                lstAllData = TempData["OldRooms"] as eBookingModify;

                lstAllData_Temp = TempData["OldRooms"] as eBookingModify;
                if (TempData["OldRooms"] != null)
                {
                    TempData.Keep();


                    //if (TempData["OldRooms_Temp"] != null) { TempData.Keep(); lstAllData_Temp = TempData["OldRooms"] as eBookingModify; }

                    lstBookdetails = lstAllData_Temp.BookingRoomDetails;


                    foreach (var item in lstBookdetails)
                    {
                        item.IsActive = true;
                    }



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
                                        var lst = lstBookdetails.Where(u => u.Occupancy == Occupancy.ToString() && u.IsActive == true).ToList();
                                        if (lst.Count > 0)
                                        {
                                            var firstRecord = lst.FirstOrDefault();
                                            //lstBookdetails.Remove(firstRecord);
                                            firstRecord.IsActive = false;

                                            DataRow dr = dtBookingDetails.NewRow();
                                            dr["iBDId"] = Convert.ToInt32(firstRecord.iBookingDetailsId);
                                            dr["iRoomId"] = Convert.ToString(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iRoomId);
                                            dr["iRPId"] = Convert.ToInt32(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].RPID);
                                            dr["iOccupancy"] = Occupancy;
                                            dr["dAvgRoomRateWithExtraBed"] = Convert.ToDecimal(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dBasePrice);
                                            dr["dAvgTaxes"] = Convert.ToDecimal(propDetail.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes);
                                            dr["iAdults"] = Convert.ToInt16(firstRecord.Adult);
                                            dr["iChildren"] = Convert.ToInt16(firstRecord.Children);
                                            dr["sChildrenAge"] = Convert.ToString(firstRecord.sChildrenAge);
                                            dtBookingDetails.Rows.Add(dr);

                                            lstDeletedBookingdetails.Add(Convert.ToInt32(firstRecord.iBookingDetailsId));
                                        }
                                    }
                                }
                            }
                        }

                    }
                    propDetail.lstDeletedBookingDetails = lstDeletedBookingdetails;

                    //Get Time Zone for this booking
                    string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                    decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                    decimal? Offset = zone;


                    bool bType = true;
                    if (propDetail.sRequestType.Trim().ToLower() == "modifyrate")
                    {
                        bType = false;
                    }

                    BookingAmedCancelation objCharges = BL_Booking.GetAmendCacellationCharges(BookingId, dtBookingDetails, Convert.ToDecimal(Offset), clsUtils.ConvertddmmyyyytoDateTime(propDetail.sCheckInDate), clsUtils.ConvertddmmyyyytoDateTime(propDetail.sCheckOutDate), bType);
                    if (objCharges.lstRoomCharges.Count > 0)
                    {
                        result = new { st = 1, msg = "Cancellation charges applicable.", lstdata = OneFineRateAppUtil.clsUtils.ConvertToJson(objCharges.lstRoomCharges), Totalcharges = objCharges.TotalCanellationCharges, symbol = lstAllData_Temp.Symbol };
                        propDetail.dTotalCancellationCharges = objCharges.TotalCanellationCharges;
                        TempData["RoomCancelationCharges"] = objCharges.lstBookingRoomCharges;
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
            TempData["propDetails"] = propDetail;
            TempData.Keep();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ModificationReview()

        {
            ViewBag.HeaderBarData = "Preview";
            var propDetail = new PropDetailsM();
            var lstAllData = new eBookingModify();
            var lstRoomCancelCharges = new List<eBookingAmendCancelBookingRoomIdWiseCharges>();
            try
            {


                if (TempData["OldRooms"] != null) { TempData.Keep(); lstAllData = TempData["OldRooms"] as eBookingModify; }



                if (TempData["propDetails"] != null) { TempData.Keep(); propDetail = TempData["propDetails"] as PropDetailsM; }

                //feteched room cancellation charges booking details id wise
                if (TempData["RoomCancelationCharges"] != null) { TempData.Keep(); lstRoomCancelCharges = TempData["RoomCancelationCharges"] as List<eBookingAmendCancelBookingRoomIdWiseCharges>; }


                propDetail.dOFRServiceCharge = Convert.ToDecimal(BL_Bidding.GetBidMaster(lstAllData.sCurrencyCode).dOFRServiceCharge);

                propDetail.BookingRoomDetails = lstAllData.BookingRoomDetails;
                propDetail.dTotalAmountPaid = Convert.ToDecimal(lstAllData.SubTotal);




                propDetail.scheckIn = String.Format("{0:dddd MMM d yyyy}", propDetail.sCheckInDate);
                propDetail.scheckOut = String.Format("{0:dddd MMM d yyyy}", propDetail.sCheckOutDate);
                propDetail.sExtra1 = lstAllData.sExtra1;

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



                if (dTotal > 0)
                {
                    decimal? PromoDiscount = 0;
                    string PromoCode = lstAllData.sExtra1;
                    if (!String.IsNullOrEmpty(PromoCode))
                    {
                        string[] Arr;
                        if (PromoCode.Contains("%"))
                        {
                            Arr = PromoCode.Split('%');
                            decimal amt = Convert.ToDecimal(Arr[0]);

                            PromoDiscount = (dTotal * amt) / 100;
                        }
                        else
                        {
                            string[] stringSeparators = new string[] { "Val" };
                            Arr = PromoCode.Split(stringSeparators, StringSplitOptions.None);

                            decimal amt = Convert.ToDecimal(Arr[0]);

                            PromoDiscount = (dTotal) - amt;
                        }
                        propDetail.dDiscountedBidPrice = Convert.ToDecimal(PromoDiscount);
                    }
                }
            }
            catch
            {

                return View("Error");
            }
            return View(propDetail);
        }


        public ActionResult UpdateBooking(PropDetailsM objOldData)
        {
            var obj = new PropDetailsM();
            var lstRoomCancelCharges = new List<eBookingAmendCancelBookingRoomIdWiseCharges>();
            var lstAllData = new eBookingModify();

            try
            {
                Decimal? ExchangeRate = 1, dTotalAmount = 0, dTaxes = 0, dTotalExtraBedAmount = 0;
                int? Days = 0;

                //feteched left booking details rooms which were not modified
                if (TempData["OldRooms"] != null) { TempData.Keep(); lstAllData = TempData["OldRooms"] as eBookingModify; }

                //feteched fresh booking details rooms
                if (TempData["propDetails"] != null) { TempData.Keep(); obj = TempData["propDetails"] as PropDetailsM; }

                //feteched room cancellation charges booking details id wise
                if (TempData["RoomCancelationCharges"] != null) { TempData.Keep(); lstRoomCancelCharges = TempData["RoomCancelationCharges"] as List<eBookingAmendCancelBookingRoomIdWiseCharges>; }


                //fetch the data of booking and booking object created
                etblBookingTx objBook = BL_Booking.GetBooking(Convert.ToInt64(lstAllData.BookingId));


                //Type of Amendment


                if (obj.sRequestType != "ModifyRate")
                {
                    objBook.dtCheckIn = clsUtils.ConvertddmmyyyytoDateTime(obj.sCheckInDate);
                    objBook.dtChekOut = clsUtils.ConvertddmmyyyytoDateTime(obj.sCheckOutDate);

                }

                string CurrencyCode = Session["CurrCode"] != null ? Session["CurrCode"].ToString() : "INR";

                //decimal OFRServiceTax = Convert.ToDecimal(BL_Bidding.GetBidMaster(CurrencyCode != null ? CurrencyCode.ToString() : "INR").dOFRServiceCharge);
                decimal OFRServiceTax = obj.TaxCharges.dOFRServiceCharge + obj.TaxCharges.TaxOnServiceCharge;
                Days = Convert.ToInt32((Convert.ToDateTime(objBook.dtChekOut) - Convert.ToDateTime(objBook.dtCheckIn)).TotalDays);

                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", obj.objBooking.sCurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1 / objExchange.dRate;
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


                for (int i = 0; i < lstAllData.BookingRoomDetails.Where(u => u.IsActive == true).ToList().Count; i++)
                {
                    decimal extrabedcharges = Convert.ToDecimal(lstAllData.BookingRoomDetails[i].dExtraBedRate);
                    decimal total = extrabedcharges;
                    decimal RoomAmt = Convert.ToDecimal(lstAllData.BookingRoomDetails[i].RoomRate) * Convert.ToDecimal(ExchangeRate);

                    dTotalAmount += (RoomAmt) * Days;
                    dTaxes += Convert.ToDecimal(lstAllData.BookingRoomDetails[i].dTaxes) * ExchangeRate;
                    dTotalExtraBedAmount = total * Days;
                }

                decimal CustomerTaxes = Convert.ToDecimal(dTaxes) + Convert.ToDecimal(OFRServiceTax);

                decimal AmtPaid = Convert.ToDecimal(obj.dTotalAmountPaid);
                decimal CancelAmt = Convert.ToDecimal(obj.dTotalCancellationCharges);
                decimal Balance = Convert.ToDecimal(dTotalAmount + dTaxes) - (Convert.ToDecimal(AmtPaid) - CancelAmt);
                objBook.dRefundAmount = Balance;
                objBook.cRefundStatus = "P";
                if (Balance < 0)
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
                    objBook.sOAuth = objOldData.sAuthCode;
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
                    // objBook.dRefundAmount = null;
                    //objBook.cRefundStatus = null;
                }

                objBook.dTotalAmount = dTotalAmount - dTotalExtraBedAmount;
                objBook.dTaxes = CustomerTaxes;
                objBook.dTaxesOriginal = CustomerTaxes;
                objBook.dTaxesForHotel = dTaxes;
                objBook.dTotalExtraBedAmount = dTotalExtraBedAmount;
                objBook.dCustomerPayable = dTotalAmount;
                objBook.dDiscountedBidPrice = obj.dDiscountedBidPrice;

                decimal Rate = obj.dCommissionRate;
                if (Rate != 0)
                {
                    decimal Comm = (Convert.ToDecimal(dTotalAmount) * Rate) / 100;
                    objBook.dTotalComm = Comm * ExchangeRate;
                    objBook.dTotalCommOriginal = Comm * ExchangeRate;
                }

                int result = BL_Booking.Amendments_UpdateBooking(objBook, objBooking_Amend, lstBookDetails, lstCancelPolicy, lstDayTaxes, obj.lstDeletedBookingDetails, lstRoomCancelCharges, lstDayTaxesDateWise);

                if (result == 1)
                {
                    //Update Datewise booking rate 
                    Task.Run(() => BL_Booking.UpdateDaywiseBookingRate(Convert.ToInt32(objBook.iBookingId)));

                    TempData["RoomCancelationCharges"] = null;
                    TempData["OldRooms"] = null;
                    TempData["propDetails"] = null;
                    TempData["msg"] = "Room modified successfully";
                    string EmailIds = "";
                    if (Balance < 0)
                    {
                        BookingGuestDetails obja = new BookingGuestDetails();
                        obja = BL_PropDetails.GetBookingDetailsForGuestsRoomsInfo(Convert.ToInt32(objBook.iBookingId));
                        etblBookingTrakerTx objTrack = new etblBookingTrakerTx();
                        List<etblBookingGuestMap> lst = new List<etblBookingGuestMap>();
                        List<etblBookingDetailsTx> lsttblBookDetails = new List<etblBookingDetailsTx>();

                        List<string> emailIdList = new List<string>();

                        if (obja.GuestData != null)
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
                                        iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                        // iRPId = Convert.ToInt32(item["planid"]),
                                        dtActionDate = DateTime.Now
                                    });
                                    emailIdList.Add(item["gemail"].ToString());
                                    //EmailIds += Convert.ToString(item["gemail"]) + ",";
                                }
                            }
                        }

                        //EmailIds += obj.objBooking.sEmailOFR + ",";
                        //if (EmailIds != "")
                        //{
                        //    EmailIds = EmailIds.TrimEnd(',');
                        //}

                        EmailIds = string.Join(",", emailIdList.Distinct());
                        var lastIndex = EmailIds.LastIndexOf(',');
                        EmailIds = lastIndex != -1 ? EmailIds.Remove(lastIndex, 1) : EmailIds;
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

                        //Object created for tblBookingTx
                        obja.objBooking.dtActionDate = DateTime.Now;
                        obja.objBooking.sSpecialOccasion = SpecialOccasion;



                        //Object created for tblBookingTrakerTx
                        //objTrck.BookingStatus = "PP";
                        //objTrck.dtActionDate = DateTime.Now;
                        //objTrck.iBookingId = obj.objBooking.iBookingId;

                        //lsttblBookDetails = obja.lsttblBookDetails;


                        //int j = BL_Booking.UpdateBooking_AddGuestInformation(obja.objBooking, lst, objTrck, lsttblBookDetails);
                        // return RedirectToAction("Index");
                    }
                    else
                    {
                        var customerModel = new NegotiationEmailTempleteModel();
                        var html_Customer = "";
                        var booking = BL_Booking.GetBooking(objBook.iBookingId);
                        //etblBookingTrakerTx objtrk = new etblBookingTrakerTx();
                        //objtrk.iBookingId = objBook.iBookingId;
                        //objtrk.dtActionDate = DateTime.Now;
                        //objtrk.BookingStatus = booking.BookingStatus;
                        booking.PaymentStatus = "C";
                        booking.dtActionDate = DateTime.Now;
                        var Result = BL_Booking.UpdateBookingStatus(booking, null);
                        var bookingModel = BL_Booking.GetBookingModifyDetails(objBook.iBookingId);
                        customerModel.BookingModify = bookingModel;
                        customerModel.Status = "" + bookingModel.Booker + "! Thanks for initiating the payment. Click the link below to continue." + Environment.NewLine + "";
                        var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                        customerModel.CallbackUrl = websiteBaseUrl + "UpdateGuestBookingDetailsAmmend/" + clsUtils.Encode(objBook.iBookingId.ToString());
                        html_Customer = this.RenderViewToString("_EmailTemplete", customerModel);
                        Task.Run(() => MailComponent.SendEmail(bookingModel.EmailOFR, "", "", "Payment Initialization", html_Customer, null, null, true, null, null));


                    }

                    var customerModifyModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(objBook.iBookingId));
                    var guestEmailIds = string.Join(",", BL_Booking.GetBookingGuestDetails(objBook.iBookingId));
                    customerModifyModel.cBookingStatus = (customerModifyModel.cBookingStatus != "MD" && customerModifyModel.sExtra4 == "N" || customerModifyModel.cBookingStatus != "MR" && customerModifyModel.sExtra4 == "N") ? "MD" : customerModifyModel.cBookingStatus;
                    customerModifyModel.BookingStatus = (customerModifyModel.cBookingStatus != "MD" && customerModifyModel.sExtra4 == "N" || customerModifyModel.cBookingStatus != "MR" && customerModifyModel.sExtra4 == "N") ? "Booking Modified" : customerModifyModel.BookingStatus;

                    if (objBook.cBookingType == "C")
                    {
                        customerModifyModel.CompanyName = BL_Booking.GetCompanyNameByUserEmail(0, customerModifyModel.CompanyId);
                    }

                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(objBook.iPropId.Value);

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

                    // var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                    //string confirmationLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/BookingConfirmation/" + clsUtils.Encode(customerModel.BookingId);

                    #region Revenue Manager

                    var revenueManagerDetail = BL_tblEmailSettingsM.GetRecord("RevenueManager");
                    var revenuManagerModel = new NegotiationEmailTempleteModel();
                    revenuManagerModel.IsRevenueManagerFormat = true;
                    revenuManagerModel.BookingModify = customerModifyModel;
                    revenuManagerModel.BookingModify.sCurrencyCode = "INR";
                    revenuManagerModel.BookingModify.Symbol = "₹";
                    revenuManagerModel.BookingModify.Comm = Math.Round(Convert.ToDecimal(revenuManagerModel.BookingModify.Comm), 2).ToString();
                    var html_RevenueManager = this.RenderViewToString("_BookingConfirmationTemplateRM", revenuManagerModel.BookingModify);

                    MailComponent.SendEmail(sRevenueManagerEmail, "", revenueManagerDetail.sEmail, "OneFineRate-Modification! Confirmation No:" + objBook.iBookingId, html_RevenueManager, null, null, true, null, null);
                    // confirmation/ammendment/cancellation
                    MailComponent.SendEmail(sConfirmationContactEmail, "", revenueManagerDetail.sEmail, "OneFineRate-Modification! Confirmation No:" + objBook.iBookingId, html_RevenueManager, null, null, true, null, null);

                    string extranetUrl = System.Configuration.ConfigurationManager.AppSettings["OFRBaseUrl"] + "Bookings/ModifyBooking?=" + clsUtils.Encode(objBook.iPropId.ToString());

                    string notificationSMS_Mngr = "Your Booking Id: " + objBook.iBookingId + " is now amended. Please view the amendment by clicking the link :" + extranetUrl;

                    var status1 = clsUtils.sendSMS(sRevenueManagerContact, notificationSMS_Mngr);

                    #endregion Revenue Manager

                    #region For Hotel

                    var revenuHotelModel = new NegotiationEmailTempleteModel();
                    customerModifyModel.IsForHotel = true;
                    revenuManagerModel.BookingModify = customerModifyModel;

                    revenuManagerModel.BookingModify = customerModifyModel;
                    revenuManagerModel.BookingModify.sCurrencyCode = "INR";
                    revenuManagerModel.BookingModify.Symbol = "₹";
                    var html_Hotel = this.RenderViewToString("_BookingConfirmationTemplateRM", revenuManagerModel.BookingModify);

                    MailComponent.SendEmail(sPrimaryContactEmail, "", revenueManagerDetail.sEmail, "OneFineRate-Modification! Confirmation No:" + objBook.iBookingId, html_Hotel, null, null, true, null, null);

                    #endregion For Hotel

                    #region User

                    if (Convert.ToDecimal(customerModifyModel.RefundAmount) != 0)
                    {
                        var refundModel = new eRefundModel();
                        refundModel.CustomerName = customerModifyModel.Booker;
                        refundModel.ConfirmationNumber = customerModifyModel.BookingId;
                        refundModel.RefundAmount = customerModifyModel.RefundAmount.ToString();
                        var todayDate = DateTime.Now;
                        refundModel.RefundInitiationDate = string.Concat(todayDate.ToString("ddd"), ", ", todayDate.ToString("dd"), " ", todayDate.ToString("MMM"), "'", todayDate.ToString("yy"));
                        var html_Refund_Details = this.RenderViewToString("_RefundEmailTemplate", refundModel);
                        Task.Run(() => MailComponent.SendEmail(guestEmailIds, "", "", "OneFineRate-Refund! Confirmation No:" + objBook.iBookingId, html_Refund_Details, null, null, true, null, null));
                        string notificationMsg_User_With_Refund = "Your Booking ID: " + customerModifyModel.BookingId + " is now amended. INR " + customerModifyModel.RefundAmount + " is being refunded to you.";
                        var status = clsUtils.sendSMS(customerModifyModel.MobileOFR, notificationMsg_User_With_Refund);
                    }
                    else
                    {
                        string notificationSMS_User = "Your Booking ID: " + objBook.iBookingId + " is now amended";
                        Task.Run(() => clsUtils.sendSMS(objBook.sMobileOFR, notificationSMS_User));
                    }
                    customerModifyModel.Symbol = "₹";
                    customerModifyModel.sCurrencyCode = "INR";
                    var html_EmailCustomer = this.RenderViewToString("_BookingEmailTemplate", customerModifyModel);
                    MailComponent.SendEmail(guestEmailIds, "", "", "OneFineRate-Modification! Confirmation No: " + objBook.iBookingId, html_EmailCustomer, null, null, true, null, null);

                    #endregion

                    return RedirectToAction("Index");
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


        public ActionResult GetHotelRooms(string propId, string sCheckIn, string sCheckOut, string sRoomData, string sRequestType, string cBookingType)
        {
            try
            {
                string CurrencyCode = Session["CurrCode"] != null ? Session["CurrCode"].ToString() : "INR";

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
                        dbRequestParamModel.dtCheckIn = clsUtils.ConvertddmmyyyytoDateTime(sCheckIn);
                        dbRequestParamModel.dtCheckOut = clsUtils.ConvertddmmyyyytoDateTime(sCheckOut);
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


                    //var ofrServiceChargeModel = BL_Bidding.GetBidMaster(CurrencyCode);

                    //ViewData["dOFRServiceCharge"] = Convert.ToDecimal(ofrServiceChargeModel.dOFRServiceCharge);

                    var property_With_RoomList = BL_PropDetails.GetRoomsOfFHotel(dbRequestParamModel, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, cBookingType == "C");

                    property_With_RoomList.iTotalDays = (dbRequestParamModel.dtCheckOut - dbRequestParamModel.dtCheckIn).TotalDays.ToString();
                    property_With_RoomList.iPropId = dbRequestParamModel.iPropId;
                    property_With_RoomList.sRequestType = sRequestType;
                    property_With_RoomList.sCheckInDate = sCheckIn;
                    property_With_RoomList.sCheckOutDate = sCheckOut;
                    ViewData["dOFRServiceCharge"] = (property_With_RoomList.TaxCharges.dOFRServiceCharge + property_With_RoomList.TaxCharges.TaxOnServiceCharge);
                    TempData["DbRequestModel"] = dbRequestParamModel;

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

            obj.objBooking.Birthday = Convert.ToBoolean(obj.bBirthday);
            obj.objBooking.Anniversary = Convert.ToBoolean(obj.bAnniversary);
            obj.objBooking.Honeymoon = Convert.ToBoolean(obj.bHoneymoon);
            if (obj.objBooking.iCounterOffer == null)
                obj.objBooking.iCounterOffer = 0;

            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            return View(obj);
        }


        [Route("UpdateGuestBookingDetailsAmmend/{bookingId}", Name = "UpdateGuestBookingDetailsAmmend")]
        public ActionResult UpdateGuestBookingDetailsAmmend(string bookingId)
        {
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Modify Booking";
            int iBookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails obj = new BookingGuestDetails();
            obj = BL_PropDetails.GetBookingDetailsForGuestsRoomsInfo(iBookingId);

            Status = obj.objBooking.BookingStatus;
            CCType = obj.objBooking.ccType;

            if (Status == "HR" || Status == "CO" || Status == "FO")
            {

                if (CCType == "CA" || CCType == "BA" || CCType == "RA")
                {
                    return RedirectToAction("BalancePayment", new { bookingId = bookingId });
                }
                else if (CCType == "CR" || CCType == "RR" || CCType == "FO")
                {
                    return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
                else
                {
                    return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
            }
            else if (Status == "BP" || Status == "NA" || Status == "CA" || Status == "NS" || Status == "FA" || Status == "BA")
            {
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
                //return RedirectToRoute("BookingConfirmation", new { bookingId = id });
                return RedirectToAction("Index");

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
                                sGuestName = Convert.ToString(item["gname"]),
                                sGuestEmail = Convert.ToString(item["gemail"]),
                                sGuestMobile = Convert.ToString(item["gphone"]),
                                sBedPreference = Convert.ToString(item["ptype"]),
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                // iRPId = Convert.ToInt32(item["planid"]),
                                dtActionDate = DateTime.Now
                            });
                            emailIdList.Add(item["gemail"].ToString());
                            //EmailIds += Convert.ToString(item["gemail"]) + ",";
                        }
                    }
                }

                //EmailIds += obj.objBooking.sEmailOFR + ",";
                //if (EmailIds != "")
                //{
                //    EmailIds = EmailIds.Replace(",,", ",");
                //    EmailIds = EmailIds.TrimEnd(',');
                //}

                EmailIds = string.Join(",", emailIdList.Distinct());

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

                //Object created for tblBookingTx
                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.sSpecialOccasion = SpecialOccasion;



                //Object created for tblBookingTrakerTx
                objTrck.BookingStatus = "P";
                objTrck.dtActionDate = DateTime.Now;
                objTrck.iBookingId = obj.objBooking.iBookingId;

                lsttblBookDetails = obj.lsttblBookDetails;


                int j = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);
                if (j > 0)
                {
                    var customerModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(obj.objBooking.iBookingId));

                    if (obj.objBooking.cBookingType == "C")
                    {
                        customerModel.CompanyName = "Company Name";
                    }

                    var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                    customerModel.Symbol = "₹";
                    customerModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerModel);
                    Task.Run(() => MailComponent.SendEmail(EmailIds, "", "", "OneFineRate- New Booking! Confirmation No. : " + obj.objBooking.iBookingId, html_Customer, null, null, true, null, null));

                    string confirmationLink = websiteBaseUrl + "/BookingConfirmation/" + clsUtils.Encode(customerModel.BookingId);

                    //Send SMS
                    string message = "New Booking at OFR! Your Booking no. is  " + customerModel.BookingId + " , " + customerModel.CheckIn + " to " + customerModel.ChekOut + " for " + customerModel.NoOfRooms + " rooms." +
                        "A detailed confirmation has been sent to your registered email address. You can also review it on " + confirmationLink +
                        " .You save more on One Fine Rate. Tell your friends about OFR and save even more in your next transaction with us.";
                    Task.Run(() => clsUtils.sendSMS(customerModel.MobileOFR, message));

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


        [HttpPost]
        public ActionResult UpdateGuestInformationAmmend(BookingGuestDetails obj)
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
                                sGuestName = Convert.ToString(item["gname"]),
                                sGuestEmail = Convert.ToString(item["gemail"]),
                                sGuestMobile = Convert.ToString(item["gphone"]),
                                sBedPreference = Convert.ToString(item["ptype"]),
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
                                // iRPId = Convert.ToInt32(item["planid"]),
                                dtActionDate = DateTime.Now
                            });
                            emailIdList.Add(item["gemail"].ToString());
                            //EmailIds += Convert.ToString(item["gemail"]) + ",";
                        }
                    }
                }

                //EmailIds += obj.objBooking.sEmailOFR + ",";
                //if (EmailIds != "")
                //{
                //    EmailIds = EmailIds.TrimEnd(',');
                //}

                EmailIds = string.Join(",", emailIdList.Distinct());

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

                //Object created for tblBookingTx
                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.sSpecialOccasion = SpecialOccasion;



                //Object created for tblBookingTrakerTx
                objTrck.BookingStatus = "P";
                objTrck.dtActionDate = DateTime.Now;
                objTrck.iBookingId = obj.objBooking.iBookingId;

                lsttblBookDetails = obj.lsttblBookDetails;


                int j = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);
                if (j > 0)
                {
                    var customerModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(obj.objBooking.iBookingId));

                    if (obj.objBooking.cBookingType == "C")
                    {
                        customerModel.CompanyName = "Company Name";
                    }

                    var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                    customerModel.Symbol = "₹";
                    customerModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerModel);
                    Task.Run(() => MailComponent.SendEmail(EmailIds, "", "", "OneFineRate- Modification! Confirmation No. : " + obj.objBooking.iBookingId, html_Customer, null, null, true, null, null));
                    return RedirectToRoute("BookingConfirmation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(obj.objBooking.iBookingId)) });
                }

            }
            catch (Exception)
            {
                return View("UpdateGuestBookingDetailsAmmend", obj);
            }

            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();


            return View("UpdateGuestBookingDetailsAmmend", obj);
        }


        public ActionResult UpdateBookingDetails(string bookingId)
        {
            ViewBag.HeaderBarData = "Modify Booking";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));
            return View();
        }


        public ActionResult CancelBooking(string bookingId, string bookingDetailsIds)
        {
            try
            {
                int bookingId_decoded = Convert.ToInt32(clsUtils.Decode(bookingId));

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
                var bookingModel = BL_Booking.CancelBooking(Convert.ToInt64(bookingId_decoded), zone, dtTblBookingDetailsIds);
                bookingModel.Refund = System.Math.Abs(bookingModel.Refund);

                var bookingForNotification = BL_Booking.GetBookingModifyDetails(bookingId_decoded);//newly added aditya

                if (bookingModel != null)
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

                    if (bookingModel.Refund != 0)
                    {
                        string notificationMsg_User_With_Refund = "Your Booking ID: " + bookingForNotification.BookingId + " is now cancelled. INR " + bookingModel.Refund + " is being refunded to you.";
                        Task.Run(() => clsUtils.sendSMS(bookingForNotification.MobileOFR, notificationMsg_User_With_Refund));
                    }
                    else
                    {
                        string notificationMsg_User = "Your Booking ID: " + bookingForNotification.BookingId + " is now cancelled.";
                        Task.Run(() => clsUtils.sendSMS(bookingForNotification.MobileOFR, notificationMsg_User));
                    }

                    string notificationMsg_RevenueMgr = "Cancellation! Booking id : " + bookingForNotification.BookingId + "  from " + bookingForNotification.CheckIn + " to " + bookingForNotification.ChekOut + " for " + bookingModel.RoomsCancelled + (bookingModel.RoomsCancelled != 1 && bookingModel.RoomsCancelled != 0 ? " rooms." : " room.");

                    Task.Run(() => clsUtils.sendSMS(sRevenueManagerContact, notificationMsg_RevenueMgr));

                    #region Revenue Manager And Admin (OFR)

                    bookingModel.IsFormatForRevenueMgr = true;
                    var html_CancellationDetails_AdminOfr = this.RenderViewToString("_CancellationEmailTemplate", bookingModel);

                    bookingModel.IsFormatForHotel = true;
                    bookingModel.IsFormatForRevenueMgr = false;
                    var html_CancellationDetails_Hotel = this.RenderViewToString("_CancellationEmailTemplate", bookingModel);

                    // SEnd TO OFR admin
                    Task.Run(() => MailComponent.SendEmail(paraxisAdmin.sEmail, "", "", "OneFineRate-Cancellation! Confirmation No:" + bookingId_decoded, html_CancellationDetails_AdminOfr, null, null, true, null, null));

                    // confirmation/ammendment/cancellation
                    Task.Run(() => MailComponent.SendEmail(hotelEmails, "", paraxisAdmin.sEmail, "OneFineRate-Cancellation! Confirmation No:" + bookingId_decoded, html_CancellationDetails_Hotel, null, null, true, null, null));

                    #endregion Reveneue Manager                    

                    #region For User


                    bookingForNotification.CancellationCharge = bookingModel.CancellationCharges.ToString();
                    bookingForNotification.RefundAmount = bookingModel.Refund.ToString();


                    // Send confirmation mail to user (New Changes June)
                    bookingForNotification.sCurrencyCode = "INR";
                    bookingForNotification.Symbol = "₹";
                    var html_CancellationDetails_User = this.RenderViewToString("_CancelConfirmationTemplate_User", bookingForNotification);
                    Task.Run(() => MailComponent.SendEmail(bookingForNotification.EmailOFR, "", "", "OneFineRate –Cancellation! Confirmation No. : " + bookingId_decoded, html_CancellationDetails_User, null, null, true, null, null));

                    //Added to send seperate email in case of Refund (New Changes June)
                    if (bookingModel.Refund != 0)
                    {
                        var refundModel = new eRefundModel();
                        refundModel.CustomerName = bookingModel.CustName;
                        refundModel.ConfirmationNumber = bookingModel.BookingId;
                        refundModel.RefundAmount = Math.Abs(bookingModel.Refund).ToString();
                        var todayDate = DateTime.Now;
                        refundModel.RefundInitiationDate = string.Concat(todayDate.ToString("ddd"), ", ", todayDate.ToString("dd"), " ", todayDate.ToString("MMM"), "'", todayDate.ToString("yy"));
                        var html_Refund_Details = this.RenderViewToString("_RefundEmailTemplate", refundModel);
                        Task.Run(() => MailComponent.SendEmail(bookingForNotification.EmailOFR, "", "", "OneFineRate-Refund! Confirmation No:" + bookingId_decoded, html_Refund_Details, null, null, true, null, null));
                    }

                    #endregion

                    //Send Invoice if Partial cancellation

                    if (bookingModel.Status.ToLower() == "md" || bookingModel.Status.ToLower() == "mc")
                    {
                        SendInvoiceAfterGuestInformationUpdate(Convert.ToInt32(bookingForNotification.BookingId));
                    }

                    TempData["msg"] = "The Booking has been cancelled !";
                }

                //return RedirectToAction("Modify", new { bookingId = bookingId });
                return RedirectToAction("Index");
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
                eCancelBookingResponse response = BL_Booking.GetCancellationCharge(bookingId, zone, dtTblBookingDetailsIds);
                return Json(new { cancellationCharge = clsUtils.ConvertNumberToCommaSeprated(response.CancellationCharges), cancellationPolicy = response.CancellationPolicy, refund = response.Refund }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return PartialView("Error");
            }
        }


        [Route("BookingConfirmation/{bookingId}", Name = "BookingConfirmation")]
        public ActionResult BookingConfirmation(string bookingId)
        {
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Confirmation";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var BookingModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(bookId), true);

            Status = BookingModel.cBookingStatus;
            CCType = BookingModel.ccType;

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
                long lbookingid = BL_Booking.GetBookingIdByLinkedId(BookingModel.iLinkedBookingId);
                string id = OneFineRateAppUtil.clsUtils.Encode(lbookingid.ToString());
                //return RedirectToRoute("BookingConfirmation", new { bookingId = id });
                BookingModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(lbookingid));
            }
            BookingModel.sCurrencyCode = "INR";
            BookingModel.Symbol = "₹";
            return View(BookingModel);
        }

        public ActionResult RoomInfo(string propid, string roomid)
        {
            string CurrencyCode = Session["CurrCode"] != null ? Session["CurrCode"].ToString() : "INR";
            RoomDetails objRoomDetails = BL_PropDetails.GetRoomDetails(Convert.ToInt32(propid), Convert.ToInt32(roomid), CurrencyCode);
            return PartialView("pvRoomInfo", objRoomDetails);
        }

        #region Helper Method Non Action



        private string ResendVerification(string phone)
        {
            try
            {
                var phoneVerificationCode = new Random().Next(100000, 999999);
                var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                clsUtils.sendSMS(phone, "Your OTP verification code is " + phoneVerificationCode);
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


        #endregion Helper Method Non Action


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="type">h or g , h - hotel , g- guest</param>
        /// <returns></returns>
        public ActionResult GetInvoice(string bookingId, string type)
        {
            var encodedBookingId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var invoideDetail = BL_Invoice.GetInvoiceDetailByBooking(encodedBookingId);

            if (invoideDetail != null)
            {
                if (type == "h") // Hotel
                {
                    invoideDetail.sInvoiceNumber = invoideDetail.iBookingId.ToString() + "/" + "02" + ((invoideDetail.cBookingStatus == "MD" || invoideDetail.cBookingStatus == "MC") ? "/MOD" : "");
                    invoideDetail.HotelOrGuest = HotelOrGuest.Hotel;
                }
                else // Guest
                {
                    invoideDetail.sInvoiceNumber = invoideDetail.iBookingId.ToString() + "/" + "01" + ((invoideDetail.cBookingStatus == "MD" || invoideDetail.cBookingStatus == "MC") ? "/MOD" : "");
                    invoideDetail.HotelOrGuest = HotelOrGuest.Guest;
                }
            }

            return PartialView("_Invoice", invoideDetail);
        }


        [HttpPost]
        public ActionResult SendMailAndUpdateTaxAffectedBookings(string iBookingId, string type, string commaSeperatedEmail)
        {
            try
            {
                var encodedBookingId = Convert.ToInt32(iBookingId);

                if (!string.IsNullOrEmpty(commaSeperatedEmail))
                {
                    var invoideDetail = BL_Invoice.GetInvoiceDetailByBooking(encodedBookingId);

                    if (invoideDetail != null)
                    {
                        if (type == "h") // Hotel
                        {
                            invoideDetail.sInvoiceNumber = invoideDetail.iBookingId + "/" + "02" + (invoideDetail.cBookingStatus == "MD" ? "/MOD" : "");
                            invoideDetail.HotelOrGuest = HotelOrGuest.Hotel;
                        }
                        else // Guest
                        {
                            invoideDetail.sInvoiceNumber = invoideDetail.iBookingId + "/" + "01" + (invoideDetail.cBookingStatus == "MD" ? "/MOD" : "");
                            invoideDetail.HotelOrGuest = HotelOrGuest.Guest;
                        }

                        invoideDetail.IsGeneratingPdf = true;

                        var html_Customer = this.RenderViewToString("_InvoiceEmailTemplate", invoideDetail);

                        var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                        var pdfBytes = htmlToPdf.GeneratePdf(html_Customer);
                        var attachment = new Attachment(new MemoryStream(pdfBytes), "Invoice#" + invoideDetail.iBookingId + ".pdf");

                        var ccMail = string.Empty;
                        //ccMail = "himanshuS@futuresoftindia.com";

                        MailComponent.SendEmail(commaSeperatedEmail, ccMail, "", "OneFineRate! Invoice# :" + invoideDetail.iBookingId, html_Customer, attachment, null, true, new MemoryStream(pdfBytes), "Invoice#" + invoideDetail.iBookingId + ".pdf");

                        //HtmlToPdf converter = new HtmlToPdf();

                        //PdfDocument doc = converter.ConvertHtmlString(html_Customer);

                        //using (MemoryStream memoryStream = new MemoryStream())
                        //{
                        //    doc.Save(memoryStream);
                        //    byte[] bytes = memoryStream.ToArray();
                        //    memoryStream.Close();

                        //    var attachment = new Attachment(new MemoryStream(bytes), "Invoice#" + invoideDetail.iBookingId + ".pdf");
                        //    MailComponent.SendEmail(commaSeperatedEmail, "deepaka@futuresoftindia.com", "", "OneFineRate! Invoice# :" + invoideDetail.iBookingId, html_Customer, attachment, null, true, new MemoryStream(bytes), "Invoice#" + invoideDetail.iBookingId + ".pdf");
                        //}

                        //doc.Close();
                    }

                    return Json(new { status = true, message = "Email sent to selected email address successfully!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Please enter at least one email address." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //TO DO
                var ccMail = string.Empty;
                ccMail = "himanshuS@futuresoftindia.com";
                MailComponent.SendEmail(ccMail, ccMail, "", "OneFineRate! PDF ERROR", ex.ToString(), null, null, true, null, "Invoice Error#");
                return Json(new { status = false, message = "An error occurred while sending the email!" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownloadBookingPdf(string bookingId, string type)
        {
            try
            {
                var encodedBookingId = Convert.ToInt32(bookingId);

                if (!string.IsNullOrEmpty(type))
                {
                    var invoideDetail = BL_Invoice.GetInvoiceDetailByBooking(encodedBookingId);

                    if (invoideDetail != null)
                    {
                        if (type == HotelOrGuest.Hotel.ToString()) // Hotel
                        {
                            invoideDetail.sInvoiceNumber = invoideDetail.iBookingId + "/" + "02" + (invoideDetail.cBookingStatus == "MD" ? "/MOD" : "");
                            invoideDetail.HotelOrGuest = HotelOrGuest.Hotel;
                        }
                        else // Guest
                        {
                            invoideDetail.sInvoiceNumber = invoideDetail.iBookingId + "/" + "01" + (invoideDetail.cBookingStatus == "MD" ? "/MOD" : "");
                            invoideDetail.HotelOrGuest = HotelOrGuest.Guest;
                        }

                        invoideDetail.IsGeneratingPdf = true;

                        var html_Customer = this.RenderViewToString("_Invoice", invoideDetail);

                        var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                        var pdfBytes = htmlToPdf.GeneratePdf(html_Customer);

                        // return resulted pdf document 
                        FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf");
                        fileResult.FileDownloadName = "Invoice#" + encodedBookingId + ".pdf";
                        return fileResult;
                    }
                }

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //TO DO
                var ccMail = string.Empty;
                ccMail = "himanshuS@futuresoftindia.com";
                MailComponent.SendEmail(ccMail, ccMail, "", "OneFineRate! PDF ERROR", ex.ToString(), null, null, true, null, "Invoice Error#");
                throw ex;
            }
        }

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