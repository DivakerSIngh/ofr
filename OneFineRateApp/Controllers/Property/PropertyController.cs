using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Data.Entity.Validation;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace OneFineRateApp.Controllers.Property
{
    public class PropertyController : BaseController
    {
        // GET: Property
        [Route("Property")]
        public ActionResult Index()
        {
            etblPropertyM obj = new etblPropertyM();
            obj.StateList = BL_tblStateM.StateList(obj.iStateId, obj.iCountryId);
            obj.CityList = BL_tblCityM.CityList(obj.iCityId, obj.iStateId, obj.iCountryId);
            obj.AreaList = BL_tblMacroAreaM.AreasList(obj.iAreaId, obj.iCityId);
            obj.LocalityList = BL_Locality.LocalityList(obj.iLocalityId, obj.iAreaId);
            obj.Mode = "Add";
            obj.latitude = Convert.ToDecimal("28.577286048");
            obj.longitude = Convert.ToDecimal("77.203674316");
            obj.sInitialHotelName = "";
            return View(obj);
        }
        [HttpPost]
        public ActionResult Add(etblPropertyM prop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    prop.dtActionDate = DateTime.Now;
                    if (prop.Mode == "Add")
                    {
                        prop.cStatus = "I";
                        int result = BL_tblPropertyM.AddRecord(prop);
                        if (result == 1)
                        {
                            TempData["msg"] = "Property Added Successfully";
                            return RedirectToAction("List");
                        }
                        else
                        {
                            return View("Index", prop);
                        }
                    }
                    else
                    {
                        int result = BL_tblPropertyM.UpdateRecord(prop);
                        if (result == 1)
                        {
                            TempData["msg"] = "Property Modified Successfully";
                            return RedirectToAction("List");
                        }
                        else
                        {
                            return View("Index", prop);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            else
            {
                TempData["ERROR"] = "Validation failed! Please correct the errors and try again.";
                prop.StateList = BL_tblStateM.StateList(prop.iStateId, prop.iCountryId);
                prop.CityList = BL_tblCityM.CityList(prop.iCityId, prop.iStateId, prop.iCountryId);
                prop.AreaList = BL_tblMacroAreaM.AreasList(prop.iAreaId, prop.iCityId);
                prop.LocalityList = BL_Locality.LocalityList(prop.iLocalityId, prop.iAreaId);
                prop.latitude = prop.dLatitude;
                prop.longitude = prop.dLongitude;
                prop.Mode = "Edit";

                return View("Index", prop);
            }
            //else
            //{
            //    string errormsg = "";
            //    foreach (ModelState modelState in ViewData.ModelState.Values)
            //    {
            //        foreach (ModelError error in modelState.Errors)
            //        {
            //            errormsg += error.ErrorMessage;
            //            errormsg += "</br>";
            //        }
            //    }
            //}

        }
        public ActionResult List()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewDashboard_Mobile(int PropId)
        {

            Session["PropId"] = PropId;
            Session["CurrCode"] = BL_Currency.GetCurrencyByPropId(PropId);
            //Session["Symbol"] = BL_Currency.GetSymbolByPropId(PropId);
            Session["Flag"] = BL_Currency.GetFlagByPropId(PropId);
            TempData["PropID"] = PropId;

            return View("../Dashboard/Dashboard");
        }
        [Authorize]
        public ActionResult ViewDashboard(int PropId, string PropName)
        {
            Session["MenuType"] = "2";
            Session["PropId"] = PropId;
            Session["CurrCode"] = BL_Currency.GetCurrencyByPropId(PropId);
            //Session["Symbol"] = BL_Currency.GetSymbolByPropId(PropId);
            Session["Flag"] = BL_Currency.GetFlagByPropId(PropId);
            Session["PropName"] = PropName;

            //eDashboardModel dashboardModel = new eDashboardModel();
            //dashboardModel.iPropId = PropId;


            //return View("../Dashboard/Dashboard", dashboardModel);

            // return RedirectToAction("ViewDashboard1", new { PropId = PropId });
            return RedirectToAction("ViewDashboard1");
        }


        [HttpGet]
        public JsonResult GetBookingOverviewData(int iPropId, int iDays)
        {
            try
            {
                var data = BL_tblPropertyM.GetBookingOverview_GraphData(iPropId, iDays);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = new eBookingOverviewGraphModel() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetNegotiationOverviewData(int iPropId, int iDays)
        {
            try
            {
                var data = BL_tblPropertyM.GetNegotiationOverview_GraphData(iPropId, iDays);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = new eNegotiationOverviewGraphModel() }, JsonRequestBehavior.AllowGet);
            }
        }

       
        public ActionResult GetPerformanceOverview(int iPropId, int iDays)
        {
            try
            {
                var data = BL_tblPropertyM.GetPerformanceOverview_GraphData(iPropId, iDays);

                return PartialView("~/Views/Dashboard/_PerformanceOverview.cshtml", data);
            }
            catch (Exception)
            {
                return Content("");
            }
        }


        [HttpGet]
        public JsonResult GetBookingInsightsData(int iPropId, int iDays, string cType, string bookingOrRevenue)
        {
            try
            {
                var data = BL_tblPropertyM.GetBookingInsights_GraphData(iPropId, iDays, cType, bookingOrRevenue);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = new eBookingInsightsGraphModel() }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public ActionResult ViewDashboard1()
        {
            return View("../Dashboard/Dashboard");
        }

        /// <summary>
        /// Not in Use
        /// </summary>
        /// <returns></returns>
        public JsonResult BindBestPossibleRates()
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            List<etblPropertyDashboardView> results = new List<etblPropertyDashboardView>();
            results = BL_tblPropertyM.GetAllBestPossibleRates(PropId);
            return Json(new
                {
                    suggestions = results
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit()
        {
            int id = 0;
            if (Session["PropId"] != null)
            {
                id = Convert.ToInt32(Session["PropId"].ToString());
            }
            else
            {
                throw new Exception("Property ID Null");
            }
            etblPropertyM_User obj = new etblPropertyM_User();
            try
            {
                obj = BL_tblPropertyM.GetSingleRecordById_User(id);
                obj.sviewType = "Edit";
                obj.StateList = BL_tblStateM.StateList(obj.iStateId, obj.iCountryId);
                obj.CityList = BL_tblCityM.CityList(obj.iCityId, obj.iStateId, obj.iCountryId);
                obj.AreaList = BL_tblMacroAreaM.AreasList(obj.iAreaId, obj.iCityId);
                obj.LocalityList = BL_Locality.LocalityList(obj.iLocalityId, obj.iAreaId);
                obj.AccessbilityItems = BL_tblAccessbilityM.GetAccessbilities(obj.sAccessbilityIds);
                obj.AwardsItems = BL_tblAwardM.GetAwards(obj.sAwardIds);
                obj.AffilationItems = BL_tblAffiliationM.GetAffilations(obj.sAffiliationIds);
                obj.LanguagesItems = BL_tblLanguageM.GetLanguages(obj.sLanguageIds);
                //for only showing purpose
                obj.sHotelNameO = obj.sHotelName;
                obj.iChainIdO = obj.iChainId;
                obj.iCountryIdO = obj.iCountryId;
                obj.iStateIdO = obj.iStateId;
                obj.iCityIdO = obj.iCityId;
                obj.iAreaIdO = obj.iAreaId;
                obj.iLocalityIdO = obj.iLocalityId;

                ViewBag.PropertyLocalityMap = GetLocalitiesJson(BL_tblPropertyM.GetPropertyLocalityMap(id));
            }
            catch (Exception)
            {
                throw;
            }
            return View(obj);
        }
        public ActionResult EditMaster(int id)
        {
            etblPropertyM obj = new etblPropertyM();
            try
            {
                obj = BL_tblPropertyM.GetSingleRecordById(id);
                obj.sviewType = "EditMaster";
                obj.StateList = BL_tblStateM.StateList(obj.iStateId, obj.iCountryId);
                obj.CityList = BL_tblCityM.CityList(obj.iCityId, obj.iStateId, obj.iCountryId);
                obj.AreaList = BL_tblMacroAreaM.AreasList(obj.iAreaId, obj.iCityId);
                obj.LocalityList = BL_Locality.LocalityList(obj.iLocalityId, obj.iAreaId);
                obj.latitude = obj.dLatitude;
                obj.longitude = obj.dLongitude;
                obj.Mode = "Edit";
                obj.sInitialHotelName = obj.sHotelName;

            }
            catch (Exception ex)
            {
                throw;
            }
            return View("Index", obj);
        }

        public JsonResult GetLocations(string txt, int cityId)
        {
            List<PNames> data = BL_Locality.MacroLocalitiesAjaxCall(txt, cityId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ValidateHotelName(string sHotelName, string InitialHotelName)
        //{
        //    bool ifNotExist = true;
        //    if (sHotelName == InitialHotelName)
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    ifNotExist = BL_tblPropertyM.CheckDataExist(sHotelName);

        //    return Json(ifNotExist, JsonRequestBehavior.AllowGet);

        //}
        public JsonResult ValidateYearBuilt(short iYearBuilt)
        {
            bool ifNotTrue = true;
            short CurrentYear = Convert.ToInt16(DateTime.Now.Year.ToString());

            if (iYearBuilt > CurrentYear)
            {
                return Json("Year of Built should not be greater than current year", JsonRequestBehavior.AllowGet);
                //return Json("Year of Built should be less than current year", JsonRequestBehavior.AllowGet);
            }
            else if (iYearBuilt < 1500)
            {
                ifNotTrue = false;
                //return Json("Year of Built should be less than current year", JsonRequestBehavior.AllowGet);
            }
            return Json(ifNotTrue, JsonRequestBehavior.AllowGet);


        }
        public JsonResult ValidateLastRenovation(short iLastRenovation)
        {
            bool ifNotTrue = true;
            short CurrentYear = Convert.ToInt16(DateTime.Now.Year.ToString());

            if (iLastRenovation > CurrentYear)
            {
                ifNotTrue = false;
                //return Json("Year of Built should be less than current year", JsonRequestBehavior.AllowGet);
            }
            return Json(ifNotTrue, JsonRequestBehavior.AllowGet);


        }
        public JsonResult BindMacroArea(string id)
        {
            int cid = int.Parse(id);
            List<eMacroAreaM> results = BL_tblMacroAreaM.GetAllRecordsById(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult BindLocality(string id)
        {
            int cid = int.Parse(id);
            List<etblLocality> results = BL_Locality.LocalityList(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Modify(etblPropertyM_User prop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        prop.dtActionDate = DateTime.Now;
                        //get all AccessbilityId comma seperated
                        if (prop.SelectedAccessbility != null)
                        {
                            prop.sAccessbilityIds = prop.SelectedAccessbility.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all AwardsIds comma seperated
                        if (prop.SelectedAwards != null)
                        {
                            prop.sAwardIds = prop.SelectedAwards.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all AffilationsIds comma seperated
                        if (prop.SelectedAffilations != null)
                        {
                            prop.sAffiliationIds = prop.SelectedAffilations.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all languages comma seperated
                        if (prop.SelectedLanguages != null)
                        {
                            prop.sLanguageIds = prop.SelectedLanguages.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        if (prop.SelectPrimaryLocalities != null)
                        {
                            JArray jArray = (JArray)JsonConvert.DeserializeObject(prop.SelectPrimaryLocalities.Replace("\\", "\""));
                            if (jArray != null)
                            {
                                List<etblPropertyLocalityMap> lstPropertyLocalityMap = new List<etblPropertyLocalityMap>();
                                foreach (var item in jArray)
                                {
                                    lstPropertyLocalityMap.Add(new etblPropertyLocalityMap()
                                    {
                                        iPropId = prop.iPropId,
                                        iAreaLocalityId = Convert.ToInt32(item["Id"]),
                                        cAreaLocality = Convert.ToString(item["Status"]),
                                        dtActionDate = DateTime.Now,
                                        iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId
                                    });
                                }
                                prop.PropertyLocalityMapList = lstPropertyLocalityMap;
                            }
                        }
                        prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        int result = BL_tblPropertyM.UpdateRecord_User(prop);
                        if (result == 1)
                        {
                            TempData["msg"] = "Property Informaion Modified Successfully";
                            return RedirectToAction("Edit");
                        }
                        else
                        {
                            TempData["Error"] = "Some unknown error had happen !";
                            return View(prop.sviewType, prop);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                prop.AccessbilityItems = BL_tblAccessbilityM.GetAccessbilities(prop.sAccessbilityIds);
                prop.AwardsItems = BL_tblAwardM.GetAwards(prop.sAwardIds);
                prop.AffilationItems = BL_tblAffiliationM.GetAffilations(prop.sAffiliationIds);
                prop.LanguagesItems = BL_tblLanguageM.GetLanguages(prop.sLanguageIds);
                prop.StateList = BL_tblStateM.StateList(prop.iStateId, prop.iCountryId);
                prop.CityList = BL_tblCityM.CityList(prop.iCityId, prop.iStateId, prop.iCountryId);
                prop.AreaList = BL_tblMacroAreaM.AreasList(prop.iAreaId, prop.iCityId);
                prop.LocalityList = BL_Locality.LocalityList(prop.iLocalityId, prop.iAreaId);
                //prop.sAccessbilityIds = prop.SelectedAccessbility.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                ////get all AwardsIds comma seperated
                //prop.sAwardIds = prop.SelectedAwards.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                ////get all AffilationsIds comma seperated
                //prop.sAffiliationIds = prop.SelectedAffilations.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                ////get all languages comma seperated
                //prop.sLanguageIds = prop.SelectedLanguages.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                //for only showing purpose
                prop.sHotelNameO = prop.sHotelName;
                prop.iChainIdO = prop.iChainId;
                prop.iCountryIdO = prop.iCountryId;
                prop.iStateIdO = prop.iStateId;
                prop.iCityIdO = prop.iCityId;
                prop.iAreaIdO = prop.iAreaId;
                prop.iLocalityIdO = prop.iLocalityId;
                ViewBag.PropertyLocalityMap = GetLocalitiesJson(BL_tblPropertyM.GetPropertyLocalityMap(prop.iPropId));
                TempData["Error"] = "Validation failed ! Please correct the errors and try again.";
            }
            catch (Exception)
            {

            }
            return View(prop.sviewType, prop);
        }
        public JsonResult BindStates(string id)
        {
            int cid = int.Parse(id);

            List<etblStateM> results = BL_tblStateM.GetAllRecordsById(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult BindCity(string id)
        {
            int cid = int.Parse(id);
            List<etblCityM> results = BL_tblCityM.GetAllRecordsById(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        [NonAction]
        public string GetLocalitiesJson(List<etblPropertyLocalityMap> listPropertyLocalityMap)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            if (listPropertyLocalityMap != null && listPropertyLocalityMap.Count > 0)
            {
                jsonBuilder.Append("[");
                foreach (var item in listPropertyLocalityMap)
                {
                    jsonBuilder.Append("{");
                    jsonBuilder.Append("\"Id\":\"" + item.iAreaLocalityId + "\",");
                    jsonBuilder.Append("\"Name\":\"" + item.cAreaLocalityName + "\",");
                    jsonBuilder.Append("\"Status\":\"" + item.cAreaLocality + "\",");
                    jsonBuilder.Append("\"RowId\":\"" + String.Concat("deleteRow_", item.iAreaLocalityId, "|", item.cAreaLocality) + "\"");
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Append("]");
            }
            return jsonBuilder.ToString().Replace(",]", "]");
        }


        [Authorize]
        public ActionResult MappedHotels()
        {
            if (Session["UserDetails"] != null)
            {
                int iUserID = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                ViewData["UserId"] = iUserID;
                List<eUsersPropertyMap> LEU = BL_tblPropertyM.GetListForSingle(iUserID);
                if (LEU.Count == 1) //if (LEU.Count == 1 && Session["PropId"] == null)
                {
                    return RedirectToAction("ViewDashboard", "Property", new { PropId = LEU[0].iPropId.ToString(), PropName = LEU[0].sHotelName });
                }
            }
            Session["PropId"] = null;
            return View();
        }
        public string UpdateStatus(int Id, bool Mode)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblPropertyM obj = new etblPropertyM();
                obj = BL_tblPropertyM.GetSingleRecordById(Id);
                if (Mode == true)
                {
                    obj.cStatus = "A";
                }
                else
                {
                    obj.cStatus = "D";
                }
                int i = BL_tblPropertyM.UpdateStatus(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Updated successfully." };
                }
                else
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        [Authorize]
        public ActionResult AuthPendingNego(int PropId, string PropName)
        {
            Session["MenuType"] = "2";
            Session["PropId"] = PropId;
            Session["CurrCode"] = BL_Currency.GetCurrencyByPropId(PropId);
            //Session["Symbol"] = BL_Currency.GetSymbolByPropId(PropId);
            Session["Flag"] = BL_Currency.GetFlagByPropId(PropId);
            Session["PropName"] = PropName;
            //return RedirectToAction("ViewDashboard1", new { PropId = PropId });
            return RedirectToAction("Index", "PendingNegotiations");
        }

    }
}
