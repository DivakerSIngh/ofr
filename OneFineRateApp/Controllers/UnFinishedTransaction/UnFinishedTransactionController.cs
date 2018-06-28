using FutureSoft.Util;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OneFineRateApp.App_Helper;
using System.Configuration;
using System.Data;

namespace OneFineRateApp.Controllers.UnFinishedTransaction
{
    public class UnFinishedTransactionController : BaseController
    {
        // GET: UnFinishedTransaction
        [Route("UnFinishedTransaction")]
        public ActionResult Index()
        {
            if (Session["Result"] != null)
                ViewData["Result"] = Session["Result"].ToString();
            else
                ViewData["Result"] = "";

            Session["Result"] = null;
            etblUnfinishedTransactions obj = new etblUnfinishedTransactions();
            return View(obj);
        }


        #region UnfinishedBalanceToBePaid

        public ActionResult UnfinishedBalanceToBePaid(etblUnfinishedTransactionsToBePaid objprop)
        {
            etblUnfinishedTransactionsToBePaid objData = new etblUnfinishedTransactionsToBePaid();
            if (objprop.iBookingID != null)
            {

                objData = BL_UnfinishedTransactions.GetUnfinishedBalanceToBePaidData(objprop.iBookingID);
            }
            else
            {
                long BookingId = 0;
                if (HttpContext.Request.Params["Id"] != null) { BookingId = Convert.ToInt32(HttpContext.Request.Params["Id"]); }
                //objData = BL_UnfinishedTransactions.GetUnfinishedBalanceToBePaidData(BookingId);
                var obj = BL_Booking.GetUnfinishedTransactionToSendRevenueManager(BookingId);
                obj.sBookType = "N";
                return View("NegotiationAccepted", obj);
            }

            return View("UnfinishedBalanceToBePaid", objData);
        }
        public ActionResult UnfinishedBalanceToBePaidSendLink(etblUnfinishedTransactionsToBePaid objData)
        {
            string adminEmail = objData.sCustomerEmailID;
            string adminsCcEmail = "";// "sumits@futuresoftindia.com";
            string adminsBCcEmail = "";
            string adminsSubject = "Unfinished Transaction - Balance To Be Paid";
            string adminsBody = "<table border='1'>";
            adminsBody += "<tr><td>Hotel Name: </td><td>" + objData.sHotelName + "</td>";
            adminsBody += "<tr><td>Star Category: </td><td>" + objData.iStarCategory + "</td>";
            adminsBody += "<tr><td>Address: </td><td>" + objData.sHotelAddress + "</td>";
            adminsBody += "<tr><td>Your Negotiation Amount: </td><td>" + objData.dNegotiationAmount + "</td>";
            adminsBody += "<tr><td>Advanced Paid: </td><td>" + objData.dAdvancePaid + "</td>";
            adminsBody += "<tr><td>Balance Payment: </td><td>" + objData.dBalancePayment + "</td>";
            adminsBody += "<tr><td colspna='2'><a href='onefinerate.azurewebsites.net'>Click To Pay Balance Amount</a> </td>";
            adminsBody += "</table>";

            MailComponent.SendEmail(adminEmail, adminsCcEmail, adminsBCcEmail, adminsSubject, adminsBody, null, null, true,null,null);
            TempData["msg"] = "Link sent to customer successfully";

            return RedirectToAction("UnfinishedBalanceToBePaid", objData);

        }

        #endregion

        #region UnfinishedHotelNotSelected

        public ActionResult UnfinishedHotelNotSelected(long bookingId)
        {
            eBidding objdata = new eBidding();
            objdata = BL_Bidding.GetSearchedBidHotelsListForUnfinished(bookingId);
            objdata.iBookingId = (int)bookingId;
            TempData["BidSearchHotels"] = objdata;
            return View(objdata);
        }

        public ActionResult UpdateBooking(eBidding obj)
        {
            try
            {
                int bookingId, PropId;
                PropId = obj.SelectedPropId;
                bookingId = obj.iBookingId;

                eBidBookingResult Robj = BL_Bidding.UpdateBidBooking(bookingId, PropId);
                if (Robj.Status == "Success")
                {
                    var bookingModel = BL_Booking.GetBooking(bookingId);

                    string Status = "Thank you " + bookingModel.sTitleOFR + " " + bookingModel.sFirstNameOFR + "! We have recieved your final payment.";
                    Task.Run(() => MailComponent.SendEmail(bookingModel.sEmailOFR, "", "", "Hotel Selected", Status, null, null, false, null, null));
                    Task.Run(() => clsUtils.sendSMS(bookingModel.sMobileOFR, Status));
                    Session["Result"] = "Hotel selected successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception E)
            {
                Session["Result"] = "There was an error selecting the hotel. Please try again.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetUpgradeHotelDetails(int bookingid, int propid)
        {
            var obj = new eBidding();

            if (TempData["BidSearchHotels"] != null) { TempData.Keep(); obj = TempData["BidSearchHotels"] as eBidding; }


            var prop = obj.lstBidRoomsData.Where(u => u.iPropId == propid).SingleOrDefault();


            BiddingHotelsUpgrade objUpgrade = BL_Bidding.GetHotelUpgradeResult(propid, bookingid);

            objUpgrade.objBidRoomsData = prop;
            objUpgrade.iBookingId = bookingid;
            objUpgrade.iPropId = propid;

            TempData["BIdUpgradeHotels"] = objUpgrade;
            return PartialView("BidInfo", objUpgrade);
        }

        public ActionResult UpdateBookingOnUpgrade(BiddingHotelsUpgrade obj)
        {
            try
            {
                TempData.Peek("BIdUpgradeHotels");
                TempData.Peek("BidSearchHotels");
                TempData.Peek("BidSearchData");
                int bookingId, PropId;
                PropId = obj.iPropId;
                bookingId = obj.iBookingId;

                eBidBookingResult Robj = BL_Bidding.UpdateBidBooking(bookingId, PropId);
                if (Robj.Status == "Success")
                {
                    var bookingModel = BL_Booking.GetBooking(bookingId);

                    string Status = "Thank you " + bookingModel.sTitleOFR + " " + bookingModel.sFirstNameOFR + "! We have recieved your final payment.";
                    Task.Run(() => MailComponent.SendEmail(bookingModel.sEmailOFR, "", "", "Hotel Selected", Status, null, null, false, null, null));
                    Task.Run(() => clsUtils.sendSMS(bookingModel.sMobileOFR, Status));
                    Session["Result"] = "Hotel selected successfully!";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception E)
            {
                Session["Result"] = "There was an error selecting the hotel. Please try again.";
            }
            return RedirectToAction("Index");
        }
        public ActionResult GetLeftPayment(BiddingHotelsUpgrade objUpgrade)
        {
            try
            {
                var obj = new BiddingHotelsUpgrade();
                if (TempData["BIdUpgradeHotels"] != null) { TempData.Keep(); obj = TempData["BIdUpgradeHotels"] as BiddingHotelsUpgrade; }

                var prop = obj.lstRoomsData.Where(u => u.iRoomId == objUpgrade.iRoomId).SingleOrDefault();

                var obj1 = new eBiddingHotelsUpgradeRoomsList();
                obj1 = prop as eBiddingHotelsUpgradeRoomsList;
                var result = BL_Bidding.UpdateUpgradeBidBookingUnfinished(Convert.ToInt64(objUpgrade.iBookingId), obj.iPropId, objUpgrade.iRoomId, obj1.TotalDifference, obj1.TaxDifference, objUpgrade.sAuthCode);
                if (result.Status == "Success")
                {
                    var bookingModel = BL_Booking.GetBooking(Convert.ToInt64(objUpgrade.iBookingId));

                    string Status = "Thank you " + bookingModel.sTitleOFR + " " + bookingModel.sFirstNameOFR + "! We have recieved your final payment.";
                    Task.Run(() => MailComponent.SendEmail(bookingModel.sEmailOFR, "", "", "Hotel Selected", Status, null, null, false, null, null));
                    Task.Run(() => clsUtils.sendSMS(bookingModel.sMobileOFR, Status));
                    Session["Result"] = "Hotel selected successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception E)
            {
                Session["Result"] = "There was an error selecting the hotel. Please try again.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult GotCounterOffer()
        {
            long? BookingId = 0;
            if (HttpContext.Request.Params["Id"] != null) { BookingId = Convert.ToInt32(HttpContext.Request.Params["Id"]); }
            ViewBag.HeaderBarData = "Preview";
            BalancePaymentModel model = new BalancePaymentModel();

            model = BL_Booking.GetNegotionTransactionDataForBooking(Convert.ToInt32(BookingId));
            ViewBag.StatusType = model.cBookingStatus;
            return View("NegotiationCounterOffer", model);
        }
        #endregion

        #region NegotiationForHotelPending

        public ActionResult NegotiationForHotelPending(long bookingId)
        {
            ViewBag.HeaderBarData = "Confirmation";
            var BookingModel = BL_Booking.GetBookingModifyDetails(bookingId);
            BookingModel.Symbol = "₹";
            return View(BookingModel);
        }

        // GET: Room Details
        public ActionResult RoomInfo(string propid, string roomid)
        {
            string CurrencyCode = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";
            RoomDetails objRoomDetails = BL_PropDetails.GetRoomDetails(Convert.ToInt32(propid), Convert.ToInt32(roomid), CurrencyCode);
            return PartialView("pvRoomInfo", objRoomDetails);
        }

        public int SendMailToRevenueManager(int bookingId)
        {
            try
            {
                var customerModel = new NegotiationEmailTempleteModel();
                // Added to show hotel Information in email template
                var bookingModel = BL_Booking.GetBookingModifyDetails_Notifications(bookingId);
                customerModel.BookingModify = bookingModel;


                Decimal? ExchangeRate = 1;
                // get from tblEmailSetting
                var revenueManagerDetail = BL_tblEmailSettingsM.GetRecord("RevenueManager");

                // get from propertyEdit page 
                var pRevenueManager = BL_PropDetails.GetEmail_PhoneByPropId(bookingModel.PropId);

                string sPrimaryContactEmail;
                string sConfirmationContactEmail;
                string sRevenueManagerEmail;
                string sRevenueManagerContact;

                pRevenueManager.TryGetValue("sPrimaryContactEmail", out sPrimaryContactEmail);
                pRevenueManager.TryGetValue("sConfirmationContactEmail", out sConfirmationContactEmail);
                pRevenueManager.TryGetValue("sRevenueManagerEmail", out sRevenueManagerEmail);
                pRevenueManager.TryGetValue("sRevenueManagerContact", out sRevenueManagerContact);

                string emails = sRevenueManagerEmail + "," + sConfirmationContactEmail + "," + sPrimaryContactEmail;


                customerModel = new NegotiationEmailTempleteModel();
                customerModel.BookingModify = bookingModel;
                //customerModel.Status = "Thanks " + bookingModel.Booker + "! We have recieved your negotiation." + Environment.NewLine + "We will get back to you within 3 hours.";
                customerModel.Status = "Thank you " + bookingModel.Booker + "! We have recieved your bargain." + Environment.NewLine + bookingModel.sMessage;

                var revenuManagerModel = new NegotiationEmailTempleteModel();
                //revenuManagerModel.Status = "Hi " + revenueManageName +", Please take action for the following Negotiation Request.";
                revenuManagerModel.Status = "Hi, Please take action for the following bargain Request.";
                revenuManagerModel.IsRevenueManagerFormat = true;


                if (bookingModel.sCurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById( "INR", bookingModel.sCurrencyCode);
                    if (objExchange.dRate != null)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }
                bookingModel.dTotalNegotiateAmount = bookingModel.dTotalNegotiateAmount * ExchangeRate;
                bookingModel.dTaxes = Convert.ToDecimal(bookingModel.Tax) * Convert.ToDecimal(ExchangeRate);
                revenuManagerModel.BookingModify = bookingModel;

                string extranetUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "Account/Login?sPropId=" + clsUtils.Encode(bookingModel.PropId.ToString());

                string shortExtranetUrl = clsUtils.Shorten(extranetUrl);

                string notificationMsg_RevenueMgr = "You have received a new bargain offer. The guest has chosen your hotel! Click " + shortExtranetUrl + " to view the offer and process yet another reservation from OFR. This offer is valid for the next three hours.";

                var html_RevenueManager = this.RenderViewToString("_NegotiationEmailTemplete", revenuManagerModel);

                Task.Run(() => clsUtils.sendSMS(sRevenueManagerContact, notificationMsg_RevenueMgr));

                Task.Run(() => MailComponent.SendEmail(sRevenueManagerEmail, revenueManagerDetail.sEmail, "", "Reminder - New Bargain", html_RevenueManager, null, null, true, null, null));

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int SendCustAcceptReminderEmail(int bookingId)
        {
            try
            {
                string notificationSMS_User = string.Empty;
                var bookingDetails = BL_Booking.GetBookingModifyDetails(bookingId);
                string statusType = bookingDetails.cBookingStatus;
                var model = new NegotiationEmailTempleteModel();
                model.BookingModify = bookingDetails;
                var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                string negoStatusUrl = websiteBaseUrl + "Negotiation/NegotiationStatus?bookingId=" + clsUtils.Encode(bookingId.ToString());

                string shortNegoStatusUrl = clsUtils.Shorten(negoStatusUrl);

                //bool Accepted = true;
                switch (statusType) // Notification mail/SMS to customer. Conserve commission checked and managed.
                {
                    case "HR": // Hotel has rejected the negotiation. Conserve commission not applied.
                        //model.Status = "Sorry! Your Negotiation is Rejected. Please Check the related offer details by clicking the link below";
                        notificationSMS_User = "Sorry! We tried our best. Your rate offer was low for your chosen dates. There are other savings to be made on related offers. Click the link to continue your savings with One Fine Rate. " + shortNegoStatusUrl;
                        model.Status = notificationSMS_User;
                        //Accepted = false;
                        break;
                    case "CO": // Hotel has countered the negotiation.
                    case "RR": // Rate reduced for "Original rate" through conserve commission.
                    case "CR": // Rate reduced for Counter offer through conserve commission.
                    case "FO": // Final Offer given.
                        model.Status = "Progress! You have received a  counter offer ! Please check the details by clicking the link below: ";
                        notificationSMS_User = model.Status;
                        //Accepted = false;
                        break;
                    case "BP": // Hotel has accepted the negotiation.
                    case "RA": // Negotiation accepted by reducing conserve commission on "Original rate".
                    case "NA": // Negotiation accepted.
                        model.Status = "Congratulations! Your offer is accepted! Please Check the details by clicking the link below: ";
                        notificationSMS_User = model.Status;
                        break;
                    case "CA": //  counter offer accepted.
                    case "FA": //  Final offer accepted.
                        model.Status = "Counter offer was accepted by you! Please Check the details by clicking the link below: ";
                        notificationSMS_User = model.Status;
                        break;
                    case "OA": //  counter offer accepted.
                    case "NS": //  counter offer accepted.
                        model.Status = "Please Check the details by clicking the link below: ";
                        notificationSMS_User = model.Status;
                        break;
                }

                // if (!Accepted)
                //  model.CallbackUrl = websiteBaseUrl + "Negotiation/NegotiationStatus?bookingId=" + clsUtils.Encode(bookingId.ToString());
                model.CallbackUrl = shortNegoStatusUrl;
                //else
                //  model.CallbackUrl = websiteBaseUrl + "NegoConfirmed/" + clsUtils.Encode(bookingId.ToString());

                var html = this.RenderViewToString("_EmailTemplete", model);

                Task.Run(() => clsUtils.sendSMS(bookingDetails.MobileOFR, notificationSMS_User + model.CallbackUrl));

                Task.Run(() => MailComponent.SendEmail(bookingDetails.EmailOFR, "", "", "Bargain Status", html, null, null, true, null, null));

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string ProvideFinalOffer(int BookingId, int FinalOffer, int Convert)
        {
            try
            {
                string s = BL_Booking.ProvideFinalOffer(BookingId, FinalOffer, ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert);
                if (s == "")
                {

                    var bookingDetails = BL_Booking.GetBookingModifyDetails_Notifications(BookingId);
                    var model = new NegotiationEmailTempleteModel();
                    model.Status = "Progress! You have received a  counter offer! Please check the details by clicking the link below: ";

                    bookingDetails.dTaxes = System.Convert.ToDecimal(bookingDetails.sExtra2);
                    model.BookingModify = bookingDetails;

                    var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                    model.CallbackUrl = websiteBaseUrl + "Negotiation/NegotiationStatus?bookingId=" + clsUtils.Encode(BookingId.ToString());

                    model.CallbackUrl = clsUtils.Shorten(model.CallbackUrl);                    
                    // string notifySMS_User = model.Status + Environment.NewLine + "<a href=\"" + model.CallbackUrl + "\"></a>";

                    var html = this.RenderViewToString("_EmailTemplete", model);
                    var testEmail = "";

                    Task.Run(() => clsUtils.sendSMS(bookingDetails.MobileOFR, model.Status + model.CallbackUrl));
                    Task.Run(() => MailComponent.SendEmail(bookingDetails.EmailOFR, testEmail, "", "Bargain Status", html, null, null, true, null, null));

                    return "{ \"st\" : 1 , \"Status\" : \"Final offer provided successfully.\" }";
                }
                else
                {
                    return "{ \"st\" : 0 , \"Status\" : \"" + s + "\" }";
                }
            }
            catch (Exception)
            {
                return "{ \"st\" : 0 , \"Status\" : \"Please try again.\" }";
            }
        }

        #endregion


        public string ReleaseInventory(int bookingId)
        {
            try
            {
                string s = BL_Booking.ReleaseInventory(bookingId, ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                if (s == "")
                {
                    return "{ \"st\" : 1 , \"Status\" : \"Inventory released successfully.\" }";
                }
                else
                {
                    return "{ \"st\" : 0 , \"Status\" : \"" + s + "\" }";
                }
            }
            catch (Exception)
            {
                return "{ \"st\" : 0 , \"Status\" : \"Please try again.\" }";
            }
        }

        [Route("BidHotelDetails/{sPropId}/{sRoomId}", Name = "BidHotelDetails")]
        public ActionResult GetBidSearchedHotelDetails(string sPropId, string sRoomId)
        {
            if (!string.IsNullOrEmpty(sPropId))
            {
                string CurrencyCode = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";

                int propId = Convert.ToInt32(clsUtils.Decode(sPropId));
                int roomId = Convert.ToInt32(clsUtils.Decode(sRoomId));
                PropDetailsM objSearch = new PropDetailsM();
                objSearch.iPropId = propId;
                objSearch.dtCheckIn = DateTime.Now.Date.AddDays(10);
                objSearch.dtCheckOut = DateTime.Now.Date.AddDays(11);
                objSearch.bLogin = User.Identity.IsAuthenticated;
                objSearch.Currency = CurrencyCode;

                #region RoomOccupancySearch

                DataTable dtRoomOccupancySearch = new DataTable();
                dtRoomOccupancySearch.Columns.AddRange(new DataColumn[3] { 
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("iAdults", typeof(short)),
                    new DataColumn("children",typeof(short)) });


                DataTable dtChildrenAgeSearch = new DataTable();
                dtChildrenAgeSearch.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))});

                #endregion

                var propDetails = BL_PropDetails.GetPropertyDetails(propId, objSearch, dtRoomOccupancySearch, dtChildrenAgeSearch);
                var roomDetails = BL_PropDetails.GetRoomDetails(propId, roomId, CurrencyCode);
                propDetails.BidRoomDetails = roomDetails;

                return View("_HotelDetails", propDetails);

            }

            return View("Error");
        }
    }
}