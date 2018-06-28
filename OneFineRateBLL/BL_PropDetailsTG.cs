using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_PropDetailsTG
    {
        public static PropDetailsM GetPropertyDetails(string sVendorId, string roomTypeIds)
        {
            PropDetailsM propM = new PropDetailsM();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region General Details

                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@VendorId", sVendorId);
                propM = db.Database.SqlQuery<PropDetailsM>("uspGetOfferReviewDetailsbyID_TG @VendorId", MyParam).FirstOrDefault();

                propM.TG_Hotel = new TG_Hotel();
                propM.TG_Hotel.RoomAmenities = GetRoomAmenities(sVendorId, roomTypeIds);

                SqlParameter[] tripAdvisorParam = new SqlParameter[1];
                tripAdvisorParam[0] = new SqlParameter("@VendorId", sVendorId);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetTripAdvisorRating_TG", tripAdvisorParam);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    propM.objTripAdvisonReviews = (from rw in ds.Tables[0].AsEnumerable()
                                                   select new etblPropertyTripAdvisorM()
                                                   {
                                                       iRating = Convert.ToDecimal(rw["iRating"]),
                                                       sRatingImageURL = Convert.ToString(rw["sRatingImageURL"]),
                                                       sRankingString = Convert.ToString(rw["sRankingString"]),
                                                       iReviewsCount = Convert.ToInt32(rw["iReviewsCount"]),
                                                       sWebURL = Convert.ToString(rw["sWebURL"])
                                                   }).FirstOrDefault();

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    propM.lstTripAdvisonReviews = (from rw in ds.Tables[1].AsEnumerable()
                                                   select new etblTripAdvisorReviews()
                                                   {
                                                       iReviewId = Convert.ToInt32(rw["iReviewId"]),
                                                       sTitle = Convert.ToString(rw["sTitle"]),
                                                       sText = Convert.ToString(rw["sText"]),
                                                       sReviewer = Convert.ToString(rw["sReviewer"]),
                                                       iRating = Convert.ToDecimal(rw["iRating"]),
                                                       sRatingImageURL = Convert.ToString(rw["sRatingImageURL"]),
                                                       sReviewURL = Convert.ToString(rw["sReviewURL"]),
                                                       sTripType = Convert.ToString(rw["sTripType"])
                                                   }).ToList();

                }

                SqlParameter[] hotelFacilitiesParam = new SqlParameter[1];
                hotelFacilitiesParam[0] = new SqlParameter("@VendorId", sVendorId);
                var objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmentiesTG @VendorId", hotelFacilitiesParam).ToList();
                propM.lstetblHotelFacilities = objresultHotelFacilities;

                string[] arrMapURL = new string[50];
                string sPropertyNameresult = "";
                arrMapURL = propM.sPropertyName.Split(' ');
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
                sPropertyNameresult += "+" + propM.sCity;
                propM.slargeMapURL = "https://www.google.com/maps/place/" + sPropertyNameresult + "/@" + propM.dLatitude + "," + propM.dLongitude + ",17z?hl=en-US";

                #endregion


                #region Gallery Details

                DataSet dsGallery = new DataSet();

                using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];

                    MyParams[0] = new System.Data.SqlClient.SqlParameter("@VendorId", sVendorId);
                    dsGallery = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetPropertyPhotosTG", MyParams);
                }

                List<etblPhotoGallery> objresulttblPhotoGallery = new List<etblPhotoGallery>();
                for (int lstphoto = 0; lstphoto < dsGallery.Tables[0].Rows.Count; lstphoto++)
                {
                    objresulttblPhotoGallery.Add(new etblPhotoGallery()
                    {
                        //iVendorId = dsGallery.Tables[0].Rows[lstphoto]["sHotelCode"].ToString(),
                        sMainImgUrl = dsGallery.Tables[0].Rows[lstphoto]["sImageUrl"].ToString(),
                        // sthumbImgUrl = dsGallery.Tables[0].Rows[lstphoto]["sThumbImgUrl"].ToString(),
                        bIsPrimaryH = dsGallery.Tables[0].Rows[lstphoto]["bIsPrimaryH"] == "1" ? true : false,

                    });
                }
                propM.lstetblPhotoGallery = objresulttblPhotoGallery;

                //List<etblHotelFacilities> objresultetblHotelFacilities = new List<etblHotelFacilities>();
                //for (int lstHotelFacilities = 0; lstHotelFacilities < dsGallery.Tables[1].Rows.Count; lstHotelFacilities++)
                //{
                //    objresultetblHotelFacilities.Add(new etblHotelFacilities()
                //    {
                //        iHoteFacilityId = Convert.ToInt32(dsGallery.Tables[1].Rows[lstHotelFacilities]["iAmenityId"].ToString()),
                //        sHotelFacilites = dsGallery.Tables[1].Rows[lstHotelFacilities]["sDescription"].ToString(),
                //    });
                //}
                //propM.lstetblHotelFacilities = objresultetblHotelFacilities;

                #endregion


                #region DistanceToAirportRailway

                if (propM.sDistanceToAirportRailway1 != null && (propM.dDistanceToAirportRailway1 != null || propM.dDistanceToAirportRailway1 != 0))
                {
                    propM.sDistanceToAirportRailway = propM.sDistanceToAirportRailway1 + "(" + propM.dDistanceToAirportRailway1 + " Km) ";
                }

                if (propM.sDistanceToAirportRailway2 != null && (propM.dDistanceToAirportRailway2 != null || propM.dDistanceToAirportRailway2 != 0))
                {
                    propM.sDistanceToAirportRailway = propM.sDistanceToAirportRailway + propM.sDistanceToAirportRailway2 + "(" + propM.dDistanceToAirportRailway2 + " Km) ";

                }

                if (propM.sDistanceToAirportRailway3 != null && (propM.dDistanceToAirportRailway3 != null || propM.dDistanceToAirportRailway3 != 0))
                {
                    propM.sDistanceToAirportRailway = propM.sDistanceToAirportRailway + propM.sDistanceToAirportRailway3 + "(" + propM.dDistanceToAirportRailway3 + " Km)";
                }
                #endregion

                #region NearestTransport1

                if (propM.sNearestTransport1 != null && (propM.dNearestTransport1 != null || propM.dNearestTransport1 != 0))
                {
                    propM.sNearestTransport = propM.sNearestTransport1 + "(" + propM.dNearestTransport1 + " Km) ";
                }

                if (propM.sNearestTransport2 != null && (propM.dNearestTransport2 != null || propM.dNearestTransport2 != 0))
                {
                    propM.sNearestTransport = propM.sNearestTransport + propM.sNearestTransport2 + "(" + propM.dNearestTransport2 + " Km) ";
                }

                if (propM.sNearestTransport3 != null && (propM.dNearestTransport3 != null || propM.dNearestTransport3 != 0))
                {
                    propM.sNearestTransport = propM.sNearestTransport + propM.sNearestTransport3 + "(" + propM.dNearestTransport3 + " Km) ";
                }

                #endregion


                return propM;
            }
        }

        public static List<TG_RoomAmenity> GetRoomAmenities(string sVendorId, string roomTypeIds)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] roomAmenityParam = new SqlParameter[2];
                roomAmenityParam[0] = new SqlParameter("@VendorId", sVendorId);
                roomAmenityParam[1] = new SqlParameter("@iRoomTypeId", roomTypeIds);
                var roomAmenities = db.Database.SqlQuery<TG_RoomAmenity>("uspGetRoomAmenitiesTG @VendorId,@iRoomTypeId", roomAmenityParam).ToList();

                return roomAmenities;
            }
        }

        public static RoomDetails GetRoomOccupancyDetails(string sVendorId, string sRoomTypeId)
        {
            RoomDetails objRoomDetails = null;
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var roomDescription = db.tblPropertyRoomDescriptionTGs.Where(x => x.iVendorId == sVendorId && x.iRoomTypeId == sRoomTypeId).FirstOrDefault();

                    if (roomDescription != null)
                    {
                        objRoomDetails = new RoomDetails();

                        objRoomDetails.MaxAdultOccupancy = roomDescription.iMax_Adult_Occupancy.Value;

                        objRoomDetails.MaxChildOccupancy = roomDescription.iMax_Child_Occupancy.Value;

                        objRoomDetails.sDefaultImage = roomDescription.sRoomType_ImagePath;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objRoomDetails;
        }


        public static RoomDetails GetRoomDetails(string sVendorId, string sRoomTypeId)
        {
            RoomDetails objRoomDetails = new RoomDetails();
            List<RoomImages> _arrRoomImages = new List<RoomImages>();
            List<RoomPolicy> _arrRoomPolicy = new List<RoomPolicy>();
            List<RoomAmenities> _arrRoomAmenities = new List<RoomAmenities>();
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var roomDescription = db.tblPropertyRoomDescriptionTGs.Where(x => x.iVendorId == sVendorId && x.iRoomTypeId == sRoomTypeId).FirstOrDefault();

                    if (roomDescription != null)
                    {
                        _arrRoomImages.Add(new RoomImages
                        {
                            sRoomImage = roomDescription.sRoomType_ImagePath
                        });
                        objRoomDetails.sDefaultImage = roomDescription.sRoomType_ImagePath;
                        objRoomDetails.sRoomName = roomDescription.sRoomType;
                        objRoomDetails.sDescription = roomDescription.sDescription;
                        objRoomDetails.Policy = new List<RoomPolicy>();
                        objRoomDetails.Policy.Add(new RoomPolicy
                        {
                            iMax_Adult_Occupancy = roomDescription.iMax_Adult_Occupancy,
                            iMax_Child_Occupancy = roomDescription.iMax_Child_Occupancy,
                            iMax_Guest_Occupancy = roomDescription.iMax_Guest_Occupancy,
                            iMax_Infant_Occupancy = roomDescription.iMax_Infant_Occupancy

                        });

                        var roomAmenities = db.tblPropertyRoomLevelAmenityTGs.Where(x => x.iVendorId == roomDescription.iVendorId && x.iRoomTypeId == roomDescription.iRoomTypeId).ToList();
                        if (roomAmenities.Count > 0)
                        {
                            objRoomDetails.RoomAmenities = new List<RoomAmenities>();
                            foreach (var item in roomAmenities)
                            {
                                objRoomDetails.RoomAmenities.Add(new RoomAmenities
                                {
                                    sAmenity = item.sDescription
                                });
                            }
                        }
                    }
                }

                //SqlParameter[] MyParam = new SqlParameter[2];
                //MyParam[0] = new SqlParameter("@sVendorId", sVendorId);
                //MyParam[1] = new SqlParameter("@sRoomTypeId", sRoomTypeId);
                //DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetRoomDetailsTG", MyParam);

            }
            catch (Exception ex) { throw ex; }

            objRoomDetails.Images = _arrRoomImages;

            return objRoomDetails;
        }

        public static List<TG_HotelAmenity> GetHotelAmenities(int[] amenityCategoryIds)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                // Call Proc

                var result = db.Database.SqlQuery<TG_HotelAmenity>("uspGetHotelFacilitiesByHeaderTG @sAmenityIds",
                    new SqlParameter("@sAmenityIds", string.Join(",", amenityCategoryIds))).ToList();
                //.Select(x => new TG_HotelAmenity
                //{
                //    code = x.Columns["iAmenityId"].ToString(),
                //    Description = x.Columns["sHeader"].ToString()
                //}).ToList();

                return result;
            }
        }

        public static PropDetailsM GetPropertyDetailsWithoutGallery(string sVendorId, string roomTypeIds)
        {
            PropDetailsM propM = new PropDetailsM();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                #region General Details

                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@VendorId", sVendorId);
                propM = db.Database.SqlQuery<PropDetailsM>("uspGetOfferReviewDetailsbyID_TG @VendorId", MyParam).FirstOrDefault();

                propM.TG_Hotel = new TG_Hotel();
                propM.TG_Hotel.RoomAmenities = GetRoomAmenities(sVendorId, roomTypeIds);

                SqlParameter[] tripAdvisorParam = new SqlParameter[1];
                tripAdvisorParam[0] = new SqlParameter("@VendorId", sVendorId);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetTripAdvisorRating_TG", tripAdvisorParam);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    propM.objTripAdvisonReviews = (from rw in ds.Tables[0].AsEnumerable()
                                                   select new etblPropertyTripAdvisorM()
                                                   {
                                                       iRating = Convert.ToDecimal(rw["iRating"]),
                                                       sRatingImageURL = Convert.ToString(rw["sRatingImageURL"]),
                                                       sRankingString = Convert.ToString(rw["sRankingString"]),
                                                       iReviewsCount = Convert.ToInt32(rw["iReviewsCount"]),
                                                       sWebURL = Convert.ToString(rw["sWebURL"])
                                                   }).FirstOrDefault();

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    propM.lstTripAdvisonReviews = (from rw in ds.Tables[1].AsEnumerable()
                                                   select new etblTripAdvisorReviews()
                                                   {
                                                       iReviewId = Convert.ToInt32(rw["iReviewId"]),
                                                       sTitle = Convert.ToString(rw["sTitle"]),
                                                       sText = Convert.ToString(rw["sText"]),
                                                       sReviewer = Convert.ToString(rw["sReviewer"]),
                                                       iRating = Convert.ToDecimal(rw["iRating"]),
                                                       sRatingImageURL = Convert.ToString(rw["sRatingImageURL"]),
                                                       sReviewURL = Convert.ToString(rw["sReviewURL"]),
                                                       sTripType = Convert.ToString(rw["sTripType"])
                                                   }).ToList();

                }

                SqlParameter[] hotelFacilitiesParam = new SqlParameter[1];
                hotelFacilitiesParam[0] = new SqlParameter("@VendorId", sVendorId);
                var objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmentiesTG @VendorId", hotelFacilitiesParam).ToList();
                propM.lstetblHotelFacilities = objresultHotelFacilities;


                #endregion

                return propM;
            }
        }


        public static PropDetailsM GetPropertyFacility(string sVendorId)
        {
            PropDetailsM propM = new PropDetailsM();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                #region General Details

                var prop = db.tblPropertyMs.Where(x => x.iVendorId == sVendorId).Select(x => new { x.sAddress, x.sHotelName, x.iPropId }).FirstOrDefault();

                propM.sPropertyAddress = prop.sAddress;
                propM.sPropertyName = prop.sHotelName;
                propM.iPropId = prop.iPropId;

                SqlParameter[] hotelFacilitiesParam = new SqlParameter[1];
                hotelFacilitiesParam[0] = new SqlParameter("@VendorId", sVendorId);
                var objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmentiesTG @VendorId", hotelFacilitiesParam).ToList();
                propM.lstetblHotelFacilities = objresultHotelFacilities;

                #endregion

                return propM;
            }
        }

        public static List<etblHotelFacilities> GetFirstFourHotelFacilities(long iPropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var vendorId = db.tblPropertyMs.Where(x => x.iPropId == iPropId).Select(x => new { x.iVendorId }).FirstOrDefault();
                SqlParameter[] hotelFacilitiesParam = new SqlParameter[1];
                hotelFacilitiesParam[0] = new SqlParameter("@VendorId", vendorId.iVendorId);

                var objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmentiesTG @VendorId", hotelFacilitiesParam).Take(4).ToList();
                return objresultHotelFacilities;
            }
        }

        public static int AddUpdateRoomDetails(string sVendorId, string sRoomTypeId, int maxAdult, int maxChild, string imagePath)
        {

            var status = -1;
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var roomDescription = db.tblPropertyRoomDescriptionTGs.Where(x => x.iVendorId == sVendorId && x.iRoomTypeId == sRoomTypeId).FirstOrDefault();

                    if (roomDescription == null)
                    {
                        tblPropertyRoomDescriptionTG roomDetailsDb = new tblPropertyRoomDescriptionTG();
                        roomDetailsDb.iRoomTypeId = sRoomTypeId;
                        roomDetailsDb.iVendorId = sVendorId;
                        roomDetailsDb.iMax_Adult_Occupancy = (short)maxAdult;
                        roomDetailsDb.iMax_Child_Occupancy = (short)maxChild;
                        roomDetailsDb.sRoomType_ImagePath = imagePath;
                        db.tblPropertyRoomDescriptionTGs.Add(roomDetailsDb);
                    }
                    else
                    {
                        roomDescription.iMax_Adult_Occupancy = (short)maxAdult;
                        roomDescription.iMax_Child_Occupancy = (short)maxChild;
                        roomDescription.sRoomType_ImagePath = imagePath;
                    }

                    status = db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public static eServiceChargeM GetOfrServiceCharge(DateTime checkIn, DateTime checkOut)
        {
            var model = new eServiceChargeM();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var record = db.tblServiceChargeMs.Where(x => DbFunctions.TruncateTime(checkIn) >= DbFunctions.TruncateTime(x.dtFrom)
                && DbFunctions.TruncateTime(checkOut) <= DbFunctions.TruncateTime(x.dtTo) && x.cStatus == "A").FirstOrDefault();

                if (record != null)
                {
                    model.ServiceChargeTG = record.dServiceCharge;

                    if (record.cGstValueType == "p")
                    {
                        model.GstOnServiceChargeTG = (record.dServiceCharge * record.dGstValue) / 100;
                        model.GstOnServiceChargePercentTG = record.dGstValue;
                    }
                    else
                    {
                        model.GstOnServiceChargeTG = record.dGstValue;
                        model.GstOnServiceChargePercentTG = (record.dGstValue / record.dServiceCharge) * 100;
                    }

                    model.TotalServiceChargeTG = model.ServiceChargeTG + model.GstOnServiceChargeTG;
                    model.GstValue = record.dGstValue;
                    model.GstValueType = record.cGstValueType;
                }

                return model;
            }
        }
    }
}