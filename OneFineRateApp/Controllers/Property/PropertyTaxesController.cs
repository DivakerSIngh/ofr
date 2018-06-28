using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using OneFineRateAppUtil;

namespace OneFineRateApp.Controllers.Property
{
    public class PropertyTaxesController : BaseController
    {
        etblPropertyTaxMap obj = new etblPropertyTaxMap();
        // GET: PropertyTaxes
        [Route("PropertyTaxes")]
        public ActionResult Index()
        {
            return View(obj);
        }
        public ActionResult Add()
        {
            obj.ListTaxes = BL_Tax.GetAllTax();
            obj.ListRatePlans = BL_tblPropertyRoomMap.GetAllPropertyTypes(Convert.ToInt32(Session["PropId"]));
            obj.Mode = "ADD";
            ViewBag.heading = "Add Taxes";
            return PartialView("PropertyTax", obj);
        }
        public ActionResult Edit(int Id)
        {
            obj = BL_tblPropertyTaxMap.GetSingleRecordById(Id);
            ViewBag.heading = "Modify Taxes";
            obj.Mode = "Edit";
            if (obj.dtStayFrom != null)
            {
                obj.stayfrom = String.Format("{0:dd/MM/yyyy}", obj.dtStayFrom);
            }
            if (obj.dtStayTo != null)
            {
                obj.stayto = String.Format("{0:dd/MM/yyyy}", obj.dtStayTo);
            }
            //obj.PlanId = obj.iRoomId + "|" + obj.iRPId;
            if (obj.iRoomId == null)
                obj.iRoomId = 0;
            obj.ListRatePlans = BL_tblPropertyRoomMap.GetAllPropertyTypes(Convert.ToInt32(Session["PropId"]));
            obj.ListTaxes = BL_Tax.GetAllTaxOfProperty(Id);
            return PartialView("PropertyTax", obj);
        }

        [HttpPost]
        public ActionResult AddUpdate(etblPropertyTaxMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.iRoomId == 0)
                        eObj.iRoomId = null;

                    if (eObj.SelectedTaxes != null)
                    {
                        JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.SelectedTaxes.Replace("\\", "\""));
                        if (jArray != null)
                        {
                            List<etblPropertyTaxesMap> lstPropertyTaxesMap = new List<etblPropertyTaxesMap>();
                            foreach (var item in jArray)
                            {
                                lstPropertyTaxesMap.Add(new etblPropertyTaxesMap()
                                {
                                    iTaxId = Convert.ToInt32(item["TaxId"]),
                                    bIsPercent = Convert.ToBoolean(item["Type"]),
                                    dValue = Convert.ToDecimal(item["value"]),
                                    dtActionDate = DateTime.Now,
                                    iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId
                                });
                            }
                            eObj.PropertyTaxesList = lstPropertyTaxesMap;
                        }
                    }
                    //if (eObj.Mode == "Edit")
                    //{
                    //    bool ResultType = BL_tblPropertyTaxMap.CheckRecordExist(eObj.iPropId, eObj.iRoomId,eObj.iPropTaxId);
                    //    //var existingRecord = BL_tblPropertyTaxMap.GetSingleRecordById(eObj.iPropTaxId);

                    //    //if(string.IsNullOrEmpty(eObj.PlanId))
                    //    //{
                    //    //    if (existingRecord.iRoomId != null)
                    //    //    {
                    //    //        var ratePlanResult = new { st = 0, msg = "Tax already added / mapped with another ratePlan ." };
                    //    //        return Json(ratePlanResult, JsonRequestBehavior.AllowGet);
                    //    //    }
                    //    //}
                    //    //else
                    //    //{
                    //    if (ResultType == true)
                    //    {
                    //        //var ratePlanResult = new { st = 0, msg = "Tax already added / mapped with another ratePlan ." };
                    //        var ratePlanResult = new { st = 0, msg = "Tax already added." };
                    //        return Json(ratePlanResult, JsonRequestBehavior.AllowGet);
                    //    }
                    //    //}
                    //}

                    eObj.dtStayFrom = clsUtils.ConvertddmmyyyytoDateTime(eObj.stayfrom);
                    eObj.dtStayTo = clsUtils.ConvertddmmyyyytoDateTime(eObj.stayto);

                    int i = BL_tblPropertyTaxMap.CheckStatus(Convert.ToInt32(eObj.iPropId), Convert.ToInt32(eObj.iRPId), Convert.ToInt32(eObj.iRoomId), Convert.ToDateTime(eObj.dtStayFrom), Convert.ToDateTime(eObj.dtStayTo), Convert.ToInt32(eObj.iPropTaxId));
                    if (i > 0)
                    {
                        result = new { st = 0, msg = "Taxes for these dates has already been made." };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (eObj.Mode == "ADD")
                    {
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.dtActionDate = DateTime.Now;
                        eObj.cStatus = "A";
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        int j = BL_tblPropertyTaxMap.AddRecord(eObj);
                        if (j == 1)
                        {
                            bool bookingAffected = false;
                            var statusChange = BL_tblPropertyTaxMap.CheckAffectedBookingsAfterPropertyTaxChange(Convert.ToDateTime(eObj.dtStayFrom), Convert.ToDateTime(eObj.dtStayTo));
                            if (statusChange.Key == 1)
                            {
                                bookingAffected = true;
                            }
                            result = new { st = 1, bookingAffected = bookingAffected, msg = "Added successfully." };
                        }
                        else if (j == 2)
                        {
                            result = new { st = 0, msg = "Tax already exists for selected Rate Plan and Stay Validity" };
                        }
                        else if (j == 3)
                        {
                            result = new { st = 0, msg = "Tax already exists for single room types" };
                        }
                        else if (j == 4)
                        {
                            result = new { st = 0, msg = "Tax already exists for all rooms" };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time" };
                        }
                    }
                    else if (eObj.Mode == "Edit")
                    {
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        eObj.dtActionDate = DateTime.Now;
                        int j = BL_tblPropertyTaxMap.UpdateRecord(eObj);
                        if (j == 1)
                        {
                            bool bookingAffected = false;
                            var statusChange = BL_tblPropertyTaxMap.CheckAffectedBookingsAfterPropertyTaxChange(Convert.ToDateTime(eObj.dtStayFrom), Convert.ToDateTime(eObj.dtStayTo));
                            if (statusChange.Key == 1)
                            {
                                bookingAffected = true;
                            }
                            result = new { st = 1, bookingAffected = bookingAffected, msg = "Updated successfully." };
                        }
                        else if (j == 2)
                        {
                            result = new { st = 0, msg = "Tax already exixts for selected Rate Plan and Stay Validity" };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time" };
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

                eObj.ListRatePlans = BL_tblPropertyRatePlanMap.GetAllRatePlans(Convert.ToInt32(Session["PropId"]));
                eObj.ListTaxes = BL_Tax.GetAllTaxOfProperty(eObj.iPropTaxId);

            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time" };
            }
            //  return PartialView("PropertyTax", eObj);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public string UpdateStatus(int Id, bool Mode)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                obj = BL_tblPropertyTaxMap.GetSingleRecordById(Id);
                if (Mode == true)
                {
                    obj.cStatus = "A";
                }
                else
                {
                    obj.cStatus = "I";
                }
                int i = BL_tblPropertyTaxMap.UpdateStatus(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Updated successfully." };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = "Tax already exixts for selected Rate Plan and Stay Validity" };
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
    }
}