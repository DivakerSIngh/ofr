using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class FeedbackController : BaseController
    {
        // GET: Feedback
        public ActionResult Index(string bookingId,string customerEmail)
        {
            try
            {
                DataTable BookingTripAdvisorReview = new DataTable();
                DataColumn col3 = null;
                col3 = new DataColumn("[BookingId]", typeof(Int32));
                BookingTripAdvisorReview.Columns.Add(col3);
                col3 = new DataColumn("[ReviewEmailDate]", typeof(DateTime));
                BookingTripAdvisorReview.Columns.Add(col3);
                col3 = new DataColumn("[ReviewLinkClicked]", typeof(bool));
                BookingTripAdvisorReview.Columns.Add(col3);
                col3 = new DataColumn("[dtReviewLinkClicked]", typeof(DateTime));
                BookingTripAdvisorReview.Columns.Add(col3);
                col3 = new DataColumn("[EmailSent]", typeof(String));
                BookingTripAdvisorReview.Columns.Add(col3);
                DataRow dtr = BookingTripAdvisorReview.NewRow();
                dtr["[BookingId]"] = Convert.ToInt32(clsUtils.Decode(bookingId));
                dtr["[ReviewLinkClicked]"] = true;
                dtr["[dtReviewLinkClicked]"] = DateTime.Now;
                BookingTripAdvisorReview.Rows.Add(dtr);
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@BookingTripAdvisorReview", BookingTripAdvisorReview);
                MyParam[0].TypeName = "[dbo].[BookingTripAdvisorReview]";
                MyParam[1] = new SqlParameter("@customerEmail", clsUtils.Decode(customerEmail));
                SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[UpdateBookingTripAdvisorReviewById]", MyParam);
                var userData = OneFineRateBLL.BL_PropDetails.GetUserDetails(Convert.ToInt32(clsUtils.Decode(bookingId)));
                var userInfo =  "locationId=" + userData.LoctionId + "&email=" + userData.sUserEmail + "&firstName=" + userData.sUserFirstName + "&lastName=" + userData.sUserLastName + "&city=" + userData.sCity;
                string hash = FutureSoft.ClassFiles.SimpleHash.GetMd5Hash(ConfigurationManager.AppSettings["tripadvisorencryptionkey"].ToString() +userInfo);
                string Hex = FutureSoft.ClassFiles.SimpleHash.ConvertStringToHex(userInfo);
                ViewBag.Url = ConfigurationManager.AppSettings["tripadvisorreviewform"].ToString() + "?locationId=" + userData.LoctionId + "&partnerId=" + ConfigurationManager.AppSettings["tripadvisorpartnerId"].ToString() + "&display=true&lang=en_EN&allowMobile&" + hash + Hex;
            }
            catch (Exception ex)
            {
            Debug.WriteLine(ex.ToString());
            }
            return View("ReviewForm");
        }
    }
}