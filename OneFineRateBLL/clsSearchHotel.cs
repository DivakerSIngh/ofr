using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using Newtonsoft.Json;
using System.Data.Entity;

namespace OneFineRateBLL
{
    public class clsSearchHotel
    {
        public static string CitySearch()
        {
            string data = "";
            try
            {
                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.Version = "0.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                objOTA_HotelAvailRQ.SearchCacheLevel = "VeryRecent";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();

                AddressSH objAddress = new AddressSH();
                objAddress.CityName = "New Delhi";

                CountryNameSH objCountryName = new CountryNameSH();
                objCountryName.Code = "India";
                objAddress.CountryName = objCountryName;

                HotelRef objHotelRef = new HotelRef();

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = "2016-10-05";
                objStayDateRange.End = "2016-10-06";

                RateRange objRateRange = new RateRange();
                objRateRange.MinRate = "2000";
                objRateRange.MaxRate = "10000";

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();
                RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();

                GuestCountsSH objGuestCounts = new GuestCountsSH();

                List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();


                GuestCountSH objGuestCount1 = new GuestCountSH();
                objGuestCount1.Count = "2";
                objGuestCount1.AgeQualifyingCode = "10";
                objGuestCountArr.Add(objGuestCount1);

                GuestCountSH objGuestCount2 = new GuestCountSH();
                objGuestCount2.Count = "1";
                objGuestCount2.AgeQualifyingCode = "8";
                objGuestCount2.Age = "5";
                objGuestCountArr.Add(objGuestCount2);

                //GuestCountSH objGuestCount3 = new GuestCountSH();
                //objGuestCount3.Count = "1";
                //objGuestCount3.AgeQualifyingCode = "8";
                //objGuestCount3.Age = "10";
                //objGuestCountArr.Add(objGuestCount3);

                //GuestCountSH objGuestCount1 = new GuestCountSH();
                //objGuestCount1.Count = "1";
                //objGuestCount1.AgeQualifyingCode = "10";
                //objGuestCountArr.Add(objGuestCount1);

                //GuestCountSH objGuestCount2 = new GuestCountSH();
                //objGuestCount2.Count = "1";
                //objGuestCount2.AgeQualifyingCode = "8";
                //objGuestCount2.Age = "10";
                //objGuestCountArr.Add(objGuestCount2);

                objGuestCounts.GuestCount = objGuestCountArr;

                objRoomStayCandidate.GuestCounts = objGuestCounts;

                objRoomStayCandidateArr.Add(objRoomStayCandidate);

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();

                Pagination objPagination = new Pagination();
                objPagination.Enabled = "true";
                objPagination.HotelsFrom = "01";
                objPagination.HotelsTo = "50";

                UserAuthentication objUserAuthentication = new UserAuthentication();
                objUserAuthentication.Username = "samaara";
                objUserAuthentication.PropertyId = "1300001224";
                objUserAuthentication.Password = "test@567";

                Promotion objPromotion = new Promotion();
                objPromotion.Name = "HOTEL";
                objPromotion.Type = "StayPeriod";

                //objTPA_Extensions.Promotion = objPromotion;
                objTPA_Extensions.UserAuthentication = objUserAuthentication;
                objTPA_Extensions.Pagination = objPagination;

                objCriterion.HotelRef = objHotelRef;
                objCriterion.RoomStayCandidates = objRoomStayCandidates;
                objCriterion.StayDateRange = objStayDateRange;
                objCriterion.TPA_Extensions = objTPA_Extensions;
                objCriterion.Address = objAddress;
                //objCriterion.RateRange = objRateRange;


                objHotelSearchCriteria.Criterion = objCriterion;

                objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;


                objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                objEnvelope.Body = objBody;

                string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);

                data = clsTravelGuruApi.SearchTravelGuruApi(xmlString);

                //==> Convert xml string to class object <==
                EnvelopeRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeRes>(data);

                //SaveTravelGuruData(objEnvelopeSHRes);
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public static PropDetailsM HotelspecificSearch(PropDetailsM objSearchDetails, DataTable dtRoomOccupancySearch)
        {
            string data = "";
            PropDetailsM objresult = new PropDetailsM();
            try
            {
                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.Version = "0.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                objOTA_HotelAvailRQ.SearchCacheLevel = "Live";//"VeryRecent";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();

                HotelRef objHotelRef = new HotelRef();

                objHotelRef.HotelCode = objSearchDetails.iVendorId;         // "00007112" "00010375";

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = objSearchDetails.dtCheckIn.ToString("yyyy-MM-dd");
                objStayDateRange.End = objSearchDetails.dtCheckOut.ToString("yyyy-MM-dd");

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                // Room 1
                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();
                RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();

                GuestCountsSH objGuestCounts = new GuestCountsSH();

                List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();

                for (int i = 0; i < dtRoomOccupancySearch.Rows.Count; i++)
                {
                    GuestCountSH objGuestCount1 = new GuestCountSH();
                    objGuestCount1.AgeQualifyingCode = dtRoomOccupancySearch.Rows[i]["AgeQualifyingCode"].ToString();
                    objGuestCount1.Count = dtRoomOccupancySearch.Rows[i]["Count"].ToString();
                    objGuestCount1.Age = dtRoomOccupancySearch.Rows[i]["Age"].ToString();
                    objGuestCountArr.Add(objGuestCount1);
                }

                objGuestCounts.GuestCount = objGuestCountArr;

                objRoomStayCandidate.GuestCounts = objGuestCounts;

                objRoomStayCandidateArr.Add(objRoomStayCandidate);
                // End

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();
                objTPA_Extensions.SeoEnabled = "false";

                UserAuthentication objUserAuthentication = new UserAuthentication();
                objUserAuthentication.Username = "samaara";
                objUserAuthentication.PropertyId = "1300001224";
                objUserAuthentication.Password = "test@567";

                objTPA_Extensions.UserAuthentication = objUserAuthentication;

                objCriterion.HotelRef = objHotelRef;
                objCriterion.RoomStayCandidates = objRoomStayCandidates;
                objCriterion.StayDateRange = objStayDateRange;
                objCriterion.TPA_Extensions = objTPA_Extensions;


                objHotelSearchCriteria.Criterion = objCriterion;

                objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;


                objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                objEnvelope.Body = objBody;

                string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);

                data = clsTravelGuruApi.SearchTravelGuruApi(xmlString);

                //==> Convert xml string to class object <==
                EnvelopeHRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeHRes>(data);
                objresult = PopulateObject(objEnvelopeSHRes, objSearchDetails.iVendorId);

            }
            catch (Exception ex)
            {

            }
            return objresult;
        }

        public static PropDetailsM PopulateObject(EnvelopeHRes objEnvelopeSHRes, string VendorId)
        {
            PropDetailsM objSet = new PropDetailsM();

            objSet.sPropertyName = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.BasicPropertyInfo.HotelName;
            AddressHRes objAddressHRes = new AddressHRes();
            objAddressHRes = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.BasicPropertyInfo.Address;
            objSet.sPropertyAddress = objAddressHRes.AddressLine + "," + objAddressHRes.CityName;

            #region Map Location

            string[] arrMapURL = new string[50];
            string sPropertyNameresult = "";
            arrMapURL = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.BasicPropertyInfo.HotelName.Split(' ');
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
            Position objPosition = new Position();
            objPosition = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.BasicPropertyInfo.Position;

            sPropertyNameresult += "+" + objAddressHRes.CityName;
            objSet.dLatitude = Convert.ToDecimal(objPosition.Latitude);
            objSet.dLongitude = Convert.ToDecimal(objPosition.Longitude);
            objSet.slargeMapURL = "https://www.google.com/maps/place/" + sPropertyNameresult + "/" + objPosition.Latitude + "," + objPosition.Longitude + ",17z?hl=en-US";

            #endregion

            #region Gallery & Facilities

            Multimedia objMultimedia = new Multimedia();
            objMultimedia = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.Multimedia;


            #region Gallery Details

            SqlConnection objConn = default(SqlConnection);
            DataSet dsGallery = new DataSet();
            objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            objConn.Open();

            System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];

            MyParams[0] = new System.Data.SqlClient.SqlParameter("@VendorId", VendorId);
            dsGallery = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetHotelDetailsbyID_TG", MyParams);
            objConn.Close();



            List<etblPhotoGallery> objresulttblPhotoGallery = new List<etblPhotoGallery>();
            for (int lstphoto = 0; lstphoto < dsGallery.Tables[0].Rows.Count; lstphoto++)
            {
                objresulttblPhotoGallery.Add(new etblPhotoGallery()
                {
                    iVendorId = dsGallery.Tables[0].Rows[lstphoto]["sHotelCode"].ToString(),
                    sMainImgUrl = dsGallery.Tables[0].Rows[lstphoto]["sLargeImgUrl"].ToString(),
                    sthumbImgUrl = dsGallery.Tables[0].Rows[lstphoto]["sThumbImgUrl"].ToString()

                });
            }
            objSet.lstetblPhotoGallery = objresulttblPhotoGallery;

            #endregion

            //objresulttblPhotoGallery.Add(new etblPhotoGallery()
            //{
            //    sMainImgUrl = "",
            //    sthumbImgUrl = ""

            //});
            //objSet.lstetblPhotoGallery = objresulttblPhotoGallery;

            List<etblHotelFacilities> objresultetblHotelFacilities = new List<etblHotelFacilities>();
            for (int lstHotelFacilities = 0; lstHotelFacilities < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.Amenities.PropertyAmenities.Count; lstHotelFacilities++)
            {
                objresultetblHotelFacilities.Add(new etblHotelFacilities()
                {
                    iHoteFacilityId = Convert.ToInt32(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.Amenities.PropertyAmenities[lstHotelFacilities].Code.ToString()),
                    sHotelFacilites = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.Amenities.PropertyAmenities[lstHotelFacilities].Description.ToString(),
                    sImgUrl = ""
                });
            }
            objSet.lstetblHotelFacilities = objresultetblHotelFacilities;

            #endregion

            #region NearestTransport
            int arraylen = 0;
            if (objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI.Count < 3)
            {
                arraylen = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI.Count;
            }
            else
            {
                arraylen = 3;
            }
            for (int i = 0; i < arraylen; i++)
            {
                objSet.sNearestTransport += objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI[i].POIName + "(" + objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI[i].POIDistance + " Km) ";
            }

            #endregion

            #region DistanceToAirportRailway

            for (int j = 0; j < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI.Count; j++)
            {
                if (objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI[j].POIName.Contains("Airport"))
                {
                    objSet.sDistanceToAirportRailway += objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI[j].POIName + "(" + objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI[j].POIDistance + " Km) ";
                }
            }

            #endregion

            #region Rooms Details



            List<etblRooms> objetblRooms = new List<etblRooms>();

            for (int i = 0; i < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType.Count; i++)
            {
                List<RoomTaxes> objresultRoomTaxes = new List<RoomTaxes>();
                for (int lstTaxes = 0; lstTaxes < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomRates.RoomRate[i].Rates.Rate.Base.Taxes.Tax.Count; lstTaxes++)
                {
                    if (Convert.ToInt64(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomRates.RoomRate[i].Rates.Rate.Base.Taxes.Tax[lstTaxes].Code) == Convert.ToInt64(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType[i].RoomTypeCode.ToString()))
                    {
                        objresultRoomTaxes.Add(new RoomTaxes()
                        {
                            iRoomId = Convert.ToInt32(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomRates.RoomRate[i].Rates.Rate.Base.Taxes.Tax[lstTaxes].Code.ToString()),
                            ValidFrom = Convert.ToDateTime(null),
                            ValidTo = Convert.ToDateTime(null),
                            Tax = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomRates.RoomRate[i].Rates.Rate.Base.Taxes.Tax[lstTaxes].Amount.ToString()

                        });
                    }
                }

                objetblRooms.Add(new etblRooms()
                {
                    iRoomId = Convert.ToInt64(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType[i].RoomTypeCode.ToString()),
                    sRoomName = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType[i].RoomType.ToString(),
                    MaxOccupancy = Convert.ToByte(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType[i].TPA_Extensions.RoomType.MaxAdult.ToString()),
                    //TwinBed = Convert.ToInt16(ds.Tables[0].Rows[i]["TwinBed"].ToString()),
                    //MaxExtraBeds = Convert.ToByte(ds.Tables[0].Rows[i]["MaxExtraBeds"].ToString()),
                    //iAvailableInventory = Convert.ToInt32(ds.Tables[0].Rows[i]["iAvailableInventory"].ToString()),
                    //ExtraBedCharges = Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraBedCharges"].ToString()),
                    sRoomImgUrl = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType[i].RoomDescription.Image[0].ToString(),
                    //Amen1 = ds.Tables[0].Rows[i]["Amen1"].ToString(),
                    //Amen2 = ds.Tables[0].Rows[i]["Amen2"].ToString(),
                    //Amen3 = ds.Tables[0].Rows[i]["Amen3"].ToString(),
                    //Amen4 = ds.Tables[0].Rows[i]["Amen4"].ToString(),
                    //Amen5 = ds.Tables[0].Rows[i]["Amen5"].ToString(),
                    //Amen6 = ds.Tables[0].Rows[i]["Amen6"].ToString(),
                    //Amen7 = ds.Tables[0].Rows[i]["Amen7"].ToString(),
                    //Amen8 = ds.Tables[0].Rows[i]["Amen8"].ToString(),
                    lstRoomTaxes = objresultRoomTaxes
                });

            }
            objSet.lstetblRooms = objetblRooms;



            for (int lstRooms = 0; lstRooms < objSet.lstetblRooms.Count; lstRooms++)
            {
                List<etblRatePlans> objresultetblRatePlans = new List<etblRatePlans>();
                for (int j = 0; j < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan.Count; j++)
                {

                    List<CancellationPolcy> objresultCancellationPolicy = new List<CancellationPolcy>();
                    for (int lstCancellation = 0; lstCancellation < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].CancelPenalties.CancelPenalty.Count; lstCancellation++)
                    {
                        for (int lstCancellationRoom = 0; lstCancellationRoom < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].CancelPenalties.CancelPenalty.Count; lstCancellationRoom++)
                        {
                            if (Convert.ToInt32(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].RatePlanCode) == Convert.ToInt32(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomRates.RoomRate[lstCancellationRoom].RatePlanCode))
                            {
                                for (int lstCancellationRoomPenaltyDesc = 0; lstCancellationRoomPenaltyDesc < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].CancelPenalties.CancelPenalty[lstCancellationRoom].PenaltyDescription.Count; lstCancellationRoomPenaltyDesc++)
                                {
                                    objresultCancellationPolicy.Add(new CancellationPolcy()
                                    {
                                        iRPId = Convert.ToInt32(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].RatePlanCode.ToString()),
                                        ValidFrom = Convert.ToDateTime(null),
                                        ValidTo = Convert.ToDateTime(null),
                                        CancellationPolicy = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].CancelPenalties.CancelPenalty[lstCancellationRoom].PenaltyDescription[lstCancellationRoomPenaltyDesc].Name.ToString() + "( " + objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].CancelPenalties.CancelPenalty[lstCancellationRoom].PenaltyDescription[lstCancellationRoomPenaltyDesc].Text.ToString() + " )"
                                    });
                                }


                            }
                        }

                    }

                    for (int lstRateInclision = 0; lstRateInclision < objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].RatePlanInclusions.RatePlanInclusionDesciption.Text.Count; lstRateInclision++)
                    {
                        objresultetblRatePlans.Add(new etblRatePlans()
                        {
                            iRoomId = Convert.ToInt64(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RoomTypes.RoomType[lstRooms].RoomTypeCode.ToString()),
                            RPID = Convert.ToInt32(objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].RatePlanCode),
                            RateInclusion = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.RatePlans.RatePlan[j].RatePlanInclusions.RatePlanInclusionDesciption.Text[lstRateInclision].ToString(),
                            lstCancellationPolcy = objresultCancellationPolicy
                        });
                    }

                }
                objSet.lstetblRooms[lstRooms].lstRatePlan = objresultetblRatePlans;
            }






            //DataSet ds = new DataSet();
            //objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            //objConn.Open();

            //System.Data.SqlClient.SqlParameter[] MyParamsRooms = new System.Data.SqlClient.SqlParameter[18];

            //MyParamsRooms[0] = new System.Data.SqlClient.SqlParameter("@VendorId", objSearchDetails.iVendorId);
            //MyParamsRooms[1] = new System.Data.SqlClient.SqlParameter("@dtCheckIn", objSearchDetails.dtCheckIn);
            //MyParamsRooms[2] = new System.Data.SqlClient.SqlParameter("@dtCheckOut", objSearchDetails.dtCheckOut);
            //MyParamsRooms[3] = new System.Data.SqlClient.SqlParameter("@bLogin", objSearchDetails.bLogin);
            //MyParamsRooms[4] = new System.Data.SqlClient.SqlParameter("@Currency", objSearchDetails.Currency);
            //MyParamsRooms[5] = new System.Data.SqlClient.SqlParameter("@bIsAirConditioning", objSearchDetails.bIsAirConditioning);
            //MyParamsRooms[6] = new System.Data.SqlClient.SqlParameter("@bIsBathtub", objSearchDetails.bIsBathtub);
            //MyParamsRooms[7] = new System.Data.SqlClient.SqlParameter("@bIsFlatScreenTV", objSearchDetails.bIsFlatScreenTV);
            //MyParamsRooms[8] = new System.Data.SqlClient.SqlParameter("@bIsSoundproof", objSearchDetails.bIsSoundproof);
            //MyParamsRooms[9] = new System.Data.SqlClient.SqlParameter("@bIsView", objSearchDetails.bIsView);
            //MyParamsRooms[10] = new System.Data.SqlClient.SqlParameter("@bIsInternetFacilities", objSearchDetails.bIsInternetFacilities);
            //MyParamsRooms[11] = new System.Data.SqlClient.SqlParameter("@bIsPrivatePool", objSearchDetails.bIsPrivatePool);
            //MyParamsRooms[12] = new System.Data.SqlClient.SqlParameter("@bIsRoomService", objSearchDetails.bIsRoomService);
            //MyParamsRooms[13] = new System.Data.SqlClient.SqlParameter("@dMinPrice", objSearchDetails.dMinPrice);
            //MyParamsRooms[14] = new System.Data.SqlClient.SqlParameter("@dMaxPrice", objSearchDetails.dMaxPrice);
            //MyParamsRooms[15] = new System.Data.SqlClient.SqlParameter("@SpecialDeal", objSearchDetails.SpecialDeal);
            //MyParamsRooms[16] = new System.Data.SqlClient.SqlParameter("@RoomOccupancySearch", dtRoomOccupancySearch);
            //MyParamsRooms[17] = new System.Data.SqlClient.SqlParameter("@ChildrenAgeSearch", dtChildrenAgeSearch);
            //ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspSearchRoomsForHotel", MyParamsRooms);
            //objConn.Close();

            //if (ds.Tables.Count > 0)
            //{




            //    DataTable dtdistinct = ds.Tables[2].AsEnumerable()
            //                            .GroupBy(r => new { iRoomId = r["iRoomId"], RPID = r["RPID"] })
            //                            .Select(g => g.OrderBy(r => r["iRoomId"]).First())
            //                            .CopyToDataTable();

            //    var DistinctRatePlanList = (from t in dtdistinct.AsEnumerable()
            //                                select t).Distinct().ToList();








            //    for (int m = 0; m < objSet.lstetblRooms.Count; m++)
            //    {
            //        for (int n = 0; n < objSet.lstetblRooms[m].lstRatePlan.Count; n++)
            //        {
            //            List<etblOccupancy> objresultetblOccupancy = new List<etblOccupancy>();
            //            for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
            //            {
            //                string[] arr = new string[2];
            //                if (objSet.lstetblRooms[m].lstRatePlan[n].iRoomId == Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"].ToString())
            //                    && objSet.lstetblRooms[m].lstRatePlan[n].RPID == Convert.ToInt64(ds.Tables[2].Rows[k]["RPID"].ToString()))
            //                {
            //                    arr = ds.Tables[2].Rows[k]["Persons"].ToString().Split(',');
            //                    objresultetblOccupancy.Add(new etblOccupancy()
            //                    {
            //                        iRoomId = Convert.ToInt64(ds.Tables[2].Rows[k]["iRoomId"].ToString()),
            //                        RPID = Convert.ToInt32(ds.Tables[2].Rows[k]["RPID"].ToString()),
            //                        RatePlan = ds.Tables[2].Rows[k]["RatePlan"].ToString(),
            //                        iOccupancy = Convert.ToInt32(ds.Tables[2].Rows[k]["iOccupancy"].ToString()),
            //                        ExtraBeds = Convert.ToInt32(ds.Tables[2].Rows[k]["ExtraBeds"].ToString()),
            //                        ExtraBedCharges = Convert.ToDecimal(ds.Tables[2].Rows[k]["ExtraBedCharges"].ToString()),
            //                        dPrice = Convert.ToDecimal(ds.Tables[2].Rows[k]["dPrice"].ToString()),
            //                        iAdults = Convert.ToInt32(arr[0].ToString()),
            //                        iChildrens = Convert.ToInt32(arr[1].ToString()),
            //                        Cnt = Convert.ToInt32(ds.Tables[2].Rows[k]["Cnt"].ToString()),
            //                        blsPromo = Convert.ToBoolean(ds.Tables[2].Rows[k]["bIsPromo"].ToString())
            //                    });


            //                }
            //                objSet.lstetblRooms[m].lstRatePlan[n].lstetblOccupancy = objresultetblOccupancy;
            //            }
            //        }
            //    }



            //    List<OccupancyData> lstOccData = new List<OccupancyData>();
            //    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
            //    {
            //        lstOccData.Add(new OccupancyData()
            //        {
            //            Occupancy = Convert.ToInt16(ds.Tables[4].Rows[i]["Occupancy"]),
            //            Rooms = Convert.ToInt16(ds.Tables[4].Rows[i]["Rooms"]),
            //            Total = 0
            //        });
            //    }
            //    objSet.lstOccData = lstOccData;

            //    objSet.Symbol = ds.Tables[5].Rows[0].Field<string>("Symbol");


            //}

            #endregion



            return objSet;

        }

        public static string MultiVendor()
        {
            string data = "";
            try
            {
                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.Version = "0.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                objOTA_HotelAvailRQ.SearchCacheLevel = "VeryRecent";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();

                List<HotelRef> objHotelRefArr = new List<HotelRef>();

                HotelRef objHotelRef1 = new HotelRef();
                objHotelRef1.HotelCode = "00009457";
                objHotelRefArr.Add(objHotelRef1);

                HotelRef objHotelRef2 = new HotelRef();
                objHotelRef2.HotelCode = "00003171";
                objHotelRefArr.Add(objHotelRef2);

                HotelRef objHotelRef3 = new HotelRef();
                objHotelRef3.HotelCode = "00004333";
                objHotelRefArr.Add(objHotelRef3);

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = "2016-09-24";
                objStayDateRange.End = "2016-09-25";

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();
                RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();

                GuestCountsSH objGuestCounts = new GuestCountsSH();

                List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();
                GuestCountSH objGuestCount1 = new GuestCountSH();
                objGuestCount1.Count = "1";
                objGuestCount1.AgeQualifyingCode = "10";
                objGuestCountArr.Add(objGuestCount1);

                GuestCountSH objGuestCount2 = new GuestCountSH();
                objGuestCount2.Count = "1";
                objGuestCount2.AgeQualifyingCode = "8";
                objGuestCount2.Age = "10";
                objGuestCountArr.Add(objGuestCount2);

                objGuestCounts.GuestCount = objGuestCountArr;

                objRoomStayCandidate.GuestCounts = objGuestCounts;

                objRoomStayCandidateArr.Add(objRoomStayCandidate);

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();
                objTPA_Extensions.SeoEnabled = "false";

                UserAuthentication objUserAuthentication = new UserAuthentication();

                //Travel Guru Webservice credential 
                objUserAuthentication.Username = "samaara";
                objUserAuthentication.PropertyId = "1300001224";
                objUserAuthentication.Password = "test@567";

                objTPA_Extensions.UserAuthentication = objUserAuthentication;

                objCriterion.HotelRefMultiVendor = objHotelRefArr;
                objCriterion.RoomStayCandidates = objRoomStayCandidates;
                objCriterion.StayDateRange = objStayDateRange;
                objCriterion.TPA_Extensions = objTPA_Extensions;


                objHotelSearchCriteria.Criterion = objCriterion;

                objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;


                objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                objEnvelope.Body = objBody;

                string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);
                xmlString = xmlString.Replace("HotelRefList", "HotelRef");
                data = clsTravelGuruApi.SearchTravelGuruApi(xmlString);

                //==> Convert xml string to class object <==
                EnvelopeRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeRes>(data);
            }
            catch (Exception ex)
            {

            }
            return data;
        }

        /// <summary>
        /// Travel Guru Live data of selected hotels to know the Hotel availabilty and chepeast rate to show in the search list
        /// </summary>
        /// <param name="hotelSearchDataTG"></param>
        /// <param name="dtRoomOccupancySearch"></param>
        /// <param name="dtChildrenAgeSearch"></param>
        /// <param name="sCheckIn"></param>
        /// <param name="sCheckOut"></param>
        /// <param name="sExchangeRate"></param>
        /// <param name="dMinPrice"></param>
        /// <param name="dMaxPrice"></param>
        /// <returns></returns>
        public static List<PropSearchResponseModel> FetchMultiVendorOfDB(List<PropSearchResponseModel> hotelSearchDataTG, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch, string sCheckIn, string sCheckOut, decimal sExchangeRate, decimal dMinPrice, decimal dMaxPrice)
        {
            List<PropSearchResponseModel> rtrnList = new List<PropSearchResponseModel>();
            List<RoomStayRes> RoomStayTotal = new List<RoomStayRes>();
            string data = "";
            try
            {
                string usernameTg = ConfigurationManager.AppSettings["UserNameTG"].ToString();
                string propertyIdTg = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
                string passwordTg = ConfigurationManager.AppSettings["PasswordTG"].ToString();


                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.Version = "0.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                // objOTA_HotelAvailRQ.SearchCacheLevel = "VeryRecent";
                objOTA_HotelAvailRQ.SearchCacheLevel = "Live";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();

                List<HotelRef> objHotelRefArr = new List<HotelRef>();

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = sCheckIn;
                objStayDateRange.End = sCheckOut;

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();

                ///Children Details in newDT
                var newDT = from row in dtChildrenAgeSearch.AsEnumerable()
                            group row by new { ID = row.Field<int>("ID"), Age = row.Field<short>("Age") } into grp
                            select new
                            {
                                ID = grp.Key.ID,
                                Age = grp.Key.Age,
                                Count = grp.Count()
                            };

                // Room Details with occupancy
                foreach (DataRow DR in dtRoomOccupancySearch.Rows)
                {
                    RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();
                    GuestCountsSH objGuestCounts = new GuestCountsSH();

                    List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();

                    GuestCountSH objGuestCount1 = new GuestCountSH();
                    objGuestCount1.Count = DR["iAdults"].ToString();
                    objGuestCount1.AgeQualifyingCode = "10"; //for adult
                    objGuestCountArr.Add(objGuestCount1);

                    foreach (var DR1 in newDT)
                    {
                        if (DR["ID"].ToString() == DR1.ID.ToString())
                        {
                            GuestCountSH objGuestCount2 = new GuestCountSH();
                            objGuestCount2.Count = DR1.Count.ToString();
                            objGuestCount2.AgeQualifyingCode = "8"; //for child
                            objGuestCount2.Age = DR1.Age.ToString();
                            objGuestCountArr.Add(objGuestCount2);
                        }
                    }
                    objGuestCounts.GuestCount = objGuestCountArr;
                    objRoomStayCandidate.GuestCounts = objGuestCounts;
                    objRoomStayCandidateArr.Add(objRoomStayCandidate);
                }

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();
                objTPA_Extensions.SeoEnabled = "false";

                UserAuthentication objUserAuthentication = new UserAuthentication();
                objUserAuthentication.Username = usernameTg;
                objUserAuthentication.PropertyId = propertyIdTg;
                objUserAuthentication.Password = passwordTg;

                for (int req = 0; req < hotelSearchDataTG.Count; req++)
                //for (int req = 0; req < 1; req++)
                {
                    HotelRef objHotelRef1 = new HotelRef();
                    objHotelRef1.HotelCode = hotelSearchDataTG[req].iVendorId;
                    objHotelRefArr.Add(objHotelRef1);

                    if ((req > 0 && req % 50 == 49) || (req == hotelSearchDataTG.Count - 1))
                    {
                        objTPA_Extensions.UserAuthentication = objUserAuthentication;

                        objCriterion.HotelRefMultiVendor = objHotelRefArr;
                        objCriterion.RoomStayCandidates = objRoomStayCandidates;
                        objCriterion.StayDateRange = objStayDateRange;
                        objCriterion.TPA_Extensions = objTPA_Extensions;


                        objHotelSearchCriteria.Criterion = objCriterion;

                        objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                        objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                        objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;


                        objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                        objEnvelope.Body = objBody;

                        string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);
                        xmlString = xmlString.Replace("HotelRefList", "HotelRef");
                        data = clsTravelGuruApi.SearchTravelGuruApi(xmlString);

                        //==> Convert xml string to class object <==
                        EnvelopeRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeRes>(data);
                        if (objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.Count > 0)
                        {
                            List<RoomStayRes> RoomStay = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.ToList();
                            RoomStayTotal.AddRange(RoomStay);
                        }
                        objHotelRefArr = new List<HotelRef>();

                        if (RoomStayTotal.Count >= 50)
                            break;
                    }
                }

                // Cheapest Rate Plan code
                foreach (RoomStayRes room in RoomStayTotal)
                {
                    string CheapestRP = room.TPA_Extensions.LowestRatePlanId;
                    string Vendor = room.BasicPropertyInfo.HotelCode;

                    decimal Price = 0;
                    decimal priceWihtoutDiscount = 0;
                    int counter = 0;
                    int Quantity = 0;
                    foreach (RoomRate roomrate in room.RoomRates.RoomRate)
                    {
                        if (roomrate.RatePlanCode == CheapestRP)
                        {
                            decimal discount = 0;

                            if (roomrate.Rates.Rate.Discount != null)
                            {
                                discount = Convert.ToDecimal(roomrate.Rates.Rate.Discount.AmountBeforeTax);
                            }

                            Price += Convert.ToDecimal(roomrate.Rates.Rate.Base.AmountBeforeTax) - discount;
                            priceWihtoutDiscount += Convert.ToDecimal(roomrate.Rates.Rate.Base.AmountBeforeTax);
                            counter++;
                        }
                    }

                    foreach (var RP in room.RatePlans.RatePlan)
                    {
                        if (RP.RatePlanCode == CheapestRP)
                        {
                            Quantity = Convert.ToInt32(RP.AvailableQuantity);
                            break;
                        }
                    }

                    var filteredList = hotelSearchDataTG.Where(x => x.iVendorId == Vendor).FirstOrDefault();

                    filteredList.dPrice = sExchangeRate * Price / counter;
                    filteredList.dPriceRP = sExchangeRate * priceWihtoutDiscount / counter;
                    filteredList.dDiscountPercent = Math.Floor((filteredList.dPriceRP - filteredList.dPrice) / filteredList.dPriceRP * 100);
                    filteredList.iInventory = Quantity;

                    if (filteredList.dPrice >= dMinPrice && (dMaxPrice == 0 || filteredList.dPrice <= dMaxPrice))
                    {
                        rtrnList.Add(filteredList);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rtrnList;
        }

        public static string ConvertToXmlStringSH(EnvelopeSH objEnvelope)
        {
            string xmlString = "";
            try
            {
                XmlSerializer xsSubmitSH = new XmlSerializer(typeof(EnvelopeSH));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                // exclude xsi and xsd namespaces by adding the following:
                ns.Add(string.Empty, string.Empty);

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {

                        xsSubmitSH.Serialize(xmlWriter, objEnvelope, ns);
                    }
                    xmlString = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {

            }
            return xmlString;
        }

        //public static void SaveTravelGuruData(EnvelopeRes objTravelGuru)
        //{
        //    try
        //    {
        //        #region # DataTable for BasicPropertyInfomation
        //        DataTable dtBasicPropertyInfo = new DataTable();
        //        dtBasicPropertyInfo.TableName = "BasicPropertyInfomation";
        //        dtBasicPropertyInfo.Columns.Add("HotelCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("CurrencyCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("StopCell", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("LowestRatePlanCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("Rank", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("HotelType", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("ReviewRating", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("ReviewCount", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("OverviewUrl", typeof(string));

        //        DataRow drBasicPropertyInfo;
        //        #endregion// --> End <--//

        //        #region # DataTable for RoomType
        //        DataTable dtRoomType = new DataTable();
        //        dtRoomType.TableName = "RoomType";
        //        dtRoomType.Columns.Add("HotelCode", typeof(string));
        //        dtRoomType.Columns.Add("RoomTypeCode", typeof(string));
        //        dtRoomType.Columns.Add("RoomType", typeof(string));
        //        dtRoomType.Columns.Add("NonSmoking", typeof(string));
        //        dtRoomType.Columns.Add("Smoking", typeof(string));
        //        dtRoomType.Columns.Add("MinChildAge", typeof(string));
        //        dtRoomType.Columns.Add("MaxChildAge", typeof(string));
        //        dtRoomType.Columns.Add("MaxGuest", typeof(string));
        //        dtRoomType.Columns.Add("MaxChild", typeof(string));
        //        dtRoomType.Columns.Add("MaxAdult", typeof(string));

        //        DataRow drRoomType;
        //        #endregion End

        //        #region # DataTable for RatePlan
        //        DataTable dtRatePlan = new DataTable();
        //        dtRatePlan.TableName = "RatePlan";
        //        dtRatePlan.Columns.Add("HotelCode", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanType", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanName", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanCode", typeof(string));
        //        dtRatePlan.Columns.Add("AvailableQuantity", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanInclusion", typeof(string));
        //        dtRatePlan.Columns.Add("DiscountCouponEnable", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanDesc", typeof(string));

        //        DataRow drRatePlan;
        //        #endregion

        //        #region # DataTable for RatePlanCancelPenaltyDescription
        //        DataTable dtRatePlanCancelPenaltyDesc = new DataTable();
        //        dtRatePlanCancelPenaltyDesc.TableName = "RatePlanCancelPenaltyDesc";
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("HotelCode", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("RatePlanCode", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("NonRefundable", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("PenaltyName", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("PenaltyDescription", typeof(string));

        //        DataRow drRatePlanCancelPenaltyDesc;
        //        #endregion

        //        #region # DataTable for RatePlanCancelPenalty
        //        DataTable dtRatePlanCancelPenalty = new DataTable();
        //        dtRatePlanCancelPenalty.TableName = "RatePlanCancelPenalty";
        //        dtRatePlanCancelPenalty.Columns.Add("HotelCode", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("RatePlanCode", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("OffSetDropTime", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("OffSetTimeUnit", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("OffSetUnitMultiply", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("NumberOfNights", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("TaxInclusive", typeof(string));

        //        DataRow drRatePlanCancelPenalty;
        //        #endregion

        //        #region # DataTable for RoomRates
        //        DataTable dtRoomRate = new DataTable();
        //        dtRoomRate.TableName = "RoomRates";
        //        dtRoomRate.Columns.Add("HotelCode", typeof(string));
        //        dtRoomRate.Columns.Add("RatePlanCode", typeof(string));
        //        dtRoomRate.Columns.Add("RoomTypeCode", typeof(string));
        //        dtRoomRate.Columns.Add("EffectiveDate", typeof(string));
        //        dtRoomRate.Columns.Add("AmountBeforeTax", typeof(string));
        //        dtRoomRate.Columns.Add("TaxAmount", typeof(string));
        //        dtRoomRate.Columns.Add("AdditionalGuestAmountRPH", typeof(string));
        //        dtRoomRate.Columns.Add("AdditionalGuestAmountAgeQualificationCode", typeof(string));
        //        dtRoomRate.Columns.Add("AdditionalGuestAmountBeforeTax", typeof(string));
        //        dtRoomRate.Columns.Add("Bookable", typeof(string));
        //        dtRoomRate.Columns.Add("BaseChildOccupancy", typeof(string));
        //        dtRoomRate.Columns.Add("BaseAdultOccupancy", typeof(string));

        //        DataRow drRoomRate;
        //        #endregion

        //        #region # Start data save from class object to data table.
        //        if (objTravelGuru.Body.OTA_HotelAvailRS.RoomStays.RoomStay != null)
        //        {

        //            foreach (var roomstay in objTravelGuru.Body.OTA_HotelAvailRS.RoomStays.RoomStay)
        //            {
        //                // # Add data in BasisPropertyInformation
        //                drBasicPropertyInfo = dtBasicPropertyInfo.NewRow();

        //                drBasicPropertyInfo["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drBasicPropertyInfo["CurrencyCode"] = Convert.ToString(roomstay.BasicPropertyInfo.CurrencyCode);
        //                drBasicPropertyInfo["StopCell"] = Convert.ToString(roomstay.TPA_Extensions.StopSell);
        //                drBasicPropertyInfo["LowestRatePlanCode"] = Convert.ToString(roomstay.TPA_Extensions.LowestRatePlanId);
        //                drBasicPropertyInfo["Rank"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Rank);
        //                drBasicPropertyInfo["HotelType"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.HotelType);
        //                drBasicPropertyInfo["ReviewRating"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Reviews.ReviewRating);
        //                drBasicPropertyInfo["ReviewCount"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Reviews.ReviewCount);
        //                drBasicPropertyInfo["OverviewUrl"] = Convert.ToString(roomstay.TPA_Extensions.DeepLinkInformation.OverviewURL);

        //                dtBasicPropertyInfo.Rows.Add(drBasicPropertyInfo);
        //                // # End

        //                // # Add data in RoomType
        //                foreach (var roomtype in roomstay.RoomTypes.RoomType)
        //                {
        //                    drRoomType = dtRoomType.NewRow();

        //                    drRoomType["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                    drRoomType["RoomTypeCode"] = Convert.ToString(roomtype.RoomTypeCode);
        //                    drRoomType["RoomType"] = Convert.ToString(roomtype.RoomType);
        //                    drRoomType["NonSmoking"] = Convert.ToString(roomtype.NonSmoking);
        //                    drRoomType["Smoking"] = Convert.ToString(roomtype.Smoking);
        //                    drRoomType["MinChildAge"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MinChildAge);
        //                    drRoomType["MaxChildAge"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxChildAge);
        //                    drRoomType["MaxGuest"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxGuest);
        //                    drRoomType["MaxChild"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxChild);
        //                    drRoomType["MaxAdult"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxAdult);

        //                    dtRoomType.Rows.Add(drRoomType);
        //                }
        //                // End

        //                // # Add data in RatePlan
        //                foreach (var rateplan in roomstay.RatePlans.RatePlan)
        //                {
        //                    drRatePlan = dtRatePlan.NewRow();

        //                    drRatePlan["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                    drRatePlan["RatePlanType"] = Convert.ToString(rateplan.RatePlanType);
        //                    drRatePlan["RatePlanName"] = Convert.ToString(rateplan.RatePlanName);
        //                    drRatePlan["RatePlanCode"] = Convert.ToString(rateplan.RatePlanCode);
        //                    drRatePlan["AvailableQuantity"] = Convert.ToString(rateplan.AvailableQuantity);

        //                    string RatePlanInclusion = "";
        //                    if (rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text != null)
        //                    {
        //                        if (rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text.Count > 1)
        //                        {
        //                            foreach (var rateplaninclusion in rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text)
        //                            {
        //                                string comma = Convert.ToString(rateplaninclusion).Trim() == "" ? "" : ",";
        //                                RatePlanInclusion += Convert.ToString(rateplaninclusion).Trim() + comma;
        //                            }
        //                            RatePlanInclusion = RatePlanInclusion.Length > 1 ? RatePlanInclusion.Substring(0, RatePlanInclusion.Length - 1) : "";
        //                        }
        //                        else
        //                        {
        //                            RatePlanInclusion = Convert.ToString(rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text[0]);
        //                        }
        //                    }
        //                    drRatePlan["RatePlanInclusion"] = RatePlanInclusion;//Convert.ToString(rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text);
        //                    drRatePlan["DiscountCouponEnable"] = Convert.ToString(rateplan.TPA_Extensions.DiscountCouponDisplayIndicator.Enabled);

        //                    string RatePlanDesc = "";
        //                    if (rateplan.RatePlanDescription.Text != null)
        //                    {
        //                        if (rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text.Count > 1)
        //                        {
        //                            foreach (var rateplandesc in rateplan.RatePlanDescription.Text)
        //                            {
        //                                string comma = Convert.ToString(rateplandesc).Trim() == "" ? "" : ",";

        //                                RatePlanDesc += Convert.ToString(rateplandesc) + comma;
        //                            }
        //                            RatePlanDesc = RatePlanDesc.Length > 0 ? RatePlanDesc.Substring(0, RatePlanDesc.Length - 1) : "";
        //                        }
        //                        else
        //                        {
        //                            RatePlanDesc = Convert.ToString(rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text[0]);
        //                        }
        //                    }
        //                    drRatePlan["RatePlanDesc"] = RatePlanDesc;//Convert.ToString(rateplan.RatePlanDescription.Text);

        //                    dtRatePlan.Rows.Add(drRatePlan);

        //                    foreach (var rateplancancelpenaltydesc in rateplan.CancelPenalties.CancelPenalty)
        //                    {
        //                        // # Add data in RatePlan CancelPenalty
        //                        foreach (var PenaltyDesc in rateplancancelpenaltydesc.PenaltyDescription)
        //                        {
        //                            drRatePlanCancelPenaltyDesc = dtRatePlanCancelPenaltyDesc.NewRow();

        //                            drRatePlanCancelPenaltyDesc["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                            drRatePlanCancelPenaltyDesc["RatePlanCode"] = Convert.ToString(rateplan.RatePlanCode);
        //                            drRatePlanCancelPenaltyDesc["NonRefundable"] = Convert.ToString(rateplancancelpenaltydesc.NonRefundable);
        //                            drRatePlanCancelPenaltyDesc["PenaltyName"] = Convert.ToString(PenaltyDesc.Name);
        //                            drRatePlanCancelPenaltyDesc["PenaltyDescription"] = Convert.ToString(PenaltyDesc.Text);

        //                            dtRatePlanCancelPenaltyDesc.Rows.Add(drRatePlanCancelPenaltyDesc);
        //                        }
        //                        // # End

        //                        // # Add data in CancelPenalty
        //                        drRatePlanCancelPenalty = dtRatePlanCancelPenalty.NewRow();

        //                        drRatePlanCancelPenalty["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                        drRatePlanCancelPenalty["RatePlanCode"] = Convert.ToString(rateplan.RatePlanCode);
        //                        drRatePlanCancelPenalty["OffSetDropTime"] = rateplancancelpenaltydesc.DeadLine == null ? "" : Convert.ToString(rateplancancelpenaltydesc.DeadLine.OffsetDropTime);
        //                        drRatePlanCancelPenalty["OffSetTimeUnit"] = rateplancancelpenaltydesc.DeadLine == null ? "" : Convert.ToString(rateplancancelpenaltydesc.DeadLine.OffsetTimeUnit);
        //                        drRatePlanCancelPenalty["OffSetUnitMultiply"] = rateplancancelpenaltydesc.DeadLine == null ? "" : Convert.ToString(rateplancancelpenaltydesc.DeadLine.OffsetUnitMultiplier);

        //                        drRatePlanCancelPenalty["NumberOfNights"] = rateplancancelpenaltydesc.AmountPercent == null ? "" : Convert.ToString(rateplancancelpenaltydesc.AmountPercent.NmbrOfNights);
        //                        drRatePlanCancelPenalty["TaxInclusive"] = rateplancancelpenaltydesc.AmountPercent == null ? "" : Convert.ToString(rateplancancelpenaltydesc.AmountPercent.TaxInclusive);

        //                        dtRatePlanCancelPenalty.Rows.Add(drRatePlanCancelPenalty);

        //                        // # End
        //                    }

        //                }
        //                // # End

        //                // # Add data in RoomRate
        //                foreach (var roomrate in roomstay.RoomRates.RoomRate)
        //                {
        //                    drRoomRate = dtRoomRate.NewRow();

        //                    drRoomRate["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                    drRoomRate["RatePlanCode"] = Convert.ToString(roomrate.RatePlanCode);
        //                    drRoomRate["RoomTypeCode"] = Convert.ToString(roomrate.RoomID);
        //                    drRoomRate["EffectiveDate"] = Convert.ToString(roomrate.Rates.Rate.EffectiveDate);
        //                    drRoomRate["AmountBeforeTax"] = Convert.ToString(roomrate.Rates.Rate.Base.AmountBeforeTax);
        //                    drRoomRate["TaxAmount"] = Convert.ToString(roomrate.Rates.Rate.Base.Taxes.Amount);
        //                    drRoomRate["AdditionalGuestAmountRPH"] = roomrate.Rates.Rate.AdditionalGuestAmounts == null ? "" : Convert.ToString(roomrate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.RPH);
        //                    drRoomRate["AdditionalGuestAmountAgeQualificationCode"] = roomrate.Rates.Rate.AdditionalGuestAmounts == null ? "" : Convert.ToString(roomrate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.AgeQualifyingCode);
        //                    drRoomRate["AdditionalGuestAmountBeforeTax"] = roomrate.Rates.Rate.AdditionalGuestAmounts == null ? "0" : Convert.ToString(roomrate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.Amount.AmountBeforeTax);
        //                    drRoomRate["Bookable"] = Convert.ToString(roomrate.Rates.Rate.Bookable);
        //                    drRoomRate["BaseChildOccupancy"] = Convert.ToString(roomrate.Rates.Rate.BaseChildOccupancy);
        //                    drRoomRate["BaseAdultOccupancy"] = Convert.ToString(roomrate.Rates.Rate.BaseAdultOccupancy);

        //                    dtRoomRate.Rows.Add(drRoomRate);
        //                }
        //                // # End
        //            }
        //            DataSet ds = new DataSet();
        //            ds.Tables.Add(dtBasicPropertyInfo);
        //            ds.Tables.Add(dtRoomType);
        //            ds.Tables.Add(dtRatePlan);
        //            ds.Tables.Add(dtRatePlanCancelPenaltyDesc);
        //            ds.Tables.Add(dtRatePlanCancelPenalty);
        //            ds.Tables.Add(dtRoomRate);
        //        }
        //        #endregion

        //        #region # Save data from class object to database
        //        SqlCommand cmd = new SqlCommand();
        //        SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        //        cmd.Connection = conn;
        //        cmd.Connection.Open();
        //        cmd.CommandText = "uspSaveTravelGuruSearchData";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@sessionid", SqlDbType.VarChar).Value = "1234";
        //        cmd.Parameters.Add("@tblBasicPropertyInfoTGSearch_Tmp", SqlDbType.Structured).Value = dtBasicPropertyInfo;
        //        cmd.Parameters.Add("@tblRoomTypeTGSearch_Tmp", SqlDbType.Structured).Value = dtRoomType;
        //        cmd.Parameters.Add("@tblRatePlanTGSearch_Tmp", SqlDbType.Structured).Value = dtRatePlan;
        //        cmd.Parameters.Add("@tblRatePlanCancelPenaltyDescTGSearch_Tmp", SqlDbType.Structured).Value = dtRatePlanCancelPenaltyDesc;
        //        cmd.Parameters.Add("@tblRatePlanCancelPenaltyTGSearch_Tmp", SqlDbType.Structured).Value = dtRatePlanCancelPenalty;
        //        cmd.Parameters.Add("@tblRoomRateTGsearch_Tmp", SqlDbType.Structured).Value = dtRoomRate;

        //        cmd.ExecuteNonQuery();
        //        cmd.Connection.Close();
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public static void SaveTravelGuruHotelDetails(EnvelopeHRes objTravelGuru)
        //{
        //    try
        //    {
        //        #region # DataTable for BasicPropertyInfomation
        //        DataTable dtBasicPropertyInfo = new DataTable();
        //        dtBasicPropertyInfo.TableName = "BasicPropertyInfomation";

        //        dtBasicPropertyInfo.Columns.Add("HotelCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("HotelName", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("CurrencyCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("Latitude", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("Longitude", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("Address", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("City", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("PostalCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("StateName", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("CountryName", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("AwardRating", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("StopCell", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("LowestRatePlanCode", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("FlexibleCheckIn", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("StateSeoId", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("CitySeoId", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("NoOfRooms", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("NoOfFloors", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("HotelType", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("Description", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("CheckInTime", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("CheckOutTime", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("AreaSeoId", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("Area", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("AmenityDescription", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("ReviewImageUrl", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("ThumbnailUrl", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("ThumbnailImageName", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("ImageUrl", typeof(string));
        //        dtBasicPropertyInfo.Columns.Add("OverviewUrl", typeof(string));

        //        DataRow drBasicPropertyInfo;
        //        #endregion// --> End <--//

        //        #region # DataTable for BasicPropertyImages
        //        DataTable dtBasicPropertyImg = new DataTable();
        //        dtBasicPropertyImg.TableName = "BasicPropertyImg";
        //        dtBasicPropertyImg.Columns.Add("HotelCode", typeof(string));
        //        dtBasicPropertyImg.Columns.Add("ThumbImgUrl", typeof(string));
        //        dtBasicPropertyImg.Columns.Add("ThumbCategory", typeof(string));
        //        dtBasicPropertyImg.Columns.Add("ThumbCaption", typeof(string));
        //        dtBasicPropertyImg.Columns.Add("LargeImgUrl", typeof(string));
        //        dtBasicPropertyImg.Columns.Add("LargeCategory", typeof(string));
        //        dtBasicPropertyImg.Columns.Add("LargeCaption", typeof(string));

        //        DataRow drBasicPropertyImg;
        //        #endregion

        //        #region # DataTable for BasisPropertyPOI
        //        DataTable dtBasicPropertyPOI = new DataTable();
        //        dtBasicPropertyPOI.TableName = "BasicPropertyPOI";
        //        dtBasicPropertyPOI.Columns.Add("HotelCode", typeof(string));
        //        dtBasicPropertyPOI.Columns.Add("POIName", typeof(string));
        //        dtBasicPropertyPOI.Columns.Add("POIDistance", typeof(string));

        //        DataRow drBasicPropertyPOI;
        //        #endregion

        //        #region # DataTable for BasisPropertyAmenities
        //        DataTable dtBasicPropertyAmenities = new DataTable();
        //        dtBasicPropertyAmenities.TableName = "BasicPropertyAmenities";
        //        dtBasicPropertyAmenities.Columns.Add("HoTelCode", typeof(string));
        //        dtBasicPropertyAmenities.Columns.Add("Description", typeof(string));
        //        dtBasicPropertyAmenities.Columns.Add("Code", typeof(string));

        //        DataRow drBasicPropertyAmenities;
        //        #endregion

        //        #region # DataTable for RoomType
        //        DataTable dtRoomType = new DataTable();
        //        dtRoomType.TableName = "RoomType";
        //        dtRoomType.Columns.Add("HotelCode", typeof(string));
        //        dtRoomType.Columns.Add("RoomTypeCode", typeof(string));
        //        dtRoomType.Columns.Add("RoomType", typeof(string));
        //        dtRoomType.Columns.Add("NonSmoking", typeof(string));
        //        dtRoomType.Columns.Add("Smoking", typeof(string));
        //        dtRoomType.Columns.Add("MinChildAge", typeof(string));
        //        dtRoomType.Columns.Add("MaxChildAge", typeof(string));
        //        dtRoomType.Columns.Add("MaxGuest", typeof(string));
        //        dtRoomType.Columns.Add("MaxChild", typeof(string));
        //        dtRoomType.Columns.Add("MaxAdult", typeof(string));
        //        dtRoomType.Columns.Add("Description", typeof(string));
        //        dtRoomType.Columns.Add("SmImg", typeof(string));
        //        dtRoomType.Columns.Add("LrImg", typeof(string));

        //        DataRow drRoomType;
        //        #endregion End

        //        #region # DataTable for RatePlan
        //        DataTable dtRatePlan = new DataTable();
        //        dtRatePlan.TableName = "RatePlan";
        //        dtRatePlan.Columns.Add("HotelCode", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanType", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanName", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanCode", typeof(string));
        //        dtRatePlan.Columns.Add("AvailableQuantity", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanInclusion", typeof(string));
        //        dtRatePlan.Columns.Add("DiscountCouponEnable", typeof(string));
        //        dtRatePlan.Columns.Add("RatePlanDesc", typeof(string));

        //        DataRow drRatePlan;
        //        #endregion

        //        #region # DataTable for RatePlanCancelPenaltyDescription
        //        DataTable dtRatePlanCancelPenaltyDesc = new DataTable();
        //        dtRatePlanCancelPenaltyDesc.TableName = "RatePlanCancelPenaltyDesc";
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("HotelCode", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("RatePlanCode", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("NonRefundable", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("PenaltyName", typeof(string));
        //        dtRatePlanCancelPenaltyDesc.Columns.Add("PenaltyDescription", typeof(string));

        //        DataRow drRatePlanCancelPenaltyDesc;
        //        #endregion

        //        #region # DataTable for RatePlanCancelPenalty
        //        DataTable dtRatePlanCancelPenalty = new DataTable();
        //        dtRatePlanCancelPenalty.TableName = "RatePlanCancelPenalty";
        //        dtRatePlanCancelPenalty.Columns.Add("HotelCode", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("RatePlanCode", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("OffSetDropTime", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("OffSetTimeUnit", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("OffSetUnitMultiply", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("NumberOfNights", typeof(string));
        //        dtRatePlanCancelPenalty.Columns.Add("TaxInclusive", typeof(string));

        //        DataRow drRatePlanCancelPenalty;
        //        #endregion

        //        #region # DataTable for RoomRates
        //        DataTable dtRoomRate = new DataTable();
        //        dtRoomRate.TableName = "RoomRates";
        //        dtRoomRate.Columns.Add("HotelCode", typeof(string));
        //        dtRoomRate.Columns.Add("RatePlanCode", typeof(string));
        //        dtRoomRate.Columns.Add("RoomTypeCode", typeof(string));
        //        dtRoomRate.Columns.Add("EffectiveDate", typeof(string));
        //        dtRoomRate.Columns.Add("AmountBeforeTax", typeof(string));
        //        dtRoomRate.Columns.Add("TaxAmount", typeof(string));
        //        dtRoomRate.Columns.Add("AdditionalGuestAmountRPH", typeof(string));
        //        dtRoomRate.Columns.Add("AdditionalGuestAmountAgeQualificationCode", typeof(string));
        //        dtRoomRate.Columns.Add("AdditionalGuestAmountBeforeTax", typeof(string));
        //        dtRoomRate.Columns.Add("Bookable", typeof(string));
        //        dtRoomRate.Columns.Add("BaseChildOccupancy", typeof(string));
        //        dtRoomRate.Columns.Add("BaseAdultOccupancy", typeof(string));

        //        DataRow drRoomRate;
        //        #endregion

        //        #region # DataTable for RoomRateTaxes
        //        DataTable dtRoomRateTax = new DataTable();
        //        dtRoomRateTax.TableName = "RoomRatesTax";
        //        dtRoomRateTax.Columns.Add("HotelCode", typeof(string));
        //        dtRoomRateTax.Columns.Add("RatePlanCode", typeof(string));
        //        dtRoomRateTax.Columns.Add("RoomTypeCode", typeof(string));
        //        dtRoomRateTax.Columns.Add("EffectiveDate", typeof(string));
        //        dtRoomRateTax.Columns.Add("Amount", typeof(string));
        //        dtRoomRateTax.Columns.Add("Code", typeof(string));

        //        DataRow drRoomRateTax;
        //        #endregion

        //        #region # Start data save from class object to data table.
        //        if (objTravelGuru.Body.OTA_HotelAvailRS.RoomStays.RoomStay != null)
        //        {

        //            var roomstay = objTravelGuru.Body.OTA_HotelAvailRS.RoomStays.RoomStay;
        //            // # Add data in BasisPropertyInformation
        //            drBasicPropertyInfo = dtBasicPropertyInfo.NewRow();

        //            drBasicPropertyInfo["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //            drBasicPropertyInfo["HotelName"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelName);
        //            drBasicPropertyInfo["CurrencyCode"] = Convert.ToString(roomstay.BasicPropertyInfo.CurrencyCode);
        //            drBasicPropertyInfo["Latitude"] = Convert.ToString(roomstay.BasicPropertyInfo.Position.Latitude);
        //            drBasicPropertyInfo["Longitude"] = Convert.ToString(roomstay.BasicPropertyInfo.Position.Longitude);
        //            drBasicPropertyInfo["Address"] = Convert.ToString(roomstay.BasicPropertyInfo.Address.AddressLine);
        //            drBasicPropertyInfo["City"] = Convert.ToString(roomstay.BasicPropertyInfo.Address.CityName);
        //            drBasicPropertyInfo["PostalCode"] = Convert.ToString(roomstay.BasicPropertyInfo.Address.PostalCode);
        //            drBasicPropertyInfo["StateName"] = Convert.ToString(roomstay.BasicPropertyInfo.Address.StateProv);
        //            drBasicPropertyInfo["CountryName"] = Convert.ToString(roomstay.BasicPropertyInfo.Address.CountryName);
        //            drBasicPropertyInfo["AwardRating"] = Convert.ToString(roomstay.BasicPropertyInfo.Award.Rating);
        //            drBasicPropertyInfo["StopCell"] = Convert.ToString(roomstay.TPA_Extensions.StopSell);
        //            drBasicPropertyInfo["LowestRatePlanCode"] = Convert.ToString(roomstay.TPA_Extensions.LowestRatePlanId);
        //            drBasicPropertyInfo["FlexibleCheckIn"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.IsFlexibleCheckIn);
        //            drBasicPropertyInfo["StateSeoId"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.StateSeoId);
        //            drBasicPropertyInfo["CitySeoId"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.CitySeoId);
        //            drBasicPropertyInfo["NoOfRooms"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.NumberOfRooms);
        //            drBasicPropertyInfo["NoOfFloors"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.NumberOfFloors);
        //            drBasicPropertyInfo["HotelType"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.HotelType);
        //            drBasicPropertyInfo["Description"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Description);
        //            drBasicPropertyInfo["CheckInTime"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.CheckInTime);
        //            drBasicPropertyInfo["CheckOutTime"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.CheckOutTime);
        //            drBasicPropertyInfo["AreaSeoId"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.AreaSeoId);
        //            drBasicPropertyInfo["Area"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Area);
        //            drBasicPropertyInfo["AmenityDescription"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.AmenityDescription);
        //            drBasicPropertyInfo["ReviewImageUrl"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Reviews.ReviewImageUrl);
        //            drBasicPropertyInfo["ThumbnailUrl"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Multimedia.ThumbnailUrl);
        //            drBasicPropertyInfo["ThumbnailImageName"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Multimedia.ThumbnailImageName);
        //            drBasicPropertyInfo["ImageUrl"] = Convert.ToString(roomstay.TPA_Extensions.HotelBasicInformation.Multimedia.ImageUrl);
        //            drBasicPropertyInfo["OverviewUrl"] = Convert.ToString(roomstay.TPA_Extensions.DeepLinkInformation.OverviewURL);

        //            dtBasicPropertyInfo.Rows.Add(drBasicPropertyInfo);
        //            // # End

        //            // # Add data in BasisPropertyImage
        //            foreach (var PropertyImg in roomstay.TPA_Extensions.HotelBasicInformation.Multimedia.ImageJSON.ImagesList)
        //            {
        //                drBasicPropertyImg = dtBasicPropertyImg.NewRow();

        //                drBasicPropertyImg["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drBasicPropertyImg["ThumbImgUrl"] = Convert.ToString(PropertyImg.ThumbnailImageObj.Url);
        //                drBasicPropertyImg["ThumbCategory"] = Convert.ToString(PropertyImg.ThumbnailImageObj.Category);
        //                drBasicPropertyImg["ThumbCaption"] = Convert.ToString(PropertyImg.ThumbnailImageObj.Caption);
        //                drBasicPropertyImg["LargeImgUrl"] = Convert.ToString(PropertyImg.LargeImageObj.Url);
        //                drBasicPropertyImg["LargeCategory"] = Convert.ToString(PropertyImg.LargeImageObj.Category);
        //                drBasicPropertyImg["LargeCaption"] = Convert.ToString(PropertyImg.LargeImageObj.Caption);

        //                dtBasicPropertyImg.Rows.Add(drBasicPropertyImg);
        //            }
        //            // # End

        //            // # Add data in BasisPropertyPOI
        //            foreach (var PropertyPOI in roomstay.TPA_Extensions.HotelBasicInformation.POI.HotelPOI)
        //            {
        //                drBasicPropertyPOI = dtBasicPropertyPOI.NewRow();

        //                drBasicPropertyPOI["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drBasicPropertyPOI["POIName"] = Convert.ToString(PropertyPOI.POIName);
        //                drBasicPropertyPOI["POIDistance"] = Convert.ToString(PropertyPOI.POIDistance);

        //                dtBasicPropertyPOI.Rows.Add(drBasicPropertyPOI);
        //            }
        //            // # End

        //            // # Add data in BasisPropertyPOI
        //            foreach (var Amenities in roomstay.TPA_Extensions.HotelBasicInformation.Amenities.PropertyAmenities)
        //            {
        //                drBasicPropertyAmenities = dtBasicPropertyAmenities.NewRow();

        //                drBasicPropertyAmenities["HoTelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drBasicPropertyAmenities["Description"] = Convert.ToString(Amenities.Description);
        //                drBasicPropertyAmenities["Code"] = Convert.ToString(Amenities.Code);

        //                dtBasicPropertyAmenities.Rows.Add(drBasicPropertyAmenities);
        //            }
        //            // # End

        //            // # Add data in RoomType
        //            foreach (var roomtype in roomstay.RoomTypes.RoomType)
        //            {
        //                drRoomType = dtRoomType.NewRow();

        //                drRoomType["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drRoomType["RoomTypeCode"] = Convert.ToString(roomtype.RoomTypeCode);
        //                drRoomType["RoomType"] = Convert.ToString(roomtype.RoomType);
        //                drRoomType["NonSmoking"] = Convert.ToString(roomtype.NonSmoking);
        //                drRoomType["Smoking"] = Convert.ToString(roomtype.Smoking);
        //                drRoomType["MinChildAge"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MinChildAge);
        //                drRoomType["MaxChildAge"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxChildAge);
        //                drRoomType["MaxGuest"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxGuest);
        //                drRoomType["MaxChild"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxChild);
        //                drRoomType["MaxAdult"] = Convert.ToString(roomtype.TPA_Extensions.RoomType.MaxAdult);

        //                drRoomType["Description"] = Convert.ToString(roomtype.RoomDescription.Text);

        //                if (roomtype.RoomDescription.Image.Count > 0)
        //                {
        //                    List<string> Img = roomtype.RoomDescription.Image;
        //                    drRoomType["SmImg"] = Convert.ToString(Img[0]);
        //                    if (roomtype.RoomDescription.Image.Count > 1)
        //                    {
        //                        drRoomType["LrImg"] = Convert.ToString(Img[1]);
        //                    }
        //                    else
        //                    {
        //                        drRoomType["LrImg"] = Convert.ToString("");
        //                    }
        //                }
        //                else
        //                {
        //                    drRoomType["SmImg"] = Convert.ToString("");
        //                    drRoomType["LrImg"] = Convert.ToString("");
        //                }

        //                dtRoomType.Rows.Add(drRoomType);
        //            }
        //            // End

        //            // # Add data in RatePlan
        //            foreach (var rateplan in roomstay.RatePlans.RatePlan)
        //            {
        //                drRatePlan = dtRatePlan.NewRow();

        //                drRatePlan["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drRatePlan["RatePlanType"] = Convert.ToString(rateplan.RatePlanType);
        //                drRatePlan["RatePlanName"] = Convert.ToString(rateplan.RatePlanName);
        //                drRatePlan["RatePlanCode"] = Convert.ToString(rateplan.RatePlanCode);
        //                drRatePlan["AvailableQuantity"] = Convert.ToString(rateplan.AvailableQuantity);

        //                string RatePlanInclusion = "";
        //                if (rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text != null)
        //                {
        //                    if (rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text.Count > 1)
        //                    {
        //                        foreach (var rateplaninclusion in rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text)
        //                        {
        //                            string comma = Convert.ToString(rateplaninclusion).Trim() == "" ? "" : ",";
        //                            RatePlanInclusion += Convert.ToString(rateplaninclusion).Trim() + comma;
        //                        }
        //                        RatePlanInclusion = RatePlanInclusion.Length > 1 ? RatePlanInclusion.Substring(0, RatePlanInclusion.Length - 1) : "";
        //                    }
        //                    else
        //                    {
        //                        RatePlanInclusion = Convert.ToString(rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text[0]);
        //                    }
        //                }
        //                drRatePlan["RatePlanInclusion"] = RatePlanInclusion;//Convert.ToString(rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text);
        //                drRatePlan["DiscountCouponEnable"] = Convert.ToString(rateplan.TPA_Extensions.DiscountCouponDisplayIndicator.Enabled);

        //                string RatePlanDesc = "";
        //                if (rateplan.RatePlanDescription.Text != null)
        //                {
        //                    if (rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text.Count > 1)
        //                    {
        //                        foreach (var rateplandesc in rateplan.RatePlanDescription.Text)
        //                        {
        //                            string comma = Convert.ToString(rateplandesc).Trim() == "" ? "" : ",";

        //                            RatePlanDesc += Convert.ToString(rateplandesc) + comma;
        //                        }
        //                        RatePlanDesc = RatePlanDesc.Length > 0 ? RatePlanDesc.Substring(0, RatePlanDesc.Length - 1) : "";
        //                    }
        //                    else
        //                    {
        //                        RatePlanDesc = Convert.ToString(rateplan.RatePlanInclusions.RatePlanInclusionDesciption.Text[0]);
        //                    }
        //                }
        //                drRatePlan["RatePlanDesc"] = RatePlanDesc;//Convert.ToString(rateplan.RatePlanDescription.Text);

        //                dtRatePlan.Rows.Add(drRatePlan);

        //                foreach (var rateplancancelpenaltydesc in rateplan.CancelPenalties.CancelPenalty)
        //                {
        //                    // # Add data in RatePlan CancelPenalty
        //                    foreach (var PenaltyDesc in rateplancancelpenaltydesc.PenaltyDescription)
        //                    {
        //                        drRatePlanCancelPenaltyDesc = dtRatePlanCancelPenaltyDesc.NewRow();

        //                        drRatePlanCancelPenaltyDesc["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                        drRatePlanCancelPenaltyDesc["RatePlanCode"] = Convert.ToString(rateplan.RatePlanCode);
        //                        drRatePlanCancelPenaltyDesc["NonRefundable"] = Convert.ToString(rateplancancelpenaltydesc.NonRefundable);
        //                        drRatePlanCancelPenaltyDesc["PenaltyName"] = Convert.ToString(PenaltyDesc.Name);
        //                        drRatePlanCancelPenaltyDesc["PenaltyDescription"] = Convert.ToString(PenaltyDesc.Text);

        //                        dtRatePlanCancelPenaltyDesc.Rows.Add(drRatePlanCancelPenaltyDesc);
        //                    }
        //                    // # End

        //                    // # Add data in CancelPenalty
        //                    drRatePlanCancelPenalty = dtRatePlanCancelPenalty.NewRow();

        //                    drRatePlanCancelPenalty["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                    drRatePlanCancelPenalty["RatePlanCode"] = Convert.ToString(rateplan.RatePlanCode);
        //                    drRatePlanCancelPenalty["OffSetDropTime"] = rateplancancelpenaltydesc.DeadLine == null ? "" : Convert.ToString(rateplancancelpenaltydesc.DeadLine.OffsetDropTime);
        //                    drRatePlanCancelPenalty["OffSetTimeUnit"] = rateplancancelpenaltydesc.DeadLine == null ? "" : Convert.ToString(rateplancancelpenaltydesc.DeadLine.OffsetTimeUnit);
        //                    drRatePlanCancelPenalty["OffSetUnitMultiply"] = rateplancancelpenaltydesc.DeadLine == null ? "" : Convert.ToString(rateplancancelpenaltydesc.DeadLine.OffsetUnitMultiplier);

        //                    drRatePlanCancelPenalty["NumberOfNights"] = rateplancancelpenaltydesc.AmountPercent == null ? "" : Convert.ToString(rateplancancelpenaltydesc.AmountPercent.NmbrOfNights);
        //                    drRatePlanCancelPenalty["TaxInclusive"] = rateplancancelpenaltydesc.AmountPercent == null ? "" : Convert.ToString(rateplancancelpenaltydesc.AmountPercent.TaxInclusive);

        //                    dtRatePlanCancelPenalty.Rows.Add(drRatePlanCancelPenalty);

        //                    // # End
        //                }


        //            }
        //            // # End

        //            // # Add data in RoomRate
        //            foreach (var roomrate in roomstay.RoomRates.RoomRate)
        //            {
        //                drRoomRate = dtRoomRate.NewRow();

        //                drRoomRate["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                drRoomRate["RatePlanCode"] = Convert.ToString(roomrate.RatePlanCode);
        //                drRoomRate["RoomTypeCode"] = Convert.ToString(roomrate.RoomID);
        //                drRoomRate["EffectiveDate"] = Convert.ToString(roomrate.Rates.Rate.EffectiveDate);
        //                drRoomRate["AmountBeforeTax"] = Convert.ToString(roomrate.Rates.Rate.Base.AmountBeforeTax);
        //                drRoomRate["TaxAmount"] = Convert.ToString(roomrate.Rates.Rate.Base.Taxes.Amount);
        //                drRoomRate["AdditionalGuestAmountRPH"] = roomrate.Rates.Rate.AdditionalGuestAmounts == null ? "" : Convert.ToString(roomrate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.RPH);
        //                drRoomRate["AdditionalGuestAmountAgeQualificationCode"] = roomrate.Rates.Rate.AdditionalGuestAmounts == null ? "" : Convert.ToString(roomrate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.AgeQualifyingCode);
        //                drRoomRate["AdditionalGuestAmountBeforeTax"] = roomrate.Rates.Rate.AdditionalGuestAmounts == null ? "0" : Convert.ToString(roomrate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.Amount.AmountBeforeTax);
        //                drRoomRate["Bookable"] = Convert.ToString(roomrate.Rates.Rate.Bookable);
        //                drRoomRate["BaseChildOccupancy"] = Convert.ToString(roomrate.Rates.Rate.BaseChildOccupancy);
        //                drRoomRate["BaseAdultOccupancy"] = Convert.ToString(roomrate.Rates.Rate.BaseAdultOccupancy);

        //                dtRoomRate.Rows.Add(drRoomRate);

        //                // # Add data in RoomRateTaxes
        //                foreach (var roomtaxes in roomrate.Rates.Rate.Base.Taxes.Tax)
        //                {
        //                    drRoomRateTax = dtRoomRateTax.NewRow();

        //                    drRoomRateTax["HotelCode"] = Convert.ToString(roomstay.BasicPropertyInfo.HotelCode);
        //                    drRoomRateTax["RatePlanCode"] = Convert.ToString(roomrate.RatePlanCode);
        //                    drRoomRateTax["RoomTypeCode"] = Convert.ToString(roomrate.RoomID);
        //                    drRoomRateTax["EffectiveDate"] = Convert.ToString(roomrate.Rates.Rate.EffectiveDate);
        //                    drRoomRateTax["Amount"] = Convert.ToString(roomtaxes.Amount);
        //                    drRoomRateTax["Code"] = Convert.ToString(roomtaxes.Code);

        //                    dtRoomRateTax.Rows.Add(drRoomRateTax);
        //                }
        //                // # End
        //            }
        //            // # End



        //            DataSet ds = new DataSet();
        //            ds.Tables.Add(dtBasicPropertyInfo);
        //            ds.Tables.Add(dtBasicPropertyImg);
        //            ds.Tables.Add(dtBasicPropertyPOI);
        //            ds.Tables.Add(dtBasicPropertyAmenities);
        //            ds.Tables.Add(dtRoomType);
        //            ds.Tables.Add(dtRatePlan);
        //            ds.Tables.Add(dtRatePlanCancelPenaltyDesc);
        //            ds.Tables.Add(dtRatePlanCancelPenalty);
        //            ds.Tables.Add(dtRoomRate);
        //            ds.Tables.Add(dtRoomRateTax);
        //        }
        //        #endregion

        //        #region # Save data from class object to database
        //        SqlCommand cmd = new SqlCommand();
        //        SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        //        cmd.Connection = conn;
        //        cmd.Connection.Open();
        //        cmd.CommandText = "uspSaveTravelGuruSpecificHotelData";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@sessionid", SqlDbType.VarChar).Value = "1234";
        //        cmd.Parameters.Add("@tblBasicPropertyInfoTGSpecific_Tmp", SqlDbType.Structured).Value = dtBasicPropertyInfo;
        //        cmd.Parameters.Add("@tblBasicPropertyImgTGSpecific_Tmp", SqlDbType.Structured).Value = dtBasicPropertyImg;
        //        cmd.Parameters.Add("@tblBasicPropertyPOITGSpecific_Tmp", SqlDbType.Structured).Value = dtBasicPropertyPOI;
        //        cmd.Parameters.Add("@tblBasicPropertyAmenitiesTGSpecific_Tmp", SqlDbType.Structured).Value = dtBasicPropertyAmenities;
        //        cmd.Parameters.Add("@tblRoomTypeTGSpecific_Tmp", SqlDbType.Structured).Value = dtRoomType;
        //        cmd.Parameters.Add("@tblRatePlanTGSpecific_Tmp", SqlDbType.Structured).Value = dtRatePlan;
        //        cmd.Parameters.Add("@tblRatePlanCancelPenaltyDescTGSpecific_Tmp", SqlDbType.Structured).Value = dtRatePlanCancelPenaltyDesc;
        //        cmd.Parameters.Add("@tblRatePlanCancelPenaltyTGSpecific_Tmp", SqlDbType.Structured).Value = dtRatePlanCancelPenalty;
        //        cmd.Parameters.Add("@tblRoomRateTGSpecific_Tmp", SqlDbType.Structured).Value = dtRoomRate;
        //        cmd.Parameters.Add("@tblRoomRateTaxTGSpecific_Tmp", SqlDbType.Structured).Value = dtRoomRateTax;

        //        cmd.ExecuteNonQuery();
        //        cmd.Connection.Close();
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSearchingOnlyRoom">If searching only for rooms from OfferReview modify search then We will not process the data of Hotel Images, Policy, Hotel description etc.. 
        /// Only Room Information will be processed and send to Client</param>
        /// <param name="sVendorId">Unique Id for Hotel according to TravelGuru</param>
        /// <param name="sCheckIn">Check In Date in Travel Guru Api Format yyyy-MM-dd</param>
        /// <param name="sCheckOut">Check In Date in Travel Guru Api Format yyyy-MM-dd</param>
        /// <param name="dtRoomOccupancySearch"> Adult Information</param>
        /// <param name="dtChildrenAgeSearch">Child Information</param>
        /// <param name="roomData">Required to structure the Room Result according to Occupancy</param>
        /// <returns></returns>
        public static TG_Hotel FetchHotelsDetailsByVendorId(bool isSearchingOnlyRoom, string sVendorId, string sCheckIn, string sCheckOut, DataTable dtRoomOccupancySearch, DataTable dtChildrenAgeSearch, List<RoomData> roomData)
        {
            TG_Hotel tgHotel = new TG_Hotel();
            Dictionary<string, decimal> roomIdsAndRatesDictionary = new Dictionary<string, decimal>();
            List<TG_RoomDetails> tempRoomList = new List<TG_RoomDetails>();
            try
            {
                #region API call
                string usernameTg = ConfigurationManager.AppSettings["UserNameTG"].ToString();
                string propertyIdTg = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
                string passwordTg = ConfigurationManager.AppSettings["PasswordTG"].ToString();

                string data = string.Empty;

                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                //objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "PRICE";
                objOTA_HotelAvailRQ.Version = "1.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                //objOTA_HotelAvailRQ.SearchCacheLevel = "VeryRecent";
                objOTA_HotelAvailRQ.SearchCacheLevel = "Live";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();


                List<HotelRef> objHotelRefArr = new List<HotelRef>();

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = sCheckIn;
                objStayDateRange.End = sCheckOut;

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();

                var newDT = from row in dtChildrenAgeSearch.AsEnumerable()
                            group row by new { ID = row.Field<int>("ID"), Age = row.Field<short>("Age") } into grp
                            select new
                            {
                                ID = grp.Key.ID,
                                Age = grp.Key.Age,
                                Count = grp.Count()
                            };

                foreach (DataRow DR in dtRoomOccupancySearch.Rows)
                {
                    RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();
                    GuestCountsSH objGuestCounts = new GuestCountsSH();

                    List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();

                    GuestCountSH objGuestCount1 = new GuestCountSH();
                    objGuestCount1.Count = DR["iAdults"].ToString();
                    objGuestCount1.AgeQualifyingCode = "10";
                    objGuestCountArr.Add(objGuestCount1);

                    foreach (var DR1 in newDT)
                    {
                        if (DR["ID"].ToString() == DR1.ID.ToString())
                        {
                            GuestCountSH objGuestCount2 = new GuestCountSH();
                            objGuestCount2.Count = DR1.Count.ToString();
                            objGuestCount2.AgeQualifyingCode = "8";
                            objGuestCount2.Age = DR1.Age.ToString();
                            objGuestCountArr.Add(objGuestCount2);
                        }
                    }
                    objGuestCounts.GuestCount = objGuestCountArr;
                    objRoomStayCandidate.GuestCounts = objGuestCounts;
                    objRoomStayCandidateArr.Add(objRoomStayCandidate);
                }

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();
                objTPA_Extensions.SeoEnabled = "false";

                UserAuthentication objUserAuthentication = new UserAuthentication();
                objUserAuthentication.Username = usernameTg;
                objUserAuthentication.PropertyId = propertyIdTg;
                objUserAuthentication.Password = passwordTg;


                HotelRef objHotelRef1 = new HotelRef();
                objHotelRef1.HotelCode = sVendorId;
                objHotelRefArr.Add(objHotelRef1);

                objTPA_Extensions.UserAuthentication = objUserAuthentication;

                objCriterion.HotelRefMultiVendor = objHotelRefArr;
                objCriterion.RoomStayCandidates = objRoomStayCandidates;
                objCriterion.StayDateRange = objStayDateRange;
                objCriterion.TPA_Extensions = objTPA_Extensions;


                objHotelSearchCriteria.Criterion = objCriterion;

                objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;


                objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                objEnvelope.Body = objBody;

                string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);
                xmlString = xmlString.Replace("HotelRefList", "HotelRef");
                data = clsTravelGuruApi.SearchTravelGuruApi(xmlString);

                EnvelopeHRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeHRes>(data);

                #endregion api call

                #region Processing Result

                if (objEnvelopeSHRes.Body.OTA_HotelAvailRS != null && objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays != null)
                {
                    var roomStay = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay;

                    #region hotelInfo

                    if (!isSearchingOnlyRoom && roomStay != null)
                    {
                        tgHotel.AmenityDescription = roomStay.TPA_Extensions.HotelBasicInformation.AmenityDescription;
                        tgHotel.CurrencyCode = roomStay.BasicPropertyInfo.CurrencyCode;
                        tgHotel.HotelCode = roomStay.BasicPropertyInfo.HotelCode;
                        tgHotel.HotelName = roomStay.BasicPropertyInfo.HotelName;
                        tgHotel.Description = roomStay.TPA_Extensions.HotelBasicInformation.Description;
                        tgHotel.Latitude = roomStay.BasicPropertyInfo.Position.Latitude;
                        tgHotel.Longitude = roomStay.BasicPropertyInfo.Position.Longitude;
                        tgHotel.AddressLine1 = roomStay.BasicPropertyInfo.Address.AddressLine;
                        tgHotel.CityName = roomStay.BasicPropertyInfo.Address.CityName;
                        tgHotel.State = roomStay.BasicPropertyInfo.Address.StateProv;
                        tgHotel.Country = roomStay.BasicPropertyInfo.Address.CountryName;
                        tgHotel.PostalCode = roomStay.BasicPropertyInfo.Address.PostalCode;
                        tgHotel.Rating = roomStay.BasicPropertyInfo.Award.Rating;

                        #region BasicPropertyInfo

                        var hotelBasicInfo = roomStay.TPA_Extensions.HotelBasicInformation;

                        tgHotel.Area = hotelBasicInfo.Area;
                        tgHotel.CheckInTime = hotelBasicInfo.CheckInTime;
                        tgHotel.CheckOutTime = hotelBasicInfo.CheckOutTime;
                        tgHotel.HotelCategory = hotelBasicInfo.HotelCategory;
                        tgHotel.HotelType = hotelBasicInfo.HotelType;
                        tgHotel.NumberOfFloors = hotelBasicInfo.NumberOfFloors;

                        tgHotel.NumberOfRooms = hotelBasicInfo.NumberOfRooms;
                        tgHotel.IsFlexibleCheckIn = hotelBasicInfo.IsFlexibleCheckIn;
                        tgHotel.ReviewImageUrl = hotelBasicInfo.Reviews.ReviewImageUrl;
                        tgHotel.NumberOfFloors = hotelBasicInfo.NumberOfFloors;

                        tgHotel.MainImageUrl = hotelBasicInfo.Multimedia.ImageUrl;
                        tgHotel.ThumbnailImageName = hotelBasicInfo.Multimedia.ThumbnailImageName;
                        tgHotel.ThumbnailUrl = hotelBasicInfo.Multimedia.ThumbnailUrl;

                        if (hotelBasicInfo.Multimedia.ImageJSON != null)
                        {
                            foreach (var galleryImage in hotelBasicInfo.Multimedia.ImageJSON.ImagesList)
                            {
                                tgHotel.GalleryImages.Add(
                                    new TG_Image
                                    {
                                        Caption = galleryImage.LargeImageObj.Caption,
                                        Category = galleryImage.LargeImageObj.Category,
                                        ImageUrl = galleryImage.LargeImageObj.Url,
                                        ImageUrlThumb = galleryImage.ThumbnailImageObj.Url
                                    });
                            }
                        }

                        foreach (var amenity in hotelBasicInfo.Amenities.PropertyAmenities)
                        {
                            tgHotel.HotelAmenities.Add(
                                new TG_HotelAmenity
                                {
                                    code = amenity.Code,
                                    Description = amenity.Description
                                });
                        }
                        if (hotelBasicInfo.POI != null)
                        {
                            foreach (var poi in hotelBasicInfo.POI.HotelPOI)
                            {
                                tgHotel.POIData.Add(
                                    new TG_HotelPOI
                                    {
                                        POIName = poi.POIName,
                                        POIDistance = poi.POIDistance
                                    });
                            }
                        }


                        #endregion
                    }

                    #endregion hotelInfo

                    #region roomInfo

                    if (roomStay != null && roomStay.RoomTypes != null)
                    {
                        foreach (var roomTypeItem in roomStay.RoomTypes.RoomType)
                        {
                            TG_RoomDetails tgRoom = new TG_RoomDetails();

                            tgRoom.RoomId = roomTypeItem.RoomTypeCode;
                            tgRoom.VendorId = sVendorId;
                            tgRoom.Description = roomTypeItem.RoomDescription.Text;
                            tgRoom.RoomImages = roomTypeItem.RoomDescription.Image;
                            tgRoom.RoomName = roomTypeItem.RoomType;

                            var roomRatesOfThisRoom = roomStay.RoomRates.RoomRate.Where(x => x.RoomID == roomTypeItem.RoomTypeCode).ToList();

                            if (roomRatesOfThisRoom.Count == 0)
                            {
                                continue;
                            }

                            var ratePlanCodesOfThisRoom = roomRatesOfThisRoom.Select(x => x.RatePlanCode).Distinct().ToList();


                            foreach (var ratePlanCodeItem in ratePlanCodesOfThisRoom)
                            {
                                var ratePlanFromApi = roomStay.RatePlans.RatePlan.Where(x => x.RatePlanCode == ratePlanCodeItem).FirstOrDefault();

                                var ratePlan = new TG_RatePlan();

                                ratePlan.RatePlanCode = ratePlanFromApi.RatePlanCode;
                                ratePlan.RatePlanName = ratePlanFromApi.RatePlanName;

                                var roomRatesByCurrentRatePlan = roomRatesOfThisRoom.Where(x => x.RatePlanCode == ratePlanCodeItem).ToList();

                                ratePlan.TotalRoomRate = roomRatesByCurrentRatePlan.Sum(x => Convert.ToDecimal(x.Rates.Rate.Base.AmountBeforeTax));

                                ratePlan.TotalRoomRateWithoutDiscount = roomRatesByCurrentRatePlan.Sum(x => Convert.ToDecimal(x.Rates.Rate.Base.AmountBeforeTax));

                                ratePlan.TotalTax = roomRatesByCurrentRatePlan.Sum(x => Convert.ToDecimal(x.Rates.Rate.Base.Taxes.Amount));

                                for (int i = 0; i < roomRatesByCurrentRatePlan.Count; i++)
                                {
                                    if (roomRatesByCurrentRatePlan[i].Rates.Rate.AdditionalGuestAmounts != null)
                                    {
                                        ratePlan.TotalExtraBedCharge += roomRatesByCurrentRatePlan[i].Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.Sum(x => Convert.ToDecimal(x.Amount.AmountBeforeTax));
                                    }
                                }

                                foreach (var roomRate in roomRatesByCurrentRatePlan)
                                {
                                    decimal discountPerRoomRate = 0;
                                    decimal totalDiscount = 0;

                                    if (roomRate.Rates.Rate.Discount != null)
                                    {
                                        discountPerRoomRate = Convert.ToDecimal(roomRate.Rates.Rate.Discount.AmountBeforeTax);

                                        for (int i = 0; i < roomRatesByCurrentRatePlan.Count; i++)
                                        {
                                            if (roomRatesByCurrentRatePlan[i].Rates.Rate.Discount != null)
                                            {
                                                totalDiscount += decimal.Parse(roomRatesByCurrentRatePlan[i].Rates.Rate.Discount.AmountBeforeTax);
                                            }
                                        }

                                        ratePlan.TotalRoomRate -= totalDiscount;
                                    }

                                    if (roomRate.Rates.Rate.AdditionalGuestAmounts != null)
                                    {
                                        tgRoom.HavingExtraBed = true;
                                        tgRoom.ExtraBedCharge = roomRate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.Sum(x => Convert.ToDecimal(x.Amount.AmountBeforeTax));
                                        tgRoom.ExtraBedCont = roomRate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.Count;
                                    }

                                    var currentRateForSorting = Convert.ToDecimal(roomRate.Rates.Rate.Base.AmountBeforeTax) - discountPerRoomRate;

                                    if (tgRoom.LowestRate < currentRateForSorting)
                                    {
                                        tgRoom.LowestRate = currentRateForSorting;
                                    }

                                    for (int i = 0; i < roomData.Count; i++)
                                    {
                                        var roomRateRow = new TG_RoomRate();
                                        roomRateRow.RoomCount = 1;
                                        roomRateRow.RoomOccupancyAdults = roomData[i].adult;

                                        for (int j = 0; j < roomData[i].ChildAge.Count; j++)
                                        {
                                            if (Convert.ToInt32(roomData[i].ChildAge[j].Age) >= Convert.ToInt32(roomTypeItem.TPA_Extensions.RoomType.MinChildAge) && Convert.ToInt32(roomData[i].ChildAge[j].Age) <= Convert.ToInt32(roomTypeItem.TPA_Extensions.RoomType.MaxChildAge))
                                            {
                                                roomRateRow.TotalChild += 1;
                                                roomRateRow.RoomOccupancyChild += 1;
                                            }
                                            else if (Convert.ToInt32(roomData[i].ChildAge[j].Age) > Convert.ToInt32(roomTypeItem.TPA_Extensions.RoomType.MaxChildAge))
                                            {
                                                roomRateRow.RoomOccupancyAdults += 1;
                                            }
                                            else
                                            {
                                                roomRateRow.TotalChild += 1;
                                            }
                                        }

                                        if (discountPerRoomRate > 0)
                                            roomRateRow.HasDiscount = true;

                                        roomRateRow.TotalAdults = roomData[i].adult;
                                        roomRateRow.RoomRate = Convert.ToDecimal(roomRate.Rates.Rate.Base.AmountBeforeTax) - discountPerRoomRate;
                                        roomRateRow.RatePerRoomPerDayWithoutDiscount = Convert.ToDecimal(roomRate.Rates.Rate.Base.AmountBeforeTax) / roomData.Count;
                                        roomRateRow.RatePerRoomPerDay = roomRateRow.RoomRate / roomData.Count;

                                        roomRateRow.Tax = Convert.ToDecimal(roomRate.Rates.Rate.Base.Taxes.Amount);
                                        roomRateRow.TaxPerDay = roomRateRow.Tax;

                                        if (roomRate.Rates.Rate.AdditionalGuestAmounts != null)
                                        {
                                            roomRateRow.ExtrabedCharge = roomRate.Rates.Rate.AdditionalGuestAmounts.AdditionalGuestAmount.Sum(x => Convert.ToDecimal(x.Amount.AmountBeforeTax));
                                        }

                                        roomRateRow.ValidFrom = roomRate.Rates.Rate.EffectiveDate;
                                        ratePlan.RoomRates.Add(roomRateRow);
                                    }

                                    break;
                                }

                                var filteredRates = ratePlan.RoomRates.GroupBy(i => new
                                {
                                    Adults = i.RoomOccupancyAdults,
                                    Childrens = i.RoomOccupancyChild

                                }).Select(g => new TG_RoomRate
                                {
                                    RoomCount = g.Sum(i => i.RoomCount),
                                    ExtrabedCharge = g.Sum(i => i.ExtrabedCharge),
                                    HasDiscount = g.Select(x => x.HasDiscount).FirstOrDefault(),
                                    RoomRate = g.Sum(i => i.RoomRate),
                                    RatePerRoomPerDayWithoutDiscount = g.Sum(i => i.RatePerRoomPerDayWithoutDiscount),
                                    TotalAdults = g.Sum(i => i.TotalAdults),
                                    TotalChild = g.Sum(i => i.TotalChild),
                                    RoomOccupancyAdults = g.Key.Adults,
                                    RoomOccupancyChild = g.Key.Childrens,
                                    Tax = g.Sum(i => i.Tax),
                                    RatePerRoomPerDay = g.Select(i => i.RatePerRoomPerDay).FirstOrDefault(),
                                    TaxPerDay = g.Select(i => i.TaxPerDay).FirstOrDefault()

                                }).ToList();

                                ratePlan.RoomRates = filteredRates;

                                //Cancellation Policy Processing
                                foreach (var cancellationPolicyItem in ratePlanFromApi.CancelPenalties.CancelPenalty)
                                {

                                    if (cancellationPolicyItem.DeadLine != null && cancellationPolicyItem.DeadLine.OffsetUnitMultiplier != null)
                                    {
                                        ratePlan.OffsetDropTime = cancellationPolicyItem.DeadLine.OffsetDropTime;
                                        ratePlan.OffsetTimeUnit = cancellationPolicyItem.DeadLine.OffsetTimeUnit;
                                        ratePlan.OffsetUnitMultiplier = int.Parse(cancellationPolicyItem.DeadLine.OffsetUnitMultiplier);

                                        ratePlan.NumberOfNights = int.Parse(cancellationPolicyItem.AmountPercent.NmbrOfNights);
                                        ratePlan.IsTaxInclusive = cancellationPolicyItem.AmountPercent.TaxInclusive == "true" ? true : false;
                                    }

                                    if (cancellationPolicyItem.NonRefundable == "false")
                                    {
                                        ratePlan.IsFreeCancellation = true;
                                        ratePlan.IsNonRefundable = false;
                                    }
                                    else if (cancellationPolicyItem.NonRefundable == "true")
                                    {
                                        ratePlan.IsFreeCancellation = false;
                                        ratePlan.IsNonRefundable = true;
                                    }

                                    if (cancellationPolicyItem.PenaltyDescription != null)
                                    {

                                        foreach (var penaltyDescriptionItem in cancellationPolicyItem.PenaltyDescription)
                                        {
                                            if (penaltyDescriptionItem.Name == "CANCELLATION_POLICY")
                                            {
                                                ratePlan.CancellationPolicyDescription += penaltyDescriptionItem.Text + Environment.NewLine;
                                            }
                                            else
                                            {
                                                ratePlan.CancellationPolicyDescription += penaltyDescriptionItem.Name.Replace('_', ' ') + Environment.NewLine;
                                            }

                                        }
                                    }
                                }

                                //RateInclusions Processing
                                foreach (var rateInclusionsItem in ratePlanFromApi.RatePlanInclusions.RatePlanInclusionDesciption.Text)
                                {
                                    ratePlan.RateInclusions += rateInclusionsItem;
                                }
                                
                                ratePlan.RateInclusions = ratePlan.RateInclusions.TrimEnd(new char[] { ',', ' ' });
                                if (string.IsNullOrEmpty(ratePlan.RateInclusions))
                                    ratePlan.RateInclusions = "Room Only" + Environment.NewLine;
                                tgRoom.RatePlans.Add(ratePlan);
                                tgRoom.RatePlans = tgRoom.RatePlans.OrderBy(x => x.TotalRoomRate).ToList();

                            }

                            tgRoom.MaxOccupancy = roomTypeItem.TPA_Extensions.RoomType.MaxGuest;
                            tgRoom.MaxAdult = roomTypeItem.TPA_Extensions.RoomType.MaxAdult;
                            tgRoom.MaxChild = roomTypeItem.TPA_Extensions.RoomType.MaxChild;
                            tgRoom.MaxChildAge = roomTypeItem.TPA_Extensions.RoomType.MaxChildAge;
                            tgRoom.MinChildAge = roomTypeItem.TPA_Extensions.RoomType.MinChildAge;

                            tgHotel.RoomDetails.Add(tgRoom);
                            tgHotel.RoomDetails = tgHotel.RoomDetails.OrderBy(x => x.LowestRate).ToList();
                        }
                    }

                    #endregion roomInfo
                }

                #endregion Processing Result
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return tgHotel;
        }


        public static async Task<List<T>> FetchTodayHotelPriceAsync<T>(List<T> genericObjectList, decimal exchangeRate) where T : IHotelResponseType
        {
            try
            {
                var hotelCodeArr = genericObjectList.Select(x => x.iVendorId).ToArray();
                Dictionary<string, decimal> hotelPriceDictionary = new Dictionary<string, decimal>();

                #region API Call

                List<RoomStayRes> RoomStayTotal = new List<RoomStayRes>();
                string data = string.Empty;

                string usernameTg = ConfigurationManager.AppSettings["UserNameTG"].ToString();
                string propertyIdTg = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
                string passwordTg = ConfigurationManager.AppSettings["PasswordTG"].ToString();

                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.Version = "0.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                objOTA_HotelAvailRQ.SearchCacheLevel = "VeryRecent";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();

                List<HotelRef> hotelRefArr = new List<HotelRef>();

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = DateTime.Today.ToString("yyyy-MM-dd");
                objStayDateRange.End = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();

                RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();
                GuestCountsSH objGuestCounts = new GuestCountsSH();

                List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();

                GuestCountSH adultDetail = new GuestCountSH();
                adultDetail.Count = "1";
                adultDetail.AgeQualifyingCode = "10";
                objGuestCountArr.Add(adultDetail);
                GuestCountSH childDetail = new GuestCountSH();
                childDetail.Count = "0";
                childDetail.AgeQualifyingCode = "8";
                childDetail.Age = "0";
                objGuestCountArr.Add(childDetail);
                objGuestCounts.GuestCount = objGuestCountArr;
                objRoomStayCandidate.GuestCounts = objGuestCounts;
                objRoomStayCandidateArr.Add(objRoomStayCandidate);

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();
                objTPA_Extensions.SeoEnabled = "false";

                UserAuthentication userAuthenticationDetail = new UserAuthentication();
                userAuthenticationDetail.Username = usernameTg;
                userAuthenticationDetail.PropertyId = propertyIdTg;
                userAuthenticationDetail.Password = passwordTg;

                if (hotelCodeArr != null && hotelCodeArr.Count() > 0)
                {
                    var chunckFiftyResultArr = from index in Enumerable.Range(0, hotelCodeArr.Length) group hotelCodeArr[index] by index / 50;

                    foreach (var chunckFisty in chunckFiftyResultArr)
                    {
                        foreach (var hotelCode in chunckFisty)
                        {
                            HotelRef hotelRef = new HotelRef();
                            hotelRef.HotelCode = hotelCode;
                            hotelRefArr.Add(hotelRef);
                        }

                        objTPA_Extensions.UserAuthentication = userAuthenticationDetail;

                        objCriterion.HotelRefMultiVendor = hotelRefArr;
                        objCriterion.RoomStayCandidates = objRoomStayCandidates;
                        objCriterion.StayDateRange = objStayDateRange;
                        objCriterion.TPA_Extensions = objTPA_Extensions;

                        objHotelSearchCriteria.Criterion = objCriterion;

                        objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                        objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                        objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;

                        objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                        objEnvelope.Body = objBody;

                        string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);
                        xmlString = xmlString.Replace("HotelRefList", "HotelRef");
                        data = await clsTravelGuruApi.SearchTravelGuruApiAsync(xmlString);


                        //Deserilize the data using fromxml
                        EnvelopeRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeRes>(data);
                        if (objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays != null)
                        {
                            List<RoomStayRes> RoomStay = await objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.AsQueryable().ToListAsync();
                            RoomStayTotal.AddRange(RoomStay);
                        }

                        hotelRefArr.Clear();

                    }
                }

                #endregion API Call

                foreach (RoomStayRes room in RoomStayTotal)
                {
                    string cheapestRatePlanCode = room.TPA_Extensions.LowestRatePlanId;
                    string vendorId = room.BasicPropertyInfo.HotelCode;

                    var roomRate = room.RoomRates.RoomRate.Where(x => x.RatePlanCode == cheapestRatePlanCode);

                    if (roomRate != null)
                    {
                        var price = roomRate.Select(x => x.Rates.Rate.Base.AmountBeforeTax).FirstOrDefault();
                        if (price != null)
                        {
                            var exchangePriceValue = Math.Round(decimal.Parse(price) * exchangeRate, 2);
                            genericObjectList.Where(x => x.iVendorId == vendorId).FirstOrDefault().dPrice = exchangePriceValue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return genericObjectList;
        }

        public static List<T> FetchTodayHotelPrice<T>(List<T> genericObjectList, decimal exchangeRate) where T : IHotelResponseType
        {
            try
            {
                var hotelCodeArr = genericObjectList.Select(x => x.iVendorId).ToArray();
                Dictionary<string, decimal> hotelPriceDictionary = new Dictionary<string, decimal>();

                #region API Call

                List<RoomStayRes> RoomStayTotal = new List<RoomStayRes>();
                string data = string.Empty;

                string usernameTg = ConfigurationManager.AppSettings["UserNameTG"].ToString();
                string propertyIdTg = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
                string passwordTg = ConfigurationManager.AppSettings["PasswordTG"].ToString();

                EnvelopeSH objEnvelope = new EnvelopeSH();
                objEnvelope.Soap = "http://schemas.xmlsoap.org/soap/envelope/";

                BodySH objBody = new BodySH();

                OTA_HotelAvailRQ objOTA_HotelAvailRQ = new OTA_HotelAvailRQ();
                objOTA_HotelAvailRQ.Xmlns = "http://www.opentravel.org/OTA/2003/05";
                objOTA_HotelAvailRQ.RequestedCurrency = "INR";
                objOTA_HotelAvailRQ.SearchCacheLevel = "TG_RANKING";
                objOTA_HotelAvailRQ.SortOrder = "TG_RANKING";
                objOTA_HotelAvailRQ.Version = "0.0";
                objOTA_HotelAvailRQ.PrimaryLangID = "en";
                objOTA_HotelAvailRQ.SearchCacheLevel = "VeryRecent";

                AvailRequestSegments objAvailRequestSegments = new AvailRequestSegments();

                AvailRequestSegment objAvailRequestSegment = new AvailRequestSegment();

                HotelSearchCriteria objHotelSearchCriteria = new HotelSearchCriteria();

                Criterion objCriterion = new Criterion();

                List<HotelRef> hotelRefArr = new List<HotelRef>();

                StayDateRange objStayDateRange = new StayDateRange();
                objStayDateRange.Start = DateTime.Today.ToString("yyyy-MM-dd");
                objStayDateRange.End = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

                RoomStayCandidates objRoomStayCandidates = new RoomStayCandidates();

                List<RoomStayCandidate> objRoomStayCandidateArr = new List<RoomStayCandidate>();

                RoomStayCandidate objRoomStayCandidate = new RoomStayCandidate();
                GuestCountsSH objGuestCounts = new GuestCountsSH();

                List<GuestCountSH> objGuestCountArr = new List<GuestCountSH>();

                GuestCountSH adultDetail = new GuestCountSH();
                adultDetail.Count = "1";
                adultDetail.AgeQualifyingCode = "10";
                objGuestCountArr.Add(adultDetail);
                GuestCountSH childDetail = new GuestCountSH();
                childDetail.Count = "0";
                childDetail.AgeQualifyingCode = "8";
                childDetail.Age = "0";
                objGuestCountArr.Add(childDetail);
                objGuestCounts.GuestCount = objGuestCountArr;
                objRoomStayCandidate.GuestCounts = objGuestCounts;
                objRoomStayCandidateArr.Add(objRoomStayCandidate);

                objRoomStayCandidates.RoomStayCandidate = objRoomStayCandidateArr;

                TPA_Extensions objTPA_Extensions = new TPA_Extensions();
                objTPA_Extensions.SeoEnabled = "false";

                UserAuthentication userAuthenticationDetail = new UserAuthentication();
                userAuthenticationDetail.Username = usernameTg;
                userAuthenticationDetail.PropertyId = propertyIdTg;
                userAuthenticationDetail.Password = passwordTg;

                if (hotelCodeArr != null && hotelCodeArr.Count() > 0)
                {
                    var chunckFiftyResultArr = from index in Enumerable.Range(0, hotelCodeArr.Length) group hotelCodeArr[index] by index / 50;

                    foreach (var chunckFisty in chunckFiftyResultArr)
                    {
                        foreach (var hotelCode in chunckFisty)
                        {
                            HotelRef hotelRef = new HotelRef();
                            hotelRef.HotelCode = hotelCode;
                            hotelRefArr.Add(hotelRef);
                        }

                        objTPA_Extensions.UserAuthentication = userAuthenticationDetail;

                        objCriterion.HotelRefMultiVendor = hotelRefArr;
                        objCriterion.RoomStayCandidates = objRoomStayCandidates;
                        objCriterion.StayDateRange = objStayDateRange;
                        objCriterion.TPA_Extensions = objTPA_Extensions;

                        objHotelSearchCriteria.Criterion = objCriterion;

                        objAvailRequestSegment.HotelSearchCriteria = objHotelSearchCriteria;

                        objAvailRequestSegments.AvailRequestSegment = objAvailRequestSegment;

                        objOTA_HotelAvailRQ.AvailRequestSegments = objAvailRequestSegments;

                        objBody.OTA_HotelAvailRQ = objOTA_HotelAvailRQ;

                        objEnvelope.Body = objBody;

                        string xmlString = clsSearchHotel.ConvertToXmlStringSH(objEnvelope);
                        xmlString = xmlString.Replace("HotelRefList", "HotelRef");
                        data = clsTravelGuruApi.SearchTravelGuruApi(xmlString);

                        EnvelopeRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopeRes>(data);
                        if (objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays != null)
                        {
                            List<RoomStayRes> RoomStay = objEnvelopeSHRes.Body.OTA_HotelAvailRS.RoomStays.RoomStay.ToList();
                            RoomStayTotal.AddRange(RoomStay);
                        }

                        hotelRefArr.Clear();

                    }
                }

                #endregion API Call

                foreach (RoomStayRes room in RoomStayTotal)
                {
                    string cheapestRatePlanCode = room.TPA_Extensions.LowestRatePlanId;
                    string vendorId = room.BasicPropertyInfo.HotelCode;

                    var roomRate = room.RoomRates.RoomRate.Where(x => x.RatePlanCode == cheapestRatePlanCode);

                    if (roomRate != null)
                    {
                        var price = roomRate.Select(x => x.Rates.Rate.Base.AmountBeforeTax).FirstOrDefault();
                        if (price != null)
                        {
                            var exchangePriceValue = Math.Round(decimal.Parse(price) * exchangeRate, 2);
                            genericObjectList.Where(x => x.iVendorId == vendorId).FirstOrDefault().dPrice = exchangePriceValue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return genericObjectList;
        }

    }

    #region Envelop Class Request
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeSH
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public BodySH Body { get; set; }
        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class BodySH
    {
        [XmlElement(ElementName = "OTA_HotelAvailRQ", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OTA_HotelAvailRQ OTA_HotelAvailRQ { get; set; }
    }
    #endregion

    #region Request Class

    [XmlRoot(ElementName = "HotelRef", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelRef
    {
        [XmlAttribute(AttributeName = "HotelCode")]
        public string HotelCode { get; set; }
    }

    [XmlRoot(ElementName = "StayDateRange", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class StayDateRange
    {
        [XmlAttribute(AttributeName = "End")]
        public string End { get; set; }
        [XmlAttribute(AttributeName = "Start")]
        public string Start { get; set; }
    }

    [XmlRoot(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCountSH
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
        [XmlAttribute(AttributeName = "Count")]
        public string Count { get; set; }
        [XmlAttribute(AttributeName = "Age")]
        public string Age { get; set; }
        [XmlAttribute(AttributeName = "ResGuestRPH")]
        public string ResGuestRPH { get; set; }
    }



    [XmlRoot(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCountsSH
    {
        [XmlElement(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<GuestCountSH> GuestCount { get; set; }
    }

    [XmlRoot(ElementName = "RoomStayCandidate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStayCandidate
    {
        [XmlElement(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuestCountsSH GuestCounts { get; set; }
    }

    [XmlRoot(ElementName = "RoomStayCandidates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStayCandidates
    {
        [XmlElement(ElementName = "RoomStayCandidate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomStayCandidate> RoomStayCandidate { get; set; }
    }

    [XmlRoot(ElementName = "UserAuthentication", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class UserAuthentication
    {
        [XmlAttribute(AttributeName = "password")]
        public string Password { get; set; }
        [XmlAttribute(AttributeName = "propertyId")]
        public string PropertyId { get; set; }
        [XmlAttribute(AttributeName = "username")]
        public string Username { get; set; }
    }

    [XmlRoot(ElementName = "Pagination", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Pagination
    {
        [XmlAttribute(AttributeName = "enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "hotelsFrom")]
        public string HotelsFrom { get; set; }
        [XmlAttribute(AttributeName = "hotelsTo")]
        public string HotelsTo { get; set; }
    }

    [XmlRoot(ElementName = "Reviews", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Reviews
    {
    }

    [XmlRoot(ElementName = "Promotion", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Promotion
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TPA_Extensions
    {
        [XmlElement(ElementName = "UserAuthentication", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public UserAuthentication UserAuthentication { get; set; }
        [XmlAttribute(AttributeName = "SeoEnabled", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string SeoEnabled { get; set; }
        [XmlElement(ElementName = "Pagination", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Pagination Pagination { get; set; }
        [XmlElement(ElementName = "Reviews", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Reviews Reviews { get; set; }
        [XmlElement(ElementName = "Promotion", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Promotion Promotion { get; set; }
    }

    [XmlRoot(ElementName = "Criterion", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Criterion
    {
        [XmlElement(ElementName = "HotelRef", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelRef HotelRef { get; set; }
        [XmlElement(ElementName = "StayDateRange", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public StayDateRange StayDateRange { get; set; }
        [XmlElement(ElementName = "RoomStayCandidates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStayCandidates RoomStayCandidates { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
        [XmlElement(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AddressSH Address { get; set; }
        [XmlElement(ElementName = "RateRange", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RateRange RateRange { get; set; }

        // For Multi Vendor
        [XmlElement(ElementName = "HotelRefList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<HotelRef> HotelRefMultiVendor { get; set; }
    }

    [XmlRoot(ElementName = "HotelSearchCriteria", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelSearchCriteria
    {
        [XmlElement(ElementName = "Criterion", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Criterion Criterion { get; set; }
    }

    [XmlRoot(ElementName = "AvailRequestSegment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AvailRequestSegment
    {
        [XmlElement(ElementName = "HotelSearchCriteria", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelSearchCriteria HotelSearchCriteria { get; set; }
    }

    [XmlRoot(ElementName = "AvailRequestSegments", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AvailRequestSegments
    {
        [XmlElement(ElementName = "AvailRequestSegment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AvailRequestSegment AvailRequestSegment { get; set; }
    }

    [XmlRoot(ElementName = "OTA_HotelAvailRQ", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_HotelAvailRQ
    {
        [XmlElement(ElementName = "AvailRequestSegments", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AvailRequestSegments AvailRequestSegments { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "RequestedCurrency")]
        public string RequestedCurrency { get; set; }
        [XmlAttribute(AttributeName = "SearchCacheLevel")]
        public string SearchCacheLevel { get; set; }
        [XmlAttribute(AttributeName = "SortOrder")]
        public string SortOrder { get; set; }
        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "PrimaryLangID")]
        public string PrimaryLangID { get; set; }
    }

    [XmlRoot(ElementName = "CountryName")]
    public class CountryNameSH
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "Address")]
    public class AddressSH
    {
        [XmlElement(ElementName = "CityName")]
        public string CityName { get; set; }
        [XmlElement(ElementName = "CountryName")]
        public CountryNameSH CountryName { get; set; }
    }

    [XmlRoot(ElementName = "RateRange")]
    public class RateRange
    {
        [XmlAttribute(AttributeName = "MinRate")]
        public string MinRate { get; set; }
        [XmlAttribute(AttributeName = "MaxRate")]
        public string MaxRate { get; set; }
    }

    #endregion

    #region Response Data For City Search
    [XmlRoot(ElementName = "RoomDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomDescription
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Text { get; set; }
        [XmlElement(ElementName = "Image", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> Image { get; set; }
    }

    [XmlRoot(ElementName = "Occupancy", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Occupancy
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
        [XmlAttribute(AttributeName = "MaxOccupancy")]
        public string MaxOccupancy { get; set; }
        [XmlAttribute(AttributeName = "MaxAge")]
        public string MaxAge { get; set; }
        [XmlAttribute(AttributeName = "MinAge")]
        public string MinAge { get; set; }
    }

    [XmlRoot(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomTypeRes
    {
        [XmlAttribute(AttributeName = "maxAdult")]
        public string MaxAdult { get; set; }
        [XmlAttribute(AttributeName = "maxChild")]
        public string MaxChild { get; set; }
        [XmlAttribute(AttributeName = "maxChildAge")]
        public string MaxChildAge { get; set; }
        [XmlAttribute(AttributeName = "maxGuest")]
        public string MaxGuest { get; set; }
        [XmlAttribute(AttributeName = "minChildAge")]
        public string MinChildAge { get; set; }
        [XmlAttribute(AttributeName = "propertyLevel")]
        public string PropertyLevel { get; set; }
        [XmlAttribute(AttributeName = "smoking")]
        public string Smoking { get; set; }
        [XmlElement(ElementName = "RoomDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomDescription RoomDescription { get; set; }
        [XmlElement(ElementName = "AdditionalDetails", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string AdditionalDetails { get; set; }
        [XmlElement(ElementName = "Occupancy", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Occupancy> Occupancy { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "NonSmoking")]
        public string NonSmoking { get; set; }
        [XmlAttribute(AttributeName = "RoomType")]
        public string RoomType { get; set; }
        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }
    }

    [XmlRoot(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TPA_ExtensionsRes
    {
        [XmlElement(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomTypeRes RoomType { get; set; }
        [XmlElement(ElementName = "AdditionalGuests", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AdditionalGuests AdditionalGuests { get; set; }
        [XmlElement(ElementName = "DiscountCouponDisplayIndicator", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DiscountCouponDisplayIndicator DiscountCouponDisplayIndicator { get; set; }
        [XmlElement(ElementName = "BestRatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string BestRatePlan { get; set; }
        [XmlElement(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Rate Rate { get; set; }
        [XmlElement(ElementName = "HotelBasicInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelBasicInformation HotelBasicInformation { get; set; }
        [XmlElement(ElementName = "DeepLinkInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DeepLinkInformation DeepLinkInformation { get; set; }
        [XmlAttribute(AttributeName = "LowestRatePlanId")]
        public string LowestRatePlanId { get; set; }
        [XmlAttribute(AttributeName = "StopSell")]
        public string StopSell { get; set; }
        [XmlElement(ElementName = "HotelsInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelsInfo HotelsInfo { get; set; }
    }

    [XmlRoot(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomTypesRes
    {
        [XmlElement(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomTypeRes> RoomType { get; set; }
    }

    [XmlRoot(ElementName = "PenaltyDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PenaltyDescription
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "AmountPercent", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AmountPercent
    {
        [XmlAttribute(AttributeName = "NmbrOfNights")]
        public string NmbrOfNights { get; set; }
        [XmlAttribute(AttributeName = "TaxInclusive")]
        public string TaxInclusive { get; set; }
    }

    [XmlRoot(ElementName = "Deadline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Deadline
    {
        [XmlAttribute(AttributeName = "OffsetDropTime=")]
        public string OffsetDropTime { get; set; }
        [XmlAttribute(AttributeName = "OffsetTimeUnit")]
        public string OffsetTimeUnit { get; set; }
        [XmlAttribute(AttributeName = "OffsetUnitMultiplier")]
        public string OffsetUnitMultiplier { get; set; }
    }

    [XmlRoot(ElementName = "CancelPenalty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CancelPenalty
    {
        [XmlElement(ElementName = "Deadline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        //public string Deadline { get; set; }
        public Deadline DeadLine { get; set; }
        [XmlElement(ElementName = "AmountPercent", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AmountPercent AmountPercent { get; set; }
        [XmlElement(ElementName = "PenaltyDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<PenaltyDescription> PenaltyDescription { get; set; }
        [XmlAttribute(AttributeName = "NonRefundable")]
        public string NonRefundable { get; set; }
    }

    [XmlRoot(ElementName = "CancelPenalties", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CancelPenalties
    {
        [XmlElement(ElementName = "CancelPenalty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<CancelPenalty> CancelPenalty { get; set; }
    }

    [XmlRoot(ElementName = "RatePlanDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanDescription
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> Text { get; set; }
    }

    [XmlRoot(ElementName = "RatePlanInclusionDesciption", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanInclusionDesciption
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> Text { get; set; }
    }

    [XmlRoot(ElementName = "RatePlanInclusions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanInclusions
    {
        [XmlElement(ElementName = "RatePlanInclusionDesciption", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlanInclusionDesciption RatePlanInclusionDesciption { get; set; }
    }

    [XmlRoot(ElementName = "AdditionalGuest", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AdditionalGuest
    {
        [XmlAttribute(AttributeName = "adults")]
        public string Adults { get; set; }
        [XmlAttribute(AttributeName = "children")]
        public string Children { get; set; }
        [XmlAttribute(AttributeName = "roomNo")]
        public string RoomNo { get; set; }
    }

    [XmlRoot(ElementName = "AdditionalGuests", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AdditionalGuests
    {
        [XmlElement(ElementName = "AdditionalGuest", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AdditionalGuest AdditionalGuest { get; set; }
    }

    [XmlRoot(ElementName = "DiscountCouponDisplayIndicator", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DiscountCouponDisplayIndicator
    {
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
    }

    [XmlRoot(ElementName = "RatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanRes
    {
        [XmlElement(ElementName = "CancelPenalties", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CancelPenalties CancelPenalties { get; set; }
        [XmlElement(ElementName = "RatePlanDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlanDescription RatePlanDescription { get; set; }
        [XmlElement(ElementName = "RatePlanInclusions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlanInclusions RatePlanInclusions { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "AvailableQuantity")]
        public string AvailableQuantity { get; set; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
        [XmlAttribute(AttributeName = "RatePlanName")]
        public string RatePlanName { get; set; }
        [XmlAttribute(AttributeName = "RatePlanType")]
        public string RatePlanType { get; set; }
    }

    [XmlRoot(ElementName = "RatePlans", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlansRes
    {
        [XmlElement(ElementName = "RatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RatePlanRes> RatePlan { get; set; }
    }

    [XmlRoot(ElementName = "Tax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Tax
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TaxesRes
    {
        [XmlElement(ElementName = "Tax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Tax> Tax { get; set; }
        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Base
    {
        [XmlElement(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Taxes Taxes { get; set; }
        [XmlAttribute(AttributeName = "AmountBeforeTax")]
        public string AmountBeforeTax { get; set; }
    }

    [XmlRoot(ElementName = "Amount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Amount
    {
        [XmlAttribute(AttributeName = "AmountBeforeTax")]
        public string AmountBeforeTax { get; set; }
    }

    [XmlRoot(ElementName = "AdditionalGuestAmount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AdditionalGuestAmount
    {
        [XmlElement(ElementName = "Amount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Amount Amount { get; set; }
        [XmlAttribute(AttributeName = "RPH")]
        public string RPH { get; set; }
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
    }

    [XmlRoot(ElementName = "AdditionalGuestAmounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AdditionalGuestAmounts
    {
        [XmlElement(ElementName = "AdditionalGuestAmount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<AdditionalGuestAmount> AdditionalGuestAmount { get; set; }
        //public AdditionalGuestAmount AdditionalGuestAmount { get; set; }
    }

    [XmlRoot(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Rate
    {
        [XmlAttribute(AttributeName = "BaseAdultOccupancy")]
        public string BaseAdultOccupancy { get; set; }
        [XmlAttribute(AttributeName = "BaseChildOccupancy")]
        public string BaseChildOccupancy { get; set; }
        [XmlAttribute(AttributeName = "Bookable")]
        public string Bookable { get; set; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }
        [XmlElement(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Base Base { get; set; }
        [XmlElement(ElementName = "Discount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Discount Discount { get; set; }
        [XmlElement(ElementName = "AdditionalGuestAmounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AdditionalGuestAmounts AdditionalGuestAmounts { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "EffectiveDate")]
        public string EffectiveDate { get; set; }
    }

    [XmlRoot(ElementName = "Discount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Discount
    {
        [XmlElement(ElementName = "DiscountReason", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string DiscountReason { get; set; }
        [XmlAttribute(AttributeName = "AppliesTo")]
        public string AppliesTo { get; set; }
        [XmlAttribute(AttributeName = "AmountBeforeTax")]
        public string AmountBeforeTax { get; set; }
    }

    [XmlRoot(ElementName = "Rates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Rates
    {
        [XmlElement(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Rate Rate { get; set; }
    }

    [XmlRoot(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCountRes
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
        [XmlAttribute(AttributeName = "Count")]
        public string Count { get; set; }
    }

    [XmlRoot(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCountsRes
    {
        [XmlElement(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<GuestCountRes> GuestCount { get; set; }
    }

    [XmlRoot(ElementName = "RoomRate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomRate
    {
        [XmlElement(ElementName = "Rates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Rates Rates { get; set; }
        [XmlElement(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuestCountsRes GuestCounts { get; set; }
        [XmlAttribute(AttributeName = "RoomID")]
        public string RoomID { get; set; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
    }

    [XmlRoot(ElementName = "RoomRates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomRatesRes
    {
        [XmlElement(ElementName = "RoomRate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomRate> RoomRate { get; set; }
    }

    [XmlRoot(ElementName = "BasicPropertyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BasicPropertyInfoRes
    {
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "HotelCode")]
        public string HotelCode { get; set; }
    }

    [XmlRoot(ElementName = "Reviews", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ReviewsRes
    {
        [XmlAttribute(AttributeName = "ReviewCount")]
        public string ReviewCount { get; set; }
        [XmlAttribute(AttributeName = "ReviewRating")]
        public string ReviewRating { get; set; }
    }

    [XmlRoot(ElementName = "HotelBasicInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelBasicInformation
    {
        [XmlElement(ElementName = "Reviews", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ReviewsRes Reviews { get; set; }
        [XmlAttribute(AttributeName = "HotelType")]
        public string HotelType { get; set; }
        [XmlAttribute(AttributeName = "Rank")]
        public string Rank { get; set; }
    }

    [XmlRoot(ElementName = "DeepLinkInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DeepLinkInformation
    {
        [XmlAttribute(AttributeName = "overviewURL")]
        public string OverviewURL { get; set; }
    }

    [XmlRoot(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStayRes
    {
        [XmlElement(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomTypesRes RoomTypes { get; set; }
        [XmlElement(ElementName = "RatePlans", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlansRes RatePlans { get; set; }
        [XmlElement(ElementName = "RoomRates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomRatesRes RoomRates { get; set; }
        [XmlElement(ElementName = "BasicPropertyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BasicPropertyInfoRes BasicPropertyInfo { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsRes TPA_Extensions { get; set; }
    }

    [XmlRoot(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStaysRes
    {
        [XmlElement(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomStayRes> RoomStay { get; set; }
    }

    [XmlRoot(ElementName = "HotelsInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelsInfo
    {
        [XmlAttribute(AttributeName = "available")]
        public string Available { get; set; }
        [XmlAttribute(AttributeName = "deals")]
        public string Deals { get; set; }
        [XmlAttribute(AttributeName = "maxPrice")]
        public string MaxPrice { get; set; }
        [XmlAttribute(AttributeName = "minPrice")]
        public string MinPrice { get; set; }
        [XmlAttribute(AttributeName = "total")]
        public string Total { get; set; }
    }

    [XmlRoot(ElementName = "OTA_HotelAvailRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_HotelAvailRS
    {
        [XmlElement(ElementName = "Success", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Success { get; set; }
        [XmlElement(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStaysRes RoomStays { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "AltLangID")]
        public string AltLangID { get; set; }
        [XmlAttribute(AttributeName = "PrimaryLangID")]
        public string PrimaryLangID { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class BodyRes
    {
        [XmlElement(ElementName = "OTA_HotelAvailRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OTA_HotelAvailRS OTA_HotelAvailRS { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeRes
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public BodyRes Body { get; set; }
        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
    }
    #endregion

    #region Response Data For Hotel Specific Search
    [XmlRoot(ElementName = "RoomDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomDescriptionHRes
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Text { get; set; }
        [XmlElement(ElementName = "Image", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> Image { get; set; }
    }

    [XmlRoot(ElementName = "Occupancy", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OccupancyHRes
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
        [XmlAttribute(AttributeName = "MaxOccupancy")]
        public string MaxOccupancy { get; set; }
        [XmlAttribute(AttributeName = "MaxAge")]
        public string MaxAge { get; set; }
        [XmlAttribute(AttributeName = "MinAge")]
        public string MinAge { get; set; }
    }

    [XmlRoot(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomTypeHRes
    {
        [XmlAttribute(AttributeName = "maxAdult")]
        public string MaxAdult { get; set; }
        [XmlAttribute(AttributeName = "maxChild")]
        public string MaxChild { get; set; }
        [XmlAttribute(AttributeName = "maxChildAge")]
        public string MaxChildAge { get; set; }
        [XmlAttribute(AttributeName = "maxGuest")]
        public string MaxGuest { get; set; }
        [XmlAttribute(AttributeName = "minChildAge")]
        public string MinChildAge { get; set; }
        [XmlAttribute(AttributeName = "propertyLevel")]
        public string PropertyLevel { get; set; }
        [XmlAttribute(AttributeName = "smoking")]
        public string Smoking { get; set; }
        [XmlElement(ElementName = "RoomDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomDescriptionHRes RoomDescription { get; set; }
        [XmlElement(ElementName = "AdditionalDetails", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string AdditionalDetails { get; set; }
        [XmlElement(ElementName = "Occupancy", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<OccupancyHRes> Occupancy { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsHRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "NonSmoking")]
        public string NonSmoking { get; set; }
        [XmlAttribute(AttributeName = "RoomType")]
        public string RoomType { get; set; }
        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }
    }

    [XmlRoot(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TPA_ExtensionsHRes
    {
        [XmlElement(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomTypeHRes RoomType { get; set; }
        [XmlElement(ElementName = "DiscountCouponDisplayIndicator", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DiscountCouponDisplayIndicatorHRes DiscountCouponDisplayIndicator { get; set; }
        [XmlElement(ElementName = "BestRatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string BestRatePlan { get; set; }
        [XmlElement(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Rate Rate { get; set; }
        [XmlElement(ElementName = "HotelBasicInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelBasicInformationHRes HotelBasicInformation { get; set; }
        [XmlElement(ElementName = "DeepLinkInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DeepLinkInformationHRes DeepLinkInformation { get; set; }
        [XmlAttribute(AttributeName = "LowestRatePlanId")]
        public string LowestRatePlanId { get; set; }
        [XmlAttribute(AttributeName = "StopSell")]
        public string StopSell { get; set; }
    }

    [XmlRoot(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomTypesHRes
    {
        [XmlElement(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomTypeHRes> RoomType { get; set; }
    }

    [XmlRoot(ElementName = "PenaltyDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PenaltyDescriptionHRes
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "CancelPenalty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CancelPenaltyHRes
    {
        [XmlElement(ElementName = "Deadline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        //public string Deadline { get; set; }
        public Deadline DeadLine { get; set; }
        [XmlElement(ElementName = "AmountPercent", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AmountPercent AmountPercent { get; set; }
        [XmlElement(ElementName = "PenaltyDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<PenaltyDescriptionHRes> PenaltyDescription { get; set; }
        [XmlAttribute(AttributeName = "NonRefundable")]
        public string NonRefundable { get; set; }

        ////[XmlElement(ElementName = "Deadline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        ////public string Deadline { get; set; }
        ////[XmlElement(ElementName = "PenaltyDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        ////public List<PenaltyDescriptionHRes> PenaltyDescription { get; set; }
        ////[XmlAttribute(AttributeName = "NonRefundable")]
        ////public string NonRefundable { get; set; }
    }

    [XmlRoot(ElementName = "CancelPenalties", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CancelPenaltiesHRes
    {
        [XmlElement(ElementName = "CancelPenalty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<CancelPenaltyHRes> CancelPenalty { get; set; }
    }

    [XmlRoot(ElementName = "RatePlanDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanDescriptionHRes
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> Text { get; set; }
    }

    [XmlRoot(ElementName = "RatePlanInclusionDesciption", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanInclusionDesciptionHRes
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> Text { get; set; }
    }

    [XmlRoot(ElementName = "RatePlanInclusions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanInclusionsHRes
    {
        [XmlElement(ElementName = "RatePlanInclusionDesciption", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlanInclusionDesciptionHRes RatePlanInclusionDesciption { get; set; }
    }

    [XmlRoot(ElementName = "DiscountCouponDisplayIndicator", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DiscountCouponDisplayIndicatorHRes
    {
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
    }

    [XmlRoot(ElementName = "RatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlanHRes
    {
        [XmlElement(ElementName = "CancelPenalties", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CancelPenaltiesHRes CancelPenalties { get; set; }
        [XmlElement(ElementName = "RatePlanDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlanDescriptionHRes RatePlanDescription { get; set; }
        [XmlElement(ElementName = "RatePlanInclusions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlanInclusionsHRes RatePlanInclusions { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsHRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "AvailableQuantity")]
        public string AvailableQuantity { get; set; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
        [XmlAttribute(AttributeName = "RatePlanName")]
        public string RatePlanName { get; set; }
        [XmlAttribute(AttributeName = "RatePlanType")]
        public string RatePlanType { get; set; }
    }

    [XmlRoot(ElementName = "RatePlans", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlansHRes
    {
        [XmlElement(ElementName = "RatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RatePlanHRes> RatePlan { get; set; }
    }

    [XmlRoot(ElementName = "Tax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TaxHRes
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TaxesHRes
    {
        [XmlElement(ElementName = "Tax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<TaxHRes> Tax { get; set; }
        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BaseHRes
    {
        [XmlElement(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TaxesHRes Taxes { get; set; }
        [XmlAttribute(AttributeName = "AmountBeforeTax")]
        public string AmountBeforeTax { get; set; }
    }

    [XmlRoot(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RateHRes
    {
        [XmlAttribute(AttributeName = "BaseAdultOccupancy")]
        public string BaseAdultOccupancy { get; set; }
        [XmlAttribute(AttributeName = "BaseChildOccupancy")]
        public string BaseChildOccupancy { get; set; }
        [XmlAttribute(AttributeName = "Bookable")]
        public string Bookable { get; set; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }
        [XmlElement(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BaseHRes Base { get; set; }
        [XmlElement(ElementName = "Discount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Discount Discount { get; set; }
        [XmlElement(ElementName = "AdditionalGuestAmounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AdditionalGuestAmounts AdditionalGuestAmounts { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsHRes TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "EffectiveDate")]
        public string EffectiveDate { get; set; }
    }

    [XmlRoot(ElementName = "Rates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatesHRes
    {
        [XmlElement(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RateHRes Rate { get; set; }
    }

    [XmlRoot(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCountHRes
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
        [XmlAttribute(AttributeName = "Count")]
        public string Count { get; set; }
    }

    [XmlRoot(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCountsHRes
    {
        [XmlElement(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<GuestCount> GuestCount { get; set; }
    }

    [XmlRoot(ElementName = "RoomRate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomRateHRes
    {
        [XmlElement(ElementName = "Rates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatesHRes Rates { get; set; }
        [XmlElement(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuestCountsHRes GuestCounts { get; set; }
        [XmlAttribute(AttributeName = "RoomID")]
        public string RoomID { get; set; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
    }

    [XmlRoot(ElementName = "RoomRates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomRatesHRes
    {
        [XmlElement(ElementName = "RoomRate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomRateHRes> RoomRate { get; set; }
    }

    [XmlRoot(ElementName = "Position", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Position
    {
        [XmlAttribute(AttributeName = "Latitude")]
        public string Latitude { get; set; }
        [XmlAttribute(AttributeName = "Longitude")]
        public string Longitude { get; set; }
    }

    [XmlRoot(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AddressHRes
    {
        [XmlElement(ElementName = "AddressLine", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string AddressLine { get; set; }
        [XmlElement(ElementName = "CityName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CityName { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "StateProv", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string StateProv { get; set; }
        [XmlElement(ElementName = "CountryName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CountryName { get; set; }
    }

    [XmlRoot(ElementName = "Award", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Award
    {
        [XmlAttribute(AttributeName = "Rating")]
        public string Rating { get; set; }
    }

    [XmlRoot(ElementName = "BasicPropertyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BasicPropertyInfoHRes
    {
        [XmlElement(ElementName = "Position", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Position Position { get; set; }
        [XmlElement(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AddressHRes Address { get; set; }
        [XmlElement(ElementName = "Award", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Award Award { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "HotelCode")]
        public string HotelCode { get; set; }
        [XmlAttribute(AttributeName = "HotelName")]
        public string HotelName { get; set; }
    }

    [XmlRoot(ElementName = "Reviews", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ReviewsHRes
    {
        [XmlAttribute(AttributeName = "ReviewImageUrl")]
        public string ReviewImageUrl { get; set; }
    }

    [XmlRoot(ElementName = "ThumbnailImageObj", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ThumbnailImageObj
    {
        [XmlAttribute(AttributeName = "caption")]
        public string Caption { get; set; }
        [XmlAttribute(AttributeName = "category")]
        public string Category { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "sizeName")]
        public string SizeName { get; set; }
        [XmlAttribute(AttributeName = "subCategory")]
        public string SubCategory { get; set; }
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
    }

    [XmlRoot(ElementName = "LargeImageObj", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class LargeImageObj
    {
        [XmlAttribute(AttributeName = "caption")]
        public string Caption { get; set; }
        [XmlAttribute(AttributeName = "category")]
        public string Category { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "sizeName")]
        public string SizeName { get; set; }
        [XmlAttribute(AttributeName = "subCategory")]
        public string SubCategory { get; set; }
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
    }

    [XmlRoot(ElementName = "ImagesList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ImagesList
    {
        [XmlElement(ElementName = "ThumbnailImageObj", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ThumbnailImageObj ThumbnailImageObj { get; set; }
        [XmlElement(ElementName = "LargeImageObj", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public LargeImageObj LargeImageObj { get; set; }
    }

    [XmlRoot(ElementName = "ImageJSON", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ImageJSON
    {
        [XmlElement(ElementName = "ImagesList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<ImagesList> ImagesList { get; set; }
    }

    [XmlRoot(ElementName = "Multimedia", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Multimedia
    {
        [XmlElement(ElementName = "ImageJSON", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ImageJSON ImageJSON { get; set; }
        [XmlAttribute(AttributeName = "ImageUrl")]
        public string ImageUrl { get; set; }
        [XmlAttribute(AttributeName = "ThumbnailImageName")]
        public string ThumbnailImageName { get; set; }
        [XmlAttribute(AttributeName = "ThumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }

    [XmlRoot(ElementName = "HotelPOI", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelPOI
    {
        [XmlAttribute(AttributeName = "POIDistance")]
        public string POIDistance { get; set; }
        [XmlAttribute(AttributeName = "POIName")]
        public string POIName { get; set; }
    }

    [XmlRoot(ElementName = "POI", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class POI
    {
        [XmlElement(ElementName = "HotelPOI", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<HotelPOI> HotelPOI { get; set; }
    }

    [XmlRoot(ElementName = "PropertyAmenities", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PropertyAmenitiesHRes
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
    }

    [XmlRoot(ElementName = "Amenities", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AmenitiesHRes
    {
        [XmlElement(ElementName = "PropertyAmenities", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<PropertyAmenitiesHRes> PropertyAmenities { get; set; }
    }

    [XmlRoot(ElementName = "CrossLinks", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CrossLinks
    {
        [XmlAttribute(AttributeName = "PropertyName")]
        public string PropertyName { get; set; }
        [XmlAttribute(AttributeName = "URL")]
        public string URL { get; set; }
    }

    [XmlRoot(ElementName = "HotelBasicInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelBasicInformationHRes
    {
        [XmlElement(ElementName = "Reviews", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ReviewsHRes Reviews { get; set; }
        [XmlElement(ElementName = "Multimedia", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Multimedia Multimedia { get; set; }
        [XmlElement(ElementName = "POI", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public POI POI { get; set; }
        [XmlElement(ElementName = "Amenities", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AmenitiesHRes Amenities { get; set; }
        [XmlElement(ElementName = "CrossLinks", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<CrossLinks> CrossLinks { get; set; }
        [XmlAttribute(AttributeName = "AmenityDescription")]
        public string AmenityDescription { get; set; }
        [XmlAttribute(AttributeName = "Area")]
        public string Area { get; set; }
        [XmlAttribute(AttributeName = "AreaSeoId")]
        public string AreaSeoId { get; set; }
        [XmlAttribute(AttributeName = "CheckInTime")]
        public string CheckInTime { get; set; }
        [XmlAttribute(AttributeName = "CheckOutTime")]
        public string CheckOutTime { get; set; }
        [XmlAttribute(AttributeName = "CitySeoId")]
        public string CitySeoId { get; set; }
        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "HotelType")]
        public string HotelType { get; set; }
        [XmlAttribute(AttributeName = "NumberOfFloors")]
        public string NumberOfFloors { get; set; }
        [XmlAttribute(AttributeName = "NumberOfRooms")]
        public string NumberOfRooms { get; set; }
        [XmlAttribute(AttributeName = "StateSeoId")]
        public string StateSeoId { get; set; }
        [XmlAttribute(AttributeName = "isFlexibleCheckIn")]
        public string IsFlexibleCheckIn { get; set; }

        public string HotelCategory { get; set; }
    }

    [XmlRoot(ElementName = "DeepLinkInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DeepLinkInformationHRes
    {
        [XmlAttribute(AttributeName = "overviewURL")]
        public string OverviewURL { get; set; }
    }

    [XmlRoot(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStayHRes
    {
        [XmlElement(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomTypesHRes RoomTypes { get; set; }
        [XmlElement(ElementName = "RatePlans", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlansHRes RatePlans { get; set; }
        [XmlElement(ElementName = "RoomRates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomRatesHRes RoomRates { get; set; }
        [XmlElement(ElementName = "BasicPropertyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BasicPropertyInfoHRes BasicPropertyInfo { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_ExtensionsHRes TPA_Extensions { get; set; }
    }

    [XmlRoot(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStaysHRes
    {
        [XmlElement(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStayHRes RoomStay { get; set; }
    }

    [XmlRoot(ElementName = "OTA_HotelAvailRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_HotelAvailRSHRes
    {
        [XmlElement(ElementName = "Success", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Success { get; set; }
        [XmlElement(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStaysHRes RoomStays { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "AltLangID")]
        public string AltLangID { get; set; }
        [XmlAttribute(AttributeName = "PrimaryLangID")]
        public string PrimaryLangID { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class BodyHRes
    {
        [XmlElement(ElementName = "OTA_HotelAvailRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OTA_HotelAvailRSHRes OTA_HotelAvailRS { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeHRes
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public BodyHRes Body { get; set; }
        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
    }
    #endregion
}