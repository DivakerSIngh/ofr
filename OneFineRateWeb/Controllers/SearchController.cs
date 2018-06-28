using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Device.Location;
using System.Data;
using OneFineRateWeb.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace OneFineRateWeb.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SearchController : BaseController
    {
       // [OutputCache(Duration = 10, VaryByParam = "*")] // Cached for 10 second
        public ActionResult Index(PropSearchRequestModel model)
        {
            try
            {
              
                eWebSiteSearchPage obj = new eWebSiteSearchPage();
                obj.HotelFacilityItems = BL_WebSiteSearchPage.GetAllHotelFacilities("");
                obj.RoomFacilityItems = BL_WebSiteSearchPage.GetAllRoomFacilities("");
                model.sRequestType = model.sRequestType ?? "buy";
                obj.sRequestType = model.sRequestType;
                string currencySymbol = string.Empty;

                var hotelList = GetHotelSearchData(model, out currencySymbol);
                obj.PropertySearchedList = hotelList;
                obj.TotalPropertySearchedList.AddRange(obj.PropertySearchedList);
                obj.sCurrencySymbol = currencySymbol;
                obj.dExchangeRate = 1;
                Session["PropSearchList"] = obj;

                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                    if (objExchange.dRate.HasValue)
                    {
                        obj.dExchangeRate = objExchange.dRate.Value;
                    }
                }
                TempData.Keep();
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetHotelListBYSearchParam(PropSearchRequestModel model)
        {
            string currencySymbol = string.Empty;
            var hotelList = GetHotelSearchData(model, out currencySymbol);

            return Json(new { hotelList = hotelList, currencySymbol = currencySymbol }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetHotelListPartial(PropSearchRequestModel model)
        {
            string currencySymbol = string.Empty;
            var hotelList = GetHotelSearchData(model, out currencySymbol);
            eWebSiteSearchPage obj = new eWebSiteSearchPage();
            obj.sRequestType = model.sRequestType;
            obj.sCurrencySymbol = currencySymbol;
            obj.PropertySearchedList = hotelList;
            obj.TotalPropertySearchedList.AddRange(obj.PropertySearchedList);
            Session["PropSearchList"] = obj;
            return PartialView("_HotelList", obj);
        }


        [HttpGet]
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "User")] // Cached for 60 second
        public PartialViewResult GetTGHotelListPartial(PropSearchRequestModel model)
        {
            eWebSiteSearchPage obj = new eWebSiteSearchPage();

            try
            {
                decimal dExchangeRate = 1;

                if (Session["PropSearchList"] != null)
                {
                    obj = Session["PropSearchList"] as eWebSiteSearchPage;
                }
                if (model.sRequestType == "buy")
                {
                    var TG_hotelList = Get_TG_HotelSearchData(model, out dExchangeRate);

                    obj.dExchangeRate = dExchangeRate;
                    obj.TGPropertySearchedList = TG_hotelList;
                    obj.TotalPropertySearchedList.AddRange(obj.TGPropertySearchedList);

                }
                Session["PropSearchRequestModel"] = model;
                Session["PropSearchList"] = obj;
            }
            catch (Exception)
            {
                // throw;
            }

            return PartialView("_TGHotelList1", obj);
        }


        public PartialViewResult GetMapViewPartial()
        {
            eWebSiteSearchPage obj = Session["PropSearchList"] as eWebSiteSearchPage;

            //var data = new List<eDropDown>();

            //if (obj != null && obj.TotalPropertySearchedList.Count > 0)
            //{
            //    data = obj.TotalPropertySearchedList.Select(x => new eDropDown { Id = x.iPropId, Name = x.sHotelName }).ToList();
            //}

            //Session["PropNameSearchList"] = data;

            return PartialView("_MapView", obj);
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        public JsonResult GetHotelNames(string txt, string placeId, string type, string sRequestType)
        {
            int itype = 0;
            int iReqType = 0;
            switch (type)
            {
                case "City":
                    itype = 3;
                    break;
                case "Area":
                    itype = 4;
                    break;
                case "Locality":
                    itype = 5;
                    break;
            }

            switch (sRequestType)
            {
                case "buy":
                    iReqType = 0;
                    break;
                case "negotiate":
                    iReqType = 1;
                    break;
            }

            //List<eDropDown> data = new List<eDropDown>();

            List<eDropDown> data = BL_WebSiteSearchPage.GetAllHotels(txt, Convert.ToInt32(placeId), itype, iReqType);

            //var propertySearchedList = Session["PropNameSearchList"] as List<eDropDown>;

            //if (propertySearchedList != null && propertySearchedList.Count > 0)
            //{
            //    data = propertySearchedList.Where(x => x.Name.ToLower().Contains(txt.ToLower())).ToList();
            //}

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //[OutputCache(Duration = 100, VaryByParam = "*")]
        public JsonResult GetLocalityNames(string txt, string PlaceId, string Type, string sRequestType)
        {
            int itype = 0;
            int iReqType = 0;
            switch (Type)
            {
                case "City":
                    itype = 3;
                    break;
                case "Area":
                    itype = 4;
                    break;
                case "Locality":
                    itype = 5;
                    break;

            }

            switch (sRequestType)
            {
                case "buy":
                    iReqType = 0;
                    break;
                case "negotiate":
                    iReqType = 1;
                    break;
            }
            List<eDropDown> data = BL_WebSiteSearchPage.GetAllHotelsLocality(txt, Convert.ToInt32(PlaceId), itype, iReqType);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ToggleFavorite(string iPropId)
        {
            int retrn = BL_WebSiteSearchPage.ToggleFavs(iPropId, User.Identity.GetUserId<long>().ToString());

            if (retrn == 1)
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);

        }

       
        [NonAction]
        
        private List<PropSearchResponseModel> GetHotelSearchData(PropSearchRequestModel model, out string currencySymbol)
        {
            var hotelSearchData = new List<PropSearchResponseModel>();
            var hotelSearchDataTG = new List<PropSearchResponseModel>();
            var hotelSearchReturnTG = new List<PropSearchResponseModel>();
            currencySymbol = string.Empty;
            if (model != null)
            {
                var roomDataResult = new List<RoomData>();
                if (model.sRoomData != null)
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(model.sRoomData);
                }

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

                var dbRequestParamModel = model.GetThisWithRoomAndHotelFailities();

                dbRequestParamModel.dtCheckIn = DateTime.Parse(model.sCheckIn ?? DateTime.Now.ToShortDateString());
                dbRequestParamModel.dtCheckOut = DateTime.Parse(model.sCheckOut ?? DateTime.Now.ToShortDateString());

                if (!string.IsNullOrEmpty(model.sHotelSearch))
                {
                    model.iAreaType = 0;

                }
                else if (model.ctype == "City")
                {
                    model.iAreaType = 3;
                }
                else if (model.ctype == "Area")
                {
                    model.iAreaType = 4;
                }
                else if (model.ctype == "Locality")
                {
                    model.iAreaType = 5;
                }

                model.bLogin = User.Identity.IsAuthenticated;
                model.iUserId = User.Identity.GetUserId<long>();
                //model.bLogin = true;

                string sCurrencySymbol;
                hotelSearchData = BL_WebSiteSearchPage.GetHotelsBySearchQ(dbRequestParamModel, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, CurrencyCode, out sCurrencySymbol);
                //if (model.sRequestType.ToLower() == "buy" && model.isSpecialDeal == false)
                //{
                //    hotelSearchDataTG = BL_WebSiteSearchPage.GetHotelsForTG(dbRequestParamModel, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, CurrencyCode, out sExchangeRate);
                //    if (hotelSearchDataTG.Count > 0)
                //    {
                //        hotelSearchReturnTG = clsSearchHotel.FetchMultiVendorOfDB(hotelSearchDataTG, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, dbRequestParamModel.dtCheckIn.ToString("yyyy-MM-dd"), dbRequestParamModel.dtCheckOut.ToString("yyyy-MM-dd"), sExchangeRate, model.dMinPrice, model.dMaxPrice);
                //        if (hotelSearchReturnTG.Count > 0)
                //            hotelSearchData.AddRange(hotelSearchReturnTG);
                //    }
                //}
                currencySymbol = sCurrencySymbol;
            }

            return hotelSearchData;
        }

        [NonAction]
        private List<PropSearchResponseModel> Get_TG_HotelSearchData(PropSearchRequestModel model, out decimal dExchangeRate)
        {
            var hotelSearchDataTG = new List<PropSearchResponseModel>();
            decimal sExchangeRate = 1;

            try
            {
                if (model != null)
                {

                    var roomDataResult = new List<RoomData>();
                    if (model.sRoomData != null)
                    {
                        roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(model.sRoomData);
                    }

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

                    var dbRequestParamModel = model.GetThisWithRoomAndHotelFailities();

                    dbRequestParamModel.dtCheckIn = DateTime.Parse(model.sCheckIn ?? DateTime.Now.ToShortDateString());
                    dbRequestParamModel.dtCheckOut = DateTime.Parse(model.sCheckOut ?? DateTime.Now.ToShortDateString());
                    if (!string.IsNullOrEmpty(model.sHotelSearch))
                    {
                        model.iAreaType = 0;

                    }
                    else if (model.ctype == "City")
                    {
                        model.iAreaType = 3;
                    }
                    else if (model.ctype == "Area")
                    {
                        model.iAreaType = 4;
                    }
                    else if (model.ctype == "Locality")
                    {
                        model.iAreaType = 5;
                    }

                    model.bLogin = User.Identity.IsAuthenticated;
                    model.iUserId = User.Identity.GetUserId<long>();

                    if (model.sRequestType.ToLower() == "buy" && model.isSpecialDeal == false)
                    {
                        var responseDB_DataTG = BL_WebSiteSearchPage.GetHotelsForTG(dbRequestParamModel, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, CurrencyCode, out sExchangeRate);
                        if (responseDB_DataTG.Count > 0)
                        {
                            //To get the real time data from the travel guru 
                            var api_responseTG_Data = clsSearchHotel.FetchMultiVendorOfDB(responseDB_DataTG, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, dbRequestParamModel.dtCheckIn.ToString("yyyy-MM-dd"), dbRequestParamModel.dtCheckOut.ToString("yyyy-MM-dd"), sExchangeRate, model.dMinPrice, model.dMaxPrice);
                            if (api_responseTG_Data.Count > 0)
                                hotelSearchDataTG.AddRange(api_responseTG_Data);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            dExchangeRate = sExchangeRate;

            return hotelSearchDataTG;
        }


        public ActionResult UpdateConversionViews(string Id, string type)
        {
            string PropId = OneFineRateAppUtil.clsUtils.Decode(Id.ToString());
            //Insert data in view conversion table
            DataTable dtPropIds = new DataTable();
            dtPropIds.Columns.AddRange(new DataColumn[1] 
                { 
                        new DataColumn("Id", typeof(int))
                });
            DataRow drpropid = dtPropIds.NewRow();
            drpropid["Id"] = Convert.ToInt32(PropId);
            dtPropIds.Rows.Add(drpropid);


            Task.Run(() => BL_PropDetails.UpdateViewForConversion(type, dtPropIds));
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }

}