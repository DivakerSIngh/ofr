using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace OneFineRateBLL
{
    public class BL_WebSiteSearchPage
    {
        public static List<eDropDown> GetAllHotels(string txt, int PlaceId, int Type, int iReqType)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eDropDown> eobj = new List<eDropDown>();
                SqlParameter[] MyParam = new SqlParameter[4];
                MyParam[0] = new SqlParameter("@iPlaceID", PlaceId);
                MyParam[1] = new SqlParameter("@iAreaType", Type);
                MyParam[2] = new SqlParameter("@sHotelSearch", txt);
                MyParam[3] = new SqlParameter("@iReqType", iReqType);
                var result = db.Database.SqlQuery<eDropDown>("uspGetSearchedHotels @iPlaceID,@iAreaType,@sHotelSearch,@iReqType ", MyParam).ToList();
                foreach (var item in result)
                {
                    eobj.Add((eDropDown)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eDropDown()));
                }
                return eobj;
            }
        }
       
        public static List<PropSearchResponseModel> GetHotelsBySearchQ(PropSearchRequestModel searchRequestModel, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch, string sCurrency, out string sCurrencySymbol)
        {
            sCurrencySymbol = string.Empty;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var propertyList = new List<PropSearchResponseModel>();

                #region SqlParameter

                SqlParameter[] MyParam = new SqlParameter[40];

                MyParam[0] = new SqlParameter("@iPlaceID", searchRequestModel.cid);
                MyParam[1] = new SqlParameter("@dtCheckIn", searchRequestModel.dtCheckIn);
                MyParam[2] = new SqlParameter("@dtCheckOut", searchRequestModel.dtCheckOut);


                MyParam[3] = new SqlParameter("@iAreaType", searchRequestModel.iAreaType);
                MyParam[4] = new SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                MyParam[5] = new SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);

                MyParam[4].TypeName = "dbo.RoomOccupancySearch";
                MyParam[5].TypeName = "dbo.ChildrenAgeSearch";



                MyParam[6] = new SqlParameter("@sHotelSearch", searchRequestModel.sHotelSearch);
                MyParam[7] = new SqlParameter("@iFilterLocality", searchRequestModel.iFilterLocality);
                MyParam[8] = new SqlParameter("@sStarRating", searchRequestModel.sStarRating);
                MyParam[9] = new SqlParameter("@dMinPrice", searchRequestModel.dMinPrice);
                MyParam[10] = new SqlParameter("@dMaxPrice", searchRequestModel.dMaxPrice);
                MyParam[11] = new SqlParameter("@bLogin", searchRequestModel.bLogin);
                MyParam[12] = new SqlParameter("@SpecialDeal", searchRequestModel.isSpecialDeal);
                MyParam[13] = new SqlParameter("@sCurrencyCode", sCurrency);

                #region RoomFacilities

                MyParam[14] = new SqlParameter("@bIsAirConditioning", searchRequestModel.IsAirConditioning);
                MyParam[15] = new SqlParameter("@bIsBathtub", searchRequestModel.IsBathtub);
                MyParam[16] = new SqlParameter("@bIsFlatScreenTV", searchRequestModel.IsFlatScreenTV);
                MyParam[17] = new SqlParameter("@bIsSoundproof", searchRequestModel.IsSoundproof);
                MyParam[18] = new SqlParameter("@bIsView", searchRequestModel.IsView);
                MyParam[19] = new SqlParameter("@bIsInternetFacilities", searchRequestModel.IsInternetFacilities);
                MyParam[20] = new SqlParameter("@bIsPrivatePool", searchRequestModel.IsPrivatePool);
                MyParam[21] = new SqlParameter("@bIsRoomService", searchRequestModel.IsRoomService);

                #endregion RoomFacilities

                #region HotelFacilities

                MyParam[22] = new SqlParameter("@Internet", searchRequestModel.IsInternet);
                MyParam[23] = new SqlParameter("@Parking", searchRequestModel.IsParking);
                MyParam[24] = new SqlParameter("@NonSmoking", searchRequestModel.IsNonSmoking);
                MyParam[25] = new SqlParameter("@FacilitiesDifferentlyAbled", searchRequestModel.IsFacilitiesDifferentlyAbled);
                MyParam[26] = new SqlParameter("@FitnessCenter", searchRequestModel.IsFitnessCenter);
                MyParam[27] = new SqlParameter("@FamilyRooms", searchRequestModel.IsFamilyRooms);
                MyParam[28] = new SqlParameter("@Restaurant", searchRequestModel.IsRestaurant);
                MyParam[29] = new SqlParameter("@RoomService", searchRequestModel.IsHotelRoomService);
                MyParam[30] = new SqlParameter("@Transfers", searchRequestModel.IsTransfers);
                MyParam[31] = new SqlParameter("@PetFriendly", searchRequestModel.IsPetFriendly);
                MyParam[32] = new SqlParameter("@Spa", searchRequestModel.IsSpa);
                MyParam[33] = new SqlParameter("@OutdoorPool", searchRequestModel.IsOutdoorPool);
                MyParam[34] = new SqlParameter("@IndoorPool", searchRequestModel.IsIndoorPool);
                MyParam[35] = new SqlParameter("@Childcare", searchRequestModel.IsChildcare);
                MyParam[36] = new SqlParameter("@ChildrensClub", searchRequestModel.IsChildrensClub);
                MyParam[37] = new SqlParameter("@LaundryServices", searchRequestModel.IsLaundryServices);
                MyParam[38] = new SqlParameter("@AirconditionerHotel", searchRequestModel.IsAirconditionerHotel);

                #endregion HotelFacilities

                MyParam[39] = new SqlParameter("@iUserId", searchRequestModel.iUserId);

                #endregion

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspGetSearchedHotelsDetails", MyParam);
                    if (ds != null && ds.Tables != null)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var responseModel = new PropSearchResponseModel();
                            
                            responseModel.iPropId = row["iPropId"] != DBNull.Value ? Int32.Parse(row["iPropId"].ToString()) : 0;
                            responseModel.Amenity1 = row["Amenity1"] != DBNull.Value ? row["Amenity1"].ToString() : null;
                            responseModel.Amenity2 = row["Amenity2"] != DBNull.Value ? row["Amenity2"].ToString() : null;
                            responseModel.Amenity3 = row["Amenity3"] != DBNull.Value ? row["Amenity3"].ToString() : null;
                            responseModel.Amenity4 = row["Amenity4"] != DBNull.Value ? row["Amenity4"].ToString() : null;
                            responseModel.Amenity5 = row["Amenity5"] != DBNull.Value ? row["Amenity5"].ToString() : null;
                            responseModel.Amenity6 = row["Amenity6"] != DBNull.Value ? row["Amenity6"].ToString() : null;
                            responseModel.Amenity7 = row["Amenity7"] != DBNull.Value ? row["Amenity7"].ToString() : null;
                            responseModel.Amenity8 = row["Amenity8"] != DBNull.Value ? row["Amenity8"].ToString() : null;
                            responseModel.Amenity9 = row["Amenity9"] != DBNull.Value ? row["Amenity9"].ToString() : null;
                            responseModel.Amenity10 = row["Amenity10"] != DBNull.Value ? row["Amenity10"].ToString() : null;
                            responseModel.Amenity11 = row["Amenity11"] != DBNull.Value ? row["Amenity11"].ToString() : null;
                            responseModel.Amenity12 = row["Amenity12"] != DBNull.Value ? row["Amenity12"].ToString() : null;
                            responseModel.Amenity13 = row["Amenity13"] != DBNull.Value ? row["Amenity13"].ToString() : null;
                            responseModel.Amenity14 = row["Amenity14"] != DBNull.Value ? row["Amenity14"].ToString() : null;
                            responseModel.Amenity15 = row["Amenity15"] != DBNull.Value ? row["Amenity15"].ToString() : null;
                            responseModel.Amenity16 = row["Amenity16"] != DBNull.Value ? row["Amenity16"].ToString() : null;
                            responseModel.Amenity17 = row["Amenity17"] != DBNull.Value ? row["Amenity17"].ToString() : null;
                            responseModel.Amenity18 = row["Amenity18"] != DBNull.Value ? row["Amenity18"].ToString() : null;

                            responseModel.bIsTG = row["bIsTG"] != DBNull.Value ? Boolean.Parse(row["bIsTG"].ToString()) : false;
                            responseModel.dDiscountPercent = row["dDiscountPercent"] != DBNull.Value ? Decimal.Parse(row["dDiscountPercent"].ToString()) : 0;
                            responseModel.dLatitude = row["dLatitude"] != DBNull.Value ? Decimal.Parse(row["dLatitude"].ToString()) : 0;
                            responseModel.dLongitude = row["dLongitude"] != DBNull.Value ? Decimal.Parse(row["dLongitude"].ToString()) : 0;
                            responseModel.dPrice = row["dPrice"] != DBNull.Value ? Decimal.Parse(row["dPrice"].ToString()) : 0;
                            responseModel.dPriceRP = row["dPriceRP"] != DBNull.Value ? Decimal.Parse(row["dPriceRP"].ToString()) : 0;
                            responseModel.iInventory = row["iInventory"] != DBNull.Value ? Int32.Parse(row["iInventory"].ToString()) : 0;
                            //responseModel.iRank = row["iRank"] != DBNull.Value ? Int32.Parse(row["iRank"].ToString()) : 0;
                            // isFewRoomsAvailable = row["isFewRoomsAvailable"] != DBNull.Value ? Boolean.Parse(row["isFewRoomsAvailable"].ToString()) : false;
                            responseModel.iStarCategoryId = row["iStarCategoryId"] != DBNull.Value ? Int16.Parse(row["iStarCategoryId"].ToString()) : default(Int16);
                            responseModel.iVendorId = row["iVendorId"] != DBNull.Value ? row["iVendorId"].ToString() : null;
                            responseModel.LastBook = row["LastBook"] != DBNull.Value ? row["LastBook"].ToString() : null;
                            responseModel.Looking = row["Looking"] != DBNull.Value ? Int32.Parse(row["Looking"].ToString()) : 0;
                            responseModel.ranking_string = row["ranking_string"] != DBNull.Value ? row["ranking_string"].ToString() : null;
                            responseModel.rating = row["rating"] != DBNull.Value ? Decimal.Parse(row["rating"].ToString()) : 0;
                            responseModel.rating_image_url = row["rating_image_url"] != DBNull.Value ? row["rating_image_url"].ToString() : null;
                            //sDescription = row["sDescription"] != DBNull.Value ? row["sDescription"].ToString() : null;
                            responseModel.sHotelName = row["sHotelName"] != DBNull.Value ? row["sHotelName"].ToString() : null;
                            responseModel.sImgUrl = row["sImgUrl"] != DBNull.Value ? row["sImgUrl"].ToString() : null;
                            responseModel.sLocality = row["sLocality"] != DBNull.Value ? row["sLocality"].ToString() : null;
                            responseModel.sOffer = row["sOffer"] != DBNull.Value ? Boolean.Parse(row["sOffer"].ToString()) : false;
                            //sOffer = row["sOffer"] != DBNull.Value ? row["sOffer"].ToString() : null;
                            responseModel.Sponsored = row["Sponsored"] != DBNull.Value ? (Boolean.Parse(row["Sponsored"].ToString()) ? 1 : 0) : 0;

                            responseModel.bIsFavourite = int.Parse(row["bIsFavourite"].ToString()) == 1 ? true : false;

                            responseModel.bIsTopHotel = int.Parse(row["bIsTopHotel"].ToString()) == 1 ? true : false;
                            responseModel.bIsPopularHotel = int.Parse(row["bIsPopularHotel"].ToString()) == 1 ? true : false;
                            responseModel.bIsNew = int.Parse(row["bIsNew"].ToString()) == 1 ? true : false;
                            responseModel.bIsHighDemand = int.Parse(row["bIsHighDemand"].ToString()) == 1 ? true : false;

                            propertyList.Add(responseModel);
                        }
                        sCurrencySymbol = ds.Tables[1].Rows[0][0].ToString();
                    }
                }

                foreach (var item in propertyList)
                {
                    item.FirstFourNonEmptyAmenity = GetFirstFourAmenityUrl(item);
                }

                return propertyList;
            }
        }
        public static List<PropSearchResponseModel> GetHotelsBySearchQCorporate(PropSearchRequestModel searchRequestModel, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch, string sCurrency, out string sCurrencySymbol)
        {
            sCurrencySymbol = string.Empty;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var propertyList = new List<PropSearchResponseModel>();

                #region SqlParameter

                SqlParameter[] MyParam = new SqlParameter[40];

                MyParam[0] = new SqlParameter("@iPlaceID", searchRequestModel.cid);
                MyParam[1] = new SqlParameter("@dtCheckIn", searchRequestModel.dtCheckIn);
                MyParam[2] = new SqlParameter("@dtCheckOut", searchRequestModel.dtCheckOut);


                MyParam[3] = new SqlParameter("@iAreaType", searchRequestModel.iAreaType);
                MyParam[4] = new SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                MyParam[5] = new SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);

                MyParam[4].TypeName = "dbo.RoomOccupancySearch";
                MyParam[5].TypeName = "dbo.ChildrenAgeSearch";



                MyParam[6] = new SqlParameter("@sHotelSearch", searchRequestModel.sHotelSearch);
                MyParam[7] = new SqlParameter("@iFilterLocality", searchRequestModel.iFilterLocality);
                MyParam[8] = new SqlParameter("@sStarRating", searchRequestModel.sStarRating);
                MyParam[9] = new SqlParameter("@dMinPrice", searchRequestModel.dMinPrice);
                MyParam[10] = new SqlParameter("@dMaxPrice", searchRequestModel.dMaxPrice);
                MyParam[11] = new SqlParameter("@bLogin", searchRequestModel.bLogin);
                MyParam[12] = new SqlParameter("@SpecialDeal", searchRequestModel.isSpecialDeal);
                MyParam[13] = new SqlParameter("@sCurrencyCode", sCurrency);

                #region RoomFacilities

                MyParam[14] = new SqlParameter("@bIsAirConditioning", searchRequestModel.IsAirConditioning);
                MyParam[15] = new SqlParameter("@bIsBathtub", searchRequestModel.IsBathtub);
                MyParam[16] = new SqlParameter("@bIsFlatScreenTV", searchRequestModel.IsFlatScreenTV);
                MyParam[17] = new SqlParameter("@bIsSoundproof", searchRequestModel.IsSoundproof);
                MyParam[18] = new SqlParameter("@bIsView", searchRequestModel.IsView);
                MyParam[19] = new SqlParameter("@bIsInternetFacilities", searchRequestModel.IsInternetFacilities);
                MyParam[20] = new SqlParameter("@bIsPrivatePool", searchRequestModel.IsPrivatePool);
                MyParam[21] = new SqlParameter("@bIsRoomService", searchRequestModel.IsRoomService);

                #endregion RoomFacilities

                #region HotelFacilities

                MyParam[22] = new SqlParameter("@Internet", searchRequestModel.IsInternet);
                MyParam[23] = new SqlParameter("@Parking", searchRequestModel.IsParking);
                MyParam[24] = new SqlParameter("@NonSmoking", searchRequestModel.IsNonSmoking);
                MyParam[25] = new SqlParameter("@FacilitiesDifferentlyAbled", searchRequestModel.IsFacilitiesDifferentlyAbled);
                MyParam[26] = new SqlParameter("@FitnessCenter", searchRequestModel.IsFitnessCenter);
                MyParam[27] = new SqlParameter("@FamilyRooms", searchRequestModel.IsFamilyRooms);
                MyParam[28] = new SqlParameter("@Restaurant", searchRequestModel.IsRestaurant);
                MyParam[29] = new SqlParameter("@RoomService", searchRequestModel.IsHotelRoomService);
                MyParam[30] = new SqlParameter("@Transfers", searchRequestModel.IsTransfers);
                MyParam[31] = new SqlParameter("@PetFriendly", searchRequestModel.IsPetFriendly);
                MyParam[32] = new SqlParameter("@Spa", searchRequestModel.IsSpa);
                MyParam[33] = new SqlParameter("@OutdoorPool", searchRequestModel.IsOutdoorPool);
                MyParam[34] = new SqlParameter("@IndoorPool", searchRequestModel.IsIndoorPool);
                MyParam[35] = new SqlParameter("@Childcare", searchRequestModel.IsChildcare);
                MyParam[36] = new SqlParameter("@ChildrensClub", searchRequestModel.IsChildrensClub);
                MyParam[37] = new SqlParameter("@LaundryServices", searchRequestModel.IsLaundryServices);
                MyParam[38] = new SqlParameter("@AirconditionerHotel", searchRequestModel.IsAirconditionerHotel);

                #endregion HotelFacilities

                MyParam[39] = new SqlParameter("@iUserId", searchRequestModel.iUserId);

                #endregion

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspGetSearchedHotelsDetailsCorp", MyParam);
                    if (ds != null && ds.Tables != null)
                    {
                       foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var responseModel = new PropSearchResponseModel();
                            
                            responseModel.iPropId = row["iPropId"] != DBNull.Value ? Int32.Parse(row["iPropId"].ToString()) : 0;
                            responseModel.Amenity1 = row["Amenity1"] != DBNull.Value ? row["Amenity1"].ToString() : null;
                            responseModel.Amenity2 = row["Amenity2"] != DBNull.Value ? row["Amenity2"].ToString() : null;
                            responseModel.Amenity3 = row["Amenity3"] != DBNull.Value ? row["Amenity3"].ToString() : null;
                            responseModel.Amenity4 = row["Amenity4"] != DBNull.Value ? row["Amenity4"].ToString() : null;
                            responseModel.Amenity5 = row["Amenity5"] != DBNull.Value ? row["Amenity5"].ToString() : null;
                            responseModel.Amenity6 = row["Amenity6"] != DBNull.Value ? row["Amenity6"].ToString() : null;
                            responseModel.Amenity7 = row["Amenity7"] != DBNull.Value ? row["Amenity7"].ToString() : null;
                            responseModel.Amenity8 = row["Amenity8"] != DBNull.Value ? row["Amenity8"].ToString() : null;
                            responseModel.Amenity9 = row["Amenity9"] != DBNull.Value ? row["Amenity9"].ToString() : null;
                            responseModel.Amenity10 = row["Amenity10"] != DBNull.Value ? row["Amenity10"].ToString() : null;
                            responseModel.Amenity11 = row["Amenity11"] != DBNull.Value ? row["Amenity11"].ToString() : null;
                            responseModel.Amenity12 = row["Amenity12"] != DBNull.Value ? row["Amenity12"].ToString() : null;
                            responseModel.Amenity13 = row["Amenity13"] != DBNull.Value ? row["Amenity13"].ToString() : null;
                            responseModel.Amenity14 = row["Amenity14"] != DBNull.Value ? row["Amenity14"].ToString() : null;
                            responseModel.Amenity15 = row["Amenity15"] != DBNull.Value ? row["Amenity15"].ToString() : null;
                            responseModel.Amenity16 = row["Amenity16"] != DBNull.Value ? row["Amenity16"].ToString() : null;
                            responseModel.Amenity17 = row["Amenity17"] != DBNull.Value ? row["Amenity17"].ToString() : null;
                            responseModel.Amenity18 = row["Amenity18"] != DBNull.Value ? row["Amenity18"].ToString() : null;

                            responseModel.bIsTG = row["bIsTG"] != DBNull.Value ? Boolean.Parse(row["bIsTG"].ToString()) : false;
                            responseModel.dDiscountPercent = row["dDiscountPercent"] != DBNull.Value ? Decimal.Parse(row["dDiscountPercent"].ToString()) : 0;
                            responseModel.dLatitude = row["dLatitude"] != DBNull.Value ? Decimal.Parse(row["dLatitude"].ToString()) : 0;
                            responseModel.dLongitude = row["dLongitude"] != DBNull.Value ? Decimal.Parse(row["dLongitude"].ToString()) : 0;
                            responseModel.dPrice = row["dPrice"] != DBNull.Value ? Decimal.Parse(row["dPrice"].ToString()) : 0;
                            responseModel.dPriceRP = row["dPriceRP"] != DBNull.Value ? Decimal.Parse(row["dPriceRP"].ToString()) : 0;
                            responseModel.iInventory = row["iInventory"] != DBNull.Value ? Int32.Parse(row["iInventory"].ToString()) : 0;
                            //responseModel.iRank = row["iRank"] != DBNull.Value ? Int32.Parse(row["iRank"].ToString()) : 0;
                            // isFewRoomsAvailable = row["isFewRoomsAvailable"] != DBNull.Value ? Boolean.Parse(row["isFewRoomsAvailable"].ToString()) : false;
                            responseModel.iStarCategoryId = row["iStarCategoryId"] != DBNull.Value ? Int16.Parse(row["iStarCategoryId"].ToString()) : default(Int16);
                            responseModel.iVendorId = row["iVendorId"] != DBNull.Value ? row["iVendorId"].ToString() : null;
                            responseModel.LastBook = row["LastBook"] != DBNull.Value ? row["LastBook"].ToString() : null;
                            responseModel.Looking = row["Looking"] != DBNull.Value ? Int32.Parse(row["Looking"].ToString()) : 0;
                            responseModel.ranking_string = row["ranking_string"] != DBNull.Value ? row["ranking_string"].ToString() : null;
                            responseModel.rating = row["rating"] != DBNull.Value ? Decimal.Parse(row["rating"].ToString()) : 0;
                            responseModel.rating_image_url = row["rating_image_url"] != DBNull.Value ? row["rating_image_url"].ToString() : null;
                            //sDescription = row["sDescription"] != DBNull.Value ? row["sDescription"].ToString() : null;
                            responseModel.sHotelName = row["sHotelName"] != DBNull.Value ? row["sHotelName"].ToString() : null;
                            responseModel.sImgUrl = row["sImgUrl"] != DBNull.Value ? row["sImgUrl"].ToString() : null;
                            responseModel.sLocality = row["sLocality"] != DBNull.Value ? row["sLocality"].ToString() : null;
                            responseModel.sOffer = row["sOffer"] != DBNull.Value ? (Convert.ToInt32(Convert.ToString(row["sOffer"])) > 0) : false;
                            //sOffer = row["sOffer"] != DBNull.Value ? row["sOffer"].ToString() : null;
                            responseModel.Sponsored = row["Sponsored"] != DBNull.Value ? (Boolean.Parse(row["Sponsored"].ToString()) ? 1 : 0) : 0;

                            responseModel.bIsFavourite = int.Parse(row["bIsFavourite"].ToString()) == 1 ? true : false;

                            responseModel.bIsTopHotel = int.Parse(row["bIsTopHotel"].ToString()) == 1 ? true : false;
                            responseModel.bIsPopularHotel = int.Parse(row["bIsPopularHotel"].ToString()) == 1 ? true : false;
                            responseModel.bIsNew = int.Parse(row["bIsNew"].ToString()) == 1 ? true : false;
                            responseModel.bIsHighDemand = int.Parse(row["bIsHighDemand"].ToString()) == 1 ? true : false;

                            propertyList.Add(responseModel);
                        }
                        sCurrencySymbol = ds.Tables[1].Rows[0][0].ToString();
                    }
                }

                foreach (var item in propertyList)
                {
                    item.FirstFourNonEmptyAmenity = GetFirstFourAmenityUrl(item);
                }

                return propertyList;
            }
        }
        /// <summary>
        /// TG hotel search based on specific criteria
        /// </summary>
        /// <param name="searchRequestModel">Model conatins the detail about the room/adult/childern and age/locality/city/area</param>
        /// <param name="dtRoomOccupancySearch"></param>
        /// <param name="dtChildrenAgeSearch"></param>
        /// <param name="sCurrency"></param>
        /// <param name="dExchangeRate"></param>
        /// <returns></returns>
        public static List<PropSearchResponseModel> GetHotelsForTG(PropSearchRequestModel searchRequestModel, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch, string sCurrency, out decimal dExchangeRate)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                dExchangeRate = 0;
                var propertyList = new List<PropSearchResponseModel>();

                #region SqlParameter

                SqlParameter[] MyParam = new SqlParameter[40];

                MyParam[0] = new SqlParameter("@iPlaceID", searchRequestModel.cid);
                MyParam[1] = new SqlParameter("@dtCheckIn", searchRequestModel.dtCheckIn);
                MyParam[2] = new SqlParameter("@dtCheckOut", searchRequestModel.dtCheckOut);


                MyParam[3] = new SqlParameter("@iAreaType", searchRequestModel.iAreaType);
                MyParam[4] = new SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
                MyParam[5] = new SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);

                MyParam[4].TypeName = "dbo.RoomOccupancySearch";
                MyParam[5].TypeName = "dbo.ChildrenAgeSearch";



                MyParam[6] = new SqlParameter("@sHotelSearch", searchRequestModel.sHotelSearch);
                MyParam[7] = new SqlParameter("@iFilterLocality", searchRequestModel.iFilterLocality);
                MyParam[8] = new SqlParameter("@sStarRating", searchRequestModel.sStarRating);
                MyParam[9] = new SqlParameter("@dMinPrice", searchRequestModel.dMinPrice);
                MyParam[10] = new SqlParameter("@dMaxPrice", searchRequestModel.dMaxPrice);
                MyParam[11] = new SqlParameter("@bLogin", searchRequestModel.bLogin);
                MyParam[12] = new SqlParameter("@SpecialDeal", searchRequestModel.isSpecialDeal);
                MyParam[13] = new SqlParameter("@sCurrencyCode", sCurrency);

                #region RoomFacilities

                MyParam[14] = new SqlParameter("@bIsAirConditioning", searchRequestModel.IsAirConditioning);
                MyParam[15] = new SqlParameter("@bIsBathtub", searchRequestModel.IsBathtub);
                MyParam[16] = new SqlParameter("@bIsFlatScreenTV", searchRequestModel.IsFlatScreenTV);
                MyParam[17] = new SqlParameter("@bIsSoundproof", searchRequestModel.IsSoundproof);
                MyParam[18] = new SqlParameter("@bIsView", searchRequestModel.IsView);
                MyParam[19] = new SqlParameter("@bIsInternetFacilities", searchRequestModel.IsInternetFacilities);
                MyParam[20] = new SqlParameter("@bIsPrivatePool", searchRequestModel.IsPrivatePool);
                MyParam[21] = new SqlParameter("@bIsRoomService", searchRequestModel.IsRoomService);

                #endregion RoomFacilities

                #region HotelFacilities

                MyParam[22] = new SqlParameter("@Internet", searchRequestModel.IsInternet);
                MyParam[23] = new SqlParameter("@Parking", searchRequestModel.IsParking);
                MyParam[24] = new SqlParameter("@NonSmoking", searchRequestModel.IsNonSmoking);
                MyParam[25] = new SqlParameter("@FacilitiesDifferentlyAbled", searchRequestModel.IsFacilitiesDifferentlyAbled);
                MyParam[26] = new SqlParameter("@FitnessCenter", searchRequestModel.IsFitnessCenter);
                MyParam[27] = new SqlParameter("@FamilyRooms", searchRequestModel.IsFamilyRooms);
                MyParam[28] = new SqlParameter("@Restaurant", searchRequestModel.IsRestaurant);
                MyParam[29] = new SqlParameter("@RoomService", searchRequestModel.IsHotelRoomService);
                MyParam[30] = new SqlParameter("@Transfers", searchRequestModel.IsTransfers);
                MyParam[31] = new SqlParameter("@PetFriendly", searchRequestModel.IsPetFriendly);
                MyParam[32] = new SqlParameter("@Spa", searchRequestModel.IsSpa);
                MyParam[33] = new SqlParameter("@OutdoorPool", searchRequestModel.IsOutdoorPool);
                MyParam[34] = new SqlParameter("@IndoorPool", searchRequestModel.IsIndoorPool);
                MyParam[35] = new SqlParameter("@Childcare", searchRequestModel.IsChildcare);
                MyParam[36] = new SqlParameter("@ChildrensClub", searchRequestModel.IsChildrensClub);
                MyParam[37] = new SqlParameter("@LaundryServices", searchRequestModel.IsLaundryServices);
                MyParam[38] = new SqlParameter("@AirconditionerHotel", searchRequestModel.IsAirconditionerHotel);

                #endregion HotelFacilities

                MyParam[39] = new SqlParameter("@iUserId", searchRequestModel.iUserId);

                #endregion

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspGetHotelsDetailsForTG", MyParam);
                    if (ds != null && ds.Tables != null)
                    {
                        //propertyList = ds.Tables[0].DataTableToList<PropSearchResponseModel>();

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var responseModel = new PropSearchResponseModel();

                            responseModel.iPropId = row["iPropId"] != DBNull.Value ? Int32.Parse(row["iPropId"].ToString()) : 0;
                            responseModel.Amenity1 = row["Amenity1"] != DBNull.Value ? row["Amenity1"].ToString() : null;
                            responseModel.Amenity2 = row["Amenity2"] != DBNull.Value ? row["Amenity2"].ToString() : null;
                            responseModel.Amenity3 = row["Amenity3"] != DBNull.Value ? row["Amenity3"].ToString() : null;
                            responseModel.Amenity4 = row["Amenity4"] != DBNull.Value ? row["Amenity4"].ToString() : null;
                            responseModel.Amenity5 = row["Amenity5"] != DBNull.Value ? row["Amenity5"].ToString() : null;
                            responseModel.Amenity6 = row["Amenity6"] != DBNull.Value ? row["Amenity6"].ToString() : null;
                            responseModel.Amenity7 = row["Amenity7"] != DBNull.Value ? row["Amenity7"].ToString() : null;
                            responseModel.Amenity8 = row["Amenity8"] != DBNull.Value ? row["Amenity8"].ToString() : null;
                            responseModel.Amenity9 = row["Amenity9"] != DBNull.Value ? row["Amenity9"].ToString() : null;
                            responseModel.Amenity10 = row["Amenity10"] != DBNull.Value ? row["Amenity10"].ToString() : null;
                            responseModel.Amenity11 = row["Amenity11"] != DBNull.Value ? row["Amenity11"].ToString() : null;
                            responseModel.Amenity12 = row["Amenity12"] != DBNull.Value ? row["Amenity12"].ToString() : null;
                            responseModel.Amenity13 = row["Amenity13"] != DBNull.Value ? row["Amenity13"].ToString() : null;
                            responseModel.Amenity14 = row["Amenity14"] != DBNull.Value ? row["Amenity14"].ToString() : null;
                            responseModel.Amenity15 = row["Amenity15"] != DBNull.Value ? row["Amenity15"].ToString() : null;
                            responseModel.Amenity16 = row["Amenity16"] != DBNull.Value ? row["Amenity16"].ToString() : null;
                            responseModel.Amenity17 = row["Amenity17"] != DBNull.Value ? row["Amenity17"].ToString() : null;
                            responseModel.Amenity18 = row["Amenity18"] != DBNull.Value ? row["Amenity18"].ToString() : null;

                            responseModel.bIsTG = row["bIsTG"] != DBNull.Value ? Boolean.Parse(row["bIsTG"].ToString()) : false;
                            responseModel.dDiscountPercent = row["dDiscountPercent"] != DBNull.Value ? Decimal.Parse(row["dDiscountPercent"].ToString()) : 0;
                            responseModel.dLatitude = row["dLatitude"] != DBNull.Value ? Decimal.Parse(row["dLatitude"].ToString()) : 0;
                            responseModel.dLongitude = row["dLongitude"] != DBNull.Value ? Decimal.Parse(row["dLongitude"].ToString()) : 0;
                            responseModel.dPrice = row["dPrice"] != DBNull.Value ? Decimal.Parse(row["dPrice"].ToString()) : 0;
                            responseModel.dPriceRP = row["dPriceRP"] != DBNull.Value ? Decimal.Parse(row["dPriceRP"].ToString()) : 0;
                            responseModel.iInventory = row["iInventory"] != DBNull.Value ? Int32.Parse(row["iInventory"].ToString()) : 0;
                            responseModel.iRank = row["iRank"] != DBNull.Value ? Int32.Parse(row["iRank"].ToString()) : 0;
                            // isFewRoomsAvailable = row["isFewRoomsAvailable"] != DBNull.Value ? Boolean.Parse(row["isFewRoomsAvailable"].ToString()) : false;
                            responseModel.iStarCategoryId = row["iStarCategoryId"] != DBNull.Value ? Int16.Parse(row["iStarCategoryId"].ToString()) : default(Int16);
                            responseModel.iVendorId = row["iVendorId"] != DBNull.Value ? row["iVendorId"].ToString() : null;
                            responseModel.LastBook = row["LastBook"] != DBNull.Value ? row["LastBook"].ToString() : null;
                            responseModel.Looking = row["Looking"] != DBNull.Value ? Int32.Parse(row["Looking"].ToString()) : 0;
                            responseModel.ranking_string = row["ranking_string"] != DBNull.Value ? row["ranking_string"].ToString() : null;
                            responseModel.rating = row["rating"] != DBNull.Value ? Decimal.Parse(row["rating"].ToString()) : 0;
                            responseModel.rating_image_url = row["rating_image_url"] != DBNull.Value ? row["rating_image_url"].ToString() : null;
                            //sDescription = row["sDescription"] != DBNull.Value ? row["sDescription"].ToString() : null;
                            responseModel.sHotelName = row["sHotelName"] != DBNull.Value ? row["sHotelName"].ToString() : null;
                            responseModel.sImgUrl = row["sImgUrl"] != DBNull.Value ? row["sImgUrl"].ToString() : null;
                            responseModel.sLocality = row["sLocality"] != DBNull.Value ? row["sLocality"].ToString() : null;
                            //sOffer = row["sOffer"] != DBNull.Value ? row["sOffer"].ToString() : null;
                            //responseModel.Sponsored = row["Sponsored"] != DBNull.Value ? Int32.Parse(row["Sponsored"].ToString()) : 0;

                            responseModel.iVendorId = row["iVendorId"] != DBNull.Value ? row["iVendorId"].ToString() : null;
                            responseModel.bIsFavourite = int.Parse(row["bIsFavourite"].ToString()) == 1 ? true : false;

                            responseModel.bIsTopHotel = int.Parse(row["bIsTopHotel"].ToString()) == 1 ? true : false;
                            responseModel.bIsPopularHotel = int.Parse(row["bIsPopularHotel"].ToString()) == 1 ? true : false;
                            responseModel.bIsNew = int.Parse(row["bIsNew"].ToString()) == 1 ? true : false;
                            responseModel.bIsHighDemand = int.Parse(row["bIsHighDemand"].ToString()) == 1 ? true : false;

                            //responseModel.FirstFourNonEmptyAmenity = GetFirstFourAmenityUrl(responseModel);

                            propertyList.Add(responseModel);
                        }
                        dExchangeRate = Convert.ToDecimal(ds.Tables[1].Rows[0][0].ToString());
                    }
                }

                foreach (var item in propertyList.OrderBy(x=>x.iRank))
                {
                    item.FirstFourNonEmptyAmenity = GetFirstFourAmenityUrl(item);
                }

                return propertyList;
            }
        }


        public static List<string> GetFirstFourAmenityUrl(PropSearchResponseModel type)
        {
            List<string> firstFourAmenityUrl = new List<string>();

            var properties = type.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allAmenityProperty = properties.Where(x => x.Name.Contains("Amenity") && x.PropertyType.Name == "String").ToList();

            foreach (var item in allAmenityProperty)
            {
                var tValue = type.GetType().GetProperty(item.Name).GetValue(type, null).ToString();

                if (!string.IsNullOrEmpty(tValue))
                {
                    firstFourAmenityUrl.Add(tValue);
                }
            }
            return firstFourAmenityUrl.Take(4).ToList();
        }



        public static List<eDropDown> GetAllHotelsLocality(string txt, int PlaceId, int Type, int iReqType)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eDropDown> eobj = new List<eDropDown>();
                SqlParameter[] MyParam = new SqlParameter[4];
                MyParam[0] = new SqlParameter("@iPlaceID", PlaceId);
                MyParam[1] = new SqlParameter("@iAreaType", Type);
                MyParam[2] = new SqlParameter("@sLocalitySearch", txt);
                MyParam[3] = new SqlParameter("@iReqType", iReqType);
                var result = db.Database.SqlQuery<eDropDown>("uspGetSearchedHotelsLocality @iPlaceID,@iAreaType,@sLocalitySearch,@iReqType ", MyParam).ToList();
                foreach (var item in result)
                {
                    eobj.Add((eDropDown)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eDropDown()));
                }
                return eobj;
            }
        }

        //Get All hotel facilities
        public static List<CheckBoxList> GetAllHotelFacilities(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                selectItems = db.tblHotelFacilitesRankMs.Select(x => new CheckBoxList()
                {
                    Text = x.sHotelFacilites,
                    Value = x.iHoteFacilityId
                }).ToList();

                return selectItems;
            }
        }

        //Get All room facilities
        public static List<CheckBoxList> GetAllRoomFacilities(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                selectItems = db.tblRoomFacilityDropDownMs.Select(x => new CheckBoxList()
                {
                    Text = x.sRoomFacilites,
                    Value = x.iRoomFacilityId
                }).ToList();

                return selectItems;
            }
        }

        //Get All room facilities
        public static List<PropDetailsM> GetDummyDataToShowOnMap()
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbList = (from p in db.tblPropertyMs
                              join m in db.tblPropertyPhotoMaps on p.iPropId equals m.iPropId
                              select new PropDetailsM
                              {
                                  sPropertyName = p.sHotelName,
                                  sPropertyAddress = p.sAddress,
                                  sDescription = p.sDescription,
                                  dLongitude = p.dLongitude,
                                  dLatitude = p.dLatitude,
                                  slargeMapURL = m.sImgUrl
                              }).Take(10).ToList();

                return dbList;
            }
        }

        public static int ToggleFavs(string iPropId, string iUserId)
        {
            int retrn = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                #region SqlParameter

                SqlParameter[] MyParam = new SqlParameter[2];

                MyParam[0] = new SqlParameter("@iUserId", iUserId);
                MyParam[1] = new SqlParameter("@ID", iPropId);

                #endregion

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    retrn = Convert.ToInt32(SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspManageUserWishList", MyParam).Tables[0].Rows[0][0].ToString());
                }

                return retrn;
            }
        }
    }


    public static class Helper
    {
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}