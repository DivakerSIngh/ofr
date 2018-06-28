using OneFineRateBLL;
using OneFineRateWeb.Handlers;
using OneFineRateWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OneFineRateBLL.Entities;
using System.Web.Routing;
using OneFineRateWeb.App_Helper;
using FutureSoft.Util;
using System.Threading.Tasks;
using OneFineRateBLL.WebUserEntities;
using OneFineRateAppUtil;
using System.Data.Entity.Validation;
using System.Data;

namespace OneFineRateWeb.Controllers.Customer
{
    public class PaymentController : BaseController
    {
        etblBookingTrakerTx objtrk = new etblBookingTrakerTx();

        /// <summary>
        /// At the time of Payment
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="propDetailsGUID"></param>
        /// <param name="cType"></param>
        /// <param name="isPointRedumptionPayment"></param>
        /// <returns></returns>
        public ActionResult PayNow(int bookingId, string propDetailsGUID, string cType = "", bool? isPointRedumptionPayment = null)
        {
            try
            {
                var previousUrl = Request.UrlReferrer;
                Decimal? ExchangeRate = 1;
                WebsiteUser userDetails = null;
                var bookingDetail = BL_Booking.GetBooking(bookingId);

                //Check inventory exist or Not
                if (bookingDetail.cBookingType != "B" && bookingDetail.sProvisionalBookingIdTG == null)
                {
                    string sRoomjson = "", sCheckIN = "", sCheckOut = "";
                    DataSet ds = BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(bookingId), "C");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["OutCome"].ToString().Trim() == "Inventory not available")
                        {
                            #region FetchRoomData

                            var roomDataResult = new List<RoomData>();
                            var bookingDetailList = BL_Booking.GetBookingDetailList(bookingId);
                            {
                                if (bookingDetailList != null)
                                {
                                    int roomId = 1;

                                    foreach (var bookingDetailsItem in bookingDetailList)
                                    {
                                        var roomData = new RoomData() { adult = bookingDetailsItem.iAdults.Value, child = bookingDetailsItem.iChildren.Value, room = roomId };

                                        if (!string.IsNullOrEmpty(bookingDetailsItem.sChildrenAge))
                                        {
                                            foreach (var childAge in bookingDetailsItem.sChildrenAge.Split(','))
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

                                    sRoomjson = clsUtils.ConvertToJson(roomDataResult);
                                    sCheckIN = Convert.ToDateTime(bookingDetail.dtCheckIn.ToString()).ToString(("dd/MM/yyyy"));
                                    sCheckOut = Convert.ToDateTime(bookingDetail.dtChekOut.ToString()).ToString(("dd/MM/yyyy"));
                                }
                            }

                            #endregion

                            TempData["ERROR"] = "The room that was selected has been sold out.";

                            string websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                            Response.Redirect(websiteBaseUrl + "OfferReview?Id=" + clsUtils.Encode(bookingDetail.iPropId.ToString()) + "&CIn=" + sCheckIN + "&COut=" + sCheckOut + "&sRoomData=" + sRoomjson + "", true);
                        }
                    }
                }
                if (bookingDetail.cBookingType != "C" && bookingDetail.cBookingType != "B" && bookingDetail.sProvisionalBookingIdTG == null)
                {
                    string sRoomjson = "", sCheckIN = "", sCheckOut = "";
                    DataSet ds = BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(bookingId), "C");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["OutCome"].ToString().Trim() == "Inventory not available")
                        {
                            #region FetchRoomData

                            var roomDataResult = new List<RoomData>();
                            var bookingDetailList = BL_Booking.GetBookingDetailList(bookingId);
                            {
                                if (bookingDetailList != null)
                                {
                                    int roomId = 1;

                                    foreach (var bookingDetailsItem in bookingDetailList)
                                    {
                                        var roomData = new RoomData() { adult = bookingDetailsItem.iAdults.Value, child = bookingDetailsItem.iChildren.Value, room = roomId };

                                        if (!string.IsNullOrEmpty(bookingDetailsItem.sChildrenAge))
                                        {
                                            foreach (var childAge in bookingDetailsItem.sChildrenAge.Split(','))
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

                                    sRoomjson = clsUtils.ConvertToJson(roomDataResult);
                                    sCheckIN = Convert.ToDateTime(bookingDetail.dtCheckIn.ToString()).ToString(("dd/MM/yyyy"));
                                    sCheckOut = Convert.ToDateTime(bookingDetail.dtChekOut.ToString()).ToString(("dd/MM/yyyy"));
                                }
                            }

                            #endregion

                            TempData["ERROR"] = "The room that was selected has been sold out.";

                            string websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                            Response.Redirect(websiteBaseUrl + "OfferReview?Id=" + clsUtils.Encode(bookingDetail.iPropId.ToString()) + "&CIn=" + sCheckIN + "&COut=" + sCheckOut + "&sRoomData=" + sRoomjson + "", true);
                        }
                    }
                }


                if (bookingDetail.sCurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",bookingDetail.sCurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }

                if (bookingDetail.iCustomerId != null && bookingDetail.iCustomerId.HasValue)
                {
                    userDetails = BL_WebsiteUser.GetSingleRecordById(bookingDetail.iCustomerId.Value);
                }
                else
                {
                    userDetails = BL_NegotiationBooking.GetGuestUserDetailById(bookingDetail.iGuestId.Value);
                }
                if (userDetails != null)
                {
                    var payURequestModel = new FormCollection();

                    if (bookingDetail != null)
                    {
                        if (bookingDetail.BookingStatus == "PC" && bookingDetail.cBookingType != "B")
                        {
                            TempData["msg"] = "Payment already made for this Booking.";
                            return Redirect(Request.UrlReferrer.ToString());
                        }

                        eTblPurchaseHistory purchaseHistory = new eTblPurchaseHistory
                        {
                            ADDRESS1 = userDetails.CurrentAddressLine1,
                            ADDRESS2 = userDetails.CurrentAddressLine2,
                            CITY = userDetails.City,
                            CUSTOMEREMAIL = userDetails.Email,
                            FIRSTNAME = userDetails.FirstName,
                            iBookingId = bookingId,
                            iCustomerId = userDetails.Id,
                            LASTNAME = userDetails.LastName,
                            REFNOEXT = "REFNOEXT",
                            ORDERSTATUS = "PP",
                            PAYMENTDATE = DateTime.Now,
                            PHONE = userDetails.PhoneNumber,
                            REFNO = "",
                            ZIPCODE = userDetails.PinCode.ToString(),
                        };

                        if (cType == "UpgradeAmt")
                        {
                            payURequestModel.Set("udf4", "UpgradeBidAmount");
                            var obj = new eBiddingHotelsUpgradeRoomsList();
                            //propDetailsGUID bidupgradehotels
                            if (Session[propDetailsGUID] != null)
                            {
                                //TempData.Keep();
                                obj = Session[propDetailsGUID] as eBiddingHotelsUpgradeRoomsList;

                                purchaseHistory.Amount = Convert.ToDecimal(obj.TotalDifferenceConverted);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        //For booking type Negotiation
                        else if (bookingDetail.cBookingType == "N")
                        {
                            payURequestModel.Set("udf4", "NegoPayment");


                            if (bookingDetail.BookingStatus == "CO" || bookingDetail.BookingStatus == "CA")
                            {
                                objtrk.iBookingId = bookingDetail.iBookingId;
                                objtrk.dtActionDate = DateTime.Now;
                                objtrk.BookingStatus = "CA";

                                bookingDetail.PaymentStatus = "P";
                                bookingDetail.BookingStatus = "CA";
                                bookingDetail.dtActionDate = DateTime.Now;

                                if (bookingDetail.ccType == "CA" || bookingDetail.ccType == "CR")
                                {
                                    BL_Booking.UpdateBookingStatus(bookingDetail, objtrk);
                                    purchaseHistory.Amount = (bookingDetail.dccPrice + Convert.ToDecimal(bookingDetail.dTaxes) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - bookingDetail.dAdvNegotiateAmount;
                                }
                                else
                                {
                                    BL_Booking.UpdateBookingStatus(bookingDetail, objtrk);
                                    purchaseHistory.Amount = (bookingDetail.iCounterOffer + Convert.ToDecimal(bookingDetail.dTaxes) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - bookingDetail.dAdvNegotiateAmount;
                                }
                            }
                            else if (bookingDetail.BookingStatus == "HR")
                            {
                                if (bookingDetail.ccType == "RR" || bookingDetail.ccType == "RA")
                                {
                                    purchaseHistory.Amount = ((bookingDetail.dccPrice + Convert.ToDecimal(bookingDetail.dTaxes) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - Convert.ToDecimal(bookingDetail.dAdvNegotiateAmount));
                                }
                                else
                                {
                                    purchaseHistory.Amount = ((bookingDetail.dTotalAmount + bookingDetail.dTotalExtraBedAmount + bookingDetail.dTaxes + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - Convert.ToDecimal(bookingDetail.dAdvNegotiateAmount));
                                }
                            }
                            else if (bookingDetail.BookingStatus == "PP")
                            {
                                purchaseHistory.Amount = bookingDetail.dAdvNegotiateAmount;
                            }
                            else if (bookingDetail.BookingStatus == "NA" && bookingDetail.ActualBookingType == "B")
                            {
                                purchaseHistory.Amount = (bookingDetail.dTotalNegotiateAmount + Convert.ToDecimal(bookingDetail.sExtra2) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - bookingDetail.dAdvNegotiateAmount;
                            }
                            else if (bookingDetail.BookingStatus == "NA" || bookingDetail.BookingStatus == "BP")
                            {
                                purchaseHistory.Amount = bookingDetail.dTotalNegotiateAmount + bookingDetail.dTaxesForHotel + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge - bookingDetail.dAdvNegotiateAmount;
                            }
                            else if (bookingDetail.BookingStatus == "NS")
                            {
                                purchaseHistory.Amount = bookingDetail.dCustomerPayable + bookingDetail.dTaxes + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge - bookingDetail.dAdvNegotiateAmount;
                            }
                            else if (bookingDetail.BookingStatus == "FO")
                            {
                                purchaseHistory.Amount = bookingDetail.dTaxes + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge + bookingDetail.dccPrice - bookingDetail.dAdvNegotiateAmount;
                            }
                            else if(bookingDetail.BookingStatus=="XP")
                            {
                                purchaseHistory.Amount = bookingDetail.dTaxes + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge + bookingDetail.dccPrice - bookingDetail.dAdvNegotiateAmount;
                            }
                        }
                        //For Normal Booking
                        else if (bookingDetail.cBookingType == "R")
                        {
                            if (bookingDetail.BookingStatus == "PP")
                            {
                                purchaseHistory.Amount = ((bookingDetail.dTotalAmount + Convert.ToDecimal(bookingDetail.dTotalExtraBedAmount) + Convert.ToDecimal(bookingDetail.dTaxes) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - Convert.ToDecimal(bookingDetail.dDiscountedBidPrice));

                                if (!string.IsNullOrEmpty(bookingDetail.sProvisionalBookingIdTG))
                                {
                                    purchaseHistory.Amount = ((bookingDetail.dTotalAmount + Convert.ToDecimal(bookingDetail.dTotalExtraBedAmount) + Convert.ToDecimal(bookingDetail.dTaxesForHotel) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - Convert.ToDecimal(bookingDetail.dDiscountedBidPrice));
                                }

                                if (isPointRedumptionPayment.HasValue)
                                {
                                    purchaseHistory.Amount = Convert.ToDecimal(bookingDetail.dTaxesForHotel) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge;    
                                }
                                else
                                {
                                    // Used in Travelguru Booking
                                    payURequestModel.Set("udf4", "FinalPayment");
                                }

                            }
                            if ((bookingDetail.BookingStatus.Trim() == "MR" && bookingDetail.PaymentStatus.Trim() == "P") || (bookingDetail.BookingStatus.Trim() == "MD" && bookingDetail.PaymentStatus.Trim() == "P"))
                            {

                                decimal Balance = Math.Abs(Convert.ToDecimal(bookingDetail.dRefundAmount))-Convert.ToDecimal(bookingDetail.dDiscountedBidPrice);
                                // changed on 02/02/2018
                                purchaseHistory.Amount = Balance;
                                payURequestModel.Set("udf4", "FinalPayment");
                            }

                        }
                        // For booking type Bidding
                        else if (bookingDetail.cBookingType == "B" && bookingDetail.BookingStatus == "PP")
                        {
                            if (bookingDetail.BookingStatus == "PP")
                            {
                                purchaseHistory.Amount = bookingDetail.dTotalAmount + bookingDetail.dTaxes + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge;
                                payURequestModel.Set("udf4", "Bid");
                            }
                        }
                        //for corporate
                        else if (bookingDetail.cBookingType == "C")
                        {
                            if (bookingDetail.BookingStatus == "PP")
                            {
                                purchaseHistory.Amount = ((bookingDetail.dTotalAmount + Convert.ToDecimal(bookingDetail.dTotalExtraBedAmount) + Convert.ToDecimal(bookingDetail.dTaxesForHotel) + bookingDetail.dServiceCharge + bookingDetail.dGSTOnServiceCharge) - Convert.ToDecimal(bookingDetail.dDiscountedBidPrice));
                                payURequestModel.Set("udf4", "FinalPayment");
                            }
                            if ((bookingDetail.BookingStatus.Trim() == "MR" && bookingDetail.PaymentStatus.Trim() == "P") || (bookingDetail.BookingStatus.Trim() == "MD" && bookingDetail.PaymentStatus.Trim() == "P"))
                            {

                                decimal Balance = Math.Abs(Convert.ToDecimal(bookingDetail.dRefundAmount));

                                purchaseHistory.Amount = Balance;
                                payURequestModel.Set("udf4", "FinalPayment");
                            }

                        }
                        //Round function has be to removed when payment gateway is live.
                        purchaseHistory.Amount = Math.Round(Convert.ToDecimal(purchaseHistory.Amount));

                        var insertedPurchagedHistoryId = BL_tblPurchaseHistory.AddRecordAndGetId(purchaseHistory);


                        //Commented to by pas the paymnet gateway
                        var transactionId = (insertedPurchagedHistoryId.ToString() + new Random(999999999).Next().ToString()).Substring(0, 10);

                        payURequestModel.Set("amount", purchaseHistory.Amount.ToString());
                        payURequestModel.Set("productinfo", "OneFineRate");
                        payURequestModel.Set("firstname", userDetails.FirstName);
                        payURequestModel.Set("email", userDetails.Email);

                        payURequestModel.Set("phone", userDetails.PhoneNumber);
                        payURequestModel.Set("key", ConfigurationManager.AppSettings["MERCHANT_KEY"]);
                        payURequestModel.Set("surl", Request.Url.GetLeftPart(UriPartial.Authority) + "/Payment/Success");
                        payURequestModel.Set("furl", Request.Url.GetLeftPart(UriPartial.Authority) + "/Payment/Failure");

                        payURequestModel.Set("txnid", transactionId);
                        payURequestModel.Set("udf1", insertedPurchagedHistoryId.ToString());
                        payURequestModel.Set("udf2", bookingId.ToString());

                        //Being Used for previous url Storage now
                        //payURequestModel.Set("udf3", userDetails.Id.ToString());
                        payURequestModel.Set("udf5", propDetailsGUID);

                        if (previousUrl != null)
                            payURequestModel.Set("udf3", previousUrl.AbsoluteUri.ToString());
                       
                        string paymentUrl = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";

                        var hashTable = PayuUtility.Compute_HashTable_Using_HashSecquence(payURequestModel);

                        var html = PayuUtility.PreparePOSTForm(paymentUrl, hashTable);

                        var skipPayment = Convert.ToBoolean(ConfigurationManager.AppSettings["SkipPayment"]);

                        if (skipPayment)
                        {
                            payURequestModel.Set("status", "success");
                            return Success(payURequestModel, skipPayment);
                        }

                        //html itself is containing the post javascript to post the data into Pa
                        return PartialView("_PayuPost", html);
                    }
                    else
                    {
                        var exceptionModel = new HandleErrorInfo(new Exception("Booking details Not found !"), "Controller", "Action");
                        return View("Error", exceptionModel);
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult Success(FormCollection responseCollection, bool? skipPayment)
        {
            try
            {
                string responseHashString = string.Empty;
                string requestHash = string.Empty;
                string transactionId = string.Empty;
                string[] hashSequence = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10".Split('|');

                if (responseCollection["status"] == "success")
                {
                    Array.Reverse(hashSequence);
                    responseHashString = ConfigurationManager.AppSettings["SALT"] + "|" + responseCollection["status"];

                    foreach (string hash_var in hashSequence)
                    {
                        responseHashString += "|";
                        responseHashString = responseHashString + (responseCollection[hash_var] ?? "");
                    }

                    requestHash = PayuUtility.Generatehash512(responseHashString).ToLower();

                    if (skipPayment.HasValue && skipPayment.Value ? requestHash != responseCollection["hash"] : requestHash == responseCollection["hash"])
                    {
                        var jsonResponseString = ToJSON(responseCollection);

                        var payuResponseModel = JsonConvert.DeserializeObject<PayUResponseModel>(jsonResponseString);

                        var purchageId = Convert.ToInt32(payuResponseModel.udf1);
                        // udf5 have session key for current booking.
                        var propDetailSGUID = payuResponseModel.udf5;


                        var existingTblPurchaseHistory = BL_tblPurchaseHistory.GetRecord(purchageId);

                        var booking = BL_Booking.GetBooking(existingTblPurchaseHistory.iBookingId);
                        int Result = 0;

                        //If page back button is clicked after getting payment reponse
                        if (existingTblPurchaseHistory.ORDERSTATUS == "C")
                        {

                            if (booking.BookingStatus == "NP" || booking.BookingStatus == "CP" || booking.BookingStatus == "PC" || booking.BookingStatus == "PS" || booking.BookingStatus == "FP" || booking.BookingStatus == "NC")
                            {
                                if (booking.bSelfTravelling == null || string.IsNullOrEmpty(booking.sFirstNameOFR))
                                {
                                    return RedirectToRoutePermanent("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(existingTblPurchaseHistory.iBookingId)) });
                                }
                                //Negotiation  it will go to BookingConfirmation
                                else
                                {
                                    return RedirectToRoutePermanent("BookingConfirmation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(existingTblPurchaseHistory.iBookingId)) });
                                }
                            }
                            else if (booking.BookingStatus == "HR" || booking.BookingStatus == "CO" || booking.BookingStatus == "FO")
                            {

                                if (booking.ccType == "CA" || booking.ccType == "BA" || booking.ccType == "RA")
                                {
                                    return RedirectToActionPermanent("BalancePayment", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(existingTblPurchaseHistory.iBookingId)) });
                                }
                            }
                            else if (booking.BookingStatus == "NA")
                            {
                                return RedirectToRoutePermanent("BalancePaymnet", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(existingTblPurchaseHistory.iBookingId)) });
                            }
                            else if (booking.BookingStatus == "BP" || booking.BookingStatus == "NA" || booking.BookingStatus == "CA" || booking.BookingStatus == "NS" || booking.BookingStatus == "FA" || booking.BookingStatus == "BA")
                            {
                                return RedirectToActionPermanent("BalancePayment", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(existingTblPurchaseHistory.iBookingId)) });
                            }
                            else
                            {
                                return RedirectToActionPermanent("Index", "Home");
                            }
                        }

                        existingTblPurchaseHistory.ORDERNO = purchageId;
                        existingTblPurchaseHistory.ORDERSTATUS = "C";
                        existingTblPurchaseHistory.PAYMENTDATE = DateTime.Now;
                        existingTblPurchaseHistory.PAYMETHOD = payuResponseModel.cardCategory;
                        try
                        {
                            existingTblPurchaseHistory.CARD_LAST_DIGITS = payuResponseModel.cardnum.Substring(payuResponseModel.cardnum.Length - 4);
                        }
                        catch
                        {

                        }
                        existingTblPurchaseHistory.CARD_TYPE = payuResponseModel.card_type;
                        existingTblPurchaseHistory.HASH = payuResponseModel.hash;

                        var paramsArr = Request.Params;

                        BL_tblPurchaseHistory.UpdateRecord(existingTblPurchaseHistory);

                        objtrk.iBookingId = existingTblPurchaseHistory.iBookingId;
                        objtrk.dtActionDate = DateTime.Now;

                        if (booking != null)
                        {
                            if (booking.cBookingType == "N" && booking.BookingStatus == "PP")
                            {
                                objtrk.BookingStatus = "NC";

                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "NC";
                                booking.dtActionDate = DateTime.Now;
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    //Inventory Update
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "N"));

                                    // Added to show hotel Information in email template
                                    var bookingModel = BL_Booking.GetBookingModifyDetails(booking.iBookingId);

                                    //Check Negotiation Application
                                    var NgAppl = BL_Booking.CheckIfNegotiationApplicableByBid(booking.iBookingId);
                                    if (NgAppl.Status == 1)
                                    {
                                        //Negotiation Auto Accepted
                                        SendNotifications(Convert.ToInt32(booking.iBookingId), "NegoAccepted");
                                        return RedirectToRoutePermanent("BalancePaymnet", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                    }
                                    else
                                    {
                                        SendNotifications(Convert.ToInt32(booking.iBookingId), "Nego");
                                        return RedirectToActionPermanent("BookingConfirmation", "Negotiation", new { bookingId = clsUtils.Encode(booking.iBookingId.ToString()) });
                                    }
                                }
                            }
                            else if (booking.cBookingType == "N" && (booking.BookingStatus == "NA" || booking.BookingStatus == "BP"))
                            {
                                objtrk.BookingStatus = "NP";

                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "NP";
                                booking.dtActionDate = DateTime.Now;
                                booking.dCustomerPayable = booking.dTotalNegotiateAmount - booking.dAdvNegotiateAmount;
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId));
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "NA"));  //Update Available Rooms Inventory
                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }
                            else if (booking.cBookingType == "N" && booking.BookingStatus == "CA")
                            {
                                objtrk.BookingStatus = "CP";

                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "CP";
                                booking.dtActionDate = DateTime.Now;
                                booking.dCustomerPayable = booking.iCounterOffer - booking.dAdvNegotiateAmount;
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId));
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "NA")); //Update Available Rooms Inventory
                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }
                            else if ((booking.cBookingType == "R") && (booking.BookingStatus.Trim() == "MR" || booking.BookingStatus.Trim() == "MD"))
                            {
                                if ((booking.BookingStatus.Trim() == "MR" || booking.BookingStatus.Trim() == "MD") && booking.PaymentStatus.Trim() == "P")
                                {
                                    objtrk.BookingStatus = booking.BookingStatus;

                                    booking.PaymentStatus = "C";
                                    booking.dtActionDate = DateTime.Now;

                                    //Booking Status Update
                                    Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                    if (Result == 1)
                                    {
                                        SendNotifications(Convert.ToInt32(booking.iBookingId)); //Send email and sms notifications
                                        if (booking.BookingStatus == "MD" || booking.BookingStatus == "MR")
                                        {
                                            return RedirectToRoute("UpdateGuestBookingDetailsAmmend", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                        }
                                        else
                                        {
                                            return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });

                                        }

                                    }
                                }
                            }

                            //Regular Booking Payment
                            else if ((booking.cBookingType == "R" || booking.cBookingType == "N") && (booking.BookingStatus == "PP" || booking.BookingStatus == "NS"))
                            {

                                if (booking.sProvisionalBookingIdTG != null && booking.sProvisionalTrxnTypeTG == "23")
                                {
                                    booking.PaymentStatus = "C";
                                    booking.BookingStatus = "PC";
                                    booking.dCustomerPayable = booking.dTotalAmount + Convert.ToDecimal(booking.dTotalExtraBedAmount);
                                    BL_Booking.UpdateBooking(booking);

                                    return RedirectToAction("FinalBooking", "BookingTG", booking);
                                }

                                objtrk.BookingStatus = "PC";

                                if (booking.BookingStatus == "NS") // for negotiation special booking , on selecting other options
                                {
                                    booking.dCustomerPayable = booking.dCustomerPayable - booking.dAdvNegotiateAmount;
                                }
                                else
                                {
                                    booking.dCustomerPayable = ((booking.dTotalAmount + Convert.ToDecimal(booking.dTotalExtraBedAmount)) - Convert.ToDecimal(booking.dDiscountedBidPrice));
                                }
                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "PC";
                                booking.dtActionDate = DateTime.Now;
                                
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);

                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId));

                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "R")); //Update Available Rooms Inventory

                                    Task.Run(() => BL_WebsiteUser.UpdateLoyalityPointProgressBooking(booking.iBookingId));

                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }
                            else if (booking.cBookingType == "N" && booking.BookingStatus == "HR")
                            {

                                objtrk.BookingStatus = "PC";
                                if (booking.ccType == "RR" || booking.ccType == "RA")
                                {
                                    booking.ActualBookingType = "R";
                                }
                                else
                                {
                                    booking.cBookingType = "R";
                                }

                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "PC";
                                booking.dtActionDate = DateTime.Now;
                                booking.dCustomerPayable = booking.dTotalAmount - Convert.ToDecimal(booking.dAdvNegotiateAmount);
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId)); //Send email and sms notifications
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "NA")); //Update Available Rooms Inventory
                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }
                            else if (booking.cBookingType == "B" && booking.BookingStatus == "PP")
                            {
                                objtrk.BookingStatus = "PC";

                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "PC";
                                booking.dtActionDate = DateTime.Now;
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId), "Bidding"); //Send email and sms notifications
                                    return RedirectToRoute("GetBidHotelsList", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }
                            else if (booking.cBookingType == "N" && booking.BookingStatus == "FO")
                            {
                                objtrk.BookingStatus = "FA";

                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "FA";
                                booking.dtActionDate = DateTime.Now;
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId)); //Send email and sms notifications
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "NA")); //Update Available Rooms Inventory
                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }
                            else if ((booking.cBookingType == "C") && (booking.BookingStatus.Trim() == "MR" || booking.BookingStatus.Trim() == "MD"))
                            {
                                if ((booking.BookingStatus.Trim() == "MR" || booking.BookingStatus.Trim() == "MD") && booking.PaymentStatus.Trim() == "P")
                                {
                                    objtrk.BookingStatus = booking.BookingStatus;

                                    booking.PaymentStatus = "C";
                                    booking.dtActionDate = DateTime.Now;
                                    Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                    if (Result == 1)
                                    {
                                        SendNotifications(Convert.ToInt32(booking.iBookingId)); //Send email and sms notifications
                                        if (booking.BookingStatus == "MD" || booking.BookingStatus == "MR")
                                        {
                                            return RedirectToRoute("UpdateGuestBookingDetailsAmmend", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                        }
                                        else
                                        {
                                            return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                        }
                                    }
                                }
                            }
                            else if ((booking.cBookingType == "C" && booking.BookingStatus == "PP"))
                            {

                                objtrk.BookingStatus = "PC";
                                booking.PaymentStatus = "C";
                                booking.dtActionDate = DateTime.Now;
                                booking.BookingStatus = "PC";
                                booking.dCustomerPayable = ((booking.dTotalAmount + Convert.ToDecimal(booking.dTotalExtraBedAmount)));
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                var bookingModel = BL_Booking.GetBookingModifyDetails(booking.iBookingId);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId));
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "C")); //Update Available Rooms Inventory
                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });

                                }
                            }
                            else if (booking.cBookingType == "N" && booking.BookingStatus == "XP")
                            {
                                objtrk.BookingStatus = "XP";
                                booking.PaymentStatus = "C";
                                booking.BookingStatus = "FA";
                                booking.dtActionDate = DateTime.Now;
                                Result = BL_Booking.UpdateBookingStatus(booking, objtrk);
                                if (Result == 1)
                                {
                                    SendNotifications(Convert.ToInt32(booking.iBookingId)); //Send email and sms notifications
                                    Task.Run(() => BL_Booking.UpdatePropertyRoomInventory(Convert.ToInt32(booking.iBookingId), "NA")); //Update Available Rooms Inventory
                                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                                }
                            }

                            ///Used for bidding upgrade.
                            if (Session[propDetailSGUID] != null)
                            {
                                var obj = new eBiddingHotelsUpgradeRoomsList();
                                obj = Session[propDetailSGUID] as eBiddingHotelsUpgradeRoomsList;


                                BL_Bidding.UpdateUpgradeBidBooking(Convert.ToInt64(booking.iBookingId), obj.iPropId, obj.iRoomId, obj.TotalDifferenceConverted, obj.TaxDifference);
                                Session[propDetailSGUID] = null;
                                if (obj.TotalDifference > 0)
                                {
                                    var upgradeModel = new NegotiationEmailTempleteModel();

                                    //  var booking = BL_Booking.GetBooking(BookingId);

                                    upgradeModel.Status = "Thank you " + booking.sTitleOFR + " " + booking.sFirstNameOFR + " " + booking.sLastNameOFR + "! We have recieved your final payment.";
                                    string bidNotificationMsg = "Thank you for your payment of INR " + Math.Round(obj.TotalDifference) + " for your booking id: " + booking.iBookingId;

                                    //Task.Run(() => MailComponent.SendEmailAsync(booking.sEmailOFR, "", "", "OneFineRate- Payment Confirmation", customerModel.Status, null, null, false));
                                    clsUtils.sendSMS(booking.sMobileOFR, bidNotificationMsg);
                                }

                                return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(booking.iBookingId)) });
                            }
                        }

                        //if (!string.IsNullOrEmpty(propDetailSGUID))
                        //{
                        //    // Remove session key.
                        //    if (Session[propDetailSGUID] != null)
                        //        Session.Remove(propDetailSGUID);
                        //}
                    }
                    else
                    {
                        Response.Write("Opps!, An Unkonwn error had happened !");
                    }
                }

                else
                {
                    Response.Write("Sorry!, An Error occurred ! " + responseCollection["status"].ToString());
                }
            }

            catch (Exception ex)
            {
                Task.Run(() => MailComponent.SendEmail("Deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com", "", "", "Payment Success", ex.ToString(), null, null, false, null, null));
                Response.Write("<span style='color:red'>" + ex.ToString() + "</span>");
            }
            return View("Failure");
        }

        public void SendNotifications(int BookingId, string Type = "")
        {
            try
            {
                var bookingModel = BL_Booking.GetBookingModifyDetails_Notifications(BookingId);
                if (bookingModel.cBookingType == "R" && bookingModel.cBookingStatus == "PC")
                {
                    //Redeem Condition
                    if (bookingModel.sExtra3 == "Redeem")
                        bookingModel.SubTotal = bookingModel.dTaxesForHotel.ToString();


                    string message = "Thank you for your payment of " + bookingModel.sCurrencyCode + " " + Math.Round(Convert.ToDecimal(bookingModel.SubTotal) + Convert.ToDecimal(bookingModel.dOfrServiceCharge) + Convert.ToDecimal(bookingModel.dOfrGSTONServiceCharge)) + " for your booking id: " + bookingModel.BookingId;
                    clsUtils.sendSMS(bookingModel.MobileOFR, message);
                }

                if (bookingModel.cBookingType == "C" && bookingModel.cBookingStatus == "PC")
                {
                    string message = "Thank you for your payment of " + bookingModel.sCurrencyCode + " " + Math.Round(Convert.ToDecimal(bookingModel.SubTotal) + Convert.ToDecimal(bookingModel.dOfrServiceCharge) + Convert.ToDecimal(bookingModel.dOfrGSTONServiceCharge)) + " for your booking id: " + bookingModel.BookingId;
                    clsUtils.sendSMS(bookingModel.MobileOFR, message);
                }

                if (bookingModel.cBookingType == "N" && bookingModel.cBookingStatus == "NP")
                {
                    string message = "Thank you for your payment of " + bookingModel.sCurrencyCode + " " + Math.Round((Convert.ToDecimal(bookingModel.dSubTotalNego) + Convert.ToDecimal(bookingModel.dOfrServiceCharge) + Convert.ToDecimal(bookingModel.dOfrGSTONServiceCharge) - Convert.ToDecimal(bookingModel.dAdvNegotiateAmount))) + " for your booking id: " + bookingModel.BookingId;
                    clsUtils.sendSMS(bookingModel.MobileOFR, message);
                }

                Task.Run(() => BL_Booking.UpdateDaywiseBookingRate(Convert.ToInt32(BookingId)));

                Task.Run(() => BL_Booking.UpdateLoyalityPointsForBooking(Convert.ToInt32(BookingId)));

                var customerModel = new NegotiationEmailTempleteModel();
                customerModel.BookingModify = bookingModel;

                if (Type == "NegoAccepted")
                {
                    customerModel.CallbackUrl = Url.Action("BalancePayment", "Negotiation", new RouteValueDictionary(new { bookingId = clsUtils.Encode(BookingId.ToString()) }), HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Host);
                    customerModel.Status = "Thank you " + bookingModel.Booker + "! Your Negotiation is confirmed.";
                    customerModel.IsNegotiationAcceptedBySystem = true;

                    string message = "Thank you for your payment of " + bookingModel.sCurrencyCode + " " + Math.Round((Convert.ToDecimal(bookingModel.dAdvNegotiateAmount))) + " for your booking id: " + bookingModel.BookingId;
                    clsUtils.sendSMS(bookingModel.MobileOFR, message);

                    var html_Customer = this.RenderViewToString("_EmailTemplete", customerModel);
                    Task.Run(() => MailComponent.SendEmail(bookingModel.EmailOFR, "", "", "Negotiation Detail", html_Customer, null, null, true, null, null));
                    clsUtils.sendSMS(bookingModel.MobileOFR, customerModel.Status);
                }
                else if (Type == "Bidding")
                {
                    customerModel = new NegotiationEmailTempleteModel();

                    var booking = BL_Booking.GetBooking(BookingId);

                    customerModel.Status = "Thank you " + booking.sTitleOFR + " " + booking.sFirstNameOFR + " " + booking.sLastNameOFR + "! We have recieved your final payment.";
                    string bidNotificationMsg = "Thank you for your payment of INR " + Math.Round(booking.dTotalAmount.Value + booking.dTaxes.Value + booking.dServiceCharge + booking.dGSTOnServiceCharge) + " for your booking id: " + booking.iBookingId;

                    //Task.Run(() => MailComponent.SendEmailAsync(booking.sEmailOFR, "", "", "OneFineRate- Payment Confirmation", customerModel.Status, null, null, false));
                    clsUtils.sendSMS(booking.sMobileOFR, bidNotificationMsg);

                }
                else if (Type == "Nego")
                {
                    var revenueManagerDetail = BL_tblEmailSettingsM.GetRecord("RevenueManager");

                    var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(bookingModel.PropId);

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

                    customerModel = new NegotiationEmailTempleteModel();
                    customerModel.BookingModify = bookingModel;

                    string negoStatusLink_User = Request.Url.GetLeftPart(UriPartial.Authority) + "/Negotiation/NegotiationStatus?bookingId=" + clsUtils.Encode(bookingModel.BookingId);

                    string shortNegoStatusLink_User = clsUtils.Shorten(negoStatusLink_User);

                    string negotiationMessage_User = "Thank you for your initial payment of " + bookingModel.sCurrencyCode + " " + Math.Round(bookingModel.dAdvNegotiateAmount.Value) + ". We have begun work  to close this transaction. For reservation status check, use reference number " + bookingModel.BookingId + " on the following link " + shortNegoStatusLink_User;

                    customerModel.Status = "Thank you " + bookingModel.Booker + "! We have recieved your bargain." + Environment.NewLine + bookingModel.sMessage;

                    string extranetUrl = System.Configuration.ConfigurationManager.AppSettings["OFRExtranetBaseUrl"] + "Account/Login?sPropId=" + clsUtils.Encode(bookingModel.PropId.ToString());

                    string shortExtranetUrl = clsUtils.Shorten(extranetUrl);

                    string notificationMsg_RevenueMgr = "You have received a new bargain enquiry. The guest has chosen your hotel! Click this link " + shortExtranetUrl + " to view the offer and process yet another reservation from OFR. This offer is valid for the next three hours.";
                    var revenuManagerModel = new NegotiationEmailTempleteModel();
                    revenuManagerModel.Notification_Link = shortExtranetUrl;
                    revenuManagerModel.IsRevenueManagerFormat = true;

                    revenuManagerModel.BookingModify = bookingModel;

                    clsUtils.sendSMS(sRevenueManagerContact, notificationMsg_RevenueMgr);

                    clsUtils.sendSMS(bookingModel.MobileOFR, negotiationMessage_User);

                    var html_Customer = this.RenderViewToString("_NegotiationEmailTemplete", customerModel);

                    var html_RevenueManager = this.RenderViewToString("_NegotiationEmailTemplete", revenuManagerModel);

                    //TO DO
                    string ccEmail = string.Empty;
                    ccEmail = "himanshuS@futuresoftindia.com";

                    Task.Run(() => MailComponent.SendEmail(bookingModel.EmailOFR, ccEmail, "", "OneFineRate- Bargain! Confirmation No : " + bookingModel.BookingId, html_Customer, null, null, true, null, null));
                    Task.Run(() => MailComponent.SendEmail(sRevenueManagerEmail, ccEmail, "", "OneFineRate- New Bargain! Confirmation No : " + bookingModel.BookingId, html_RevenueManager, null, null, true, null, null));
                }
                else
                {
                    //return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(BookingId)) });
                    //customerModel = new NegotiationEmailTempleteModel();
                    //customerModel.BookingModify = bookingModel;
                    //customerModel.Status = "Thanks " + bookingModel.Booker + "! We have recieved your final payment. Click the link below to fill the guest information." + Environment.NewLine + "";
                    //var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                    //customerModel.CallbackUrl = websiteBaseUrl + "GuestInformation/" + clsUtils.Encode(BookingId.ToString());

                    //customerModel.Status = "Thanks " + bookingModel.Booker + "! " + Environment.NewLine + "Click below to fill guest information.";
                    //customerModel.CallbackUrl = websiteBaseUrl + "GuestInformation/" + clsUtils.Encode(BookingId.ToString());
                    //html_Customer = this.RenderViewToString("_EmailTemplete", customerModel);
                    //Task.Run(() => MailComponent.SendEmailAsync(bookingModel.EmailOFR, "", "", "Guest Details", html_Customer, null, null, true));
                    //Task.Run(() => clsUtils.sendSMS(bookingModel.MobileOFR, "We have recieved your final payment. Click the link below to fill the guest information." + Environment.NewLine + customerModel.CallbackUrl));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void SendInvoice(long bookingId)
        {
            var bookingDetails = BL_Invoice.GetInvoiceDetailByBooking(bookingId);

            bookingDetails.sInvoiceNumber = bookingDetails.iBookingId + "/" + "01" + (bookingDetails.cBookingStatus == "MD" ? "/MOD" : "");
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

            bookingDetails.sInvoiceNumber = bookingDetails.iBookingId + "/" + "02" + (bookingDetails.cBookingStatus == "MD" ? "/MOD" : "");
            bookingDetails.HotelOrGuest = HotelOrGuest.Hotel;

            var html_Hotel = this.RenderViewToString("_Invoice", bookingDetails);

            //TO DO
            var ccMail = string.Empty;
            ccMail = "himanshuS@futuresoftindia.com";
            //sRevenueManagerEmail = "himanshuS@futuresoftindia.com";

            Task.Run(() =>
            {

                MailComponent.SendEmail(sRevenueManagerEmail, ccMail, "", "Invoice Detail", html_Hotel, null, null, true, null, null);
                MailComponent.SendEmail(bookingDetails.sEmailOFR, ccMail, "", "Invoice Detail", html_Customer, null, null, true, null, null);

            });
        }

        [HttpPost]
        public ActionResult Failure(FormCollection responseCollection)
        {
            ePaymentFailure obj = new ePaymentFailure();
            var error_Message = responseCollection["error_Message"].ToString();
            var error = responseCollection["error"].ToString();

            try
            {
                string responseHashString = string.Empty;
                string requestHash = string.Empty;
                string transactionId = string.Empty;
                string[] hashSequence = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10".Split('|');

                Array.Reverse(hashSequence);
                responseHashString = ConfigurationManager.AppSettings["SALT"] + "|" + responseCollection["status"];

                foreach (string hash_var in hashSequence)
                {
                    responseHashString += "|";
                    responseHashString = responseHashString + (responseCollection[hash_var] ?? "");
                }

                requestHash = PayuUtility.Generatehash512(responseHashString).ToLower();

                if (requestHash == responseCollection["hash"])
                {

                    var jsonResponseString = ToJSON(responseCollection);

                    var payuResponseModel = JsonConvert.DeserializeObject<PayUResponseModel>(jsonResponseString);

                    var purchageId = Convert.ToInt32(payuResponseModel.udf1);

                    var existingTblPurchaseHistory = BL_tblPurchaseHistory.GetRecord(purchageId);

                    existingTblPurchaseHistory.ORDERNO = purchageId;
                    existingTblPurchaseHistory.ORDERSTATUS = "C";
                    existingTblPurchaseHistory.PAYMENTDATE = DateTime.Now;
                    //existingTblPurchaseHistory.PAYMETHOD = payuResponseModel.cardCategory;
                    //existingTblPurchaseHistory.CARD_LAST_DIGITS = payuResponseModel.cardnum.Substring(payuResponseModel.cardnum.Length - 4);
                    //existingTblPurchaseHistory.CARD_TYPE = payuResponseModel.card_type;
                    existingTblPurchaseHistory.HASH = payuResponseModel.hash;

                    try
                    {
                        BL_tblPurchaseHistory.UpdateRecord(existingTblPurchaseHistory);
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {

                            }
                        }
                        throw e;
                    }


                    obj.BookingId = Convert.ToInt32(existingTblPurchaseHistory.iBookingId);
                    obj.ErrorMessage = Convert.ToString(payuResponseModel.error_Message);
                    obj.ErrorType = Convert.ToString(payuResponseModel.error_Message);

                    var booking = BL_Booking.GetBooking(existingTblPurchaseHistory.iBookingId);

                    objtrk.iBookingId = existingTblPurchaseHistory.iBookingId;
                    objtrk.dtActionDate = DateTime.Now;

                    booking.PaymentStatus = "R";
                    booking.dtActionDate = DateTime.Now;
                    BL_Booking.UpdateBookingStatus(booking, objtrk);

                    var decodeUrl = System.Net.WebUtility.HtmlDecode(payuResponseModel.udf3);

                    if (payuResponseModel.error == "E1605")
                        return Redirect(decodeUrl);


                    ViewBag.HeaderBarData = "Transaction Failed !";
                }
            }
            catch (Exception ex)
            {
                Task.Run(() => MailComponent.SendEmail("Deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com", "", "", "Payment Failure", ex.ToString(), null, null, false, null, null));
            }
            return View(obj);
        }

        public string ToJSON(FormCollection collection)
        {
            var list = new Dictionary<string, string>();
            foreach (string key in collection.Keys)
            {
                list.Add(key, collection[key]);
            }
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list);
        }

        private eProvisionalBookingResponseModel TravelGuruFinalBooking(long bookingId)
        {
            var bookingTx = BL_Booking.GetBookingDetailForFinalBooking(bookingId);

            var requestModel = new eProvisionalBookingRequestModel();
            requestModel.CheckIn = bookingTx.dtCheckIn.Value.ToString("yyyy-MM-dd");
            requestModel.CheckOut = bookingTx.dtChekOut.Value.ToString("yyyy-MM-dd");
            requestModel.Currency = bookingTx.sCurrencyCode;
            requestModel.RatePlanCode = bookingTx.sRatePlanCode;
            requestModel.RoomTypeCode = bookingTx.sRoomId;
            requestModel.HotelCode = bookingTx.sVendorId;

            var response = clsBooking.FinalBooking(requestModel);

            if (response.IsSuccedded)
            {
                Task.Run(() =>
                {
                    bookingTx.sFinalBookingIdTG = response.UniqueId;
                    bookingTx.sFinalTrxnTypeTG = response.UniqueIdType;

                    bookingTx.BookingStatus = "PC";
                    bookingTx.dtActionDate = DateTime.Now;

                    etblBookingTrakerTx dbBookingTracker = new etblBookingTrakerTx();
                    dbBookingTracker.iBookingId = bookingTx.iBookingId;
                    dbBookingTracker.BookingStatus = "PC";
                    dbBookingTracker.dtActionDate = DateTime.Now;

                    BL_Booking.UpdateBookingStatus(bookingTx, dbBookingTracker);

                    BL_Booking.UpdateDayWiseBookingRate(bookingTx.iBookingId);

                });
            }

            return response;
        }


        public ActionResult Refund(object model)
        {
            return View(model);
        }
    }
}