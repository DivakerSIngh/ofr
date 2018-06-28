using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Routing;

namespace OneFineRateWeb.Controllers.Negotiation
{
    public class OfferReviewController : BaseController
    {

        int HotelId = 0;
        // GET: OfferReview

            /// <summary>
            /// Load the Hoteal page for public view 
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
        public ActionResult Index(PropSearchRequestModel model)
        {
            //Fetch OFR service charge
            // Session["BidMaster"] = BL_Bidding.GetBidMaster(CurrencyCode);
            PropDetailsM objlist = new PropDetailsM();
            string Slocation = ""; string CIn = ""; string COut = ""; string Modify = ""; string sRoomData = "";
            if (HttpContext.Request.Params["Slocation"] != null) { Slocation = HttpContext.Request.Params["Slocation"]; }
            if (HttpContext.Request.Params["CIn"] != null) { CIn = HttpContext.Request.Params["CIn"].ToString(); }
            if (HttpContext.Request.Params["COut"] != null) { COut = HttpContext.Request.Params["COut"].ToString(); }
            if (HttpContext.Request.Params["M"] != null) { Modify = HttpContext.Request.Params["M"]; }
            if (HttpContext.Request.Params["Id"] != null) { HotelId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(HttpContext.Request.Params["Id"].ToString())); }
            if (HttpContext.Request.Params["sRoomData"] != null) { sRoomData = HttpContext.Request.Params["sRoomData"].ToString(); }

            //To make a count to update in the database 
            Task.Run(() => BL_PropDetails.UpdateRecentViewAsync(HotelId, User.Identity.GetUserId<long>()));

            if (Modify == "1")//In this modify user can only chnage the check in check out date with in the hotel 
            {
                if (Session[model.PropDetailsTempData] != null) //Take the data form the View only
                {
                    objlist = Session[model.PropDetailsTempData] as PropDetailsM;
                    ViewData["dOFRServiceCharge"] = (objlist.TaxCharges.dOFRServiceCharge + objlist.TaxCharges.TaxOnServiceCharge);
                   // TempData.Keep();
                }

                objlist.sRequestType = model.sRequestType;
                objlist.bOccuData = true;
                return View(objlist);
            }
            if (CIn != "" && COut != "" && sRoomData != "")
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


                //To Track the request form which ip on which date and the same get saved into DB also
                var tracker = new etblOfferReviewTrackerTx();
                tracker.iPropId = HotelId;
                tracker.dtActionDate = DateTime.Now;
                tracker.sIPAddress = OneFineRateAppUtil.clsUtils.getIpAddress(); //Get IP Address using "HTTP_X_FORWARDED_FOR" Server Variables

                //Call to Business logic to add record 
                BL_tblOfferReviewTrackerTx.AddRecord(tracker);

                #endregion

                // To get all room of the selected hotel which is full fill the occupancy and the other criteria
                objlist.iPropId = HotelId;
                objlist.dtCheckIn = DateTime.Parse(CIn);
                objlist.dtCheckOut = DateTime.Parse(COut);
                objlist.bLogin = User.Identity.IsAuthenticated;
                objlist.Currency = CurrencyCode;
                objlist.bIsAirConditioning = false;
                objlist.bIsBathtub = false;
                objlist.bIsFlatScreenTV = false;
                objlist.bIsSoundproof = false;
                objlist.bIsView = false;
                objlist.bIsInternetFacilities = false;
                objlist.bIsPrivatePool = false;
                objlist.bIsRoomService = false;
                objlist.dMinPrice = 0;
                objlist.dMaxPrice = 0;
                objlist.SpecialDeal = true;

                //Bussiness logic call to search tha available room 
                objlist = BL_PropDetails.GetPropertyDetails(objlist.iPropId, objlist, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch);

                //When no room available
                if (objlist.lstetblRooms.Count == 0)
                {
                    objlist.bRoomAvailable = false;
                }
                objlist.sCheckInDate = CIn;
                objlist.sCheckOutDate = COut;
                objlist.dtCheckIn = DateTime.Parse(CIn);
                objlist.dtCheckOut = DateTime.Parse(COut);
                objlist.iTotalDays = (objlist.dtCheckOut - objlist.dtCheckIn).TotalDays.ToString();
                objlist.Currency = CurrencyCode;
                objlist.hdnJsonData = sRoomData;
                objlist.sRequestType = model.sRequestType;

                if(objlist.TaxCharges != null)
                ViewData["dOFRServiceCharge"] = (objlist.TaxCharges.dOFRServiceCharge + objlist.TaxCharges.TaxOnServiceCharge);
                string propDetailsName = "propDetails" + Guid.NewGuid().ToString();
                objlist.TempDataPropDetails = propDetailsName;
                Session[propDetailsName] = objlist;
                //s.Keep(propDetailsName);
               
                return View(objlist);


            }

            if (HotelId != 0)
            {
                PropDetailsM objSearch = new PropDetailsM();
                objSearch.iPropId = HotelId;
                objSearch.dtCheckIn = DateTime.Now.Date.AddDays(5);
                objSearch.dtCheckOut = DateTime.Now.Date.AddDays(6);
                objSearch.bLogin = User.Identity.IsAuthenticated;
                objSearch.Currency = CurrencyCode;
                objSearch.bIsAirConditioning = false;
                objSearch.bIsBathtub = false;
                objSearch.bIsFlatScreenTV = false;
                objSearch.bIsSoundproof = false;
                objSearch.bIsView = false;
                objSearch.bIsInternetFacilities = false;
                objSearch.bIsPrivatePool = false;
                objSearch.bIsRoomService = false;
                objSearch.dMinPrice = 0;
                objSearch.dMaxPrice = 0;
                objSearch.SpecialDeal = true;

                #region RoomOccupancySearch

                DataTable dtRoomOccupancySearch = new DataTable();
                dtRoomOccupancySearch.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("iAdults", typeof(short)),
                    new DataColumn("children",typeof(short)) });


                DataTable dtChildrenAgeSearch = new DataTable();
                dtChildrenAgeSearch.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("ID", typeof(int)),
                    new DataColumn("Age", typeof(short))});

                #endregion

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
                }
                else
                {
                    roomDataResult = new List<RoomData> { new RoomData { adult = 2, room = 1, child = 0, ChildAge = new List<ChildAge>() } };
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

                #endregion


                #region OfferReviewTracker

                var tracker = new etblOfferReviewTrackerTx();
                tracker.iPropId = HotelId;
                tracker.dtActionDate = DateTime.Now;
                tracker.sIPAddress = OneFineRateAppUtil.clsUtils.getIpAddress();
                BL_tblOfferReviewTrackerTx.AddRecord(tracker);

                #endregion

                objlist = BL_PropDetails.GetPropertyDetails(HotelId, objSearch, dtRoomOccupancySearch, dtChildrenAgeSearch);
                objlist.sRequestType = model.sRequestType;
                objlist.bRoomAvailable = true;
                objlist.sRequestType = model.sRequestType;

                if(objlist.TaxCharges != null)
                {
                    ViewData["dOFRServiceCharge"] = (objlist.TaxCharges.dOFRServiceCharge + objlist.TaxCharges.TaxOnServiceCharge);
                }
                string propDetailsName = "propDetails" + Guid.NewGuid().ToString();
                objlist.TempDataPropDetails = propDetailsName;
                TempData[propDetailsName] = objlist;
                TempData.Keep(propDetailsName);
            }
            return View(objlist);
        }

        // GET: Room Details
        public ActionResult RoomInfo(string propid, string roomid)
        {
            TempData.Keep();
            RoomDetails objRoomDetails = BL_PropDetails.GetRoomDetails(Convert.ToInt32(propid), Convert.ToInt32(roomid), CurrencyCode);
            return PartialView("pvRoomInfo", objRoomDetails);
        }

        /// <summary>
        /// Final Room booking before payment once user click on the book and Bargain
        /// </summary>
        /// <param name="objlist"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Negotiation(PropDetailsM objlist)
        {
            //var propDetails = JsonConvert.DeserializeObject<PropDetailsM>(propObj);
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
          // var propdetailguid = "PropDetails" + Guid.NewGuid().ToString();
            if (User.Identity.IsAuthenticated)
            {
                // TempData[objlist.TempDataPropDetails] = propDetails;
                //var redirectResult = RedirectToAction("BookerInformation", "Negotiation");
                TempData["PropDetails"] = propDetails;
                
                return RedirectToAction("Index", "Negotiation", new {type = "O",info="", propdetailguid= objlist.TempDataPropDetails });
            }
            else
            {

                 TempData["PropDetails"] = propDetails;
               
                return RedirectToAction("Index", "Negotiation",new { type="test",info = "", propdetailguid = objlist.TempDataPropDetails });
                
            }
        }

        public ActionResult Information()
        {
            BookingInformation objBookingInformation = new BookingInformation();
            return View(objBookingInformation);
        }

        [HttpPost]
        public ActionResult BookingInformation(BookingInformation objBookingInformation)
        {
            return View("Information");
        }

        //public ActionResult GetRoomData(string din, string dout, string d)
        //{

        //    PropDetailsM objlist = new PropDetailsM();
        //    if (HttpContext.Request.Params["Id"] != null) { HotelId = Convert.ToInt32(OneFineRateAppUtil.clsUtils.Decode(HttpContext.Request.Params["Id"].ToString())); }
        //    if (TempData["propDetails"] != null) { TempData.Keep(); objlist = TempData["propDetails"] as PropDetailsM; }

        //    int roomno = 0;
        //    int ID = 0;
        //    short iAdults = 0;
        //    short children = 0;
        //    DataTable dtRoomOccupancySearch = new DataTable();
        //    dtRoomOccupancySearch.Columns.AddRange(new DataColumn[3] { new DataColumn("ID", typeof(int)),
        //            new DataColumn("iAdults", typeof(short)),
        //            new DataColumn("children",typeof(short)) });

        //    short Age = 0;
        //    DataTable dtChildrenAgeSearch = new DataTable();
        //    dtChildrenAgeSearch.Columns.AddRange(new DataColumn[2] { new DataColumn("ID", typeof(int)),
        //            new DataColumn("Age", typeof(short))});

        //    PropDetailsM objSearch = new PropDetailsM();
        //    objSearch.iPropId = HotelId == 0 ? objlist.iPropId : HotelId;
        //    objSearch.dtCheckIn = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(din);
        //    objSearch.dtCheckOut = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(dout);
        //    objSearch.bLogin = true;
        //    objSearch.Currency = Session["CurrencyCode"].ToString();
        //    objSearch.bIsAirConditioning = false;
        //    objSearch.bIsBathtub = false;
        //    objSearch.bIsFlatScreenTV = false;
        //    objSearch.bIsSoundproof = false;
        //    objSearch.bIsView = false;
        //    objSearch.bIsInternetFacilities = false;
        //    objSearch.bIsPrivatePool = false;
        //    objSearch.bIsRoomService = false;
        //    objSearch.dMinPrice = 0;
        //    objSearch.dMaxPrice = 0;
        //    objSearch.SpecialDeal = true;


        //    if (d != null)
        //    {
        //        JArray jArray = (JArray)JsonConvert.DeserializeObject(d.Replace("\\", "\""));
        //        if (jArray != null)
        //        {


        //            //List<etblPropertyParkingMap> lstPropertyParkingMap = new List<etblPropertyParkingMap>();
        //            foreach (var item in jArray)
        //            {

        //                roomno++;
        //                ID = roomno;
        //                iAdults = Convert.ToInt16(item["adult"]);
        //                children = Convert.ToInt16(item["child"]);
        //                dtRoomOccupancySearch.Rows.Add(ID, iAdults, children);

        //                foreach (var item1 in item["ChildAge"])
        //                {
        //                    ID = roomno;
        //                    string Agevalue = Convert.ToString(item1["Age"]);
        //                    if (Agevalue == "<1")
        //                        Age = 0;
        //                    else
        //                        Age = Convert.ToInt16(item1["Age"]);
        //                    dtChildrenAgeSearch.Rows.Add(ID, Age);
        //                }
        //            }
        //        }
        //    }

        //    objlist = BL_PropDetails.GetPropertyDetails(objlist.iPropId, objSearch, dtRoomOccupancySearch, dtChildrenAgeSearch);
        //    objlist.dtCheckIn = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(din);
        //    objlist.dtCheckOut = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(dout);
        //    objlist.iTotalDays = (objSearch.dtCheckOut - objSearch.dtCheckIn).TotalDays.ToString();
        //    objlist.Currency = Session["CurrencyCode"].ToString();
        //    return PartialView("pvHotelRooms", objlist);
        //}

        public ActionResult GetGoogleSharePage(long propId)
        {
            var propDetail = BL_PropDetails.GetPropDetailForSharing(propId);
            return View("_ShareGooglePartial", propDetail);
        }

    }
}