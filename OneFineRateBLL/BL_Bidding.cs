using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace OneFineRateBLL
{
    public class BL_Bidding
    {

        //Add Guest Details
        public static int AddGuestDetailsRecord(GuestUserDetails objDetail)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblWebsiteGuestUserMaster dbuser = (OneFineRate.tblWebsiteGuestUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(objDetail, new OneFineRate.tblWebsiteGuestUserMaster());
                    db.tblWebsiteGuestUserMasters.Add(dbuser);
                    db.SaveChanges();
                    retval = Convert.ToInt32(dbuser.Id);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }


        //Get Bid Setting record
        public static etblBidSettingsM GetBidMaster(string CurrencyCode)
        {
            etblBidSettingsM eobj = new etblBidSettingsM();
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    eobj = db.Database.SqlQuery<etblBidSettingsM>("uspGetExchangeRates @sCurrencyCode", new SqlParameter("@sCurrencyCode", CurrencyCode)).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return eobj;
        }

        //Check bidding count
        public static int GetBiddingCount(string MobileNo)
        {
            int bCount = 0;
            try
            {
                var MaxDate = DateTime.Now.AddDays(1);
                var MinDate = DateTime.Now.AddDays(-1);
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    bCount = db.tblBidCountsTxes.Where(u => u.sMobile == MobileNo && (MaxDate > u.dtActionDate && MinDate <= u.dtActionDate)).Count();
                }
            }
            catch (Exception)
            {
                bCount = 0;
            }
            return bCount;
        }
        //Add a record to tblBidCountsTx
        public static void AddRecordToBiddingCount(eBiddingCounts eobj)
        {

            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    //SqlParameter[] MyParam = new SqlParameter[1];
                    //MyParam[0] = new SqlParameter("@Mobile", eobj.sMobile);
                    //db.Database.SqlQuery<int>("uspAddBiddingCounts @Mobile", MyParam);
                    eobj.dtActionDate = DateTime.Now;
                    OneFineRate.tblBidCountsTx dbuser = (OneFineRate.tblBidCountsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblBidCountsTx());
                    db.tblBidCountsTxes.Add(dbuser);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }
        //Add a record to guest details table
        public static int AddRecord(eBiddingSearch eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    GuestUserDetails objDetail = new GuestUserDetails();
                    objDetail.Title = eobj.sUserTitle;
                    objDetail.FirstName = eobj.sUserFirstName;
                    objDetail.LastName = eobj.sUserLastName;
                    objDetail.Email = eobj.sUserEmail;
                    objDetail.PhoneNumber = eobj.sUserMobileNo;
                    OneFineRate.tblWebsiteGuestUserMaster dbuser = (OneFineRate.tblWebsiteGuestUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(objDetail, new OneFineRate.tblWebsiteGuestUserMaster());
                    db.tblWebsiteGuestUserMasters.Add(dbuser);
                    db.SaveChanges();
                    retval = Convert.ToInt32(dbuser.Id);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        public static List<eDropDown> GetAreaLocalityForBid(string txt, int Type, out List<etblIndianLocalityCordinate> cordinatesList)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eDropDown> eobj = new List<eDropDown>();
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@Type", txt);
                MyParam[1] = new SqlParameter("@Value", Type);
                var result = db.Database.SqlQuery<eDropDown>("uspGetAeaLocalityForBid @Type,@Value ", MyParam).ToList();

                List<etblIndianLocalityCordinate> polygonCordinateList = new List<etblIndianLocalityCordinate>();

                var idArray = result.Select(x => x.Name.Trim()).ToArray();
                var respectivePolygons = db.tblIndianLocalityCordinates.Where(x => idArray.Contains(x.LocalityName.Trim())).ToList();

                foreach (var item in result)
                {
                    var polygon = respectivePolygons.Where(x => x.LocalityName.Contains(item.Name.Trim())).FirstOrDefault();
                    if (polygon != null)
                    {
                        var tblCordinate = new etblIndianLocalityCordinate();

                        tblCordinate.Id = item.Id;
                        tblCordinate.LocalityName = polygon.LocalityName;
                        tblCordinate.GoogleFriendlyPlaceId = polygon.GoogleFriendlyPlaceId;
                        tblCordinate.StateName = polygon.StateName;
                        tblCordinate.tblPolygons.AddRange(polygon.tblPolygons.Select(x => new etblPolygon { Id = item.LocalityCordinates.Id, PolygonCordinates = x.PolygonCordinates }).ToList());

                        polygonCordinateList.Add(tblCordinate);
                    }

                    eobj.Add((eDropDown)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eDropDown()));
                }

                cordinatesList = polygonCordinateList;

                return eobj;
            }
        }
        /// <summary>
        /// Calculate the bid range afters election of start rating and area locality
        /// </summary>
        /// <param name="checkin"></param>
        /// <param name="checkout"></param>
        /// <param name="currency"></param>
        /// <param name="StarRating"></param>
        /// <param name="dtTblRoomOccupancySearch"></param>
        /// <param name="dtTblChildrenAgeSearch"></param>
        /// <param name="Type"></param>
        /// <param name="dtIds"></param>
        /// <returns></returns>
        public static eBidRangeValues GetBidRangeValues(DateTime checkin, DateTime checkout, string currency, int StarRating, DataTable dtTblRoomOccupancySearch, DataTable dtTblChildrenAgeSearch, string Type, DataTable dtIds)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                eBidRangeValues eobj = new eBidRangeValues();
                SqlParameter[] MyParam = new SqlParameter[8];
                MyParam[0] = new SqlParameter("@dtCheckIn", checkin);
                MyParam[1] = new SqlParameter("@dtCheckOut", checkout);
                MyParam[2] = new SqlParameter("@Currency", currency);
                MyParam[3] = new SqlParameter("@iStarCategory", StarRating);
                MyParam[4] = new SqlParameter("@RoomOccupancySearch", dtTblRoomOccupancySearch);
                MyParam[4].TypeName = "[dbo].[RoomOccupancySearch]";
                MyParam[5] = new SqlParameter("@ChildrenAgeSearch", dtTblChildrenAgeSearch);
                MyParam[5].TypeName = "[dbo].[ChildrenAgeSearch]";
                MyParam[6] = new SqlParameter("@sType", Type);
                MyParam[7] = new SqlParameter("@Ids", dtIds);
                MyParam[7].TypeName = "[dbo].[Ids]";

                DataSet ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspGetBidRange", MyParam);

                if (ds.Tables.Count > 0)
                {
                    var bidRangeTable = ds.Tables[0];

                    if (bidRangeTable != null)
                    {
                        eobj.MaxRange = Convert.ToInt32(bidRangeTable.Rows[0]["MaxRange"]);
                        eobj.MinRange = Convert.ToInt32(bidRangeTable.Rows[0]["MinRange"]);
                        eobj.MinAllowed = Convert.ToInt32(bidRangeTable.Rows[0]["MinAllowed"]);
                        eobj.Msg = bidRangeTable.Rows[0]["Msg"].ToString();
                        eobj.sSymbol = bidRangeTable.Rows[0]["sSymbol"].ToString();
                    }

                    var bidRangeLatLngTable = ds.Tables[1];

                    List<LatLngBounds> laLongBoundList = new List<LatLngBounds>();

                    if (bidRangeLatLngTable.Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            laLongBoundList.Add(new LatLngBounds()
                            {
                                dLatitude = Convert.ToDecimal(bidRangeLatLngTable.Rows[i]["dLatitude"]),
                                dLongitude = Convert.ToDecimal(bidRangeLatLngTable.Rows[i]["dLongitude"])
                            });
                        }
                    }

                    eobj.LatLngBoundList = laLongBoundList;
                }

                //eobj = db.Database.SqlQuery<eBidRangeValues>("uspGetBidRange @dtCheckIn,@dtCheckOut,@Currency,@iStarCategory,@RoomOccupancySearch,@ChildrenAgeSearch,@sType,@Ids ", MyParam).SingleOrDefault();

                return eobj;
            }
        }
        public static eBiddingSearch SearchHotelsByBidAmount(DateTime checkin, DateTime checkout, string currency, int StarRating, DataTable dtTblRoomOccupancySearch, DataTable dtTblChildrenAgeSearch, string Type, DataTable dtIds, decimal BidAmount, int stateId)
        {
            DataSet ds = new DataSet();
            eBiddingSearch obj = new eBiddingSearch();
            SqlParameter[] MyParam = new SqlParameter[10];
            MyParam[0] = new SqlParameter("@dtCheckIn", checkin);
            MyParam[1] = new SqlParameter("@dtCheckOut", checkout);
            MyParam[2] = new SqlParameter("@Currency", currency);
            MyParam[3] = new SqlParameter("@iStarCategory", StarRating);
            MyParam[4] = new SqlParameter("@RoomOccupancySearch", dtTblRoomOccupancySearch);
            MyParam[4].TypeName = "[dbo].[RoomOccupancySearch]";
            MyParam[5] = new SqlParameter("@ChildrenAgeSearch", dtTblChildrenAgeSearch);
            MyParam[5].TypeName = "[dbo].[ChildrenAgeSearch]";
            MyParam[6] = new SqlParameter("@sType", Type);
            MyParam[7] = new SqlParameter("@Ids", dtIds);
            MyParam[7].TypeName = "[dbo].[Ids]";
            MyParam[8] = new SqlParameter("@dBidAmount", BidAmount);
            MyParam[9] = new SqlParameter("@iStateId", stateId);
            using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
            {
                //objConn.Open();

                //ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSearchHotelsByBid", MyParam);                
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSearchHotelsByBid", MyParam);

                if (ds.Tables.Count > 0)
                {
                    obj.Status = Convert.ToInt16(ds.Tables[0].Rows[0]["status"]);
                    obj.Message = ds.Tables[0].Rows[0]["msg"].ToString();
                    obj.Amount = ds.Tables[0].Rows[0]["amt"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["amt"].ToString());
                    obj.Tax = ds.Tables[0].Rows[0]["Tax"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Tax"].ToString());
                    obj.sCheckIn_Display = ds.Tables[0].Rows[0]["sCheckIn_Display"].ToString();
                    obj.sCheckOut_Display = ds.Tables[0].Rows[0]["sCheckOut_Display"].ToString();
                    obj.TopMessage = ds.Tables[0].Rows[0]["TopMsg"].ToString();

                    obj.sTaxOnServiceCharge_Per = ds.Tables[0].Rows[0]["TotalServiceChargeType"].ToString();
                    obj.sTaxOnServiceCharge_Val = ds.Tables[0].Rows[0]["TaxOnServiceCharge"].ToString();
                    obj.sOFRServiceChargeIncludingTax = ds.Tables[0].Rows[0]["TotalServiceCharge"].ToString();
                    obj.sOFRServiceCharge = ds.Tables[0].Rows[0]["ServiceCharge"].ToString();
                    obj.bShowIGST = ds.Tables[0].Rows[0]["ShowIGST"].ToString() == "0" ? false : true;
                    obj.SGST_Per = ds.Tables[0].Rows[0]["SGST_Per"].ToString();
                    obj.SGST_Val = ds.Tables[0].Rows[0]["SGST_Val"].ToString();
                    obj.sHotelTax = ds.Tables[0].Rows[0]["Tax"].ToString(); 
                    obj.cGstValueType = ds.Tables[0].Rows[0]["cGSTValueType"].ToString();
                    obj.dGSTValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["dGSTValue"]);
                    obj.MinTaxPer = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MinTaxPer"]))? Convert.ToString(ds.Tables[0].Rows[0]["MinTaxPer"]):"";
                    if (obj.Status == 1)
                    {
                        List<GetHotelPreviews> lst = new List<GetHotelPreviews>();

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            lst.Add(new GetHotelPreviews()
                            {
                                Name = ds.Tables[1].Rows[i]["msg"].ToString()
                            });
                        }
                        obj.lstHotelData = lst;
                    }
                }
            }

            return obj;

        }
        public static List<GetLocalitiesName> GetAllLocalitiesName(string ids, string Type)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<GetLocalitiesName> eobj = new List<GetLocalitiesName>();
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@Ids", ids);
                MyParam[1] = new SqlParameter("@Type", Type);
                var result = db.Database.SqlQuery<GetLocalitiesName>("uspGetLocalitiesName @Ids,@Type ", MyParam).ToList();
                foreach (var item in result)
                {
                    eobj.Add((GetLocalitiesName)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new GetLocalitiesName()));
                }
                return eobj;
            }
        }
        public static eBidding GetSearchedBidHotelsList(DateTime checkin, DateTime checkout, string currency, int StarRating, DataTable dtTblRoomOccupancySearch, DataTable dtTblChildrenAgeSearch, string Type, DataTable dtIds, decimal BidAmount)
        {
            SqlConnection objConn = default(SqlConnection);
            DataSet ds = new DataSet();
            objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            objConn.Open();
            eBidding obj = new eBidding();


            SqlParameter[] MyParam = new SqlParameter[9];
            MyParam[0] = new SqlParameter("@dtCheckIn", checkin);
            MyParam[1] = new SqlParameter("@dtCheckOut", checkout);
            MyParam[2] = new SqlParameter("@Currency", currency);
            MyParam[3] = new SqlParameter("@iStarCategory", StarRating);
            MyParam[4] = new SqlParameter("@RoomOccupancySearch", dtTblRoomOccupancySearch);
            MyParam[4].TypeName = "[dbo].[RoomOccupancySearch]";
            MyParam[5] = new SqlParameter("@ChildrenAgeSearch", dtTblChildrenAgeSearch);
            MyParam[5].TypeName = "[dbo].[ChildrenAgeSearch]";
            MyParam[6] = new SqlParameter("@sType", Type);
            MyParam[7] = new SqlParameter("@Ids", dtIds);
            MyParam[7].TypeName = "[dbo].[Ids]";
            MyParam[8] = new SqlParameter("@dBidAmount", BidAmount);

            ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBidHotelsFinalList", MyParam);
            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[1].Rows.Count > 0)
                {
                    List<BidRoomsData> lstRooms = new List<BidRoomsData>();

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        List<BidAmenAppl> lstAppl = new List<BidAmenAppl>();
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {

                            if (Convert.ToInt32(ds.Tables[0].Rows[j]["iHotelId"]) == Convert.ToInt32(ds.Tables[1].Rows[i]["iPropId"].ToString()))
                            {
                                lstAppl.Add(new BidAmenAppl()
                                {
                                    iHotelId = Convert.ToInt32(ds.Tables[0].Rows[j]["iHotelId"]),
                                    Amen = Convert.ToString(ds.Tables[0].Rows[j]["Amen"]),
                                    Appl = Convert.ToString(ds.Tables[0].Rows[j]["Appl"])
                                });
                            }
                        }



                        var roomData = new BidRoomsData()
                        {
                            LastBook = ds.Tables[1].Rows[i]["LastBook"].ToString(),
                            Looking = Convert.ToInt32(ds.Tables[1].Rows[i]["Looking"]),
                            iRank = Convert.ToInt32(ds.Tables[1].Rows[i]["iRank"]),
                            sHotelName = Convert.ToString(ds.Tables[1].Rows[i]["sHotelName"]),
                            sLocality = Convert.ToString(ds.Tables[1].Rows[i]["sLocality"]),
                            //sDescription = Convert.ToString(ds.Tables[1].Rows[i]["sDescription"]),
                            iStarCategoryId = Convert.ToInt16(ds.Tables[1].Rows[i]["iStarCategoryId"]),
                            iPropId = Convert.ToInt32(ds.Tables[1].Rows[i]["iPropId"]),
                            dPrice = Convert.ToDecimal(ds.Tables[1].Rows[i]["dPrice"]),
                            dDiscPrice = Convert.ToDecimal(ds.Tables[1].Rows[i]["dDiscPrice"]),
                            iRoomId = Convert.ToInt32(ds.Tables[1].Rows[i]["iRoomId"]),
                            iRPId = Convert.ToInt32(ds.Tables[1].Rows[i]["iRPId"]),
                            sImgUrl = Convert.ToString(ds.Tables[1].Rows[i]["sImgUrl"]),
                            Discount = Convert.ToDecimal(ds.Tables[1].Rows[i]["Discount"]),
                            bUpgrade = Convert.ToBoolean(ds.Tables[1].Rows[i]["bUpgrade"]),
                            Amenity1 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity1"]),
                            Amenity2 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity2"]),
                            Amenity3 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity3"]),
                            Amenity4 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity4"]),
                            Amenity5 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity5"]),
                            Amenity6 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity6"]),
                            Amenity7 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity7"]),
                            Amenity8 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity8"]),
                            Amenity9 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity9"]),
                            Amenity10 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity10"]),
                            Amenity11 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity11"]),
                            Amenity12 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity12"]),
                            Amenity13 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity13"]),
                            Amenity14 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity14"]),
                            Amenity15 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity15"]),
                            Amenity16 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity16"]),
                            Amenity17 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity17"]),
                            Amenity18 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity18"]),
                            dLatitude = Convert.ToInt32(ds.Tables[1].Rows[i]["dLatitude"]),
                            dLongitude = Convert.ToInt32(ds.Tables[1].Rows[i]["dLongitude"]),
                            rating_image_url = Convert.ToString(ds.Tables[1].Rows[i]["rating_image_url"]),
                            rating = Convert.ToDecimal(ds.Tables[1].Rows[i]["rating"]),
                            ranking_string = Convert.ToString(ds.Tables[1].Rows[i]["ranking_string"]),
                            bIsTopHotel = Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsTopHotel"]),
                            bIsPopularHotel = Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsPopularHotel"]),
                            bIsNew = Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsNew"]),
                            bIsHighDemand = Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsHighDemand"]),
                            sCurrencySymbol = Convert.ToString(ds.Tables[1].Rows[i]["Symbol"]),
                            lstBidAmenAppl = lstAppl
                        };

                        lstRooms.Add(roomData);
                    }

                    obj.lstBidRoomsData = lstRooms;
                }

            }
            objConn.Close();

            return obj;

        }

        public static eBidding GetSearchedBidHotelsListForUnfinished(long BookingId)
        {
            SqlConnection objConn = default(SqlConnection);
            DataSet ds = new DataSet();
            objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            objConn.Open();
            eBidding obj = new eBidding();


            SqlParameter[] MyParam = new SqlParameter[1];
            MyParam[0] = new SqlParameter("@iBookingId", Convert.ToInt32(BookingId));
            ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBidHotelsFinalListForUnfinished", MyParam);
            if (ds.Tables.Count > 0)
            {


                if (ds.Tables[1].Rows.Count > 0)
                {
                    List<BidRoomsData> lstRooms = new List<BidRoomsData>();
                    List<int> lstPropIDs = new List<int>();


                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        List<BidAmenAppl> lstAppl = new List<BidAmenAppl>();
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {

                            if (Convert.ToInt32(ds.Tables[0].Rows[j]["iHotelId"]) == Convert.ToInt32(ds.Tables[1].Rows[i]["iPropId"].ToString()))
                            {
                                lstAppl.Add(new BidAmenAppl()
                                {
                                    iHotelId = Convert.ToInt32(ds.Tables[0].Rows[j]["iHotelId"]),
                                    Amen = Convert.ToString(ds.Tables[0].Rows[j]["Amen"]),
                                    Appl = Convert.ToString(ds.Tables[0].Rows[j]["Appl"])
                                });
                            }
                        }



                        var a = new BidRoomsData();

                        a.LastBook = ds.Tables[1].Rows[i]["LastBook"].ToString();
                        a.Looking = ds.Tables[1].Rows[i]["Looking"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["Looking"]) : 0;
                        a.iRank = ds.Tables[1].Rows[i]["iRank"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["iRank"]) : 0;
                        a.sHotelName = Convert.ToString(ds.Tables[1].Rows[i]["sHotelName"]);
                        a.sLocality = Convert.ToString(ds.Tables[1].Rows[i]["sLocality"]);
                        //a.sDescription = Convert.ToString(ds.Tables[1].Rows[i]["sDescription"]);
                        a.iStarCategoryId = Convert.ToInt16(ds.Tables[1].Rows[i]["iStarCategoryId"]);
                        a.iPropId = Convert.ToInt32(ds.Tables[1].Rows[i]["iPropId"]);
                        a.dPrice = ds.Tables[1].Rows[i]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[i]["dPrice"]) : 0;
                        a.dDiscPrice = ds.Tables[1].Rows[i]["dDiscPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[i]["dDiscPrice"]) : 0;
                        a.iRoomId = Convert.ToInt32(ds.Tables[1].Rows[i]["iRoomId"]);
                        a.iRPId = Convert.ToInt32(ds.Tables[1].Rows[i]["iRPId"]);
                        a.sImgUrl = Convert.ToString(ds.Tables[1].Rows[i]["sImgUrl"]);
                        a.Discount = Convert.ToDecimal(ds.Tables[1].Rows[i]["Discount"]);
                        a.bUpgrade = ds.Tables[1].Rows[i]["bUpgrade"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[i]["bUpgrade"]) : false;
                        a.Amenity1 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity1"]);
                        a.Amenity2 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity2"]);
                        a.Amenity3 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity3"]);
                        a.Amenity4 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity4"]);
                        a.Amenity5 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity5"]);
                        a.Amenity6 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity6"]);
                        a.Amenity7 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity7"]);
                        a.Amenity8 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity8"]);
                        a.Amenity9 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity9"]);
                        a.Amenity10 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity10"]);
                        a.Amenity11 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity11"]);
                        a.Amenity12 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity12"]);
                        a.Amenity13 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity13"]);
                        a.Amenity14 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity14"]);
                        a.Amenity15 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity15"]);
                        a.Amenity16 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity16"]);
                        a.Amenity17 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity17"]);
                        a.Amenity18 = Convert.ToString(ds.Tables[1].Rows[i]["Amenity18"]);
                        a.dLatitude = ds.Tables[1].Rows[i]["dLatitude"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[i]["dLatitude"]) : 0;
                        a.dLongitude = ds.Tables[1].Rows[i]["dLongitude"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[i]["dLongitude"]) : 0;
                        a.rating_image_url = Convert.ToString(ds.Tables[1].Rows[i]["rating_image_url"]);
                        a.rating = ds.Tables[1].Rows[i]["rating"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[i]["rating"]) : 0;
                        a.ranking_string = Convert.ToString(ds.Tables[1].Rows[i]["ranking_string"]);
                        a.bIsTopHotel = ds.Tables[1].Rows[i]["bIsTopHotel"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsTopHotel"]) : false;
                        a.bIsPopularHotel = ds.Tables[1].Rows[i]["bIsPopularHotel"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsPopularHotel"]) : false;
                        a.bIsNew = ds.Tables[1].Rows[i]["bIsNew"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsNew"]) : false;
                        a.bIsHighDemand = ds.Tables[1].Rows[i]["bIsHighDemand"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[i]["bIsHighDemand"]) : false;
                        a.sCurrencySymbol = Convert.ToString(ds.Tables[1].Rows[i]["Symbol"]);
                        a.lstBidAmenAppl = lstAppl;
                        a.ExtraTaxPer = Convert.ToString(ds.Tables[1].Rows[i]["ExtraTaxPer"]);
                       
                        lstRooms.Add(a);

                    }




                    obj.lstBidRoomsData = lstRooms;
                    obj.sCheckIn = Convert.ToString(ds.Tables[2].Rows[0]["dtCheckIn"]);
                    obj.sCheckOut = Convert.ToString(ds.Tables[2].Rows[0]["dtChekOut"]);
                    obj.sCheckInDateDisplay = Convert.ToString(ds.Tables[2].Rows[0]["sCheckInDate"]);
                    obj.sCheckOutDateDisplay = Convert.ToString(ds.Tables[2].Rows[0]["sCheckOutDate"]);
            }

            }
            objConn.Close();

            return obj;

        }
        public static eBidBookingResult UpdateBidBooking(int bookingId, int PropId)
        {
            try
            {
                eBidBookingResult model = new eBidBookingResult();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlConnection objConn = default(SqlConnection);
                    DataSet ds = new DataSet();
                    objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                    objConn.Open();

                    SqlParameter[] MyParam = new SqlParameter[2];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingId);
                    MyParam[1] = new SqlParameter("@iSelectedPropId", PropId);

                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSaveBidHotel", MyParam);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            model.ID = Convert.ToInt32(ds.Tables[1].Rows[0]["ID"]);
                            model.Message = Convert.ToString(ds.Tables[1].Rows[0]["Message"]);
                            model.Status = Convert.ToString(ds.Tables[1].Rows[0]["Status"]);
                        }

                    }
                    objConn.Close();

                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static BiddingHotelsUpgrade GetHotelUpgradeResult(int PropId, int BookingId)
        {
            SqlConnection objConn = default(SqlConnection);
            DataSet ds = new DataSet();
            objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            objConn.Open();
            BiddingHotelsUpgrade obj = new BiddingHotelsUpgrade();


            SqlParameter[] MyParam = new SqlParameter[2];
            MyParam[0] = new SqlParameter("@iBookingId", BookingId);
            MyParam[1] = new SqlParameter("@propid", PropId);

            ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetUpgradeList", MyParam);
            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<eBiddingHotelsUpgradeRoomsList> lstRooms = new List<eBiddingHotelsUpgradeRoomsList>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        List<eBiddingHotelsUpgradeImages> lstImages = new List<eBiddingHotelsUpgradeImages>();
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {

                            if (Convert.ToInt32(ds.Tables[0].Rows[i]["iRoomId"]) == Convert.ToInt32(ds.Tables[1].Rows[j]["RoomId"].ToString()))
                            {
                                lstImages.Add(new eBiddingHotelsUpgradeImages()
                                {
                                    iPropId = Convert.ToInt32(ds.Tables[1].Rows[j]["iPropId"]),
                                    RoomId = Convert.ToInt32(ds.Tables[1].Rows[j]["RoomId"]),
                                    bIsPrimaryR = Convert.ToBoolean(ds.Tables[1].Rows[j]["bIsPrimaryR"]),
                                    Image = ds.Tables[1].Rows[j]["Image"].ToString()
                                });
                            }
                        }

                        List<eBiddingHotelsUpgradeViews> lstViews = new List<eBiddingHotelsUpgradeViews>();
                        for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                        {

                            if (Convert.ToInt32(ds.Tables[0].Rows[i]["iRoomId"]) == Convert.ToInt32(ds.Tables[2].Rows[k]["iRoomId"].ToString()))
                            {
                                lstViews.Add(new eBiddingHotelsUpgradeViews()
                                {
                                    iPropId = Convert.ToInt32(ds.Tables[2].Rows[k]["iPropId"]),
                                    iRoomId = Convert.ToInt32(ds.Tables[2].Rows[k]["iRoomId"]),
                                    sView = ds.Tables[2].Rows[k]["sView"].ToString()
                                });
                            }
                        }

                        List<eBiddingHotelsUpgradeFacilities> lstFacilities = new List<eBiddingHotelsUpgradeFacilities>();
                        for (int l = 0; l < ds.Tables[3].Rows.Count; l++)
                        {

                            if (Convert.ToInt32(ds.Tables[0].Rows[i]["iRoomId"]) == Convert.ToInt32(ds.Tables[3].Rows[l]["iRoomId"].ToString()))
                            {
                                lstFacilities.Add(new eBiddingHotelsUpgradeFacilities()
                                {
                                    iPropId = Convert.ToInt32(ds.Tables[3].Rows[l]["iPropId"]),
                                    iRoomId = Convert.ToInt32(ds.Tables[3].Rows[l]["iRoomId"]),
                                    sFacility = ds.Tables[3].Rows[l]["sFacility"].ToString()
                                });
                            }
                        }



                        var roomData = new eBiddingHotelsUpgradeRoomsList()
                        {
                            sRoomName = Convert.ToString(ds.Tables[0].Rows[i]["sRoomName"]),
                            dSizeMtr = Convert.ToDecimal(ds.Tables[0].Rows[i]["dSizeMtr"]),
                            dSizeSqft = Convert.ToDecimal(ds.Tables[0].Rows[i]["dSizeSqft"]),
                            IsUpgradedRoom = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsUpgradedRoom"]),
                            iPropId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPropId"]),
                            dPrice = Convert.ToDecimal(ds.Tables[0].Rows[i]["dPrice"]),
                            dDiscPrice = Convert.ToDecimal(ds.Tables[0].Rows[i]["dDiscPrice"]),
                            iRoomId = Convert.ToInt32(ds.Tables[0].Rows[i]["iRoomId"]),
                            iRPId = Convert.ToInt32(ds.Tables[0].Rows[i]["iRPId"]),
                            dPriceWithTax = Convert.ToDecimal(ds.Tables[0].Rows[i]["dPriceWithTax"]),
                            dTax = Convert.ToDecimal(ds.Tables[0].Rows[i]["dTax"]),
                            sImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sImgUrl"]),
                            TotalDifference = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total Difference"]),
                            TotalDifferenceConverted = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total Difference Converted"]),
                            TaxDifference = Convert.ToDecimal(ds.Tables[0].Rows[i]["Tax Difference"]),
                            Amenity1 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity1"]),
                            Amenity2 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity2"]),
                            Amenity3 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity3"]),
                            Amenity4 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity4"]),
                            Amenity5 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity5"]),
                            Amenity6 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity6"]),
                            Amenity7 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity7"]),
                            Amenity8 = Convert.ToString(ds.Tables[0].Rows[i]["Amenity8"]),
                            Symbol = Convert.ToString(ds.Tables[0].Rows[i]["Symbol"]),
                            lstImages = lstImages,
                            lstViews = lstViews,
                            lstfacilities = lstFacilities
                        };


                        lstRooms.Add(roomData);
                    }

                    obj.lstRoomsData = lstRooms;
                }

            }
            objConn.Close();

            return obj;

        }
        public static eBidBookingResult UpdateUpgradeBidBooking(long bookingId, int PropId, int roomid, decimal extrapaid, decimal extrataxpaid)
        {
            try
            {
                eBidBookingResult model = new eBidBookingResult();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[5];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingId);
                    MyParam[1] = new SqlParameter("@iSelectedPropId", PropId);
                    MyParam[2] = new SqlParameter("@iRoomId", roomid);
                    MyParam[3] = new SqlParameter("@dExtraPaid", extrapaid);
                    MyParam[4] = new SqlParameter("@dExtraTaxPaid", extrataxpaid);
                    model = db.Database.SqlQuery<eBidBookingResult>("uspSaveBidHotelUpgrade @iBookingId,@iSelectedPropId,@iRoomId,@dExtraPaid,@dExtraTaxPaid", MyParam).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static eBidBookingResult UpdateUpgradeBidBookingUnfinished(long bookingId, int PropId, int roomid, decimal extrapaid, decimal extrataxpaid, string sAuthCode)
        {
            try
            {
                eBidBookingResult model = new eBidBookingResult();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[6];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingId);
                    MyParam[1] = new SqlParameter("@iSelectedPropId", PropId);
                    MyParam[2] = new SqlParameter("@iRoomId", roomid);
                    MyParam[3] = new SqlParameter("@dExtraPaid", extrapaid);
                    MyParam[4] = new SqlParameter("@dExtraTaxPaid", extrataxpaid);
                    MyParam[5] = new SqlParameter("@sAuthCode", sAuthCode);
                    model = db.Database.SqlQuery<eBidBookingResult>("uspSaveBidHotelUpgradeFromUnfinished @iBookingId,@iSelectedPropId,@iRoomId,@dExtraPaid,@dExtraTaxPaid,@sAuthCode", MyParam).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}