using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using FutureSoft.Util;
using System.Net.Http.Headers;
namespace SyncBooking
{
    class clsBooking
    {
        public string generateUniqueNumbers()
        {
            return (System.Guid.NewGuid().ToString());
        }
        public void InsertTracker(string iBookingId, string cSyncStatus, string sXMLString, string sError)
        {
            try
            {
                SqlParameter[] MyParam2 = new SqlParameter[4];
                MyParam2[0] = new SqlParameter("@iBookingId", iBookingId);
                MyParam2[1] = new SqlParameter("@cSyncStatus", cSyncStatus);
                MyParam2[2] = new SqlParameter("@sXMLString", sXMLString);
                MyParam2[3] = new SqlParameter("@sError", sError);

                SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspInsertSyncBookingToChannelMgrTracker", MyParam2);
            }
            catch (Exception)
            { }
        }
        public async Task OTA_HotelResNotifRQ()
        {
            string sErrorEmail = "deepaka@futuresoftindia.com";
            try
            {
                sErrorEmail = SqlHelper.ExecuteScalar(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.Text, "SELECT TOP 1 sEmail FROM tblEmailSettingsM WHERE sModule = 'ChannelMgrError'").ToString();
            }
            catch (Exception)
            { }

            StringBuilder sb = new StringBuilder();

            try
            {
                DataSet dsBooking = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingsToSyncWithChannelMgr");

                if (dsBooking != null && dsBooking.Tables.Count > 0 && dsBooking.Tables[0].Rows.Count > 0)
                {
                    for (int r = 0; r < dsBooking.Tables[0].Rows.Count; r++)
                    {

                        SqlParameter[] MyParam = new SqlParameter[1];
                        MyParam[0] = new SqlParameter("@iBookingId", dsBooking.Tables[0].Rows[r]["iBookingId"].ToString());

                        DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspHotelResNotif", MyParam);

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["iChannelMgr"].ToString() == "1") //Rate Gain
                            {
                                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                {
                                    sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:a=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\"");
                                    sb.Append(" xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"> <s:Header>");
                                    sb.Append("<a:Action s:mustUnderstand=\"1\">http://cgbridge.rategain.com/2011A/ReservationService/HotelResNotif</a:Action>");
                                    //  sb.Append("<a:MessageID>urn:uuid:8deae80c-5d3d-4269-9f0a-6d25a217ef1c</a:MessageID>");
                                  //  sb.Append("<a:ReplyTo> <a:Address>http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous</a:Address> </a:ReplyTo>");
                                    sb.Append("<a:To s:mustUnderstand=\"1\">" + ConfigurationManager.AppSettings["ReservationNotificationURL"].ToString() + "</a:To>");
                                    sb.Append("<o:Security s:mustUnderstand=\"1\" xmlns:o=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">");
                                    sb.Append("<o:UsernameToken u:Id=\"uuid-c698aabc-c710-42f9-881c-0dfe3fdcaa4f-2\">");
                                    sb.Append("<o:Username>" + ConfigurationManager.AppSettings["RateGainUserName"].ToString() + "</o:Username>");
                                    sb.Append("<o:Password o:Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">" + ConfigurationManager.AppSettings["RateGainPassword"].ToString() + "</o:Password>");
                                    sb.Append("</o:UsernameToken> </o:Security> </s:Header>");
                                    sb.Append("<s:Body>");  
                                   // sb.Append("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                                   // sb.Append(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                                    sb.Append("<OTA_HotelResNotifRQ xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" Target=\"Production\" Version=\"4\" ResStatus=\"" + dsBooking.Tables[0].Rows[r]["BookingType"].ToString() + "\">");
                                    sb.Append("<POS> <Source> <RequestorID Type=\"13\" ID=\"473\" /> <BookingChannel Type=\"7\" Primary=\"true\">");
                                    sb.Append("<CompanyName Code=\"" + ConfigurationManager.AppSettings["RateGainCode"].ToString() + "\">OneFineRate</CompanyName> </BookingChannel> </Source> </POS>");
                                    sb.Append("<HotelReservations> <HotelReservation CreateDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" LastModifyDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" ResStatus=\"" + dsBooking.Tables[0].Rows[r]["BookingType"].ToString() + "\" >");

                                    sb.Append("<RoomStays>");

                                    foreach (DataRow dr in ds.Tables[1].Rows)
                                    {
                                        sb.Append("<RoomStay RoomStayStatus=\"Book\">");
                                        sb.Append("<ResGuestRPHs>" + dr["ResGuestRPHs"].ToString() + "</ResGuestRPHs>"); // # persons 
                                        sb.Append("<RoomTypes>");
                                        sb.Append("<RoomType RoomTypeCode=\"" + dr["iRoomId"].ToString() + "\" NumberOfUnits=\"" + dr["NumberOfUnits"].ToString() + "\">");
                                        sb.Append("<RoomDescription Name=\"" + dr["sRoomName"].ToString() + "\"></RoomDescription>");
                                        sb.Append("</RoomType>");
                                        sb.Append("</RoomTypes>");

                                        sb.Append("<RatePlans>");
                                        sb.Append("<RatePlan RatePlanCode=\"" + dr["iRPId"].ToString() + "\">");
                                        //<MealsIncluded Breakfast="true" /> NOT REQUIRED
                                        sb.Append("</RatePlan>");

                                        sb.Append("</RatePlans>");
                                        sb.Append("<RoomRates>");

                                        sb.Append("<RoomRate RatePlanCode=\"" + dr["iRPId"].ToString() + "\" RoomTypeCode=\"" + dr["iRoomId"].ToString() + "\" NumberOfUnits=\"" + dr["NumberOfUnits"].ToString() + "\">");
                                        sb.Append("<Rates>");

                                        foreach (DataRow dr1 in ds.Tables[2].Rows)
                                        {
                                            if (dr["iRPId"].ToString() == dr1["iRPId"].ToString() && dr["iRoomId"].ToString() == dr1["iRoomId"].ToString())
                                            {
                                                sb.Append("<Rate EffectiveDate=\"" + dr1["dtEffectiveDate"].ToString() + "\" ExpireDate=\"" + dr1["dtExpireDate"].ToString() + "\">");
                                                sb.Append("<Base AmountBeforeTax=\"" + dr1["dAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + dr1["dAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\"/>");
                                                sb.Append("</Rate>");
                                            }
                                        }

                                        sb.Append("</Rates>");
                                        sb.Append("</RoomRate>");

                                        sb.Append("</RoomRates>");

                                        //foreach (DataRow dr3 in ds.Tables[1].Rows)
                                        //{
                                            //if (dr["iRPId"].ToString() == dr3["iRPId"].ToString() && dr["iRoomId"].ToString() == dr3["iRoomId"].ToString())
                                            //{
                                                sb.Append("<GuestCounts>");
                                                sb.Append("<GuestCount AgeQualifyingCode=\"10\" Count=\"" + dr["iAdults"].ToString() + "\" />");
                                                sb.Append("<GuestCount AgeQualifyingCode=\"8\" Count=\"" + dr["iChildren"].ToString() + "\" />");
                                                sb.Append("</GuestCounts>");

                                                sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" /> ");
                                                sb.Append("<Total AmountBeforeTax=\"" + dr["dAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + dr["dAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" > ");
                                                sb.Append("<Taxes>");
                                                sb.Append("<Tax Amount=\"" + dr["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                                sb.Append("</Taxes>");
                                                sb.Append("</Total>");
                                                sb.Append("<BasicPropertyInfo HotelCode=\"" + ds.Tables[0].Rows[0]["iPropId"].ToString() + "\" HotelName=\"" + ds.Tables[0].Rows[0]["HoteName"].ToString() + "\">");
                                                sb.Append("</BasicPropertyInfo>");
                                        //    }
                                        //}
                                        sb.Append("</RoomStay>");
                                    }
                                    sb.Append("</RoomStays>");
                                    sb.Append("<ResGuests>");

                                    
                                    foreach (DataRow dr in ds.Tables[3].Rows)
                                    {
                                        if (dr["bIsPrimary"].ToString() == "true")
                                        {
                                            sb.Append("<ResGuest ResGuestRPH=\"" + dr["ResGuestRPHs"].ToString() + "\" PrimaryIndicator=\"" + dr["bIsPrimary"].ToString() + "\">");
                                        }
                                        else
                                        {
                                            sb.Append("<ResGuest ResGuestRPH=\"" + dr["ResGuestRPHs"].ToString() + "\">");
                                        }

                                        sb.Append("<Profiles>");
                                        sb.Append("<ProfileInfo>");
                                        sb.Append("<Profile ProfileType=\"1\" StatusCode=\"\">");
                                        sb.Append("<Customer>");
                                        sb.Append("<PersonName>");
                                        sb.Append("<GivenName>" + dr["FirstName"].ToString() + "</GivenName>");
                                        sb.Append("<Surname>" + dr["LastName"].ToString() + "</Surname>");
                                        sb.Append("</PersonName>");
                                        sb.Append("<Telephone PhoneTechType=\"1\" PhoneNumber=\"" + dr["sGuestMobile"].ToString() + "\" DefaultInd=\"true\" FormattedInd=\"false\" />");
                                        sb.Append("<Email EmailType=\"1\" DefaultInd=\"true\">" + dr["sGuestEmail"].ToString() + "</Email>");
                                        sb.Append("</Customer>");
                                        sb.Append("<CompanyInfo />");
                                        sb.Append("</Profile>");
                                        sb.Append("</ProfileInfo>");
                                        sb.Append("</Profiles>");
                                        sb.Append("</ResGuest>");
                                    }
                                    foreach (DataRow dr in ds.Tables[4].Rows)
                                    {
                                        sb.Append("<ResGuest PrimaryIndicator=\"" + dr["bIsPrimary"].ToString() + "\">");

                                        sb.Append("<Profiles>");
                                        sb.Append("<ProfileInfo>");
                                        sb.Append("<Profile ProfileType=\"18\" StatusCode=\"\">");
                                        sb.Append("<Customer>");
                                        sb.Append("<PersonName>");
                                        sb.Append("<GivenName>" + dr["FirstName"].ToString() + "</GivenName>");
                                        sb.Append("<Surname>" + dr["LastName"].ToString() + "</Surname>");
                                        sb.Append("</PersonName>");
                                        sb.Append("<Telephone PhoneTechType=\"1\" PhoneNumber=\"" + dr["sGuestMobile"].ToString() + "\" DefaultInd=\"true\" FormattedInd=\"false\" />");
                                        sb.Append("<Email EmailType=\"1\" DefaultInd=\"true\">" + dr["sGuestEmail"].ToString() + "</Email>");
                                        sb.Append("</Customer>");
                                        sb.Append("<CompanyInfo />");
                                        sb.Append("</Profile>");
                                        sb.Append("</ProfileInfo>");
                                        sb.Append("</Profiles>");
                                        sb.Append("</ResGuest>");
                                    }
                                    sb.Append("</ResGuests>");
                                    sb.Append("<ResGlobalInfo>");
                                    sb.Append("<SpecialRequests>");
                                    sb.Append("<SpecialRequest>");
                                    sb.Append("<Text>"+ ds.Tables[0].Rows[0]["sSpecialRequests"].ToString() + "</Text>");
                                    sb.Append("</SpecialRequest>");
                                    sb.Append("</SpecialRequests>");
                                    sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" Duration=\"" + ds.Tables[0].Rows[0]["Duration"].ToString() + "\" /> ");
                                    sb.Append("<Total CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\">");
                                    sb.Append("<Taxes>");
                                    sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                    sb.Append("</Taxes> ");
                                    sb.Append("</Total> ");
                                    sb.Append("<HotelReservationIDs> ");
                                    sb.Append("<HotelReservationID ResID_Type=\"14\" ResID_Value=\"" + ds.Tables[0].Rows[0]["ResID"].ToString() + "\"/> ");
                                    sb.Append("</HotelReservationIDs> ");
                                    sb.Append("</ResGlobalInfo> ");
                                    sb.Append("</HotelReservation> </HotelReservations> </OTA_HotelResNotifRQ>");
                                    sb.Append("</s:Body>");
                                    sb.Append("</s:Envelope>");

                                    //------------------------SEND REQUEST to Channel Manager

                                    string responseXml = "";
                                    try
                                    {

                                        // string url = "https://rzbetal4.rategain.com/RZL4PREPRODYieldGainWS/Reservation.svc";                                        
                                        string url = ConfigurationManager.AppSettings["ReservationNotificationURL"].ToString(); // "https://rzintghospidev.rategain.com/RezYieldGainWS/Reservation.svc";
                                        HttpClient httpClient = new HttpClient();
                                        httpClient.Timeout = TimeSpan.FromMilliseconds(40000);
                                        HttpContent httpContent = new StringContent(sb.ToString());
                                        HttpResponseMessage response;

                                        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                                        
                                        req.Headers.Add("SOAPAction", "http://cgbridge.rategain.com/2011A/ReservationService/HotelResNotif");
                                        req.Method = HttpMethod.Post;
                                        req.Content = httpContent;
                                        req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");
                                        response = await httpClient.SendAsync(req);

                                        var responseXml1 = await response.Content.ReadAsStringAsync();
                                        responseXml = responseXml1;
                                        
                                        /*
                                        var httpClient = new HttpClient();

                                        var content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml");

                                        var response = await httpClient.PostAsync("http://rzbetal4.rategain.com/RZL4PREPRODYieldGainWS/Reservation.svc", content);
                                        response.EnsureSuccessStatusCode();

                                        var responseXml1 = await response.Content.ReadAsStringAsync();
                                        responseXml = responseXml1;
                                         */
                                        InsertTracker(dsBooking.Tables[0].Rows[r]["iBookingId"].ToString(), "S", sb.ToString(), "");
                                        sb.Clear();
                                    }
                                    catch (Exception ex)
                                    {
                                        InsertTracker(dsBooking.Tables[0].Rows[r]["iBookingId"].ToString(), "F", sb.ToString(), ex.Message);
                                        sb.Clear();
                                        MailComponent.SendEmail(sErrorEmail, "", "", "RateGain - Error while sync reservation to channel manager.", ex.Message, null, null, false, null, null);
                                        break;
                                    }

                                    var xmlDoc = new XmlDocument();
                                    try
                                    {
                                        responseXml = responseXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                                        MemoryStream stream = new MemoryStream();
                                        StreamWriter writer = new StreamWriter(stream);
                                        writer.Write(responseXml);
                                        writer.Flush();
                                        stream.Position = 0;
                                        xmlDoc.Load(stream);

                                        if (xmlDoc.GetElementsByTagName("Success") != null)//Success
                                        {
                                            //Update bSyncToChannelMgr = 1                                                 
                                            SqlParameter[] MyParam1 = new SqlParameter[1];
                                            MyParam1[0] = new SqlParameter("@iBookingId", dsBooking.Tables[0].Rows[r]["iBookingId"].ToString());

                                            SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateBookingSyncStatusChannelMgr", MyParam1);                                            
                                        }
                                        else//Error
                                        {
                                            XmlNodeList xmlnode;
                                            xmlnode = xmlDoc.GetElementsByTagName("Errors");

                                            for (int i = 0; i <= xmlnode[0].ChildNodes.Count - 1; i++)
                                            {
                                                if (xmlnode[0].ChildNodes[i].Attributes["ShortText"] != null)
                                                {
                                                    MailComponent.SendEmail(sErrorEmail, "", "", "RateGain -Error while reading response received from channel manager for reservation sync.", xmlnode[0].ChildNodes[i].Attributes["ShortText"].Value, null, null, false, null, null);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MailComponent.SendEmail(sErrorEmail, "", "", "RateGain -Error while reading response received from channel manager for reservation sync.", ex.Message, null, null, false, null, null);
                                    }
                                }
                            }
                            else //-------------------------------Maximojo-------------------------------------------------------------
                            {
                                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                {
                                    string EchoToken = generateUniqueNumbers();

                                    sb.Append("<OTA_HotelResNotifRQ xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" ResStatus=\"" + dsBooking.Tables[0].Rows[r]["BookingType"].ToString() + "\" Timestamp=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\"  Version=\"1.0\">");
                                    sb.Append("<POS> <Source> <RequestorID ID=\"" + ConfigurationManager.AppSettings["RequestorID_ID"].ToString() + "\" ID_Context=\"" + ConfigurationManager.AppSettings["RequestorID_ID_Context"].ToString() + "\" MessagePassword=\"" + ConfigurationManager.AppSettings["RequestorID_MessagePassword"].ToString() + "\" Type=\"" + ConfigurationManager.AppSettings["RequestorID_Type"].ToString() + "\" />");
                                    sb.Append("<BookingChannel Type=\"" + ConfigurationManager.AppSettings["BookingChannel_Type_MM"].ToString() + "\" Primary=\"true\">");
                                    sb.Append("<CompanyName Code=\"" + ConfigurationManager.AppSettings["Code_MM"].ToString() + "\" CompanyShortName=\"" + ConfigurationManager.AppSettings["CompanyName_MM"].ToString() + "\"/>");
                                    sb.Append("</BookingChannel> </Source> </POS>");
                                    sb.Append("<HotelReservations> <HotelReservation CreateDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" LastModifyDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" CreatorID=\"" + ConfigurationManager.AppSettings["RequestorID_ID_Context"].ToString() + "\" >");
                                    sb.Append("<UniqueID ID=\"" + dsBooking.Tables[0].Rows[r]["iBookingId"].ToString() + "\" Type=\"14\" />");
                                    sb.Append("<RoomStays>");

                                    foreach (DataRow dr in ds.Tables[1].Rows)
                                    {
                                        sb.Append("<RoomStay SourceOfBusiness=\"" + ConfigurationManager.AppSettings["RequestorID_ID_Context"].ToString() + "\">");
                                        sb.Append("<RoomRates>");

                                        sb.Append("<RoomRate RatePlanCode=\"" + dr["iRPId"].ToString() + "\" RoomTypeCode=\"" + dr["iRoomId"].ToString() + "\" NumberOfUnits=\"" + dr["NumberOfUnits"].ToString() + "\">");
                                        sb.Append("<Rates>");

                                        foreach (DataRow dr1 in ds.Tables[2].Rows)
                                        {
                                            if (dr["iRPId"].ToString() == dr1["iRPId"].ToString() && dr["iRoomId"].ToString() == dr1["iRoomId"].ToString())
                                            {
                                                sb.Append("<Rate EffectiveDate=\"" + dr1["dtEffectiveDate"].ToString() + "\" ExpireDate=\"" + dr1["dtExpireDate"].ToString() + "\" RateTimeUnit=\"Day\" UnitMultiplier=\"1\">");
                                                sb.Append("<Base AmountBeforeTax=\"" + dr1["dAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + dr1["dAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\"/>");
                                                //Remove AmountAfterTax if not required for Maximojo
                                                sb.Append("</Rate>");
                                            }
                                        }

                                        sb.Append("</Rates>");
                                        sb.Append("</RoomRate>");

                                        sb.Append("</RoomRates>");

                                        sb.Append("<GuestCounts IsPerRoom=\"true\">");
                                        sb.Append("<GuestCount AgeQualifyingCode=\"10\" Count=\"" + dr["iAdults"].ToString() + "\" />");
                                        sb.Append("<GuestCount AgeQualifyingCode=\"8\" Count=\"" + dr["iChildren"].ToString() + "\" />");
                                        //Remove ROW GuestCount AgeQualifyingCode=\"8\" if not required for Maximojo
                                        sb.Append("</GuestCounts>");

                                        sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" /> ");
                                        sb.Append("<Total AmountBeforeTax=\"" + dr["dAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + dr["dAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" > ");
                                        //Remove AmountAfterTax if not required for Maximojo
                                        sb.Append("<Taxes>");
                                        sb.Append("<Tax Amount=\"" + dr["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                        sb.Append("</Taxes>");
                                        sb.Append("</Total>");
                                        sb.Append("<BasicPropertyInfo HotelCode=\"" + ds.Tables[0].Rows[0]["iPropId"].ToString() + "\" HotelName=\"" + ds.Tables[0].Rows[0]["HoteName"].ToString() + "\">");
                                        sb.Append("</BasicPropertyInfo>");
                                        sb.Append("<ResGuestRPHs> <ResGuestRPH RPH=\"" + dr["ResGuestRPHs"].ToString() + "\" /></ResGuestRPHs>"); // # persons 
                                        
                                        //foreach (DataRow dr3 in ds.Tables[2].Rows)
                                        //{
                                        //    if (dr["iRPId"].ToString() == dr3["iRPId"].ToString() && dr["iRoomId"].ToString() == dr3["iRoomId"].ToString())
                                        //    {
                                        //        sb.Append("<GuestCounts IsPerRoom=\"true\">");
                                        //        sb.Append("<GuestCount AgeQualifyingCode=\"10\" Count=\"" + dr3["iAdults"].ToString() + "\" />");
                                        //        sb.Append("<GuestCount AgeQualifyingCode=\"8\" Count=\"" + dr3["iChildren"].ToString() + "\" />");
                                        //        //Remove ROW GuestCount AgeQualifyingCode=\"8\" if not required for Maximojo
                                        //        sb.Append("</GuestCounts>");

                                        //        sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" /> ");
                                        //        sb.Append("<Total AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" > ");
                                        //        //Remove AmountAfterTax if not required for Maximojo
                                        //        sb.Append("<Taxes Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                        //        /*
                                        //        sb.Append("<Taxes>");
                                        //        sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                        //        sb.Append("</Taxes>");
                                        //         * */
                                        //        sb.Append("</Total>");
                                        //        sb.Append("<BasicPropertyInfo HotelCode=\"" + ds.Tables[0].Rows[0]["iPropId"].ToString() + "\" HotelName=\"" + ds.Tables[0].Rows[0]["HoteName"].ToString() + "\">");
                                        //        //Remove HotelName if not required for Maximojo
                                        //        sb.Append("</BasicPropertyInfo>");
                                        //        sb.Append("<ResGuestRPHs> <ResGuestRPH RPH=\"" + dr["ResGuestRPHs"].ToString() + "\" /></ResGuestRPHs>"); // # persons 
                                        //    }
                                        //}
                                
                                        sb.Append("</RoomStay>");
                                    }
                                    sb.Append("</RoomStays>");
                                    sb.Append("<ResGuests>");

                                    foreach (DataRow dr in ds.Tables[3].Rows)
                                    {
                                        if (dr["bIsPrimary"].ToString() == "true")
                                        {
                                            sb.Append("<ResGuest ResGuestRPH=\"" + dr["ResGuestRPHs"].ToString() + "\" PrimaryIndicator=\"" + dr["bIsPrimary"].ToString() + "\">");
                                        }
                                        else
                                        {
                                            sb.Append("<ResGuest ResGuestRPH=\"" + dr["ResGuestRPHs"].ToString() + "\">");
                                        }

                                        sb.Append("<Profiles>");
                                        sb.Append("<ProfileInfo>");
                                        sb.Append("<Profile ProfileType=\"1\">");
                                        sb.Append("<Customer>");
                                        sb.Append("<PersonName>");
                                        sb.Append("<GivenName>" + dr["FirstName"].ToString() + "</GivenName>");
                                        sb.Append("<Surname>" + dr["LastName"].ToString() + "</Surname>");
                                        sb.Append("</PersonName>");
                                        sb.Append("<Telephone PhoneTechType=\"1\" PhoneNumber=\"" + dr["sGuestMobile"].ToString() + "\" DefaultInd=\"true\" FormattedInd=\"false\" />");
                                        sb.Append("<Email EmailType=\"1\" DefaultInd=\"true\">" + dr["sGuestEmail"].ToString() + "</Email>");
                                        sb.Append("</Customer>");
                                        sb.Append("<CompanyInfo />");
                                        sb.Append("</Profile>");
                                        sb.Append("</ProfileInfo>");
                                        sb.Append("</Profiles>");
                                        sb.Append("</ResGuest>");
                                    }
                                    sb.Append("</ResGuests>");
                                    sb.Append("<ResGlobalInfo>");
                                    // NOT REQUIRED FOR MAXIMOJO sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" Duration=\"" + ds.Tables[0].Rows[0]["Duration"].ToString() + "\" /> ");
                                    sb.Append("<Total CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\">");
                                    //Remove AmountAfterTax if not required for Maximojo
                                    /* NOT REQUIRED FOR MAXIMOJO
                                    sb.Append("<Taxes>");
                                    sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                    sb.Append("</Taxes> ");
                                     * */
                                    sb.Append("</Total> ");
                                    sb.Append("<HotelReservationIDs> ");
                                    sb.Append("<HotelReservationID ResID_Type=\"14\" ResID_Value=\"" + ds.Tables[0].Rows[0]["ResID"].ToString() + "\"/> ");
                                    sb.Append("</HotelReservationIDs> ");
                                    sb.Append("</ResGlobalInfo> ");
                                    sb.Append("</HotelReservation> </HotelReservations> </OTA_HotelResNotifRQ>");
                                    /*
                                    byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
                                    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                                    MemoryStream stream = new MemoryStream(byteArray);
                                    Maximojo.ServerClient obj = new Maximojo.ServerClient();
                                    var response = await obj.PostAsync(stream);
                                    */
                                    /////////////////////////////////Method-1/////////////////////////////////////////////
                                    /*
                                    var httpClient = new HttpClient();
                                    var response = await httpClient.PostAsync("http://staging.platform.maximojo.com/HTNG/Server.svc/std/post", new StringContent(sb.ToString()));

                                    response.EnsureSuccessStatusCode();

                                    string content = await response.Content.ReadAsStringAsync();
                                    */
                                    //////////////////////////////////////////////////////////////////////////////
                                    ////////////////////////////////////Method-2- Working//////////////////////////////////////////
                                    string responseXml = "";
                                    try
                                    {
                                        var httpClient = new HttpClient();

                                        var content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml");

                                        var response = await httpClient.PostAsync("http://staging.platform.maximojo.com/HTNG/Server.svc/std/post", content);
                                        response.EnsureSuccessStatusCode();

                                        var responseXml1 = await response.Content.ReadAsStringAsync();
                                        responseXml = responseXml1;
                                        InsertTracker(dsBooking.Tables[0].Rows[r]["iBookingId"].ToString(), "S", sb.ToString(), "");
                                    }
                                    catch (Exception ex)
                                    {
                                        InsertTracker(dsBooking.Tables[0].Rows[r]["iBookingId"].ToString(), "F", sb.ToString(), ex.Message);
                                        sb.Clear();
                                        MailComponent.SendEmail(sErrorEmail, "", "", "Maximojo - Error while sync reservation to channel manager.", ex.Message, null, null, false, null, null);
                                        break;
                                    }


                                    //////////////////////////////////////////////////////////////////////////////
                                    ////////////////////////////////////Method-3//////////////////////////////////////////
                                    /*
                                    Uri uri = new Uri("http://staging.platform.maximojo.com/HTNG/Server.svc/std/post");

                                    HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(uri);
                                    byte[] RequestBytes = Encoding.UTF8.GetBytes(sb.ToString());
                                    Request.ContentLength = RequestBytes.Length;
                                    Request.Method = "POST";
                                    Request.ContentType = "application/xml;charset=UTF-8";

                                    Stream RequestStream = Request.GetRequestStream();
                                    RequestStream.Write(RequestBytes, 0, RequestBytes.Length);
                                    RequestStream.Close();

                                    HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                                    StreamReader reader = new StreamReader(response.GetResponseStream());
                                    string ResponseMessage = reader.ReadToEnd();
                                    response.Close();
                                    */
                                    //check success status 
                                    var xmlDoc = new XmlDocument();
                                    try
                                    {
                                        responseXml = responseXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                                        MemoryStream stream = new MemoryStream();
                                        StreamWriter writer = new StreamWriter(stream);
                                        writer.Write(responseXml);
                                        writer.Flush();
                                        stream.Position = 0;
                                        xmlDoc.Load(stream);
                                        string EchoToken_MM = "";
                                        if (xmlDoc.GetElementsByTagName("OTA_HotelResNotifRS")[0].Attributes["EchoToken"] != null)
                                        {
                                            EchoToken_MM = xmlDoc.GetElementsByTagName("OTA_HotelResNotifRS")[0].Attributes["EchoToken"].Value;
                                        }

                                        if (EchoToken == EchoToken_MM)//Authentic response
                                        {
                                            if (xmlDoc.GetElementsByTagName("Success") != null)//Success
                                            {
                                                if (xmlDoc.GetElementsByTagName("UniqueID")[0].Attributes["ID"] != null)//Success
                                                {
                                                    if (xmlDoc.GetElementsByTagName("UniqueID")[0].Attributes["ID"].Value == dsBooking.Tables[0].Rows[r]["iBookingId"].ToString())//Success
                                                    {
                                                        //Update bSyncToChannelMgr = 1                                                 
                                                        SqlParameter[] MyParam1 = new SqlParameter[1];
                                                        MyParam1[0] = new SqlParameter("@iBookingId", dsBooking.Tables[0].Rows[r]["iBookingId"].ToString());

                                                        SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateBookingSyncStatusChannelMgr", MyParam1);
                                                        sb.Clear();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sb.Clear();
                                        MailComponent.SendEmail(sErrorEmail, "", "", "Maximojo -Error while reading response received from channel manager for reservation sync.", ex.Message, null, null, false, null, null);
                                    }

                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                sb.Clear();
                MailComponent.SendEmail(sErrorEmail, "", "", "Maximojo -Error while sync reservation to channel manager.", ex.Message, null, null, false, null, null);
            }

        }
    }
}
