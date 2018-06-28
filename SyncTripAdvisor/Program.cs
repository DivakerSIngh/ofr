using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using FutureSoft.Util;
using System.Data.SqlClient;
using Encription;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using OneFineRateAppUtil;
using System.Net.Mail;
using System;
namespace SyncTripAdvisor
{

    class Program
    {
        public static string RenderViewToString(string viewName)
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    string body = System.IO.File.ReadAllText(System.IO.Path.GetFullPath("Html/" + viewName));
                    return body;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        static void Main(string[] args)
        {
            string result = "", userid = "", passwd = "", url = "";
            url = ConfigurationManager.AppSettings["SMSurl"].ToString() + "?method=sendMessage&send_to=";
            userid = AESEncryption.DecryptData(ConfigurationManager.AppSettings["UIDSMS"], "HR$2pIjHR$2pIj12");
            passwd = AESEncryption.DecryptData(ConfigurationManager.AppSettings["PWDSMS"], "HR$2pIjHR$2pIj12");

            string sErrorEmail = "deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com";
            try
            {
                 sErrorEmail = SqlHelper.ExecuteScalar(ConfigurationManager.ConnectionStrings["OFR_DataContext"].ToString(), CommandType.Text, "SELECT TOP 1 sEmail FROM tblEmailSettingsM WHERE sModule = 'TripAdvisor'").ToString();
            }
            catch (Exception)
            {
            }

            try
            {

                ///////////RUN DAILY SCHEDULER FOR  to set Extranet Dashboard data and clear historical data from tblLookUp table
                DataSet ds1 = SqlHelper.ExecuteDataset(ConfigurationSettings.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspDailyScheduler");
                /////////SMS and an email will be sent to the guest one month and 15 days before the expiry of points. 
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationSettings.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetExpiringPointsToSendReminder");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sbEmail;
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        if (ds.Tables[0].Rows[i]["Name"].ToString() == "15")
                        {
                            sbEmail = new StringBuilder();
                            sbEmail.Append("<html> <body style='FONT-SIZE: 11pt; FONT-FAMILY: Calibri;'><font color=navy>Hi " + ds.Tables[0].Rows[i]["Name"].ToString() + ",");
                            sbEmail.Append("<p>Your OFR loyalty points (" + ds.Tables[0].Rows[i]["RemPoints"].ToString() + ") are going to expire in 15 days.");
                            sbEmail.Append("Please visit <a href='" + ConfigurationSettings.AppSettings["WebSiteUrl"].ToString() + "'>OneFineRate</a> to use your points as soon as possible. </p>");
                            sbEmail.Append("<p>Thank You,<br/>Team OneFineRate</p></font></body> </html>");

                            /////////////////SEND SMS---------------------

                            System.Net.WebRequest request = null;
                            System.Net.HttpWebResponse response = null;

                            try
                            {
                                url = url + ds.Tables[0].Rows[i]["PhoneNumber"].ToString() + "&v=1.1&msg=Your OFR loyalty points (" + ds.Tables[0].Rows[i]["RemPoints"].ToString() + ") are going to expire in 1 month.&userid=" + ConfigurationSettings.AppSettings["UIDSMS"].ToString() + "&password=" + ConfigurationSettings.AppSettings["PWDSMS"].ToString() + "&msg_type=text&auth_scheme=PLAIN";
                                request = System.Net.WebRequest.Create(url);

                                response = (System.Net.HttpWebResponse)request.GetResponse();
                                Stream stream = response.GetResponseStream();
                                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                                StreamReader reader = new
                                System.IO.StreamReader(stream, ec);
                                result = reader.ReadToEnd();

                                reader.Close();
                                stream.Close();
                            }
                            catch (Exception exp)
                            {
                                throw exp;
                            }
                            finally
                            {
                                if (response != null)
                                    response.Close();
                            }
                        }
                        else
                        {
                            sbEmail = new StringBuilder();
                            sbEmail.Append("<html> <body style='FONT-SIZE: 11pt; FONT-FAMILY: Calibri;'><font color=navy>Hi " + ds.Tables[0].Rows[i]["Name"].ToString() + ",");
                            sbEmail.Append("<p>Your OFR loyalty points (" + ds.Tables[0].Rows[i]["RemPoints"].ToString() + ") are going to expire in 1 month.");
                            sbEmail.Append("Please visit <a href='" + ConfigurationSettings.AppSettings["WebSiteUrl"].ToString() + "'>OneFineRate</a> to use your points as soon as possible. </p>");
                            sbEmail.Append("<p>Thank You,<br/>Team OneFineRate</p></font></body> </html>");

                            /////////////////SEND SMS---------------------

                            System.Net.WebRequest request = null;
                            System.Net.HttpWebResponse response = null;

                            try
                            {
                                url = url + ds.Tables[0].Rows[i]["PhoneNumber"].ToString() + "&v=1.1&msg=Your OFR loyalty points (" + ds.Tables[0].Rows[i]["RemPoints"].ToString() + ") are going to expire in 1 month.&userid=" + ConfigurationSettings.AppSettings["UIDSMS"].ToString() + "&password=" + ConfigurationSettings.AppSettings["PWDSMS"].ToString() + "&msg_type=text&auth_scheme=PLAIN";
                                request = System.Net.WebRequest.Create(url);

                                response = (System.Net.HttpWebResponse)request.GetResponse();
                                Stream stream = response.GetResponseStream();
                                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                                StreamReader reader = new
                                System.IO.StreamReader(stream, ec);
                                result = reader.ReadToEnd();

                                reader.Close();
                                stream.Close();
                            }
                            catch (Exception exp)
                            {
                                throw exp;
                            }
                            finally
                            {
                                if (response != null)
                                    response.Close();
                            }
                        }
                        MailComponent.SendEmail(ds.Tables[0].Rows[i]["Email"].ToString(), "", "", "OneFineRate- Loyalty Points Expiring", sbEmail.ToString(), null, null, true, null, null);

                        //Write code to send SMS
                        // ds.Tables[0].Rows[i]["PhoneNumber"].ToString() for SMS
                    }


                    // MailComponent.SendEmail(sErrorEmail, "", "", "TravelGuru Sync Error", "body", null, null, false);
                }

                // TripAdvisor Code 
                #region Getting user who has not given reviews after checkout after 3 days(feedback) and 8 days (Reminder)
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
                DataSet dt1 = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingsForTripAdvisorReview");
                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@sModule", "TripAdvisor");
                DataSet dataset = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[dbo].[GetEmailByModule]", Param);
                var Bcc = Convert.ToString(dataset.Tables[0].Rows[0]["Email"]);
                foreach (DataRow row in dt1.Tables[0].Rows)
                {
                    var feedBackHtml = RenderViewToString("FeedBackView.Html");
                    //feedBackHtml = feedBackHtml.Replace("{{FeedbackLink}}", string.Format("" + ConfigurationSettings.AppSettings["WebSiteUrl"].ToString() + "/Feedback/Index?bookingId={0}&customerEmail={1}", clsUtils.Encode(Convert.ToString(row["iBookingId"])), clsUtils.Encode(Convert.ToString(row["sCustomerEmail"]))));
                    var BlobUrl = ConfigurationManager.AppSettings["BlobUrl"].ToString();
                    var WebSiteUrl = ConfigurationManager.AppSettings["WebSiteUrl"].ToString();
                    feedBackHtml = feedBackHtml.Replace("{{Name}}", Convert.ToString(row["sCustomerName"]));
                    feedBackHtml = feedBackHtml.Replace("{{Hotel}}", Convert.ToString(row["sHotelName"]));
                    feedBackHtml = feedBackHtml.Replace("{{CITY}}", Convert.ToString(row["sCity"]));
                    feedBackHtml = feedBackHtml.Replace("{{HotelImage}}", (!string.IsNullOrEmpty(Convert.ToString(row["sImgUrl"])) ? Convert.ToString(row["sImgUrl"]) : WebSiteUrl + "/images/noimage.jpg"));
                    feedBackHtml = feedBackHtml.Replace("{{ClickReviewImage}}", BlobUrl + "currency/clickreview.gif");
                    feedBackHtml = feedBackHtml.Replace("{{OneFineRateLogoImage}}", WebSiteUrl + "/images/logoNew.png");
                    //feedBackHtml = feedBackHtml.Replace("{{FeedbackLink}}", string.Format("http://localhost:50481" + "/Feedback/Index?bookingId={0}&customerEmail={1}", clsUtils.Encode(Convert.ToString(row["iBookingId"])), clsUtils.Encode(Convert.ToString(row["sCustomerEmail"]))));
                    feedBackHtml = feedBackHtml.Replace("{{FeedbackLink}}", string.Format(WebSiteUrl + "/Feedback/Index?bookingId={0}&customerEmail={1}", clsUtils.Encode(Convert.ToString(row["iBookingId"])), clsUtils.Encode(Convert.ToString(row["sCustomerEmail"]))));
                    var subject = Convert.ToInt32(row["cType"]) == 1 ? "Reminder" : "Feedback";
                    MailComponent.SendEmail(row["sCustomerEmail"].ToString(), "", Bcc, subject, feedBackHtml.ToString(), null, null, true, null, null);
                    DataRow dtr = BookingTripAdvisorReview.NewRow();
                    dtr["[BookingId]"] = Convert.ToInt32(row["iBookingId"]);
                    dtr["[ReviewEmailDate]"] = DateTime.Now;
                    dtr["[ReviewLinkClicked]"] = false;
                    dtr["[dtReviewLinkClicked]"] = DBNull.Value;
                    dtr["[EmailSent]"] = 1;
                    BookingTripAdvisorReview.Rows.Add(dtr);

                }
                if (dt1.Tables[0] != null && dt1.Tables[0].Rows.Count > 0 && BookingTripAdvisorReview.Rows.Count > 0)
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@BookingTripAdvisorReview", BookingTripAdvisorReview);
                    MyParam[0].TypeName = "[dbo].[BookingTripAdvisorReview]";
                    SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[UpdateBookingTripAdvisorReview]", MyParam);
                }

                #endregion
                #region Getting TripAdvisorData For TripAdvisorLocId and saving

                #region TripAdvisorDataTable
                DataTable TripAdvisorData = new DataTable();
                DataColumn col = null;
                col = new DataColumn("iPropId", typeof(Int32));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iTripAdvisorLOCID", typeof(Int32));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sRatingImageURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iRanking", typeof(int));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sRankingString", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iReviewsCount", typeof(Int32));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sWebURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iLocationRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sLocationRatingURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iSleepQualityRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sSleepQualityRatingURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iRoomsRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sRoomsRatingURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iServiceRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sServiceRatingURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iValueRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sValueRatingURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("iCleanlinessRating", typeof(Decimal));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("sCleanlinessRatingURL", typeof(String));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("review_rating1_count", typeof(long));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("review_rating2_count", typeof(long));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("review_rating3_count", typeof(long));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("review_rating4_count", typeof(long));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("review_rating5_count", typeof(long));
                TripAdvisorData.Columns.Add(col);
                col = new DataColumn("write_review", typeof(String));
                TripAdvisorData.Columns.Add(col);
                #endregion


                #region TripAdvisorReviewsDataTable
                DataTable TripAdvisorReviews = new DataTable();
                DataColumn col1 = null;
                col1 = new DataColumn("iTripAdvisorLOCID", typeof(Int32));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("iReviewId", typeof(Int32));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sTitle", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sText", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sReviewer", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("iRating", typeof(Decimal));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sRatingImageURL", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sReviewURL", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("dtReviewDate", typeof(DateTime));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("dtTravelDate", typeof(DateTime));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sTripType", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                col1 = new DataColumn("sUserLocation", typeof(String));
                TripAdvisorReviews.Columns.Add(col1);
                #endregion

                #region TripAdvisorAwardsDataTable

                DataTable TripAdvisorAwards = new DataTable();
                DataColumn col2 = null;
                col2 = new DataColumn("iTripAdvisorLOCID", typeof(Int32));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("sAwardType", typeof(String));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("sDisplayName", typeof(String));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("iYear", typeof(Int32));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("sTinyImg", typeof(String));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("sSmallImg", typeof(String));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("sLargeImg", typeof(String));
                TripAdvisorAwards.Columns.Add(col2);
                col2 = new DataColumn("sCategories", typeof(String));
                TripAdvisorAwards.Columns.Add(col2);
                #endregion
                #region Getting and Adding data into datatables
                // Gets iTripAdvisor Id and Property Id from tblPropertyTripAdvisorM 
                DataSet dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetPropertyTripAdvisorId");
                if (dt.Tables[0].Rows.Count > 0)
                {
                    var tripAdvisorKey = ConfigurationManager.AppSettings["TripAdvisorKey"].ToString();

                    foreach (DataRow drow in dt.Tables[0].Rows)
                    {

                        string TripAdvisorId = drow["iTripAdvisorLOCID"].ToString();
                        string TA_url = "http://api.tripadvisor.com/api/partner/2.0/location/" + TripAdvisorId + "?key=" + tripAdvisorKey;
                        //string TA_url = "http://api.tripadvisor.com/api/partner/2.0/location/1759051?key=6F988E410E334D3CBCEBC86A6DAA4788";
                        using (WebClient wc = new WebClient())
                        {
                            string json = string.Empty;

                            try
                            {
                                json = wc.DownloadString(TA_url);
                                // This class would need to be updated when Awards are also fetched, as they most of the time appear as null.
                                TA_JSON data = JsonConvert.DeserializeObject<TA_JSON>(json);

                                DataRow drTripAdvisorData = TripAdvisorData.NewRow();
                                drTripAdvisorData["iPropId"] = Convert.ToInt32(drow["iPropId"]);
                                //drTripAdvisorData["iPropId"] = 43086;
                                drTripAdvisorData["iTripAdvisorLOCID"] = Convert.ToInt32(drow["iTripAdvisorLOCID"]);
                                //drTripAdvisorData["iTripAdvisorLOCID"] = 1759051;
                                drTripAdvisorData["sRatingImageURL"] = data.rating_image_url;
                                drTripAdvisorData["iRating"] = Convert.ToDecimal(data.rating);

                                drTripAdvisorData["sRankingString"] = data.ranking_data == null ? null : data.ranking_data.ranking_string;
                                drTripAdvisorData["iReviewsCount"] = Convert.ToInt32(data.num_reviews);
                                drTripAdvisorData["sWebURL"] = data.web_url;
                                drTripAdvisorData["write_review"] = data.write_review;
                                if (data.ranking_data == null)
                                    drTripAdvisorData["iRanking"] = DBNull.Value;
                                else
                                    drTripAdvisorData["iRanking"] = Convert.ToInt32(data.ranking_data.ranking);
                                if (data.review_rating_count != null)
                                {
                                    foreach (KeyValuePair<string, string> item in data.review_rating_count)
                                    {
                                        if (item.Key == "1")
                                        {
                                            drTripAdvisorData["review_rating1_count"] = Convert.ToInt64(item.Value);
                                        }
                                        else if (item.Key == "2")
                                        {
                                            drTripAdvisorData["review_rating2_count"] = Convert.ToInt64(item.Value);
                                        }
                                        else if (item.Key == "3")
                                        {
                                            drTripAdvisorData["review_rating3_count"] = Convert.ToInt64(item.Value);
                                        }
                                        else if (item.Key == "4")
                                        {
                                            drTripAdvisorData["review_rating4_count"] = Convert.ToInt64(item.Value);
                                        }
                                        else if (item.Key == "5")
                                        {

                                            drTripAdvisorData["review_rating5_count"] = Convert.ToInt64(item.Value);
                                        }
                                    }
                                }

                                foreach (var item in data.subratings)
                                {
                                    if (item.name == "rate_location")
                                    {
                                        drTripAdvisorData["iLocationRating"] = Convert.ToDecimal(item.value);
                                        drTripAdvisorData["sLocationRatingURL"] = item.rating_image_url;
                                    }
                                    else if (item.name == "rate_room")
                                    {
                                        drTripAdvisorData["iRoomsRating"] = Convert.ToDecimal(item.value);
                                        drTripAdvisorData["sRoomsRatingURL"] = item.rating_image_url;
                                    }
                                    else if (item.name == "rate_service")
                                    {
                                        drTripAdvisorData["iServiceRating"] = Convert.ToDecimal(item.value);
                                        drTripAdvisorData["sServiceRatingURL"] = item.rating_image_url;
                                    }
                                    else if (item.name == "rate_value")
                                    {
                                        drTripAdvisorData["iValueRating"] = Convert.ToDecimal(item.value);
                                        drTripAdvisorData["sValueRatingURL"] = item.rating_image_url;
                                    }
                                    else if (item.name == "rate_cleanliness")
                                    {
                                        drTripAdvisorData["iCleanlinessRating"] = Convert.ToDecimal(item.value);
                                        drTripAdvisorData["sCleanlinessRatingURL"] = item.rating_image_url;
                                    }
                                    else if (item.name == "rate_sleep")
                                    {
                                        drTripAdvisorData["iSleepQualityRating"] = Convert.ToDecimal(item.value);
                                        drTripAdvisorData["sSleepQualityRatingURL"] = item.rating_image_url;
                                    }
                                }

                                TripAdvisorData.Rows.Add(drTripAdvisorData);

                                foreach (var item in data.reviews)
                                {
                                    DateTime temp;
                                    if (!DateTime.TryParse(item.travel_date, out temp))
                                    {
                                        continue;
                                    }
                                    DataRow drTripAdvisorReviews = TripAdvisorReviews.NewRow();
                                    drTripAdvisorReviews["iTripAdvisorLOCID"] = Convert.ToInt32(drow["iTripAdvisorLOCID"]);
                                    drTripAdvisorReviews["iReviewId"] = Convert.ToInt32(item.id);
                                    drTripAdvisorReviews["sTitle"] = item.title;
                                    drTripAdvisorReviews["sText"] = item.text;
                                    drTripAdvisorReviews["sReviewer"] = item.user.username;
                                    drTripAdvisorReviews["iRating"] = Convert.ToDecimal(item.rating);
                                    drTripAdvisorReviews["sRatingImageURL"] = item.rating_image_url;
                                    drTripAdvisorReviews["sReviewURL"] = item.url;
                                    drTripAdvisorReviews["dtReviewDate"] = item.published_date;
                                    if (item.travel_date == null)
                                        drTripAdvisorReviews["dtTravelDate"] = DBNull.Value;
                                    else
                                        drTripAdvisorReviews["dtTravelDate"] = temp;

                                    drTripAdvisorReviews["sTripType"] = item.trip_type;
                                    drTripAdvisorReviews["sUserLocation"] = item.user.user_location.name;
                                    TripAdvisorReviews.Rows.Add(drTripAdvisorReviews);
                                }
                                foreach (var item in data.awards)
                                {
                                    DataRow drTripAdvisorAwards = TripAdvisorAwards.NewRow();
                                    drTripAdvisorAwards["iTripAdvisorLOCID"] = Convert.ToInt32(drow["iTripAdvisorLOCID"]);
                                    drTripAdvisorAwards["sAwardType"] = item.award_type;
                                    drTripAdvisorAwards["sDisplayName"] = item.display_name;
                                    drTripAdvisorAwards["iYear"] = item.year;
                                    drTripAdvisorAwards["sTinyImg"] = item.images.tiny;
                                    drTripAdvisorAwards["sSmallImg"] = item.images.small;
                                    drTripAdvisorAwards["sLargeImg"] = item.images.large;
                                    if (item.categories != null && item.categories.Count > 0)
                                    {
                                        drTripAdvisorAwards["sCategories"] = string.Join(";", item.categories);
                                    }
                                    TripAdvisorAwards.Rows.Add(drTripAdvisorAwards);
                                }

                            }
                            catch (WebException ex)
                            {
                                Debug.WriteLine(ex.ToString());
                            }

                        }
                    }
                }
                #endregion
                #region Updating Data for Tripadvisor

                if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0 && TripAdvisorData.Rows.Count > 0)
                {
                    SqlParameter[] MyParam = new SqlParameter[4];
                    MyParam[0] = new SqlParameter("@TripAdvisorData", TripAdvisorData);
                    MyParam[0].TypeName = "[dbo].[TripAdvisorData]";
                    MyParam[1] = new SqlParameter("@TripAdvisorReviews", TripAdvisorReviews);
                    MyParam[1].TypeName = "[dbo].[TripAdvisorReviews]";
                    MyParam[2] = new SqlParameter("@LastUpdatedRecord", dt.Tables[1].Rows[0]["LastUpdatedRecord"]);
                    MyParam[3] = new SqlParameter("@TripAdvisorAwards", TripAdvisorAwards);
                    MyParam[3].TypeName = "[dbo].[TripAdvisorAwards]";
                    SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateTripAdvisorData", MyParam);
                }
                #endregion
                #endregion

            }
            catch (Exception ex)
            {
                  MailComponent.SendEmail(sErrorEmail, "", "", "Loyalty points email sending error", ex.ToString(), null, null, false,null,null);
            }

        }


        #region TA Classes
        public class AddressObj
        {
            public string street1 { get; set; }
            public object street2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string postalcode { get; set; }
            public string address_string { get; set; }
        }

        public class TripType
        {
            public string name { get; set; }
            public string value { get; set; }
            public string localized_name { get; set; }
        }

        public class UserLocation
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class User
        {
            public string username { get; set; }
            public UserLocation user_location { get; set; }
        }

        public class Review
        {
            public string id { get; set; }
            public string lang { get; set; }
            public string location_id { get; set; }
            public string published_date { get; set; }
            public int rating { get; set; }
            public string helpful_votes { get; set; }
            public string rating_image_url { get; set; }
            public string url { get; set; }
            public string trip_type { get; set; }
            public string travel_date { get; set; }
            public string text { get; set; }
            public User user { get; set; }
            public string title { get; set; }
        }

        public class Ancestor
        {
            public object abbrv { get; set; }
            public string level { get; set; }
            public string name { get; set; }
            public string location_id { get; set; }
        }

        public Dictionary<string, string> ReviewRatingCount
        {
            get;
            set;
        }

        public class Subrating
        {
            public string rating_image_url { get; set; }
            public string name { get; set; }
            public string value { get; set; }
            public string localized_name { get; set; }
        }

        public class RankingData
        {
            public string ranking_string { get; set; }
            public string ranking_out_of { get; set; }
            public string geo_location_id { get; set; }
            public string ranking { get; set; }
            public string geo_location_name { get; set; }
        }

        public class Category
        {
            public string name { get; set; }
            public string localized_name { get; set; }
        }

        public class Subcategory
        {
            public string name { get; set; }
            public string localized_name { get; set; }
        }

        public class Images
        {
            public string tiny { get; set; }
            public string small { get; set; }
            public string large { get; set; }
        }

        public class awards
        {
            public string award_type { get; set; }
            public string year { get; set; }
            public Images images { get; set; }
            public List<string> categories { get; set; }
            public string display_name { get; set; }
        }

        public class TA_JSON
        {
            public AddressObj address_obj { get; set; }
            public string latitude { get; set; }
            public string rating { get; set; }
            public string description { get; set; }
            public string location_id { get; set; }
            public List<TripType> trip_types { get; set; }
            public List<Review> reviews { get; set; }
            public string write_review { get; set; }
            public List<Ancestor> ancestors { get; set; }
            public string longitude { get; set; }
            public object hours { get; set; }
            public object percent_recommended { get; set; }
            public Dictionary<string, string> review_rating_count { get; set; }
            public List<Subrating> subratings { get; set; }
            public RankingData ranking_data { get; set; }
            public string photo_count { get; set; }
            public string location_string { get; set; }
            public string web_url { get; set; }
            public string price_level { get; set; }
            public string rating_image_url { get; set; }
            public List<awards> awards { get; set; }
            public string name { get; set; }
            public string num_reviews { get; set; }
            public Category category { get; set; }
            public List<Subcategory> subcategory { get; set; }
            public string see_all_photos { get; set; }
        }

        #endregion
    }
}
