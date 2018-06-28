using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace OneFineRateBLL
{
    public class clsReservation
    {
        #region Reservation - RateGain"
        public void OTA_HotelResNotifRQ(long iBookingId)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] MyParam = new SqlParameter[1];
            MyParam[0] = new SqlParameter("@iBookingId", iBookingId);

            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspHotelResNotif", MyParam);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["iChannelMgr"].ToString() == "1") //Rate Gain
                {
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:a=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\"");
                        sb.Append("xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"> <s:Header>");
                        sb.Append("<a:Action s:mustUnderstand=\"1\">http://cgbridge.rategain.com/2011A/ReservationService/HotelResNotif</a:Action>");
                        //  sb.Append("<a:MessageID>urn:uuid:8deae80c-5d3d-4269-9f0a-6d25a217ef1c</a:MessageID>");
                        sb.Append("<a:ReplyTo> <a:Address>http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous</a:Address> </a:ReplyTo>");
                        sb.Append("<a:To s:mustUnderstand=\"1\">" + ConfigurationManager.AppSettings["ReservationNotificationURL"].ToString() + "</a:To>");
                        sb.Append("<o:Security s:mustUnderstand=\"1\" xmlns:o=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">");
                        sb.Append("<o:UsernameToken u:Id=\"uuid-c698aabc-c710-42f9-881c-0dfe3fdcaa4f-2\">");
                        sb.Append("<o:Username>" + ConfigurationManager.AppSettings["RageGainUserName"].ToString() + "</o:Username>");
                        sb.Append("<o:Password o:Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">" + ConfigurationManager.AppSettings["RageGainPassword"].ToString() + "</o:Password>");
                        sb.Append("</o:UsernameToken> </o:Security> </s:Header>");
                        sb.Append("<s:Body xmlns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                        sb.Append("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                        sb.Append("<OTA_HotelResNotifRQ TimeStamp=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" Target=\"Production\" Version=\"2.01\" ResStatus=\"Commit\">");
                        sb.Append("<POS> <Source> <RequestorID Type=\"13\" ID=\"473\" /> <BookingChannel Type=\"7\" Primary=\"true\">");
                        sb.Append("<CompanyName Code=\"941\">OneFineRate</CompanyName> </BookingChannel> </Source> </POS>");
                        sb.Append("<HotelReservations> <HotelReservation CreateDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" LastModifyDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" ResStatus=\"Commit\" >");

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

                            foreach (DataRow dr3 in ds.Tables[2].Rows)
                            {
                                if (dr["iRPId"].ToString() == dr3["iRPId"].ToString() && dr["iRoomId"].ToString() == dr3["iRoomId"].ToString())
                                {
                                    sb.Append("<GuestCounts>");
                                    sb.Append("<GuestCount AgeQualifyingCode=\"10\" Count=\"" + dr3["iAdults"].ToString() + "\" />");
                                    sb.Append("<GuestCount AgeQualifyingCode=\"8\" Count=\"" + dr3["iChildren"].ToString() + "\" />");
                                    sb.Append("</GuestCounts>");

                                    sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" /> ");
                                    sb.Append("<Total AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" > ");
                                    sb.Append("<Taxes>");
                                    sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                    sb.Append("</Taxes>");
                                    sb.Append("</Total>");
                                    sb.Append("<BasicPropertyInfo HotelCode=\"" + ds.Tables[0].Rows[0]["iPropId"].ToString() + "\" HotelName=\"" + ds.Tables[0].Rows[0]["HoteName"].ToString() + "\">");
                                    sb.Append("</BasicPropertyInfo>");
                                }
                            }
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
                            sb.Append("<GivenName>\"" + dr["FirstName"].ToString() + "\"</GivenName>");
                            sb.Append("<Surname>\"" + dr["LastName"].ToString() + "\"</Surname>");
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
                        sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" Duration=\"" + ds.Tables[0].Rows[0]["Duration"].ToString() + "\" /> ");
                        sb.Append("<Total CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\">");
                        sb.Append("<Taxes>");
                        sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                        sb.Append("</Taxes> ");
                        sb.Append("</Total> ");
                        sb.Append("<HotelReservationIDs> ");
                        sb.Append("<HotelReservationID ResID_Type=\"14\" ResID_Value=" + ds.Tables[0].Rows[0]["ResID"].ToString() + "/> ");
                        sb.Append("</HotelReservationIDs> ");
                        sb.Append("</ResGlobalInfo> ");
                        sb.Append("</HotelReservation> </HotelReservations> </OTA_HotelResNotifRQ>");
                    }
                }
            }
        }
        #endregion

        #region Reservation - Maximojo"
        public void OTA_HotelResNotifRQ_MM(long iBookingId)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] MyParam = new SqlParameter[1];
            MyParam[0] = new SqlParameter("@iBookingId", iBookingId);

            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspHotelResNotif", MyParam);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["iChannelMgr"].ToString() == "1") //Rate Gain
                {
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:a=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\"");
                        sb.Append("xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"> <s:Header>");
                        sb.Append("<a:Action s:mustUnderstand=\"1\">http://cgbridge.rategain.com/2011A/ReservationService/HotelResNotif</a:Action>");
                        //  sb.Append("<a:MessageID>urn:uuid:8deae80c-5d3d-4269-9f0a-6d25a217ef1c</a:MessageID>");
                        sb.Append("<a:ReplyTo> <a:Address>http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous</a:Address> </a:ReplyTo>");
                        sb.Append("<a:To s:mustUnderstand=\"1\">" + ConfigurationManager.AppSettings["ReservationNotificationURL"].ToString() + "</a:To>");
                        sb.Append("<o:Security s:mustUnderstand=\"1\" xmlns:o=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">");
                        sb.Append("<o:UsernameToken u:Id=\"uuid-c698aabc-c710-42f9-881c-0dfe3fdcaa4f-2\">");
                        sb.Append("<o:Username>" + ConfigurationManager.AppSettings["RageGainUserName"].ToString() + "</o:Username>");
                        sb.Append("<o:Password o:Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">" + ConfigurationManager.AppSettings["RageGainPassword"].ToString() + "</o:Password>");
                        sb.Append("</o:UsernameToken> </o:Security> </s:Header>");
                        sb.Append("<s:Body xmlns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                        sb.Append("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                        sb.Append("<OTA_HotelResNotifRQ TimeStamp=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" Target=\"Production\" Version=\"2.01\" ResStatus=\"Commit\">");
                        sb.Append("<POS> <Source> <RequestorID Type=\"13\" ID=\"473\" /> <BookingChannel Type=\"7\" Primary=\"true\">");
                        sb.Append("<CompanyName Code=\"941\">OneFineRate</CompanyName> </BookingChannel> </Source> </POS>");
                        sb.Append("<HotelReservations> <HotelReservation CreateDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" LastModifyDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" ResStatus=\"Commit\" >");

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

                            foreach (DataRow dr3 in ds.Tables[2].Rows)
                            {
                                if (dr["iRPId"].ToString() == dr3["iRPId"].ToString() && dr["iRoomId"].ToString() == dr3["iRoomId"].ToString())
                                {
                                    sb.Append("<GuestCounts>");
                                    sb.Append("<GuestCount AgeQualifyingCode=\"10\" Count=\"" + dr3["iAdults"].ToString() + "\" />");
                                    sb.Append("<GuestCount AgeQualifyingCode=\"8\" Count=\"" + dr3["iChildren"].ToString() + "\" />");
                                    sb.Append("</GuestCounts>");

                                    sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" /> ");
                                    sb.Append("<Total AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" > ");
                                    sb.Append("<Taxes>");
                                    sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                    sb.Append("</Taxes>");
                                    sb.Append("</Total>");
                                    sb.Append("<BasicPropertyInfo HotelCode=\"" + ds.Tables[0].Rows[0]["iPropId"].ToString() + "\" HotelName=\"" + ds.Tables[0].Rows[0]["HoteName"].ToString() + "\">");
                                    sb.Append("</BasicPropertyInfo>");
                                }
                            }
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
                            sb.Append("<GivenName>\"" + dr["FirstName"].ToString() + "\"</GivenName>");
                            sb.Append("<Surname>\"" + dr["LastName"].ToString() + "\"</Surname>");
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
                        sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" Duration=\"" + ds.Tables[0].Rows[0]["Duration"].ToString() + "\" /> ");
                        sb.Append("<Total CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\">");
                        sb.Append("<Taxes>");
                        sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                        sb.Append("</Taxes> ");
                        sb.Append("</Total> ");
                        sb.Append("<HotelReservationIDs> ");
                        sb.Append("<HotelReservationID ResID_Type=\"14\" ResID_Value=" + ds.Tables[0].Rows[0]["ResID"].ToString() + "/> ");
                        sb.Append("</HotelReservationIDs> ");
                        sb.Append("</ResGlobalInfo> ");
                        sb.Append("</HotelReservation> </HotelReservations> </OTA_HotelResNotifRQ>");
                    }
                }
                else //Maximojo
                {
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        string EchoToken = "";
                        sb.Append("<OTA_HotelResNotifRQ xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" ResStatus=\"Commit\" TimeStamp=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\"  Version=\"1.0\">");
                        sb.Append("<POS> <Source> <RequestorID Type=\"13\" ID=\"473\" /> <BookingChannel Type=\"7\" Primary=\"true\">");
                        sb.Append("<CompanyName Code=\"941\">OneFineRate</CompanyName> </BookingChannel> </Source> </POS>");
                        sb.Append("<HotelReservations> <HotelReservation CreateDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" LastModifyDateTime=\"" + ds.Tables[0].Rows[0]["dtReservationDate"].ToString() + "\" ResStatus=\"Commit\" >");

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

                            foreach (DataRow dr3 in ds.Tables[2].Rows)
                            {
                                if (dr["iRPId"].ToString() == dr3["iRPId"].ToString() && dr["iRoomId"].ToString() == dr3["iRoomId"].ToString())
                                {
                                    sb.Append("<GuestCounts>");
                                    sb.Append("<GuestCount AgeQualifyingCode=\"10\" Count=\"" + dr3["iAdults"].ToString() + "\" />");
                                    sb.Append("<GuestCount AgeQualifyingCode=\"8\" Count=\"" + dr3["iChildren"].ToString() + "\" />");
                                    sb.Append("</GuestCounts>");

                                    sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtCheckIn"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" /> ");
                                    sb.Append("<Total AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" > ");
                                    sb.Append("<Taxes>");
                                    sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                                    sb.Append("</Taxes>");
                                    sb.Append("</Total>");
                                    sb.Append("<BasicPropertyInfo HotelCode=\"" + ds.Tables[0].Rows[0]["iPropId"].ToString() + "\" HotelName=\"" + ds.Tables[0].Rows[0]["HoteName"].ToString() + "\">");
                                    sb.Append("</BasicPropertyInfo>");
                                }
                            }
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
                            sb.Append("<GivenName>\"" + dr["FirstName"].ToString() + "\"</GivenName>");
                            sb.Append("<Surname>\"" + dr["LastName"].ToString() + "\"</Surname>");
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
                        sb.Append("<TimeSpan Start=\"" + ds.Tables[0].Rows[0]["dtTimeStamp"].ToString() + "\" End=\"" + ds.Tables[0].Rows[0]["dtCheckOut"].ToString() + "\" Duration=\"" + ds.Tables[0].Rows[0]["Duration"].ToString() + "\" /> ");
                        sb.Append("<Total CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" AmountBeforeTax=\"" + ds.Tables[0].Rows[0]["TotalAmountBeforeTax"].ToString() + "\" AmountAfterTax=\"" + ds.Tables[0].Rows[0]["TotalAmountAfterTax"].ToString() + "\">");
                        sb.Append("<Taxes>");
                        sb.Append("<Tax Amount=\"" + ds.Tables[0].Rows[0]["dTaxes"].ToString() + "\" CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" /> ");
                        sb.Append("</Taxes> ");
                        sb.Append("</Total> ");
                        sb.Append("<HotelReservationIDs> ");
                        sb.Append("<HotelReservationID ResID_Type=\"14\" ResID_Value=" + ds.Tables[0].Rows[0]["ResID"].ToString() + "/> ");
                        sb.Append("</HotelReservationIDs> ");
                        sb.Append("</ResGlobalInfo> ");
                        sb.Append("</HotelReservation> </HotelReservations> </OTA_HotelResNotifRQ>");
                    }
                }
            }
        }
        #endregion
    }
}