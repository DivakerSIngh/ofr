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
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Configuration;

namespace OneFineRateWeb.Controllers.Bidding
{
    public class BiddingController : BaseController
    {
        DataTable dtTblRoomOccupancySearch = new DataTable();
        DataTable dtTblChildrenAgeSearch = new DataTable();
        DateTime CheckIn, CheckOut;
        eBiddingCounts bidObj = new eBiddingCounts();

        // GET: Bidding
        [Route("wish-to-bid")]
        public ActionResult Index(PropSearchRequestModel model)
        {
            string type = "";
            if (HttpContext.Request.Params["type"] != null && HttpContext.Request.Params["type"] != "") { type = HttpContext.Request.Params["type"]; }
            ViewBag.type = type == "" ? null : type;

            eBiddingSearch obj = new eBiddingSearch();

            if (User.Identity.IsAuthenticated)
            {
                var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                obj.sUserTitle = user.Title;
                obj.sUserFirstName = user.FirstName;
                obj.sUserLastName = user.LastName;
                obj.sUserEmail = user.Email;
                obj.sUserMobileNo = user.PhoneNumber;
                obj.iStateId = user.StateId.HasValue? user.StateId.Value:0;
            }

            obj.sSearchId = Convert.ToInt32(model.cid);
            obj.sSearchName = model.cname;
            obj.sSearchType = model.ctype == "City" ? "C" : model.ctype == "Locality" ? "L" : "A";
            obj.ctype = model.ctype;
            obj.sJsonRoomData = model.sRoomData;
            obj.sCheckIn = model.sCheckIn;
            obj.sCheckOut = model.sCheckOut;
            obj.cname = model.cname;
            obj.type = ViewBag.type;
            obj.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();
            List<etblIndianLocalityCordinate> cordinatesList;

            var list = BL_Bidding.GetAreaLocalityForBid(obj.sSearchType, obj.sSearchId, out cordinatesList);
            obj.lstPolygonData = cordinatesList;

            obj.sJsonLocality = OneFineRateAppUtil.clsUtils.ConvertToJson(list);
            obj.BidSearchData = "BidSearchData" + Guid.NewGuid().ToString();
            obj.BidData = "BidData" + Guid.NewGuid().ToString();
            Session[obj.BidSearchData] = obj;
            Session[obj.BidData] = model;
            //TempData.Keep();
            return View(obj);
        }
        [Route("Map")]
        public ActionResult Map()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLocalities(string Type, int Id)
        {
            //TO DO
            //Take care later
            List<etblIndianLocalityCordinate> polygonData;
            var list = BL_Bidding.GetAreaLocalityForBid(Type, Id, out polygonData);
            return Json(new
            {
                list,
                polygonData = polygonData
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetBidRange(string Localities, int RatingId, string Type,string bidData)
        {
            PropSearchRequestModel obj = new PropSearchRequestModel();
            string sRoomData = string.Empty;
            if (Session[bidData] != null)
            {
               // TempData.Keep();
                obj = Session[bidData] as PropSearchRequestModel;
                sRoomData = obj.sRoomData;
            }

            string[] ids = Localities.TrimEnd(',').Split(',');

            DataTable dtIds = new DataTable();
            dtIds.Columns.AddRange(new DataColumn[1] { new DataColumn("Id", typeof(int)) });

            foreach (var item in ids)
            {
                DataRow dtLocality = dtIds.NewRow();
                dtLocality["Id"] = item;
                dtIds.Rows.Add(dtLocality);
            }

            GetRoomDataAndDates(sRoomData, obj.sCheckIn, obj.sCheckOut);

            string Currency = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";

            var list = BL_Bidding.GetBidRangeValues(CheckIn, CheckOut, Currency, RatingId, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, Type, dtIds);

            return Json(new { list }, JsonRequestBehavior.AllowGet);

        }
        public void GetRoomDataAndDates(string sRoomData, string sCheckIn, string sCheckOut )
        {
            #region FetchRoomData and bind in datatables

            var roomDataResult = new List<RoomData>();

            if (sRoomData != null)
            {
                roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);
            }

            dtTblRoomOccupancySearch.Columns.AddRange(new DataColumn[3]
                {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("iAdults", typeof(short)),
                        new DataColumn("children",typeof(short))
                });

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

            CheckIn = DateTime.Parse(sCheckIn);
            CheckOut = DateTime.Parse(sCheckOut);
        }
        public ActionResult ValidateUserData(string Title, string FirstName, string LastName, string EmailID, string Mobile, string VC, string OTPCode, decimal BidAmount, string RoomData, string Localities, int RatingId, string Type, decimal Taxes, decimal MinRange, decimal MaxRange, string Symbol, string SelectedAreaId, string URL, int iStateId,string bidSearchData)
        {
            var stronglObject = new eBiddingSearch();
            try
            {
                TempData["URL"] = URL.ToString().Replace("wish-to-bid", "Search").Replace("bid", "negotiate");
                if (Session[bidSearchData] != null) {
                    //TempData.Keep();
                    stronglObject = Session[bidSearchData] as eBiddingSearch; }
                stronglObject.dBidPrice = Convert.ToDecimal(BidAmount);
                stronglObject.sJsonRoomData = stronglObject.sJsonRoomData;
                stronglObject.sLocalityType = Type;
                stronglObject.sLocalityData = Localities;
                stronglObject.sStarRating = RatingId;
                stronglObject.dMaxRange = MaxRange;
                stronglObject.dMinRange = MinRange;
                stronglObject.dTaxes = Taxes;
                stronglObject.Symbol = Symbol;
                stronglObject.sSelectedAreaId = SelectedAreaId;
                RoomData = stronglObject.sJsonRoomData;

                stronglObject.iBiddingCount = BL_Bidding.GetBiddingCount(Mobile);

                if (stronglObject.iBiddingCount < int.Parse(ConfigurationManager.AppSettings["bidLimit"]))
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                        stronglObject.sUserTitle = user.Title;
                        stronglObject.sUserFirstName = FirstName;
                        stronglObject.sUserLastName = LastName;
                        stronglObject.sUserEmail = EmailID;
                        stronglObject.iUserId = user.Id;
                        stronglObject.iStateId = user.StateId.HasValue ? user.StateId.Value : iStateId;

                        if (user.PhoneNumber == Mobile)
                        {
                            GetRoomDataAndDates(RoomData, stronglObject.sCheckIn, stronglObject.sCheckOut);
                            string[] ids = stronglObject.sLocalityData.TrimEnd(',').Split(',');

                            DataTable dtIds = new DataTable();
                            dtIds.Columns.AddRange(new DataColumn[1] { new DataColumn("Id", typeof(int)) });

                            foreach (var item in ids)
                            {
                                DataRow dtLocality = dtIds.NewRow();
                                dtLocality["Id"] = item;
                                dtIds.Rows.Add(dtLocality);
                            }

                            string Currency = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";
                            var list = BL_Bidding.SearchHotelsByBidAmount(CheckIn, CheckOut, Currency, RatingId, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, Type, dtIds, stronglObject.dBidPrice, iStateId);

                            if (list.Status == 1)
                            {
                                stronglObject.lstHotelData = list.lstHotelData;
                                stronglObject.Amount = list.Amount;
                                stronglObject.Tax = list.Tax;
                                stronglObject.sCheckIn_Display = list.sCheckIn_Display;
                                stronglObject.sCheckOut_Display = list.sCheckOut_Display;
                                stronglObject.TopMessage = list.TopMessage;
                                stronglObject.MinTaxPer = list.MinTaxPer;

                                stronglObject.SGST_Per = list.SGST_Per;
                                stronglObject.SGST_Val = list.SGST_Val;
                                stronglObject.sHotelTax = list.sHotelTax;
                                stronglObject.sOFRServiceCharge = list.sOFRServiceCharge;
                                stronglObject.sOFRServiceChargeIncludingTax = list.sOFRServiceChargeIncludingTax;
                                stronglObject.sTaxOnServiceCharge_Per = list.sTaxOnServiceCharge_Per;
                                stronglObject.sTaxOnServiceCharge_Val= list.sTaxOnServiceCharge_Val;
                                stronglObject.bShowIGST = list.bShowIGST;
                                stronglObject.cGstValueType = list.cGstValueType;
                                stronglObject.dGSTValue = list.dGSTValue;
                            }
                            else
                            {
                                stronglObject.iBiddingCount = stronglObject.iBiddingCount + 1;
                            }
                            var maxBiddingCount = Convert.ToInt32(ConfigurationManager.AppSettings["bidLimit"]);
                            stronglObject.Message = stronglObject.iBiddingCount==(maxBiddingCount-1)?string.Concat(list.Message, " You have only one more chance remaining."): stronglObject.iBiddingCount==maxBiddingCount? "We're sorry, but you have utilized all you three chances now." : list.Message;
                            stronglObject.Status = list.Status;

                            //Increment bidding count for a mobile
                            bidObj.sMobile = Mobile;
                            BL_Bidding.AddRecordToBiddingCount(bidObj);

                            return Json(new { Status = list.Status, msg = stronglObject.Message, count = stronglObject.iBiddingCount }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (OTPCode != "" && OTPCode != null)
                            {
                                if (DecodeVC(VC) == OTPCode)
                                {
                                    GetRoomDataAndDates(RoomData, stronglObject.sCheckIn, stronglObject.sCheckOut);

                                    string[] ids = stronglObject.sLocalityData.TrimEnd(',').Split(',');

                                    DataTable dtIds = new DataTable();
                                    dtIds.Columns.AddRange(new DataColumn[1] { new DataColumn("Id", typeof(int)) });

                                    foreach (var item in ids)
                                    {
                                        DataRow dtLocality = dtIds.NewRow();
                                        dtLocality["Id"] = item;
                                        dtIds.Rows.Add(dtLocality);
                                    }

                                    string Currency = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";
                                    var list = BL_Bidding.SearchHotelsByBidAmount(CheckIn, CheckOut, Currency, RatingId, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, Type, dtIds, stronglObject.dBidPrice, iStateId);

                                    if (list.Status == 1)
                                    {
                                        stronglObject.lstHotelData = list.lstHotelData;
                                        stronglObject.Amount = list.Amount;
                                        stronglObject.Tax = list.Tax;
                                        stronglObject.sCheckIn_Display = list.sCheckIn_Display;
                                        stronglObject.sCheckOut_Display = list.sCheckOut_Display;
                                        stronglObject.TopMessage = list.TopMessage;
                                        stronglObject.MinTaxPer = list.MinTaxPer;

                                        stronglObject.SGST_Per = list.SGST_Per;
                                        stronglObject.SGST_Val = list.SGST_Val;
                                        stronglObject.sHotelTax = list.sHotelTax;
                                        stronglObject.sOFRServiceCharge = list.sOFRServiceCharge;
                                        stronglObject.sOFRServiceChargeIncludingTax = list.sOFRServiceChargeIncludingTax;
                                        stronglObject.sTaxOnServiceCharge_Per = list.sTaxOnServiceCharge_Per;
                                        stronglObject.sTaxOnServiceCharge_Val = list.sTaxOnServiceCharge_Val;
                                        stronglObject.bShowIGST = list.bShowIGST;
                                    }
                                    else
                                    {
                                        stronglObject.iBiddingCount = stronglObject.iBiddingCount + 1;
                                    }
                                    var maxBiddingCount = Convert.ToInt32(ConfigurationManager.AppSettings["bidLimit"]);
                                    stronglObject.Message = stronglObject.iBiddingCount == (maxBiddingCount - 1) ? string.Concat(list.Message, " You have only one more chance remaining.") : stronglObject.iBiddingCount == maxBiddingCount ? "We're sorry, but you have utilized all you three chances now." : list.Message;
                                    stronglObject.Status = list.Status;

                                    //Increment bidding count for a mobile
                                    bidObj.sMobile = Mobile;
                                    BL_Bidding.AddRecordToBiddingCount(bidObj);

                                    return Json(new { st = 2, Status = list.Status, msg = list.Message, count = stronglObject.iBiddingCount }, JsonRequestBehavior.AllowGet);
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
                        stronglObject.iStateId = iStateId;

                        if (OTPCode != "" && OTPCode != null)
                        {
                            if (DecodeVC(VC) == OTPCode)
                            {
                                GetRoomDataAndDates(RoomData, stronglObject.sCheckIn, stronglObject.sCheckOut);

                                string[] ids = stronglObject.sLocalityData.TrimEnd(',').Split(',');

                                DataTable dtIds = new DataTable();
                                dtIds.Columns.AddRange(new DataColumn[1] { new DataColumn("Id", typeof(int)) });

                                foreach (var item in ids)
                                {
                                    DataRow dtLocality = dtIds.NewRow();
                                    dtLocality["Id"] = item;
                                    dtIds.Rows.Add(dtLocality);
                                }

                                string Currency = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";
                                var list = BL_Bidding.SearchHotelsByBidAmount(CheckIn, CheckOut, Currency, RatingId, dtTblRoomOccupancySearch, dtTblChildrenAgeSearch, Type, dtIds, stronglObject.dBidPrice, iStateId);

                                if (list.Status == 1)
                                {
                                    stronglObject.lstHotelData = list.lstHotelData;
                                    stronglObject.Amount = list.Amount;
                                    stronglObject.Tax = list.Tax;
                                    stronglObject.sCheckIn_Display = list.sCheckIn_Display;
                                    stronglObject.sCheckOut_Display = list.sCheckOut_Display;
                                    stronglObject.TopMessage = list.TopMessage;
                                    stronglObject.MinTaxPer = list.MinTaxPer;
                                    stronglObject.SGST_Per = list.SGST_Per;
                                    stronglObject.SGST_Val = list.SGST_Val;
                                    stronglObject.sHotelTax = list.sHotelTax;
                                    stronglObject.sOFRServiceCharge = list.sOFRServiceCharge;
                                    stronglObject.sOFRServiceChargeIncludingTax = list.sOFRServiceChargeIncludingTax;
                                    stronglObject.sTaxOnServiceCharge_Per = list.sTaxOnServiceCharge_Per;
                                    stronglObject.sTaxOnServiceCharge_Val = list.sTaxOnServiceCharge_Val;
                                    stronglObject.bShowIGST = list.bShowIGST;
                                }
                                else
                                {
                                    stronglObject.iBiddingCount = stronglObject.iBiddingCount + 1;
                                }

                                var maxBiddingCount = Convert.ToInt32(ConfigurationManager.AppSettings["bidLimit"]);
                                stronglObject.Message = stronglObject.iBiddingCount == (maxBiddingCount - 1) ? string.Concat(list.Message, " You have only one more chance remaining.") : stronglObject.iBiddingCount == maxBiddingCount ? "We're sorry, but you have utilized all you three chances now." : list.Message;
                                stronglObject.Status = list.Status;

                                //Increment bidding count for a mobile
                                bidObj.sMobile = Mobile;
                                BL_Bidding.AddRecordToBiddingCount(bidObj);

                                return Json(new { st = 2, Status = list.Status, msg = stronglObject.Message, count = stronglObject.iBiddingCount }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                return Json(new { st = 0, msg = "OTP is incorrect." }, JsonRequestBehavior.AllowGet);

                            }
                        }
                        else
                        {
                            string VerificationCode = ResendVerification(stronglObject.sUserMobileNo);

                            if (VerificationCode == "error")
                            {
                                return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                stronglObject.sVerificationCode = VerificationCode;
                            }
                        }
                    }
                }
                else
                {
                    return Json(new { st = 0, msg = "Sorry, you have exceeded maximum number of bid attempts for the day. Please click Bargain button to search hotels of your choice." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
            
            Session[stronglObject.BidSearchData] = stronglObject;
            //TempData.Keep();

            return Json(new { st = 1, status = true, VC = stronglObject.sVerificationCode }, JsonRequestBehavior.AllowGet);

        }
        private string DecodeVC(string VerificationCode)
        {
            try
            {
                var decode = OneFineRateAppUtil.clsUtils.Decode(VerificationCode);
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
                var encodedCode = OneFineRateAppUtil.clsUtils.Encode(phoneVerificationCode.ToString());
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

        public ActionResult GetUserDetails(string bidSearchData)
        {
            var stronglObject = new eBiddingSearch();
            try
            {
                if (Session[bidSearchData] != null) {
                   // TempData.Keep();
                    stronglObject = Session[bidSearchData] as eBiddingSearch; }
                     ViewBag.type = "G";
            }
            catch
            {

            }
            return PartialView("pGuestDetails", stronglObject);
        }

        public ActionResult ReSendOTPCode(string Mobile, string VC)
        {
            var encodedCode = "";
            try
            {
                int phoneVerificationCode;
                if (VC != string.Empty)
                {
                    phoneVerificationCode = Convert.ToInt32(DecodeVC(VC));
                }
                else
                {
                    phoneVerificationCode = clsUtils.GetVerificationCode();
                }
                encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                string message = phoneVerificationCode + " is your One Time Mobile verification code for OneFineRate";
                clsUtils.sendSMS(Mobile, message);
            }
            catch (Exception)
            {
                return Json(new { st = "0" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { st = "1", VC = encodedCode }, JsonRequestBehavior.AllowGet);
        }

        public static string getJsonfromDatable(DataTable dt)
        {
            StringBuilder str = new StringBuilder();

            str.Append("[");

            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");

                string jsonval = "";

                jsonval = "\"value\" : \"" + dr[0] + "\" ,";
                str.Append(jsonval);


                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);


            return str.ToString();
        }

        public ActionResult BiddingInfo(string bidSearchData)
        {
            Decimal? ExchangeRate = 1;
            var obj = new eBiddingSearch();
            ViewBag.HeaderBarData = "Congratulations! We have found hotels matching your bid";

            if (Session[bidSearchData] != null)
            {
            //    TempData.Keep();
                obj = Session[bidSearchData] as eBiddingSearch;
            }

            obj.lstLocalities = BL_Bidding.GetAllLocalitiesName(obj.sLocalityData, obj.sLocalityType);

            DateTime checkin = DateTime.Parse(obj.sCheckIn);
            DateTime checkOut = DateTime.Parse(obj.sCheckOut);

            obj.sCheckInDay = String.Format("{0:dddd MMM d yyyy}", checkin);
            obj.sCheckOutDay = String.Format("{0:dddd MMM d yyyy}", checkOut);

            obj.iNights = Convert.ToInt32((checkOut - checkin).TotalDays);
            var data = GetRoomsCount(obj.sJsonRoomData);
            obj.iRooms = data.room;
            obj.iChildrens = data.child;
            obj.iAdults = data.adult;

            string CurrencyCode = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";

            if (CurrencyCode != "INR")
            {
                etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR", CurrencyCode);
                if (objExchange.dRate != 0)
                {
                    ExchangeRate = objExchange.dRate;
                }
            }

            obj.dTaxes = obj.Tax;  //taxes provided are full n final.
            obj.dTotalPrice = obj.dBidPrice * obj.iRooms * obj.iNights;
            obj.dGrandTotal = obj.dTotalPrice + obj.dTaxes;

            return View(obj);
        }

        public RoomData GetRoomsCount(string sRoomData)
        {
            RoomData obj = new RoomData();

            try
            {
                var roomDataResult = new List<RoomData>();
                if (sRoomData != null)
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(sRoomData);
                }
                foreach (var room in roomDataResult)
                {
                    obj.room += 1;
                    obj.adult += room.adult;
                    obj.child += room.child;
                }
            }
            catch (Exception)
            {

            }
            return obj;
        }


        [HttpPost]
        public ActionResult PayNow(eBiddingSearch obj)
        {
            try
            {
                Decimal? ExchangeRate = 1;
                etblBookingTx bookobj = new etblBookingTx();

                if (User.Identity.IsAuthenticated)
                {
                    var user = BL_WebsiteUser.GetSingleRecordById(User.Identity.GetUserId<long>());
                    bookobj.iCustomerId = user.Id;
                    bookobj.iGuestId = 0;
                }
                else
                {
                    var stronglObject = Session[obj.BidSearchData] as eBiddingSearch;
                    GuestUserDetails objDetail = new GuestUserDetails();
                    objDetail.Title = obj.sUserTitle == "1" ? "Mr." : obj.sUserTitle == "2" ? "Ms." : "";
                    objDetail.FirstName = obj.sUserFirstName;
                    objDetail.LastName = obj.sUserLastName;
                    objDetail.Email = obj.sUserEmail;
                    objDetail.PhoneNumber = obj.sUserMobileNo;
                    objDetail.iStateId = stronglObject.iStateId;
                    int i = BL_Bidding.AddGuestDetailsRecord(objDetail);
                    if (i > 0)
                    {
                        bookobj.iGuestId = i;

                    }
                    else
                    {
                        return View("BiddingInfo", obj);
                    }
                }

                string CurrencyCode = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";

                if (CurrencyCode != "INR")
                {
                    etblExchangeRatesM objExchange = BL_ExchangeRate.GetSingleRecordById("INR",CurrencyCode);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = 1/objExchange.dRate;
                    }
                }

                bookobj.dtCheckIn = Convert.ToDateTime(obj.sCheckIn); ;
                bookobj.dtChekOut = Convert.ToDateTime(obj.sCheckOut);
                bookobj.dtReservationDate = DateTime.Now;
                bookobj.cBookingType = "B";
                bookobj.sTitleOFR = obj.sUserTitle;
                bookobj.sFirstNameOFR = obj.sUserFirstName;
                bookobj.sLastNameOFR = obj.sUserLastName;
                bookobj.sEmailOFR = obj.sUserEmail;
                bookobj.sMobileOFR = obj.sUserMobileNo;
                bookobj.dtActionDate = DateTime.Now;
                bookobj.BookingStatus = "PP";
                bookobj.PaymentStatus = "P";
                bookobj.dBidAmount = obj.dBidPrice * ExchangeRate;
                bookobj.dTotalAmount = obj.dTotalPrice * ExchangeRate;
                bookobj.dTaxes = obj.dTaxes * ExchangeRate;
                bookobj.dTaxesForHotel = obj.dTaxes * ExchangeRate;
                bookobj.sCurrencyCode = Session["CurrencyCode"] != null ? Session["CurrencyCode"].ToString() : "INR";
                bookobj.iBidStarCategory = Convert.ToInt16(obj.sStarRating);
                bookobj.sBidType = obj.sLocalityType;
                bookobj.sIDs = obj.sLocalityData.TrimEnd(',');
                bookobj.dServiceCharge =Convert.ToDecimal(obj.sOFRServiceCharge);
                bookobj.dGSTOnServiceCharge = Convert.ToDecimal(obj.sTaxOnServiceCharge_Val);
                bookobj.dGSTServiceType = obj.cGstValueType;
                bookobj.dGSTValue =Convert.ToString( obj.dGSTValue);
                string TimeZone = Session["TimeZone"] != null ? Session["TimeZone"].ToString() : "+5:30";
                decimal zone = Convert.ToDecimal(TimeZone.Replace(":", ".").Replace("+", ""));
                bookobj.iCountryOffset = zone;

                #region FetchRoomData and bind in datatables

                var roomDataResult = new List<RoomData>();
                if (obj.sJsonRoomData != null)
                {
                    roomDataResult = new JavaScriptSerializer().Deserialize<List<RoomData>>(obj.sJsonRoomData);
                }

                List<etblBIDRoomAdultsTx> lstBid = new List<etblBIDRoomAdultsTx>();

                foreach (var room in roomDataResult)
                {
                    etblBIDRoomAdultsTx RoomObj = new etblBIDRoomAdultsTx();
                    RoomObj.iRoomNo = Convert.ToInt16(room.room);
                    RoomObj.iAdults = Convert.ToInt16(room.adult);
                    RoomObj.iChildren = Convert.ToInt16(room.child);

                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    foreach (var child in room.ChildAge)
                    {
                        str.Append(child.Age);
                        str.Append(",");
                    }

                    string ChildAges = str.ToString().TrimEnd(',');
                    RoomObj.sChildAge = ChildAges;
                    lstBid.Add(RoomObj);
                }

                #endregion

                etblBookingTrakerTx trkobj = new etblBookingTrakerTx();
                trkobj.BookingStatus = "PP";
                trkobj.dtActionDate = DateTime.Now;
                
                int j = BL_Booking.AddBookingForBid(bookobj, trkobj, lstBid);
                if (j > 0)
                {
                    obj.iBookingId = j;
                    Session[obj.BidSearchData] = obj;
                  //  TempData.Keep();
                    return RedirectToAction("PayNow", "Payment", new { bookingId = j });
                }
                else
                {

                }
            }
            catch (Exception)
            {

            }

            Session[obj.BidSearchData] = obj;
          //  TempData.Keep();
            return View("BiddingInfo", obj);
        }

        public ActionResult BiddingPreview(string bidSearchData)
        {
            var obj = new eBiddingSearch();
            if (Session[bidSearchData] != null) { //TempData.Keep();
                obj = Session[bidSearchData] as eBiddingSearch; }
            return View(obj);
        }

        public ActionResult OpenPreviewPage(eBiddingSearch obj)
        {
            Session[obj.BidSearchData] = obj;
          //  TempData.Keep();
            return RedirectToAction("BiddingInfo");
        }

        //Called after accepting bid amount on payment gateway
        [Route("GetBidHotelsList/{bookingId}", Name = "GetBidHotelsList")]
        public ActionResult GetBidSearchedHotels(string bookingId)
        {
            int bookId = Convert.ToInt32(clsUtils.Decode(bookingId));


            //fetch the data of booking
            var booking = BL_Booking.GetBooking(bookId);

            if (booking.iPropId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            eBidding objdata = new eBidding();
            objdata = BL_Bidding.GetSearchedBidHotelsListForUnfinished(bookId);

            if (objdata.lstBidRoomsData.Count > 0)
            {
                DataTable dtPropIds = new DataTable();
                dtPropIds.Columns.AddRange(new DataColumn[1]
                {
                        new DataColumn("Id", typeof(int))
                });

                foreach (var item in objdata.lstBidRoomsData)
                {
                    DataRow drpropid = dtPropIds.NewRow();
                    drpropid["Id"] = item.iPropId;
                    dtPropIds.Rows.Add(drpropid);
                }

                BL_PropDetails.UpdateViewForConversion("B", dtPropIds, bookId);
            }
            objdata.iBookingId = bookId;
            objdata.BidSearchHotels = "BidSearchHotels" + Guid.NewGuid().ToString();
            Session[objdata.BidSearchHotels] = objdata;
            return View(objdata);

        }
        
        public ActionResult UpdateBooking(eBidding obj)
        {
            try
            {
                int bookingId, PropId;
                PropId = obj.SelectedPropId;
                bookingId = obj.iBookingId;

                eBidBookingResult Robj = BL_Bidding.UpdateBidBooking(bookingId, PropId);
                if (Robj.Status == "Success")
                {
                    //var bookingModel = BL_Booking.GetBooking(bookingId);

                    //string Status = "Thanks " + bookingModel.sFirstNameOFR + "! We have recieved your final payment.";
                    //Task.Run(() => MailComponent.SendEmailAsync(bookingModel.sEmailOFR, "", "", "Hotel Selected", Status, null, null, false));
                    //Task.Run(() => clsUtils.sendSMS(bookingModel.sMobileOFR, Status));
                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(Robj.ID)) });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// After Final Selection of the bid property after making payment
        /// </summary>
        /// <param name="bookingid"></param>
        /// <param name="propid"></param>
        /// <param name="bidSearchHotels"></param>
        /// <returns></returns>
        public ActionResult GetUpgradeHotelDetails(int bookingid, int propid,string bidSearchHotels)
        {
            var obj = new eBidding();

            if (Session[bidSearchHotels] != null) { //TempData.Keep();
                obj = Session[bidSearchHotels] as eBidding; }


            var prop = obj.lstBidRoomsData.Where(u => u.iPropId == propid).SingleOrDefault();


            BiddingHotelsUpgrade objUpgrade = BL_Bidding.GetHotelUpgradeResult(propid, bookingid);

            objUpgrade.objBidRoomsData = prop;
            objUpgrade.iBookingId = bookingid;
            objUpgrade.iPropId = propid;
            objUpgrade.BIdUpgradeHotels = "BIdUpgradeHotels" + Guid.NewGuid().ToString();
            Session[objUpgrade.BIdUpgradeHotels] = objUpgrade;
            return PartialView("BidInfo", objUpgrade);
        }

        public ActionResult UpdateBookingOnUpgrade(BiddingHotelsUpgrade obj)
        {
            try
            {
               // TempData.Peek("BIdUpgradeHotels");
                //TempData.Peek("BidSearchHotels");
              //  TempData.Peek("BidSearchData");
                int bookingId, PropId;
                PropId = obj.iPropId;
                bookingId = obj.iBookingId;

                eBidBookingResult Robj = BL_Bidding.UpdateBidBooking(bookingId, PropId);
                if (Robj.Status == "Success")
                {
                    //var bookingModel = BL_Booking.GetBooking(bookingId);

                    //string Status = "Thanks " + bookingModel.sFirstNameOFR + "! We have recieved your final payment.";
                    //Task.Run(() => MailComponent.SendEmailAsync(bookingModel.sEmailOFR, "", "", "Hotel Selected", Status, null, null, false));
                    //Task.Run(() => clsUtils.sendSMS(bookingModel.sMobileOFR, Status));
                    return RedirectToRoute("GuestInformation", new { bookingId = OneFineRateAppUtil.clsUtils.Encode(Convert.ToString(Robj.ID)) });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult GetLeftPayment(BiddingHotelsUpgrade objUpgrade)
        {
            var obj = new BiddingHotelsUpgrade();
            if (Session[objUpgrade.BIdUpgradeHotels] != null) { //TempData.Keep();
                obj = Session[objUpgrade.BIdUpgradeHotels] as BiddingHotelsUpgrade; }

            var prop = obj.lstRoomsData.Where(u => u.iRoomId == objUpgrade.iRoomId).SingleOrDefault();

            Session[objUpgrade.BIdUpgradeHotels] = prop;
            return RedirectToAction("PayNow", "Payment", new { bookingId = objUpgrade.iBookingId, propDetailsGUID= objUpgrade.BIdUpgradeHotels, cType = "UpgradeAmt" });
        }
        

        [Route("BidHotelDetails/{sPropId}/{sRoomId}", Name = "BidHotelDetails")]
        public ActionResult GetBidSearchedHotelDetails(string sPropId, string sRoomId)
        {
            if (!string.IsNullOrEmpty(sPropId))
            {
                int propId = Convert.ToInt32(clsUtils.Decode(sPropId));
                int roomId = Convert.ToInt32(clsUtils.Decode(sRoomId));
                PropDetailsM objSearch = new PropDetailsM();
                objSearch.iPropId = propId;
                objSearch.dtCheckIn = DateTime.Now.Date.AddDays(10);
                objSearch.dtCheckOut = DateTime.Now.Date.AddDays(11);
                objSearch.bLogin = User.Identity.IsAuthenticated;
                objSearch.Currency = CurrencyCode;

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

                var propDetails = BL_PropDetails.GetPropertyDetails(propId, objSearch, dtRoomOccupancySearch, dtChildrenAgeSearch);
                var roomDetails = BL_PropDetails.GetRoomDetails(propId, roomId, CurrencyCode);
                propDetails.BidRoomDetails = roomDetails;
                propDetails.sRoomId = roomId.ToString();

                return View("_HotelDetails", propDetails);

            }

            return View("Error");
        }
    }
}