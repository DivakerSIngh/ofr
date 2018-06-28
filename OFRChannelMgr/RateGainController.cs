using OFRChannelMgr.DB_Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace OFRChannelMgr
{
    public class RateGainController : ApiController
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
            // Reading data as XML string to log to files - In case message structure is changed
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlreq.Content.ReadAsStreamAsync().Result);
            var str = xmlDoc.InnerXml;

            clsUtil objUtil = new clsUtil();
            Int64 iID = objUtil.UpdateChannelMgrTracker(0, 1, str);

            if (str == null)
            {
                return null;
            }
            #region "HotelPropertyListGet"
            else if (str.IndexOf("HotelPropertyListGetRQ") > 0)
            {
                //SqlParameter[] MyParam = new SqlParameter[2];
                //MyParam[0] = new SqlParameter("@devicetype", DeviceType);
                //MyParam[1] = new SqlParameter("@deviceid", DeviceId);
                //SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspSaveDeviceInfo", MyParam);

                //XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/HotelPropertyListGetRQ/Authentication");
                //for (int i = 0; i < nodeList.Count; i++)
                //{
                //    string UserName = nodeList[i].Attributes["UserName"].Value;
                //    string Password = nodeList[i].Attributes["Password"].Value;
                //}
                string UserName = "", Password = "";
                try
                {
                    UserName = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["UserName"].Value;
                    Password = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["Password"].Value;

                    // clsUtil objUtil = new clsUtil();
                    if (objUtil.IsAuthenticatedRateGain(UserName, Password))
                    {
                        DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetOFRhotelsForChannelMgr");

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<HotelPropertyListGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Success/>");

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow dr1 in ds.Tables[1].Rows)
                                {
                                    sb.Append("<Hotels ChainCode=\"" + dr1["iChainId"].ToString() + "\" >");
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        if (dr1["iChainId"].ToString() == dr["iChainId"].ToString())
                                        {
                                            sb.Append("<Hotel HotelCode=\"" + dr["iPropId"].ToString() + "\" Name=\"" + dr["sHotelName"].ToString() + "\"> </Hotel>");
                                        }
                                    }
                                    sb.Append("</Hotels>");
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    sb.Append("<Hotels ChainCode=\"" + dr["iPropId"].ToString() + "\" >");
                                    sb.Append("<Hotel HotelCode=\"" + dr["iPropId"].ToString() + "\" Name=\"" + dr["sHotelName"].ToString() + "\"> </Hotel>");
                                    sb.Append("</Hotels>");
                                }
                                goto A;
                            }
                            foreach (DataRow dr in ds.Tables[0].Select("iChainId is null"))
                            {
                                sb.Append("<Hotels ChainCode=\"" + dr["iPropId"].ToString() + "\" >");
                                sb.Append("<Hotel HotelCode=\"" + dr["iPropId"].ToString() + "\" Name=\"" + dr["sHotelName"].ToString() + "\"> </Hotel>");
                                sb.Append("</Hotels>");
                            }
                        }
                        A:
                        sb.Append("</HotelPropertyListGetRS>");
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<HotelPropertyListGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. </Error> </Errors> </HotelPropertyListGetRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 1, "");
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<HotelPropertyListGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. </Error> </Errors> </HotelPropertyListGetRS>", Encoding.UTF8, "application/xml")

                    };
                }
            }
            #endregion
            #region "HotelProductListGet"
            else if (str.IndexOf("HotelProductListGetRQ") > 0)
            {
                string UserName = "", Password = "";
                try
                {
                    UserName = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["UserName"].Value;
                    Password = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["Password"].Value;

                    // clsUtil objUtil = new clsUtil();
                    if (objUtil.IsAuthenticatedRateGain(UserName, Password))
                    {
                        string HotelCode = xmlDoc.GetElementsByTagName("HotelProductListRequest")[0].Attributes["HotelCode"].Value;

                        SqlParameter[] MyParam = new SqlParameter[1];
                        MyParam[0] = new SqlParameter("@HotelCode", HotelCode);

                        DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetOFRHotelProductsForChannelMgr", MyParam);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<HotelProductListGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\">");

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "0")
                            {
                                sb.Append("<Errors> <Error Type=\"3\" Code=\"392\" > Invalid hotel code. </Error> </Errors>");
                            }
                            else
                            {
                                if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                {
                                    sb.Append("<Success/>");
                                    sb.Append("<HotelProducts HotelCode=\"" + HotelCode + "\">");

                                    foreach (DataRow dr in ds.Tables[1].Rows)
                                    {
                                        sb.Append("<HotelProduct> <ProductReference InvTypeCode=\"" + dr["iRoomId"].ToString() + "\" RatePlanCode=\"" + dr["iRPId"].ToString() + "\"> </ProductReference> <RoomTypeName>" + dr["sRoomName"].ToString() + "</RoomTypeName><RateTypeName>" + dr["sRatePlan"].ToString() + "</RateTypeName> </HotelProduct>");
                                        //<HotelProduct> <ProductReference InvTypeCode="DLX" RatePlanCode="BAR"> </ProductReference> <RoomTypeName>Deluxe King</RoomTypeName> <RateTypeName>Super Saver</RateTypeName> </HotelProduct>
                                    }
                                    sb.Append("</HotelProducts>");
                                }
                                else
                                {
                                    sb.Append("<Errors> <Error Type=\"3\" Code=\"392\" > Invalid hotel code. </Error> </Errors>");
                                }
                            }
                        }
                        else
                        {
                            sb.Append("<Success/>");
                        }
                        sb.Append("</HotelProductListGetRS>");
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<HotelProductListGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. </Error> </Errors> </HotelProductListGetRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 1, "");
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<HotelProductListGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. </Error> </Errors> </HotelProductListGetRS>", Encoding.UTF8, "application/xml")

                    };
                }
            }
            #endregion
            #region "HotelARIGetRQ"
            else if (str.IndexOf("HotelARIGetRQ") > 0)
            {
                string UserName = "", Password = "";
                try
                {
                    UserName = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["UserName"].Value;
                    Password = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["Password"].Value;

                    //clsUtil objUtil = new clsUtil();
                    if (objUtil.IsAuthenticatedRateGain(UserName, Password))
                    {
                        string HotelCode = xmlDoc.GetElementsByTagName("HotelARIGetRequests")[0].Attributes["HotelCode"].Value;
                        string InvTypeCode = xmlDoc.GetElementsByTagName("ProductReference")[0].Attributes["InvTypeCode"].Value;
                        string RatePlanCode = xmlDoc.GetElementsByTagName("ProductReference")[0].Attributes["RatePlanCode"].Value;
                        string Start = xmlDoc.GetElementsByTagName("ApplicationControl")[0].Attributes["Start"].Value;
                        string End = xmlDoc.GetElementsByTagName("ApplicationControl")[0].Attributes["End"].Value;

                        SqlParameter[] MyParam = new SqlParameter[5];
                        MyParam[0] = new SqlParameter("@HotelCode", HotelCode);
                        MyParam[1] = new SqlParameter("@InvTypeCode", InvTypeCode);
                        MyParam[2] = new SqlParameter("@RatePlanCode", RatePlanCode);
                        MyParam[3] = new SqlParameter("@Start", Start);
                        MyParam[4] = new SqlParameter("@End", End);

                        DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetOFRHotelARIForChannelMgr", MyParam);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<HotelARIGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\">");

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "0")
                            {
                                sb.Append("<Errors> <Error Type=\"3\" Code=\"" + ds.Tables[0].Rows[0]["ErrorCode"].ToString() + "\" >" + ds.Tables[0].Rows[0]["ErrDesc"].ToString() + " </Error> </Errors>");
                                sb.Append("</HotelARIGetRS>");
                                objUtil.UpdateChannelMgrTracker(iID, 1, "");
                                return new HttpResponseMessage()
                                {
                                    Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                                };
                            }
                            else
                            {
                                if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                {
                                    sb.Append("<Success/>");
                                    sb.Append("<HotelARIDataSet HotelCode=\"" + HotelCode + "\">");

                                    int iTotalRows = 0, i = 0;

                                    foreach (DataRow dr1 in ds.Tables[1].Rows)
                                    {
                                        iTotalRows = Convert.ToInt16(dr1["iCount"].ToString());

                                        if (iTotalRows > 0)
                                        {
                                            sb.Append("<HotelARIData ItemIdentifier=\"1\">");
                                            sb.Append("<ProductReference InvTypeCode=\"" + InvTypeCode + "\" RatePlanCode=\"" + RatePlanCode + "\"> </ProductReference>");
                                            sb.Append("<ApplicationControl Start=\"" + dr1["dtStayDayStart"].ToString() + "\" End=\"" + dr1["dtStayDayEnd"].ToString() + "\"/>");
                                            sb.Append("<RateAmounts Currency=\"" + ds.Tables[0].Rows[0]["sCurrencyCode"].ToString() + "\">");
                                            i = 0;
                                            foreach (DataRow dr in ds.Tables[2].Rows)
                                            {

                                                if (dr1["dtStayDayStart"].ToString() == dr["dtStayDayStart"].ToString() && dr1["dtStayDayEnd"].ToString() == dr["dtStayDayEnd"].ToString())
                                                {
                                                    i++;
                                                    sb.Append("<Base OccupancyCode=\"" + dr["iOccupancy"].ToString() + "\" Amount=\"" + dr["dPrice"].ToString() + "\"/>");
                                                }

                                                if (i == iTotalRows)
                                                {
                                                    sb.Append("<Additional OccupancyCode=\"AA\" Amount=\"" + ds.Tables[0].Rows[0]["dExtraBedCharges"].ToString() + "\"/>");
                                                    sb.Append("</RateAmounts>");

                                                    sb.Append("<Availability Master=\"" + dr["CloseOut"].ToString() + "\" Arrival=\"" + dr["CTA"].ToString() + "\" Departure=\"" + dr["CTD"].ToString() + "\"/>");

                                                    sb.Append("<BookingLimit Sold=\"" + dr["iSoldInventory"].ToString() + "\">");
                                                    //sb.Append("<BaseAllotment ReleaseDays=\"0\" Sold=\"" + dr["iSoldInventory"].ToString() + "\" Allotment=\"" + dr["iAvailableInventory"].ToString() + "\"/>");
                                                    //sb.Append("<TransientAllotment ReleaseDays=\"0\" Sold=\"" + dr["iSoldInventory"].ToString() + "\" Allotment=\"" + dr["iAvailableInventory"].ToString() + "\"/>");
                                                    sb.Append("<BaseAllotment Sold=\"" + dr["iSoldInventory"].ToString() + "\" Allotment=\"" + dr["iAvailableInventory"].ToString() + "\"/>");
                                                    sb.Append("<TransientAllotment Sold=\"" + dr["iSoldInventory"].ToString() + "\" Allotment=\"" + dr["iAvailableInventory"].ToString() + "\"/>");

                                                    sb.Append("</BookingLimit>");

                                                    sb.Append("<BookingRules>");
                                                    if (dr["bIsBefore"].ToString() == "1")
                                                    {
                                                        sb.Append("<MinAdvancedBookingOffset>" + dr["MinAdvancedBookingOffset"].ToString() + "</MinAdvancedBookingOffset>");
                                                    }
                                                    else
                                                    {
                                                        sb.Append("<MaxAdvancedBookingOffset>" + dr["MaxAdvancedBookingOffset"].ToString() + "</MaxAdvancedBookingOffset>");
                                                    }



                                                    sb.Append("<MinLoSOnArrival>" + dr["iMinLengthStayRP"].ToString() + "</MinLoSOnArrival>");
                                                    sb.Append("<MaxLoSOnArrival>" + dr["iMaxLengthStayRP"].ToString() + "</MaxLoSOnArrival>");
                                                    //Commented on 02-May-18 as per RateGain Request
                                                    //sb.Append("<MinLoSThrough>" + dr["iMinLengthStayRP"].ToString() + "</MinLoSThrough>");
                                                    //sb.Append("<MaxLoSThrough>" + dr["iMaxLengthStayRP"].ToString() + "</MaxLoSThrough>");
                                                    sb.Append("</BookingRules>");
                                                    sb.Append("<Description>" + ds.Tables[0].Rows[0]["sRatePlan"].ToString() + "</Description> </HotelARIData>");
                                                    break;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            sb.Append("<HotelARIStatus ItemIdentifier=\"2\"> <ProductReference InvTypeCode=\"" + InvTypeCode + "\" RatePlanCode=\"" + RatePlanCode + "\">");
                                            sb.Append("</ProductReference> <ApplicationControl Start=\"" + dr1["dtStayDay"].ToString() + "\" End=\"" + dr1["dtStayDay"].ToString() + "\"/>");
                                            sb.Append("<Status Code=\"842\">No inventory loaded for the requested dates</Status> </HotelARIStatus>");
                                        }
                                    }

                                    sb.Append("</HotelARIDataSet>");
                                }
                                else
                                {
                                    sb.Append("<Errors> <Error Type=\"3\" Code=\"392\" > Invalid hotel code. </Error> </Errors>");
                                }
                            }
                        }
                        else
                        {
                            sb.Append("<Success/>");
                        }
                        sb.Append("</HotelARIGetRS>");
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<HotelARIGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. Uid:" + UserName + " Pwd:" + Password + " </Error> </Errors> </HotelARIGetRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 1, "");
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<HotelARIGetRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. </Error> </Errors> </HotelARIGetRS>", Encoding.UTF8, "application/xml")

                    };
                }
            }
            #endregion
            #region " HotelARIUpdateRQ - Request OTA to update the ARI information using this message"
            else if (str.IndexOf("HotelARIUpdateRQ") > 0)
            {
                string UserName = "", Password = "", Currency = "INR", UpdateType = "Partial", Master = "Open", Departure = "Open", Arrival = "Open", Description = "";
                string sStartDate = "", sEndDate = "", HotelCode = "", InvTypeCode = "", RatePlanCode = "";
                bool Mon = true, Tue = true, Wed = true, Thu = true, Fri = true, Sat = true, Sun = true;
                decimal AdditionalGuestAmount = 0;
                int ReleaseDays = -1, Allotment = -1, MinAdvancedBookingOffset = -1, MaxAdvancedBookingOffset = -1, MinLoSOnArrival = -1;
                int MaxLoSOnArrival = -1, MinLoSThrough = -1, MaxLoSThrough = -1;
                bool status = true;
                XmlNodeList xmlnode;
                StringBuilder sb = new StringBuilder();

                sb.Append("<HotelARIUpdateRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\">");


                try
                {
                    UserName = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["UserName"].Value;
                    Password = xmlDoc.GetElementsByTagName("Authentication")[0].Attributes["Password"].Value;

                    // clsUtil objUtil = new clsUtil();
                    if (objUtil.IsAuthenticatedRateGain(UserName, Password))
                    {
                        HotelCode = xmlDoc.GetElementsByTagName("HotelARIUpdateRequest")[0].Attributes["HotelCode"].Value;
                        UpdateType = xmlDoc.GetElementsByTagName("HotelARIUpdateRequest")[0].Attributes["UpdateType"].Value;

                        xmlnode = xmlDoc.GetElementsByTagName("HotelARIData");

                        int NoofGuests = -1, MealCode = 0;
                        decimal AmountAfterTax = 0;

                        DataTable MealCodes = new DataTable();
                        DataColumn col1 = null;
                        col1 = new DataColumn("Id", typeof(int));
                        MealCodes.Columns.Add(col1);

                        DataTable OccAmount = new DataTable();
                        DataColumn col = null;
                        col = new DataColumn("RatePlan", typeof(int));
                        OccAmount.Columns.Add(col);
                        col = new DataColumn("dPrice", typeof(double));
                        OccAmount.Columns.Add(col);
                        col = new DataColumn("bIsPromo", typeof(bool));
                        OccAmount.Columns.Add(col);

                        DataRow drOccAmount;
                        DataRow drMeals;
                        // --------------Loop on HotelARIData
                        for (int r = 0; r <= xmlnode.Count - 1; r++)
                        {
                            for (int c = 0; c <= xmlnode[r].ChildNodes.Count - 1; c++)
                            {
                                if (xmlnode[r].ChildNodes[c].Name == "ProductReference")
                                {
                                    if (xmlnode[r].ChildNodes[c].Attributes["InvTypeCode"] != null)
                                    {
                                        InvTypeCode = xmlnode[r].ChildNodes[c].Attributes["InvTypeCode"].Value;
                                    }
                                    if (xmlnode[r].ChildNodes[c].Attributes["RatePlanCode"] != null)
                                    {
                                        RatePlanCode = xmlnode[r].ChildNodes[c].Attributes["RatePlanCode"].Value;
                                    }
                                }

                                if (xmlnode[r].ChildNodes[c].Name == "ApplicationControl")
                                {
                                    if (xmlnode[r].ChildNodes[c].Attributes["Start"] != null)
                                    {
                                        sStartDate = xmlnode[r].ChildNodes[c].Attributes["Start"].Value;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["End"] != null)
                                    {
                                        sEndDate = xmlnode[r].ChildNodes[c].Attributes["End"].Value;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Mon"] != null)
                                    {
                                        Mon = xmlnode[r].ChildNodes[c].Attributes["Mon"].Value == "true" ? true : false;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Tue"] != null)
                                    {
                                        Tue = xmlnode[r].ChildNodes[c].Attributes["Tue"].Value == "true" ? true : false;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Wed"] != null)
                                    {
                                        Wed = xmlnode[r].ChildNodes[c].Attributes["Wed"].Value == "true" ? true : false;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Thu"] != null)
                                    {
                                        Thu = xmlnode[r].ChildNodes[c].Attributes["Thu"].Value == "true" ? true : false;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Fri"] != null)
                                    {
                                        Fri = xmlnode[r].ChildNodes[c].Attributes["Fri"].Value == "true" ? true : false;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Sat"] != null)
                                    {
                                        Sat = xmlnode[r].ChildNodes[c].Attributes["Sat"].Value == "true" ? true : false;
                                    }

                                    if (xmlnode[r].ChildNodes[c].Attributes["Sun"] != null)
                                    {
                                        Sun = xmlnode[r].ChildNodes[c].Attributes["Sun"].Value == "true" ? true : false;
                                    }
                                }

                                if (xmlnode[r].ChildNodes[c].Name == "RateAmounts")
                                {
                                    if (xmlnode[r].ChildNodes[c].Attributes["Currency"] != null)
                                    {
                                        Currency = xmlnode[r].ChildNodes[c].Attributes["Currency"].Value;
                                    }

                                    for (int k = 0; k <= xmlnode[r].ChildNodes[c].ChildNodes.Count - 1; k++)
                                    {
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "Additional")
                                        {
                                            if (xmlnode[r].ChildNodes[c].ChildNodes[k].Attributes["OccupancyCode"].Value == "AA")
                                            {
                                                AdditionalGuestAmount = Convert.ToDecimal(xmlnode[r].ChildNodes[c].ChildNodes[k].Attributes["Amount"].Value);
                                            }
                                            if (AdditionalGuestAmount < 0)
                                            {
                                                AdditionalGuestAmount = Convert.ToDecimal(xmlnode[r].ChildNodes[c].ChildNodes[k].Attributes["Amount"].Value);
                                            }
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "Base")
                                        {
                                            NoofGuests = objUtil.ConvertOccupancyCode(xmlnode[r].ChildNodes[c].ChildNodes[k].Attributes["OccupancyCode"].Value);
                                            AmountAfterTax = Convert.ToDecimal(xmlnode[r].ChildNodes[c].ChildNodes[k].Attributes["Amount"].Value);

                                            drOccAmount = OccAmount.NewRow();
                                            drOccAmount["RatePlan"] = NoofGuests;
                                            drOccAmount["dPrice"] = AmountAfterTax;
                                            if (AmountAfterTax > 99999999)
                                            {
                                                status = false;
                                                sb.Append("<Errors> <Error Type=\"3\" Code=\"143\" > Price can not be greater than 99999999. </Error> </Errors>");
                                                sb.Append("</HotelARIUpdateRS>");

                                                objUtil.UpdateChannelMgrTracker(iID, 1, "");
                                                return new HttpResponseMessage()
                                                {
                                                    Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                                                };
                                            }
                                            drOccAmount["bIsPromo"] = false;
                                            OccAmount.Rows.Add(drOccAmount);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MealPlans" && xmlnode[r].ChildNodes[c].ChildNodes[k].HasChildNodes)
                                        {
                                            for (int l = 0; l <= xmlnode[r].ChildNodes[c].ChildNodes[k].ChildNodes.Count - 1; l++)
                                            {
                                                //if (xmlnode[r].ChildNodes[c].ChildNodes[k].ChildNodes[l].Attributes["Included"] != null && xmlnode[r].ChildNodes[c].ChildNodes[k].ChildNodes[l].Attributes["Included"].Value == "true")
                                                //{
                                                if (xmlnode[r].ChildNodes[c].ChildNodes[k].ChildNodes[l].Attributes["MealPlanCode"] != null)
                                                {
                                                    drMeals = MealCodes.NewRow();
                                                    MealCode = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].ChildNodes[l].Attributes["MealPlanCode"].Value);
                                                    drMeals["Id"] = MealCode;
                                                    MealCodes.Rows.Add(drMeals);
                                                }
                                                //}
                                            }
                                        }
                                    }
                                }

                                ///----------------AVAILABILITY
                                if (xmlnode[r].ChildNodes[c].Name == "Availability")
                                {
                                    if (xmlnode[r].ChildNodes[c].Attributes["Master"] != null)
                                    {
                                        Master = xmlnode[r].ChildNodes[c].Attributes["Master"].Value;
                                    }
                                    if (xmlnode[r].ChildNodes[c].Attributes["Arrival"] != null)
                                    {
                                        Arrival = xmlnode[r].ChildNodes[c].Attributes["Arrival"].Value;
                                    }
                                    if (xmlnode[r].ChildNodes[c].Attributes["Departure"] != null)
                                    {
                                        Departure = xmlnode[r].ChildNodes[c].Attributes["Departure"].Value;
                                    }

                                }
                                // --------------------------Inventory
                                if (xmlnode[r].ChildNodes[c].Name == "BookingLimit")
                                {
                                    if (xmlnode[r].ChildNodes[c].ChildNodes[0] != null)
                                    {
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[0].Attributes["FreeSale"] != null && xmlnode[r].ChildNodes[c].ChildNodes[0].Attributes["FreeSale"].Value == "On")
                                        {
                                            Allotment = 100; //Unlimited
                                        }
                                        else
                                        {
                                            if (xmlnode[r].ChildNodes[c].ChildNodes[0].Attributes["ReleaseDays"] != null)
                                            {
                                                ReleaseDays = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[0].Attributes["ReleaseDays"].Value);
                                            }

                                            if (xmlnode[r].ChildNodes[c].ChildNodes[0].Attributes["Allotment"] != null)
                                            {
                                                Allotment = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[0].Attributes["Allotment"].Value);
                                            }
                                        }
                                    }
                                }
                                // --------------------------Min LOS
                                if (xmlnode[r].ChildNodes[c].Name == "BookingRules")
                                {

                                    for (int k = 0; k <= xmlnode[r].ChildNodes[c].ChildNodes.Count - 1; k++)
                                    {
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MinAdvancedBookingOffset")
                                        {
                                            MinAdvancedBookingOffset = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MaxAdvancedBookingOffset")
                                        {
                                            MaxAdvancedBookingOffset = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MinLoSOnArrival")
                                        {
                                            MinLoSOnArrival = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MaxLoSOnArrival")
                                        {
                                            MaxLoSOnArrival = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MinLoSThrough")
                                        {
                                            MinLoSThrough = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "MaxLoSThrough")
                                        {
                                            MaxLoSThrough = Convert.ToInt16(xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText);
                                        }
                                        if (xmlnode[r].ChildNodes[c].ChildNodes[k].Name == "Description")
                                        {
                                            Description = xmlnode[r].ChildNodes[c].ChildNodes[k].InnerText;
                                        }
                                    }

                                }

                            }
                            //----------------------------------------------------------
                            SqlParameter[] MyParam = new SqlParameter[29];
                            MyParam[0] = new SqlParameter("@HotelCode", HotelCode);
                            MyParam[1] = new SqlParameter("@UpdateType", UpdateType);
                            MyParam[2] = new SqlParameter("@CurrencyCode", Currency);
                            MyParam[3] = new SqlParameter("@iRoomId", InvTypeCode);
                            MyParam[4] = new SqlParameter("@iRPId", RatePlanCode);
                            MyParam[5] = new SqlParameter("@Start", sStartDate);
                            MyParam[6] = new SqlParameter("@End", sEndDate);
                            MyParam[7] = new SqlParameter("@Mon", Mon);
                            MyParam[8] = new SqlParameter("@Tue", Tue);
                            MyParam[9] = new SqlParameter("@Wed", Wed);
                            MyParam[10] = new SqlParameter("@Thu", Thu);
                            MyParam[11] = new SqlParameter("@Fri", Fri);
                            MyParam[12] = new SqlParameter("@Sat", Sat);
                            MyParam[13] = new SqlParameter("@Sun", Sun);
                            MyParam[14] = new SqlParameter("@AdditionalGuestAmount", AdditionalGuestAmount);
                            MyParam[15] = new SqlParameter("@OccAmount", OccAmount);
                            MyParam[16] = new SqlParameter("@MealCodes", MealCodes);
                            MyParam[17] = new SqlParameter("@Master", Master);
                            MyParam[18] = new SqlParameter("@Arrival", Arrival);
                            MyParam[19] = new SqlParameter("@Departure", Departure);
                            MyParam[20] = new SqlParameter("@MinAdvancedBookingOffset", MinAdvancedBookingOffset);
                            MyParam[21] = new SqlParameter("@MaxAdvancedBookingOffset", MaxAdvancedBookingOffset);
                            MyParam[22] = new SqlParameter("@MinLoSOnArrival", MinLoSOnArrival);
                            MyParam[23] = new SqlParameter("@MaxLoSOnArrival", MaxLoSOnArrival);
                            MyParam[24] = new SqlParameter("@MinLoSOnArrivalThrough", MinLoSThrough);
                            MyParam[25] = new SqlParameter("@MaxLoSOnArrivalThrough", MaxLoSThrough);
                            MyParam[26] = new SqlParameter("@Description", Description);
                            MyParam[27] = new SqlParameter("@ReleaseDays", ReleaseDays);
                            MyParam[28] = new SqlParameter("@Allotment", Allotment);

                            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateHotelARIForChannelMgr", MyParam);

                            OccAmount.Clear();
                            // OccAmount = new DataTable();
                            MealCodes.Clear();
                            // MealCodes = new DataTable();

                            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                                {
                                    status = false;
                                    sb.Append("<Errors> <Error Type=\"3\" Code=\"" + ds.Tables[0].Rows[0][1].ToString() + "\" >" + ds.Tables[0].Rows[0][2].ToString() + " </Error> </Errors>");
                                    sb.Append("</HotelARIUpdateRS>");
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
                                sb.Append("<Errors> <Error Type=\"3\" Code=\"392\" > Invalid hotel code. </Error> </Errors>");
                                sb.Append("</HotelARIUpdateRS>");
                                break;
                            }

                        }

                        /*
                               xmlnode = xmlDoc.GetElementsByTagName("RateAmounts");

                               // --------------OCCUPANCY WISE RATES & INCLUSIONS
                               for (int i = 0; i <= xmlnode.Count - 1; i++)
                               {
                                   for (int k = 0; k <= xmlnode[i].ChildNodes.Count - 1; k++)
                                   {
                                       if (xmlnode[i].ChildNodes[k].Name == "Base")
                                       {
                                           if (xmlnode[i].ChildNodes[k].Attributes["OccupancyCode"].Value == "AA" || xmlnode[i].ChildNodes[k].Attributes["OccupancyCode"].Value == "AC" || xmlnode[i].ChildNodes[k].Attributes["OccupancyCode"].Value == "EB" || xmlnode[i].ChildNodes[k].Attributes["OccupancyCode"].Value == "BC")
                                           {
                                               if (xmlnode[i].ChildNodes[k].Attributes["OccupancyCode"].Value == "AA")
                                               {
                                                   AdditionalGuestAmount = Convert.ToDecimal(xmlnode[i].ChildNodes[k].Attributes["Amount"].Value);
                                               }
                                               if (AdditionalGuestAmount < 0)
                                               {
                                                   AdditionalGuestAmount = Convert.ToDecimal(xmlnode[i].ChildNodes[k].Attributes["Amount"].Value);
                                               }
                                           }
                                           else
                                           {
                                               NoofGuests = objUtil.ConvertOccupancyCode(xmlnode[i].ChildNodes[k].Attributes["OccupancyCode"].Value);
                                               AmountAfterTax = Convert.ToDecimal(xmlnode[i].ChildNodes[k].Attributes["Amount"].Value);

                                               drOccAmount = OccAmount.NewRow();
                                               drOccAmount["RatePlan"] = NoofGuests;
                                               drOccAmount["dPrice"] = AmountAfterTax;
                                               drOccAmount["bIsPromo"] = false;
                                               OccAmount.Rows.Add(drOccAmount);
                                           }
                                       }
                                       if (xmlnode[i].ChildNodes[k].Name == "MealPlans" && xmlnode[i].ChildNodes[k].HasChildNodes)
                                       {
                                           for (int l = 0; l <= xmlnode[i].ChildNodes[k].ChildNodes.Count - 1; l++)
                                           {
                                               if (xmlnode[i].ChildNodes[k].ChildNodes[l].Attributes["Included"] != null && xmlnode[i].ChildNodes[k].ChildNodes[l].Attributes["Included"].Value == "true")
                                               {
                                                   drMeals = MealCodes.NewRow();
                                                   MealCode = Convert.ToInt16(xmlnode[i].ChildNodes[k].ChildNodes[l].Attributes["MealPlanCode"].Value);
                                                   drMeals["Id"] = MealCode;
                                                   MealCodes.Rows.Add(drMeals);
                                               }
                                           }
                                       }
                                   }
                               }
                         * */

                        if (!status)
                        {
                            //sb.Append("<Errors> <Error Type=\"3\" Code=\"392\" > Invalid hotel code. </Error> </Errors>");
                            //sb.Append("</HotelARIUpdateRS>");
                        }
                        else
                        {
                            sb.Append("<Success/>");
                            sb.Append("</HotelARIUpdateRS>");
                        }

                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/xml")

                        };
                    }
                    else
                    {
                        objUtil.UpdateChannelMgrTracker(iID, 1, "");
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("<HotelARIUpdateRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials. Uid:" + UserName + " Pwd:" + Password + "</Error> </Errors> </HotelARIUpdateRS>", Encoding.UTF8, "application/xml")

                        };
                    }
                }
                catch (Exception ex)
                {
                    objUtil.UpdateChannelMgrTracker(iID, 1, "");
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("<HotelARIUpdateRS xmlns=\"http://cgbridge.rategain.com/OTA/2012/05\" TimeStamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "\" Target=\"Production\" Version=\"1.1\"> <Errors> <Error Type=\"4\" Code=\"450\" > Invalid Authentication Credentials." + ex.ToString() + " </Error> </Errors> </HotelARIUpdateRS>", Encoding.UTF8, "application/xml")

                    };
                }
            }
            #endregion
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<HotelPropertyListGetRS > <Errors> <Error Type=\"3\" Code=\"424\" > Unknown chain code. </Error> </Errors> </HotelPropertyListGetRS>");
                objUtil.UpdateChannelMgrTracker(iID, 1, "");
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