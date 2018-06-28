using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace OneFineRateBLL
{
    public class BL_PropDetails
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="objSearchDetails"></param>
        /// <param name="dtRoomOccupancySearch"></param>
        /// <param name="dtChildrenAgeSearch"></param>
        /// <returns></returns>
        public static PropDetailsM GetPropertyDetails(int propId, PropDetailsM objSearchDetails, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch)
        {
            dtRoomOccupancySearch.TableName = "ABC";
            dtChildrenAgeSearch.TableName = "XYZ";

            PropDetailsM propDetail = new PropDetailsM();

            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region General Details
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@PropId", propId);
                var objresult = db.Database.SqlQuery<PropDetailsM>("uspGetOfferReviewDetailsbyID @PropId", MyParam).ToList();
                propDetail = (PropDetailsM)OneFineRateAppUtil.clsUtils.ConvertToObject(objresult[0], propDetail);


                SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                propDetail.lstetblHotelFacilities = objresultHotelFacilities;

                string[] arrMapURL = new string[50];
                string sPropertyNameresult = "";
                arrMapURL = propDetail.sPropertyName.Split(' ');
                for (int i = 0; i < arrMapURL.Length; i++)
                {
                    if (i == 0)
                    {
                        sPropertyNameresult = arrMapURL[i];
                    }
                    else
                    {
                        sPropertyNameresult += "+" + arrMapURL[i];
                    }
                }
                sPropertyNameresult += "+" + propDetail.sCity;
                propDetail.slargeMapURL = "https://www.google.com/maps/place/" + sPropertyNameresult + "/@" + propDetail.dLatitude + "," + propDetail.dLongitude + ",17z/";


                //Fetch credit card images
                //string[] arrCreditCards = new string[1000];
                //arrCreditCards = objMapping.sCreditCardId.Split(',');
                //List<CreditCards> lstcreditcards = new List<CreditCards>();
                // foreach (var item in arrCreditCards)
                // {
                //     lstcreditcards.Add(new CreditCards()
                //     {
                //         sImg = item
                //     });
                // }
                // objMapping.lstCreditCards = lstcreditcards;


                #endregion

                #region Parking Transport Transfer Available

                if (propDetail.bAirportTransferAvalible == true)
                {
                    List<PropertyParkingTransport> objPropertyParkingTransport = new List<PropertyParkingTransport>();
                    objPropertyParkingTransport = (from s1 in db.tblPropertyParkingMaps
                                                   select new PropertyParkingTransport
                                                   {
                                                       iPropId = s1.iPropId,
                                                       sCarName = s1.sCarName,
                                                       cAirportRail = s1.cAirportRail,
                                                       bIsFree = s1.bIsFree,
                                                       dOnewayCharge = s1.dOnewayCharge,
                                                       dTwowayCharge = s1.dTwowayCharge
                                                   }).Where(u => u.iPropId == propId).ToList();

                    propDetail.lstPropertyParkingTransport = objPropertyParkingTransport;

                }

                #endregion

                #region Dinning and Restaurants
                List<PropertyDiningAndRestaurants> objDining = new List<PropertyDiningAndRestaurants>();
                objDining = (from s1 in db.tblPropertyDiningMaps
                             join s2 in db.tblPropertyMs on s1.iPropId equals s2.iPropId
                             select new PropertyDiningAndRestaurants
                             {
                                 iPropId = s1.iPropId,
                                 sRestaurantName = s1.sRestaurantName,
                                 cType = s1.cType,
                                 sDescription = s1.sDescription,
                                 bBreakfast = s1.bBreakfast,
                                 bLunch = s1.bLunch,
                                 bDinner = s1.bDinner,
                                 //sTimingFromHH = s1.sTimingFromHH,
                                 //sTimingFromMM = s1.sTimingFromMM,
                                 //sTimingToHH = s1.sTimingToHH,
                                 //sTimingToMM = s1.sTimingToMM,
                                 bActive = s1.bActive,
                                 s24hours = s1.b24hours

                             }).Where(u => u.iPropId == propId && u.bActive == true).ToList();

                propDetail.lstPropertyDiningAndRestaurants = objDining;

                #endregion

                #region Wellness Facilites

                PropertyWellnessFacilities objWellness = new PropertyWellnessFacilities();
                objWellness = (from s1 in db.tblPropertySpaMaps
                               join s2 in db.tblPropertyMs on s1.iPropId equals s2.iPropId
                               select new PropertyWellnessFacilities
                               {
                                   iPropId = s1.iPropId,
                                   sSpaName = s1.sSpaName,
                                   sSpaDesc = s1.sSpaDesc,
                                   bHotsprings = s1.bHotsprings,
                                   bSauna = s1.bSauna,
                                   bMudbath = s1.bMudbath,
                                   bSpaTub = s1.bSpaTub,
                                   bAdvancedBooking = s1.bAdvancedBooking,
                                   bSteamRoom = s1.bSteamRoom,
                                   b24hours = s1.b24hours
                               }).Where(u => u.iPropId == propId).SingleOrDefault();

                propDetail.objPropertyWellnessFacilities = objWellness;

                #endregion

                #region Property Recreation (Need to write the query to get the recreation data)
                //TODO:  
                #endregion

                #region DistanceToAirportRailway

                if (propDetail.sDistanceToAirportRailway1 != null && (propDetail.dDistanceToAirportRailway1 != null || propDetail.dDistanceToAirportRailway1 != 0))
                {
                    propDetail.sDistanceToAirportRailway = propDetail.sDistanceToAirportRailway1 + "(" + propDetail.dDistanceToAirportRailway1 + " Km) ";
                }

                if (propDetail.sDistanceToAirportRailway2 != null && (propDetail.dDistanceToAirportRailway2 != null || propDetail.dDistanceToAirportRailway2 != 0))
                {
                    propDetail.sDistanceToAirportRailway = propDetail.sDistanceToAirportRailway + propDetail.sDistanceToAirportRailway2 + "(" + propDetail.dDistanceToAirportRailway2 + " Km) ";

                }

                if (propDetail.sDistanceToAirportRailway3 != null && (propDetail.dDistanceToAirportRailway3 != null || propDetail.dDistanceToAirportRailway3 != 0))
                {
                    propDetail.sDistanceToAirportRailway = propDetail.sDistanceToAirportRailway + propDetail.sDistanceToAirportRailway3 + "(" + propDetail.dDistanceToAirportRailway3 + " Km)";
                }
                #endregion

                #region NearestTransport1

                if (propDetail.sNearestTransport1 != null && (propDetail.dNearestTransport1 != null || propDetail.dNearestTransport1 != 0))
                {
                    propDetail.sNearestTransport = propDetail.sNearestTransport1 + "(" + propDetail.dNearestTransport1 + " Km) ";
                }

                if (propDetail.sNearestTransport2 != null && (propDetail.dNearestTransport2 != null || propDetail.dNearestTransport2 != 0))
                {
                    propDetail.sNearestTransport = propDetail.sNearestTransport + propDetail.sNearestTransport2 + "(" + propDetail.dNearestTransport2 + " Km) ";
                }

                if (propDetail.sNearestTransport3 != null && (propDetail.dNearestTransport3 != null || propDetail.dNearestTransport3 != 0))
                {
                    propDetail.sNearestTransport = propDetail.sNearestTransport + propDetail.sNearestTransport3 + "(" + propDetail.dNearestTransport3 + " Km) ";
                }
                #endregion

                #region TripAdvisorReviews&Ratings


                etblPropertyTripAdvisorM objTripReviews = new etblPropertyTripAdvisorM();
                objTripReviews = (from t1 in db.tblPropertyTripAdvisorMs
                                  select new etblPropertyTripAdvisorM
                                  {
                                      iPropId = t1.iPropId,
                                      iTripAdvisorLOCID = t1.iTripAdvisorLOCID,
                                      sRatingImageURL = t1.sRatingImageURL,
                                      iRating = t1.iRating,
                                      sRankingString = t1.sRankingString,
                                      iReviewsCount = t1.iReviewsCount,
                                      sWebURL = t1.sWebURL,
                                      Review_Rating_Point1 = t1.review_rating1_count,
                                      Review_Rating_Point2 = t1.review_rating2_count,

                                      Review_Rating_Point3 = t1.review_rating3_count,

                                      Review_Rating_Point4 = t1.review_rating4_count,
                                      Review_Rating_Point5 = t1.review_rating5_count,
                                      iLocationRating = t1.iLocationRating,
                                      sLocationRatingURL = t1.sLocationRatingURL,
                                      iSleepQualityRating = t1.iSleepQualityRating,
                                      sSleepQualityRatingURL = t1.sSleepQualityRatingURL,
                                      iRoomsRating = t1.iRoomsRating,
                                      sRoomsRatingURL = t1.sRoomsRatingURL,
                                      iServiceRating = t1.iServiceRating,
                                      sServiceRatingURL = t1.sServiceRatingURL,
                                      iValueRating = t1.iValueRating,
                                      sValueRatingURL = t1.sValueRatingURL,
                                      iCleanlinessRating = t1.iCleanlinessRating,
                                      sCleanlinessRatingURL = t1.sCleanlinessRatingURL,
                                      write_review = t1.write_review
                                  }).Where(u => u.iPropId == propId).SingleOrDefault();
                if (objTripReviews != null)
                {
                    var totalReviewCount = (objTripReviews.Review_Rating_Point1 + objTripReviews.Review_Rating_Point2 + objTripReviews.Review_Rating_Point3 + objTripReviews.Review_Rating_Point4 + objTripReviews.Review_Rating_Point5);
                    objTripReviews.Review_Rating_Point1 = ((objTripReviews.Review_Rating_Point1 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point2 = ((objTripReviews.Review_Rating_Point2 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point3 = ((objTripReviews.Review_Rating_Point3 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point4 = ((objTripReviews.Review_Rating_Point4 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point5 = ((objTripReviews.Review_Rating_Point5 * 100) / totalReviewCount);
                }

                if (objTripReviews == null)
                    objTripReviews = new etblPropertyTripAdvisorM();


                propDetail.objTripAdvisonReviews = objTripReviews;

                List<etblTripAdvisorReviews> lstTripReviews = new List<etblTripAdvisorReviews>();
                lstTripReviews = (from s1 in db.tblTripAdvisorReviews
                                  join s2 in db.tblPropertyTripAdvisorMs on s1.iTripAdvisorLOCID equals s2.iTripAdvisorLOCID
                                  select new etblTripAdvisorReviews
                                  {
                                      iTripAdvisorLOCID = s1.iTripAdvisorLOCID,
                                      iReviewId = s1.iReviewId,
                                      sTitle = s1.sTitle,
                                      sText = s1.sText,
                                      sReviewer = s1.sReviewer,
                                      iRating = s1.iRating,
                                      sRatingImageURL = s1.sRatingImageURL,
                                      sReviewURL = s1.sReviewURL,
                                      dtReviewDate = s1.dtReviewDate,
                                      dtTravelDate = s1.dtTravelDate,
                                      sTripType = s1.sTripType,
                                      sUserLocation = s1.sUserLocation,
                                      iPropId = s2.iPropId,
                                      bIsJointlyCollected = s1.bIsJointlyCollected != null ? s1.bIsJointlyCollected.Value : false
                                      //sDayMonth = String.Format("{0:dd MMM}", s1.dtReviewDate),
                                      //sYear = Convert.ToDateTime(s1.dtReviewDate.ToString()).Year.ToString()
                                  }).Where(u => u.iPropId == propId).ToList();

                propDetail.lstTripAdvisonReviews = lstTripReviews;

                #endregion

                #region
                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@iPropId", propId);
                DataSet dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetAwardLogByPropId", Param);
                if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    etblPropertyAwards award = new etblPropertyAwards();
                    award.sSmallImg = Convert.ToString(dt.Tables[0].Rows[0]["url"]);
                    propDetail.Awards = award;
                }

                #endregion

                #region Rooms Details

                SqlConnection objConn = default(SqlConnection);
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[18];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iHotelID", objSearchDetails.iPropId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtCheckIn", objSearchDetails.dtCheckIn);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@dtCheckOut", objSearchDetails.dtCheckOut);
                MyParams[3] = new System.Data.SqlClient.SqlParameter("@bLogin", objSearchDetails.bLogin);
                MyParams[4] = new System.Data.SqlClient.SqlParameter("@Currency", objSearchDetails.Currency);
                MyParams[5] = new System.Data.SqlClient.SqlParameter("@bIsAirConditioning", objSearchDetails.bIsAirConditioning);
                MyParams[6] = new System.Data.SqlClient.SqlParameter("@bIsBathtub", objSearchDetails.bIsBathtub);
                MyParams[7] = new System.Data.SqlClient.SqlParameter("@bIsFlatScreenTV", objSearchDetails.bIsFlatScreenTV);
                MyParams[8] = new System.Data.SqlClient.SqlParameter("@bIsSoundproof", objSearchDetails.bIsSoundproof);
                MyParams[9] = new System.Data.SqlClient.SqlParameter("@bIsView", objSearchDetails.bIsView);
                MyParams[10] = new System.Data.SqlClient.SqlParameter("@bIsInternetFacilities", objSearchDetails.bIsInternetFacilities);
                MyParams[11] = new System.Data.SqlClient.SqlParameter("@bIsPrivatePool", objSearchDetails.bIsPrivatePool);
                MyParams[12] = new System.Data.SqlClient.SqlParameter("@bIsRoomService", objSearchDetails.bIsRoomService);
                MyParams[13] = new System.Data.SqlClient.SqlParameter("@dMinPrice", objSearchDetails.dMinPrice);
                MyParams[14] = new System.Data.SqlClient.SqlParameter("@dMaxPrice", objSearchDetails.dMaxPrice);
                MyParams[15] = new System.Data.SqlClient.SqlParameter("@SpecialDeal", objSearchDetails.SpecialDeal);
                MyParams[16] = new System.Data.SqlClient.SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                MyParams[17] = new System.Data.SqlClient.SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "[dbo].[uspSearchRoomsForHotel]", MyParams);
                objConn.Close();

                if (ds.Tables.Count > 0)
                {
                    List<etblRooms> objetblRooms = new List<etblRooms>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        List<RoomTaxes> objresultRoomTaxes = new List<RoomTaxes>();

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int lstTaxes = 0; lstTaxes < ds.Tables[1].Rows.Count; lstTaxes++)
                            {
                                if (Convert.ToInt64(ds.Tables[1].Rows[lstTaxes]["iRoomId"]) == Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()))
                                {

                                    int dcount = ds.Tables[1].Select("iRoomId=" + Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()) + "").Length;

                                    if (dcount == 1)
                                    {
                                        objresultRoomTaxes.Add(new RoomTaxes()
                                        {
                                            iRoomId = Convert.ToInt32(ds.Tables[1].Rows[lstTaxes]["iRoomId"]),
                                            ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidFrom"]),
                                            ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidTo"]),
                                            Tax = ds.Tables[1].Rows[lstTaxes]["Tax"].ToString(),
                                            TaxString = ""

                                        });
                                    }
                                    else
                                    {
                                        objresultRoomTaxes.Add(new RoomTaxes()
                                        {
                                            iRoomId = Convert.ToInt32(ds.Tables[1].Rows[lstTaxes]["iRoomId"]),
                                            ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidFrom"]),
                                            ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidTo"]),
                                            Tax = ds.Tables[1].Rows[lstTaxes]["Tax"].ToString(),
                                            TaxString = "(Valid From : " + ds.Tables[1].Rows[lstTaxes]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[1].Rows[lstTaxes]["ValidTo"].ToString().Replace("00:00:00", "") + ") "

                                        });
                                    }
                                }
                            }
                        }

                        objetblRooms.Add(new etblRooms()
                        {
                            iRoomId = Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()),
                            sRoomName = ds.Tables[0].Rows[i]["sRoomName"].ToString(),
                            MaxOccupancy = Convert.ToByte(ds.Tables[0].Rows[i]["MaxOccupancy"].ToString()),
                            TwinBed = Convert.ToInt16(ds.Tables[0].Rows[i]["TwinBed"].ToString()),
                            MaxExtraBeds = Convert.ToByte(ds.Tables[0].Rows[i]["MaxExtraBeds"].ToString()),
                            iAvailableInventory = Convert.ToInt32(ds.Tables[0].Rows[i]["iAvailableInventory"].ToString()),
                            ExtraBedCharges = ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString()),
                            sRoomImgUrl = ds.Tables[0].Rows[i]["sRoomImgUrl"].ToString(),
                            Amen1 = ds.Tables[0].Rows[i]["Amen1"].ToString(),
                            Amen2 = ds.Tables[0].Rows[i]["Amen2"].ToString(),
                            Amen3 = ds.Tables[0].Rows[i]["Amen3"].ToString(),
                            Amen4 = ds.Tables[0].Rows[i]["Amen4"].ToString(),
                            Amen5 = ds.Tables[0].Rows[i]["Amen5"].ToString(),
                            Amen6 = ds.Tables[0].Rows[i]["Amen6"].ToString(),
                            Amen7 = ds.Tables[0].Rows[i]["Amen7"].ToString(),
                            Amen8 = ds.Tables[0].Rows[i]["Amen8"].ToString(),
                            lstRoomTaxes = objresultRoomTaxes
                        });

                    }
                    propDetail.lstetblRooms = objetblRooms;



                    DataTable dtdistinct = ds.Tables[2].AsEnumerable()
                                            .GroupBy(r => new { iRoomId = r["iRoomId"], RPID = r["RPID"], RPlan = r["RatePlan"] })
                                            .Select(g => g.OrderBy(r => r["iRoomId"]).First())
                                            .CopyToDataTable();

                    var DistinctRatePlanList = (from t in dtdistinct.AsEnumerable()
                                                select t).Distinct().ToList();




                    for (int lstRooms = 0; lstRooms < propDetail.lstetblRooms.Count; lstRooms++)
                    {
                        List<etblRatePlans> objresultetblRatePlans = new List<etblRatePlans>();
                        for (int j = 0; j < DistinctRatePlanList.Count; j++)
                        {

                            List<CancellationPolcy> objresultCancellationPolicy = new List<CancellationPolcy>();
                            for (int lstCancellation = 0; lstCancellation < ds.Tables[3].Rows.Count; lstCancellation++)
                            {
                                if (Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]) == Convert.ToInt32(DistinctRatePlanList[j]["RPID"]))
                                {

                                    CancellationPolcy objcancel = new CancellationPolcy();
                                    objcancel.iRPId = Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]);
                                    objcancel.ValidFrom = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidFrom"]);
                                    objcancel.ValidTo = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidTo"]);
                                    int diff = (objcancel.ValidTo - objcancel.ValidFrom).Days;
                                    if (diff > 1)
                                        objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[3].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[3].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ";
                                    else
                                        objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                    objcancel.PolicyName = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                    objcancel.CancellationMsg = ds.Tables[3].Rows[lstCancellation]["CancellationMsg"].ToString();
                                    objresultCancellationPolicy.Add(objcancel);
                                    //objresultCancellationPolicy.Add(new CancellationPolcy()
                                    //{
                                    //    iRPId = Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]),
                                    //    ValidFrom = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidFrom"]),
                                    //    ValidTo = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidTo"]),
                                    //    CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[3].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[3].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ",
                                    //    PolicyName = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString()
                                    //});
                                }
                            }

                            if (Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()) == propDetail.lstetblRooms[lstRooms].iRoomId)
                            {
                                objresultetblRatePlans.Add(new etblRatePlans()
                                {
                                    iRoomId = Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()),
                                    RPID = Convert.ToInt32(DistinctRatePlanList[j]["RPID"].ToString()),
                                    iOccupancy = Convert.ToInt32(DistinctRatePlanList[j]["iOccupancy"]),
                                    RateInclusion = DistinctRatePlanList[j]["RateInclusion"].ToString(),
                                    lstCancellationPolcy = objresultCancellationPolicy,
                                    RatePlan = DistinctRatePlanList[j]["RatePlan"].ToString()
                                });
                            }
                        }
                        propDetail.lstetblRooms[lstRooms].lstRatePlan = objresultetblRatePlans;
                    }


                    for (int m = 0; m < propDetail.lstetblRooms.Count; m++)
                    {
                        for (int n = 0; n < propDetail.lstetblRooms[m].lstRatePlan.Count; n++)
                        {
                            List<etblOccupancy> objresultetblOccupancy = new List<etblOccupancy>();
                            for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                            {

                                List<TaxesDateRoomRateWise> lstRoomTaxes = new List<TaxesDateRoomRateWise>();
                                for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                                {
                                    if (propDetail.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[6].Rows[tax]["RoomID"].ToString())
                                   && propDetail.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[6].Rows[tax]["RPID"].ToString())
                                  && propDetail.lstetblRooms[m].lstRatePlan[n].iOccupancy == Convert.ToInt64(ds.Tables[6].Rows[tax]["iOccupancy"].ToString())
                                 && propDetail.lstetblRooms[m].lstRatePlan[n].RatePlan == ds.Tables[6].Rows[tax]["RatePlan"].ToString())
                                    {
                                        lstRoomTaxes.Add(new TaxesDateRoomRateWise()
                                        {
                                            dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                                            RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : -1,
                                            RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                                            bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                                            iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : -1,
                                            dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                                            TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                                            TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                                        });
                                    }
                                }

                                string[] arr = new string[2];
                                if (propDetail.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"].ToString())
                                    && propDetail.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"].ToString())
                                    //&& propDetail.lstetblRooms[m].lstRatePlan[n].iOccupancy == Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"].ToString())
                                    && propDetail.lstetblRooms[m].lstRatePlan[n].RatePlan == ds.Tables[2].Rows[k]["RatePlan"].ToString())
                                {
                                    arr = ds.Tables[2].Rows[k]["Persons"].ToString().Split(',');
                                    objresultetblOccupancy.Add(new etblOccupancy()
                                    {
                                        iRoomId = ds.Tables[2].Rows[k]["iRoomId"] != DBNull.Value ? Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"]) : -1,
                                        RPID = ds.Tables[2].Rows[k]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"]) : -1,
                                        RatePlan = ds.Tables[2].Rows[k]["RatePlan"] != DBNull.Value ? ds.Tables[2].Rows[k]["RatePlan"].ToString() : null,
                                        iOccupancy = ds.Tables[2].Rows[k]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"]) : -1,
                                        ExtraBeds = ds.Tables[2].Rows[k]["ExtraBeds"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["ExtraBeds"]) : -1,
                                        ExtraBedCharges = ds.Tables[2].Rows[k]["ExtraBedCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["ExtraBedCharges"]) : -1,
                                        dPrice = ds.Tables[2].Rows[k]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPrice"]) : -1,
                                        dBasePrice = ds.Tables[2].Rows[k]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dBasePrice"]) : -1,
                                        dPriceRP = ds.Tables[2].Rows[k]["dPriceRP"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPriceRP"]) : -1,
                                        iAdults = !string.IsNullOrEmpty(arr[0]) ? Convert.ToInt32(arr[0]) : -1,
                                        iChildrens = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : -1,
                                        Cnt = ds.Tables[2].Rows[k]["Cnt"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["Cnt"]) : -1,
                                        iPromoType = ds.Tables[2].Rows[k]["iPromoType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iPromoType"]) : -1,
                                        blsPromo = ds.Tables[2].Rows[k]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[2].Rows[k]["bIsPromo"]) : false,
                                        ChildrenAge = ds.Tables[2].Rows[k]["ChildrenAge"] != DBNull.Value ? ds.Tables[2].Rows[k]["ChildrenAge"].ToString() : null,
                                        dTaxes = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["tax"]) : 0,
                                        dTaxesForHotel = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["tax"]) : 0,
                                        lstTaxesDateWise = lstRoomTaxes//.Where(x=>x.iOccupancy == Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"])).ToList()

                                    });
                                }
                                propDetail.lstetblRooms[m].lstRatePlan[n].lstetblOccupancy = objresultetblOccupancy;
                            }
                        }
                    }

                    List<TaxesDateRoomRateWise> lstRoomTaxes_All = new List<TaxesDateRoomRateWise>();
                    for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                    {

                        lstRoomTaxes_All.Add(new TaxesDateRoomRateWise()
                        {
                            dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                            RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : 0,
                            RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                            bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                            iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : 0,
                            dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                            TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                            TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                        });

                    }
                    List<TaxesSeprateDateRoomRateWise> lstRoomTaxesDateWise_All = new List<TaxesSeprateDateRoomRateWise>();
                    for (int tax = 0; tax < ds.Tables[9].Rows.Count; tax++)
                    {
                        var seperatetax = ds.Tables[9].Rows[tax];

                        lstRoomTaxesDateWise_All.Add(new TaxesSeprateDateRoomRateWise()
                        {
                            dtStay = seperatetax["dtStay"] != DBNull.Value ? Convert.ToString(seperatetax["dtStay"]) : "",
                            iRoomId = seperatetax["iRoomId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iRoomId"]) : 0,
                            RPID = seperatetax["RPID"] != DBNull.Value ? Convert.ToInt32(seperatetax["RPID"]) : -1,
                            sTaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : "",
                            iOccupancy = seperatetax["iOccupancy"] != DBNull.Value ? Convert.ToInt32(seperatetax["iOccupancy"]) : 0,
                            dPrice = seperatetax["dPrice"] != DBNull.Value ? Convert.ToDecimal(seperatetax["dPrice"]) : 0,
                            MaxTaxPer = seperatetax["MaxTaxPer"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxPer"]) : 0,
                            MaxTaxVal = seperatetax["MaxTaxVal"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxVal"]) : 0,
                            TaxId = seperatetax["iTaxId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iTaxId"]) : 0,
                            TaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : ""
                        });

                    }


                    propDetail.lstTaxesDateWise_OfferReview = lstRoomTaxes_All;
                    propDetail.lstTaxesDateWiseAll_OfferReview = lstRoomTaxesDateWise_All;
                    List<OccupancyData> lstOccData = new List<OccupancyData>();
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        lstOccData.Add(new OccupancyData()
                        {
                            Occupancy = Convert.ToInt16(ds.Tables[4].Rows[i]["Occupancy"]),
                            Rooms = Convert.ToInt16(ds.Tables[4].Rows[i]["Rooms"]),
                            Total = 0
                        });
                    }
                    propDetail.lstOccData = lstOccData;

                    propDetail.Symbol = ds.Tables[5].Rows[0].Field<string>("Symbol");
                    propDetail.dCommissionRate = ds.Tables[5].Rows[0].Field<decimal>("CommissionRate");
                    propDetail.sCheckInDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckInDate");
                    propDetail.sCheckOutDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckOutDate");
                    #endregion

                    #region RoomMax Taxes
                    List<etblRoomMaxTaxes> maxTax = new List<etblRoomMaxTaxes>();
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        etblRoomMaxTaxes roomatax = new etblRoomMaxTaxes();
                        roomatax.RoomID = Convert.ToInt32(ds.Tables[7].Rows[i]["RoomID"]);
                        roomatax.TaxPer = Convert.ToString(ds.Tables[7].Rows[i]["TaxPer"]);
                        maxTax.Add(roomatax);
                    }

                    #endregion
                    propDetail.MaxCharges = maxTax;
                    #region Service Charges Date Wise
                    etblTaxCharges matax = new etblTaxCharges();
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        matax.dOFRServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["ServiceCharge"] is DBNull)?"0.0": ds.Tables[8].Rows[i]["ServiceCharge"]);
                        matax.TaxOnServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TaxOnServiceCharge"] is DBNull) ? "0.0": ds.Tables[8].Rows[i]["TaxOnServiceCharge"]);
                        matax.TotalServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TotalServiceCharge"] is DBNull) ? "0.0": ds.Tables[8].Rows[i]["TotalServiceCharge"]);
                        matax.cGstValueType = Convert.ToString(ds.Tables[8].Rows[i]["cGstValueType"]);
                        matax.dGstValue = Convert.ToString(Math.Round(Convert.ToDecimal((ds.Tables[8].Rows[i]["dGstValue"] is DBNull) ? "0.0": ds.Tables[8].Rows[i]["dGstValue"])));
                    }

                    #endregion
                    propDetail.TaxCharges = matax;
                    #region Gallery Details

                    //List<etblPhotoGallery> objgalleryMain = new List<etblPhotoGallery>();
                    //objgalleryMain = (from s in db.tblPropertyPhotoMaps
                    //                  select new etblPhotoGallery()
                    //                  {
                    //                      iPropId = s.iPropId,
                    //                      bIsPrimaryH = s.bIsPrimaryH,
                    //                      sMainImgUrl = s.sImgUrl,
                    //                      sthumbImgUrl = s.sImgUrl
                    //                  }).Where(m => m.iPropId == propId && m.bIsPrimaryH == true).AsQueryable().ToList();

                    //List<etblPhotoGallery> objgalleryThumb = new List<etblPhotoGallery>();
                    //objgalleryThumb = (from s in db.tblPropertyPhotoMaps
                    //                   select new etblPhotoGallery()
                    //                   {
                    //                       iPropId = s.iPropId,
                    //                       bIsPrimaryH = s.bIsPrimaryH,
                    //                       sMainImgUrl = s.sImgUrl,
                    //                       sthumbImgUrl = s.sImgUrl //Path.GetFileNameWithoutExtension(s.UniqueAzureFileName) + "_thumb" + Path.GetExtension(s.UniqueAzureFileName)
                    //                   }).Where(m => m.iPropId == propId && m.bIsPrimaryH == false).AsQueryable().ToList();


                    //foreach (var item in objgalleryThumb)
                    //{
                    //    string[] arr = new string[2];
                    //    arr = item.sthumbImgUrl.Split('/');
                    //    item.sthumbImgUrl = arr[0] + "/" + Path.GetFileNameWithoutExtension(arr[1]) + "_thumb" + Path.GetExtension(arr[1]);
                    //}

                }

                #endregion

                objConn.Open();

                #region Other Details

                SqlParameter[] MyParamNew = new SqlParameter[1];
                MyParamNew[0] = new SqlParameter("@iPropId", propId);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetPropertyPhotos", MyParamNew);
                objConn.Close();
                List<etblPhotoGallery> lstImages = new List<etblPhotoGallery>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstImages.Add(new etblPhotoGallery()
                    {
                        iPropId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPropId"]),
                        bIsPrimaryH = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimaryH"]),
                        bIsPrimaryR = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimaryR"]),
                        sMainImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sMainImgUrl"]),
                        sthumbImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sthumbImgUrl"]),
                        sName = Convert.ToString(ds.Tables[0].Rows[i]["sName"]),
                        iPhotoSubCatId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPhotoSubCatId"])

                    });
                }
                propDetail.lstetblPhotoGallery = lstImages;

                #endregion

                return propDetail;
            }
        }

        public static PropDetailsM GetPropertyDetailsCor(int propId, PropDetailsM objSearchDetails, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch)
        {
            dtRoomOccupancySearch.TableName = "ABC";
            dtChildrenAgeSearch.TableName = "XYZ";
            PropDetailsM objMapping = new PropDetailsM();
            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region General Details
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@PropId", propId);
                var objresult = db.Database.SqlQuery<PropDetailsM>("uspGetOfferReviewDetailsbyID @PropId", MyParam).ToList();
                objMapping = (PropDetailsM)OneFineRateAppUtil.clsUtils.ConvertToObject(objresult[0], objMapping);


                SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                objMapping.lstetblHotelFacilities = objresultHotelFacilities;

                string[] arrMapURL = new string[50];
                string sPropertyNameresult = "";
                arrMapURL = objMapping.sPropertyName.Split(' ');
                for (int i = 0; i < arrMapURL.Length; i++)
                {
                    if (i == 0)
                    {
                        sPropertyNameresult = arrMapURL[i];
                    }
                    else
                    {
                        sPropertyNameresult += "+" + arrMapURL[i];
                    }
                }
                sPropertyNameresult += "+" + objMapping.sCity;
                objMapping.slargeMapURL = "https://www.google.com/maps/place/" + sPropertyNameresult + "/@" + objMapping.dLatitude + "," + objMapping.dLongitude + ",17z/";


                //Fetch credit card images
                //string[] arrCreditCards = new string[1000];
                //arrCreditCards = objMapping.sCreditCardId.Split(',');
                //List<CreditCards> lstcreditcards = new List<CreditCards>();
                // foreach (var item in arrCreditCards)
                // {
                //     lstcreditcards.Add(new CreditCards()
                //     {
                //         sImg = item
                //     });
                // }
                // objMapping.lstCreditCards = lstcreditcards;


                #endregion

                #region Parking Transport Transfer Available

                if (objMapping.bAirportTransferAvalible == true)
                {
                    List<PropertyParkingTransport> objPropertyParkingTransport = new List<PropertyParkingTransport>();
                    objPropertyParkingTransport = (from s1 in db.tblPropertyParkingMaps
                                                   select new PropertyParkingTransport
                                                   {
                                                       iPropId = s1.iPropId,
                                                       sCarName = s1.sCarName,
                                                       cAirportRail = s1.cAirportRail,
                                                       bIsFree = s1.bIsFree,
                                                       dOnewayCharge = s1.dOnewayCharge,
                                                       dTwowayCharge = s1.dTwowayCharge
                                                   }).Where(u => u.iPropId == propId).ToList();

                    objMapping.lstPropertyParkingTransport = objPropertyParkingTransport;

                }

                #endregion

                #region Dinning and Restaurants
                List<PropertyDiningAndRestaurants> objDining = new List<PropertyDiningAndRestaurants>();
                objDining = (from s1 in db.tblPropertyDiningMaps
                             join s2 in db.tblPropertyMs on s1.iPropId equals s2.iPropId
                             select new PropertyDiningAndRestaurants
                             {
                                 iPropId = s1.iPropId,
                                 sRestaurantName = s1.sRestaurantName,
                                 cType = s1.cType,
                                 sDescription = s1.sDescription,
                                 bBreakfast = s1.bBreakfast,
                                 bLunch = s1.bLunch,
                                 bDinner = s1.bDinner,
                                 //sTimingFromHH = s1.sTimingFromHH,
                                 //sTimingFromMM = s1.sTimingFromMM,
                                 //sTimingToHH = s1.sTimingToHH,
                                 //sTimingToMM = s1.sTimingToMM,
                                 bActive = s1.bActive,
                                 s24hours = s1.b24hours

                             }).Where(u => u.iPropId == propId && u.bActive == true).ToList();

                objMapping.lstPropertyDiningAndRestaurants = objDining;

                #endregion

                #region Wellness Facilites

                PropertyWellnessFacilities objWellness = new PropertyWellnessFacilities();
                objWellness = (from s1 in db.tblPropertySpaMaps
                               join s2 in db.tblPropertyMs on s1.iPropId equals s2.iPropId
                               select new PropertyWellnessFacilities
                               {
                                   iPropId = s1.iPropId,
                                   sSpaName = s1.sSpaName,
                                   sSpaDesc = s1.sSpaDesc,
                                   bHotsprings = s1.bHotsprings,
                                   bSauna = s1.bSauna,
                                   bMudbath = s1.bMudbath,
                                   bSpaTub = s1.bSpaTub,
                                   bAdvancedBooking = s1.bAdvancedBooking,
                                   bSteamRoom = s1.bSteamRoom,
                                   b24hours = s1.b24hours
                               }).Where(u => u.iPropId == propId).SingleOrDefault();

                objMapping.objPropertyWellnessFacilities = objWellness;

                #endregion

                #region DistanceToAirportRailway

                if (objMapping.sDistanceToAirportRailway1 != null && (objMapping.dDistanceToAirportRailway1 != null || objMapping.dDistanceToAirportRailway1 != 0))
                {
                    objMapping.sDistanceToAirportRailway = objMapping.sDistanceToAirportRailway1 + "(" + objMapping.dDistanceToAirportRailway1 + " Km) ";
                }

                if (objMapping.sDistanceToAirportRailway2 != null && (objMapping.dDistanceToAirportRailway2 != null || objMapping.dDistanceToAirportRailway2 != 0))
                {
                    objMapping.sDistanceToAirportRailway = objMapping.sDistanceToAirportRailway + objMapping.sDistanceToAirportRailway2 + "(" + objMapping.dDistanceToAirportRailway2 + " Km) ";

                }

                if (objMapping.sDistanceToAirportRailway3 != null && (objMapping.dDistanceToAirportRailway3 != null || objMapping.dDistanceToAirportRailway3 != 0))
                {
                    objMapping.sDistanceToAirportRailway = objMapping.sDistanceToAirportRailway + objMapping.sDistanceToAirportRailway3 + "(" + objMapping.dDistanceToAirportRailway3 + " Km)";
                }
                #endregion

                #region NearestTransport1

                if (objMapping.sNearestTransport1 != null && (objMapping.dNearestTransport1 != null || objMapping.dNearestTransport1 != 0))
                {
                    objMapping.sNearestTransport = objMapping.sNearestTransport1 + "(" + objMapping.dNearestTransport1 + " Km) ";
                }

                if (objMapping.sNearestTransport2 != null && (objMapping.dNearestTransport2 != null || objMapping.dNearestTransport2 != 0))
                {
                    objMapping.sNearestTransport = objMapping.sNearestTransport + objMapping.sNearestTransport2 + "(" + objMapping.dNearestTransport2 + " Km) ";
                }

                if (objMapping.sNearestTransport3 != null && (objMapping.dNearestTransport3 != null || objMapping.dNearestTransport3 != 0))
                {
                    objMapping.sNearestTransport = objMapping.sNearestTransport + objMapping.sNearestTransport3 + "(" + objMapping.dNearestTransport3 + " Km) ";
                }
                #endregion

                #region TripAdvisorReviews&Ratings


                etblPropertyTripAdvisorM objTripReviews = new etblPropertyTripAdvisorM();
                objTripReviews = (from t1 in db.tblPropertyTripAdvisorMs
                                  select new etblPropertyTripAdvisorM
                                  {
                                      iPropId = t1.iPropId,
                                      iTripAdvisorLOCID = t1.iTripAdvisorLOCID,
                                      sRatingImageURL = t1.sRatingImageURL,
                                      iRating = t1.iRating,
                                      sRankingString = t1.sRankingString,
                                      iReviewsCount = t1.iReviewsCount,
                                      sWebURL = t1.sWebURL,
                                      Review_Rating_Point1 = t1.review_rating1_count,
                                      Review_Rating_Point2 = t1.review_rating2_count,

                                      Review_Rating_Point3 = t1.review_rating3_count,

                                      Review_Rating_Point4 = t1.review_rating4_count,
                                      Review_Rating_Point5 = t1.review_rating5_count,
                                      iLocationRating = t1.iLocationRating,
                                      sLocationRatingURL = t1.sLocationRatingURL,
                                      iSleepQualityRating = t1.iSleepQualityRating,
                                      sSleepQualityRatingURL = t1.sSleepQualityRatingURL,
                                      iRoomsRating = t1.iRoomsRating,
                                      sRoomsRatingURL = t1.sRoomsRatingURL,
                                      iServiceRating = t1.iServiceRating,
                                      sServiceRatingURL = t1.sServiceRatingURL,
                                      iValueRating = t1.iValueRating,
                                      sValueRatingURL = t1.sValueRatingURL,
                                      iCleanlinessRating = t1.iCleanlinessRating,
                                      sCleanlinessRatingURL = t1.sCleanlinessRatingURL,
                                      write_review = t1.write_review
                                  }).Where(u => u.iPropId == propId).SingleOrDefault();
                if (objTripReviews != null)
                {
                    var totalReviewCount = (objTripReviews.Review_Rating_Point1 + objTripReviews.Review_Rating_Point2 + objTripReviews.Review_Rating_Point3 + objTripReviews.Review_Rating_Point4 + objTripReviews.Review_Rating_Point5);
                    objTripReviews.Review_Rating_Point1 = ((objTripReviews.Review_Rating_Point1 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point2 = ((objTripReviews.Review_Rating_Point2 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point3 = ((objTripReviews.Review_Rating_Point3 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point4 = ((objTripReviews.Review_Rating_Point4 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point5 = ((objTripReviews.Review_Rating_Point5 * 100) / totalReviewCount);
                }

                if (objTripReviews == null)
                    objTripReviews = new etblPropertyTripAdvisorM();


                objMapping.objTripAdvisonReviews = objTripReviews;

                List<etblTripAdvisorReviews> lstTripReviews = new List<etblTripAdvisorReviews>();
                lstTripReviews = (from s1 in db.tblTripAdvisorReviews
                                  join s2 in db.tblPropertyTripAdvisorMs on s1.iTripAdvisorLOCID equals s2.iTripAdvisorLOCID
                                  select new etblTripAdvisorReviews
                                  {
                                      iTripAdvisorLOCID = s1.iTripAdvisorLOCID,
                                      iReviewId = s1.iReviewId,
                                      sTitle = s1.sTitle,
                                      sText = s1.sText,
                                      sReviewer = s1.sReviewer,
                                      iRating = s1.iRating,
                                      sRatingImageURL = s1.sRatingImageURL,
                                      sReviewURL = s1.sReviewURL,
                                      dtReviewDate = s1.dtReviewDate,
                                      dtTravelDate = s1.dtTravelDate,
                                      sTripType = s1.sTripType,
                                      sUserLocation = s1.sUserLocation,
                                      iPropId = s2.iPropId,
                                      bIsJointlyCollected = s1.bIsJointlyCollected != null ? s1.bIsJointlyCollected.Value : false
                                      //sDayMonth = String.Format("{0:dd MMM}", s1.dtReviewDate),
                                      //sYear = Convert.ToDateTime(s1.dtReviewDate.ToString()).Year.ToString()
                                  }).Where(u => u.iPropId == propId).ToList();

                objMapping.lstTripAdvisonReviews = lstTripReviews;

                #endregion

                #region
                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@iPropId", propId);
                DataSet dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetAwardLogByPropId", Param);
                if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    etblPropertyAwards award = new etblPropertyAwards();
                    award.sSmallImg = Convert.ToString(dt.Tables[0].Rows[0]["url"]);
                    objMapping.Awards = award;
                }

                #endregion

                #region Rooms Details

                SqlConnection objConn = default(SqlConnection);
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[18];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iHotelID", objSearchDetails.iPropId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtCheckIn", objSearchDetails.dtCheckIn);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@dtCheckOut", objSearchDetails.dtCheckOut);
                MyParams[3] = new System.Data.SqlClient.SqlParameter("@bLogin", objSearchDetails.bLogin);
                MyParams[4] = new System.Data.SqlClient.SqlParameter("@Currency", objSearchDetails.Currency);
                MyParams[5] = new System.Data.SqlClient.SqlParameter("@bIsAirConditioning", objSearchDetails.bIsAirConditioning);
                MyParams[6] = new System.Data.SqlClient.SqlParameter("@bIsBathtub", objSearchDetails.bIsBathtub);
                MyParams[7] = new System.Data.SqlClient.SqlParameter("@bIsFlatScreenTV", objSearchDetails.bIsFlatScreenTV);
                MyParams[8] = new System.Data.SqlClient.SqlParameter("@bIsSoundproof", objSearchDetails.bIsSoundproof);
                MyParams[9] = new System.Data.SqlClient.SqlParameter("@bIsView", objSearchDetails.bIsView);
                MyParams[10] = new System.Data.SqlClient.SqlParameter("@bIsInternetFacilities", objSearchDetails.bIsInternetFacilities);
                MyParams[11] = new System.Data.SqlClient.SqlParameter("@bIsPrivatePool", objSearchDetails.bIsPrivatePool);
                MyParams[12] = new System.Data.SqlClient.SqlParameter("@bIsRoomService", objSearchDetails.bIsRoomService);
                MyParams[13] = new System.Data.SqlClient.SqlParameter("@dMinPrice", objSearchDetails.dMinPrice);
                MyParams[14] = new System.Data.SqlClient.SqlParameter("@dMaxPrice", objSearchDetails.dMaxPrice);
                MyParams[15] = new System.Data.SqlClient.SqlParameter("@SpecialDeal", objSearchDetails.SpecialDeal);
                MyParams[16] = new System.Data.SqlClient.SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                MyParams[17] = new System.Data.SqlClient.SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSearchRoomsForHotelCorp", MyParams);
                objConn.Close();

                if (ds.Tables.Count > 0)
                {
                    List<etblRooms> objetblRooms = new List<etblRooms>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        List<RoomTaxes> objresultRoomTaxes = new List<RoomTaxes>();

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int lstTaxes = 0; lstTaxes < ds.Tables[1].Rows.Count; lstTaxes++)
                            {
                                if (Convert.ToInt64(ds.Tables[1].Rows[lstTaxes]["iRoomId"]) == Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()))
                                {

                                    int dcount = ds.Tables[1].Select("iRoomId=" + Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()) + "").Length;

                                    if (dcount == 1)
                                    {
                                        objresultRoomTaxes.Add(new RoomTaxes()
                                        {
                                            iRoomId = Convert.ToInt32(ds.Tables[1].Rows[lstTaxes]["iRoomId"]),
                                            ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidFrom"]),
                                            ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidTo"]),
                                            Tax = ds.Tables[1].Rows[lstTaxes]["Tax"].ToString(),
                                            TaxString = ""

                                        });
                                    }
                                    else
                                    {
                                        objresultRoomTaxes.Add(new RoomTaxes()
                                        {
                                            iRoomId = Convert.ToInt32(ds.Tables[1].Rows[lstTaxes]["iRoomId"]),
                                            ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidFrom"]),
                                            ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidTo"]),
                                            Tax = ds.Tables[1].Rows[lstTaxes]["Tax"].ToString(),
                                            TaxString = "(Valid From : " + ds.Tables[1].Rows[lstTaxes]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[1].Rows[lstTaxes]["ValidTo"].ToString().Replace("00:00:00", "") + ") "

                                        });
                                    }



                                }
                            }
                        }

                        objetblRooms.Add(new etblRooms()
                        {
                            iRoomId = Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()),
                            sRoomName = ds.Tables[0].Rows[i]["sRoomName"].ToString(),
                            MaxOccupancy = Convert.ToByte(ds.Tables[0].Rows[i]["MaxOccupancy"].ToString()),
                            TwinBed = Convert.ToInt16(ds.Tables[0].Rows[i]["TwinBed"].ToString()),
                            MaxExtraBeds = Convert.ToByte(ds.Tables[0].Rows[i]["MaxExtraBeds"].ToString()),
                            iAvailableInventory = Convert.ToInt32(ds.Tables[0].Rows[i]["iAvailableInventory"].ToString()),
                            ExtraBedCharges = ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString()),
                            sRoomImgUrl = ds.Tables[0].Rows[i]["sRoomImgUrl"].ToString(),
                            Amen1 = ds.Tables[0].Rows[i]["Amen1"].ToString(),
                            Amen2 = ds.Tables[0].Rows[i]["Amen2"].ToString(),
                            Amen3 = ds.Tables[0].Rows[i]["Amen3"].ToString(),
                            Amen4 = ds.Tables[0].Rows[i]["Amen4"].ToString(),
                            Amen5 = ds.Tables[0].Rows[i]["Amen5"].ToString(),
                            Amen6 = ds.Tables[0].Rows[i]["Amen6"].ToString(),
                            Amen7 = ds.Tables[0].Rows[i]["Amen7"].ToString(),
                            Amen8 = ds.Tables[0].Rows[i]["Amen8"].ToString(),
                            lstRoomTaxes = objresultRoomTaxes
                        });

                    }

                    objMapping.lstetblRooms = objetblRooms;


                    DataTable dtdistinct = ds.Tables[2].AsEnumerable()
                                            .GroupBy(r => new { iRoomId = r["iRoomId"], RPID = r["RPID"] })
                                            .Select(g => g.OrderBy(r => r["iRoomId"]).First())
                                            .CopyToDataTable();

                    var DistinctRatePlanList = (from t in dtdistinct.AsEnumerable()
                                                select t).Distinct().ToList();


                    for (int lstRooms = 0; lstRooms < objMapping.lstetblRooms.Count; lstRooms++)
                    {
                        List<etblRatePlans> objresultetblRatePlans = new List<etblRatePlans>();
                        for (int j = 0; j < DistinctRatePlanList.Count; j++)
                        {

                            List<CancellationPolcy> objresultCancellationPolicy = new List<CancellationPolcy>();
                            for (int lstCancellation = 0; lstCancellation < ds.Tables[3].Rows.Count; lstCancellation++)
                            {
                                if (Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]) == Convert.ToInt32(DistinctRatePlanList[j]["RPID"]))
                                {

                                    CancellationPolcy objcancel = new CancellationPolcy();
                                    objcancel.iRPId = Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]);
                                    objcancel.ValidFrom = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidFrom"]);
                                    objcancel.ValidTo = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidTo"]);
                                    int diff = (objcancel.ValidTo - objcancel.ValidFrom).Days;
                                    if (diff > 1)
                                        objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[3].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[3].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ";
                                    else
                                        objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                    objcancel.PolicyName = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                    objcancel.CancellationMsg = ds.Tables[3].Rows[lstCancellation]["CancellationMsg"].ToString();
                                    objresultCancellationPolicy.Add(objcancel);
                                    //objresultCancellationPolicy.Add(new CancellationPolcy()
                                    //{
                                    //    iRPId = Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]),
                                    //    ValidFrom = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidFrom"]),
                                    //    ValidTo = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidTo"]),
                                    //    CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[3].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[3].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ",
                                    //    PolicyName = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString()
                                    //});
                                }
                            }

                            if (Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()) == objMapping.lstetblRooms[lstRooms].iRoomId)
                            {
                                objresultetblRatePlans.Add(new etblRatePlans()
                                {
                                    iRoomId = Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()),
                                    RPID = Convert.ToInt32(DistinctRatePlanList[j]["RPID"].ToString()),
                                    iOccupancy = Convert.ToInt32(DistinctRatePlanList[j]["iOccupancy"]),
                                    RateInclusion = DistinctRatePlanList[j]["RateInclusion"].ToString(),
                                    lstCancellationPolcy = objresultCancellationPolicy
                                });
                            }
                        }
                        objMapping.lstetblRooms[lstRooms].lstRatePlan = objresultetblRatePlans;
                    }


                    for (int m = 0; m < objMapping.lstetblRooms.Count; m++)
                    {
                        for (int n = 0; n < objMapping.lstetblRooms[m].lstRatePlan.Count; n++)
                        {
                            List<etblOccupancy> objresultetblOccupancy = new List<etblOccupancy>();
                            for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                            {

                                List<TaxesDateRoomRateWise> lstRoomTaxes = new List<TaxesDateRoomRateWise>();
                                for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                                {
                                    if (objMapping.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[6].Rows[tax]["RoomID"].ToString())
                                   && objMapping.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[6].Rows[tax]["RPID"].ToString())
                                   && objMapping.lstetblRooms[m].lstRatePlan[n].iOccupancy == Convert.ToInt64(ds.Tables[6].Rows[tax]["iOccupancy"].ToString()))
                                    {
                                        lstRoomTaxes.Add(new TaxesDateRoomRateWise()
                                        {
                                            dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                                            RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : -1,
                                            RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                                            bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                                            iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : -1,
                                            dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                                            TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                                            TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                                        });
                                    }
                                }

                                string[] arr = new string[2];
                                if (objMapping.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"].ToString())
                                    && objMapping.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[2].Rows[k]["RPID"].ToString()))
                                {
                                    arr = ds.Tables[2].Rows[k]["Persons"].ToString().Split(',');
                                    objresultetblOccupancy.Add(new etblOccupancy()
                                    {
                                        iRoomId = ds.Tables[2].Rows[k]["iRoomId"] != DBNull.Value ? Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"]) : -1,
                                        RPID = ds.Tables[2].Rows[k]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"]) : -1,
                                        RatePlan = ds.Tables[2].Rows[k]["RatePlan"] != DBNull.Value ? ds.Tables[2].Rows[k]["RatePlan"].ToString() : null,
                                        iOccupancy = ds.Tables[2].Rows[k]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"]) : -1,
                                        ExtraBeds = ds.Tables[2].Rows[k]["ExtraBeds"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["ExtraBeds"]) : -1,
                                        ExtraBedCharges = ds.Tables[2].Rows[k]["ExtraBedCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["ExtraBedCharges"]) : -1,
                                        dPrice = ds.Tables[2].Rows[k]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPriceWithoutExtraBed"]) : -1,
                                        dBasePrice = ds.Tables[2].Rows[k]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dBasePrice"]) : -1,
                                        dPriceRP = ds.Tables[2].Rows[k]["dPriceRP"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPriceRP"]) : -1,
                                        iAdults = !string.IsNullOrEmpty(arr[0]) ? Convert.ToInt32(arr[0]) : -1,
                                        iChildrens = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : -1,
                                        Cnt = ds.Tables[2].Rows[k]["Cnt"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["Cnt"]) : -1,
                                        iPromoType = ds.Tables[2].Rows[k]["iPromoType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iPromoType"]) : -1,
                                        blsPromo = ds.Tables[2].Rows[k]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[2].Rows[k]["bIsPromo"]) : false,
                                        ChildrenAge = ds.Tables[2].Rows[k]["ChildrenAge"] != DBNull.Value ? ds.Tables[2].Rows[k]["ChildrenAge"].ToString() : null,
                                        dTaxes = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["tax"]) : 0,
                                        dTaxesForHotel = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["tax"]) : 0,
                                        lstTaxesDateWise = lstRoomTaxes

                                    });



                                }
                                objMapping.lstetblRooms[m].lstRatePlan[n].lstetblOccupancy = objresultetblOccupancy;
                            }
                        }
                    }

                    List<TaxesDateRoomRateWise> lstRoomTaxes_All = new List<TaxesDateRoomRateWise>();
                    for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                    {

                        lstRoomTaxes_All.Add(new TaxesDateRoomRateWise()
                        {
                            dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                            RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : 0,
                            RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                            bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                            iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : 0,
                            dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                            TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                            TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                        });

                    }

                    List<TaxesSeprateDateRoomRateWise> lstRoomTaxesDateWise_All = new List<TaxesSeprateDateRoomRateWise>();
                    for (int tax = 0; tax < ds.Tables[9].Rows.Count; tax++)
                    {
                        var seperatetax = ds.Tables[9].Rows[tax];

                        lstRoomTaxesDateWise_All.Add(new TaxesSeprateDateRoomRateWise()
                        {
                            dtStay = seperatetax["dtStay"] != DBNull.Value ? Convert.ToString(seperatetax["dtStay"]) : "",
                            iRoomId = seperatetax["iRoomId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iRoomId"]) : 0,
                            RPID = seperatetax["RPID"] != DBNull.Value ? Convert.ToInt32(seperatetax["RPID"]) : -1,
                            sTaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : "",
                            iOccupancy = seperatetax["iOccupancy"] != DBNull.Value ? Convert.ToInt32(seperatetax["iOccupancy"]) : 0,
                            dPrice = seperatetax["dPrice"] != DBNull.Value ? Convert.ToDecimal(seperatetax["dPrice"]) : 0,
                            MaxTaxPer = seperatetax["MaxTaxPer"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxPer"]) : 0,
                            MaxTaxVal = seperatetax["MaxTaxVal"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxVal"]) : 0,
                            TaxId = seperatetax["iTaxId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iTaxId"]) : 0

                        });

                    }


                    objMapping.lstTaxesDateWiseAll_OfferReview = lstRoomTaxesDateWise_All;
                    objMapping.lstTaxesDateWise_OfferReview = lstRoomTaxes_All;
                    List<OccupancyData> lstOccData = new List<OccupancyData>();
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        lstOccData.Add(new OccupancyData()
                        {
                            Occupancy = Convert.ToInt16(ds.Tables[4].Rows[i]["Occupancy"]),
                            Rooms = Convert.ToInt16(ds.Tables[4].Rows[i]["Rooms"]),
                            Total = 0
                        });
                    }
                    objMapping.lstOccData = lstOccData;

                    objMapping.Symbol = ds.Tables[5].Rows[0].Field<string>("Symbol");
                    objMapping.dCommissionRate = ds.Tables[5].Rows[0].Field<decimal>("CommissionRate");
                    objMapping.sCheckInDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckInDate");
                    objMapping.sCheckOutDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckOutDate");


                }

                #endregion
                #region RoomMax Taxes

                List<etblRoomMaxTaxes> lstMaxTax = new List<etblRoomMaxTaxes>();
                etblTaxCharges maxTax = new etblTaxCharges();

                if (ds.Tables.Count > 7 && ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        etblRoomMaxTaxes roomatax = new etblRoomMaxTaxes();
                        roomatax.RoomID = Convert.ToInt32(ds.Tables[7].Rows[i]["RoomID"]);
                        roomatax.TaxPer = Convert.ToString(ds.Tables[7].Rows[i]["TaxPer"]);
                        lstMaxTax.Add(roomatax);
                    }

                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        maxTax.dOFRServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["ServiceCharge"] is DBNull) ? "0.0" : ds.Tables[8].Rows[i]["ServiceCharge"]);
                        maxTax.TaxOnServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TaxOnServiceCharge"] is DBNull) ? "0.0" : ds.Tables[8].Rows[i]["TaxOnServiceCharge"]);
                        maxTax.TotalServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TotalServiceCharge"] is DBNull)? "0.0": ds.Tables[8].Rows[i]["TotalServiceCharge"]);
                        maxTax.cGstValueType = Convert.ToString(ds.Tables[8].Rows[i]["cGstValueType"]);
                        maxTax.dGstValue = Convert.ToString(Math.Round(Convert.ToDecimal((ds.Tables[8].Rows[i]["dGstValue"] is DBNull)? "0.0": ds.Tables[8].Rows[i]["dGstValue"])));
                    }
                }

                #endregion
                objMapping.MaxCharges = lstMaxTax;

                objMapping.TaxCharges = maxTax;

                objConn.Open();

                #region Other Details

                SqlParameter[] MyParamNew = new SqlParameter[1];
                MyParamNew[0] = new SqlParameter("@iPropId", propId);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetPropertyPhotos", MyParamNew);
                objConn.Close();
                List<etblPhotoGallery> lstImages = new List<etblPhotoGallery>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstImages.Add(new etblPhotoGallery()
                    {
                        iPropId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPropId"]),
                        bIsPrimaryH = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimaryH"]),
                        bIsPrimaryR = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimaryR"]),
                        sMainImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sMainImgUrl"]),
                        sthumbImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sthumbImgUrl"]),
                        sName = Convert.ToString(ds.Tables[0].Rows[i]["sName"]),
                        iPhotoSubCatId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPhotoSubCatId"])

                    });
                }
                objMapping.lstetblPhotoGallery = lstImages;

                #endregion

                return objMapping;
            }
        }

        public static PropDetailsM GetPropertyDetailsToRedeemPoints(int propId, PropDetailsM objSearchDetails, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch)
        {
            PropDetailsM _propDetails = new PropDetailsM();

            List<etblHotelFacilities> _hotelFacilities = new List<etblHotelFacilities>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                #region General Details

                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@PropId", propId);
                var objresult = db.Database.SqlQuery<PropDetailsM>("uspGetOfferReviewDetailsbyID @PropId", MyParam).ToList();
                _propDetails = (PropDetailsM)OneFineRateAppUtil.clsUtils.ConvertToObject(objresult[0], _propDetails);


                SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                _hotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                _propDetails.lstetblHotelFacilities = _hotelFacilities;

                string[] arrMapURL = new string[50];
                string sPropertyNameresult = "";
                arrMapURL = _propDetails.sPropertyName.Split(' ');

                for (int i = 0; i < arrMapURL.Length; i++)
                {
                    if (i == 0)
                    {
                        sPropertyNameresult = arrMapURL[i];
                    }
                    else
                    {
                        sPropertyNameresult += "+" + arrMapURL[i];
                    }
                }

                sPropertyNameresult += "+" + _propDetails.sCity;
                _propDetails.slargeMapURL = "https://www.google.com/maps/place/" + sPropertyNameresult + "/@" + _propDetails.dLatitude + "," + _propDetails.dLongitude + ",17z/";

                #endregion General Details

                #region Parking Transport Transfer Available

                if (_propDetails.bAirportTransferAvalible == true)
                {
                    List<PropertyParkingTransport> objPropertyParkingTransport = new List<PropertyParkingTransport>();
                    objPropertyParkingTransport = (from s1 in db.tblPropertyParkingMaps
                                                   select new PropertyParkingTransport
                                                   {
                                                       iPropId = s1.iPropId,
                                                       sCarName = s1.sCarName,
                                                       cAirportRail = s1.cAirportRail,
                                                       bIsFree = s1.bIsFree,
                                                       dOnewayCharge = s1.dOnewayCharge,
                                                       dTwowayCharge = s1.dTwowayCharge
                                                   }).Where(u => u.iPropId == propId).ToList();

                    _propDetails.lstPropertyParkingTransport = objPropertyParkingTransport;
                }

                #endregion Parking Transport Transfer Available

                #region Dinning and Restaurants

                List<PropertyDiningAndRestaurants> objDining = new List<PropertyDiningAndRestaurants>();

                objDining = (from s1 in db.tblPropertyDiningMaps
                             join s2 in db.tblPropertyMs on s1.iPropId equals s2.iPropId
                             select new PropertyDiningAndRestaurants
                             {
                                 iPropId = s1.iPropId,
                                 sRestaurantName = s1.sRestaurantName,
                                 cType = s1.cType,
                                 sDescription = s1.sDescription,
                                 bBreakfast = s1.bBreakfast,
                                 bLunch = s1.bLunch,
                                 bDinner = s1.bDinner,
                                 bActive = s1.bActive,
                                 s24hours = s1.b24hours

                             }).Where(u => u.iPropId == propId && u.bActive == true).ToList();

                _propDetails.lstPropertyDiningAndRestaurants = objDining;

                #endregion Dinning and Restaurants

                #region Wellness Facilites

                PropertyWellnessFacilities objWellness = new PropertyWellnessFacilities();
                objWellness = (from s1 in db.tblPropertySpaMaps
                               join s2 in db.tblPropertyMs on s1.iPropId equals s2.iPropId
                               select new PropertyWellnessFacilities
                               {
                                   iPropId = s1.iPropId,
                                   sSpaName = s1.sSpaName,
                                   sSpaDesc = s1.sSpaDesc,
                                   bHotsprings = s1.bHotsprings,
                                   bSauna = s1.bSauna,
                                   bMudbath = s1.bMudbath,
                                   bSpaTub = s1.bSpaTub,
                                   bAdvancedBooking = s1.bAdvancedBooking,
                                   bSteamRoom = s1.bSteamRoom,
                                   b24hours = s1.b24hours
                               }).Where(u => u.iPropId == propId).SingleOrDefault();

                _propDetails.objPropertyWellnessFacilities = objWellness;

                #endregion Wellness Facilites

                #region DistanceToAirportRailway

                if (_propDetails.sDistanceToAirportRailway1 != null && (_propDetails.dDistanceToAirportRailway1 != null || _propDetails.dDistanceToAirportRailway1 != 0))
                {
                    _propDetails.sDistanceToAirportRailway = _propDetails.sDistanceToAirportRailway1 + "(" + _propDetails.dDistanceToAirportRailway1 + " Km) ";
                }

                if (_propDetails.sDistanceToAirportRailway2 != null && (_propDetails.dDistanceToAirportRailway2 != null || _propDetails.dDistanceToAirportRailway2 != 0))
                {
                    _propDetails.sDistanceToAirportRailway = _propDetails.sDistanceToAirportRailway + _propDetails.sDistanceToAirportRailway2 + "(" + _propDetails.dDistanceToAirportRailway2 + " Km) ";
                }

                if (_propDetails.sDistanceToAirportRailway3 != null && (_propDetails.dDistanceToAirportRailway3 != null || _propDetails.dDistanceToAirportRailway3 != 0))
                {
                    _propDetails.sDistanceToAirportRailway = _propDetails.sDistanceToAirportRailway + _propDetails.sDistanceToAirportRailway3 + "(" + _propDetails.dDistanceToAirportRailway3 + " Km)";
                }
                #endregion

                #region NearestTransport1

                if (_propDetails.sNearestTransport1 != null && (_propDetails.dNearestTransport1 != null || _propDetails.dNearestTransport1 != 0))
                {
                    _propDetails.sNearestTransport = _propDetails.sNearestTransport1 + "(" + _propDetails.dNearestTransport1 + " Km) ";
                }

                if (_propDetails.sNearestTransport2 != null && (_propDetails.dNearestTransport2 != null || _propDetails.dNearestTransport2 != 0))
                {
                    _propDetails.sNearestTransport = _propDetails.sNearestTransport + _propDetails.sNearestTransport2 + "(" + _propDetails.dNearestTransport2 + " Km) ";
                }

                if (_propDetails.sNearestTransport3 != null && (_propDetails.dNearestTransport3 != null || _propDetails.dNearestTransport3 != 0))
                {
                    _propDetails.sNearestTransport = _propDetails.sNearestTransport + _propDetails.sNearestTransport3 + "(" + _propDetails.dNearestTransport3 + " Km) ";
                }
                #endregion

                #region TripAdvisorReviews&Ratings


                etblPropertyTripAdvisorM objTripReviews = new etblPropertyTripAdvisorM();
                objTripReviews = (from t1 in db.tblPropertyTripAdvisorMs
                                  select new etblPropertyTripAdvisorM
                                  {
                                      iPropId = t1.iPropId,
                                      iTripAdvisorLOCID = t1.iTripAdvisorLOCID,
                                      sRatingImageURL = t1.sRatingImageURL,
                                      iRating = t1.iRating,
                                      sRankingString = t1.sRankingString,
                                      iReviewsCount = t1.iReviewsCount,
                                      sWebURL = t1.sWebURL,
                                      Review_Rating_Point1 = t1.review_rating1_count,
                                      Review_Rating_Point2 = t1.review_rating2_count,
                                      Review_Rating_Point3 = t1.review_rating3_count,
                                      Review_Rating_Point4 = t1.review_rating4_count,
                                      Review_Rating_Point5 = t1.review_rating5_count,
                                      iLocationRating = t1.iLocationRating,
                                      sLocationRatingURL = t1.sLocationRatingURL,
                                      iSleepQualityRating = t1.iSleepQualityRating,
                                      sSleepQualityRatingURL = t1.sSleepQualityRatingURL,
                                      iRoomsRating = t1.iRoomsRating,
                                      sRoomsRatingURL = t1.sRoomsRatingURL,
                                      iServiceRating = t1.iServiceRating,
                                      sServiceRatingURL = t1.sServiceRatingURL,
                                      iValueRating = t1.iValueRating,
                                      sValueRatingURL = t1.sValueRatingURL,
                                      iCleanlinessRating = t1.iCleanlinessRating,
                                      sCleanlinessRatingURL = t1.sCleanlinessRatingURL,
                                      write_review = t1.write_review
                                  }).Where(u => u.iPropId == propId).SingleOrDefault();

                if (objTripReviews != null)
                {
                    var totalReviewCount = (objTripReviews.Review_Rating_Point1 + objTripReviews.Review_Rating_Point2 + objTripReviews.Review_Rating_Point3 + objTripReviews.Review_Rating_Point4 + objTripReviews.Review_Rating_Point5);
                    objTripReviews.Review_Rating_Point1 = ((objTripReviews.Review_Rating_Point1 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point2 = ((objTripReviews.Review_Rating_Point2 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point3 = ((objTripReviews.Review_Rating_Point3 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point4 = ((objTripReviews.Review_Rating_Point4 * 100) / totalReviewCount);
                    objTripReviews.Review_Rating_Point5 = ((objTripReviews.Review_Rating_Point5 * 100) / totalReviewCount);
                }

                _propDetails.objTripAdvisonReviews = objTripReviews ?? new etblPropertyTripAdvisorM();

                List<etblTripAdvisorReviews> lstTripReviews = new List<etblTripAdvisorReviews>();

                lstTripReviews = (from s1 in db.tblTripAdvisorReviews
                                  join s2 in db.tblPropertyTripAdvisorMs on s1.iTripAdvisorLOCID equals s2.iTripAdvisorLOCID
                                  select new etblTripAdvisorReviews
                                  {
                                      iTripAdvisorLOCID = s1.iTripAdvisorLOCID,
                                      iReviewId = s1.iReviewId,
                                      sTitle = s1.sTitle,
                                      sText = s1.sText,
                                      sReviewer = s1.sReviewer,
                                      iRating = s1.iRating,
                                      sRatingImageURL = s1.sRatingImageURL,
                                      sReviewURL = s1.sReviewURL,
                                      dtReviewDate = s1.dtReviewDate,
                                      dtTravelDate = s1.dtTravelDate,
                                      sTripType = s1.sTripType,
                                      sUserLocation = s1.sUserLocation,
                                      iPropId = s2.iPropId,
                                      bIsJointlyCollected = s1.bIsJointlyCollected != null ? s1.bIsJointlyCollected.Value : false
                                  }).Where(u => u.iPropId == propId).ToList();

                _propDetails.lstTripAdvisonReviews = lstTripReviews;

                #endregion

                #region Awards

                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@iPropId", propId);
                DataSet dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetAwardLogByPropId", Param);
                if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    etblPropertyAwards award = new etblPropertyAwards();
                    award.sSmallImg = Convert.ToString(dt.Tables[0].Rows[0]["url"]);
                    _propDetails.Awards = award;
                }

                #endregion Awards


                DataSet ds = new DataSet();


                using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    #region Other Details

                    SqlParameter[] MyParamNew = new SqlParameter[1];
                    MyParamNew[0] = new SqlParameter("@iPropId", propId);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetPropertyPhotos", MyParamNew);

                    List<etblPhotoGallery> lstImages = new List<etblPhotoGallery>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        lstImages.Add(new etblPhotoGallery()
                        {
                            iPropId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPropId"]),
                            bIsPrimaryH = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimaryH"]),
                            bIsPrimaryR = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimaryR"]),
                            sMainImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sMainImgUrl"]),
                            sthumbImgUrl = Convert.ToString(ds.Tables[0].Rows[i]["sthumbImgUrl"]),
                            sName = Convert.ToString(ds.Tables[0].Rows[i]["sName"]),
                            iPhotoSubCatId = Convert.ToInt32(ds.Tables[0].Rows[i]["iPhotoSubCatId"])
                        });
                    }

                    _propDetails.lstetblPhotoGallery = lstImages;

                    #endregion Other Details

                    #region Rooms Details

                    SqlParameter[] sqlParams = new SqlParameter[7];

                    sqlParams[0] = new SqlParameter("@iHotelID", objSearchDetails.iPropId);
                    sqlParams[1] = new SqlParameter("@dtCheckIn", objSearchDetails.dtCheckIn);
                    sqlParams[2] = new SqlParameter("@dtCheckOut", objSearchDetails.dtCheckOut);
                    sqlParams[3] = new SqlParameter("@iCustomerId", objSearchDetails.iUserId);
                    sqlParams[4] = new SqlParameter("@Currency", objSearchDetails.Currency);
                    sqlParams[5] = new SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                    sqlParams[6] = new SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);

                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "[dbo].[uspSearchRoomsForHotelForRedemption]", sqlParams);

                    if (ds.Tables.Count > 1)
                    {
                        List<etblRooms> objetblRooms = new List<etblRooms>();

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            List<RoomTaxes> objresultRoomTaxes = new List<RoomTaxes>();

                            objetblRooms.Add(new etblRooms()
                            {
                                iRoomId = Convert.ToInt64(ds.Tables[1].Rows[i]["iRoomId"].ToString()),
                                sRoomName = ds.Tables[1].Rows[i]["sRoomName"].ToString(),
                                MaxOccupancy = Convert.ToByte(ds.Tables[1].Rows[i]["MaxOccupancy"].ToString()),
                                TwinBed = Convert.ToInt16(ds.Tables[1].Rows[i]["TwinBed"].ToString()),
                                MaxExtraBeds = Convert.ToByte(ds.Tables[1].Rows[i]["MaxExtraBeds"].ToString()),
                                iAvailableInventory = Convert.ToInt32(ds.Tables[1].Rows[i]["iAvailableInventory"].ToString()),
                                ExtraBedCharges = ds.Tables[1].Rows[i]["ExtraBedCharges"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["ExtraBedCharges"].ToString()),
                                sRoomImgUrl = ds.Tables[1].Rows[i]["sRoomImgUrl"].ToString(),
                                Amen1 = ds.Tables[1].Rows[i]["Amen1"].ToString(),
                                Amen2 = ds.Tables[1].Rows[i]["Amen2"].ToString(),
                                Amen3 = ds.Tables[1].Rows[i]["Amen3"].ToString(),
                                Amen4 = ds.Tables[1].Rows[i]["Amen4"].ToString(),
                                Amen5 = ds.Tables[1].Rows[i]["Amen5"].ToString(),
                                Amen6 = ds.Tables[1].Rows[i]["Amen6"].ToString(),
                                Amen7 = ds.Tables[1].Rows[i]["Amen7"].ToString(),
                                Amen8 = ds.Tables[1].Rows[i]["Amen8"].ToString(),
                                lstRoomTaxes = objresultRoomTaxes
                            });
                        }

                        _propDetails.lstetblRooms = objetblRooms;

                        DataTable dtdistinct = ds.Tables[2].AsEnumerable()
                                                .GroupBy(r => new { iRoomId = r["iRoomId"], RPID = r["RPID"], RPlan = r["RatePlan"] })
                                                .Select(g => g.OrderBy(r => r["iRoomId"]).First())
                                                .CopyToDataTable();

                        var DistinctRatePlanList = (from t in dtdistinct.AsEnumerable() select t).Distinct().ToList();

                        for (int lstRooms = 0; lstRooms < _propDetails.lstetblRooms.Count; lstRooms++)
                        {
                            List<etblRatePlans> objresultetblRatePlans = new List<etblRatePlans>();

                            for (int j = 0; j < DistinctRatePlanList.Count; j++)
                            {
                                List<CancellationPolcy> objresultCancellationPolicy = new List<CancellationPolcy>();
                                for (int lstCancellation = 0; lstCancellation < ds.Tables[3].Rows.Count; lstCancellation++)
                                {
                                    if (Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]) == Convert.ToInt32(DistinctRatePlanList[j]["RPID"]))
                                    {
                                        CancellationPolcy objcancel = new CancellationPolcy();
                                        objcancel.iRPId = Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]);
                                        objcancel.ValidFrom = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidFrom"]);
                                        objcancel.ValidTo = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidTo"]);
                                        int diff = (objcancel.ValidTo - objcancel.ValidFrom).Days;
                                        if (diff > 1)
                                            objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[3].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[3].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ";
                                        else
                                            objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                        objcancel.PolicyName = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                        objcancel.CancellationMsg = ds.Tables[3].Rows[lstCancellation]["CancellationMsg"].ToString();
                                        objresultCancellationPolicy.Add(objcancel);
                                    }
                                }

                                if (Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()) == _propDetails.lstetblRooms[lstRooms].iRoomId)
                                {
                                    objresultetblRatePlans.Add(new etblRatePlans()
                                    {
                                        iRoomId = Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()),
                                        RPID = Convert.ToInt32(DistinctRatePlanList[j]["RPID"].ToString()),
                                        iOccupancy = Convert.ToInt32(DistinctRatePlanList[j]["iOccupancy"]),
                                        RateInclusion = DistinctRatePlanList[j]["RateInclusion"].ToString(),
                                        lstCancellationPolcy = objresultCancellationPolicy,
                                        RatePlan = DistinctRatePlanList[j]["RatePlan"].ToString()
                                    });
                                }
                            }

                            _propDetails.lstetblRooms[lstRooms].lstRatePlan = objresultetblRatePlans;
                        }


                        for (int m = 0; m < _propDetails.lstetblRooms.Count; m++)
                        {
                            for (int n = 0; n < _propDetails.lstetblRooms[m].lstRatePlan.Count; n++)
                            {
                                List<etblOccupancy> objresultetblOccupancy = new List<etblOccupancy>();
                                for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                                {
                                    List<TaxesDateRoomRateWise> lstRoomTaxes = new List<TaxesDateRoomRateWise>();
                                    for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                                    {
                                        if (_propDetails.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[6].Rows[tax]["RoomID"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[6].Rows[tax]["RPID"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].iOccupancy == Convert.ToInt64(ds.Tables[6].Rows[tax]["iOccupancy"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RatePlan == ds.Tables[6].Rows[tax]["RatePlan"].ToString())
                                        {
                                            lstRoomTaxes.Add(new TaxesDateRoomRateWise()
                                            {
                                                dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                                                RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : -1,
                                                RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                                                bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                                                iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : -1,
                                                dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                                                TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                                                TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                                            });
                                        }
                                    }

                                    string[] arr = new string[2];
                                    if (_propDetails.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RatePlan == ds.Tables[2].Rows[k]["RatePlan"].ToString())
                                    {
                                        arr = ds.Tables[2].Rows[k]["Persons"].ToString().Split(',');
                                        objresultetblOccupancy.Add(new etblOccupancy()
                                        {
                                            iRoomId = ds.Tables[2].Rows[k]["iRoomId"] != DBNull.Value ? Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"]) : -1,
                                            RPID = ds.Tables[2].Rows[k]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"]) : -1,
                                            RatePlan = ds.Tables[2].Rows[k]["RatePlan"] != DBNull.Value ? ds.Tables[2].Rows[k]["RatePlan"].ToString() : null,
                                            iOccupancy = ds.Tables[2].Rows[k]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"]) : -1,
                                            ExtraBeds = ds.Tables[2].Rows[k]["ExtraBeds"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["ExtraBeds"]) : -1,
                                            ExtraBedCharges = ds.Tables[2].Rows[k]["ExtraBedCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["ExtraBedCharges"]) : -1,
                                            dPrice = ds.Tables[2].Rows[k]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPrice"]) : -1,
                                            iPoints = ds.Tables[2].Rows[k]["Points"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["Points"]) : 0,
                                            dBasePrice = ds.Tables[2].Rows[k]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dBasePrice"]) : -1,
                                            dPriceRP = ds.Tables[2].Rows[k]["dPriceRP"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPriceRP"]) : -1,
                                            iAdults = !string.IsNullOrEmpty(arr[0]) ? Convert.ToInt32(arr[0]) : -1,
                                            iChildrens = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : -1,
                                            Cnt = ds.Tables[2].Rows[k]["Cnt"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["Cnt"]) : -1,
                                            iPromoType = ds.Tables[2].Rows[k]["iPromoType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iPromoType"]) : -1,
                                            blsPromo = ds.Tables[2].Rows[k]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[2].Rows[k]["bIsPromo"]) : false,
                                            ChildrenAge = ds.Tables[2].Rows[k]["ChildrenAge"] != DBNull.Value ? ds.Tables[2].Rows[k]["ChildrenAge"].ToString() : null,
                                            dTaxes = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["tax"]) : 0,
                                            dTaxesForHotel = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["tax"]) : 0,
                                            lstTaxesDateWise = lstRoomTaxes

                                        });
                                    }

                                    _propDetails.lstetblRooms[m].lstRatePlan[n].lstetblOccupancy = objresultetblOccupancy;
                                }
                            }
                        }

                        List<TaxesDateRoomRateWise> lstRoomTaxes_All = new List<TaxesDateRoomRateWise>();

                        for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                        {
                            lstRoomTaxes_All.Add(new TaxesDateRoomRateWise()
                            {
                                dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                                RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : 0,
                                RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                                bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                                iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : 0,
                                dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                                TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                                TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                            });
                        }

                        List<TaxesSeprateDateRoomRateWise> lstRoomTaxesDateWise_All = new List<TaxesSeprateDateRoomRateWise>();

                        for (int tax = 0; tax < ds.Tables[9].Rows.Count; tax++)
                        {
                            var seperatetax = ds.Tables[9].Rows[tax];

                            lstRoomTaxesDateWise_All.Add(new TaxesSeprateDateRoomRateWise()
                            {
                                dtStay = seperatetax["dtStay"] != DBNull.Value ? Convert.ToString(seperatetax["dtStay"]) : "",
                                iRoomId = seperatetax["iRoomId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iRoomId"]) : 0,
                                RPID = seperatetax["RPID"] != DBNull.Value ? Convert.ToInt32(seperatetax["RPID"]) : -1,
                                sTaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : "",
                                iOccupancy = seperatetax["iOccupancy"] != DBNull.Value ? Convert.ToInt32(seperatetax["iOccupancy"]) : 0,
                                dPrice = seperatetax["dPrice"] != DBNull.Value ? Convert.ToDecimal(seperatetax["dPrice"]) : 0,
                                MaxTaxPer = seperatetax["MaxTaxPer"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxPer"]) : 0,
                                MaxTaxVal = seperatetax["MaxTaxVal"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxVal"]) : 0,
                                TaxId = seperatetax["iTaxId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iTaxId"]) : 0,
                                TaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : ""
                            });
                        }

                        _propDetails.lstTaxesDateWise_OfferReview = lstRoomTaxes_All;
                        _propDetails.lstTaxesDateWiseAll_OfferReview = lstRoomTaxesDateWise_All;
                        List<OccupancyData> lstOccData = new List<OccupancyData>();

                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            lstOccData.Add(new OccupancyData()
                            {
                                Occupancy = Convert.ToInt16(ds.Tables[4].Rows[i]["Occupancy"]),
                                Rooms = Convert.ToInt16(ds.Tables[4].Rows[i]["Rooms"]),
                                Total = 0
                            });
                        }

                        _propDetails.lstOccData = lstOccData;

                        _propDetails.Symbol = ds.Tables[5].Rows[0].Field<string>("Symbol");
                        _propDetails.dCommissionRate = ds.Tables[5].Rows[0].Field<decimal>("CommissionRate");
                        _propDetails.sCheckInDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckInDate");
                        _propDetails.sCheckOutDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckOutDate");

                        #region RoomMax Taxes

                        List<etblRoomMaxTaxes> maxTax = new List<etblRoomMaxTaxes>();
                        for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                        {
                            etblRoomMaxTaxes roomatax = new etblRoomMaxTaxes();
                            roomatax.RoomID = Convert.ToInt32(ds.Tables[7].Rows[i]["RoomID"]);
                            roomatax.TaxPer = Convert.ToString(ds.Tables[7].Rows[i]["TaxPer"]);
                            maxTax.Add(roomatax);
                        }
                        _propDetails.MaxCharges = maxTax;

                        #endregion

                        #region Service Charges Date Wise

                        etblTaxCharges matax = new etblTaxCharges();

                        for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                        {
                            matax.dOFRServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["ServiceCharge"] is DBNull)?"0.0": ds.Tables[8].Rows[i]["ServiceCharge"]);
                            matax.TaxOnServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TaxOnServiceCharge"] is DBNull)?"0.0": ds.Tables[8].Rows[i]["TaxOnServiceCharge"]);
                            matax.TotalServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TotalServiceCharge"] is DBNull)?"0.0": ds.Tables[8].Rows[i]["TotalServiceCharge"]);
                            matax.cGstValueType = Convert.ToString(ds.Tables[8].Rows[i]["cGstValueType"]);
                            matax.dGstValue = Convert.ToString(Math.Round(Convert.ToDecimal((ds.Tables[8].Rows[i]["dGstValue"] is DBNull)?"0.0": ds.Tables[8].Rows[i]["dGstValue"])));
                        }
                        _propDetails.TaxCharges = matax;

                        #endregion

                    }

                    #endregion Rooms Details

                    return _propDetails;
                }
            }
        }

        public static PropDetailsM GetRoomReviewDetailsToRedeemPoints(int propId, long customerId, string checkIn, string checkOut, string currency,
            int roomId, int ratePlanId, bool isPromo, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch)
        {
            PropDetailsM _propDetails = null;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                DataSet ds = new DataSet();

                using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    #region Rooms Details

                    SqlParameter[] sqlParams = new SqlParameter[10];

                    sqlParams[0] = new SqlParameter("@iHotelID", propId);
                    sqlParams[1] = new SqlParameter("@dtCheckIn", checkIn);
                    sqlParams[2] = new SqlParameter("@dtCheckOut", checkOut);
                    sqlParams[3] = new SqlParameter("@iCustomerId", customerId);
                    sqlParams[4] = new SqlParameter("@iRPId", ratePlanId);
                    sqlParams[5] = new SqlParameter("@iRoomId", roomId);
                    sqlParams[6] = new SqlParameter("@bIsPromo", isPromo);                    
                    sqlParams[7] = new SqlParameter("@Currency", currency);
                    sqlParams[8] = new SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                    sqlParams[9] = new SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);                    

                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "[dbo].[uspPreviewOfSelectedRoomForRedemption]", sqlParams);

                    if (ds.Tables.Count > 0)
                    {
                        _propDetails = new PropDetailsM();
                        

                        SqlParameter[] sqlHotelFacilitiesParams = new SqlParameter[1];
                        sqlHotelFacilitiesParams[0] = new SqlParameter("@PropId", propId);
                        var facilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", sqlHotelFacilitiesParams).ToList();
                        _propDetails.lstetblHotelFacilities = facilities;


                        List<etblRooms> objetblRooms = new List<etblRooms>();

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            List<RoomTaxes> objresultRoomTaxes = new List<RoomTaxes>();

                            objetblRooms.Add(new etblRooms()
                            {
                                iRoomId = Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()),
                                sRoomName = ds.Tables[0].Rows[i]["sRoomName"].ToString(),
                                MaxOccupancy = Convert.ToByte(ds.Tables[0].Rows[i]["MaxOccupancy"].ToString()),                                
                                TwinBed = Convert.ToInt16(ds.Tables[0].Rows[i]["TwinBed"].ToString()),
                                MaxExtraBeds = Convert.ToByte(ds.Tables[0].Rows[i]["MaxExtraBeds"].ToString()),
                                iAvailableInventory = Convert.ToInt32(ds.Tables[0].Rows[i]["iAvailableInventory"].ToString()),
                                ExtraBedCharges = ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString()),
                                sRoomImgUrl = ds.Tables[0].Rows[i]["sRoomImgUrl"].ToString(),
                                Amen1 = ds.Tables[0].Rows[i]["Amen1"].ToString(),
                                Amen2 = ds.Tables[0].Rows[i]["Amen2"].ToString(),
                                Amen3 = ds.Tables[0].Rows[i]["Amen3"].ToString(),
                                Amen4 = ds.Tables[0].Rows[i]["Amen4"].ToString(),
                                Amen5 = ds.Tables[0].Rows[i]["Amen5"].ToString(),
                                Amen6 = ds.Tables[0].Rows[i]["Amen6"].ToString(),
                                Amen7 = ds.Tables[0].Rows[i]["Amen7"].ToString(),
                                Amen8 = ds.Tables[0].Rows[i]["Amen8"].ToString(),
                                lstRoomTaxes = objresultRoomTaxes
                            });
                        }

                        _propDetails.lstetblRooms = objetblRooms;

                        DataTable dtdistinct = ds.Tables[1].AsEnumerable()
                                                .GroupBy(r => new { iRoomId = r["iRoomId"], RPID = r["RPID"], RPlan = r["RatePlan"] })
                                                .Select(g => g.OrderBy(r => r["iRoomId"]).First())
                                                .CopyToDataTable();

                        var DistinctRatePlanList = (from t in dtdistinct.AsEnumerable() select t).Distinct().ToList();

                        for (int lstRooms = 0; lstRooms < _propDetails.lstetblRooms.Count; lstRooms++)
                        {
                            List<etblRatePlans> objresultetblRatePlans = new List<etblRatePlans>();

                            for (int j = 0; j < DistinctRatePlanList.Count; j++)
                            {
                                List<CancellationPolcy> objresultCancellationPolicy = new List<CancellationPolcy>();
                                for (int lstCancellation = 0; lstCancellation < ds.Tables[2].Rows.Count; lstCancellation++)
                                {
                                    if (Convert.ToInt32(ds.Tables[2].Rows[lstCancellation]["iRPId"]) == Convert.ToInt32(DistinctRatePlanList[j]["RPID"]))
                                    {
                                        CancellationPolcy objcancel = new CancellationPolcy();
                                        objcancel.iRPId = Convert.ToInt32(ds.Tables[2].Rows[lstCancellation]["iRPId"]);
                                        objcancel.ValidFrom = Convert.ToDateTime(ds.Tables[2].Rows[lstCancellation]["ValidFrom"]);
                                        objcancel.ValidTo = Convert.ToDateTime(ds.Tables[2].Rows[lstCancellation]["ValidTo"]);
                                        int diff = (objcancel.ValidTo - objcancel.ValidFrom).Days;
                                        if (diff > 1)
                                            objcancel.CancellationPolicy = ds.Tables[2].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[2].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[2].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ";
                                        else
                                            objcancel.CancellationPolicy = ds.Tables[2].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                        objcancel.PolicyName = ds.Tables[2].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                        objcancel.CancellationMsg = ds.Tables[2].Rows[lstCancellation]["CancellationMsg"].ToString();
                                        objresultCancellationPolicy.Add(objcancel);
                                    }
                                }

                                if (Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()) == _propDetails.lstetblRooms[lstRooms].iRoomId)
                                {
                                    objresultetblRatePlans.Add(new etblRatePlans()
                                    {
                                        iRoomId = Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()),
                                        RPID = Convert.ToInt32(DistinctRatePlanList[j]["RPID"].ToString()),
                                        iOccupancy = Convert.ToInt32(DistinctRatePlanList[j]["iOccupancy"]),
                                        RateInclusion = DistinctRatePlanList[j]["RateInclusion"].ToString(),
                                        lstCancellationPolcy = objresultCancellationPolicy,
                                        RatePlan = DistinctRatePlanList[j]["RatePlan"].ToString()
                                    });
                                }
                            }

                            _propDetails.lstetblRooms[lstRooms].lstRatePlan = objresultetblRatePlans;
                        }

                        for (int m = 0; m < _propDetails.lstetblRooms.Count; m++)
                        {
                            for (int n = 0; n < _propDetails.lstetblRooms[m].lstRatePlan.Count; n++)
                            {
                                List<etblOccupancy> objresultetblOccupancy = new List<etblOccupancy>();
                                for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                                {
                                    List<TaxesDateRoomRateWise> lstRoomTaxes = new List<TaxesDateRoomRateWise>();
                                    for (int tax = 0; tax < ds.Tables[5].Rows.Count; tax++)
                                    {
                                        if (_propDetails.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[5].Rows[tax]["RoomID"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[5].Rows[tax]["RPID"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].iOccupancy == Convert.ToInt64(ds.Tables[5].Rows[tax]["iOccupancy"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RatePlan == ds.Tables[5].Rows[tax]["RatePlan"].ToString())
                                        {
                                            lstRoomTaxes.Add(new TaxesDateRoomRateWise()
                                            {
                                                dtDate = ds.Tables[5].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[5].Rows[tax]["dtDate"]) : DateTime.Now,
                                                RoomID = ds.Tables[5].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[tax]["RoomID"]) : -1,
                                                RPID = ds.Tables[5].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[tax]["RPID"]) : -1,
                                                bIsPromo = ds.Tables[5].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[5].Rows[tax]["bIsPromo"]) : false,
                                                iOccupancy = ds.Tables[5].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[tax]["iOccupancy"]) : -1,
                                                dBasePrice = ds.Tables[5].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[5].Rows[tax]["dBasePrice"]) : 0,
                                                TaxPer = ds.Tables[5].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[5].Rows[tax]["TaxPer"]) : 0,
                                                TaxVal = ds.Tables[5].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[5].Rows[tax]["TaxVal"]) : 0                                                
                                            });
                                        }
                                    }

                                    string[] arr = new string[2];
                                    if (_propDetails.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[1].Rows[k]["iRoomId"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt32(ds.Tables[1].Rows[k]["RPID"].ToString())
                                        && _propDetails.lstetblRooms[m].lstRatePlan[n].RatePlan == ds.Tables[1].Rows[k]["RatePlan"].ToString())
                                    {
                                        arr = ds.Tables[1].Rows[k]["Persons"].ToString().Split(',');
                                        objresultetblOccupancy.Add(new etblOccupancy()
                                        {
                                            iRoomId = ds.Tables[1].Rows[k]["iRoomId"] != DBNull.Value ? Convert.ToInt64(ds.Tables[1].Rows[k]["iRoomId"]) : -1,
                                            RPID = ds.Tables[1].Rows[k]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[k]["RPID"]) : -1,
                                            RatePlan = ds.Tables[1].Rows[k]["RatePlan"] != DBNull.Value ? ds.Tables[1].Rows[k]["RatePlan"].ToString() : null,
                                            iOccupancy = ds.Tables[1].Rows[k]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[k]["iOccupancy"]) : -1,
                                            ExtraBeds = ds.Tables[1].Rows[k]["ExtraBeds"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[k]["ExtraBeds"]) : -1,
                                            ExtraBedCharges = ds.Tables[1].Rows[k]["ExtraBedCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[k]["ExtraBedCharges"]) : -1,
                                            dPrice = ds.Tables[1].Rows[k]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[k]["dPrice"]) : -1,
                                            iPoints = ds.Tables[1].Rows[k]["Points"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[k]["Points"]) : 0,
                                            dBasePrice = ds.Tables[1].Rows[k]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[k]["dBasePrice"]) : -1,
                                            dPriceRP = ds.Tables[1].Rows[k]["dPriceRP"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[k]["dPriceRP"]) : -1,
                                            iAdults = !string.IsNullOrEmpty(arr[0]) ? Convert.ToInt32(arr[0]) : -1,
                                            iChildrens = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : -1,
                                            Cnt = ds.Tables[1].Rows[k]["Cnt"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[k]["Cnt"]) : -1,
                                            iPromoType = ds.Tables[1].Rows[k]["iPromoType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[k]["iPromoType"]) : -1,
                                            blsPromo = ds.Tables[1].Rows[k]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[k]["bIsPromo"]) : false,
                                            ChildrenAge = ds.Tables[1].Rows[k]["ChildrenAge"] != DBNull.Value ? ds.Tables[1].Rows[k]["ChildrenAge"].ToString() : null,
                                            dTaxes = ds.Tables[1].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[k]["tax"]) : 0,
                                            dTaxesForHotel = ds.Tables[1].Rows[k]["tax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[k]["tax"]) : 0,
                                            lstTaxesDateWise = lstRoomTaxes,
                                            
                                            // Setting Static value as 1 as Only Room is applicable in reward points redeem.
                                            iNoOfRooms = 1
                                            
                                        });
                                    }

                                    _propDetails.lstetblRooms[m].lstRatePlan[n].lstetblOccupancy = objresultetblOccupancy;
                                }
                            }
                        }

                        List<TaxesDateRoomRateWise> lstRoomTaxes_All = new List<TaxesDateRoomRateWise>();

                        for (int tax = 0; tax < ds.Tables[5].Rows.Count; tax++)
                        {
                            lstRoomTaxes_All.Add(new TaxesDateRoomRateWise()
                            {
                                dtDate = ds.Tables[5].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[5].Rows[tax]["dtDate"]) : DateTime.Now,
                                RoomID = ds.Tables[5].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[tax]["RoomID"]) : 0,
                                RPID = ds.Tables[5].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[tax]["RPID"]) : -1,
                                bIsPromo = ds.Tables[5].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[5].Rows[tax]["bIsPromo"]) : false,
                                iOccupancy = ds.Tables[5].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[tax]["iOccupancy"]) : 0,
                                dBasePrice = ds.Tables[5].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[5].Rows[tax]["dBasePrice"]) : 0,
                                TaxPer = ds.Tables[5].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[5].Rows[tax]["TaxPer"]) : 0,
                                TaxVal = ds.Tables[5].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[5].Rows[tax]["TaxVal"]) : 0
                            });
                        }

                        List<TaxesSeprateDateRoomRateWise> lstRoomTaxesDateWise_All = new List<TaxesSeprateDateRoomRateWise>();

                        for (int tax = 0; tax < ds.Tables[8].Rows.Count; tax++)
                        {
                            var seperatetax = ds.Tables[8].Rows[tax];

                            lstRoomTaxesDateWise_All.Add(new TaxesSeprateDateRoomRateWise()
                            {
                                dtStay = seperatetax["dtStay"] != DBNull.Value ? Convert.ToString(seperatetax["dtStay"]) : "",
                                iRoomId = seperatetax["iRoomId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iRoomId"]) : 0,
                                RPID = seperatetax["RPID"] != DBNull.Value ? Convert.ToInt32(seperatetax["RPID"]) : -1,
                                sTaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : "",
                                iOccupancy = seperatetax["iOccupancy"] != DBNull.Value ? Convert.ToInt32(seperatetax["iOccupancy"]) : 0,
                                dPrice = seperatetax["dPrice"] != DBNull.Value ? Convert.ToDecimal(seperatetax["dPrice"]) : 0,
                                MaxTaxPer = seperatetax["MaxTaxPer"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxPer"]) : 0,
                                MaxTaxVal = seperatetax["MaxTaxVal"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxVal"]) : 0,
                                TaxId = seperatetax["iTaxId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iTaxId"]) : 0,
                                TaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : ""
                            });
                        }

                        _propDetails.lstTaxesDateWise_OfferReview = lstRoomTaxes_All;
                        _propDetails.lstTaxesDateWiseAll_OfferReview = lstRoomTaxesDateWise_All;
                        List<OccupancyData> lstOccData = new List<OccupancyData>();

                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            lstOccData.Add(new OccupancyData()
                            {
                                Occupancy = Convert.ToInt16(ds.Tables[3].Rows[i]["Occupancy"]),
                                Rooms = Convert.ToInt16(ds.Tables[3].Rows[i]["Rooms"]),
                                Total = 0
                            });
                        }

                        _propDetails.lstOccData = lstOccData;

                        _propDetails.Symbol = ds.Tables[4].Rows[0].Field<string>("Symbol");
                        _propDetails.dCommissionRate = ds.Tables[4].Rows[0].Field<decimal>("CommissionRate");
                        _propDetails.sCheckInDate_Display = ds.Tables[4].Rows[0].Field<string>("sCheckInDate");
                        _propDetails.sCheckOutDate_Display = ds.Tables[4].Rows[0].Field<string>("sCheckOutDate");
                        _propDetails.sPropertyName = ds.Tables[4].Rows[0].Field<string>("HotelName");
                        _propDetails.sImageUrlTG = ds.Tables[4].Rows[0].Field<string>("sHotelImageUrl");
                        _propDetails.RatingImageUrl = ds.Tables[4].Rows[0].Field<string>("sRatingImageUrl");
                        _propDetails.sPropertyAddress = ds.Tables[4].Rows[0].Field<string>("sAddress");
                        _propDetails.iStarCategory = ds.Tables[4].Rows[0].Field<short>("iStarCategoryId");
                        _propDetails.sTripAdvisorRankingString = ds.Tables[4].Rows[0].Field<string>("sRankingString");
                        
                        #region RoomMax Taxes

                        List<etblRoomMaxTaxes> maxTax = new List<etblRoomMaxTaxes>();
                        for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                        {
                            etblRoomMaxTaxes roomatax = new etblRoomMaxTaxes();
                            roomatax.RoomID = Convert.ToInt32(ds.Tables[6].Rows[i]["RoomID"]);
                            roomatax.TaxPer = Convert.ToString(ds.Tables[6].Rows[i]["TaxPer"]);
                            maxTax.Add(roomatax);
                        }
                        _propDetails.MaxCharges = maxTax;

                        #endregion

                        #region Service Charges Date Wise

                        etblTaxCharges matax = new etblTaxCharges();

                        for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                        {
                            matax.dOFRServiceCharge = Convert.ToDecimal((ds.Tables[7].Rows[i]["ServiceCharge"] is DBNull) ? "0.0" : ds.Tables[7].Rows[i]["ServiceCharge"]);
                            matax.TaxOnServiceCharge = Convert.ToDecimal((ds.Tables[7].Rows[i]["TaxOnServiceCharge"] is DBNull) ? "0.0" : ds.Tables[7].Rows[i]["TaxOnServiceCharge"]);
                            matax.TotalServiceCharge = Convert.ToDecimal((ds.Tables[7].Rows[i]["TotalServiceCharge"] is DBNull) ? "0.0" : ds.Tables[7].Rows[i]["TotalServiceCharge"]);
                            matax.cGstValueType = Convert.ToString(ds.Tables[7].Rows[i]["cGstValueType"]);
                            matax.dGstValue = Convert.ToString(Math.Round(Convert.ToDecimal((ds.Tables[7].Rows[i]["dGstValue"] is DBNull) ? "0.0" : ds.Tables[7].Rows[i]["dGstValue"])));
                        }
                        _propDetails.TaxCharges = matax;

                        #endregion
                    }

                    #endregion Rooms Details

                    return _propDetails;
                }
            }
        }


        public static Dictionary<string, string> GetEmail_PhoneByPropId(int propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var model = db.tblPropertyMs.Where(x => x.iPropId == propId).Select(x => new { x.sRevenueManagerContact, x.sPrimaryContactEmail, x.sRevenueManagerEmail, x.sConfirmationContactEmail, x.sConfirmationContactContact }).FirstOrDefault();

                Dictionary<string, string> details = new Dictionary<string, string>();
                details.Add("sPrimaryContactEmail", model.sPrimaryContactEmail);
                details.Add("sRevenueManagerEmail", model.sRevenueManagerEmail);
                details.Add("sConfirmationContactEmail", model.sConfirmationContactEmail);
                details.Add("sRevenueManagerContact", model.sRevenueManagerContact);
                details.Add("sConfirmationContact", model.sConfirmationContactContact);
                return details;
            }
        }
        public static PropDetailsM GetUserDetails(int bookingId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var userData = (from booking in db.tblBookingTxes
                                join user in db.tblWebsiteUserMaters on booking.iCustomerId equals user.Id
                                join propAdvisorM in db.tblPropertyTripAdvisorMs on booking.iPropId equals propAdvisorM.iPropId
                                where booking.iBookingId == bookingId
                                select new PropDetailsM
                                {
                                    sUserFirstName = user.FirstName,
                                    sUserLastName = user.LastName,
                                    sCity = user.City,
                                    LoctionId = propAdvisorM.iTripAdvisorLOCID.Value,
                                    sUserEmail = user.Email
                                }
                                  ).SingleOrDefault();
                return userData;
            }
        }
        public static PropDetailsM GetPropDetailForSharing(long propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var model = db.tblPropertyMs
                    .Where(x => x.iPropId == propId)
                    .Select(x => new PropDetailsM
                    {
                        lstetblPhotoGallery = x.tblPropertyPhotoMaps.Take(1)
                            .Select(p => new etblPhotoGallery { sMainImgUrl = p.sImgUrl }).ToList(),
                        sDescription = x.sDescription,
                        sImageUrlTG = x.sImageUrlTG
                    }).FirstOrDefault();

                return model;
            }
        }

        public static RoomDetails GetRoomDetails(int propid, int roomid, string Currency)
        {
            RoomDetails objRoomDetails = new RoomDetails();
            List<RoomImages> _arrRoomImages = new List<RoomImages>();
            List<RoomPolicy> _arrRoomPolicy = new List<RoomPolicy>();
            List<RoomAmenities> _arrRoomAmenities = new List<RoomAmenities>();
            try
            {
                SqlParameter[] MyParam = new SqlParameter[3];
                MyParam[0] = new SqlParameter("@propid", propid);
                MyParam[1] = new SqlParameter("@roomid", roomid);
                MyParam[2] = new SqlParameter("@Currency", Currency);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetRoomDetails", MyParam);
                if (ds != null)
                {
                    // Room Info
                    objRoomDetails.sRoomName = Convert.ToString(ds.Tables[0].Rows[0]["RoomName"]);
                    objRoomDetails.sSizeSqft = Convert.ToString(ds.Tables[0].Rows[0]["SizeSqft"]);
                    objRoomDetails.sSizeMtr = Convert.ToString(ds.Tables[0].Rows[0]["SizeMtr"]);
                    objRoomDetails.sSingleBed = Convert.ToString(ds.Tables[0].Rows[0]["SingleBed"]);
                    objRoomDetails.sDoubleBed = Convert.ToString(ds.Tables[0].Rows[0]["DoubleBed"]);
                    objRoomDetails.sOutdoorLocation = Convert.ToString(ds.Tables[0].Rows[0]["OutdoorLocation"]);
                    objRoomDetails.sDefaultImage = Convert.ToString(ds.Tables[0].Rows[0]["Image"]);
                    objRoomDetails.iMaxOccupancy = Convert.ToInt32(ds.Tables[0].Rows[0]["iMaxOccupancy"]);


                    objRoomDetails.Amenity1 = Convert.ToString(ds.Tables[0].Rows[0]["Amen1"]);
                    objRoomDetails.Amenity2 = Convert.ToString(ds.Tables[0].Rows[0]["Amen2"]);
                    objRoomDetails.Amenity3 = Convert.ToString(ds.Tables[0].Rows[0]["Amen3"]);
                    objRoomDetails.Amenity4 = Convert.ToString(ds.Tables[0].Rows[0]["Amen4"]);
                    objRoomDetails.Amenity5 = Convert.ToString(ds.Tables[0].Rows[0]["Amen5"]);
                    objRoomDetails.Amenity6 = Convert.ToString(ds.Tables[0].Rows[0]["Amen6"]);
                    objRoomDetails.Amenity7 = Convert.ToString(ds.Tables[0].Rows[0]["Amen7"]);
                    objRoomDetails.Amenity8 = Convert.ToString(ds.Tables[0].Rows[0]["Amen8"]);


                    // Images
                    foreach (DataRow drImg in ds.Tables[1].Rows)
                    {
                        RoomImages objRoomImages = new RoomImages();
                        objRoomImages.sRoomImage = Convert.ToString(drImg["Images"]);
                        _arrRoomImages.Add(objRoomImages);
                    }
                    // Policy
                    foreach (DataRow drPolicy in ds.Tables[2].Rows)
                    {
                        RoomPolicy objRoomPolicy = new RoomPolicy();
                        objRoomPolicy.sAge = "";
                        objRoomPolicy.iComplimentaryStayAge = Convert.ToInt32(drPolicy["iComplimentaryStayAge"]);
                        objRoomPolicy.iExtraBedRequiredFrom = Convert.ToInt32(drPolicy["iExtraBedRequiredFrom"]);
                        objRoomPolicy.sExtraCharge = Convert.ToString(drPolicy["ExtraCharge"]);
                        _arrRoomPolicy.Add(objRoomPolicy);
                    }
                    // Room Amenities
                    foreach (DataRow drAmenity in ds.Tables[3].Rows)
                    {
                        RoomAmenities objRoomAmenities = new RoomAmenities();
                        objRoomAmenities.sAmenity = Convert.ToString(drAmenity["sRoomAmenity"]);
                        _arrRoomAmenities.Add(objRoomAmenities);
                    }
                    objRoomDetails.Images = _arrRoomImages;
                    objRoomDetails.Policy = _arrRoomPolicy;
                    objRoomDetails.RoomAmenities = _arrRoomAmenities;
                }
            }
            catch (Exception ex) { throw ex; }

            return objRoomDetails;
        }

        public static decimal GetNegotiationAmt(DataTable dt)
        {
            SqlConnection objConn = default(SqlConnection);
            try
            {
                Decimal? Price = 0;
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@NegotiationRatePlans", dt);

                Price = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetNegotiationAmount", MyParams).Tables[0].Rows[0].Field<decimal?>("Price");
                return Price.HasValue ? Price.Value : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }

        public static BookingGuestDetails GetBookingDetailsForGuests(int BookingId)
        {

            etblBookingTx eobj = new etblBookingTx();
            BookingGuestDetails objMapping = new BookingGuestDetails();
            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                var dbobj = db.tblBookingTxes.SingleOrDefault(u => u.iBookingId == BookingId);
                if (dbobj != null)
                    eobj = (etblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);

                objMapping.objBooking = eobj;

                if(eobj.sExtra3 == "Redeem")
                objMapping.objBooking.RedeemedPoints = eobj.sExtra4;

                #region Rooms Details

                SqlConnection objConn = default(SqlConnection);
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@BookingId", BookingId);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBookingDetails", MyParams);
                objConn.Close();

                if (ds.Tables.Count > 0)
                {
                    List<etblBookingDetailsTx> objetblBookingsDetails = new List<etblBookingDetailsTx>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objetblBookingsDetails.Add(new etblBookingDetailsTx()
                        {
                            iBookingDetailsID = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingDetailsID"].ToString()),
                            iBookingId = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingId"].ToString()),
                            iRoomId = Convert.ToString(ds.Tables[0].Rows[i]["iRoomId"].ToString()),
                            iRPId = Convert.ToString(ds.Tables[0].Rows[i]["iRPId"].ToString()),
                            iRooms = Convert.ToInt16(ds.Tables[0].Rows[i]["iRooms"].ToString()),
                            sRoomName = Convert.ToString(ds.Tables[0].Rows[i]["sRoomName"].ToString()),
                            sRPName = Convert.ToString(ds.Tables[0].Rows[i]["sRPName"].ToString()),
                            iOccupancy = Convert.ToInt16(ds.Tables[0].Rows[i]["iOccupancy"].ToString())
                        });
                    }

                    objMapping.lsttblBookDetails = objetblBookingsDetails;


                    objMapping.sMainImgUrl = ds.Tables[1].Rows[0]["sMainImgUrl"].ToString();
                    objMapping.iStarCategory = Convert.ToInt16(ds.Tables[1].Rows[0]["iStarCategory"].ToString());
                    objMapping.sPropertyName = ds.Tables[1].Rows[0]["sPropertyName"].ToString();
                    objMapping.RatingImageUrl = ds.Tables[1].Rows[0]["sRatingImageURl"].ToString();
                    objMapping.RatingString = ds.Tables[1].Rows[0]["sRankingString"].ToString();
                    objMapping.sPropertyAddress = ds.Tables[1].Rows[0]["sPropertyAddress"].ToString();
                    objMapping.scheckIn = ds.Tables[1].Rows[0]["scheckIn"].ToString();
                    objMapping.scheckInYear = ds.Tables[1].Rows[0]["scheckInYear"].ToString();
                    objMapping.scheckOut = ds.Tables[1].Rows[0]["scheckOut"].ToString();
                    objMapping.scheckOutYear = ds.Tables[1].Rows[0]["scheckOutYear"].ToString();
                    objMapping.iTotalDays = ds.Tables[1].Rows[0]["iTotalDays"].ToString();
                    objMapping.Symbol = ds.Tables[1].Rows[0]["Symbol"].ToString();
                    objMapping.iNoOfRooms = ds.Tables[1].Rows[0]["iNoOfRooms"].ToString();
                    objMapping.iPropId = ds.Tables[1].Rows[0]["iPropId"].ToString();


                    List<etblHotelFacilities> lstetblHotel = new List<etblHotelFacilities>();
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        lstetblHotel.Add(new etblHotelFacilities()
                        {
                            iHoteFacilityId = Convert.ToInt32(ds.Tables[2].Rows[i]["iHoteFacilityId"]),
                            sHotelFacilites = Convert.ToString(ds.Tables[2].Rows[i]["sHotelFacilites"]),
                            iRank = Convert.ToInt32(ds.Tables[2].Rows[i]["iRank"]),
                            sImgUrl = Convert.ToString(ds.Tables[2].Rows[i]["sImgUrl"])
                        });
                    }

                    objMapping.lstetblHotelFacilities = lstetblHotel;

                    if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        var corporateEmail = ds.Tables[3].Rows[0]["CorporateEmail"].ToString();
                        var email = ds.Tables[3].Rows[0]["Email"].ToString();
                        var isGuest = Convert.ToBoolean(ds.Tables[3].Rows[0]["isGuest"].ToString());
                        var customerId = Convert.ToInt32(ds.Tables[3].Rows[0]["Id"].ToString());
                        objMapping.CustomerId = customerId;

                        if (isGuest)
                        {
                            objMapping.IsGuestBooking = true;
                            objMapping.GstnEntityName = ds.Tables[3].Rows[0]["GstnEntityName"].ToString();
                            objMapping.GstnEntityType = ds.Tables[3].Rows[0]["GstnEntityType"].ToString();
                            objMapping.GstnNumber = ds.Tables[3].Rows[0]["GstnNumber"].ToString();
                        }
                        else
                        {
                            var corporateRecord = BL_WebsiteUser.GetCorporateDetailByCorporateEmail(corporateEmail);

                            if (corporateRecord == null)
                            {
                                corporateRecord = BL_WebsiteUser.GetCorporateDetailByCorporateEmail(corporateEmail);
                            }

                            if (corporateRecord != null && !string.IsNullOrEmpty(corporateRecord.sRegisteredEntityName))
                            {
                                objMapping.IsCorporateBooking = true;
                                objMapping.GstnEntityName = corporateRecord.sRegisteredEntityName;
                                objMapping.GstnEntityType = corporateRecord.sEntityType;
                                objMapping.GstnNumber = corporateRecord.sGstinNumber;
                            }
                            else
                            {
                                objMapping.IsRegularBooking = true;
                                objMapping.GstnEntityName = ds.Tables[3].Rows[0]["GstnEntityName"].ToString();
                                objMapping.GstnEntityType = ds.Tables[3].Rows[0]["GstnEntityType"].ToString();
                                objMapping.GstnNumber = ds.Tables[3].Rows[0]["GstnNumber"].ToString();
                            }
                        }
                    }

                    #endregion
                }

                return objMapping;
            }
        }
        /// <summary>
        /// Update the View count in the database 
        /// </summary>
        /// <param name="ipropId"></param>
        /// <param name="iCustomerId"></param>
        public static void UpdateRecentViewAsync(int ipropId, long iCustomerId)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[2];
                    MyParam[0] = new SqlParameter("@CustomerId", iCustomerId);
                    MyParam[1] = new SqlParameter("@iPropId", ipropId);
                    db.Database.ExecuteSqlCommand("uspSetRecentViews @CustomerId,@iPropId", MyParam);
                    //db.Database.SqlQuery<object>("uspSetRecentViews @CustomerId,@iPropId", MyParam).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task UpdateViewForConversion(string cType, DataTable ids, long? BookingId = null)
        {
            try
            {

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[3];
                    MyParam[0] = new SqlParameter("@Ids", ids);
                    MyParam[0].TypeName = "dbo.Ids";
                    MyParam[1] = new SqlParameter("@cType", cType);

                    if (BookingId == null)
                    {
                        MyParam[2] = new SqlParameter("@iBookingId", DBNull.Value);
                    }
                    else
                    {
                        MyParam[2] = new SqlParameter("@iBookingId", BookingId);
                    }

                    await db.Database.ExecuteSqlCommandAsync("uspInsertViewForConv @Ids,@cType,@iBookingId", MyParam);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PropDetailsM GetRoomsOfFHotel(PropDetailsM propSearchRequestDetail, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch, bool isCorporate)
        {

            PropDetailsM propertyM_Result = new PropDetailsM();
            DataSet ds = new DataSet();
            dtRoomOccupancySearch.TableName = "tblRoomOccupancy";
            dtChildrenAgeSearch.TableName = "tblChildrenAge";

            //Get General Details
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@PropId", propSearchRequestDetail.iPropId);
                var objresult = db.Database.SqlQuery<PropDetailsM>("uspGetOfferReviewDetailsbyID @PropId", MyParam).ToList();
                propertyM_Result = (PropDetailsM)OneFineRateAppUtil.clsUtils.ConvertToObject(objresult[0], propertyM_Result);

                SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                MyParamHotelFacilities[0] = new SqlParameter("@PropId", propSearchRequestDetail.iPropId);
                var objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                propertyM_Result.lstetblHotelFacilities = objresultHotelFacilities;
            }

            using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
            {
                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[18];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iHotelID", propSearchRequestDetail.iPropId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtCheckIn", propSearchRequestDetail.dtCheckIn);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@dtCheckOut", propSearchRequestDetail.dtCheckOut);
                MyParams[3] = new System.Data.SqlClient.SqlParameter("@bLogin", propSearchRequestDetail.bLogin);
                MyParams[4] = new System.Data.SqlClient.SqlParameter("@Currency", propSearchRequestDetail.Currency);
                MyParams[5] = new System.Data.SqlClient.SqlParameter("@bIsAirConditioning", propSearchRequestDetail.bIsAirConditioning);
                MyParams[6] = new System.Data.SqlClient.SqlParameter("@bIsBathtub", propSearchRequestDetail.bIsBathtub);
                MyParams[7] = new System.Data.SqlClient.SqlParameter("@bIsFlatScreenTV", propSearchRequestDetail.bIsFlatScreenTV);
                MyParams[8] = new System.Data.SqlClient.SqlParameter("@bIsSoundproof", propSearchRequestDetail.bIsSoundproof);
                MyParams[9] = new System.Data.SqlClient.SqlParameter("@bIsView", propSearchRequestDetail.bIsView);
                MyParams[10] = new System.Data.SqlClient.SqlParameter("@bIsInternetFacilities", propSearchRequestDetail.bIsInternetFacilities);
                MyParams[11] = new System.Data.SqlClient.SqlParameter("@bIsPrivatePool", propSearchRequestDetail.bIsPrivatePool);
                MyParams[12] = new System.Data.SqlClient.SqlParameter("@bIsRoomService", propSearchRequestDetail.bIsRoomService);
                MyParams[13] = new System.Data.SqlClient.SqlParameter("@dMinPrice", propSearchRequestDetail.dMinPrice);
                MyParams[14] = new System.Data.SqlClient.SqlParameter("@dMaxPrice", propSearchRequestDetail.dMaxPrice);
                MyParams[15] = new System.Data.SqlClient.SqlParameter("@SpecialDeal", propSearchRequestDetail.SpecialDeal);
                MyParams[16] = new System.Data.SqlClient.SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                MyParams[17] = new System.Data.SqlClient.SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);

                if (isCorporate)
                {
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSearchRoomsForHotelCorp", MyParams);
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSearchRoomsForHotel", MyParams);
                }
            }

            #region Room Result Processing

            if (ds.Tables.Count > 0)
            {
                List<etblRooms> objetblRooms = new List<etblRooms>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    List<RoomTaxes> objresultRoomTaxes = new List<RoomTaxes>();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int lstTaxes = 0; lstTaxes < ds.Tables[1].Rows.Count; lstTaxes++)
                        {
                            if (Convert.ToInt64(ds.Tables[1].Rows[lstTaxes]["iRoomId"]) == Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()))
                            {

                                int dcount = ds.Tables[1].Select("iRoomId=" + Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()) + "").Length;

                                if (dcount == 1)
                                {
                                    objresultRoomTaxes.Add(new RoomTaxes()
                                    {
                                        iRoomId = Convert.ToInt32(ds.Tables[1].Rows[lstTaxes]["iRoomId"]),
                                        ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidFrom"]),
                                        ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidTo"]),
                                        Tax = ds.Tables[1].Rows[lstTaxes]["Tax"].ToString(),
                                        TaxString = ""

                                    });
                                }
                                else
                                {
                                    objresultRoomTaxes.Add(new RoomTaxes()
                                    {
                                        iRoomId = Convert.ToInt32(ds.Tables[1].Rows[lstTaxes]["iRoomId"]),
                                        ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidFrom"]),
                                        ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[lstTaxes]["ValidTo"]),
                                        Tax = ds.Tables[1].Rows[lstTaxes]["Tax"].ToString(),
                                        TaxString = "(Valid From : " + ds.Tables[1].Rows[lstTaxes]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[1].Rows[lstTaxes]["ValidTo"].ToString().Replace("00:00:00", "") + ") "

                                    });
                                }



                            }
                        }
                    }

                    objetblRooms.Add(new etblRooms()
                    {
                        iRoomId = Convert.ToInt64(ds.Tables[0].Rows[i]["iRoomId"].ToString()),
                        sRoomName = ds.Tables[0].Rows[i]["sRoomName"].ToString(),
                        MaxOccupancy = Convert.ToByte(ds.Tables[0].Rows[i]["MaxOccupancy"].ToString()),
                        TwinBed = Convert.ToInt16(ds.Tables[0].Rows[i]["TwinBed"].ToString()),
                        MaxExtraBeds = Convert.ToByte(ds.Tables[0].Rows[i]["MaxExtraBeds"].ToString()),
                        iAvailableInventory = Convert.ToInt32(ds.Tables[0].Rows[i]["iAvailableInventory"].ToString()),
                        ExtraBedCharges = ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString() == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString()),
                        sRoomImgUrl = ds.Tables[0].Rows[i]["sRoomImgUrl"].ToString(),
                        Amen1 = ds.Tables[0].Rows[i]["Amen1"].ToString(),
                        Amen2 = ds.Tables[0].Rows[i]["Amen2"].ToString(),
                        Amen3 = ds.Tables[0].Rows[i]["Amen3"].ToString(),
                        Amen4 = ds.Tables[0].Rows[i]["Amen4"].ToString(),
                        Amen5 = ds.Tables[0].Rows[i]["Amen5"].ToString(),
                        Amen6 = ds.Tables[0].Rows[i]["Amen6"].ToString(),
                        Amen7 = ds.Tables[0].Rows[i]["Amen7"].ToString(),
                        Amen8 = ds.Tables[0].Rows[i]["Amen8"].ToString(),
                        lstRoomTaxes = objresultRoomTaxes
                    });

                }
                propertyM_Result.lstetblRooms = objetblRooms;


                DataTable dtdistinct = ds.Tables[2].AsEnumerable()
                                        .GroupBy(r => new { iRoomId = r["iRoomId"], RPID = r["RPID"] })
                                        .Select(g => g.OrderBy(r => r["iRoomId"]).First())
                                        .CopyToDataTable();

                var DistinctRatePlanList = (from t in dtdistinct.AsEnumerable()
                                            select t).Distinct().ToList();


                for (int lstRooms = 0; lstRooms < propertyM_Result.lstetblRooms.Count; lstRooms++)
                {
                    List<etblRatePlans> objresultetblRatePlans = new List<etblRatePlans>();
                    for (int j = 0; j < DistinctRatePlanList.Count; j++)
                    {

                        List<CancellationPolcy> objresultCancellationPolicy = new List<CancellationPolcy>();
                        for (int lstCancellation = 0; lstCancellation < ds.Tables[3].Rows.Count; lstCancellation++)
                        {
                            if (Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]) == Convert.ToInt32(DistinctRatePlanList[j]["RPID"]))
                            {

                                CancellationPolcy objcancel = new CancellationPolcy();
                                objcancel.iRPId = Convert.ToInt32(ds.Tables[3].Rows[lstCancellation]["iRPId"]);
                                objcancel.ValidFrom = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidFrom"]);
                                objcancel.ValidTo = Convert.ToDateTime(ds.Tables[3].Rows[lstCancellation]["ValidTo"]);
                                int diff = (objcancel.ValidTo - objcancel.ValidFrom).Days;
                                if (diff > 1)
                                    objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString() + " (Valid From : " + ds.Tables[3].Rows[lstCancellation]["ValidFrom"].ToString().Replace("00:00:00", "") + " Valid To : " + ds.Tables[3].Rows[lstCancellation]["ValidTo"].ToString().Replace("00:00:00", "") + ") ";
                                else
                                    objcancel.CancellationPolicy = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                objcancel.PolicyName = ds.Tables[3].Rows[lstCancellation]["CancellationPolicy"].ToString();
                                objcancel.CancellationMsg = ds.Tables[3].Rows[lstCancellation]["CancellationMsg"].ToString();
                                objresultCancellationPolicy.Add(objcancel);
                            }
                        }

                        if (Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()) == propertyM_Result.lstetblRooms[lstRooms].iRoomId)
                        {
                            objresultetblRatePlans.Add(new etblRatePlans()
                            {
                                iRoomId = Convert.ToInt64(DistinctRatePlanList[j]["iRoomId"].ToString()),
                                RPID = Convert.ToInt32(DistinctRatePlanList[j]["RPID"].ToString()),
                                RateInclusion = DistinctRatePlanList[j]["RateInclusion"].ToString(),
                                lstCancellationPolcy = objresultCancellationPolicy
                            });
                        }
                    }
                    propertyM_Result.lstetblRooms[lstRooms].lstRatePlan = objresultetblRatePlans;
                }

                for (int m = 0; m < propertyM_Result.lstetblRooms.Count; m++)
                {
                    for (int n = 0; n < propertyM_Result.lstetblRooms[m].lstRatePlan.Count; n++)
                    {
                        List<etblOccupancy> objresultetblOccupancy = new List<etblOccupancy>();
                        for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                        {
                            List<TaxesDateRoomRateWise> lstRoomTaxes = new List<TaxesDateRoomRateWise>();
                            for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                            {
                                if (propertyM_Result.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[6].Rows[tax]["RoomID"].ToString())
                               && propertyM_Result.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[6].Rows[tax]["RPID"].ToString()))
                                {
                                    lstRoomTaxes.Add(new TaxesDateRoomRateWise()
                                    {
                                        dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                                        RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : -1,
                                        RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                                        bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                                        iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : -1,
                                        dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                                        TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                                        TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                                    });
                                }
                            }

                            string[] arr = new string[2];
                            if (propertyM_Result.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"].ToString())
                                && propertyM_Result.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[2].Rows[k]["RPID"].ToString()))
                            {
                                arr = ds.Tables[2].Rows[k]["Persons"].ToString().Split(',');

                                var occupancy = new etblOccupancy();

                                occupancy.iRoomId = ds.Tables[2].Rows[k]["iRoomId"] != DBNull.Value ? Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"]) : -1;
                                occupancy.RPID = ds.Tables[2].Rows[k]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"]) : -1;
                                occupancy.RatePlan = ds.Tables[2].Rows[k]["RatePlan"] != DBNull.Value ? ds.Tables[2].Rows[k]["RatePlan"].ToString() : null;
                                occupancy.iOccupancy = ds.Tables[2].Rows[k]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"]) : -1;
                                occupancy.ExtraBeds = ds.Tables[2].Rows[k]["ExtraBeds"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["ExtraBeds"]) : -1;
                                occupancy.ExtraBedCharges = ds.Tables[2].Rows[k]["ExtraBedCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["ExtraBedCharges"]) : -1;
                                occupancy.dBasePrice = ds.Tables[2].Rows[k]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dBasePrice"]) : -1;
                                occupancy.dPrice = ds.Tables[2].Rows[k]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPrice"]) : -1;

                                if (isCorporate)
                                {
                                    occupancy.dPrice = occupancy.dBasePrice;
                                }

                                occupancy.dPriceRP = ds.Tables[2].Rows[k]["dPriceRP"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[k]["dPriceRP"]) : -1;
                                occupancy.iAdults = !string.IsNullOrEmpty(arr[0]) ? Convert.ToInt32(arr[0]) : -1;
                                occupancy.iChildrens = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : -1;
                                occupancy.Cnt = ds.Tables[2].Rows[k]["Cnt"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["Cnt"]) : -1;
                                occupancy.iPromoType = ds.Tables[2].Rows[k]["iPromoType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["iPromoType"]) : -1;
                                occupancy.blsPromo = ds.Tables[2].Rows[k]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[2].Rows[k]["bIsPromo"]) : false;
                                occupancy.ChildrenAge = ds.Tables[2].Rows[k]["ChildrenAge"] != DBNull.Value ? ds.Tables[2].Rows[k]["ChildrenAge"].ToString() : null;
                                occupancy.dTaxes = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["tax"]) : 0;
                                occupancy.dTaxesForHotel = ds.Tables[2].Rows[k]["tax"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[k]["tax"]) : 0;

                                occupancy.lstTaxesDateWise = lstRoomTaxes;

                                objresultetblOccupancy.Add(occupancy);
                            }
                            propertyM_Result.lstetblRooms[m].lstRatePlan[n].lstetblOccupancy = objresultetblOccupancy;
                        }
                    }
                }

                List<TaxesDateRoomRateWise> lstRoomTaxes_All = new List<TaxesDateRoomRateWise>();
                for (int tax = 0; tax < ds.Tables[6].Rows.Count; tax++)
                {

                    lstRoomTaxes_All.Add(new TaxesDateRoomRateWise()
                    {
                        dtDate = ds.Tables[6].Rows[tax]["dtDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[6].Rows[tax]["dtDate"]) : DateTime.Now,
                        RoomID = ds.Tables[6].Rows[tax]["RoomID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RoomID"]) : 0,
                        RPID = ds.Tables[6].Rows[tax]["RPID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["RPID"]) : -1,
                        bIsPromo = ds.Tables[6].Rows[tax]["bIsPromo"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[6].Rows[tax]["bIsPromo"]) : false,
                        iOccupancy = ds.Tables[6].Rows[tax]["iOccupancy"] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[tax]["iOccupancy"]) : 0,
                        dBasePrice = ds.Tables[6].Rows[tax]["dBasePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["dBasePrice"]) : 0,
                        TaxPer = ds.Tables[6].Rows[tax]["TaxPer"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxPer"]) : 0,
                        TaxVal = ds.Tables[6].Rows[tax]["TaxVal"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[6].Rows[tax]["TaxVal"]) : 0
                    });

                }
                List<TaxesSeprateDateRoomRateWise> lstRoomTaxesDateWise_All = new List<TaxesSeprateDateRoomRateWise>();
                for (int tax = 0; tax < ds.Tables[9].Rows.Count; tax++)
                {
                    var seperatetax = ds.Tables[9].Rows[tax];

                    lstRoomTaxesDateWise_All.Add(new TaxesSeprateDateRoomRateWise()
                    {
                        dtStay = seperatetax["dtStay"] != DBNull.Value ? Convert.ToString(seperatetax["dtStay"]) : "",
                        iRoomId = seperatetax["iRoomId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iRoomId"]) : 0,
                        RPID = seperatetax["RPID"] != DBNull.Value ? Convert.ToInt32(seperatetax["RPID"]) : -1,
                        sTaxName = seperatetax["sTaxName"] != DBNull.Value ? Convert.ToString(seperatetax["sTaxName"]) : "",
                        iOccupancy = seperatetax["iOccupancy"] != DBNull.Value ? Convert.ToInt32(seperatetax["iOccupancy"]) : 0,
                        dPrice = seperatetax["dPrice"] != DBNull.Value ? Convert.ToDecimal(seperatetax["dPrice"]) : 0,
                        MaxTaxPer = seperatetax["MaxTaxPer"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxPer"]) : 0,
                        MaxTaxVal = seperatetax["MaxTaxVal"] != DBNull.Value ? Convert.ToDecimal(seperatetax["MaxTaxVal"]) : 0,
                        TaxId = seperatetax["iTaxId"] != DBNull.Value ? Convert.ToInt32(seperatetax["iTaxId"]) : 0

                    });

                }


                propertyM_Result.lstTaxesDateWise_OfferReview = lstRoomTaxes_All;
                propertyM_Result.lstTaxesDateWiseAll_OfferReview = lstRoomTaxesDateWise_All;
                List<OccupancyData> lstOccData = new List<OccupancyData>();
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    lstOccData.Add(new OccupancyData()
                    {
                        Occupancy = Convert.ToInt16(ds.Tables[4].Rows[i]["Occupancy"]),
                        Rooms = Convert.ToInt16(ds.Tables[4].Rows[i]["Rooms"]),
                        Total = 0
                    });
                }

                #endregion Room Result Processing

                propertyM_Result.lstOccData = lstOccData;

                propertyM_Result.Symbol = ds.Tables[5].Rows[0].Field<string>("Symbol");
                propertyM_Result.dCommissionRate = ds.Tables[5].Rows[0].Field<decimal>("CommissionRate");
                propertyM_Result.sCheckInDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckInDate");
                propertyM_Result.sCheckOutDate_Display = ds.Tables[5].Rows[0].Field<string>("sCheckOutDate");
                #region RoomMax Taxes
                List<etblRoomMaxTaxes> maxTax = new List<etblRoomMaxTaxes>();
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    etblRoomMaxTaxes roomatax = new etblRoomMaxTaxes();
                    roomatax.RoomID = Convert.ToInt32(ds.Tables[7].Rows[i]["RoomID"]);
                    roomatax.TaxPer = Convert.ToString(ds.Tables[7].Rows[i]["TaxPer"]);
                    maxTax.Add(roomatax);
                }

                #endregion
                propertyM_Result.MaxCharges = maxTax;
                #region Service Charges Date Wise
                etblTaxCharges matax = new etblTaxCharges();
                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    matax.dOFRServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["ServiceCharge"] is DBNull) ? "0.0" : ds.Tables[8].Rows[i]["ServiceCharge"]);
                    matax.TaxOnServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TaxOnServiceCharge"] is DBNull) ? "0.0" : ds.Tables[8].Rows[i]["TaxOnServiceCharge"]);
                    matax.TotalServiceCharge = Convert.ToDecimal((ds.Tables[8].Rows[i]["TotalServiceCharge"] is DBNull) ? "0.0" : ds.Tables[8].Rows[i]["TotalServiceCharge"]);
                    matax.cGstValueType = Convert.ToString(ds.Tables[8].Rows[i]["cGstValueType"]);
                    matax.dGstValue = Convert.ToString(Math.Round(Convert.ToDecimal((ds.Tables[8].Rows[i]["dGstValue"] is DBNull) ? "0.0" : ds.Tables[8].Rows[i]["dGstValue"])));
                }

                #endregion
                propertyM_Result.TaxCharges = matax;
            }
            return propertyM_Result;
        }

        public static BookingGuestDetails GetBookingDetailsForGuestsRoomsInfo(int BookingId)
        {

            etblBookingTx eobj = new etblBookingTx();
            BookingGuestDetails objMapping = new BookingGuestDetails();
            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                var dbobj = db.tblBookingTxes.SingleOrDefault(u => u.iBookingId == BookingId);
                if (dbobj != null)
                    eobj = (etblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);

                objMapping.objBooking = eobj;


                #region Rooms Details

                SqlConnection objConn = default(SqlConnection);
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@BookingId", BookingId);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBookingGuestRoomDetails", MyParams);
                objConn.Close();

                if (ds.Tables.Count > 0)
                {
                    List<etblBookingDetailsTx> objetblBookingsDetails = new List<etblBookingDetailsTx>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objetblBookingsDetails.Add(new etblBookingDetailsTx()
                        {
                            iBookingDetailsID = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingDetailsID"].ToString()),
                            iBookingId = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingId"].ToString()),
                            iRoomId = Convert.ToString(ds.Tables[0].Rows[i]["iRoomId"].ToString()),
                            iRPId = Convert.ToString(ds.Tables[0].Rows[i]["iRPId"].ToString()),
                            iRooms = Convert.ToInt16(ds.Tables[0].Rows[i]["iRooms"].ToString()),
                            sRoomName = Convert.ToString(ds.Tables[0].Rows[i]["sRoomName"].ToString()),
                            sRPName = Convert.ToString(ds.Tables[0].Rows[i]["sRPName"].ToString()),
                            iOccupancy = Convert.ToInt16(ds.Tables[0].Rows[i]["iOccupancy"].ToString()),
                            sGuestName = Convert.ToString(ds.Tables[0].Rows[i]["sGuestName"].ToString()),
                            sGuestEmail = Convert.ToString(ds.Tables[0].Rows[i]["sGuestEmail"].ToString()),
                            sGuestMobile = Convert.ToString(ds.Tables[0].Rows[i]["sGuestMobile"].ToString()),
                            sBedPreference = Convert.ToString(ds.Tables[0].Rows[i]["sBedPreference"].ToString()),
                            bIsPrimary = Convert.ToBoolean(ds.Tables[0].Rows[i]["bIsPrimary"].ToString()),
                            sCountryPhoneCode = Convert.ToString(ds.Tables[0].Rows[i]["sCountryPhoneCode"].ToString())

                        });
                    }

                    objMapping.lsttblBookDetails = objetblBookingsDetails;


                    objMapping.sMainImgUrl = ds.Tables[1].Rows[0]["sMainImgUrl"].ToString();
                    objMapping.iStarCategory = Convert.ToInt16(ds.Tables[1].Rows[0]["iStarCategory"].ToString());
                    objMapping.sPropertyName = ds.Tables[1].Rows[0]["sPropertyName"].ToString();
                    objMapping.sPropertyAddress = ds.Tables[1].Rows[0]["sPropertyAddress"].ToString();
                    objMapping.RatingImageUrl = ds.Tables[1].Rows[0]["sRatingImageURl"].ToString();
                    objMapping.RatingString = ds.Tables[1].Rows[0]["sRankingString"].ToString();
                    objMapping.scheckIn = ds.Tables[1].Rows[0]["scheckIn"].ToString();
                    objMapping.scheckInYear = ds.Tables[1].Rows[0]["scheckInYear"].ToString();
                    objMapping.scheckOut = ds.Tables[1].Rows[0]["scheckOut"].ToString();
                    objMapping.scheckOutYear = ds.Tables[1].Rows[0]["scheckOutYear"].ToString();
                    objMapping.iTotalDays = ds.Tables[1].Rows[0]["iTotalDays"].ToString();
                    objMapping.Symbol = ds.Tables[1].Rows[0]["Symbol"].ToString();
                    objMapping.iNoOfRooms = ds.Tables[1].Rows[0]["iNoOfRooms"].ToString();
                    objMapping.iPropId = ds.Tables[1].Rows[0]["iPropId"].ToString();
                    objMapping.iVendorId = ds.Tables[1].Rows[0]["sVendorId"].ToString();
                    objMapping.bAnniversary = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAnniversary"].ToString());
                    objMapping.bHoneymoon = Convert.ToBoolean(ds.Tables[1].Rows[0]["bHoneymoon"].ToString());
                    objMapping.bBirthday = Convert.ToBoolean(ds.Tables[1].Rows[0]["bBirthday"].ToString());


                    List<etblHotelFacilities> lstetblHotel = new List<etblHotelFacilities>();
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        lstetblHotel.Add(new etblHotelFacilities()
                        {
                            iHoteFacilityId = Convert.ToInt32(ds.Tables[2].Rows[i]["iHoteFacilityId"]),
                            sHotelFacilites = Convert.ToString(ds.Tables[2].Rows[i]["sHotelFacilites"]),
                            iRank = Convert.ToInt32(ds.Tables[2].Rows[i]["iRank"]),
                            sImgUrl = Convert.ToString(ds.Tables[2].Rows[i]["sImgUrl"])
                        });
                    }
                    objMapping.lstetblHotelFacilities = lstetblHotel;

                    if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        var corporateEmail = ds.Tables[3].Rows[0]["CorporateEmail"].ToString();
                        var email = ds.Tables[3].Rows[0]["Email"].ToString();
                        var isGuest = Convert.ToBoolean(ds.Tables[3].Rows[0]["isGuest"].ToString());
                        var customerId = Convert.ToInt32(ds.Tables[3].Rows[0]["Id"].ToString());
                        objMapping.CustomerId = customerId;

                        if (isGuest)
                        {
                            objMapping.IsGuestBooking = true;
                            objMapping.GstnEntityName = ds.Tables[3].Rows[0]["GstnEntityName"].ToString();
                            objMapping.GstnEntityType = ds.Tables[3].Rows[0]["GstnEntityType"].ToString();
                            objMapping.GstnNumber = ds.Tables[3].Rows[0]["GstnNumber"].ToString();
                        }
                        else
                        {
                            var corporateRecord = BL_WebsiteUser.GetCorporateDetailByCorporateEmail(corporateEmail);

                            if (corporateRecord == null)
                            {
                                corporateRecord = BL_WebsiteUser.GetCorporateDetailByCorporateEmail(corporateEmail);
                            }

                            if (corporateRecord != null && !string.IsNullOrEmpty(corporateRecord.sRegisteredEntityName))
                            {
                                objMapping.IsCorporateBooking = true;
                                objMapping.GstnEntityName = corporateRecord.sRegisteredEntityName;
                                objMapping.GstnEntityType = corporateRecord.sEntityType;
                                objMapping.GstnNumber = corporateRecord.sGstinNumber;
                            }
                            else
                            {
                                objMapping.IsRegularBooking = true;
                                objMapping.GstnEntityName = ds.Tables[3].Rows[0]["GstnEntityName"].ToString();
                                objMapping.GstnEntityType = ds.Tables[3].Rows[0]["GstnEntityType"].ToString();
                                objMapping.GstnNumber = ds.Tables[3].Rows[0]["GstnNumber"].ToString();
                            }
                        }
                    }
                    #endregion
                }

                return objMapping;
            }
        }

        public static string GetPropertyName(int iPropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblPropertyMs.Where(u => u.iPropId == iPropId).Select(x => x.sHotelName).FirstOrDefault();
            }
        }
       
    }
}