using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblPropertyM
    {

        #region Graph Data

        public static HotelRank GetHotelRank(int iPropId)
        {
            try
            {
                List<HotelRank> obj = new List<HotelRank>();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {

                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@PropId", iPropId);
                    obj = db.Database.SqlQuery<HotelRank>("uspGetRankForDashBoard @PropId", MyParam).ToList();
                    if (obj.Count > 0)
                    {
                        return obj[0];
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static eBookingOverviewGraphModel GetBookingOverview_GraphData(int propId, int numberOfDays)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParams = new SqlParameter[2];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iPropId", propId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@Days", numberOfDays);

                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspGetBookingOverviewDashBoard", MyParams);
                eBookingOverviewGraphModel model = new eBookingOverviewGraphModel();

                if (ds.Tables.Count > 0)
                {
                    #region ChartDataProcessing

                    if (ds.Tables[0] != null)
                    {
                        model.TotalCompetitors = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalComp"].ToString());
                    };

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        model.TotalViews_Ours.Bids = Convert.ToInt32(ds.Tables[1].Rows[0]["Bids"].ToString());
                        model.TotalViews_Ours.Bookings = Convert.ToInt32(ds.Tables[1].Rows[0]["Bookings"].ToString());
                        model.TotalViews_Ours.Negotiations = Convert.ToInt32(ds.Tables[1].Rows[0]["Negotiations"].ToString());
                        model.TotalViews_Ours.Corporate = Convert.ToInt32(ds.Tables[1].Rows[0]["Corporate"].ToString());
                    };

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        model.TotalViews_Competitors.Bids = Convert.ToInt32(ds.Tables[2].Rows[0]["Bids"].ToString());
                        model.TotalViews_Competitors.Bookings = Convert.ToInt32(ds.Tables[2].Rows[0]["Bookings"].ToString());
                        model.TotalViews_Competitors.Negotiations = Convert.ToInt32(ds.Tables[2].Rows[0]["Negotiations"].ToString());
                        model.TotalViews_Competitors.Corporate = Convert.ToInt32(ds.Tables[2].Rows[0]["Corporate"].ToString());
                    };

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        model.TotalBookings_Ours.Bids = Convert.ToInt32(ds.Tables[3].Rows[0]["Bids"].ToString());
                        model.TotalBookings_Ours.Bookings = Convert.ToInt32(ds.Tables[3].Rows[0]["Bookings"].ToString());
                        model.TotalBookings_Ours.Negotiations = Convert.ToInt32(ds.Tables[3].Rows[0]["Negotiations"].ToString());
                        model.TotalBookings_Ours.Corporate = Convert.ToInt32(ds.Tables[3].Rows[0]["Corporate"].ToString());
                    };

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        model.TotalBookings_Competitors.Bids = Convert.ToInt32(ds.Tables[4].Rows[0]["Bids"].ToString());
                        model.TotalBookings_Competitors.Bookings = Convert.ToInt32(ds.Tables[4].Rows[0]["Bookings"].ToString());
                        model.TotalBookings_Competitors.Negotiations = Convert.ToInt32(ds.Tables[4].Rows[0]["Negotiations"].ToString());
                        model.TotalBookings_Competitors.Corporate = Convert.ToInt32(ds.Tables[4].Rows[0]["Corporate"].ToString());
                    };

                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        model.TotalConversions_Ours.Bids = Convert.ToInt32(ds.Tables[5].Rows[0]["Bids"].ToString());
                        model.TotalConversions_Ours.Bookings = Convert.ToInt32(ds.Tables[5].Rows[0]["Bookings"].ToString());
                        model.TotalConversions_Ours.Negotiations = Convert.ToInt32(ds.Tables[5].Rows[0]["Negotiations"].ToString());
                        model.TotalBookings_Competitors.Corporate = Convert.ToInt32(ds.Tables[5].Rows[0]["Corporate"].ToString());
                    };

                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        model.TotalConversions_Competitors.Bids = Convert.ToInt32(ds.Tables[6].Rows[0]["Bids"].ToString());
                        model.TotalConversions_Competitors.Bookings = Convert.ToInt32(ds.Tables[6].Rows[0]["Bookings"].ToString());
                        model.TotalConversions_Competitors.Negotiations = Convert.ToInt32(ds.Tables[6].Rows[0]["Negotiations"].ToString());
                        model.TotalBookings_Competitors.Corporate = Convert.ToInt32(ds.Tables[6].Rows[0]["Corporate"].ToString());
                    };

                    #endregion
                }

                return model;
            }
        }

        public static eNegotiationOverviewGraphModel GetNegotiationOverview_GraphData(int propId, int numberOfDays)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                eNegotiationOverviewGraphModel model = new eNegotiationOverviewGraphModel();

                SqlParameter[] MyParams = new SqlParameter[2];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iPropId", propId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@Days", numberOfDays);

                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspGetNegotiationInsightsDashBoard", MyParams);

                if (ds.Tables.Count > 0)
                {
                    #region ChartDataProcessing

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        model.TotalNegotiation_Ours.Accepted = Convert.ToInt32(ds.Tables[0].Rows[0]["Accepted"].ToString());
                        model.TotalNegotiation_Ours.CounterOffer = Convert.ToInt32(ds.Tables[0].Rows[0]["CounterOffer"].ToString());
                        model.TotalNegotiation_Ours.Rejected = Convert.ToInt32(ds.Tables[0].Rows[0]["Rejected"].ToString());
                    };

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        model.TotalNegotiation_Competitors.Accepted = Convert.ToInt32(ds.Tables[1].Rows[0]["Accepted"].ToString());
                        model.TotalNegotiation_Competitors.CounterOffer = Convert.ToInt32(ds.Tables[1].Rows[0]["CounterOffer"].ToString());
                        model.TotalNegotiation_Competitors.Rejected = Convert.ToInt32(ds.Tables[1].Rows[0]["Rejected"].ToString());
                    };

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        model.TotalAccepted_Ours.Accepted = Convert.ToInt32(ds.Tables[2].Rows[0]["Accepted"].ToString());
                        model.TotalAccepted_Ours.NoAction = Convert.ToInt32(ds.Tables[2].Rows[0]["NoAction"].ToString());
                        model.TotalAccepted_Ours.Rejected = Convert.ToInt32(ds.Tables[2].Rows[0]["Rejected"].ToString());
                    };

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        model.TotalAccepted_Competitors.Accepted = Convert.ToInt32(ds.Tables[3].Rows[0]["Accepted"].ToString());
                        model.TotalAccepted_Competitors.NoAction = Convert.ToInt32(ds.Tables[3].Rows[0]["NoAction"].ToString());
                        model.TotalAccepted_Competitors.Rejected = Convert.ToInt32(ds.Tables[3].Rows[0]["Rejected"].ToString());
                    };

                    #endregion
                }

                return model;
            }
        }

        public static ePerformanceOverviewGraphModel GetPerformanceOverview_GraphData(int propId, int numberOfDays)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParams = new SqlParameter[2];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@Days", numberOfDays);

                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspPerformanceOverviewDashBoard", MyParams);
                ePerformanceOverviewGraphModel model = new ePerformanceOverviewGraphModel();

                if (ds.Tables.Count > 0)
                {
                    #region ChartDataProcessing

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            var performanceModel = new PerformaceGraphData();

                            performanceModel.YearType = ds.Tables[0].Rows[i]["sType"].ToString();
                            performanceModel.Bids = int.Parse(ds.Tables[0].Rows[i]["Bid"].ToString());
                            performanceModel.Negotiations = int.Parse(ds.Tables[0].Rows[i]["Negotiate"].ToString());
                            performanceModel.Buy = int.Parse(ds.Tables[0].Rows[i]["Buy"].ToString());
                            performanceModel.Corporate = int.Parse(ds.Tables[0].Rows[i]["Corporate"].ToString());
                            performanceModel.OverAll = int.Parse(ds.Tables[0].Rows[i]["OverAll"].ToString());

                            model.RoomNights.Add(performanceModel);
                        }
                    };

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            var performanceModel = new PerformaceGraphData();

                            performanceModel.YearType = ds.Tables[1].Rows[i]["sType"].ToString();
                            performanceModel.Bids = int.Parse(ds.Tables[1].Rows[i]["Bid"].ToString());
                            performanceModel.Negotiations = int.Parse(ds.Tables[1].Rows[i]["Negotiate"].ToString());
                            performanceModel.Buy = int.Parse(ds.Tables[1].Rows[i]["Buy"].ToString());
                            performanceModel.Corporate = int.Parse(ds.Tables[1].Rows[i]["Corporate"].ToString());
                            performanceModel.OverAll = int.Parse(ds.Tables[1].Rows[i]["OverAll"].ToString());

                            model.Revenue.Add(performanceModel);
                        }
                    };

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            var performanceModel = new PerformaceGraphData();

                            performanceModel.YearType = ds.Tables[2].Rows[i]["sType"].ToString();
                            performanceModel.Bids = int.Parse(ds.Tables[2].Rows[i]["Bid"].ToString());
                            performanceModel.Negotiations = int.Parse(ds.Tables[2].Rows[i]["Negotiate"].ToString());
                            performanceModel.Buy = int.Parse(ds.Tables[2].Rows[i]["Buy"].ToString());
                            performanceModel.Corporate = int.Parse(ds.Tables[2].Rows[i]["Corporate"].ToString());
                            performanceModel.OverAll = int.Parse(ds.Tables[2].Rows[i]["OverAll"].ToString());

                            model.AvgDailyRate.Add(performanceModel);
                        }
                    };

                    #endregion
                }

                return model;
            }
        }

        public static eBookingInsightsGraphModel GetBookingInsights_GraphData(int propId, int numberOfDays, string cType, string bookingOrRevenue)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                eBookingInsightsGraphModel model = new eBookingInsightsGraphModel();

                SqlParameter[] MyParams = new SqlParameter[4];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@Days", numberOfDays);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@BookingType", cType);
                MyParams[3] = new System.Data.SqlClient.SqlParameter("@NightsOrRevenue", bookingOrRevenue);

                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspBookingInsightsRoomsProductivityDashBoard", MyParams);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            var bookingInsightsModel = new BookingInsightsGraphData();

                            bookingInsightsModel.DataFor = ds.Tables[0].Rows[i]["DataFor"].ToString();

                            if (ds.Tables[0].Rows[i]["cType"].ToString() == "O")//(i < 4)
                            {
                                bookingInsightsModel.Y0_Overall = int.Parse(ds.Tables[0].Rows[i]["0-1"].ToString());
                                bookingInsightsModel.Y1_Overall = int.Parse(ds.Tables[0].Rows[i]["2-3"].ToString());
                                bookingInsightsModel.Y2_Overall = int.Parse(ds.Tables[0].Rows[i]["4-7"].ToString());
                                bookingInsightsModel.Y3_Overall = int.Parse(ds.Tables[0].Rows[i]["8-15"].ToString());
                                bookingInsightsModel.Y4_Overall = int.Parse(ds.Tables[0].Rows[i]["16-30"].ToString());
                                bookingInsightsModel.Y5_Overall = int.Parse(ds.Tables[0].Rows[i]["31-60"].ToString());
                                bookingInsightsModel.Y6_Overall = int.Parse(ds.Tables[0].Rows[i]["61-90"].ToString());
                                bookingInsightsModel.Y7_Overall = int.Parse(ds.Tables[0].Rows[i]["90+"].ToString());

                            }
                            else
                            {
                                //if (cType != "O")
                                //{
                                bookingInsightsModel.Y0 = int.Parse(ds.Tables[0].Rows[i]["0-1"].ToString());
                                bookingInsightsModel.Y1 = int.Parse(ds.Tables[0].Rows[i]["2-3"].ToString());
                                bookingInsightsModel.Y2 = int.Parse(ds.Tables[0].Rows[i]["4-7"].ToString());
                                bookingInsightsModel.Y3 = int.Parse(ds.Tables[0].Rows[i]["8-15"].ToString());
                                bookingInsightsModel.Y4 = int.Parse(ds.Tables[0].Rows[i]["16-30"].ToString());
                                bookingInsightsModel.Y5 = int.Parse(ds.Tables[0].Rows[i]["31-60"].ToString());
                                bookingInsightsModel.Y6 = int.Parse(ds.Tables[0].Rows[i]["61-90"].ToString());
                                bookingInsightsModel.Y7 = int.Parse(ds.Tables[0].Rows[i]["90+"].ToString());
                                //}
                            }
                            model.LeadTime.Add(bookingInsightsModel);
                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            var bookingInsightsModel = new BookingInsightsGraphData();

                            bookingInsightsModel.DataFor = ds.Tables[1].Rows[i]["DataFor"].ToString();
                            if (ds.Tables[1].Rows[i]["cType"].ToString() == "O")//(i < 4)
                            {
                                bookingInsightsModel.Y0_Overall = int.Parse(ds.Tables[1].Rows[i]["1"].ToString());
                                bookingInsightsModel.Y1_Overall = int.Parse(ds.Tables[1].Rows[i]["2"].ToString());
                                bookingInsightsModel.Y2_Overall = int.Parse(ds.Tables[1].Rows[i]["3"].ToString());
                                bookingInsightsModel.Y3_Overall = int.Parse(ds.Tables[1].Rows[i]["4"].ToString());
                                bookingInsightsModel.Y4_Overall = int.Parse(ds.Tables[1].Rows[i]["5"].ToString());
                                bookingInsightsModel.Y5_Overall = int.Parse(ds.Tables[1].Rows[i]["6"].ToString());
                                bookingInsightsModel.Y6_Overall = int.Parse(ds.Tables[1].Rows[i]["7"].ToString());
                                bookingInsightsModel.Y7_Overall = int.Parse(ds.Tables[1].Rows[i]["8"].ToString());
                                bookingInsightsModel.Y8_Overall = int.Parse(ds.Tables[1].Rows[i]["9"].ToString());
                                bookingInsightsModel.Y9_Overall = int.Parse(ds.Tables[1].Rows[i]["10+"].ToString());
                            }
                            else
                            {
                                bookingInsightsModel.Y0 = int.Parse(ds.Tables[1].Rows[i]["1"].ToString());
                                bookingInsightsModel.Y1 = int.Parse(ds.Tables[1].Rows[i]["2"].ToString());
                                bookingInsightsModel.Y2 = int.Parse(ds.Tables[1].Rows[i]["3"].ToString());
                                bookingInsightsModel.Y3 = int.Parse(ds.Tables[1].Rows[i]["4"].ToString());
                                bookingInsightsModel.Y4 = int.Parse(ds.Tables[1].Rows[i]["5"].ToString());
                                bookingInsightsModel.Y5 = int.Parse(ds.Tables[1].Rows[i]["6"].ToString());
                                bookingInsightsModel.Y6 = int.Parse(ds.Tables[1].Rows[i]["7"].ToString());
                                bookingInsightsModel.Y7 = int.Parse(ds.Tables[1].Rows[i]["8"].ToString());
                                bookingInsightsModel.Y8 = int.Parse(ds.Tables[1].Rows[i]["9"].ToString());
                                bookingInsightsModel.Y9 = int.Parse(ds.Tables[1].Rows[i]["10+"].ToString());
                            }
                            model.LengthOfStay.Add(bookingInsightsModel);
                        }
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            var bookingInsightsModel = new BookingInsightsGraphData();

                            bookingInsightsModel.DataFor = ds.Tables[2].Rows[i]["DataFor"].ToString();
                            if (ds.Tables[2].Rows[i]["cType"].ToString() == "O")//(i < 4)
                            {
                                bookingInsightsModel.Y0_Overall = int.Parse(ds.Tables[2].Rows[i]["1"].ToString());
                                bookingInsightsModel.Y1_Overall = int.Parse(ds.Tables[2].Rows[i]["2"].ToString());
                                bookingInsightsModel.Y2_Overall = int.Parse(ds.Tables[2].Rows[i]["3"].ToString());
                                bookingInsightsModel.Y3_Overall = int.Parse(ds.Tables[2].Rows[i]["4"].ToString());
                                bookingInsightsModel.Y4_Overall = int.Parse(ds.Tables[2].Rows[i]["5"].ToString());
                                bookingInsightsModel.Y5_Overall = int.Parse(ds.Tables[2].Rows[i]["6"].ToString());
                                bookingInsightsModel.Y6_Overall = int.Parse(ds.Tables[2].Rows[i]["7"].ToString());
                                bookingInsightsModel.Y7_Overall = int.Parse(ds.Tables[2].Rows[i]["8"].ToString());
                            }
                            else
                            {
                                bookingInsightsModel.Y0 = int.Parse(ds.Tables[2].Rows[i]["1"].ToString());
                                bookingInsightsModel.Y1 = int.Parse(ds.Tables[2].Rows[i]["2"].ToString());
                                bookingInsightsModel.Y2 = int.Parse(ds.Tables[2].Rows[i]["3"].ToString());
                                bookingInsightsModel.Y3 = int.Parse(ds.Tables[2].Rows[i]["4"].ToString());
                                bookingInsightsModel.Y4 = int.Parse(ds.Tables[2].Rows[i]["5"].ToString());
                                bookingInsightsModel.Y5 = int.Parse(ds.Tables[2].Rows[i]["6"].ToString());
                                bookingInsightsModel.Y6 = int.Parse(ds.Tables[2].Rows[i]["7"].ToString());
                                bookingInsightsModel.Y7 = int.Parse(ds.Tables[2].Rows[i]["8"].ToString());
                            }
                            model.NoOfRooms.Add(bookingInsightsModel);
                        }
                    }


                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            var bookingInsightsModel = new BookingInsightsGraphData();

                            bookingInsightsModel.DataFor = ds.Tables[3].Rows[i]["DataFor"].ToString();
                            if (ds.Tables[3].Rows[i]["cType"].ToString() == "O")//(i < 4)
                            {
                                bookingInsightsModel.Y0_Overall = int.Parse(ds.Tables[3].Rows[i]["Mon"].ToString());
                                bookingInsightsModel.Y1_Overall = int.Parse(ds.Tables[3].Rows[i]["Tue"].ToString());
                                bookingInsightsModel.Y2_Overall = int.Parse(ds.Tables[3].Rows[i]["Wed"].ToString());
                                bookingInsightsModel.Y3_Overall = int.Parse(ds.Tables[3].Rows[i]["Thur"].ToString());
                                bookingInsightsModel.Y4_Overall = int.Parse(ds.Tables[3].Rows[i]["Fri"].ToString());
                                bookingInsightsModel.Y5_Overall = int.Parse(ds.Tables[3].Rows[i]["Sat"].ToString());
                                bookingInsightsModel.Y6_Overall = int.Parse(ds.Tables[3].Rows[i]["Sun"].ToString());
                            }
                            else
                            {
                                bookingInsightsModel.Y0 = int.Parse(ds.Tables[3].Rows[i]["Mon"].ToString());
                                bookingInsightsModel.Y1 = int.Parse(ds.Tables[3].Rows[i]["Tue"].ToString());
                                bookingInsightsModel.Y2 = int.Parse(ds.Tables[3].Rows[i]["Wed"].ToString());
                                bookingInsightsModel.Y3 = int.Parse(ds.Tables[3].Rows[i]["Thur"].ToString());
                                bookingInsightsModel.Y4 = int.Parse(ds.Tables[3].Rows[i]["Fri"].ToString());
                                bookingInsightsModel.Y5 = int.Parse(ds.Tables[3].Rows[i]["Sat"].ToString());
                                bookingInsightsModel.Y6 = int.Parse(ds.Tables[3].Rows[i]["Sun"].ToString());
                            }
                            model.DayOfWeek.Add(bookingInsightsModel);
                        }
                    }
                    //NOT REQUIRED
                    //if (ds.Tables[4].Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    //    {
                    //        var roomProductivityModel = new RoomProductivityGraphData();
                    //        roomProductivityModel.RoomName = ds.Tables[4].Rows[i]["sRoomName"].ToString();
                    //        roomProductivityModel.Value = int.Parse(ds.Tables[4].Rows[i]["Value"].ToString());
                    //        model.RoomProductivities.Add(roomProductivityModel);
                    //    }
                    //}
                }

                return model;
            }
        }

        #endregion

        //Check record exist or not
        public static bool CheckDataExist(string Name, int CityId, int AreaId, int LocalityId)
        {
            bool ifNotExist = true;
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var dbobj = db.tblPropertyMs.Select(u => new { u.sHotelName, u.iCityId, u.iAreaId, u.iLocalityId }).SingleOrDefault(u => u.sHotelName == Name && u.iCityId == CityId && u.iAreaId == AreaId && u.iLocalityId == LocalityId);
                    if (dbobj != null)
                        ifNotExist = false;
                }

            }
            catch (Exception)
            {
                ifNotExist = false;
            }
            return ifNotExist;
        }
        //Get Single Record
        public static etblPropertyM GetSingleRecordById(int id)
        {
            etblPropertyM eobj = new etblPropertyM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyMs.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertyM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get Single Record for user
        public static etblPropertyM_User GetSingleRecordById_User(Int64 id)
        {
            etblPropertyM_User eobj = new etblPropertyM_User();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyMs.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertyM_User)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Add new record
        public static int AddRecord(etblPropertyM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyM dbuser = (OneFineRate.tblPropertyM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyM());

                    dbuser.dMinIncome = 500;
                    dbuser.dtMinIncomeActionDate = DateTime.Now;
                    dbuser.iMinIncomeActionBy = dbuser.iActionBy;

                    db.tblPropertyMs.Add(dbuser);
                    db.SaveChanges();
                    int iPropId = dbuser.iPropId;

                    etblPropertyPolicyMap obj = new etblPropertyPolicyMap();
                    obj.iPropId = iPropId;
                    obj.bChildrenAllowed = false;
                    obj.bAlcoholAllowedOnsite = false;
                    obj.bAlcoholServedOnsite = false;
                    obj.bVisitorsAllowed = false;
                    obj.bPetsAllowed = false;
                    obj.bPartiesAllowed = false;
                    obj.b24HrsCheckIn = true;
                    obj.b24HrsCheckout = true;
                  

                    OneFineRate.tblPropertyPolicyMap dbPolicy = (OneFineRate.tblPropertyPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(obj, new OneFineRate.tblPropertyPolicyMap());
                    db.tblPropertyPolicyMaps.Add(dbPolicy);
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Update a record
        public static int UpdateRecord(etblPropertyM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblPropertyM obj = (OneFineRate.tblPropertyM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyM());
                    db.tblPropertyMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        //Update a record
        public static int UpdateRecord_User(etblPropertyM_User eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyM obj = (OneFineRate.tblPropertyM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyM());
                    db.tblPropertyMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    //remove old mapings
                    db.tblPropertyLocalityMaps.RemoveRange(db.tblPropertyLocalityMaps.Where(x => x.iPropId == eobj.iPropId));

                    //Add mapings
                    if (eobj.PropertyLocalityMapList != null)
                    {
                        db.tblPropertyLocalityMaps.AddRange(eobj.PropertyLocalityMapList.Select(x => new tblPropertyLocalityMap()
                        {
                            iPropId = x.iPropId,
                            iAreaLocalityId = x.iAreaLocalityId,
                            cAreaLocality = x.cAreaLocality,
                            dtActionDate = x.dtActionDate,
                            iActionBy = x.iActionBy
                        }).ToList());
                    }
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Update status
        public static int UpdateStatus(etblPropertyM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblPropertyM obj = (OneFineRate.tblPropertyM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyM());
                    db.tblPropertyMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Get list of records
        public static List<etblPropertyM> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblPropertyM> user = new List<etblPropertyM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var objTblUser = (from prop in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                                  join city in db.tblCityMs on prop.iCityId equals city.iCityId into Cities
                                  from PropCity in Cities
                                  join state in db.tblStateMs on prop.iStateId equals state.iStateId into States
                                  from PropState in States
                                  join country in db.tblCountryMs on prop.iCountryId equals country.iCountryId into Countries
                                  from PropCountry in Countries
                                  join t2 in db.tblUserMs on prop.iActionBy equals t2.iUserId

                                  join locality in db.tblLocalityMs on prop.iLocalityId equals locality.iLocalityId into Localities
                                  from PropLocality in Localities.DefaultIfEmpty()
                                  join Macroarea in db.tblMacroAreaMs on prop.iAreaId equals Macroarea.iAreaId into MacroAreas
                                  from PropMacroArea in MacroAreas.DefaultIfEmpty()
                                
                                  select new etblPropertyM
                                  {
                                      sHotelName = prop.sHotelName,
                                      iPropId = prop.iPropId,
                                      cStatus = prop.cStatus == "A" ? "Live" : "Disable",
                                      dtActionDate = prop.dtActionDate,
                                      ActionBy = t2.sFirstName + " " + t2.sLastName,
                                      sAddress = prop.sAddress + ", " + PropLocality.sLocality + ", " + PropMacroArea.sArea + ", " + PropCity.sCity + ", " + PropState.sState + ", " + PropCountry.sCountry + ", " + prop.iPinCode.ToString()
                                  }).Where(u => u.sHotelName.Contains(param.sSearch) || u.sAddress.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.iPropId) : objTblUser.OrderByDescending(u => u.iPropId);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sHotelName) : objTblUser.OrderByDescending(u => u.sHotelName);
                        break;
                }
                ////implemented paging
                var lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    user.Add((etblPropertyM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyM()));
                }
                return user;
            }
        }


        public static List<etblPropertyLocalityMap> GetPropertyLocalityMap(int propId)
        {
            List<etblPropertyLocalityMap> objMapping = new List<etblPropertyLocalityMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<PNames> lstPNames = BL_Locality.MacroLocalities();
                objMapping = (from map in db.tblPropertyLocalityMaps.ToList()
                              join loc in lstPNames on map.iAreaLocalityId equals Convert.ToInt32(loc.Id.Replace("|L", "").Replace("|A", ""))
                              where map.iPropId == propId
                              && (loc.Id.Contains("|") && map.cAreaLocality == loc.Id.ToString().Substring(loc.Id.IndexOf("|") + 1, 1))
                              select new etblPropertyLocalityMap()
                              {
                                  iPropId = propId,
                                  iAreaLocalityId = Convert.ToInt32(loc.Id.Replace("|L", "").Replace("|A", "")),
                                  cAreaLocalityName = loc.Name,
                                  cAreaLocality = map.cAreaLocality,
                                  iActionBy = map.iActionBy,
                                  dtActionDate = map.dtActionDate
                              }).ToList();
                return objMapping;
            }
        }

        public static List<eUsersPropertyMap> GetUserPropertyList(OneFineRateAppUtil.jQueryDataTableParamModel param, int iUserId, out int TotalCount)
        {
            List<eUsersPropertyMap> objMapping = new List<eUsersPropertyMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var ourl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                var props = (from prop in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                             join city in db.tblCityMs on prop.iCityId equals city.iCityId into Cities
                             from PropCity in Cities
                             join state in db.tblStateMs on prop.iStateId equals state.iStateId into States
                             from PropState in States
                             join country in db.tblCountryMs on prop.iCountryId equals country.iCountryId into Countries
                             from PropCountry in Countries

                             join map in db.tblUserPropertyMaps on new { prop.iPropId, iUserId } equals new { map.iPropId, map.iUserId } into MP
                             from MappedProps in MP

                             join locality in db.tblLocalityMs on prop.iLocalityId equals locality.iLocalityId into Localities
                             from PropLocality in Localities.DefaultIfEmpty()
                             join Macroarea in db.tblMacroAreaMs on prop.iAreaId equals Macroarea.iAreaId into MacroAreas
                             from PropMacroArea in MacroAreas.DefaultIfEmpty()



                             join B1 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "PC", p3 = "C", p4 = DateTime.Today } equals new { p1 = (int)B1.iPropId, p2 = B1.BookingStatus, p3 = B1.PaymentStatus, p4 = (DateTime)B1.dtCheckIn }
                             into Booking
                             from BookingList in Booking.DefaultIfEmpty()//.Where(a => a.BookingStatus == "PC" && a.PaymentStatus == "C" && a.dtCheckIn == DateTime.Today)

                             join B2 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "N", p3 = "NC" } equals new { p1 = (int)B2.iPropId, p2 = B2.cBookingType, p3 = B2.BookingStatus }
                             into Pending
                             from PendingList in Pending.DefaultIfEmpty()//.Where(a => a.cBookingType == "N" && a.BookingStatus == "NC").DefaultIfEmpty()

                             select new
                             {
                                 iUserId,
                                 MappedProps.iPropId,
                                 //  prop.sWebSite,
                                 sWebSite = "",
                                 sHotelName = prop.cStatus == "A" ? prop.sHotelName : prop.sHotelName + " (Disabled)",
                                 sAddress = prop.sAddress + ", " + PropCity.sCity + ", " + PropState.sState + ", " + PropCountry.sCountry + ", " + prop.iPinCode.ToString(),
                                 // sAddress = prop.sAddress + ", " + PropLocality.sLocality + ", " + PropMacroArea.sArea + ", " + PropCity.sCity + ", " + PropState.sState + ", " + PropCountry.sCountry + ", " + prop.iPinCode.ToString(),
                                 Pending = Pending.Count(),
                                 Arriving = Booking.Count()
                             }).Distinct().Where(u => u.sHotelName.Contains(param.sSearch) | u.sAddress.Contains(param.sSearch)).AsQueryable(); //

                TotalCount = props.Count();

                switch (param.iSortingCols)
                {
                    case 0:
                        props = param.sortDirection == "asc" ? props.OrderBy(u => u.iPropId) : props.OrderByDescending(u => u.iPropId);
                        break;
                    case 1:
                        props = param.sortDirection == "asc" ? props.OrderBy(u => u.sHotelName.Trim()) : props.OrderByDescending(u => u.sHotelName.Trim());
                        break;
                    case 3:
                        props = param.sortDirection == "asc" ? props.OrderBy(u => u.Arriving) : props.OrderByDescending(u => u.Arriving);
                        break;
                    case 4:
                        props = param.sortDirection == "asc" ? props.OrderBy(u => u.Pending) : props.OrderByDescending(u => u.Pending);
                        break;
                    default:
                        props = param.sortDirection == "asc" ? props.OrderBy(u => u.sHotelName.Trim()) : props.OrderByDescending(u => u.Pending);
                        break;

                }

                var lstUser = props.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();


                objMapping.AddRange(lstUser.Select(item => (eUsersPropertyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eUsersPropertyMap())));
            }

            return objMapping;
        }

        public static List<SelectListItem> GetUserPropertyList(int iUserId)
        {
            List<SelectListItem> objMapping = new List<SelectListItem>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var ourl = ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();

                var props = (from prop in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                             join city in db.tblCityMs on prop.iCityId equals city.iCityId into Cities
                             from PropCity in Cities
                             join state in db.tblStateMs on prop.iStateId equals state.iStateId into States
                             from PropState in States
                             join country in db.tblCountryMs on prop.iCountryId equals country.iCountryId into Countries
                             from PropCountry in Countries

                             join map in db.tblUserPropertyMaps on new { prop.iPropId, iUserId } equals new { map.iPropId, map.iUserId } into MP
                             from MappedProps in MP

                             join locality in db.tblLocalityMs on prop.iLocalityId equals locality.iLocalityId into Localities
                             from PropLocality in Localities.DefaultIfEmpty()
                             join Macroarea in db.tblMacroAreaMs on prop.iAreaId equals Macroarea.iAreaId into MacroAreas
                             from PropMacroArea in MacroAreas.DefaultIfEmpty()



                             join B1 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "PC", p3 = "C", p4 = DateTime.Today } equals new { p1 = (int)B1.iPropId, p2 = B1.BookingStatus, p3 = B1.PaymentStatus, p4 = (DateTime)B1.dtCheckIn }
                             into Booking
                             from BookingList in Booking.DefaultIfEmpty()//.Where(a => a.BookingStatus == "PC" && a.PaymentStatus == "C" && a.dtCheckIn == DateTime.Today)

                             join B2 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "N", p3 = "NC" } equals new { p1 = (int)B2.iPropId, p2 = B2.cBookingType, p3 = B2.BookingStatus }
                             into Pending
                             from PendingList in Pending.DefaultIfEmpty()//.Where(a => a.cBookingType == "N" && a.BookingStatus == "NC").DefaultIfEmpty()

                             select new SelectListItem
                             {
                                 Value = MappedProps.iPropId.ToString(),
                                 //  prop.sWebSite,
                                 Text = prop.cStatus == "A" ? prop.sHotelName : prop.sHotelName + " (Disabled)",
                                 // sAddress = prop.sAddress + ", " + PropLocality.sLocality + ", " + PropMacroArea.sArea + ", " + PropCity.sCity + ", " + PropState.sState + ", " + PropCountry.sCountry + ", " + prop.iPinCode.ToString(),
                             }).Distinct().AsQueryable(); //
                objMapping.AddRange(props);
            }
            return objMapping;
        }


        public static List<eUsersPropertyMap> GetListForSingle(int iUserId)
        {
            List<eUsersPropertyMap> objMapping = new List<eUsersPropertyMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var props = (from prop in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                             join map in db.tblUserPropertyMaps on new { prop.iPropId, iUserId } equals new { map.iPropId, map.iUserId } into MP
                             from MappedProps in MP
                             join city in db.tblCityMs on prop.iCityId equals city.iCityId into Cities
                             from PropCity in Cities
                             join state in db.tblStateMs on prop.iStateId equals state.iStateId into States
                             from PropState in States
                             join country in db.tblCountryMs on prop.iCountryId equals country.iCountryId into Countries
                             from PropCountry in Countries

                             select new
                             {
                                 iUserId,
                                 MappedProps.iPropId,
                                 prop.sWebSite,
                                 sHotelName = prop.cStatus == "A" ? prop.sHotelName : prop.sHotelName + " (Disabled)",
                                 sAddress = prop.sAddress + ", " + PropCity.sCity + ", " + PropState.sState + ", " + PropCountry.sCountry + ", " + prop.iPinCode.ToString(),
                                 Pending = 0,
                                 Arriving = 0
                             }).Distinct().AsQueryable();

                if (props.Count() == 1)
                {
                    var lstUser = props.ToList();
                    objMapping.AddRange(lstUser.Select(item => (eUsersPropertyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eUsersPropertyMap())));
                }
            }
            return objMapping;
        }
        public static List<PNames> GetAllPropertyNameByUserId(int iUserId)
        {

            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                objMapping = (from prop in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                              join map in db.tblUserPropertyMaps on new { prop.iPropId, iUserId } equals new { map.iPropId, map.iUserId } into MP
                              from MappedProps in MP
                              join B2 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "N", p3 = "NC" } equals new { p1 = (int)B2.iPropId, p2 = B2.cBookingType, p3 = B2.BookingStatus }
                              into Pending
                              from PendingList in Pending.DefaultIfEmpty()
                              select new PNames()
                              {
                                  Id = prop.iPropId.ToString(),
                                  Name = prop.sHotelName.Trim(),
                                  iCityId = Pending.Count()
                              }).Distinct().AsQueryable().ToList();

                return objMapping;
            }
        }
        public static List<PNames> GetAllPropertyNameByUserIdForPendingNegotiation(int iUserId)
        {
          
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                objMapping = (from prop in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                              join map in db.tblUserPropertyMaps on new { prop.iPropId, iUserId } equals new { map.iPropId, map.iUserId } into MP
                              from MappedProps in MP
                              join B2 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "N", p3 = "NC" } equals new { p1 = (int)B2.iPropId, p2 = B2.cBookingType, p3 = B2.BookingStatus }
                              into Pending
                              from PendingList in Pending.DefaultIfEmpty() where Pending.Count() > 0
                              select new PNames()
                              {
                                  Id = prop.iPropId.ToString(),
                                  Name = prop.sHotelName.Trim(),
                                  iCityId = Pending.Count()
                              }).Distinct().AsQueryable().ToList();

                return objMapping;
            }
        }
        public static List<PNames> GetAllPropertyName(int propId, string characters)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                              join t2 in db.tblLocalityMs on t1.iLocalityId equals t2.iLocalityId
                              join t3 in db.tblMacroAreaMs on t1.iAreaId equals t3.iAreaId
                              join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                              join t5 in db.tblStateMs on t1.iStateId equals t5.iStateId
                              where t1.iPropId != propId
                              select new PNames()
                              {
                                  Id = t1.iPropId.ToString(),
                                  Name = t1.sHotelName.Trim() + " , " + t2.sLocality.Trim() + " , " + t3.sArea.Trim() + " , " + t4.sCity.Trim() + " , " + t5.sState.Trim() + "(" + t1.iPropId + ")"
                              }).Where(u => u.Name.Contains(characters)).AsQueryable().ToList();
                return objMapping;
            }
        }
        public static List<PNames> GetAllPropertyList(int propId)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                              join t2 in db.tblLocalityMs on t1.iLocalityId equals t2.iLocalityId
                              join t3 in db.tblMacroAreaMs on t1.iAreaId equals t3.iAreaId
                              join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                              join t5 in db.tblStateMs on t1.iStateId equals t5.iStateId
                              where t1.iPropId != propId
                              select new PNames()
                              {
                                  Id = t1.iPropId.ToString(),
                                  Name = t1.sHotelName.Trim() + " , " + t2.sLocality.Trim() + " , " + t3.sArea.Trim() + " , " + t4.sCity.Trim() + " , " + t5.sState.Trim() + "(" + t1.iPropId + ")"
                              }).AsQueryable().ToList();
                return objMapping;
            }
        }

        /// <summary>
        /// Not in Use
        /// </summary>
        /// <param name="PropId"></param>
        /// <returns></returns>
        public static List<etblPropertyDashboardView> GetAllBestPossibleRates(int PropId)
        {
            List<etblPropertyDashboardView> promorec = new List<etblPropertyDashboardView>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@PropId", PropId);
                var data = db.Database.SqlQuery<etblPropertyDashboardView>("uspGetBestPossibleRatesForDashBoard_Temp @PropId", MyParam).ToList();

                foreach (var item in data)
                {
                    promorec.Add((etblPropertyDashboardView)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyDashboardView()));
                }

                return promorec;
            }
        }


        public static DataSet GetDataforPropertyDashboard(int propid)
        {
            clsDB obj = new clsDB();
            DataSet ds = new DataSet();
            ds = obj.ListPropertyDashboard(propid);
            return ds;
        }


        public static List<PNames> GetAllPropertyListExTG(string hotelSearchTxt)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                              join t2 in db.tblLocalityMs on t1.iLocalityId equals t2.iLocalityId
                              join t3 in db.tblMacroAreaMs on t1.iAreaId equals t3.iAreaId
                              join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                              join t5 in db.tblStateMs on t1.iStateId equals t5.iStateId
                              select new PNames()
                              {
                                  Id = t1.iPropId.ToString(),
                                  Name = t1.sHotelName.Trim() + " , " +
                                  t2.sLocality.Trim() + " , " +
                                  t3.sArea.Trim() + " , " +
                                  t4.sCity.Trim() + " , " +
                                  t5.sState.Trim() +
                                  "(" + t1.iPropId + ")"
                              }).Where(x => x.Name.Contains(hotelSearchTxt)).ToList();
                return objMapping;
            }
        }

        public static int GetPendingChangesCount(int iPropId)
        {
            int CNT = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var objMapping = (from prop in db.tblPropertyMs.Where(a => a.iPropId == iPropId)
                                  join B2 in db.tblBookingTxes on new { p1 = prop.iPropId, p2 = "N", p3 = "NC" } equals new { p1 = (int)B2.iPropId, p2 = B2.cBookingType, p3 = B2.BookingStatus }
                                  into Pending
                                  from PendingList in Pending.DefaultIfEmpty()
                                  select new
                                  {
                                      CNT = Pending.Count()
                                  }).Distinct().AsQueryable().ToList();

                CNT = objMapping[0].CNT;
            }
            return CNT;
        }

        public static IEnumerable<object> GetAllPropertyNameList(string hotelSearchTxt)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var result = (from t1 in db.tblPropertyMs.Where(a => a.bIsTG == null || a.bIsTG == false)
                              join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                              where t1.sHotelName.Contains(hotelSearchTxt)
                              select new
                              {
                                  Id = t1.iPropId.ToString(),
                                  Name = "(" + t1.iPropId + ")" + t1.sHotelName.Trim() + ", " + t4.sCity.Trim()
                              }).ToList();

                //join t2 in db.tblLocalityMs on t1.iLocalityId equals t2.iLocalityId
                // join t3 in db.tblMacroAreaMs on t1.iAreaId equals t3.iAreaId
                // join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                // join t5 in db.tblStateMs on t1.iStateId equals t5.iStateId

                return result;
            }
        }


        public static IEnumerable<object> GetAllCityNameList(string citySearchTxt)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var result = (from t1 in db.tblCityMs
                              where t1.cStatus == "A" && t1.sCity.Contains(citySearchTxt)
                              select new
                              {
                                  Id = t1.iCityId.ToString(),
                                  Name = t1.sCity.Trim()
                              }).Take(100).ToList();

                return result;
            }
        }
    }
}