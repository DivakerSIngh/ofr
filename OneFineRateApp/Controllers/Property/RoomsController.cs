using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;

namespace OneFineRateApp.Controllers.Property
{
    public class RoomsController : BaseController
    {
        etblPropertyRoomMap obj = new etblPropertyRoomMap();
        etblPropertyRoomAmentiesMap objnew = new etblPropertyRoomAmentiesMap();
        etblPropertyRoomTypeRoomAmentiesMap objRoomType = new etblPropertyRoomTypeRoomAmentiesMap();
        // GET: Rooms
        [Route("Rooms")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewRoom()
        {
            obj.Mode = "Add";
            obj.AccessibilityItems = BL_tblRoomAccessibilityM.GetRoomAccessibility(obj.sRoomAccessibilityIds);
            obj.BuildingcharacteristicsItems = BL_tblBuildingCharacteristicsM.GetBuildingCharacteristics(obj.sBuildingCharacteristicsIds);
            obj.OutdoorViewItems = BL_tblRoomOutdoorViewM.GetRoomOutdoorViews(obj.sRoomOutdoorViewIds);
            ViewBag.HeaderText = "Add Room";
            return PartialView("_PropertyRoom", obj);
        }
        [HttpPost]
        public ActionResult Add(etblPropertyRoomMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    decimal? sqft = 0, sqmtr = 0;
                    sqft = eObj.dSizeSqft;
                    sqmtr = eObj.dSizeMtr;

                    if (eObj.SizeType == "Sq.Mtr")
                    {
                        eObj.dSizeSqft = sqmtr;
                        eObj.dSizeMtr = sqft;
                    }
                    //get all room accessbility comma seperated
                    if (eObj.SelectedAccessibility != null)
                    {
                        eObj.sRoomAccessibilityIds = eObj.SelectedAccessibility.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    //get all room building characteristics comma seperated
                    if (eObj.SelectedBuildingcharacteristics != null)
                    {
                        eObj.sBuildingCharacteristicsIds = eObj.SelectedBuildingcharacteristics.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    //get all room building characteristics comma seperated
                    if (eObj.SelectedOutdoorView != null)
                    {
                        eObj.sRoomOutdoorViewIds = eObj.SelectedOutdoorView.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    eObj.dtActionDate = DateTime.Now;
                    eObj.bActive = true;
                    eObj.sSizeType = eObj.SizeType;

                    if (eObj.Mode == "Add")
                    {
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        int j = BL_tblPropertyRoomMap.AddRecord(eObj);
                        if (j == 1)
                        {
                            result = new { st = 1, msg = "Added Successfully ." };
                        }
                        else if (j == 2)
                        {
                            result = new { st = 0, msg = "Room Code already exists" };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }
                    }
                    else
                    {
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        int i = BL_tblPropertyRoomMap.UpdateRecord(eObj);
                        if (i == 1)
                        {
                            result = new { st = 1, msg = "Updated Successfully ." };
                        }
                        else if (i == 2)
                        {
                            result = new { st = 0, msg = "Room Code already exists" };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }

                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int Id)
        {
            ViewBag.HeaderText = "Modify Room";
            obj = BL_tblPropertyRoomMap.GetSingleRecordById(Id);
            obj.Mode = "Edit";
            obj.SizeType = obj.sSizeType;
            decimal ft = (decimal)obj.dSizeSqft;
            decimal mt = (decimal)obj.dSizeMtr;
            if (obj.SizeType == "Sq.Mtr")
            {
                obj.dSizeSqft = mt;
                obj.dSizeMtr = ft;
            }
            else
            {
                obj.dSizeSqft = ft;
                obj.dSizeMtr = mt;
            }
            
            obj.AccessibilityItems = BL_tblRoomAccessibilityM.GetRoomAccessibility(obj.sRoomAccessibilityIds);
            obj.BuildingcharacteristicsItems = BL_tblBuildingCharacteristicsM.GetBuildingCharacteristics(obj.sBuildingCharacteristicsIds);
            obj.OutdoorViewItems = BL_tblRoomOutdoorViewM.GetRoomOutdoorViews(obj.sRoomOutdoorViewIds);
            return PartialView("_PropertyRoom", obj);
        }
        public string UpdateStatus(int Id, bool Mode)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                obj = BL_tblPropertyRoomMap.GetSingleRecordById(Id);
                obj.bActive = Mode ? true : false;
                int i = BL_tblPropertyRoomMap.UpdateRecord(obj);
                if (i == 1)
                {
                    if(Mode)
                    {
                        result = new { st = 1, msg = "Room enabled Successfully ." };
                    }
                    else
                    {
                        result = new { st = 1, msg = "Room disabled Successfully ." };
                    }
                    
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
        public ActionResult CommonAmenties()
        {
            
            try
            {
                objnew = BL_tblPropertyRoomAmentiesMap.GetSingleRecordById(Convert.ToInt32(Session["PropId"]));

                objnew.RoomAnenitiesRadio = BL_tblHotelRoomAmenityM.GetHotelRoomAmenityRadio();
                objnew.iPropId = Convert.ToInt32(Session["PropId"]);
                objnew.BasicRoomAmenitiesItems = BL_tblHotelRoomAmenityM.GetHotelRoomAmenityCheckBox(objnew.sBasicRoomAmenities);
                objnew.AdditionalAmenitiesItems = BL_tblHotelRoomAmenityAdditionalM.GetHotelRoomAmenityAdditional(objnew.sAdditionalRoomAmenities);
                objnew.BathroomAmenitiesItems = BL_tblHotelRoomAmenityBathRoomM.GetHotelRoomAmenityBathRoom(objnew.sBathRoomAmenities);
                objnew.BeddingandLinensItems = BL_tblHotelRoomAmenityBeddingM.GetHotelRoomAmenityBedding(objnew.sBeddingRoomAmenities);

            }
            catch (Exception)
            {

            }
             return View("CommonAmenities", objnew);
        }
        [HttpPost]
        public ActionResult ModifyCommonAmenities(etblPropertyRoomAmentiesMap obj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        obj.dtActionDate = DateTime.Now;
                        //get all basic rooms amenities comma seperated
                        if (obj.SelectedBasicRoomAmenities != null)
                        {
                            obj.sBasicRoomAmenities = obj.SelectedBasicRoomAmenities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Additional amenities comma seperated
                        if (obj.SelectedAdditionalAmenities != null)
                        {
                            obj.sAdditionalRoomAmenities = obj.SelectedAdditionalAmenities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all bathroom amenities comma seperated
                        if (obj.SelectedBathroomAmenities != null)
                        {
                            obj.sBathRoomAmenities = obj.SelectedBathroomAmenities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Bedding and Linens comma seperated
                        if (obj.SelectedBeddingandLinens != null)
                        {
                            obj.sBeddingRoomAmenities = obj.SelectedBeddingandLinens.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                    
                        int j = BL_tblPropertyRoomAmentiesMap.UpdateRecord(obj);
                        if (j == 1)
                        {
                           
                            result = new { st = 1, msg = "Common Room Amenities Modified Successfully." };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RoomAmenties(string id, string name)
        {
            try
            {
                objRoomType = BL_tblPropertyRoomTypeRoomAmentiesMap.GetSingleRecordById(Convert.ToInt32(id));

                //get master commaon Amenities Data
                objnew = BL_tblPropertyRoomAmentiesMap.GetSingleRecordById(Convert.ToInt32(Session["PropId"]));

                //--Get Ids that would be disabled---------------------
                if (objnew.sBasicRoomAmenities != null)
                {
                    objRoomType.DisabledBasicRoomAmenities = objnew.sBasicRoomAmenities.Trim().Split(',');
                }
                if (objnew.sAdditionalRoomAmenities != null)
                {
                    objRoomType.DisabledAdditionalAmenities = objnew.sAdditionalRoomAmenities.Trim().Split(',');
                }
                if (objnew.sBathRoomAmenities != null)
                {
                    objRoomType.DisabledBathroomAmenities = objnew.sBathRoomAmenities.Trim().Split(',');
                }
                if (objnew.sBeddingRoomAmenities != null)
                {
                    objRoomType.DisabledBeddingandLinens = objnew.sBeddingRoomAmenities.Trim().Split(',');
                }
                //-----------------------------------------------

                objRoomType.RoomAnenitiesRadio = BL_tblHotelRoomAmenityM.GetHotelRoomAmenityRadio();
                objRoomType.iPropId = Convert.ToInt32(Session["PropId"]);
                objRoomType.iRoomId = Convert.ToInt32(id);
                objRoomType.BasicRoomAmenitiesItems = BL_tblHotelRoomAmenityM.GetHotelRoomAmenityCheckBox(objRoomType.sBasicRoomAmenities, objnew.sBasicRoomAmenities);
                objRoomType.AdditionalAmenitiesItems = BL_tblHotelRoomAmenityAdditionalM.GetHotelRoomAmenityAdditional(objRoomType.sAdditionalRoomAmenities, objnew.sAdditionalRoomAmenities);
                objRoomType.BathroomAmenitiesItems = BL_tblHotelRoomAmenityBathRoomM.GetHotelRoomAmenityBathRoom(objRoomType.sBathRoomAmenities, objnew.sBathRoomAmenities);
                objRoomType.BeddingandLinensItems = BL_tblHotelRoomAmenityBeddingM.GetHotelRoomAmenityBedding(objRoomType.sBeddingRoomAmenities, objnew.sBeddingRoomAmenities);
                objRoomType.iCommonBasicAmenitieRadio = objnew.iBasicAmentiesRadio;
                objRoomType.sRoomName = name;

            }
            catch(Exception)
            {

            }
            return View("Amenities", objRoomType);
        }
        [HttpPost]
        public ActionResult ModifyRoomAmenities(etblPropertyRoomTypeRoomAmentiesMap obj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        obj.dtActionDate = DateTime.Now;
                        //get all basic rooms amenities comma seperated
                        if (obj.SelectedBasicRoomAmenities != null)
                        {
                            obj.sBasicRoomAmenities = obj.SelectedBasicRoomAmenities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Additional amenities comma seperated
                        if (obj.SelectedAdditionalAmenities != null)
                        {
                            obj.sAdditionalRoomAmenities = obj.SelectedAdditionalAmenities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all bathroom amenities comma seperated
                        if (obj.SelectedBathroomAmenities != null)
                        {
                            obj.sBathRoomAmenities = obj.SelectedBathroomAmenities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Bedding and Linens comma seperated
                        if (obj.SelectedBeddingandLinens != null)
                        {
                            obj.sBeddingRoomAmenities = obj.SelectedBeddingandLinens.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }

                        int j = BL_tblPropertyRoomTypeRoomAmentiesMap.UpdateRecord(obj);
                        if (j == 1)
                        {

                            result = new { st = 1, msg = "Room Amenities Modified Successfully." };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}