using OFRChannelMgr.DB_Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace OFRChannelMgr
{
    public class MaximojoController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>

        public HttpResponseMessage Post(HttpRequestMessage xmlreq)//(HttpRequestMessage xmlreq)
        {
            //HttpRequestMessage xmlreq = null;
            // Reading data as XML string to log to files - In case message structure is changed
            string EchoToken = "";
            var xmlDoc = new XmlDocument();
            try
            {
                string smlResult = xmlreq.Content.ReadAsStringAsync().Result;
                smlResult = smlResult.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                //smlResult ="<OTA_HotelRatePlanRQ xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" SummaryOnly=\"true\" Version=\"1\" EchoToken=\"a9b04a84-edc2-4639-8a55-829ec506b884\" xmlns=\"http://www.opentravel.org/OTA/2003/05\"><RatePlans><RatePlan><HotelRef HotelCode=\"44718\" /></RatePlan></RatePlans></OTA_HotelRatePlanRQ>";
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(smlResult);
                writer.Flush();
                stream.Position = 0;
                //smlResult = smlResult.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                xmlDoc.Load(stream);
                //xmlDoc.Load(xmlreq.Content.ReadAsStreamAsync().Result);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("<OTA_HotelRatePlanRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"a9b04a84-edc2-4639-8a55-829ec506b884\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"" + ex.Message + "\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelRatePlanRS>", Encoding.UTF8, "application/xml")

                };
            }
            var str = xmlDoc.InnerXml;

            clsUtil objUtil = new clsUtil();
            Int64 iID = objUtil.UpdateChannelMgrTracker(0, 2, str);

            if (str == null)
            {
                return null;
            }

            #region "OTA_HotelRatePlan -- Fetch Room Rate Plan information"
            else if (str.IndexOf("OTA_HotelRatePlanRQ") > 0)
            {
                try
                {
                    int i = 0, iTotalRows = 0;
                    string SummaryOnly = xmlDoc.GetElementsByTagName("OTA_HotelRatePlanRQ")[0].Attributes["SummaryOnly"].Value;
                    EchoToken = xmlDoc.GetElementsByTagName("OTA_HotelRatePlanRQ")[0].Attributes["EchoToken"] != null ? xmlDoc.GetElementsByTagName("OTA_HotelRatePlanRQ")[0].Attributes["EchoToken"].Value : "";
                    string sStartDate = "", sEndDate = "", HotelCode = "";

                    if (SummaryOnly == "false") //If SummaryOnly =true means requesting only for room rateplan information. If SummaryOnly=false means requesting for rates                   
                    {
                        sStartDate = xmlDoc.GetElementsByTagName("DateRange")[0].Attributes["Start"].Value;
                        sEndDate = xmlDoc.GetElementsByTagName("DateRange")[0].Attributes["End"].Value;
                    }

                    HotelCode = xmlDoc.GetElementsByTagName("HotelRef")[0].Attributes["HotelCode"].Value;

                    SqlParameter[] MyParam = new SqlParameter[4];
                    MyParam[0] = new SqlParameter("@HotelCode", HotelCode);
                    MyParam[1] = new SqlParameter("@Rates", SummaryOnly);
                    MyParam[2] = new SqlParameter("@Start", sStartDate);
                    MyParam[3] = new SqlParameter("@End", sEndDate);

                    DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetOFRHotelRPForMaximojoChannelMgr", MyParam);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<OTA_HotelRatePlanRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\">");

                    if (ds != null && ds.Tables.Count > 3 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sb.Append("<Success/>");
                        sb.Append("<RatePlans HotelCode=\"" + HotelCode + "\">");

                        if (SummaryOnly == "true") //If SummaryOnly =true means requesting only for room rateplan information. If SummaryOnly=false means requesting for rates                   
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                sb.Append("<RatePlan CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" RatePlanCode=\"" + dr["iRPId"].ToString() + "\" RatePlanStatusType=\"" + dr["cStatus"].ToString() + "\" >");

                                if (ds != null && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    sb.Append("<Rates>");
                                    foreach (DataRow dr1 in ds.Tables[2].Rows)
                                    {
                                        if (dr["iRPId"].ToString() == dr1["iRPId"].ToString())
                                        {
                                            sb.Append("<Rate InvCode=\"" + dr1["sRoomName"].ToString() + "\" InvTypeCode=\"" + dr1["iRoomId"].ToString() + "\"/>");
                                        }
                                    }
                                    sb.Append("</Rates>");

                                }
                                sb.Append("<Description Name=\"" + dr["sRatePlan"].ToString() + "\">");
                                sb.Append("<Text Language=\"USENGLISH\">" + dr["sRatePlan"].ToString() + "</Text>");
                                sb.Append("</Description>");
                                sb.Append("</RatePlan>");
                            }
                        }
                        else
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                sb.Append("<RatePlan CurrencyCode=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\" End=\"" + dr["dtStayDayEnd"].ToString() + "\" RatePlanCode=\"" + dr["iRPPromoId"].ToString() + "\" RatePlanStatusType=\"" + dr["cStatus"].ToString() + "\" Start=\"" + dr["dtStayDayStart"].ToString() + "\">");

                                if (ds != null && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    sb.Append("<Rates>");
                                    i = 1;
                                    foreach (DataRow dr1 in ds.Tables[2].Rows)
                                    {
                                        if (dr["iRPPromoId"].ToString() == dr1["iRPPromoId"].ToString() && dr["dtStayDayStart"].ToString() == dr1["dtStayDayStart"].ToString())
                                        {
                                            iTotalRows = Convert.ToInt16(dr1["Cnt"].ToString());

                                            foreach (DataRow dr2 in ds.Tables[3].Rows)
                                            {
                                                if (dr1["iRPPromoId"].ToString() == dr2["iRPPromoId"].ToString() && dr1["iRoomId"].ToString() == dr2["iRoomId"].ToString() && dr1["dtStayDayStart"].ToString() == dr2["dtStayDayStart"].ToString())
                                                {

                                                    if (i == 1)
                                                    {
                                                        sb.Append("<Rate InvTypeCode=\"" + dr1["iRoomId"].ToString() + "\" End=\"" + dr["dtStayDayEnd"].ToString() + "\" Start=\"" + dr1["dtStayDayStart"].ToString() + "\">");
                                                        sb.Append("<BaseByGuestAmts>");
                                                        sb.Append("<BaseByGuestAmt AmountBeforeTax=\"" + dr2["BeforeTax"].ToString() + "\" NumberOfGuests=\"" + dr2["iOccupancy"].ToString() + "\"/>");

                                                    }
                                                    if (i == iTotalRows)
                                                    {
                                                        if (i > 1)
                                                        {
                                                            sb.Append("<BaseByGuestAmt AmountBeforeTax=\"" + dr2["BeforeTax"].ToString() + "\" NumberOfGuests=\"" + dr2["iOccupancy"].ToString() + "\"/>");
                                                        }
                                                        sb.Append("</BaseByGuestAmts>");
                                                        sb.Append("<AdditionalGuestAmounts>");
                                                        sb.Append("<AdditionalGuestAmount AgeQualifyingCode=\"10\" Amount=\"" + ds.Tables[0].Rows[0]["dExtraBedCharges"].ToString() + "\"/>");
                                                        sb.Append("<AdditionalGuestAmount AgeQualifyingCode=\"8\" Amount=\"" + ds.Tables[0].Rows[0]["dExtraBedChargesChild"].ToString() + "\"/>");
                                                        sb.Append("</AdditionalGuestAmounts>");
                                                        sb.Append("</Rate>");
                                                        i = 1;
                                                        break;
                                                    }
                                                    if (i > 1)
                                                    {
                                                        sb.Append("<BaseByGuestAmt AmountBeforeTax=\"" + dr2["BeforeTax"].ToString() + "\" NumberOfGuests=\"" + dr2["iOccupancy"].ToString() + "\"/>");
                                                    }
                                                    i++;
                                                }

                                            }
                                        }
                                    }
                                    sb.Append("</Rates>");

                                }

                                sb.Append("</RatePlan>");
                            }
                        }
                        sb.Append("</RatePlans>");
                        sb.Append("</OTA_HotelRatePlanRS>");

                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                    else if (ds != null && ds.Tables.Count == 1)
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<OTA_HotelRatePlanRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"" + ds.Tables[0].Rows[0]["ErrDesc"].ToString() + "\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelRatePlanRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<OTA_HotelRatePlanRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"Invalid details\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelRatePlanRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<OTA_HotelRatePlanRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"" + ex.Message + "\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelRatePlanRS>", Encoding.UTF8, "application/xml")

                    };
                }
            }
            #endregion

            #region "OTA_HotelAvailGet -- Fetch the availability, stop sell status and minimum stays for one or more room rate plans for a single hotel."
            else if (str.IndexOf("OTA_HotelAvailGetRQ") > 0)
            {
                try
                {
                    string TransactionIdentifier = "";
                    if (xmlDoc.GetElementsByTagName("OTA_HotelAvailGetRQ")[0].Attributes["TransactionIdentifier"] != null)
                    {
                        TransactionIdentifier = xmlDoc.GetElementsByTagName("OTA_HotelAvailGetRQ")[0].Attributes["TransactionIdentifier"].Value;
                    }


                    EchoToken = xmlDoc.GetElementsByTagName("OTA_HotelAvailGetRQ")[0].Attributes["EchoToken"] != null ? xmlDoc.GetElementsByTagName("OTA_HotelAvailGetRQ")[0].Attributes["EchoToken"].Value : "";
                    string sStartDate = "", sEndDate = "", HotelCode = "";

                    sStartDate = xmlDoc.GetElementsByTagName("DateRange")[0].Attributes["Start"].Value;
                    sEndDate = xmlDoc.GetElementsByTagName("DateRange")[0].Attributes["End"].Value;

                    HotelCode = xmlDoc.GetElementsByTagName("HotelRef")[0].Attributes["HotelCode"].Value;

                    SqlParameter[] MyParam = new SqlParameter[3];
                    MyParam[0] = new SqlParameter("@HotelCode", HotelCode);
                    MyParam[1] = new SqlParameter("@Start", sStartDate);
                    MyParam[2] = new SqlParameter("@End", sEndDate);

                    DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetOFRHotelARIForMaximojoChannelMgr", MyParam);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<OTA_HotelAvailGetRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\">");

                    if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["cStatus"] == "0")
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<OTA_HotelAvailGetRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"" + ds.Tables[0].Rows[0]["ErrDesc"].ToString() + "\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelAvailGetRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                    else if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 && ds.Tables[0].Rows[0]["cStatus"].ToString() == "1")
                    {
                        sb.Append("<Success/>");
                        sb.Append("<AvailStatusMessages HotelCode=\"" + HotelCode + "\">");


                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            sb.Append("<AvailStatusMessage BookingLimit=\"" + dr["iAvailableInventory"].ToString() + "\" >");

                            sb.Append("<StatusApplicationControl End=\"" + dr["dtStayDayEnd"].ToString() + "\" Start=\"" + dr["dtStayDayStart"].ToString() + "\" InvTypeCode=\"" + dr["iRoomID"].ToString() + "\" RatePlanCode=\"" + dr["iRPPromoId"].ToString() + "\" />");

                            sb.Append("<RestrictionStatus Restriction=\"Master\" Status=\"" + dr["CloseOut"].ToString() + "\" />");

                            if (dr["iMinLengthStayRP"].ToString() != "0")
                            {
                                sb.Append("<LengthsOfStay>");
                                sb.Append("<LengthOfStay MinMaxMessageType=\"SetMinLOS\" Time=\"" + dr["iMinLengthStayRP"].ToString() + "\" /> ");

                                if (dr["iMinLengthStayRP"].ToString() != "0")
                                {
                                    sb.Append("<LengthOfStay MinMaxMessageType=\"SetMaxLOS\" Time=\"" + dr["iMaxLengthStayRP"].ToString() + "\" /> ");
                                }

                                sb.Append("</LengthsOfStay>");
                            }

                            sb.Append("</AvailStatusMessage>");
                        }


                        sb.Append("</AvailStatusMessages>");
                        sb.Append("</OTA_HotelAvailGetRS>");

                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<OTA_HotelAvailGetRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"No records found.\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelAvailGetRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception ex)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 2, null);
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<OTA_HotelAvailGetRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"" + ex.Message + "\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelAvailGetRS>", Encoding.UTF8, "application/xml")

                    };
                }

            }
            #endregion

            #region "OTA_HotelAvailGet -- Update the availability, stop sell status and minimum stays for one or more room types for a single hotel."
            else if (str.IndexOf("OTA_HotelAvailNotif") > 0)
            {
                try
                {
                    string sStartDate = "", sEndDate = "", HotelCode = "", InvTypeCode = "-1", RatePlanCode = "-1", ErrDesc = "";
                    Int16 Mon = 1, Tue = 1, Wed = 1, Thu = 1, Fri = 1, Sat = 1, Sun = 1;
                    bool StopSell = false, status = true;
                    int BookingLimit = -1, SetMinLOS = -1, SetMaxLOS = -1;
                    StringBuilder sb = new StringBuilder();

                    XmlNodeList xmlnode;
                    HotelCode = xmlDoc.GetElementsByTagName("AvailStatusMessages")[0].Attributes["HotelCode"].Value;
                    xmlnode = xmlDoc.GetElementsByTagName("AvailStatusMessage");

                    for (int i = 0; i <= xmlnode.Count - 1; i++)
                    {
                        BookingLimit = (xmlnode[i].Attributes["BookingLimit"] != null) ? Convert.ToInt16(xmlnode[i].Attributes["BookingLimit"].Value) : 0;

                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Start"] != null)
                        {
                            sStartDate = xmlnode[i].ChildNodes.Item(0).Attributes["Start"].Value;
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["End"] != null)
                        {
                            sEndDate = xmlnode[i].ChildNodes.Item(0).Attributes["End"].Value;
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["InvTypeCode"] != null)
                        {
                            InvTypeCode = xmlnode[i].ChildNodes.Item(0).Attributes["InvTypeCode"].Value;
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["RatePlanCode"] != null)
                        {
                            RatePlanCode = xmlnode[i].ChildNodes.Item(0).Attributes["RatePlanCode"].Value;
                        }

                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Mon"] != null)
                        {
                            Mon = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Mon"].Value);
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Tue"] != null)
                        {
                            Tue = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Tue"].Value);
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Weds"] != null)
                        {
                            Wed = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Weds"].Value);
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Thur"] != null)
                        {
                            Thu = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Thur"].Value);
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Fri"] != null)
                        {
                            Fri = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Fri"].Value);
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Sat"] != null)
                        {
                            Sat = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Sat"].Value);
                        }
                        if (xmlnode[i].ChildNodes.Item(0).Attributes["Sun"] != null)
                        {
                            Sun = Convert.ToInt16(xmlnode[i].ChildNodes.Item(0).Attributes["Sun"].Value);
                        }

                        if (xmlnode[i].ChildNodes.Count > 1)
                        {
                            for (int j = 0; j < xmlnode[i].ChildNodes.Count; j++)
                            {
                                if (xmlnode[i].ChildNodes[j].Name == "LengthsOfStay")
                                {
                                    if (xmlnode[i].ChildNodes[j].HasChildNodes)
                                    {
                                        for (int k = 0; k < xmlnode[i].ChildNodes[j].ChildNodes.Count; k++)
                                        {
                                            if (xmlnode[i].ChildNodes[j].ChildNodes[k].Attributes["MinMaxMessageType"] != null)
                                            {
                                                if (xmlnode[i].ChildNodes[j].ChildNodes[k].Attributes["MinMaxMessageType"].Value == "SetMinLOS")
                                                {
                                                    SetMinLOS = Convert.ToInt16(xmlnode[i].ChildNodes.Item(1).ChildNodes.Item(0).Attributes["Time"].Value);
                                                }
                                                else if (xmlnode[i].ChildNodes[j].ChildNodes[k].Attributes["MinMaxMessageType"].Value == "SetMaxLOS")
                                                {
                                                    SetMaxLOS = Convert.ToInt16(xmlnode[i].ChildNodes.Item(1).ChildNodes.Item(0).Attributes["Time"].Value);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (xmlnode[i].ChildNodes[j].Name == "RestrictionStatus")
                                {
                                    StopSell = (xmlnode[i].ChildNodes[j].Attributes["Status"].Value == "Close") ? true : false;
                                }
                            }
                        }

                        SqlParameter[] MyParam = new SqlParameter[16];
                        MyParam[0] = new SqlParameter("@HotelCode", HotelCode);
                        MyParam[1] = new SqlParameter("@BookingLimit", BookingLimit);
                        MyParam[2] = new SqlParameter("@iRoomId", InvTypeCode);
                        MyParam[3] = new SqlParameter("@iRPId", RatePlanCode);
                        MyParam[4] = new SqlParameter("@Start", sStartDate);
                        MyParam[5] = new SqlParameter("@End", sEndDate);
                        MyParam[6] = new SqlParameter("@MinLOS", SetMinLOS);
                        MyParam[7] = new SqlParameter("@MaxLOS", SetMaxLOS);
                        MyParam[8] = new SqlParameter("@StopSell", StopSell);
                        MyParam[9] = new SqlParameter("@Mon", Mon);
                        MyParam[10] = new SqlParameter("@Tue", Tue);
                        MyParam[11] = new SqlParameter("@Wed", Wed);
                        MyParam[12] = new SqlParameter("@Thu", Thu);
                        MyParam[13] = new SqlParameter("@Fri", Fri);
                        MyParam[14] = new SqlParameter("@Sat", Sat);
                        MyParam[15] = new SqlParameter("@Sun", Sun);

                        DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateHotelASSForMaximojoChannelMgr", MyParam);

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "0")
                            {
                                status = false;
                                ErrDesc = ds.Tables[0].Rows[0]["ErrDesc"].ToString();
                                break;
                            }
                            else
                            {
                                status = true;
                            }
                        }
                        else
                        {
                            status = false;
                            ErrDesc = "No record found.";
                            break;
                        }
                    }

                    if (!status)
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<OTA_HotelAvailNotifRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"><Errors xmlns=\"http://www.opentravel.org/OTA/2003/05\"><Error Type=\"3\" ShortText=\"" + ErrDesc + "\" Code=\"448\" Status=\"NotProcessed\" /></Errors></OTA_HotelAvailNotifRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        sb.Append("<OTA_HotelAvailNotifRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\">");
                        sb.Append("<Success/>");
                        sb.Append("</OTA_HotelAvailNotifRS>");

                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }

                }
                catch (Exception ex)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 2, null);
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<OTA_HotelAvailNotifRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"><Errors xmlns=\"http://www.opentravel.org/OTA/2003/05\"><Error Type=\"3\" ShortText=\"" + ex.Message + "\" Code=\"448\" Status=\"NotProcessed\" /></Errors></OTA_HotelAvailNotifRS>", Encoding.UTF8, "application/xml")

                    };
                }

            }
            #endregion

            #region "OTA_HotelRateAmountNotifRQ -- Update rates and inclusions for one or more room types for a single hotel."
            else if (str.IndexOf("OTA_HotelRateAmountNotif") > 0)
            {
                try
                {
                    string sStartDate = "", sEndDate = "", HotelCode = "", InvTypeCode = "-1", RatePlanCode = "-1";
                    bool Mon = true, Tue = true, Wed = true, Thu = true, Fri = true, Sat = true, Sun = true, status = false;
                    string AgeQualifyingCode = "10", AgeQualifyingCodeAdult = "10", CurrencyCode = "INR", ErrDesc = "";
                    decimal AmountBeforeTax = 0, AmountAfterTax = 0, AdditionalGuestAmount = 0, AdditionalGuestAmountAdult = 0;
                    int NoofGuests = -1;
                    StringBuilder sb = new StringBuilder();

                    XmlNodeList xmlnode;
                    HotelCode = xmlDoc.GetElementsByTagName("RateAmountMessages")[0].Attributes["HotelCode"].Value;
                    xmlnode = xmlDoc.GetElementsByTagName("RateAmountMessage");

                    DataTable OccAmount = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("RatePlan", typeof(int));
                    OccAmount.Columns.Add(col);
                    col = new DataColumn("dPrice", typeof(double));
                    OccAmount.Columns.Add(col);
                    col = new DataColumn("bIsPromo", typeof(bool));
                    OccAmount.Columns.Add(col);

                    for (int i = 0; i <= xmlnode.Count - 1; i++)
                    {
                        if (xmlnode[i].ChildNodes.Count > 1)
                        {
                            //StatusApplicationControl TAG
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["InvTypeCode"] != null)
                            {
                                InvTypeCode = xmlnode[i].ChildNodes.Item(0).Attributes["InvTypeCode"].Value;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["RatePlanCode"] != null)
                            {
                                RatePlanCode = xmlnode[i].ChildNodes.Item(0).Attributes["RatePlanCode"].Value;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Start"] != null)
                            {
                                sStartDate = xmlnode[i].ChildNodes.Item(0).Attributes["Start"].Value;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["End"] != null)
                            {
                                sEndDate = xmlnode[i].ChildNodes.Item(0).Attributes["End"].Value;
                            }


                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Mon"] != null)
                            {
                                Mon = xmlnode[i].ChildNodes.Item(0).Attributes["Mon"].Value == "true" ? true : false;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Tue"] != null)
                            {
                                Tue = xmlnode[i].ChildNodes.Item(0).Attributes["Tue"].Value == "true" ? true : false;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Weds"] != null)
                            {
                                Wed = xmlnode[i].ChildNodes.Item(0).Attributes["Weds"].Value == "true" ? true : false;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Thur"] != null)
                            {
                                Thu = xmlnode[i].ChildNodes.Item(0).Attributes["Thur"].Value == "true" ? true : false;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Fri"] != null)
                            {
                                Fri = xmlnode[i].ChildNodes.Item(0).Attributes["Fri"].Value == "true" ? true : false;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Sat"] != null)
                            {
                                Sat = xmlnode[i].ChildNodes.Item(0).Attributes["Sat"].Value == "true" ? true : false;
                            }
                            if (xmlnode[i].ChildNodes.Item(0).Attributes["Sun"] != null)
                            {
                                Sun = xmlnode[i].ChildNodes.Item(0).Attributes["Sun"].Value == "true" ? true : false;
                            }

                            //Rates TAG
                            if (xmlnode[i].ChildNodes.Item(1).HasChildNodes)
                            {
                                for (int j = 0; j <= xmlnode[i].ChildNodes.Item(1).ChildNodes.Count - 1; j++)
                                {

                                    ////RATES
                                    DataRow drOccAmount;
                                    if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].HasChildNodes)
                                    {
                                        for (int k = 0; k <= xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes.Count - 1; k++)
                                        {
                                            if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].Attributes["CurrencyCode"] != null)
                                            {
                                                CurrencyCode = xmlnode[i].ChildNodes.Item(1).ChildNodes[j].Attributes["CurrencyCode"].Value;
                                            }
                                            if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].HasChildNodes)
                                            {
                                                for (int l = 0; l <= xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes.Count - 1; l++)
                                                {
                                                    if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "AdditionalGuestAmount")
                                                    {
                                                        AgeQualifyingCode = xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["AgeQualifyingCode"].Value;
                                                        AdditionalGuestAmount = Convert.ToDecimal(xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["Amount"].Value);
                                                        if (AgeQualifyingCode == "10")
                                                        {
                                                            AgeQualifyingCodeAdult = AgeQualifyingCode;
                                                            AdditionalGuestAmountAdult = AdditionalGuestAmount;
                                                        }
                                                    }
                                                    if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "BaseByGuestAmt")
                                                    {
                                                        NoofGuests = Convert.ToInt16(xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["NumberOfGuests"].Value);
                                                        if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["AmountAfterTax"] != null)
                                                        {
                                                            AmountAfterTax = Convert.ToDecimal(xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["AmountAfterTax"].Value);
                                                        }
                                                        if (xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["AmountBeforeTax"] != null)
                                                        {
                                                            AmountBeforeTax = Convert.ToDecimal(xmlnode[i].ChildNodes.Item(1).ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes["AmountBeforeTax"].Value);
                                                        }

                                                        drOccAmount = OccAmount.NewRow();
                                                        drOccAmount["RatePlan"] = NoofGuests;
                                                        drOccAmount["dPrice"] = AmountBeforeTax;
                                                        drOccAmount["bIsPromo"] = false;
                                                        OccAmount.Rows.Add(drOccAmount);
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    //////////UPDATE DATABASE
                                    if (AgeQualifyingCodeAdult == "8")
                                    {
                                        AgeQualifyingCodeAdult = "10";
                                        AdditionalGuestAmountAdult = AdditionalGuestAmount;
                                    }

                                    SqlParameter[] MyParam = new SqlParameter[15];
                                    MyParam[0] = new SqlParameter("@HotelCode", HotelCode);
                                    MyParam[1] = new SqlParameter("@iRoomId", InvTypeCode);
                                    MyParam[2] = new SqlParameter("@iRPId", RatePlanCode);
                                    MyParam[3] = new SqlParameter("@Start", sStartDate);
                                    MyParam[4] = new SqlParameter("@End", sEndDate);
                                    MyParam[5] = new SqlParameter("@Mon", Mon);
                                    MyParam[6] = new SqlParameter("@Tue", Tue);
                                    MyParam[7] = new SqlParameter("@Wed", Wed);
                                    MyParam[8] = new SqlParameter("@Thu", Thu);
                                    MyParam[9] = new SqlParameter("@Fri", Fri);
                                    MyParam[10] = new SqlParameter("@Sat", Sat);
                                    MyParam[11] = new SqlParameter("@Sun", Sun);
                                    MyParam[12] = new SqlParameter("@AdditionalGuestAmount", AdditionalGuestAmountAdult);
                                    MyParam[13] = new SqlParameter("@OccAmount", OccAmount);
                                    MyParam[14] = new SqlParameter("@CurrencyCode", CurrencyCode);
                                    DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateHotelAmountPerOccForMaximojoChannelMgr", MyParam);

                                    OccAmount.Clear();
                                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows[0][0].ToString() == "0")
                                        {
                                            status = false;
                                            ErrDesc = ds.Tables[0].Rows[0]["ErrDesc"].ToString();
                                            break;
                                        }
                                        else
                                        {
                                            status = true;
                                        }
                                    }
                                    else
                                    {
                                        status = false;
                                        ErrDesc = "No record found.";
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!status)
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<OTA_HotelRateAmountNotifRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"><Errors xmlns=\"http://www.opentravel.org/OTA/2003/05\"><Error Type=\"3\" ShortText=\"" + ErrDesc + "\" Code=\"448\" Status=\"NotProcessed\" /></Errors></OTA_HotelRateAmountNotifRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        sb.Append("<OTA_HotelRateAmountNotifRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\">");
                        sb.Append("<Success/>");
                        sb.Append("</OTA_HotelRateAmountNotifRS>");

                        objUtil.UpdateChannelMgrTracker(iID, 2, null);
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception ex)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 2, null);
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<OTA_HotelRateAmountNotifRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"><Errors xmlns=\"http://www.opentravel.org/OTA/2003/05\"><Error Type=\"3\" ShortText=\"" + ex.Message + "\" Code=\"448\" Status=\"NotProcessed\" /></Errors></OTA_HotelRateAmountNotifRS>", Encoding.UTF8, "application/xml")

                    };
                }

            }
            #endregion
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<OTA_HotelRatePlanRS xmlns=\"http://www.opentravel.org/OTA/2003/05\" EchoToken=\"" + EchoToken + "\" TimeStamp=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'") + "\"  Version=\"1.0\"> <Error Type=\"3\" ShortText=\"Invalid deatils\" Code=\"448\" Status=\"NotProcessed\" /> </OTA_HotelRatePlanRS>");

                return new HttpResponseMessage()
                {
                    Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                };
            }

        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}