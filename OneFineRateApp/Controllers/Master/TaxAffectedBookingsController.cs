using FutureSoft.Util;
using OneFineRateApp.App_Helper;
using OneFineRateBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class TaxAffectedBookingsController : BaseController
    {
        [Route("TaxAffectedBookings")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCitySearchData(string citySearchTxt)
        {
            var cityList = BL_tblPropertyM.GetAllCityNameList(citySearchTxt);

            return Json(new { cityList }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult SendMailAndUpdateTaxAffectedBookings(string selectedBookingIds)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedBookingIds))
                {
                    var userId = ((BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                    List<string> deliveredBookingIdInEmail = new List<string>();

                    long[] bookingIdArr = selectedBookingIds.Split(',').Select(long.Parse).ToArray();

                    var affectedBoolingList = BL_Booking.GetTaxAffectedBookings(bookingIdArr);

                    bool isError = false;

                    foreach (var item in affectedBoolingList)
                    {                       
                        try
                        {
                            item.sCheckIn = item.dtCheckIn.ToString("dd MMM yyyy");
                            item.sCheckOut = item.dtCheckOut.ToString("dd MMM yyyy");

                            var html_Customer = this.RenderViewToString("_TaxChangeBookingAlert", item);
                            
                            //TO DO
                            var ccMail = string.Empty;
                            ccMail = "himanshuS@futuresoftindia.com";
                            MailComponent.SendEmail(item.sEmailOFR, ccMail, "", "OneFineRate! Confirmation No:" + item.iBookingId, html_Customer, null, null, true,null,null);
                            deliveredBookingIdInEmail.Add(item.iBookingId.ToString());
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                        }
                    }

                    var result = BL_Booking.UpdateTaxAffectedBookings(deliveredBookingIdInEmail.ToArray(), userId);

                    if(isError)
                    {
                        return Json(new { status = false, message = "An error occured occurred while sending email." }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { status = true, message = "Record updated and email sent to selected record successfully!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Please select at least one booking." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "An error occurred while updating the record!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}