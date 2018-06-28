using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using OneFineRateAppUtil;
using System.Threading.Tasks;
using System.IO;

namespace OneFineRateWeb.Controllers.TravelGuru
{
    public class OfferReviewTGController : BaseController
    {
        [HttpGet]
        //[OutputCache(Duration = 30, VaryByParam = "Id;cIn;cOut;sLocation;sRoomData", VaryByCustom = "User")] // Cached for 30 second
        public ActionResult Index(string Id, string cIn, string cOut, string sLocation, string sRoomData, string sRequestType)
        {
            PropDetailsM propDetails = new PropDetailsM();

            if (!string.IsNullOrEmpty(Id))
            {
                PropSearchRequestModel dbRequestParamModel = new PropSearchRequestModel();
                dbRequestParamModel.iVendorId = Id;
                var roomDataResult = new List<RoomData>();

                #region RoomOccupancySearch

                DataTable dtTblRoomOccupancySearch = new DataTable();

                dtTblRoomOccupancySearch.Columns.AddRange(new DataColumn[3] 
                { 
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                });

                DataTable dtTblChildrenAgeSearch = new DataTable();
                dtTblChildrenAgeSearch.Columns.AddRange(new DataColumn[2] 
                { 
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))
                });

                if (!string.IsNullOrEmpty(cIn) && !string.IsNullOrEmpty(cOut))
                {
                    dbRequestParamModel.dtCheckIn = DateTime.Parse(cIn);
                    dbRequestParamModel.dtCheckOut = DateTime.Parse(cOut);
                }
                else
                {
                    dbRequestParamModel.dtCheckIn = DateTime.Now.Date.AddDays(1);
                    dbRequestParamModel.dtCheckOut = DateTime.Now.Date.AddDays(2);

                    cIn = DateTime.Today.ToString("dd/MM/yyyy");
                    cOut = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                }

                dbRequestParamModel.bLogin = User.Identity.IsAuthenticated;
                dbRequestParamModel.sCurrency = CurrencyCode;

                //Insert the room data in the table created 
                if (!string.IsNullOrEmpty(sRoomData))
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);
                }
                else
                {
                    roomDataResult.Add(new RoomData() { adult = 2, child = 0, room = 1, ChildAge = new List<ChildAge>() { new ChildAge() { Age = "0" } } });
                }

                foreach (var room in roomDataResult)
                {
                    DataRow roomOccupancy = dtTblRoomOccupancySearch.NewRow();
                    roomOccupancy["ID"] = room.room;
                    roomOccupancy["iAdults"] = room.adult;
                    roomOccupancy["children"] = room.child;
                    dtTblRoomOccupancySearch.Rows.Add(roomOccupancy);

                    foreach (var child in room.ChildAge)
                    {
                        DataRow dtChildrenAge = dtTblChildrenAgeSearch.NewRow();
                        dtChildrenAge["ID"] = room.room;
                        dtChildrenAge["Age"] = child.Age;
                        dtTblChildrenAgeSearch.Rows.Add(dtChildrenAge);
                    }
                }

                sRoomData = clsUtils.ConvertToJson(roomDataResult);


                #endregion

                string roomIds = string.Empty;
                List<string> roomIdsArr = new List<string>();
                List<int> hotelAmenitiesIdsArr = new List<int>();

                var ofrServiceCharge = BL_PropDetailsTG.GetOfrServiceCharge(dbRequestParamModel.dtCheckIn,dbRequestParamModel.dtCheckOut);

                //Get TG hotel details 
                var hotelDetailsFromApi = clsSearchHotel.FetchHotelsDetailsByVendorId(false, dbRequestParamModel.iVendorId, dbRequestParamModel.dtCheckIn.ToString("yyyy-MM-dd"), dbRequestParamModel.dtCheckOut.ToString("yyyy-MM-dd"), dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, roomDataResult);

                if (hotelDetailsFromApi != null && !string.IsNullOrEmpty(hotelDetailsFromApi.HotelCode))
                {
                    roomIdsArr = hotelDetailsFromApi.RoomDetails.Select(x => x.RoomId).ToList();
                    roomIds = string.Join(",", roomIdsArr);

                    hotelAmenitiesIdsArr = hotelDetailsFromApi.HotelAmenities.Select(x => int.Parse(x.code)).ToList();

                    hotelDetailsFromApi.HotelAmenities = BL_PropDetailsTG.GetHotelAmenities(hotelAmenitiesIdsArr.ToArray());

                    if (hotelDetailsFromApi.GalleryImages.Count <= 0)
                    {
                        propDetails = BL_PropDetailsTG.GetPropertyDetails(dbRequestParamModel.iVendorId, roomIds);
                    }
                    else
                    {
                        propDetails = BL_PropDetailsTG.GetPropertyDetailsWithoutGallery(dbRequestParamModel.iVendorId, roomIds);
                    }
                }
                else
                {
                    propDetails = BL_PropDetailsTG.GetPropertyDetails(dbRequestParamModel.iVendorId, roomIds);
                }

                propDetails.ServiceChargeTG = ofrServiceCharge;
              
                propDetails.cStatus = "A";
                propDetails.iVendorId = Id;

                propDetails.sRoomData = sRoomData;
                propDetails.scheckIn = cIn;
                propDetails.scheckOut = cOut;
                propDetails.dtCheckIn = dbRequestParamModel.dtCheckIn;
                propDetails.dtCheckOut = dbRequestParamModel.dtCheckOut;
                hotelDetailsFromApi.RoomAmenities = propDetails.TG_Hotel.RoomAmenities;
                propDetails.TG_Hotel = hotelDetailsFromApi;
                propDetails.sRequestType = sRequestType;
                propDetails.Currency = dbRequestParamModel.sCurrency;

                if (propDetails.TG_Hotel != null && propDetails.TG_Hotel.RoomAmenities != null)
                {
                    foreach (var roomId in roomIdsArr)
                    {
                        var tgRoom = hotelDetailsFromApi.RoomDetails.Where(x => x.RoomId == roomId).FirstOrDefault();
                        tgRoom.RoomAmenities = propDetails.TG_Hotel.RoomAmenities.Where(x => x.iRoomTypeId == tgRoom.RoomId).ToList();
                    }
                }

                Task.Run(() => BL_PropDetails.UpdateRecentViewAsync(propDetails.iPropId, User.Identity.GetUserId<long>()));

                ViewBag.dExchangeRate = 1;

                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",CurrencyCode);
                    if (objExchange.dRate.HasValue)
                    {
                        ViewBag.dExchangeRate = objExchange.dRate.Value;
                    }
                }

                propDetails.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);

            }
            return View(propDetails);
        }


        public ActionResult RoomInformation(string sVendorId, string sRoomTypeId)
        {
            var propDetail = TempData["propDetailsTG"] as PropDetailsM;

            RoomDetails objRoomDetails = BL_PropDetailsTG.GetRoomDetails(sVendorId, sRoomTypeId);

            if (objRoomDetails.Policy == null)
            {
                objRoomDetails.Policy = new List<RoomPolicy>();
            }

            if (propDetail != null)
            {
                var roomDetail = propDetail.TG_Hotel.RoomDetails.Where(x => x.RoomId == sRoomTypeId).FirstOrDefault();
                if (roomDetail != null)
                {
                    if (roomDetail.RoomImages != null && roomDetail.RoomImages.Count > 0)
                    {
                        objRoomDetails.Images = new List<RoomImages>();
                        objRoomDetails.ThumbImages = new List<RoomImages>();

                        foreach (var item in roomDetail.RoomImages)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                objRoomDetails.ThumbImages.Add(new RoomImages() { sRoomImage = item });
                                objRoomDetails.Images.Add(new RoomImages() { sRoomImage = item });


                                var imageName = Path.GetFileName(item);

                                //if (imageName.Contains("_TN"))
                                //{
                                //    objRoomDetails.ThumbImages.Add(new RoomImages() { sRoomImage = item });
                                //}
                                //else
                                //{
                                //    objRoomDetails.Images.Add(new RoomImages() { sRoomImage = item });
                                //}
                            }
                        }
                    }
                    var ratePlans = roomDetail.RatePlans;
                    objRoomDetails.sRoomName = roomDetail.RoomName;
                    objRoomDetails.ExtrabedCharge = ratePlans.FirstOrDefault().TotalExtraBedCharge.ToString();
                    foreach (var item in roomDetail.RatePlans)
                    {
                        objRoomDetails.RateInclusions += item.RateInclusions + Environment.NewLine;
                        objRoomDetails.CancellationPolicy += item.CancellationPolicyDescription + Environment.NewLine;


                        // TO DO
                        //var roomPolicy = new RoomPolicy();
                        //roomPolicy.sExtraCharge = item.TotalExtraBedCharge.ToString();
                        //roomPolicy.iMax_Guest_Occupancy = short.Parse(roomDetail.MaxOccupancy);
                        //roomPolicy.iMax_Adult_Occupancy = short.Parse(roomDetail.MaxAdult);
                        //roomPolicy.iMax_Child_Occupancy = short.Parse(roomDetail.MaxChild);
                        //roomPolicy.sAge = roomDetail.MinChildAge;
                        //objRoomDetails.Policy.Add(roomPolicy);
                    }
                }
            }
            else
            {
                objRoomDetails.ThumbImages = objRoomDetails.Images;
            }

            return PartialView("pvRoomInfo", objRoomDetails);
        }


        [HttpGet]
        public ActionResult GetRoomData(string sVendorId, string dCheckIn, string dCheckOut, string sRoomData)
        {
            try
            {

                DateTime checkIn = DateTime.ParseExact(dCheckIn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime checkOut = DateTime.ParseExact(dCheckOut, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var propDetail = new PropDetailsM();

                propDetail.dtCheckIn = checkIn;
                propDetail.dtCheckOut = checkOut;
                propDetail.sRoomData = sRoomData;
                propDetail.iVendorId = sVendorId;
                propDetail.scheckIn = dCheckIn;
                propDetail.scheckOut = dCheckOut;

                #region RoomOccupancySearch

                DataTable dtTblRoomOccupancySearch = new DataTable();
                dtTblRoomOccupancySearch.Columns.AddRange(new DataColumn[3] 
                { 
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                });

                DataTable dtTblChildrenAgeSearch = new DataTable();
                dtTblChildrenAgeSearch.Columns.AddRange(new DataColumn[2] 
                { 
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))
                });

                var roomDataResult = new List<RoomData>();

                if (!string.IsNullOrEmpty(sRoomData))
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);

                    foreach (var room in roomDataResult)
                    {
                        DataRow roomOccupancy = dtTblRoomOccupancySearch.NewRow();
                        roomOccupancy["ID"] = room.room;
                        roomOccupancy["iAdults"] = room.adult;
                        roomOccupancy["children"] = room.child;
                        dtTblRoomOccupancySearch.Rows.Add(roomOccupancy);

                        foreach (var child in room.ChildAge)
                        {
                            DataRow dtChildrenAge = dtTblChildrenAgeSearch.NewRow();
                            dtChildrenAge["ID"] = room.room;
                            if (child.Age == "<1")
                            {
                                dtChildrenAge["Age"] = "0";
                                child.Age = "0";
                            }
                            else
                            {
                                dtChildrenAge["Age"] = child.Age;
                            }
                            dtChildrenAge["Age"] = child.Age == "<1" ? "0" : child.Age;

                            dtTblChildrenAgeSearch.Rows.Add(dtChildrenAge);
                        }
                    }
                }

                #endregion

                #region Hotel Detais

                var ofrCommision = BL_Bidding.GetBidMaster(CurrencyCode);

                var TG_Hotel_With_Rooms = clsSearchHotel.FetchHotelsDetailsByVendorId(
                    true,
                    sVendorId, checkIn.ToString("yyyy-MM-dd"),
                    checkOut.ToString("yyyy-MM-dd"),
                    dtTblRoomOccupancySearch,
                    dtTblChildrenAgeSearch,
                    roomDataResult
                    );

                propDetail.dOFRServiceCharge = ofrCommision.dOFRServiceCharge;

                string roomIds = string.Empty;
                List<string> roomIdsArr = new List<string>();

                if (TG_Hotel_With_Rooms != null)
                {
                    roomIdsArr = TG_Hotel_With_Rooms.RoomDetails.Select(x => x.RoomId).ToList();
                    roomIds = string.Join(",", roomIdsArr);
                }
                var db_roomAmenities = BL_PropDetailsTG.GetRoomAmenities(sVendorId, roomIds);

                if (db_roomAmenities.Count > 0)
                {
                    foreach (var roomId in roomIdsArr)
                    {
                        var tgRoom = TG_Hotel_With_Rooms.RoomDetails.Where(x => x.RoomId == roomId).FirstOrDefault();
                        tgRoom.RoomAmenities = db_roomAmenities.Distinct().ToList();
                    }
                }

                propDetail.TG_Hotel = TG_Hotel_With_Rooms;

                ViewBag.dExchangeRate = 1;

                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                    if (objExchange.dRate.HasValue)
                    {
                        ViewBag.dExchangeRate = objExchange.dRate.Value;
                    }
                }
                propDetail.Symbol = BL_ExchangeRate.GetSymbolByCurrencyCode(CurrencyCode);

               
                #endregion

                return PartialView("pvHotelRooms", propDetail);
            }
            catch (Exception)
            {
                return Content("An Error Occured !");
            }
        }
    }
}