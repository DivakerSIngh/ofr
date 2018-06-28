using Newtonsoft.Json;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using OneFineRateWeb.App_Helper;
using FutureSoft.Util;
using Encription;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Globalization;
using OneFineRateWeb.Handlers;


namespace OneFineRateWeb.Controllers.Corporate
{
    [Authorize]
    [CorporateAuthrization]
    public class CorporateController : BaseController
    {
        //[HttpGet]
        public ActionResult Preview(string info, string type,string propDetailsTempData)
        {
            var propDetails = new PropDetailsM();

            if (TempData["PropDetails"] != null)
            {
             //   TempData.Keep();
                propDetails = TempData["PropDetails"] as PropDetailsM;
                Session[propDetailsTempData] = propDetails;
            }
            else if (!string.IsNullOrEmpty(propDetailsTempData))
            {
                //stronglObject =JsonConvert.DeserializeObject<PropDetailsM>(propdetailguid);
                propDetails = HttpContext.Session[propDetailsTempData] as PropDetailsM;
                if (propDetails == null)
                {
                    return RedirectToActionPermanent("Index", "Home");
                }
            }
            else
            {
                return RedirectToActionPermanent("Index", "Home");
            }

            ViewBag.PrevUrl = Request.UrlReferrer;

            //decimal? negotiationAmount = Convert.ToDecimal(info);

            //ViewBag.NegotiationAmount = negotiationAmount == 0 ? null : negotiationAmount;

            //ViewBag.type = type == string.Empty ? null : type;

            if (User.Identity.IsAuthenticated)
            {
                var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                propDetails.sUserTitle = user.Title;
                propDetails.sUserFirstName = user.FirstName;
                propDetails.sUserLastName = user.LastName;
                if (propDetails.sActionType == "C"&&!string.IsNullOrEmpty(user.CorporateEmail))
                {
                    propDetails.sUserEmail = user.CorporateEmail;
                }
                else
                {
                    propDetails.sUserEmail = user.Email;
                }
                propDetails.sUserMobileNo = user.PhoneNumber;
                propDetails.iStateId = user.StateId == null ? 0 : user.StateId.Value;
            }

            DataTable dtNegotiationRatePlans = new DataTable();
            dtNegotiationRatePlans.Columns.AddRange(new DataColumn[3] { new DataColumn("RatePlan", typeof(int)),
                    new DataColumn("dPrice", typeof(decimal)),
                    new DataColumn("bIsPromo",typeof(bool)) });

            int RatePlan = 0, iNoRoom = 0, iDays = 0, iCount = 0, ExtraBed = 0;
            decimal RoomPrice = 0, DTotal = 0, ExtraCharges = 0;
            bool isPromo = false;

            for (int i = 0; i < propDetails.lstetblRooms.Count; i++)
            {
                for (int j = 0; j < propDetails.lstetblRooms[i].lstRatePlan.Count; j++)
                {
                    for (int lstOcc = 0; lstOcc < propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                    {
                        if (propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                        {
                            for (int k = 0; k < propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                            {
                                ExtraBed = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds;
                                ExtraCharges = propDetails.lstetblRooms[i].ExtraBedCharges;

                                RatePlan = propDetails.lstetblRooms[i].lstRatePlan[j].RPID;
                                RoomPrice = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice;
                                isPromo = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].blsPromo;

                                iNoRoom = propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms;
                                iDays = Convert.ToInt32(propDetails.iTotalDays);
                                iCount = iNoRoom * iDays;

                                RoomPrice = RoomPrice * (iDays);
                                ExtraCharges = (ExtraBed * ExtraCharges) * iDays;

                                DTotal += RoomPrice + ExtraCharges;
                                dtNegotiationRatePlans.Rows.Add(RatePlan, RoomPrice, isPromo);

                                iNoRoom++;
                            }
                        }
                    }
                }
            }

            propDetails.dSummaryTotal = DTotal;
            propDetails.sSummaryTotal = clsUtils.ConvertNumberToCommaSeprated(DTotal);
            propDetails.scheckIn = String.Format("{0:dddd MMM d yyyy}", propDetails.dtCheckIn);
            propDetails.scheckOut = String.Format("{0:dddd MMM d yyyy}", propDetails.dtCheckOut);
            propDetails.iNoOfRooms = iNoRoom;
            Session[propDetailsTempData] = propDetails;
         //   TempData.Keep(propDetailsTempData);
            //propDetails.lstetblPhotoGallery = new List<etblPhotoGallery>();

            //propDetails.lstetblHotelFacilities = new List<etblHotelFacilities>();
            ViewBag.HeaderBarData = "Preview";


            return View("CorporatePreview", propDetails);
        }

        [HttpGet]
        public ActionResult Search(PropSearchRequestModel model)
        {
            try
            {
                eWebSiteSearchPage obj = new eWebSiteSearchPage();
                obj.HotelFacilityItems = BL_WebSiteSearchPage.GetAllHotelFacilities(string.Empty);
                obj.RoomFacilityItems = BL_WebSiteSearchPage.GetAllRoomFacilities(string.Empty);
                string currencySymbol = string.Empty;

                //GSearch all the hotels based on the specific crietra 
                var hotelList = GetHotelSearchData(model, out currencySymbol);
                obj.TotalPropertySearchedList = hotelList;
                obj.sCurrencySymbol = currencySymbol;
                obj.dExchangeRate = 1;
                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                    if (objExchange.dRate.HasValue)
                    {
                        obj.dExchangeRate = objExchange.dRate.Value;
                    }
                }

                var data = new List<eDropDown>();

                if (obj != null && obj.TotalPropertySearchedList.Count > 0)
                {
                    data = obj.TotalPropertySearchedList.Select(x => new eDropDown { Id = x.iPropId, Name = x.sHotelName }).ToList();
                }

                Session["PropSearchRequestModel"] = model;

                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult GetHotelListPartial(PropSearchRequestModel model)
        {
            string currencySymbol = string.Empty;
            var hotelList = GetHotelSearchData(model, out currencySymbol);
            var obj = new eWebSiteSearchPage();
            obj.sRequestType = model.sRequestType;
            obj.sCurrencySymbol = currencySymbol;
            obj.TotalPropertySearchedList = hotelList;

            var data = new List<eDropDown>();

            if (obj != null && obj.TotalPropertySearchedList.Count > 0)
            {
                data = obj.TotalPropertySearchedList.Select(x => new eDropDown { Id = x.iPropId, Name = x.sHotelName }).ToList();
            }

            Session["PropSearchRequestModel"] = model;

            return PartialView("_HotelList", obj);
        }

        public JsonResult GetHotelNames(string txt)
        {
            var requestModel = Session["PropSearchRequestModel"] as PropSearchRequestModel;

            int itype = 0;
            int iReqType = 0;
            switch (requestModel.ctype)
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

            switch (requestModel.sRequestType)
            {
                case "buy":
                    iReqType = 0;
                    break;
                case "negotiate":
                    iReqType = 1;
                    break;
            }


            //List<eDropDown> data = new List<eDropDown>();

            List<eDropDown> data = BL_WebSiteSearchPage.GetAllHotels(txt, requestModel.cid, itype, iReqType);

            //var propertySearchedList = Session["PropNameSearchList"] as List<eDropDown>;

            //if (propertySearchedList != null && propertySearchedList.Count > 0)
            //{
            //    data = propertySearchedList.Where(x => x.Name.ToLower().Contains(txt.ToLower())).ToList();
            //}

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Modify the existing booking 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(string bookingId)
        {
            int bookingId_Decoded = Convert.ToInt32(clsUtils.Decode(bookingId));

            var bookingModel = BL_Booking.GetBookingModifyDetails(bookingId_Decoded);

            var roomDataResult = new List<RoomData>();

            #region FetchRoomData

            var bookingDetailList = BL_Booking.GetBookingDetailList(bookingId_Decoded);

            if (bookingDetailList != null)
            {
                int roomId = 1;

                foreach (var bookingDetail in bookingDetailList)
                {
                    var roomData = new RoomData() { adult = bookingDetail.iAdults.Value, child = bookingDetail.iChildren.Value, room = roomId, iBookingDetailId = bookingDetail.iBookingDetailsID };

                    if (!string.IsNullOrEmpty(bookingDetail.sChildrenAge))
                    {
                        foreach (var childAge in bookingDetail.sChildrenAge.Split(','))
                        {
                            roomData.ChildAge = new List<ChildAge>() { new ChildAge() { Age = childAge } };
                        }
                    }
                    else
                    {
                        roomData.ChildAge = new List<ChildAge>() { };
                    }

                    roomDataResult.Add(roomData);
                    roomId++;
                }

                var searchParams = bookingDetailList.Select(x => new { x.dtCheckIn, x.dtCheckOut }).FirstOrDefault();

                if (searchParams.dtCheckIn < DateTime.Today)
                {
                    bookingModel.BookingStatus = "expire";
                    return View(bookingModel);
                }

                if (searchParams.dtCheckIn == null && searchParams.dtCheckOut == null)
                {
                    bookingModel.sCheckIn = DateTime.Today.ToString("dd/MM/yyyy");
                    bookingModel.sCheckOut = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                }
                else
                {
                    bookingModel.sCheckIn = searchParams.dtCheckIn.Value.ToString("dd/MM/yyyy");
                    bookingModel.sCheckOut = searchParams.dtCheckOut.Value.ToString("dd/MM/yyyy");
                }

                bookingModel.sRoomData = clsUtils.ConvertToJson(roomDataResult);

            }

            #endregion
            var guid = Guid.NewGuid().ToString();
            string tempdataOldRooms = "OldRooms" + guid;
            bookingModel.TempDataOldRooms = tempdataOldRooms;
            Session[tempdataOldRooms] = bookingModel;
           // TempData.Keep(tempdataOldRooms);

            return View(bookingModel);
        }

        /// <summary>
        /// On Selction of a specific hotel when search by Corporate 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HotelDetails(PropSearchRequestModel model)
        {
            try
            {
                var serviceChargeModel = BL_Bidding.GetBidMaster(CurrencyCode);
                Session["BidMaster"] = serviceChargeModel;
                ViewData["dOFRServiceCharge"] = Convert.ToDecimal(serviceChargeModel.dOFRServiceCharge);

                var propDetail = new PropDetailsM() { sRequestType = "buy" };
                var propId = Convert.ToInt32(clsUtils.Decode(model.Id));

                Task.Run(() => BL_PropDetails.UpdateRecentViewAsync(propId, User.Identity.GetUserId<long>()));

                if (model.IsModify)
                {
                    if (Session[model.PropDetailsTempData] != null)
                    {
                        propDetail = Session[model.PropDetailsTempData] as PropDetailsM;

                     //   TempData.Keep();
                    }

                    propDetail.bOccuData = true;
                }

                else if (!string.IsNullOrEmpty(model.sCheckIn) && !string.IsNullOrEmpty(model.sCheckOut) && !string.IsNullOrEmpty(model.sRoomData))
                {
                    #region FetchRoomData and bind in datatables

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

                    #endregion

                    #region OfferReviewTracker

                    var tracker = new etblOfferReviewTrackerTx();
                    tracker.iPropId = propId;
                    tracker.dtActionDate = DateTime.Now;
                    tracker.sIPAddress = OneFineRateAppUtil.clsUtils.getIpAddress();
                    BL_tblOfferReviewTrackerTx.AddRecord(tracker);

                    #endregion

                    #region Other Details

                    propDetail.iPropId = propId;
                    propDetail.dtCheckIn = DateTime.ParseExact(model.sCheckIn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    propDetail.dtCheckOut = DateTime.ParseExact(model.sCheckOut, "dd/MM/yyyy" , CultureInfo.InvariantCulture);
                    propDetail.bLogin = User.Identity.IsAuthenticated;
                    propDetail.Currency = CurrencyCode;
                    propDetail.bIsAirConditioning = false;
                    propDetail.bIsBathtub = false;
                    propDetail.bIsFlatScreenTV = false;
                    propDetail.bIsSoundproof = false;
                    propDetail.bIsView = false;
                    propDetail.bIsInternetFacilities = false;
                    propDetail.bIsPrivatePool = false;
                    propDetail.bIsRoomService = false;
                    propDetail.dMinPrice = 0;
                    propDetail.dMaxPrice = 0;
                    propDetail.SpecialDeal = true;
                    propDetail = BL_PropDetails.GetPropertyDetailsCor(propDetail.iPropId, propDetail, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch);
                    if (propDetail.lstetblRooms.Count == 0)
                    {
                        propDetail.bRoomAvailable = false;
                    }
                    propDetail.sCheckInDate = model.sCheckIn;
                    propDetail.sCheckOutDate = model.sCheckOut;
                    propDetail.dtCheckIn = DateTime.Parse(model.sCheckIn);
                    propDetail.dtCheckOut = DateTime.Parse(model.sCheckOut);
                    propDetail.iTotalDays = (propDetail.dtCheckOut - propDetail.dtCheckIn).TotalDays.ToString();
                    propDetail.Currency = CurrencyCode;
                    propDetail.hdnJsonData = model.sRoomData;
                    ViewData["dOFRServiceCharge"] = (propDetail.TaxCharges.dOFRServiceCharge + propDetail.TaxCharges.TaxOnServiceCharge);
                    string propDetailsName = "propDetails" + Guid.NewGuid() ;
                    propDetail.TempDataPropDetails = propDetailsName;
                    Session[propDetail.TempDataPropDetails] = propDetail;

                    //TempData.Keep(propDetail.TempDataPropDetails);

                    #endregion
                }

                return View(propDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetHotelRooms(string propId, string sCheckIn, string sCheckOut, string sRoomData, string sRequestType, string tempdataOldRooms)
        {
            try
            {
                if (!string.IsNullOrEmpty(propId))
                {
                    PropDetailsM dbRequestParamModel = new PropDetailsM();
                    dbRequestParamModel.iPropId = int.Parse(propId);

                    if (string.IsNullOrEmpty(sCheckIn) && string.IsNullOrEmpty(sCheckOut))
                    {
                        dbRequestParamModel.dtCheckIn = DateTime.Now.Date.AddDays(1);
                        dbRequestParamModel.dtCheckOut = DateTime.Now.Date.AddDays(2);

                        sCheckIn = DateTime.Today.ToString("dd/MM/yyyy");
                        sCheckOut = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        dbRequestParamModel.dtCheckIn = DateTime.Parse(sCheckIn);
                        dbRequestParamModel.dtCheckOut = DateTime.Parse(sCheckOut);
                    }

                    dbRequestParamModel.bLogin = User.Identity.IsAuthenticated;
                    dbRequestParamModel.Currency = CurrencyCode;


                    #region FetchRoomData and bind in datatables

                    DataTable dtTblRoomOccupancySearch = new DataTable();
                    dtTblRoomOccupancySearch.Columns.AddRange(new DataColumn[3] {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                    });

                    DataTable dtTblChildrenAgeSearch = new DataTable();
                    dtTblChildrenAgeSearch.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))
                    });

                    var roomDataResult = new List<RoomData>();

                    if (!string.IsNullOrEmpty(sRoomData))
                    {
                        roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);
                    }
                    else
                    {
                        roomDataResult.Add(new RoomData() { adult = 2, child = 0, room = 1, ChildAge = new List<ChildAge>() { new ChildAge() { Age = "5" } } });

                        sRoomData = clsUtils.ConvertToJson(roomDataResult);
                    }

                    dbRequestParamModel.sRoomData = sRoomData;

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

                    dbRequestParamModel.bIsAirConditioning = false;
                    dbRequestParamModel.bIsBathtub = false;
                    dbRequestParamModel.bIsFlatScreenTV = false;
                    dbRequestParamModel.bIsSoundproof = false;
                    dbRequestParamModel.bIsView = false;
                    dbRequestParamModel.bIsInternetFacilities = false;
                    dbRequestParamModel.bIsPrivatePool = false;
                    dbRequestParamModel.bIsRoomService = false;
                    dbRequestParamModel.dMinPrice = 0;
                    dbRequestParamModel.dMaxPrice = 0;
                    dbRequestParamModel.SpecialDeal = true;

                    #endregion


                    //var ofrServiceChargeModel = BL_Bidding.GetBidMaster(CurrencyCode);
                    //ViewData["dOFRServiceCharge"] = Convert.ToDecimal(ofrServiceChargeModel.dOFRServiceCharge);


                    var property_With_RoomList = BL_PropDetails.GetPropertyDetailsCor(int.Parse(propId), dbRequestParamModel, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch);

                    property_With_RoomList.iTotalDays = (dbRequestParamModel.dtCheckOut - dbRequestParamModel.dtCheckIn).TotalDays.ToString();
                    property_With_RoomList.iPropId = dbRequestParamModel.iPropId;
                    property_With_RoomList.sRequestType = sRequestType;
                    property_With_RoomList.sCheckInDate = sCheckIn;
                    property_With_RoomList.sCheckOutDate = sCheckOut;
                    property_With_RoomList.dtCheckIn = dbRequestParamModel.dtCheckIn;
                    property_With_RoomList.dtCheckOut = dbRequestParamModel.dtCheckOut;

                    ViewData["dOFRServiceCharge"] = (property_With_RoomList.TaxCharges.dOFRServiceCharge + property_With_RoomList.TaxCharges.TaxOnServiceCharge);
                    var propDetailsTempData = "DbRequestModel" + Guid.NewGuid().ToString();
                    var roomCancellationChargesTempData = "RoomCancellationCharges" + Guid.NewGuid().ToString();
                    property_With_RoomList.TempDataPropDetails = propDetailsTempData;
                    property_With_RoomList.TempDataOldRooms = tempdataOldRooms;
                    property_With_RoomList.TempDataRoomCancellationCharges = roomCancellationChargesTempData;
                    Session[propDetailsTempData] = dbRequestParamModel;
                    return PartialView("_AmendmentsHotelRooms", property_With_RoomList);
                }

                return PartialView("Error");
            }

            catch (Exception)
            {
                return PartialView("Error");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Corporate(PropDetailsM objlist)
        {

            var propDetails = Session[objlist.TempDataPropDetails] as PropDetailsM;

            if (propDetails == null)
            {
                propDetails = new PropDetailsM();
            }

            //Insert data in view conversion table
            DataTable dtPropIds = new DataTable();
            dtPropIds.Columns.AddRange(new DataColumn[1]
                {
                        new DataColumn("Id", typeof(int))
                });
            DataRow drpropid = dtPropIds.NewRow();
            drpropid["Id"] = Convert.ToInt32(propDetails.iPropId);
            dtPropIds.Rows.Add(drpropid);


            //    Task.Run(() => BL_PropDetails.UpdateViewForConversion(objlist.sActionType, dtPropIds));

            propDetails.dSummaryRoomRate = Convert.ToDecimal(objlist.sSummaryRoomRate.Replace(",", ""));
            propDetails.dSummaryExtraBedCharges = Convert.ToDecimal(objlist.sSummaryExtraBedCharges.Replace(",", ""));
            propDetails.dSummaryTaxes = Convert.ToDecimal(objlist.sSummaryTaxes.Replace(",", ""));
            propDetails.dSummaryGrandTotal = Convert.ToDecimal(objlist.sSummaryGrandTotal.Replace(",", ""));



            propDetails.sSummaryRoomRate = objlist.sSummaryRoomRate;
            propDetails.sSummaryExtraBedCharges = objlist.sSummaryExtraBedCharges;
            propDetails.sSummaryTaxes = objlist.sSummaryTaxes;
            propDetails.sSummaryGrandTotal = objlist.sSummaryGrandTotal;

            propDetails.sSummaryRoomRate_display = objlist.sSummaryRoomRate_display;
            propDetails.sSummaryExtraBedCharges_display = objlist.sSummaryExtraBedCharges_display;
            propDetails.sSummaryTaxes_display = objlist.sSummaryTaxes_display;
            propDetails.sSummaryGrandTotal_display = objlist.sSummaryGrandTotal_display;

            propDetails.lstOccData = objlist.lstOccData;
            propDetails.sOccuDataModified = objlist.sOccuDataModified;


            for (int i = 0; i < propDetails.lstetblRooms.Count; i++)
            {
                for (int j = 0; j < propDetails.lstetblRooms[i].lstRatePlan.Count; j++)
                {
                    for (int lstOcc = 0; lstOcc < propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                    {
                        propDetails.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms = objlist.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms;
                    }
                }
            }
            propDetails.sActionType = objlist.sActionType;

            if (User.Identity.IsAuthenticated)
            {
                TempData["PropDetails"] = propDetails;
               
                //var redirectResult = RedirectToAction("BookerInformation", "Negotiation");
                var redirectResult = RedirectToAction("Preview", new { type = "C", propDetailsTempData = objlist.TempDataPropDetails });
                return redirectResult;
            }
            else
            {

                TempData["PropDetails"] = propDetails;
                var redirectResult = RedirectToAction("Preview", new { propDetailsTempData = objlist.TempDataPropDetails });
                return redirectResult;
            }
        }

        public ActionResult ChargeCard(decimal NegotiationAmount, string Title, string FirstName, string LastName, string EmailID, string sCountryCode, string Mobile, string iStateId, string VC, string OTPCode, decimal GrandTotal, string promocode, bool promoapplied, decimal promovalue, string promotype, string promoamenity, string promoDescription, decimal dtax, string propDetailsTempData, decimal OfrServiceCharge = 0, decimal GstOfrServiceCharge = 0, string dGSTServiceType = "", string dGSTValue = "")
        {

            var stronglObject = new PropDetailsM();
            try
            {
                Decimal? ExchangeRate = 1;
                if (Session[propDetailsTempData] != null)
                {
                    //TempData.Keep(propDetailsTempData);
                    stronglObject = Session[propDetailsTempData] as PropDetailsM;
                }

                int result = 0;
                stronglObject.dNegotiationAmt = NegotiationAmount;
                stronglObject.dSummaryGrandTotal = GrandTotal;

                stronglObject.objBooking.sExtra2 = Convert.ToString(dtax);
                stronglObject.objBooking.sCountryPhoneCode = sCountryCode;

                if (Session["CompanyId"] != null)
                {
                    stronglObject.objBooking.iCompanyId = Convert.ToInt32(Session["CompanyId"]);
                }

                if (promoapplied)
                {
                    if (promotype == "Value")
                    {
                        stronglObject.objBooking.PromoCodeValue = promovalue;
                        stronglObject.objBooking.bPromoDiscount = true;
                    }
                    else
                    {
                        stronglObject.objBooking.bPromoDiscount = false;
                        stronglObject.objBooking.bPromoAmenity = promoamenity;
                    }
                    stronglObject.objBooking.spromotype = promotype;
                    stronglObject.objBooking.sPromoCode = promocode;
                    stronglObject.objBooking.PromoCodeApplied = true;
                    stronglObject.objBooking.sExtra1 = promoDescription;
                }
                else
                {
                    stronglObject.objBooking.PromoCodeApplied = false;
                }
                if (stronglObject.Currency != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById( "INR", stronglObject.Currency);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                    stronglObject.sUserTitle = user.Title;
                    stronglObject.sUserFirstName = FirstName;
                    stronglObject.sUserLastName = LastName;
                    stronglObject.sUserEmail = EmailID;
                    stronglObject.iUserId = user.Id;
                    stronglObject.iStateId = user.StateId.HasValue ? user.StateId.Value : Convert.ToInt32(iStateId);
                    if (OfrServiceCharge != 0)
                        stronglObject.objBooking.dServiceCharge = OfrServiceCharge * (ExchangeRate == null ? 1 : ExchangeRate.Value);
                    if (GstOfrServiceCharge != 0)
                        stronglObject.objBooking.dGSTOnServiceCharge = GstOfrServiceCharge * (ExchangeRate == null ? 1 : ExchangeRate.Value);
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        stronglObject.objBooking.dGSTServiceType = dGSTServiceType;
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        stronglObject.objBooking.dGSTValue = dGSTValue;
                    if (user.PhoneNumber == Mobile)
                    {
                        result = GetBooking(stronglObject);
                        Task.Run(() => BL_WebsiteUser.UpdateRecord(user));
                        return Json(new { status = true, reqStatus = "C", result = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (OTPCode != "" && OTPCode != null)
                        {
                            if (DecodeVC(VC) == OTPCode)
                            {
                                stronglObject.sUserMobileNo = Mobile;
                                result = GetBooking(stronglObject);
                                return Json(new { status = true, reqStatus = "C", result = result }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { st = 0, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            string VerificationCode = ResendVerification(Mobile);
                            stronglObject.sVerificationCode = VerificationCode;
                        }
                    }
                }
                else
                {
                    stronglObject.sUserTitle = Title;
                    stronglObject.sUserFirstName = FirstName;
                    stronglObject.sUserLastName = LastName;
                    stronglObject.sUserEmail = EmailID;
                    stronglObject.sUserMobileNo = Mobile;
                    stronglObject.sCountryPhoneCode = sCountryCode;
                    stronglObject.iStateId = Convert.ToInt32(iStateId);
                    if (OfrServiceCharge != 0)
                        stronglObject.objBooking.dServiceCharge = OfrServiceCharge * (ExchangeRate == null ? 1 : Math.Round(ExchangeRate.Value));
                    if (GstOfrServiceCharge != 0)
                        stronglObject.objBooking.dGSTOnServiceCharge = GstOfrServiceCharge * (ExchangeRate == null ? 1 : Math.Round(ExchangeRate.Value));
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        stronglObject.objBooking.dGSTServiceType = dGSTServiceType;
                    if (!string.IsNullOrEmpty(dGSTServiceType))
                        stronglObject.objBooking.dGSTValue = dGSTValue;

                    if (OTPCode != "" && OTPCode != null)
                    {
                        if (DecodeVC(VC) == OTPCode)
                        {
                            int i = BL_NegotiationBooking.AddRecord(stronglObject);
                            if (i > 0)
                            {
                                stronglObject.iGuestId = i;



                                result = GetBooking(stronglObject);
                                return Json(new { status = true, reqStatus = "C", result = result }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { st = 0, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string VerificationCode = ResendVerification(Mobile);
                        if (VerificationCode == "error")
                        {
                            return Json(new { st = "0", msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
                        }

                        stronglObject.sVerificationCode = VerificationCode;
                    }

                }
            }
            catch (Exception)
            {
                return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { st = 1, status = true, VC = stronglObject.sVerificationCode }, JsonRequestBehavior.AllowGet);
        }

        private string DecodeVC(string VerificationCode)
        {
            try
            {
                var decode = clsUtils.Decode(VerificationCode);
                return decode;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
        //Resend Verification Code on Re-Send button Click
        private string ResendVerification(string phone)
        {
            try
            {
                var phoneVerificationCode = clsUtils.GetVerificationCode();
                var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                clsUtils.sendSMS(phone, message);
                return encodedCode;
            }
            catch (Exception)
            {
                return "error";
                throw;
            }
        }

        //Save booking data in Database
        public int GetBooking(PropDetailsM obj)
        {
            int result = 0;
            try
            {
                Decimal? ExchangeRate = 1;
                Decimal? OriginalTotalAmt = 0;
                int? Days = 0;
                etblOriginalBookingPrice objOrgBook = new etblOriginalBookingPrice();
                etblBookingNegotiationTx objNego = new etblBookingNegotiationTx();
                etblBookingTrakerTx objTrck = new etblBookingTrakerTx();
                List<etblBookingGuestMap> lst = new List<etblBookingGuestMap>();
                List<etblBookingDetailsTx> lstBookDetails = new List<etblBookingDetailsTx>();
                List<etblBookingCancellationPolicyMap> lstCancelPolicy = new List<etblBookingCancellationPolicyMap>();
                List<etblBookedDayWiseTaxAmountDetails> lstDayTaxes = new List<etblBookedDayWiseTaxAmountDetails>();
                List<etblBookedDayWiseTaxAmountDetailsAll> lstDayTaxesDateWise = new List<etblBookedDayWiseTaxAmountDetailsAll>();


                //Object created for tblBookingTx
                obj.objBooking.iPropId = obj.iPropId;
                obj.objBooking.iCustomerId = obj.iUserId;
                obj.objBooking.iGuestId = obj.iGuestId;
                if (obj.iUserId == 0)
                {
                    obj.objBooking.iCustomerId = null;
                }
                if (obj.iGuestId == 0)
                {
                    obj.objBooking.iGuestId = null;
                }
                obj.objBooking.dtCheckIn = obj.dtCheckIn;
                obj.objBooking.dtChekOut = obj.dtCheckOut;
                obj.objBooking.dtReservationDate = DateTime.Now;
                obj.objBooking.sTitleOFR = obj.sUserTitle;
                obj.objBooking.sFirstNameOFR = obj.sUserFirstName;
                obj.objBooking.sLastNameOFR = obj.sUserLastName;
                obj.objBooking.sEmailOFR = obj.sUserEmail;
                obj.objBooking.sMobileOFR = obj.sUserMobileNo;
                obj.objBooking.dtActionDate = DateTime.Now;
                obj.objBooking.BookingStatus = "PP";
                obj.objBooking.PaymentStatus = "P";
                obj.objBooking.sExtra4 = "N";
                obj.objBooking.sCurrencyCode = obj.Currency;//Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";
                if (obj.objBooking.sCurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",obj.objBooking.sCurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }

                Days = Convert.ToInt32((Convert.ToDateTime(obj.objBooking.dtChekOut) - Convert.ToDateTime(obj.objBooking.dtCheckIn)).TotalDays);

                obj.objBooking.dTotalAmount = obj.dSummaryRoomRate * ExchangeRate;


                //decimal OFRServiceTax = Convert.ToDecimal(BL_Bidding.GetBidMaster(obj.objBooking.sCurrencyCode).dOFRServiceCharge);
                decimal OFRServiceTax = obj.objBooking.dServiceCharge + obj.objBooking.dGSTOnServiceCharge;

                if (!String.IsNullOrEmpty(obj.objBooking.sExtra2))
                {
                    obj.objBooking.sExtra2 = (Convert.ToDecimal(obj.objBooking.sExtra2) * Convert.ToDecimal(ExchangeRate)).ToString();
                }
                obj.objBooking.dTaxes = (obj.dSummaryTaxes - OFRServiceTax) * ExchangeRate;
                obj.objBooking.dTaxesForHotel = (obj.dSummaryTaxes - OFRServiceTax) * ExchangeRate;
                obj.objBooking.dTotalExtraBedAmount = obj.dSummaryExtraBedCharges * ExchangeRate;

                obj.objBooking.dTaxesOriginal = obj.dSummaryTaxes * ExchangeRate;

                //Get Time Zone for this booking
                string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                obj.objBooking.iCountryOffset = zone;


                if (obj.objBooking.PromoCodeApplied == true)
                {
                    obj.objBooking.dDiscountedBidPrice = obj.objBooking.PromoCodeValue * ExchangeRate;
                }

                decimal Rate = obj.dCommissionRate;
                if (Rate != 0)
                {
                    decimal Comm = (obj.dSummaryRoomRate + obj.dSummaryExtraBedCharges) * Rate / 100;
                    obj.objBooking.dTotalComm = Comm * ExchangeRate;
                    obj.objBooking.dTotalCommOriginal = Comm * ExchangeRate;
                }

                if (obj.sActionType == "C")   //For Negotiation Type of booking
                {
                    obj.objBooking.cBookingType = "C";
                    objNego.dtActionDate = DateTime.Now;
                    objNego.cStatus = "A";

                    // obj.objBooking.dAdvNegotiateAmount = 500;
                    // obj.objBooking.dTotalNegotiateAmount = obj.dNegotiationAmt * ExchangeRate;                    
                    // obj.objBooking.iNegotiateAttempts = 1;
                    // Object created for tblBookingNegotiationTx
                    // objNego.dTotalNegotiateAmount = obj.dNegotiationAmt * ExchangeRate;
                    // objNego.dtNegotiationDate = DateTime.Now;

                }
                else   //For Normal Type of booking
                {
                    obj.objBooking.cBookingType = "R";
                }

                //Object created for tblBookingTrakerTx
                objTrck.BookingStatus = "PP";
                objTrck.dtActionDate = DateTime.Now;

                for (int i = 0; i < obj.lstetblRooms.Count; i++)
                {
                    for (int j = 0; j < obj.lstetblRooms[i].lstRatePlan.Count; j++)
                    {
                        for (int lstOcc = 0; lstOcc < obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy.Count; lstOcc++)
                        {
                            if (obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms != 0)
                            {
                                for (int k = 0; k < obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms; k++)
                                {
                                    int extrabed = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds;
                                    decimal extrabedcharges = Convert.ToDecimal(obj.lstetblRooms[i].ExtraBedCharges) * Convert.ToDecimal(ExchangeRate);
                                    decimal total = extrabedcharges * extrabed;


                                    decimal? dPriceRP = Convert.ToDecimal(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPriceRP) * Convert.ToDecimal(ExchangeRate);
                                    decimal? dBasePrice = Convert.ToDecimal(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dBasePrice) * Convert.ToDecimal(ExchangeRate);
                                    if (dPriceRP > dBasePrice)
                                    {
                                        OriginalTotalAmt += dPriceRP * Days;
                                    }
                                    else
                                    {
                                        OriginalTotalAmt += dBasePrice * Days;
                                    }

                                    lstBookDetails.Add(new etblBookingDetailsTx()
                                    {
                                        iRoomId = Convert.ToString(obj.lstetblRooms[i].iRoomId),
                                        iRPId = Convert.ToString(obj.lstetblRooms[i].lstRatePlan[j].RPID),
                                        // iRooms = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iNoOfRooms),
                                        iRooms = 1,
                                        sRoomName = obj.lstetblRooms[i].sRoomName,
                                        sRPName = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].RatePlan,
                                        iOccupancy = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy),
                                        dRoomRate = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dPrice * ExchangeRate,
                                        dExtraBedRate = total,
                                        sAmenityRatePlan = obj.lstetblRooms[i].lstRatePlan[j].RateInclusion,
                                        iAdults = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iAdults),
                                        iChildren = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iChildrens),
                                        iExtraBeds = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ExtraBeds),
                                        sChildrenAge = obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].ChildrenAge,
                                        dTaxes = (obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes * ExchangeRate) / Days,
                                        dTaxesForHotel = ((obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].dTaxes) * ExchangeRate) / Days,
                                        dtActionDate = DateTime.Now,
                                        iPromoType = Convert.ToInt16(obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iPromoType)
                                    });

                                    //if (obj.objBooking.cBookingType == "N")
                                    //{

                                }
                                var lstTaxes = obj.lstTaxesDateWise_OfferReview.Where(u => u.RPID == obj.lstetblRooms[i].lstRatePlan[j].RPID && u.RoomID == obj.lstetblRooms[i].iRoomId && u.iOccupancy == obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy).ToList();

                                for (int tax = 0; tax < lstTaxes.Count; tax++)
                                {
                                    lstDayTaxes.Add(new etblBookedDayWiseTaxAmountDetails()
                                    {
                                        dtStayDay = Convert.ToDateTime(lstTaxes[tax].dtDate),
                                        dAmount = Convert.ToDecimal(lstTaxes[tax].dBasePrice) * ExchangeRate,
                                        dTaxPerc = Convert.ToDecimal(lstTaxes[tax].TaxPer),
                                        dTaxVal = Convert.ToDecimal(lstTaxes[tax].TaxVal) * ExchangeRate,
                                        RoomID = Convert.ToInt32(lstTaxes[tax].RoomID),
                                        RPID = Convert.ToInt32(lstTaxes[tax].RPID),
                                        iOccupancy = Convert.ToInt32(lstTaxes[tax].iOccupancy),
                                        bIsPromo = Convert.ToBoolean(lstTaxes[tax].bIsPromo)
                                    });
                                }
                                var lstTaxesDateWise = obj.lstTaxesDateWiseAll_OfferReview.Where(u => u.RPID == obj.lstetblRooms[i].lstRatePlan[j].RPID && u.iRoomId == obj.lstetblRooms[i].iRoomId && u.iOccupancy == obj.lstetblRooms[i].lstRatePlan[j].lstetblOccupancy[lstOcc].iOccupancy).ToList();

                                for (int tax = 0; tax < lstTaxesDateWise.Count; tax++)
                                {
                                    lstDayTaxesDateWise.Add(new etblBookedDayWiseTaxAmountDetailsAll()
                                    {
                                        dtStayDay = Convert.ToDateTime(lstTaxesDateWise[tax].dtStay),
                                        dAmount = Convert.ToDecimal(lstTaxesDateWise[tax].dPrice) * ExchangeRate,
                                        dTaxPerc = Convert.ToDecimal(lstTaxesDateWise[tax].MaxTaxPer),
                                        dTaxVal = Convert.ToDecimal(lstTaxesDateWise[tax].MaxTaxVal) * ExchangeRate,
                                        RoomID = Convert.ToInt32(lstTaxesDateWise[tax].iRoomId),
                                        RPID = Convert.ToInt32(lstTaxesDateWise[tax].RPID),
                                        iOccupancy = Convert.ToInt32(lstTaxesDateWise[tax].iOccupancy),
                                        iTaxId = Convert.ToInt32(lstTaxesDateWise[tax].TaxId)
                                    });
                                }
                                //}
                                for (int lstCancellation = 0; lstCancellation < obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy.Count; lstCancellation++)
                                {
                                    DateTime StarDate = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].ValidFrom;
                                    DateTime EndDate = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].ValidTo;
                                    var res = new List<string>();
                                    for (var date = StarDate; date <= EndDate; date = date.AddDays(1))
                                        res.Add(date.ToString());
                                    foreach (var ddate in res)
                                    {
                                        lstCancelPolicy.Add(new etblBookingCancellationPolicyMap()
                                        {
                                            sPolicyName = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].PolicyName,
                                            dtDate = Convert.ToDateTime(ddate),
                                            dtActionDate = DateTime.Now,
                                            iRPId = obj.lstetblRooms[i].lstRatePlan[j].lstCancellationPolcy[lstCancellation].iRPId.ToString()
                                        });
                                    }
                                }

                            }
                        }

                    }
                }

                //Object for total orginal booking amount
                objOrgBook.dOriginalTotalAmount = OriginalTotalAmt;

                result = BL_Booking.AddBooking(obj, objNego, objTrck, lstBookDetails, lst, lstCancelPolicy, lstDayTaxes, objOrgBook,lstDayTaxesDateWise);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        [HttpGet]
        public JsonResult GetCompaniesAutoComplete(string companySearchTxt)
        {
            var companyList = BL_CorporateCompanyM.GetCorporateCompaniesAutoComplete(companySearchTxt);
            return this.Json(companyList, JsonRequestBehavior.AllowGet);
        }

        #region Helper Method

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

                string sCurrencySymbol;
                hotelSearchData = BL_WebSiteSearchPage.GetHotelsBySearchQCorporate(
                    dbRequestParamModel,
                    dtTblRoomOccupancySearch,
                    dtTblChildrenAgeSearch,
                    CurrencyCode,
                    out sCurrencySymbol);

                currencySymbol = sCurrencySymbol;
            }

            return hotelSearchData;
        }

        [HttpGet]
        public PartialViewResult GetPhotoGalleryAndLocation(PropDetailsM propdetails)
        {
            return PartialView("~/Views/OfferReview/pvPhotoGalleryAndLocation.cshtml",propdetails);
        }
        #endregion
    }
}