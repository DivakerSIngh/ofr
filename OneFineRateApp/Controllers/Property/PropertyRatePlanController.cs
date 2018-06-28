using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;
using System.Linq.Expressions;
using Newtonsoft.Json;
using OneFineRateAppUtil;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
namespace OneFineRateApp.Controllers.Property
{
    [Authorize]
    public class PropertyRatePlanController : BaseController
    {
        // GET: PropertyRatePlan
        [Route("PropertyRatePlan")]
        public ActionResult Index()
        {
            etblPropertyRatePlanMap obj = new etblPropertyRatePlanMap();
            TempData["PropID"] = Convert.ToInt32(Session["PropId"].ToString());
            return View(obj);
        }

        public ActionResult Save(etblPropertyRatePlanMap prop)
        {
            string strReturn = string.Empty;
            int val = 0;
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    return Json(new { st = 0, msg = error.ErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            if (ModelState.IsValid)
            {
                if (prop.bLinkToExistingRatePlan == true && prop.dValue == null)  //|| prop.dValue == 0
                {
                    return Json(new { st = 0, msg = "Discount value of parent rate plan is mandatory" }, JsonRequestBehavior.AllowGet);
                }

                if (prop.iMinLengthStay > prop.iMaxLengthStay)
                {
                    return Json(new { st = 0, msg = "Minimum length of stay should be less than Maximum length of stay" }, JsonRequestBehavior.AllowGet);
                }

                if ((prop.iMinLengthStay == 0 && prop.iMaxLengthStay != 0) || (prop.iMinLengthStay != 0 && prop.iMaxLengthStay == 0))
                {
                    return Json(new { st = 0, msg = "Either both Minimum length of stay and Maximum length of stay should be zero or none should be zero." }, JsonRequestBehavior.AllowGet);
                }

                if (prop.CancellationPolicyJSonData == "[]" || prop.CancellationPolicyJSonData == null)
                {
                    return Json(new { st = 0, msg = "Please Select Cancellation Policy." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JArray jsonResponse = JArray.Parse(prop.CancellationPolicyJSonData.Replace("\\", "\""));
                    List<CancellationPolicyGrid> jArray = (List<CancellationPolicyGrid>)JsonConvert.DeserializeObject<List<CancellationPolicyGrid>>(jsonResponse.ToString());

                    #region Date Validations
                    if (clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidFrom) <= clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidTo))
                    {
                        var counter = 0;
                        foreach (var item in jArray)
                        {
                            if (clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidFrom) <= clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidTo)
                                && clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidFrom) <= clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidFrom)
                                && clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidTo) >= clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidTo))
                            { }
                            else { counter = counter + 1; }
                        }

                        // if (clsUtils.ConvertddmmyyyytoDateTime(prop.dtCancellationValidFrom) < clsUtils.ConvertddmmyyyytoDateTime(prop.dtCancellationValidTo) && clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidFrom) <= clsUtils.ConvertddmmyyyytoDateTime(prop.dtCancellationValidFrom) && clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidTo) >= clsUtils.ConvertddmmyyyytoDateTime(prop.dtCancellationValidTo))
                        if (counter == 0)
                        {
                            #region Add Rate Plan
                            if (prop.Mode == "Add")
                            {
                                val = AddRatePlan(prop);
                            }
                            #endregion

                            #region Copy Rate Plan
                            if (prop.Mode == "Copy")
                            {
                                val = AddRatePlan(prop);
                            }
                            #endregion

                            #region Modify Rate Plan
                            if (prop.Mode == "Modify")
                            {
                                val = EditRatePlan(prop);
                            }
                            #endregion

                            #region Message
                            if (val == 1)
                            {
                                if (prop.Mode == "Add")
                                {
                                    TempData["msg"] = "Added Successfully";
                                }
                                if (prop.Mode == "Copy")
                                {
                                    TempData["msg"] = "Copied Successfully";
                                }
                                if (prop.Mode == "Modify")
                                {
                                    TempData["msg"] = "Updated Successfully";
                                }
                            }
                            else if (val == 3)
                            {
                                return Json(new { st = 0, msg = "Selected validity should be with in range of linked Rate Plan validity." }, JsonRequestBehavior.AllowGet);
                            }
                            else if (val == 4)
                            {
                                return Json(new { st = 0, msg = "Rate Plan with same name already exists in selected validity." }, JsonRequestBehavior.AllowGet);
                            }
                            else if (val == 5)
                            {
                                return Json(new { st = 0, msg = "Validity should not be less than Previous validity of rate plan." }, JsonRequestBehavior.AllowGet);
                            }
                            else if (val == 6)
                            {
                                return Json(new { st = 0, msg = "Cancellation policy for all dates must be added in selected validity range" }, JsonRequestBehavior.AllowGet);
                            }



                            #endregion
                        }
                        else
                        {
                            return Json(new { st = 0, msg = "Cancellation policy validity should be with in range of rate plan validity." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { st = 0, msg = "Valid From Date Can't be greater than Valid To Date." }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                }

            }
            else
            {
                if (prop.SelectedRoomType == null)
                {
                    return Json(new { st = 0, msg = "Please Select atleast one room." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { st = 0, msg = "Kindly try after some time." }, JsonRequestBehavior.AllowGet);
                }


            }
            return Json(new { st = 1, msg = true }, JsonRequestBehavior.AllowGet);
        }


        public int AddRatePlan(etblPropertyRatePlanMap prop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    prop.iPropId = Convert.ToInt32(Session["PropId"].ToString());
                    prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                    prop.dtActionDate = DateTime.Now;
                    prop.bIsPlus = Convert.ToBoolean(prop.iIsPlus);
                    prop.bIsPercent = Convert.ToBoolean(prop.iIsPercent);
                    prop.bIsBefore = Convert.ToBoolean(prop.iIsBefore);
                    prop.bIsSecretDeal = Convert.ToBoolean(prop.bIsSecretDeal);

                    JArray jsonResponse = JArray.Parse(prop.CancellationPolicyJSonData.Replace("\\", "\""));
                    List<CancellationPolicyGrid> jArray = (List<CancellationPolicyGrid>)JsonConvert.DeserializeObject<List<CancellationPolicyGrid>>(jsonResponse.ToString());

                    // prop.iLinkRatePlanId = prop.iRPId;

                    prop.cStatus = "A";
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedRoomType != null)
                    {
                        prop.sRoomTypeId = prop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedAmenityID != null)
                    {
                        prop.sAmenityId = prop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    //if (prop.SelectedCancellationPolicy != null)
                    //{
                    //    prop.sCancellationPolicy = prop.SelectedCancellationPolicy.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    //}
                    if (prop.bLinkToExistingRatePlan == false)
                    {
                        prop.iLinkRatePlanId = null;
                        prop.bIsPlus = null;
                        prop.bIsPercent = null;
                        prop.dValue = null;
                    }
                    int result = BL_tblPropertyRatePlanMap.AddRecord(prop, jArray);
                    return result;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EditRatePlan(etblPropertyRatePlanMap prop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    prop.iPropId = Convert.ToInt32(Session["PropId"].ToString());
                    prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                    prop.dtActionDate = DateTime.Now;
                    prop.bIsPlus = Convert.ToBoolean(prop.iIsPlus);
                    prop.bIsPercent = Convert.ToBoolean(prop.iIsPercent);
                    prop.bIsBefore = Convert.ToBoolean(prop.iIsBefore);
                    prop.bIsSecretDeal = Convert.ToBoolean(prop.bIsSecretDeal);
                    prop.iRPId = prop.iUpdateRPId;
                    prop.cStatus = "A";

                    JArray jsonResponse = JArray.Parse(prop.CancellationPolicyJSonData.Replace("\\", "\""));
                    List<CancellationPolicyGrid> jArray = (List<CancellationPolicyGrid>)JsonConvert.DeserializeObject<List<CancellationPolicyGrid>>(jsonResponse.ToString());

                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedRoomType != null)
                    {
                        prop.sRoomTypeId = prop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedAmenityID != null)
                    {
                        prop.sAmenityId = prop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    //if (prop.SelectedCancellationPolicy != null)
                    //{
                    //    prop.sCancellationPolicy = prop.SelectedCancellationPolicy.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + ", " + s2);
                    //}
                    if (prop.bLinkToExistingRatePlan == false)
                    {
                        prop.iLinkRatePlanId = null;
                        prop.bIsPlus = null;
                        prop.bIsPercent = null;
                        prop.dValue = null;
                    }
                    int result = BL_tblPropertyRatePlanMap.UpdateRecord(prop, jArray);
                    return result;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public ActionResult Add()
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            TempData["PropID"] = Convert.ToInt32(Session["PropId"].ToString());
            etblPropertyRatePlanMap obj = new etblPropertyRatePlanMap();
            obj.RoomTypeItems = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(PropId, 0);
            obj.Amenties = BL_tblPropertyRatePlanMap.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPropertyRatePlanMap.GetCancellationPolicy(PropId);
            obj.iCancellationChkBox = obj.CancellationPolicy.Count();
            obj.iMinLengthStay = 1;
            obj.Mode = "Add";
            Session["Mode"] = "Add";
            obj.heading = "Add Rate Plan";
            obj.iPropId = PropId;
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(obj.CancellationPolicyGrid);
            ViewData["cancellationJSON"] = json;

            return PartialView("pvRatePlan", obj);
        }
        public ActionResult Copy(int Id)
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            etblPropertyRatePlanMap obj = new etblPropertyRatePlanMap();
            obj = BL_tblPropertyRatePlanMap.GetAllRatePlansByID(Id);
            obj.Mode = "Copy";
            Session["Mode"] = "Copy";
            Session["RecId"] = Id;
            obj.heading = "Add Rate Plan";
            obj.RoomTypeItems = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Id, PropId, obj.iLinkRatePlanId, true );
            obj.Amenties = BL_tblPropertyRatePlanMap.GetAmentiesCheckBox(obj.sAmenityId);
            //obj.CancellationPolicy = BL_tblPropertyRatePlanMap.GetCancellationPolicy(PropId);
            obj.CancellationPolicy = BL_tblPropertyRatePlanMap.GetCancellationPolicy(PropId, obj.sCancellationPolicy);
            obj.iCancellationChkBox = obj.CancellationPolicy.Count();
            obj.iUpdateRPId = Id;
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(obj.CancellationPolicyGrid);
            ViewData["cancellationJSON"] = json;
            return PartialView("pvRatePlan", obj);
        }
        public ActionResult Update(int Id)
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            etblPropertyRatePlanMap obj = new etblPropertyRatePlanMap();
            obj = BL_tblPropertyRatePlanMap.GetAllRatePlansByID(Id);
            obj.Mode = "Modify";
            Session["Mode"] = "Modify";
            Session["RecId"] = Id;
            obj.heading = "Modify Rate Plan";
            obj.RoomTypeItems = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Id, PropId, obj.iLinkRatePlanId,  true );
            obj.Amenties = BL_tblPropertyRatePlanMap.GetAmentiesCheckBox(obj.sAmenityId);
            obj.CancellationPolicy = BL_tblPropertyRatePlanMap.GetCancellationPolicy(PropId, obj.sCancellationPolicy);
            obj.iCancellationChkBox = obj.CancellationPolicy.Count();
            obj.iUpdateRPId = Id;
            obj.iIsPlus = Convert.ToInt32(obj.bIsPlus);
            obj.iIsPercent = Convert.ToInt32(obj.bIsPercent);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(obj.CancellationPolicyGrid);
            ViewData["cancellationJSON"] = json;

            return PartialView("pvRatePlan", obj);
        }



        public ActionResult Delete(int Id)
        {
            string strReturn = string.Empty;
            try
            {
                etblPropertyRatePlanMap prop = new etblPropertyRatePlanMap();
                prop = BL_tblPropertyRatePlanMap.GetAllRatePlansByID(Id);
                if (prop.cStatus == "A") { prop.cStatus = "I"; }
                else if (prop.cStatus == "I") { prop.cStatus = "A"; }

                int val = BL_tblPropertyRatePlanMap.DeleteRecord(prop);
                if (val == 1)
                {
                    if (prop.cStatus == "I")
                    {
                        TempData["msg"] = "Disabled successfully ";
                    }
                    else
                    {
                        TempData["msg"] = "Enabled successfully";
                    }

                }
                else
                {
                    TempData["msg"] = "Kindly try after some time";
                }

            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            var result = new List<KeyValuePair<string, string>>();
            IList<RPNames> List = new List<RPNames>(BL_tblPropertyRatePlanMap.GetRatePlansForTypehead(term));
            foreach (var item in List)
            {
                result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.Name));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
            //var result3 = result.ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindCancellationPolicy()
        {
            List<CheckBoxList> results = new List<CheckBoxList>();
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            if (PropId != 0)
            {
                results = BL_tblPropertyRatePlanMap.GetCancellationPolicy(PropId);
            }
            else
            {
                results = null;
            }
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BindRoomType(int Id, string type)
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            List<CheckBoxList> results = new List<CheckBoxList>();


            if (Session["Mode"] != null)
            {
                if (Session["Mode"].ToString() == "Add")
                {
                    if (type == "ShowBase" || type == "First")
                    {
                        results = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Id, PropId, null, false);
                    }
                    else
                    {
                        if (Id != 0)
                        {
                            results = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Id, PropId, Id, false);
                        }
                        else
                        {
                            results = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(PropId, 0);
                        }
                    }

                }

                if (Session["Mode"].ToString() == "Copy" || Session["Mode"].ToString() == "Modify")
                {
                    if (type == "First")
                    {
                        etblPropertyRatePlanMap obj = new etblPropertyRatePlanMap();
                        obj = BL_tblPropertyRatePlanMap.GetAllRatePlansByID(Convert.ToInt32(Session["RecId"].ToString()));
                        results = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Convert.ToInt32(Session["RecId"].ToString()), PropId, obj.iLinkRatePlanId, true );
                    }
                    else if (type == "RPChange")
                    {
                        results = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Id, PropId, Id, false);
                    }
                    else if (type == "ShowBase")
                    {
                        results = BL_tblPropertyRatePlanMap.GetRoomTypeCheckBox(Convert.ToInt32(Session["RecId"].ToString()), PropId, null, false);
                    }
                }
            }



            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetParentRatePlanValidity(int Id, string type)
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            List<string> results = new List<string>();
            if (Id != 0)
            {
                results = BL_tblPropertyRatePlanMap.GetParentRatePlanValidity(Id, PropId);
            }
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
    }
}