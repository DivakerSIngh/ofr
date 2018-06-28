using FutureSoft.Util;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OneFineRateApp.App_Start;
using System.Configuration;

namespace OneFineRateApp.Controllers.Bookings
{
    public class PendingNegotiationsController : BaseController
    {
        // GET: PendingNegotiations
        [Route("PendingNegotiations")]
        public ActionResult Index()
        {
            var propId = Convert.ToInt32(Session["PropId"].ToString());
            TempData["PropID"] = propId;

            if (Request.Browser.IsMobileDevice)
            {
                return RedirectToAction("ViewDashboard_Mobile", "Property", new { PropId = propId });
            }

            return View();
        }

        public string AcceptRejectNegotiation(string id, string act, string CO)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                string Status = act == "R" ? (CO == "-1" ? "HR" : "CO") : "BP";
                var bookingId = Convert.ToInt32(id);
                Status = BL_PendingChanges.UpdateNegotiation(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, bookingId, Status, (CO == null || CO == "NaN" || CO == "" || CO == "-1") ? 0 : Convert.ToInt32(CO));

                if (Status != "")
                {
                    eDataForNegotiationActionMail DFM = new eDataForNegotiationActionMail();
                    DFM = BL_PendingChanges.DataForNegotiationActionMail(Convert.ToInt32(id));
                    SendInformation(Status, bookingId);
                }

                switch (Status) // Notifications for hotel, who is not to be shown conserve commission reductions. Mail content/status to customer managed later.
                {
                    case "HR": // Hotel has rejected the negotiation. 
                    case "RR":
                    case "RA":
                        result = new { st = 1, msg = "Bargain rejected and notified to customer via email." };
                        break;
                    case "CO": // Hotel has countered the negotiation.
                    case "CR":
                    case "CA":
                        result = new { st = 1, msg = "Counter offer applied and notified to customer via email." };
                        break;
                    case "BP": // Hotel has accepted the negotiation.
                        result = new { st = 1, msg = "Bargain accepted and notified to customer via email." };
                        break;
                    default:
                        result = new { st = 0, msg = "Something went wrong" };
                        break;
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Something went wrong" };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }


        [NonAction]
        private Task SendInformation(string statusType, long bookingId)
        {
            try
            {
                //var bookingDetails = BL_Booking.GetBooking(bookingId);
                var bookingModel = BL_Booking.GetBookingModifyDetails_Notifications(bookingId);

                var model = new NegotiationEmailTempleteModel();
                string notificationSMS_User = string.Empty;
                bool Accepted = true;
                switch (statusType) // Notification mail/SMS to customer. Conserve commission checked and managed.
                {
                    case "HR": // Hotel has rejected the negotiation. Conserve commission not applied.
                        var websiteBaseUrl1 = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                        string negoStatusUrl = websiteBaseUrl1 + "Negotiation/NegotiationStatus?bookingId=" + clsUtils.Encode(bookingId.ToString());

                        model.Status = "Sorry! Your offer is rejected. Please Check the related offer details by clicking the link below:";
                        notificationSMS_User = "Sorry! We tried our best. Your rate offer was low for your chosen dates.  There are other savings to be made on related offers. Click the  link to continue your savings with One Fine Rate. ";
                        Accepted = false;
                        break;
                    case "CO": // Hotel has countered the negotiation.
                    case "RR": // Rate reduced for "Original rate" through conserve commission.
                    case "CR": // Rate reduced for Counter offer through conserve commission.
                        model.Status = "Progress! You have received a  counter offer ! Please check the details by clicking the link below:";
                        notificationSMS_User = model.Status;
                        Accepted = false;
                        break;
                    case "BP": // Hotel has accepted the negotiation.
                    case "RA": // Negotiation accepted by reducing conserve commission on "Original rate".
                    case "CA": // Negotiation accepted by reducing conserve commission on counter offer.
                        model.Status = "Congratulations! Your offer is accepted! Please Check the details by clicking the link below:";
                        notificationSMS_User = model.Status;
                        break;
                }

                //bookingModel.dTaxes = Convert.ToDecimal(bookingModel.sExtra2);
                     
                model.BookingModify = bookingModel;

                var websiteBaseUrl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
                if (!Accepted)
                    model.CallbackUrl = websiteBaseUrl + "Negotiation/NegotiationStatus?bookingId=" + clsUtils.Encode(bookingId.ToString());
                else
                    model.CallbackUrl = websiteBaseUrl + "NegoConfirmed/" + clsUtils.Encode(bookingId.ToString());
                              

                var html = this.RenderViewToString("_EmailTemplete", model);

                var shortUrl = clsUtils.Shorten(model.CallbackUrl);

                var message = notificationSMS_User + shortUrl;

                //Task.Run(() => clsUtils.sendSMS("9560439101", model.Status + Environment.NewLine + model.CallbackUrl));
                clsUtils.sendSMS(bookingModel.MobileOFR, message);

                return Task.Run(() => MailComponent.SendEmail(bookingModel.EmailOFR, "", "", "Offer from OneFineRate Confirmation No: " + bookingModel.BookingId, html, null, null, true, null, null));
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}