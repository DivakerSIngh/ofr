using Newtonsoft.Json;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using OneFineRateWeb.App_Helper;
using FutureSoft.Util;
using Encription;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace OneFineRateWeb.Controllers.Negotiation
{
   
    public class NegotiationController : BaseController
    {
        public ActionResult Index(string type,string info, string propdetailguid )
        {
          //  string propdetailguid = string.Empty;
         //   string type = string.Empty;
            var stronglObject = new PropDetailsM();
          
            if (TempData["PropDetails"] != null)
            {
                stronglObject = TempData["PropDetails"] as PropDetailsM;
                Session.Add(propdetailguid, stronglObject);
            }
            else if(!string.IsNullOrEmpty(propdetailguid))
            {
                //stronglObject =JsonConvert.DeserializeObject<PropDetailsM>(propdetailguid);
                stronglObject = HttpContext.Session[propdetailguid] as PropDetailsM;
                if (stronglObject==null)
                {
                    return RedirectToActionPermanent("Index", "Home");
                }
            }
            else
            {
             return   RedirectToActionPermanent("Index", "Home");
            }
           
            decimal? NegotiationAmount = 0;
            //string type = "";

            //if (HttpContext.Request.Params["info"] != null)
            //{
            //    NegotiationAmount = !string.IsNullOrEmpty(HttpContext.Request.Params["info"].ToString())? Convert.ToDecimal(HttpContext.Request.Params["info"]):0;
            //}
            //if (HttpContext.Request.Params["type"] != null)
            //{
            //    type =HttpContext.Request.Params["type"];
            //}

            ViewBag.NegotiationAmount = info;
            //ViewBag.NegotiationAmount = NegotiationAmount == 0 ? null : NegotiationAmount;
            ViewBag.type = type == "" ? null : type;

            if (User.Identity.IsAuthenticated)
            {
                var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                stronglObject.sUserTitle = user.Title;
                stronglObject.sUserFirstName = user.FirstName;
                stronglObject.sUserLastName = user.LastName;
                stronglObject.sUserEmail = user.Email;
                stronglObject.sUserMobileNo = user.PhoneNumber;
                stronglObject.iStateId = user.StateId.HasValue ? user.StateId.Value : 0;
            }

            DataTable dtNegotiationRatePlans = new DataTable();
            dtNegotiationRatePlans.Columns.AddRange(new DataColumn[3] { new DataColumn("RatePlan", typeof(int)),
                    new DataColumn("dPrice", typeof(decimal)),
                    new DataColumn("bIsPromo",typeof(bool)) });
            int RatePlan = 0, iNoRoom = 0, iDays = 0, iCount = 0;
            decimal RoomPrice = 0, DTotal = 0, ExtraCharges = 0;
            int ExtraBed = 0;
            bool isPromo = false;


            for (int i = 0; i < stronglObject.lstetblRooms.Count; i++)
            {
                for (int j = 0; j < stronglObject.lstetblRooms[i].lstRatePlan.Count; j++)
                {
                    for (int lstOcc = 0; lstOcc < stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                    {
                        if (stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                        {
                            for (int k = 0; k < stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                            {
                                ExtraBed = stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds;
                                ExtraCharges = stronglObject.lstetblRooms[i].ExtraBedCharges;

                                RatePlan = stronglObject.lstetblRooms[i].lstRatePlan[j].RPID;
                                RoomPrice = stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice;
                                isPromo = stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].blsPromo;

                                iNoRoom = stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms;
                                iDays = Convert.ToInt32(stronglObject.iTotalDays);
                                iCount = iNoRoom * iDays;


                                RoomPrice = RoomPrice * (iDays);
                                ExtraCharges = (ExtraBed * ExtraCharges) * iDays;

                                DTotal += RoomPrice + ExtraCharges;
                                dtNegotiationRatePlans.Rows.Add(RatePlan, RoomPrice, isPromo);


                                iNoRoom++;
                            }
                        }
                    }
                }
            }
            stronglObject.dSummaryTotal = DTotal;
            stronglObject.sSummaryTotal = clsUtils.ConvertNumberToCommaSeprated(DTotal);
            //stronglObject.scheckIn = String.Format("{0:dd MMM}", stronglObject.dtCheckIn);
            //stronglObject.scheckOut = String.Format("{0:dd MMM}", stronglObject.dtCheckOut);
            //stronglObject.scheckInYear = stronglObject.dtCheckIn.Year.ToString();
            //stronglObject.scheckOutYear = stronglObject.dtCheckOut.Year.ToString();
            stronglObject.scheckIn = String.Format("{0:dddd MMM d yyyy}", stronglObject.dtCheckIn);
            stronglObject.scheckOut = String.Format("{0:dddd MMM d yyyy}", stronglObject.dtCheckOut);
            //stronglObject.iTotalDays = (stronglObject.dtCheckOut - stronglObject.dtCheckIn).TotalDays.ToString();
            decimal negoamt = BL_PropDetails.GetNegotiationAmt(dtNegotiationRatePlans);
            stronglObject.dNegotiationAmtMax = negoamt + stronglObject.dSummaryExtraBedCharges;
            stronglObject.sNegotiationAmtMax = clsUtils.ConvertNumberToCommaSeprated(stronglObject.dNegotiationAmtMax);
            stronglObject.iNoOfRooms = iNoRoom;
            stronglObject.TempDataPropDetails = propdetailguid;

            //TempData[propDetailsTempData] = stronglObject;
            //TempData.Keep(propDetailsTempData);
            ViewBag.HeaderBarData = "Preview";
            return View("NegotiationReview", stronglObject);
        }
        public ActionResult BookerInformation()
        {
            var stronglObject = new PropDetailsM();
            if (TempData["propDetails"] != null) { TempData.Keep(); stronglObject = TempData["propDetails"] as PropDetailsM; }
            decimal? NegotiationAmount = 0;
            string type = "";
            if (HttpContext.Request.Params["info"] != null && HttpContext.Request.Params["info"] != "") { NegotiationAmount = Convert.ToDecimal(HttpContext.Request.Params["info"]); }
            if (HttpContext.Request.Params["type"] != null && HttpContext.Request.Params["type"] != "") { type = HttpContext.Request.Params["type"]; }
            ViewBag.NegotiationAmount = NegotiationAmount == 0 ? null : NegotiationAmount;
            ViewBag.type = type == "" ? null : type;


            if (User.Identity.IsAuthenticated)
            {
                var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                stronglObject.sUserTitle = user.Title;
                stronglObject.sUserFirstName = user.FirstName;
                stronglObject.sUserLastName = user.LastName;
                stronglObject.sUserEmail = user.Email;
                stronglObject.sUserMobileNo = user.PhoneNumber;
            }

            return View("BookerInformation", stronglObject);
        }
        public ActionResult VerifyMobileNo(decimal NegotiationAmount, string Title, string FirstName, string LastName, string EmailID, string Mobile)
        {
            var stronglObject = new PropDetailsM();
            if (TempData["propDetails"] != null) { TempData.Keep(); stronglObject = TempData["propDetails"] as PropDetailsM; }


            if (User.Identity.IsAuthenticated)
            {
                var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                stronglObject.sUserTitle = user.Title;
                stronglObject.sUserFirstName = user.FirstName;
                stronglObject.sUserLastName = user.LastName;
                stronglObject.sUserEmail = user.Email;
                stronglObject.sUserMobileNo = user.PhoneNumber;

                if (user.PhoneNumber == Mobile)
                {
                    return Json(new { st = 0, msg = "Mobile No is already verified.Please proceed by clicking on Charge Card" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    string VerificationCode = ResendVerification(Mobile);
                    stronglObject.sVerificationCode = VerificationCode;
                }
            }
            else
            {
                stronglObject.sUserTitle = Title;
                stronglObject.sUserFirstName = FirstName;
                stronglObject.sUserLastName = LastName;
                stronglObject.sUserEmail = EmailID;
                stronglObject.sUserMobileNo = Mobile;

                string VerificationCode = ResendVerification(Mobile);
                stronglObject.sVerificationCode = VerificationCode;
            }

            return Json(new { status = true, VC = stronglObject.sVerificationCode }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChargeCard(decimal NegotiationAmount, string Title, string FirstName, string LastName, string EmailID, string sCountryCode, string Mobile, string iStateId, string VC, string OTPCode, decimal GrandTotal, string promocode, bool promoapplied, decimal promovalue, string promotype, string promoamenity, string promoDescription, decimal dtax, string propDetailsTempData, decimal OfrServiceCharge = 0, decimal GstOfrServiceCharge = 0, string dGSTServiceType = "", string dGSTValue = "")
        {

            var propDetail = new PropDetailsM();
            try
            {
                Decimal? ExchangeRate = 1;
                if (Session[propDetailsTempData] != null)
                {
                    //TempData.Keep(propDetailsTempData);
                    propDetail = Session[propDetailsTempData] as PropDetailsM;
                }
                int result = 0;
                propDetail.dNegotiationAmt = NegotiationAmount;
                propDetail.dSummaryGrandTotal = GrandTotal;

                propDetail.objBooking.sExtra2 = Convert.ToString(dtax);
                propDetail.objBooking.sCountryPhoneCode = sCountryCode;
                if (promoapplied)
                {
                    if (promotype == "Value")
                    {
                        propDetail.objBooking.PromoCodeValue = promovalue;
                        propDetail.objBooking.bPromoDiscount = true;
                    }
                    else
                    {
                        propDetail.objBooking.bPromoDiscount = false;
                        propDetail.objBooking.bPromoAmenity = promoamenity;
                    }
                    propDetail.objBooking.spromotype = promotype;
                    propDetail.objBooking.sPromoCode = promocode;
                    propDetail.objBooking.PromoCodeApplied = true;
                    propDetail.objBooking.sExtra1 = promoDescription;

                }
                else
                {
                    propDetail.objBooking.PromoCodeApplied = false;
                }
                if (propDetail.Currency != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById( "INR", propDetail.Currency);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                    propDetail.sUserTitle = user.Title;
                    propDetail.sUserFirstName = FirstName;
                    propDetail.sUserLastName = LastName;
                    propDetail.sUserEmail = EmailID;
                    propDetail.iUserId = user.Id;
                    propDetail.iStateId = user.StateId.HasValue ? user.StateId.Value : Convert.ToInt32(iStateId);

                    if (OfrServiceCharge != 0)
                        propDetail.objBooking.dServiceCharge = Math.Round(OfrServiceCharge * (ExchangeRate == null ? 1 : ExchangeRate.Value));
                    if (GstOfrServiceCharge != 0)
                        propDetail.objBooking.dGSTOnServiceCharge = Math.Round(GstOfrServiceCharge * (ExchangeRate == null ? 1 : ExchangeRate.Value));
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        propDetail.objBooking.dGSTServiceType = dGSTServiceType;
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        propDetail.objBooking.dGSTValue = dGSTValue;

                    if (user.PhoneNumber == Mobile)
                    {
                        if (!user.StateId.HasValue)
                        {
                            user.StateId = Convert.ToInt32(iStateId);
                            BL_WebsiteUser.UpdateRecord(user);
                        }

                        // Here we need to send Email ??
                        //if (propDetail.objBooking.iBookingId==0)
                        //{
                            result = GetBooking(propDetail);
                           // propDetail.objBooking.iBookingId = result;
                           // Session[propDetailsTempData] = propDetail;
                        //}
                        //else
                        //{
                        //    result =Convert.ToInt32(propDetail.objBooking.iBookingId);
                        //}
                        return Json(new { status = true, reqStatus = "C", result = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (OTPCode != "" && OTPCode != null)
                        {
                            if (DecodeVC(VC) == OTPCode)
                            {
                                propDetail.sUserMobileNo = Mobile;
                                //if (propDetail.objBooking.iBookingId != 0)
                                    result = GetBooking(propDetail);
                                //else
                                //    result = Convert.ToInt32(propDetail.objBooking.iBookingId);
                              //  Session[propDetailsTempData] = null;
                                return Json(new { status = true, reqStatus = "C", result = result }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { st = 0, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            string VerificationCode = ResendVerification(Mobile);
                            propDetail.sVerificationCode = VerificationCode;
                        }
                    }
                }
                else
                {
                    propDetail.sUserTitle = Title;
                    propDetail.sUserFirstName = FirstName;
                    propDetail.sUserLastName = LastName;
                    propDetail.sUserEmail = EmailID;
                    propDetail.sUserMobileNo = Mobile;
                    propDetail.sCountryPhoneCode = sCountryCode;
                    propDetail.iStateId = Convert.ToInt32(iStateId);

                    if (OfrServiceCharge != 0)
                        propDetail.objBooking.dServiceCharge = Math.Round(OfrServiceCharge *(ExchangeRate==null?1: ExchangeRate.Value));
                    if (GstOfrServiceCharge != 0)
                        propDetail.objBooking.dGSTOnServiceCharge = Math.Round(GstOfrServiceCharge * (ExchangeRate == null ? 1 : ExchangeRate.Value));
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        propDetail.objBooking.dGSTServiceType = dGSTServiceType;
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        propDetail.objBooking.dGSTValue = dGSTValue;

                    if (!string.IsNullOrEmpty(VC)&& OTPCode != "" && OTPCode != null)
                    {
                        if (DecodeVC(VC) == OTPCode)
                        {
                            int i = BL_NegotiationBooking.AddRecord(propDetail);
                            if (i > 0)
                            {
                                propDetail.iGuestId = i;
                                //if (propDetail.objBooking.iBookingId==0)
                               // {
                                    result = GetBooking(propDetail);
                                   // propDetail.objBooking.iBookingId = result;
                                //}
                                //else
                                //{
                                //    result = Convert.ToInt32(propDetail.objBooking.iBookingId);
                                //}
                                
                                return Json(new { status = true, reqStatus = "C", result = result }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { st = 0, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string VerificationCode = ResendVerification(Mobile);
                        if (VerificationCode == "error")
                        {
                            return Json(new { st = "0", msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
                        }

                        propDetail.sVerificationCode = VerificationCode;
                    }

                }
            }
            catch (Exception)
            {
                return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { st = 1, status = true, VC = propDetail.sVerificationCode }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// OTP Generation and sending to Mobile using SMS AAPI
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private string ResendVerification(string phone)
        {
            try
            {
                var phoneVerificationCode = clsUtils.GetVerificationCode();
                var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                string message = phoneVerificationCode + " is the One Time Mobile verification code for OneFineRate";
                clsUtils.sendSMS(phone, message);
                return encodedCode;
            }
            catch (Exception)
            {
                return "error";
                throw;
            }
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

                string message = phoneVerificationCode + " is the One Time Mobile verification code for OneFineRate";
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
        //Open Guest Information Page to fill Guest Information
        [Route("GuestInformation/{bookingId}", Name = "GuestInformation")]
        public ActionResult GuestInformation(string bookingId)
        {

            ViewBag.HeaderBarData = "Guest Information";
            int BookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails obj = new BookingGuestDetails();
            obj = BL_PropDetails.GetBookingDetailsForGuests(BookingId);

            //TO DO
            ////Commented by Aditya
            //if (obj.objBooking.bSelfTravelling != null)
            //return RedirectToAction("BookingConfirmation", new { bookingId = bookingId });

            if (obj.objBooking.iCounterOffer == null)
                obj.objBooking.iCounterOffer = 0;

            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            ViewBag.showRefundMessage = false;
            return View("GuestInformation", obj);
        }
        [Route("GuestDetails/{bookingId}", Name = "GuestDetails")]
        public ActionResult GuestDetails(string bookingId)
        {

            ViewBag.HeaderBarData = "Guest Information";
            int BookingId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(bookingId));
            BookingGuestDetails obj = new BookingGuestDetails();
            obj = BL_PropDetails.GetBookingDetailsForGuests(BookingId);

            if (obj.objBooking.iCounterOffer == null)
                obj.objBooking.iCounterOffer = 0;

            return View("GuestInformation", obj);
        }
        //Save booking data in Database
        /// <summary>
        /// After Proceed to Payment of Bargain it will go first to Chargecard and chargecard call this method 
        /// to save the booking details as we are assuming that customer will paied 500 sucessfully
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetBooking(PropDetailsM obj)
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


                //Object created for tblBookingTx
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
                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.BookingStatus = "PP";
                obj.objBooking.PaymentStatus = "P";
                obj.objBooking.sExtra4 = "N";//N means new
                obj.objBooking.sCurrencyCode = obj.Currency;//Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";

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
               

                // decimal OFRServiceTax = Convert.ToDecimal(BL_Bidding.GetBidMaster(obj.objBooking.sCurrencyCode).dOFRServiceCharge);


                if (!String.IsNullOrEmpty(obj.objBooking.sExtra2))
                {
                    obj.objBooking.sExtra2 = (((Convert.ToDecimal(obj.objBooking.sExtra2) * Convert.ToDecimal(ExchangeRate)) - obj.objBooking.dServiceCharge - obj.objBooking.dGSTOnServiceCharge) ).ToString();
                }
                obj.objBooking.dTaxes = (obj.dSummaryTaxes - (obj.objBooking.dServiceCharge + obj.objBooking.dGSTOnServiceCharge)) * ExchangeRate;
                obj.objBooking.dTaxesForHotel = (obj.dSummaryTaxes - (obj.objBooking.dServiceCharge + obj.objBooking.dGSTOnServiceCharge)) * ExchangeRate;
                obj.objBooking.dTotalExtraBedAmount = obj.dSummaryExtraBedCharges * ExchangeRate;

                obj.objBooking.dTaxesOriginal = obj.dSummaryTaxes * ExchangeRate;

                //Get Time Zone for this booking
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

                if (obj.sActionType == "N")   //For Negotiation Type of booking
                {
                    obj.objBooking.dAdvNegotiateAmount = 500;
                    obj.objBooking.dTotalNegotiateAmount = (obj.dNegotiationAmt) * ExchangeRate;
                    obj.objBooking.cBookingType = "N";
                    obj.objBooking.iNegotiateAttempts = 1;

                    //Object created for tblBookingNegotiationTx
                    objNego.dTotalNegotiateAmount = obj.dNegotiationAmt * ExchangeRate;
                    objNego.dtNegotiationDate = DateTime.Now;
                    objNego.dtActionDate = DateTime.Now;
                    objNego.cStatus = "A";

                }
                else   //For Normal Type of booking
                {
                    obj.objBooking.cBookingType = "R";
                }

                //Object created for tblBookingTrakerTx
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
                                        // iRooms = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms),
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

                                    //if (obj.objBooking.cBookingType == "N")
                                    //{

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
                                        sTaxName = Convert.ToString(lstTaxesDateWise[tax].TaxName)
                                    });
                                }
                                //}
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

                //Object for total orginal booking amount
                objOrgBook.dOriginalTotalAmount = OriginalTotalAmt;

                result = BL_Booking.AddBooking(obj, objNego, objTrck, lstBookDetails, lst, lstCancelPolicy, lstDayTaxes, objOrgBook, lstDayTaxesDateWise);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        ///  //Update Guest Information Details of Booking
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateGuestInformation(BookingGuestDetails obj)
        {
            int result;
            try
            {
                var refundModel = new eRefundModel();
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
                                sCountryPhoneCode = Convert.ToString(item["countryPhoneCode"]),
                                sBedPreference = Convert.ToString(item["ptype"]),
                                iBookingDetailsID = Convert.ToInt32(item["BookingDetailId"]),
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
                obj.objBooking.sCountryPhoneCode = obj.sCountryPhoneCode;
                result = BL_Booking.UpdateBooking_AddGuestInformation(obj.objBooking, lst, objTrck, lsttblBookDetails);
                if (result > 0)
                {

                    #region  GST Info Update

                    if (obj.IsGuestBooking)
                    {
                        var guest = BL_WebsiteUser.GetGuestRecordById(obj.CustomerId);
                        guest.GstnEntityName = obj.GstnEntityName;
                        guest.GstnEntityType = obj.GstnEntityType;
                        guest.GstnNumber = obj.GstnNumber;
                        //if(obj.GstnState.HasValue)
                        //guest.iStateId = obj.GstnState;
                        BL_WebsiteUser.UpdateGuestRecord(guest);
                    }
                    else
                    {
                        var customer = BL_WebsiteUser.GetSingleRecordById(obj.CustomerId);
                        customer.GstnEntityName = obj.GstnEntityName;
                        customer.GstnEntityType = obj.GstnEntityType;
                        customer.GstnNumber = obj.GstnNumber;
                        //if(obj.GstnState.HasValue)
                        //customer.StateId = obj.GstnState;
                        BL_WebsiteUser.UpdateRecord(customer);
                    }

                    #endregion

                    //TODO : Need to Chnage the email
                    var ccMail = string.Empty;
                    ccMail = "himanshuS@futuresoftindia.com";


                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(obj.objBooking.iPropId.Value);

                    string sPrimaryContactEmail;
                    string sConfirmationContactEmail;
                    string sRevenueManagerEmail;
                    string sRevenueManagerContact;
                    string sConfirmationContact;

                    pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                    pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                    pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);
                    pRevenueManager.TryGetValue("sConfirmationContact", out sConfirmationContact);

                    List<string> emailList = new List<string>();
                    emailList.Add(sConfirmationContactEmail);
                    emailList.Add(sRevenueManagerEmail);
                    emailList.Add(sPrimaryContactEmail);

                    string hotelEmails = string.Join(",", emailList.Distinct().ToList());

                    //Get all information of the booking to send mail

                    var customerBookingModel = BL_Booking.GetBookingModifyDetails_Notifications(Convert.ToInt32(obj.objBooking.iBookingId));
                    var revenuMangerBookingModel = customerBookingModel;
                    customerBookingModel.RatingImageUrl = obj.RatingImageUrl; //Tripadvisior
                    customerBookingModel.RatingString = obj.RatingString;// Tripadvisior
                    revenuMangerBookingModel.RatingImageUrl = obj.RatingImageUrl;
                    revenuMangerBookingModel.RatingString = obj.RatingString;

                    if (obj.objBooking.cBookingType == "B")
                    {
                        /* NOW Amenity is coming from Procedure instead of below code
                        var rateInclusions = BL_Booking.GetBidRateInclusions(obj.objBooking.iBookingId);

                        foreach (var item in customerBookingModel.BookingRoomDetails)
                        {
                            item.AmenityRatePlan = rateInclusions;
                        }
                       
                        foreach (var item in revenuMangerBookingModel.BookingRoomDetails)
                        {
                            item.AmenityRatePlan = rateInclusions;
                        } */
                        if (obj.objBooking.dRefundAmount > 0 && obj.objBooking.cBookingType.ToLower() == "b")
                        {
                            clsUtils.sendSMS(obj.objBooking.sMobileOFR, "Refund initiated for bookingId - " + obj.objBooking.iBookingId);

                            refundModel.ConfirmationNumber = obj.objBooking.iBookingId.ToString();
                            refundModel.CustomerName = obj.objBooking.sTitleOFR + " " + obj.objBooking.sFirstNameOFR + " " + obj.objBooking.sLastNameOFR;
                            refundModel.RefundAmount = clsUtils.ConvertNumberToCommaSeprated(obj.objBooking.dRefundAmount != null && obj.objBooking.dRefundAmount.Value > 0 ? Math.Round(obj.objBooking.dRefundAmount.Value, 2) : 0).ToString();
                            refundModel.RefundInitiationDate = DateTime.Now.ToShortDateString();
                            refundModel.sRefundStatusMsg = @"Your refund has been initiated for <strong>" + obj.objBooking.sCurrencyCode + " " + Convert.ToInt32(obj.objBooking.dRefundAmount) +
                                                           "</strong>. It will be refunded within the next 15 working days as per the standard banking norms, If paid through Payu wallet or 3-4 business days.";
                            refundModel.IsBiddingRefund = true;
                            var html_Customer_Bid = this.RenderViewToString("_RefundEmailTemplate", refundModel);

                            Task.Run(() => MailComponent.SendEmail(obj.objBooking.sEmailOFR, ccMail, "", "OneFineRate –Refund! Confirmation No: " + obj.objBooking.iBookingId, html_Customer_Bid, null, null, true, null, null));
                            refundModel.CustomerEmail = customerBookingModel.EmailOFR;
                            refundModel.CustomerPhone = customerBookingModel.MobileOFR;
                            refundModel.HotelName = customerBookingModel.HotelName;
                            refundModel.City = customerBookingModel.sCity;
                            refundModel.CheckInDate = customerBookingModel.CheckIn;
                            refundModel.CheckOutDate = customerBookingModel.sCheckOut;
                            refundModel.IsRevenueManagerRefund = true;
                            var paraxisAdminsEmail = BL_tblEmailSettingsM.GetRecord("RevenueManager");
                            var html_Ofr_Account = this.RenderViewToString("_RefundEmailTemplate", refundModel);
                            Task.Run(() => MailComponent.SendEmail(sConfirmationContactEmail, ccMail, "", "OneFineRate –Refund! Confirmation No: " + obj.objBooking.iBookingId, html_Ofr_Account, null, null, true, null, null));
                        }

                    }
                    if (obj.objBooking.cBookingType == "C" && User.Identity.IsAuthenticated)
                    {
                        customerBookingModel.CompanyName = BL_Booking.GetCompanyNameByUserEmail(User.Identity.GetUserId<long>());
                    }

                    #region RevenuManager

                    var paraxisAdmin = BL_tblEmailSettingsM.GetRecord("RevenueManager");

                    var OFRExtranetBaseUrl = ConfigurationManager.AppSettings["OFRExtranetBaseUrl"].ToString();

                    //Generation of the URL to furthur view by User
                    string revenueManagreConfirmationLink = OFRExtranetBaseUrl + "/BookingConfirmation/" + clsUtils.Encode(customerBookingModel.BookingId);


                    //URL shorten 
                    string short_revenueManagreConfirmationLink_ = clsUtils.Shorten(revenueManagreConfirmationLink);

                    //SMS prepartion 
                    string messageRevenueManager = "New Booking! Your Booking no. is  " + customerBookingModel.BookingId + " , " + customerBookingModel.CheckIn + " to " + customerBookingModel.ChekOut + " for " + customerBookingModel.NoOfRooms + " rooms." +
                        " A detailed confirmation has been sent to your registered email address. You can also review it on " + short_revenueManagreConfirmationLink_;
                    
                    //Send SMS to Reveneu Manager 
                    var status1 = clsUtils.sendSMS(sConfirmationContact, messageRevenueManager);

                    #endregion

                    #region For Hotel Confirmation Mail

                    var revenuManagerModel = new NegotiationEmailTempleteModel();
                    revenuManagerModel.IsRevenueManagerFormat = true;
                    revenuManagerModel.BookingModify = revenuMangerBookingModel;

                    //In case of Redeem
                    if(revenuManagerModel.BookingModify.sExtra3 != "Redeem")
                    revenuManagerModel.BookingModify.sExtra4 = null;

                    revenuManagerModel.BookingModify.Comm = Math.Round(Convert.ToDecimal(revenuManagerModel.BookingModify.Comm), 2).ToString();
                    revenuManagerModel.BookingModify.sCurrencyCode = "INR";
                    revenuManagerModel.BookingModify.Symbol = "₹";
                    
                    //Reveneu Manager email conformation 
                    var html_RevenueManager = this.RenderViewToString("_BookingConfirmationTemplateRM", revenuManagerModel.BookingModify);

                    //Praxis Admin Email Confirmation
                    var html_Admin = this.RenderViewToString("_BookingConfirmationTemplateAdmin", revenuManagerModel.BookingModify);

                    //PDF of Confirm HTML
                    var htmlToPdf_RevenueManager = new NReco.PdfGenerator.HtmlToPdfConverter();
                    var pdfBytes_RevenueManager = htmlToPdf_RevenueManager.GeneratePdf(html_RevenueManager);
                    var attachment_RevenueManager = new Attachment(new MemoryStream(pdfBytes_RevenueManager), "Booking#" + revenuMangerBookingModel.BookingId + ".pdf");

                    //Confirmation Mail to Revenue Manager 
                    MailComponent.SendEmail(hotelEmails, ccMail, paraxisAdmin.sEmail, "OneFineRate-New Booking! Confirmation No:" + obj.objBooking.iBookingId, html_RevenueManager, attachment_RevenueManager, null, true, new MemoryStream(pdfBytes_RevenueManager), "Booking#" + revenuMangerBookingModel.BookingId + ".pdf");

                    //Confirmation Mail to Praxis Admin
                    MailComponent.SendEmail(paraxisAdmin.sEmail, ccMail, "", "OneFineRate-New Booking! Confirmation No:" + obj.objBooking.iBookingId, html_Admin, null, null, true, null, null);


                    #endregion For Hotel

                    #region User Confirmation mail

                    string confirmationLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/BookingConfirmation/" + clsUtils.Encode(customerBookingModel.BookingId);

                    string short_confirmationLink = clsUtils.Shorten(confirmationLink);

                    //Send SMS
                    string message = "New Booking at OFR! Your Booking no. is  " + customerBookingModel.BookingId + " , " + customerBookingModel.CheckIn + " to " + customerBookingModel.ChekOut + " for " + customerBookingModel.NoOfRooms + " rooms." +
                        "A detailed confirmation has been sent to your registered email address. You can also review it on ";
                    var newMessage = string.Concat(message, short_confirmationLink, " .You save more on OneFineRate. Tell your friends about OFR and save even more in your next transaction with us.");
                    clsUtils.sendSMS(customerBookingModel.MobileOFR, newMessage);

                    //In case of Redeem Points, sExtra4 will contain Redeemed Points
                    if (revenuManagerModel.BookingModify.sExtra3 != "Redeem")
                        customerBookingModel.sExtra4 = null;
                    customerBookingModel.Symbol = "₹";
                    customerBookingModel.sCurrencyCode = "INR";
                    var html_Customer = this.RenderViewToString("_BookingEmailTemplate", customerBookingModel);
                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                    var pdfBytes = htmlToPdf.GeneratePdf(html_Customer);
                    var attachment_Customer = new Attachment(new MemoryStream(pdfBytes), "Booking#" + customerBookingModel.BookingId + ".pdf");
                    
                    //Confirmation Mail to Guest
                    MailComponent.SendEmail(guestEmailIds, ccMail, "", "OneFineRate-New Booking! Confirmation No:" + obj.objBooking.iBookingId, html_Customer, attachment_Customer, null, true, new MemoryStream(pdfBytes), "Booking#" + customerBookingModel.BookingId + ".pdf");

                    #endregion

                    //Sending Invoice to Customer
                    SendInvoiceAfterGuestInformationUpdate(Convert.ToInt32(obj.objBooking.iBookingId));

                    return RedirectToRoute("ShareBookingInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(obj.objBooking.iBookingId)) });
                }

            }
            catch (Exception ex)
            {
                TempData["ERROR"] = "Sorry! An Error Occurred while sending confirmation email, Please try again after some time.";
                return RedirectToRoute("ShareBookingInformation", new { bookingId = clsUtils.Encode(Convert.ToString(obj.objBooking.iBookingId)) });
            }

            ViewBag.HeaderBarData = "Guest Information";

            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

            return View("GuestInformation", obj);
        }
        //500 Negotiation amt recieved Confirmation Page
        ////[Route("AdvanceNegoConfirmation/{bookingId}", Name = "AdvanceNegoConfirmation")]
        ////public ActionResult AdvanceNegotaionConfirmation(string bookingId)
        ////{
        ////    int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

        ////    var negotiationConfirmationDetail = BL_Booking.GetUnfinishedTransactionNegotiationForHotelPending(bookId);

        ////    return View(negotiationConfirmationDetail);
        ////}

        //Negotiation Booking Confirmation Page
        [Route("Confirmation/{bookingId}", Name = "Confirmation")]
        public ActionResult NegotaionConfirmation(string bookingId)
        {
            ViewBag.HeaderBarData = "Confirmation";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var negotiationConfirmationDetail = BL_Booking.GetUnfinishedTransactionNegotiationForHotelPending(bookId);

            return View(negotiationConfirmationDetail);
        }
        //Normal Booking Confirmation Page
        [Route("BookingConfirmation/{bookingId}/{anonym?}", Name = "BookingConfirmation")]
        public ActionResult BookingConfirmation(string bookingId, string anonym = null)
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
        //Normal Booking Confirmation Page
        [Route("BConfirmation/{bookingId}", Name = "BConfirmation")]
        public ActionResult BConfirmation(string bookingId)
        {
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Confirmation";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var BookingModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(bookId));
            BookingModel.Symbol = "₹";
            Status = BookingModel.cBookingStatus;
            CCType = BookingModel.ccType;

            return View(BookingModel);
        }
        //Normal Booking Confirmation Page
        [Route("BookingStatus/{bookingId}", Name = "BookingStatus")]
        public ActionResult BookingStatusGuest(string bookingId, string phone, string email)
        {
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Confirmation";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));

            var bookingModel = BL_Booking.GetBookingDetailsByEmailOrPhone(Convert.ToInt32(bookId), phone, email);
            Status = bookingModel.cBookingStatus;
            CCType = bookingModel.ccType;

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
                long lbookingid = BL_Booking.GetBookingIdByLinkedId(bookingModel.iLinkedBookingId);
                string id = OneFineRateAppUtil.clsUtils.Encode(lbookingid.ToString());
                //return RedirectToRoute("BookingConfirmation", new { bookingId = id });
                bookingModel = BL_Booking.GetBookingModifyDetails(Convert.ToInt32(lbookingid));
            }

            return View("BookingConfirmation", bookingModel);
        }
        //Negotaion Basic Amt Recieved Page, Pay left amount
        [Route("NegoConfirmed/{bookingId}", Name = "BalancePaymnet")]
        public ActionResult BalancePayment(string bookingId)
        {
            string Status = "", CCType = "";
            ViewBag.HeaderBarData = "Preview";
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));
            BalancePaymentModel obj = new BalancePaymentModel();

            obj = BL_Booking.GetUnfinishedTransactionToSendRevenueManager(bookId);

            if (HttpContext.Request.Params["C"] != null && HttpContext.Request.Params["C"] != "")
            {
                obj.sBookType = HttpContext.Request.Params["C"].ToString();
            }
            else
            {
                obj.sBookType = "N";
            }

            Status = obj.cBookingStatus;
            CCType = obj.ccType;

            if (Status == "RM")
            {
                return RedirectToAction("Index", "Home");
            }
            if (obj.sBookType == "C" || obj.sBookType == "R")
            {
                return View(obj);
            }

            //Redirecting the pages 
            if (Status == "PC" || Status == "PS" || Status == "NP" || Status == "CP" || Status == "FP")
            {
                if (obj.bSelfTravelling == null)
                {
                    //Guest Details
                    return RedirectToRoute("GuestInformation", new { bookingId = bookingId });
                }
                else
                {
                    //Booking Confirmation
                    return RedirectToRoute("BookingConfirmation", new { bookingId = bookingId });
                }

            }
            else if (Status == "HR" || Status == "CO" || Status == "FO")
            {

                if (CCType == "CA" || CCType == "BA" || CCType == "RA")
                {
                    //Balance Payment
                    //return RedirectToAction("BalancePayment", new { bookingId = bookingId });
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
            else if (Status == "NC")
            {
                //Booking Confirmation
                return RedirectToRoute("BookingConfirmation", new { bookingId = bookingId });
            }
            else if (Status == "BP" || Status == "NA" || Status == "CA" || Status == "NS" || Status == "FA" || Status == "BA")
            {
                //Balance Payment
                //return RedirectToAction("BalancePayment", new { bookingId = bookingId });
            }
            else if (Status == "RM")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Status == "LT")
            {
                //Booking Confirmation through linked id
                long lbookingid = BL_Booking.GetBookingIdByLinkedId(obj.iLinkedBookId);
                string id = OneFineRateAppUtil.clsUtils.Encode(lbookingid.ToString());
                return RedirectToRoute("BookingConfirmation", new { bookingId = id });
            }


            return View(obj);
        }
        //Validate Promo code applied on Normal Booking
        [HttpPost]
        public ActionResult ValidatePromoCode(PropDetailsM eObj)
        {
            object result = null;
            try
            {
                OneFineRateBLL.BL_tblPropertyPromoMap.ePromoValidate Pobj = new OneFineRateBLL.BL_tblPropertyPromoMap.ePromoValidate();
                Pobj = BL_tblPropertyPromoMap.VaidatePromoCode(eObj.sPromoCode, DateTime.Now, eObj.dtCheckIn, eObj.dtCheckOut, eObj.iPropId, eObj.dSummaryTotal, eObj.dSummaryExtraBedCharges, eObj.dSummaryTaxes, Session["CurrencyCode"].ToString());
                if (Pobj.Status == 1)
                {
                    if (Pobj.rtype == 1)
                        result = new { st = 1, msg = Pobj.Error, amt = Pobj.Net_Amt, disc = Pobj.Discount, rtype = Pobj.rtype, discdesc = Pobj.DiscountValue };
                    else
                        result = new { st = 1, msg = Pobj.Error, amt = Pobj.Net_Amt, disc = Pobj.Amenity, rtype = Pobj.rtype };
                }
                else
                {
                    result = new { st = 0, msg = Pobj.Error, amt = Pobj.Net_Amt, };
                }
                //if (j == 1)
                //{
                //    result = new { st = 1, msg = "Updated successfully." };
                //}
                //else
                //{
                //    result = new { st = 0, msg = "Kindly try after some time." };
                //}


            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Redirect to this action when link on email will be clicked.
        public ActionResult NegotiationStatus(string bookingId)
        {
            try
            {
                string Status = "", CCType = "";
                ViewBag.HeaderBarData = "Preview";
                BalancePaymentModel model = new BalancePaymentModel();

                var decodedBookingId = Convert.ToInt64(clsUtils.Decode(bookingId));

                model = BL_Booking.GetNegotionTransactionDataForBooking(decodedBookingId);
                Status = model.cBookingStatus;
                CCType = model.ccType;
                //var bookiDetailList = BL_Booking.GetBookingDetailList(decodedBookingId);
                //model.BookingDetailList = bookiDetailList;
                model.iLinkedBookId = decodedBookingId;

                if (model != null && model.cBookingStatus == "PC")
                {
                    ViewBag.Message = "This Bargain was processed earlier!";
                }
                else if (model != null && model.cBookingStatus == "RM")
                {
                    ViewBag.Message = "Refund has been initiated against this booking.";
                }
                else
                {
                    ViewBag.StatusType = model.cBookingStatus;
                }



                //Redirecting the pages 
                if (Status == "PC" || Status == "PS" || Status == "NP" || Status == "CP" || Status == "FP")
                {
                    if (model.bSelfTravelling == null)
                    {
                        //Guest Details
                        return RedirectToRoute("GuestInformation", new { bookingId = bookingId });
                    }
                    else
                    {
                        //Booking Confirmation
                        return RedirectToRoute("BookingConfirmation", new { bookingId = bookingId });
                    }

                }
                else if (Status == "HR" || Status == "CO" || Status == "FO")
                {

                    if (CCType == "CA" || CCType == "BA" || CCType == "RA")
                    {
                        //Balance Payment
                        return RedirectToAction("BalancePayment", new { bookingId = bookingId });
                    }
                    else if (CCType == "CR" || CCType == "RR" || CCType == "FO")
                    {
                        //Negotiation Status
                    }
                    //Negotiation Status
                    // return RedirectToAction("NegotiationStatus", new { bookingId = bookingId });
                }
                else if (Status == "NC")
                {
                    //Booking Confirmation
                    return RedirectToRoute("BookingConfirmation", new { bookingId = bookingId });
                }
                else if (Status == "BP" || Status == "NA" || Status == "CA" || Status == "NS" || Status == "FA" || Status == "BA")
                {
                    //Balance Payment
                    return RedirectToAction("BalancePayment", new { bookingId = bookingId });
                }
                //else if (Status == "RM")
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                else if (Status == "LT")
                {
                    //Booking Confirmation through linked id
                    long lbookingid = BL_Booking.GetBookingIdByLinkedId(model.iLinkedBookId);
                    string id = OneFineRateAppUtil.clsUtils.Encode(lbookingid.ToString());
                    return RedirectToRoute("BookingConfirmation", new { bookingId = id });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        public ActionResult GetCompetitorHotels(int bookingId)
        {
            BalancePaymentModel model = new BalancePaymentModel();
            model = BL_Booking.GetCompetitorHotels(bookingId);

            model.iBookingId = bookingId;
            return PartialView("pCompetitorHotels", model);
        }
        public ActionResult RefundMoney(string id)
        {
            var refundModel = new eRefundModel();
            try
            {
                id = clsUtils.Decode(id);
                int iBookingId = Convert.ToInt32(id);
                etblBookingTx obj = BL_Booking.GetBooking(iBookingId);
                if (obj.BookingStatus != "RM")
                {

                    if (obj.BookingStatus == "CO" || obj.BookingStatus == "HR" || obj.BookingStatus == "FO")
                    {
                        obj.BookingStatus = "RM";
                        obj.dtActionDate = DateTime.Now;

                        etblBookingTrakerTx objtrk = new etblBookingTrakerTx();
                        objtrk.iBookingId = iBookingId;
                        objtrk.BookingStatus = "RM";
                        objtrk.dtActionDate = DateTime.Now;

                        int Result = BL_Booking.UpdateBookingStatus(obj, objtrk);
                        if (Result == 1)
                        {
                            //Update Available Rooms Inventory
                            Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(iBookingId), "NR"));
                            clsUtils.sendSMS(obj.sMobileOFR, "Refund initiated for bookingId - " + iBookingId);

                            refundModel.ConfirmationNumber = iBookingId.ToString();
                            //refundModel.CustomerName = obj.sTitleOFR + " " + obj.sFirstNameOFR;
                            refundModel.CustomerName = obj.sTitleOFR + " " + obj.sLastNameOFR;
                            refundModel.RefundAmount = obj.dAdvNegotiateAmount.ToString();
                            refundModel.RefundInitiationDate = DateTime.Now.ToShortDateString();
                            refundModel.IsNegotiationRefund = true;
                            var html_Customer = this.RenderViewToString("_RefundEmailTemplate", refundModel);

                            MailComponent.SendEmail(obj.sEmailOFR, "", "", "OneFineRate –Refund! Confirmation No: " + obj.iBookingId, html_Customer, null, null, true, null, null);

                            // return Json(new { st = "1", msg = "Refund Initiated successfully." }, JsonRequestBehavior.AllowGet);    

                            refundModel.sRefundStatusMsg = @"Your refund has been initiated for <strong>" + obj.sCurrencyCode + " " + Convert.ToInt32(obj.dAdvNegotiateAmount) +
                                                            "</strong>. It will be refunded within 24 hours, If paid through PAYU or 3-4 business days " +
                                                            "if paid through any other payment method as per banking norms.";
                        }
                        else
                        {
                            //return Json(new { st = "0", msg = "kindly try after some time." }, JsonRequestBehavior.AllowGet);
                            refundModel.sRefundWarningMsg = "An Error occured! kindly try after some time.";
                        }
                    }
                    else
                    {
                        //return Json(new { st = "1", msg = "Refund cannot be  initiated for this transaction." }, JsonRequestBehavior.AllowGet);
                        refundModel.sRefundWarningMsg = "Refund cannot be  initiated for this transaction.";
                    }
                }
                else
                {
                    // return Json(new { st = "0", msg = "Refund Already Initiated" }, JsonRequestBehavior.AllowGet);

                    refundModel.sRefundWarningMsg = "Refund Already Initiated.";
                }
            }
            catch (Exception)
            {
                //return Json(new { st = "0", msg = "kindly try after some time." }, JsonRequestBehavior.AllowGet);
                refundModel.sRefundWarningMsg = "An Error occured! kindly try after some time.";
            }

            //return RedirectToAction("Refund", refundModel);
            return View("~/Views/Payment/Refund.cshtml", refundModel);
        }
        //Competition hotel booking page, balance payment page
        [Route("BalancePay/{bookingId}/{selected}", Name = "BookNewNegoProperty")]
        public ActionResult BookNewNegoProperty(BalancePaymentModel obj, string bookingId, int? selected)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.HttpMethod == "GET")
                {
                    if (selected.HasValue && !string.IsNullOrEmpty(bookingId))
                    {
                        long bkngId = long.Parse(clsUtils.Decode(bookingId));
                        obj = BL_Booking.GetCompetitorHotels(bkngId);
                        obj.iSelected = selected.Value;
                        obj.iBookingId = bkngId;
                    }
                }

                ViewBag.HeaderBarData = "Preview";
                var prop = obj.lstRoomData.Where(u => u.Id == obj.iSelected).SingleOrDefault();


                decimal ExchangeRate = obj.ExchangeRate;
                if (prop != null)
                {
                    obj.objresultHotelFacilities = BL_Booking.GetHotelFacilities(prop.iHotelID);
                    obj.sImgUrl = prop.sImgUrl;
                    obj.iStarCategoryId = Convert.ToInt16(prop.iStarCategoryId);
                    obj.sHotelName = prop.sHotelName;
                    obj.sPropertyAddress = prop.sArea;
                    obj.Symbol = prop.Symbol;
                    obj.dTotalAmount = prop.dDiscPrice;
                    obj.dTaxes = prop.dTax;
                    obj.dAdvNegotiateAmount = 500 * ExchangeRate;
                    obj.iPropId = prop.iHotelID;
                    obj.dOfrServiceCharge = prop.dServiceCharge;
                    obj.dGSTOnServiceCharge = prop.dTaxOnServiceCharge;
                }
                else
                {

                    var data = obj.lstRoomDataCurrent.Where(u => u.Id == obj.iSelected).SingleOrDefault();

                    if (data != null)
                    {
                        obj.objresultHotelFacilities = BL_Booking.GetHotelFacilities(data.iHotelID);
                        obj.sImgUrl = data.sImgUrl;
                        obj.iStarCategoryId = Convert.ToInt16(data.iStarCategoryId);
                        obj.sHotelName = data.sHotelName;
                        obj.sPropertyAddress = data.sArea;
                        obj.Symbol = data.Symbol;
                        obj.dTotalAmount = data.dDiscPrice;
                        obj.dTaxes = data.dTax;
                        obj.dAdvNegotiateAmount = 500 * ExchangeRate;
                        obj.iPropId = data.iHotelID;
                        obj.dOfrServiceCharge = data.dServiceCharge;
                        obj.dGSTOnServiceCharge = data.dTaxOnServiceCharge;
                    }
                }
            }
            catch (Exception)
            {

            }
            return View(obj);
        }
        //Competition hotel booking page, sent to payment gateway by making new booking
        public ActionResult SaveNewBookingFromOldBooking(BalancePaymentModel obj)
        {
            try
            {
                if (obj.cBookingStatus == "CO" || obj.cBookingStatus == "HR")
                {
                    etblBookingTrakerTx objtrk = new etblBookingTrakerTx();
                    var bookingDetail = BL_Booking.GetBooking(obj.iLinkedBookId);
                    if (obj.cBookingStatus == "CO")
                    {
                        objtrk.BookingStatus = "CR";
                        bookingDetail.BookingStatus = "CR";

                    }
                    else //(obj.cBookingStatus=="HR")
                    {
                        objtrk.BookingStatus = "OR";
                        bookingDetail.BookingStatus = "OR";
                    }

                    objtrk.iBookingId = obj.iLinkedBookId;
                    objtrk.dtActionDate = DateTime.Now;

                    bookingDetail.dtActionDate = DateTime.Now;
                    bookingDetail.PaymentStatus = "P";
                    int i = BL_Booking.UpdateBookingStatus(bookingDetail, objtrk);
                    if (i == 1)
                    {
                        var result = BL_Booking.MakeNewBooking(Convert.ToInt32(obj.iLinkedBookId), obj.iPropId);
                        return RedirectToAction("PayNow", "Payment", new { bookingId = result.ID });
                    }
                    else
                    {
                        ViewBag.Message = "Kindly try after some time.";
                    }
                }
                else
                {
                    ViewBag.Message = "This booking is already processed.";
                }


            }
            catch (Exception)
            {
                ViewBag.Message = "Kindly try after some time.";
            }
            return View("~/Views/Negotiation/BookNewNegoProperty.cshtml", obj);

        }
        //Share booking informtion on social
        [Route("ShareInformation/{BookingId}", Name = "ShareBookingInformation")]
        public ActionResult ShareBookingInformation(string BookingId)
        {
            eBookingShareInfo obj = new eBookingShareInfo();
            try
            {
                int bookId = Convert.ToInt32(clsUtils.Decode(BookingId));
                ViewBag.HeaderBarData = "Share booking information";
                obj = BL_Booking.GetBookingDetailsForSharing(Convert.ToInt32(bookId));
                obj.PropertyId = OneFineRateAppUtil.clsUtils.Encode(obj.iPropId.ToString());
                obj.BookingId = BookingId;
                string socialShareMessage = BL_Booking.GetSocialShareMessage(bookId);
                obj.sDescription = socialShareMessage;
                if (obj.HotelDesc == null)
                {
                    obj.HotelDesc = "";
                }
            }
            catch (Exception)
            {

            }
            return View(obj);
        }
        public ActionResult ShareInformationViaEmail(eBookingShareInfo obj)
        {
            Task.Run(() => MailComponent.SendEmail(obj.sMailTo, "", "", "Share Booking Information", obj.sDescription, null, null, false, null, null));
            return RedirectToRoute("BookingConfirmation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(obj.iBookingId)) });
        }
        /// <summary>
        /// Calculate the grand total with tax after entring the negotetion amount 
        /// </summary>
        /// <param name="NegotiationAmount"></param>
        /// <param name="TotalAmt"></param>
        /// <param name="propdetailguid"></param>
        /// <returns></returns>
        public ActionResult CalculateGrandTotal(decimal NegotiationAmount, decimal TotalAmt,string propdetailguid)
        {
            try
            {
                var stronglObject = new PropDetailsM();
                if (Session[propdetailguid] != null)
                {
                    //TempData.Keep();
                    stronglObject = Session[propdetailguid] as PropDetailsM;
                }

                decimal NetTotalAmt = 0;
                Decimal? ExchangeRate = 1;

                string CurrencyCode = stronglObject.Currency; //Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";

                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",CurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = objExchange.dRate;
                    }
                }
                //  decimal OFRServiceTax = Convert.ToDecimal(BL_Bidding.GetBidMaster(CurrencyCode).dOFRServiceCharge);
                //NegotiationAmount = NegotiationAmount * Convert.ToDecimal(ExchangeRate);
                // TotalAmt = TotalAmt * Convert.ToDecimal(ExchangeRate);
                decimal OFRServiceTax = stronglObject.TaxCharges.dOFRServiceCharge + stronglObject.TaxCharges.TaxOnServiceCharge;

                List<etblBookedDayWiseTaxAmountDetails> lstDayTaxes = new List<etblBookedDayWiseTaxAmountDetails>();
                decimal percentage = NegotiationAmount / TotalAmt;

                for (int i = 0; i < stronglObject.lstetblRooms.Count; i++)
                {
                    for (int j = 0; j < stronglObject.lstetblRooms[i].lstRatePlan.Count; j++)
                    {
                        for (int lstOcc = 0; lstOcc < stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                        {
                            if (stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                            {
                                for (int k = 0; k < stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                                {
                                    for (int tax = 0; tax < stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].lstTaxesDateWise.Count; tax++)
                                    {

                                        decimal BasePrice = Convert.ToDecimal(stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dBasePrice);// *Convert.ToDecimal(ExchangeRate);
                                        decimal TaxPerc = Convert.ToDecimal(stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].lstTaxesDateWise[tax].TaxPer);// *Convert.ToDecimal(ExchangeRate);
                                        decimal TaxVal = Convert.ToDecimal(stronglObject.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].lstTaxesDateWise[tax].TaxVal);// *Convert.ToDecimal(ExchangeRate);
                                        decimal Amt = 0;
                                        Amt = BasePrice * percentage;

                                        Amt = (Amt * TaxPerc) / 100;
                                        Amt = Amt + TaxVal;

                                        NetTotalAmt += Amt;

                                    }
                                }
                            }
                        }
                    }
                }

                NetTotalAmt = NetTotalAmt + OFRServiceTax;


                NetTotalAmt = Math.Round(NetTotalAmt, 2);

                string sTaxAmt = clsUtils.ConvertNumberToCommaSeprated(NetTotalAmt);
                decimal dGrandTotal = NetTotalAmt + NegotiationAmount;
                string sGrandTotal = clsUtils.ConvertNumberToCommaSeprated(dGrandTotal);
                return Json(new { st = "1", dTaxAmt = NetTotalAmt, sTaxAmt = sTaxAmt, dGrandTotal = dGrandTotal, sGrandTotal = sGrandTotal, msg = "Success." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { st = "0", msg = "kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To get all the details to generate the invoice to customer 
        /// </summary>
        /// <param name="bookingId"></param>
        public void SendInvoiceAfterGuestInformationUpdate(long bookingId)
        {
            //TODO : Need to Replace the email
            var ccMail = string.Empty;
            ccMail = "himanshuS@futuresoftindia.com";

            var bookingDetails = BL_Invoice.GetInvoiceDetailByBooking(bookingId);

            bookingDetails.sInvoiceNumber = bookingDetails.iBookingId + "/" + "01" + (bookingDetails.cBookingStatus == "MD" ? "/MOD" : "");
            bookingDetails.HotelOrGuest = HotelOrGuest.Guest;

            var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(bookingDetails.iPropId);

            string sPrimaryContactEmail;
            string sConfirmationContactEmail;
            string sRevenueManagerEmail;
            string sRevenueManagerContact;
            string sConfirmationContact;

            pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
            pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
            pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
            pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);
            pRevenueManager.TryGetValue("sConfirmationContact", out sConfirmationContact);

            List<string> emailList = new List<string>();
            emailList.Add(sRevenueManagerEmail);
            emailList.Add(sConfirmationContactEmail);
            emailList.Add(sPrimaryContactEmail);

            string emails = string.Join(",", emailList.Distinct().ToList());

            var html_Customer = this.RenderViewToString("_Invoice", bookingDetails);

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(html_Customer);
            var attachment_Customer = new Attachment(new MemoryStream(pdfBytes), "Invoice#" + bookingDetails.iBookingId + ".pdf");

            bookingDetails.sInvoiceNumber = bookingDetails.iBookingId + "/" + "02" + (bookingDetails.cBookingStatus == "MD" ? "/MOD" : "");
            bookingDetails.HotelOrGuest = HotelOrGuest.Hotel;

            var html_Hotel = this.RenderViewToString("_Invoice", bookingDetails);
            var htmlToPdf_Hotel = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes_Hotel = htmlToPdf.GeneratePdf(html_Hotel);
            var attachment_Hotel = new Attachment(new MemoryStream(pdfBytes_Hotel), "Invoice#" + bookingDetails.iBookingId + ".pdf");

            Task.Run(() =>
            {
                //Send Invoice to Guest and Revenue Manager  
                MailComponent.SendEmail(sRevenueManagerEmail, ccMail, "", "OneFineRate! Invoice# :" + bookingDetails.iBookingId, html_Hotel, attachment_Hotel, null, true, new MemoryStream(pdfBytes_Hotel), "Invoice#" + bookingDetails.iBookingId + ".pdf");
                MailComponent.SendEmail(bookingDetails.sEmailOFR, ccMail, "", "OneFineRate! Invoice# :" + bookingDetails.iBookingId, html_Customer, attachment_Customer, null, true, new MemoryStream(pdfBytes), "Invoice#" + bookingDetails.iBookingId + ".pdf");

            });
        }
        [HttpGet]
        public JsonResult GetStatesByGST(string gstFirstTwoDigit)
        {
            var results = BL_tblStateM.GetSingleRecordByGST(gstFirstTwoDigit);
            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);
        }
    }
}