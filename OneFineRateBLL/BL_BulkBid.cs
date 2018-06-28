using OneFineRate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace OneFineRateBLL
{
    public class BL_BulkBid
    {
        public static string SaveLead(int iUserId, int PropId, string dates, string CloseOut, string CTA, string CTD, string Lead, string typ)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    List<RangeTable> RangeTable = JsonConvert.DeserializeObject<List<RangeTable>>(Lead);


                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iDays", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("dDiscount", typeof(Decimal));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId1", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId1", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId2", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId2", typeof(Int16));
                    BidRange.Columns.Add(col);

                    if (RangeTable.Count > 0)
                    {
                        foreach (RangeTable RT in RangeTable)
                        {
                            for (int i = RT.from; i <= RT.to; i++)
                            {
                                foreach (string date in Alldates)
                                {
                                    try
                                    {
                                        DataRow drBidRange = BidRange.NewRow();
                                        drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                        drBidRange["iDays"] = i;
                                        drBidRange["dDiscount"] = RT.discount;
                                        drBidRange["iAmenityId1"] = RT.Amenity1 == 0 ? (object)DBNull.Value : RT.Amenity1;
                                        drBidRange["iApplicabilityId1"] = RT.Applicability1 == 0 ? (object)DBNull.Value : RT.Applicability1;
                                        drBidRange["iAmenityId2"] = RT.Amenity2 == 0 ? (object)DBNull.Value : RT.Amenity2;
                                        drBidRange["iApplicabilityId2"] = RT.Applicability2 == 0 ? (object)DBNull.Value : RT.Applicability2;
                                        BidRange.Rows.Add(drBidRange);
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string date in Alldates)
                        {
                            try
                            {
                                DataRow drBidRange = BidRange.NewRow();
                                drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                drBidRange["iDays"] = (object)DBNull.Value;
                                drBidRange["dDiscount"] = (object)DBNull.Value;
                                drBidRange["iAmenityId1"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId1"] = (object)DBNull.Value;
                                drBidRange["iAmenityId2"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId2"] = (object)DBNull.Value;
                                BidRange.Rows.Add(drBidRange);
                            }
                            catch (Exception) { }
                        }
                    }
                    
                    if (Convert.ToInt32(typ) == 0)
                    {
                        // Update data for Corporate first
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", 2); 
                        db.Database.ExecuteSqlCommand("uspSaveLeadTimeBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);

                        // Make changes and update data for public
                        SqlParameter[] MyParam1 = new SqlParameter[7];
                        MyParam1[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam1[0].TypeName = "[dbo].[BidRange]";
                        MyParam1[1] = new SqlParameter("@iPropId", PropId);
                        MyParam1[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam1[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam1[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam1[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam1[6] = new SqlParameter("@typ", 1); 
                        db.Database.ExecuteSqlCommand("uspSaveLeadTimeBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam1);
                    }
                    else
                    {
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", Convert.ToInt32(typ));
                        db.Database.ExecuteSqlCommand("uspSaveLeadTimeBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);
                    }
                    return "a";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static string SaveLOS(int iUserId, int PropId, string dates, string CloseOut, string CTA, string CTD, string LOS, string typ)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    List<RangeTable> RangeTable = JsonConvert.DeserializeObject<List<RangeTable>>(LOS);


                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iDays", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("dDiscount", typeof(Decimal));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId1", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId1", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId2", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId2", typeof(Int16));
                    BidRange.Columns.Add(col);

                    if (RangeTable.Count > 0)
                    {
                        foreach (RangeTable RT in RangeTable)
                        {
                            for (int i = RT.from; i <= RT.to; i++)
                            {
                                foreach (string date in Alldates)
                                {
                                    try
                                    {
                                        DataRow drBidRange = BidRange.NewRow();
                                        drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                        drBidRange["iDays"] = i;
                                        drBidRange["dDiscount"] = RT.discount;
                                        drBidRange["iAmenityId1"] = RT.Amenity1 == 0 ? (object)DBNull.Value : RT.Amenity1;
                                        drBidRange["iApplicabilityId1"] = RT.Applicability1 == 0 ? (object)DBNull.Value : RT.Applicability1;
                                        drBidRange["iAmenityId2"] = RT.Amenity2 == 0 ? (object)DBNull.Value : RT.Amenity2;
                                        drBidRange["iApplicabilityId2"] = RT.Applicability2 == 0 ? (object)DBNull.Value : RT.Applicability2;
                                        BidRange.Rows.Add(drBidRange);
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string date in Alldates)
                        {
                            try
                            {
                                DataRow drBidRange = BidRange.NewRow();
                                drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                drBidRange["iDays"] = (object)DBNull.Value;
                                drBidRange["dDiscount"] = (object)DBNull.Value;
                                drBidRange["iAmenityId1"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId1"] = (object)DBNull.Value;
                                drBidRange["iAmenityId2"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId2"] = (object)DBNull.Value;
                                BidRange.Rows.Add(drBidRange);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (Convert.ToInt32(typ) == 0)
                    {
                        // Update data for Corporate first
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", 2);
                        db.Database.ExecuteSqlCommand("uspSaveLOSBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);

                        // Make changes and update data for public
                        SqlParameter[] MyParam1 = new SqlParameter[7];
                        MyParam1[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam1[0].TypeName = "[dbo].[BidRange]";
                        MyParam1[1] = new SqlParameter("@iPropId", PropId);
                        MyParam1[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam1[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam1[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam1[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam1[6] = new SqlParameter("@typ", 1);
                        db.Database.ExecuteSqlCommand("uspSaveLOSBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam1);
                    }
                    else
                    {
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", typ);
                        db.Database.ExecuteSqlCommand("uspSaveLOSBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);
                    }
                    return "a";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static string SaveRoom(int iUserId, int PropId, string dates, string CloseOut, string CTA, string CTD, string Room, string typ)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    List<RangeTable> RangeTable = JsonConvert.DeserializeObject<List<RangeTable>>(Room);


                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iDays", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("dDiscount", typeof(Decimal));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId1", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId1", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId2", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId2", typeof(Int16));
                    BidRange.Columns.Add(col);

                    if (RangeTable.Count > 0)
                    {
                        foreach (RangeTable RT in RangeTable)
                        {
                            for (int i = RT.from; i <= RT.to; i++)
                            {
                                foreach (string date in Alldates)
                                {
                                    try
                                    {
                                        DataRow drBidRange = BidRange.NewRow();
                                        drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                        drBidRange["iDays"] = i;
                                        drBidRange["dDiscount"] = RT.discount;
                                        drBidRange["iAmenityId1"] = RT.Amenity1 == 0 ? (object)DBNull.Value : RT.Amenity1;
                                        drBidRange["iApplicabilityId1"] = RT.Applicability1 == 0 ? (object)DBNull.Value : RT.Applicability1;
                                        drBidRange["iAmenityId2"] = RT.Amenity2 == 0 ? (object)DBNull.Value : RT.Amenity2;
                                        drBidRange["iApplicabilityId2"] = RT.Applicability2 == 0 ? (object)DBNull.Value : RT.Applicability2;
                                        BidRange.Rows.Add(drBidRange);
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string date in Alldates)
                        {
                            try
                            {
                                DataRow drBidRange = BidRange.NewRow();
                                drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                drBidRange["iDays"] = (object)DBNull.Value;
                                drBidRange["dDiscount"] = (object)DBNull.Value;
                                drBidRange["iAmenityId1"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId1"] = (object)DBNull.Value;
                                drBidRange["iAmenityId2"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId2"] = (object)DBNull.Value;
                                BidRange.Rows.Add(drBidRange);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (Convert.ToInt32(typ) == 0)
                    {
                        // Update data for Corporate first
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", 2);
                        db.Database.ExecuteSqlCommand("uspSaveRoomsBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);

                        // Make changes and update data for public
                        SqlParameter[] MyParam1 = new SqlParameter[7];
                        MyParam1[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam1[0].TypeName = "[dbo].[BidRange]";
                        MyParam1[1] = new SqlParameter("@iPropId", PropId);
                        MyParam1[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam1[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam1[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam1[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam1[6] = new SqlParameter("@typ", 1);
                        db.Database.ExecuteSqlCommand("uspSaveRoomsBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam1);
                    }
                    else
                    {
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", typ);
                        db.Database.ExecuteSqlCommand("uspSaveRoomsBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);
                    }
                    return "a";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="PropId"></param>
        /// <param name="dates"></param>
        /// <param name="CloseOut"></param>
        /// <param name="CTA"></param>
        /// <param name="CTD"></param>
        /// <param name="Days"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public static string SaveWeekend(int iUserId, int PropId, string dates, string CloseOut, string CTA, string CTD, string Days, string typ)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    List<DayTable> DayTable = JsonConvert.DeserializeObject<List<DayTable>>(Days);


                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iDays", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("dDiscount", typeof(Decimal));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId1", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId1", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId2", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId2", typeof(Int16));
                    BidRange.Columns.Add(col);

                    if (DayTable.Count > 0)
                    {
                        foreach (string date in Alldates)
                        {
                            bool DayExists = false;
                            foreach (DayTable DT in DayTable)
                            {
                                try
                                {
                                    DateTime GivenDate = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                    if (GivenDate.DayOfWeek.ToString() == DT.id)
                                    {
                                        DataRow drBidRange = BidRange.NewRow();
                                        drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                        drBidRange["iDays"] = DT.Weekend;
                                        drBidRange["dDiscount"] = DT.disc;
                                        drBidRange["iAmenityId1"] = DT.amen1 == 0 ? (object)DBNull.Value : DT.amen1;
                                        drBidRange["iApplicabilityId1"] = DT.app1 == 0 ? (object)DBNull.Value : DT.app1;
                                        drBidRange["iAmenityId2"] = DT.amen2 == 0 ? (object)DBNull.Value : DT.amen2;
                                        drBidRange["iApplicabilityId2"] = DT.app2 == 0 ? (object)DBNull.Value : DT.app2;
                                        BidRange.Rows.Add(drBidRange);
                                        DayExists = true;
                                        continue;
                                    }
                                }
                                catch (Exception) { }
                            }
                            if (!DayExists && (CloseOut != "" || CTA != "" || CTD != ""))
                            {
                                DataRow drBidRange = BidRange.NewRow();
                                drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                drBidRange["iDays"] = (object)DBNull.Value;
                                drBidRange["dDiscount"] = (object)DBNull.Value;
                                drBidRange["iAmenityId1"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId1"] = (object)DBNull.Value;
                                drBidRange["iAmenityId2"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId2"] = (object)DBNull.Value;
                                BidRange.Rows.Add(drBidRange);
                            }
                        }
                    }
                    else
                    {
                        foreach (string date in Alldates)
                        {
                            try
                            {
                                DataRow drBidRange = BidRange.NewRow();
                                drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                drBidRange["iDays"] = (object)DBNull.Value;
                                drBidRange["dDiscount"] = (object)DBNull.Value;
                                drBidRange["iAmenityId1"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId1"] = (object)DBNull.Value;
                                drBidRange["iAmenityId2"] = (object)DBNull.Value;
                                drBidRange["iApplicabilityId2"] = (object)DBNull.Value;
                                BidRange.Rows.Add(drBidRange);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (Convert.ToInt32(typ) == 0)
                    {
                        // Update data for Corporate first
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", 2);
                        db.Database.ExecuteSqlCommand("uspSaveWeekenBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);

                        // Make changes and update data for public
                        SqlParameter[] MyParam1 = new SqlParameter[7];
                        MyParam1[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam1[0].TypeName = "[dbo].[BidRange]";
                        MyParam1[1] = new SqlParameter("@iPropId", PropId);
                        MyParam1[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam1[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam1[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam1[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam1[6] = new SqlParameter("@typ", 1);
                        db.Database.ExecuteSqlCommand("uspSaveWeekenBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam1);
                    }
                    else
                    {
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidRange", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidRange]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", typ);
                        db.Database.ExecuteSqlCommand("uspSaveWeekenBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);
                    }
                    return "a";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static string SaveBasic(int iUserId, int PropId, string dates, string CloseOut, string CTA, string CTD, string Basic, string typ)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);

                    foreach (string date in Alldates)
                    {
                        try
                        {
                            DataRow drBidRange = BidRange.NewRow();
                            drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                            BidRange.Rows.Add(drBidRange);
                        }
                        catch (Exception) { }
                    }
                    if (Convert.ToInt32(typ) == 0)
                    {
                        // Update data for Corporate first
                        SqlParameter[] MyParam = new SqlParameter[8];
                        MyParam[0] = new SqlParameter("@BidDates", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidDates]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@dDiscount", Basic == "NaN" || Basic == "" ? (object)DBNull.Value : Basic);
                        MyParam[7] = new SqlParameter("@typ", 2);
                        db.Database.ExecuteSqlCommand("uspSaveBasicDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @dDiscount, @typ", MyParam);

                        // Make changes and update data for public
                        SqlParameter[] MyParam1 = new SqlParameter[8];
                        MyParam1[0] = new SqlParameter("@BidDates", BidRange);
                        MyParam1[0].TypeName = "[dbo].[BidDates]";
                        MyParam1[1] = new SqlParameter("@iPropId", PropId);
                        MyParam1[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam1[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam1[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam1[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam1[6] = new SqlParameter("@dDiscount", Basic == "NaN" || Basic == "" ? (object)DBNull.Value : Basic);
                        MyParam1[7] = new SqlParameter("@typ", 1);
                        db.Database.ExecuteSqlCommand("uspSaveBasicDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @dDiscount, @typ", MyParam1);
                    }
                    else
                    {
                        SqlParameter[] MyParam = new SqlParameter[8];
                        MyParam[0] = new SqlParameter("@BidDates", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidDates]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@dDiscount", Basic == "NaN" || Basic == "" ? (object)DBNull.Value : Basic);
                        MyParam[7] = new SqlParameter("@typ", typ);
                        db.Database.ExecuteSqlCommand("uspSaveBasicDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @dDiscount, @typ", MyParam);
                    }
                    return "a";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static string SaveCTACTDClosedForAllDisc(int iUserId, int PropId, string dates, string CloseOut, string CTA, string CTD, string typ)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);

                    foreach (string date in Alldates)
                    {
                        try
                        {
                            DataRow drBidRange = BidRange.NewRow();
                            drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                            BidRange.Rows.Add(drBidRange);
                        }
                        catch (Exception) { }
                    }
                    if (Convert.ToInt32(typ) == 0)
                    {
                        // Update data for Corporate first
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidDates", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidDates]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", 2);
                        db.Database.ExecuteSqlCommand("uspSaveCTACTDClosedForAllDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);

                        // Make changes and update data for public
                        SqlParameter[] MyParam1 = new SqlParameter[7];
                        MyParam1[0] = new SqlParameter("@BidDates", BidRange);
                        MyParam1[0].TypeName = "[dbo].[BidDates]";
                        MyParam1[1] = new SqlParameter("@iPropId", PropId);
                        MyParam1[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam1[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam1[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam1[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam1[6] = new SqlParameter("@typ", 1);
                        db.Database.ExecuteSqlCommand("uspSaveCTACTDClosedForAllDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam1);
                    }
                    else
                    {
                        SqlParameter[] MyParam = new SqlParameter[7];
                        MyParam[0] = new SqlParameter("@BidDates", BidRange);
                        MyParam[0].TypeName = "[dbo].[BidDates]";
                        MyParam[1] = new SqlParameter("@iPropId", PropId);
                        MyParam[2] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                        MyParam[3] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                        MyParam[4] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                        MyParam[5] = new SqlParameter("@iActionBy", iUserId);
                        MyParam[6] = new SqlParameter("@typ", typ);
                        db.Database.ExecuteSqlCommand("uspSaveCTACTDClosedForAllDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);
                    }
                    return "a";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static string CheckExistingBidDates(int PropId, string dates)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    string[] Alldates = dates.Split('$');

                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);

                    foreach (string date in Alldates)
                    {
                        try
                        {
                            DataRow drBidRange = BidRange.NewRow();
                            drBidRange["dtEffectiveDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                            BidRange.Rows.Add(drBidRange);
                        }
                        catch (Exception) { }
                    }
                    List<MissingDates> ID = new List<MissingDates>();
                    SqlParameter[] MyParam = new SqlParameter[2];
                    MyParam[0] = new SqlParameter("@BidDates", BidRange);
                    MyParam[0].TypeName = "[dbo].[BidDates]";
                    MyParam[1] = new SqlParameter("@iPropId", PropId);
                    ID = db.Database.SqlQuery<MissingDates>("uspCheckExistingBidDates @BidDates, @iPropId", MyParam).ToList();
                    if (ID.Count > 0)
                    {
                        var js = JsonConvert.SerializeObject(ID);
                        return js.ToString();
                    }
                    else
                    {
                        return "a";
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }



    public class RangeTable
    {
        public int from { get; set; }
        public int to { get; set; }
        public double discount { get; set; }
        public int Amenity1 { get; set; }
        public string AmenText1 { get; set; }
        public int Applicability1 { get; set; }
        public string ApplicabilityText1 { get; set; }
        public int Amenity2 { get; set; }
        public string AmenText2 { get; set; }
        public int Applicability2 { get; set; }
        public string ApplicabilityText2 { get; set; }
    }

    public class DayTable
    {
        public string id { get; set; }
        public double disc { get; set; }
        public int amen1 { get; set; }
        public int app1 { get; set; }
        public int amen2 { get; set; }
        public int app2 { get; set; }
        public int Weekend { get; set; }
    }

    public class MissingDates
    {
        public string Dates { get; set; }
    }
}