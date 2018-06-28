using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace OneFineRateApp.Controllers.Property
{
    public class PropertyAmenitiesController : BaseController
    {
        // GET: PropertyAmenities
        [Route("PropertyAmenities")]
        public ActionResult Index()
        {
            etblPropertyAmenitiesMap obj = new etblPropertyAmenitiesMap();
            try
            {
                int id = Convert.ToInt32(Session["PropId"]);
                obj = BL_tblPropertyAmenitiesMap.GetSingleRecordById(id);
                obj.iPropId = id;
                obj.CommonServicesItems = BL_tblHotelCommonServiceM.GetPropertyCommonServices(obj.sHotelCommonServices);
                obj.ConveniencesItems = BL_tblHotelConvenienceM.GetPropertyConveniences(obj.sHotelConvenience);
                obj.LeisureItems = BL_tblHotelLeisureM.GetPropertyLeisure(obj.sHotelLeisure);
                obj.MeetingsItems = BL_tblHotelMeetingM.GetPropertyMeetings(obj.sHotelMeetings);
                obj.HotelAmenities = BL_tblHotelAmenityM.GetHotelAmenities();
                obj.HotelParkingsRadio = BL_tblHotelParkingM.GetHotelParkingsRadio();
                obj.HotelParkingsItems = BL_tblHotelParkingM.GetHotelParkingsCheckBox(obj.sHotelParkings);
                //obj.bAirportTransferAvalible = true;
                //List<etblPropertyParkingMap> results = BL_tblPropertyAmenitiesMap.GetPropertyParkingList(id);
                //obj.JsonParkingMapData = OneFineRateAppUtil.clsUtils.ConvertToJson(results);
            }
            catch(Exception)
            {

            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Modify(etblPropertyAmenitiesMap prop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        prop.dtActionDate = DateTime.Now;
                        //get all common services comma seperated
                        if (prop.SelectedCommonServices != null)
                        {
                            prop.sHotelCommonServices = prop.SelectedCommonServices.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Convenience comma seperated
                        if (prop.SelectedConveniences != null)
                        {
                            prop.sHotelConvenience = prop.SelectedConveniences.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Leisure comma seperated
                        if (prop.SelectedLeisure != null)
                        {
                            prop.sHotelLeisure = prop.SelectedLeisure.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Meetings comma seperated
                        if (prop.SelectedMeetings != null)
                        {
                            prop.sHotelMeetings = prop.SelectedMeetings.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Hotel Parkings comma seperated
                        if (prop.SelectedHotelParkings != null)
                        {
                            prop.sHotelParkings = prop.SelectedHotelParkings.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }


                        if (prop.SelectedParkings != null)
                        {
                            JArray jArray = (JArray)JsonConvert.DeserializeObject(prop.SelectedParkings.Replace("\\", "\""));
                            if (jArray != null)
                            {
                                List<etblPropertyParkingMap> lstPropertyParkingMap = new List<etblPropertyParkingMap>();
                                foreach (var item in jArray)
                                {
                                    lstPropertyParkingMap.Add(new etblPropertyParkingMap()
                                    {
                                        iPropId = prop.iPropId,
                                        sCarName = Convert.ToString(item["Name"]),
                                        cAirportRail = Convert.ToString(item["Type"]),
                                        bIsFree = Convert.ToBoolean(item["comp"]),
                                        dOnewayCharge = Convert.ToDecimal(item["oneway"]),
                                        dTwowayCharge = Convert.ToDecimal(item["twoway"]),
                                        dtActionDate = DateTime.Now,
                                        iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId
                                    });
                                }
                                prop.HotelParkingsMapList = lstPropertyParkingMap;
                            }
                        }
                        prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                        int result = BL_tblPropertyAmenitiesMap.UpdateRecord(prop);
                        if (result == 1)
                        {
                            TempData["msg"] = "Amenities/Services Modified Successfully";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return RedirectToAction("Index");
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

            }
            return View("Edit", prop);
        }
        [NonAction]
        public string GetLocalitiesJson(List<etblPropertyParkingMap> listPropertyParkingMap)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            if (listPropertyParkingMap != null && listPropertyParkingMap.Count > 0)
            {
                jsonBuilder.Append("[");
                foreach (var item in listPropertyParkingMap)
                {
                    jsonBuilder.Append("{");
                    jsonBuilder.Append("\"Name\":\"" + item.sCarName + "\",");
                    jsonBuilder.Append("\"Type\":\"" + item.cAirportRail + "\",");
                    jsonBuilder.Append("\"oneway\":\"" + item.dOnewayCharge == "0" ? "" : item.dOnewayCharge + "\",");
                    jsonBuilder.Append("\"twoway\":\"" + item.dTwowayCharge == "0" ? "" : item.dTwowayCharge + "\",");
                    jsonBuilder.Append("\"comp\":\"" + item.bIsFree + "\",");
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Append("]");
            }
            return jsonBuilder.ToString().Replace(",]", "]");
        }
       
        public ActionResult AddPartial()
        {
            etblPropertyAmenitiesMap obj = new etblPropertyAmenitiesMap();
            int id = Convert.ToInt32(Session["PropId"]);
            obj = BL_tblPropertyAmenitiesMap.GetSingleRecordById(id);
            obj.iPropId = id;
            List<etblPropertyParkingMap> results = BL_tblPropertyAmenitiesMap.GetPropertyParkingList(id);
            obj.JsonParkingMapData = OneFineRateAppUtil.clsUtils.ConvertToJson(results);
            return PartialView("PropertyParking", obj);
        }
    }
}