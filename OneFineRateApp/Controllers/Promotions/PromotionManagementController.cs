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
using OneFineRateAppUtil;
using System.Web.Script.Serialization;

namespace OneFineRateApp.Controllers.PromotionsAndDeals
{
    public class PromotionManagementController : BaseController
    {
        // GET: PromotionManagement
        [Route("PromotionManagement")]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult BasicPromotions()
        {
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            etblPropertyPromotionMap obj = new etblPropertyPromotionMap();
            obj.iPropId = id;
            Session["Entrytype"] = "N";
            Session["PromoID"] = BL_tblPromotionManagement.GetPromoID("Basic Deal");
            obj.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(id);
            obj.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(id);
            obj.iCancelationChkBox = obj.CancellationPolicy.Count();

            return View(obj);
        }
        public ActionResult MinimumLengthPromotion()
        {
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            etblPropertyPromotionMap obj = new etblPropertyPromotionMap();
            obj.iPropId = id;
            Session["Entrytype"] = "N";
            Session["PromoID"] = BL_tblPromotionManagement.GetPromoID("Minimum Stay");
            obj.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(id);
            obj.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(id);
            obj.iCancelationChkBox = obj.CancellationPolicy.Count();

            return View(obj);
        }
        public ActionResult EarlyBookerPromotion()
        {
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            etblPropertyPromotionMap obj = new etblPropertyPromotionMap();
            obj.iPropId = id;
            Session["Entrytype"] = "N";
            Session["PromoID"] = BL_tblPromotionManagement.GetPromoID("Early Booker");
            obj.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(id);
            obj.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(id);
            obj.iCancelationChkBox = obj.CancellationPolicy.Count();

            return View(obj);
        }
        public ActionResult LastMinutePromotion()
        {
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            etblPropertyPromotionMap obj = new etblPropertyPromotionMap();
            obj.iPropId = id;
            Session["Entrytype"] = "N";
            Session["PromoID"] = BL_tblPromotionManagement.GetPromoID("Last Minutes");
            obj.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(id);
            obj.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(id);
            obj.iCancelationChkBox = obj.CancellationPolicy.Count();

            return View(obj);
        }
        public ActionResult HrsPromotions()
        {
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            etblPropertyPromotionMap obj = new etblPropertyPromotionMap();
            obj.iPropId = id;
            Session["Entrytype"] = "N";
            Session["PromoID"] = BL_tblPromotionManagement.GetPromoID("24 Hrs Promotion");
            obj.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(id);
            obj.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(id);
            obj.iCancelationChkBox = obj.CancellationPolicy.Count();

            return View(obj);
        }
        public ActionResult OFR()
        {
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            etblPropertyPromotionMap obj = new etblPropertyPromotionMap();
            obj.iPropId = id;
            Session["Entrytype"] = "N";
            Session["PromoID"] = BL_tblPromotionManagement.GetPromoID("OFR Freebies");
            obj.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(id);
            obj.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();
            obj.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(id);
            obj.iCancelationChkBox = obj.CancellationPolicy.Count();
            return View(obj);
        }


        public ActionResult Save(etblPropertyPromotionMap prop)
        {
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    if (Session["ErrorMsg"] == null)
                    {
                        Session["ErrorMsg"] = error.ErrorMessage;
                    }
                    //string str;
                    //str = "a";
                    // DoSomethingWith(error);
                }
            }

            int val = 0;
            string Entrytype = HttpContext.Session["Entrytype"].ToString();
            int id = 0;
            if (Session["PropId"] != null) { id = Convert.ToInt32(Session["PropId"].ToString()); }
            if (ModelState.IsValid)
            {
                if (Session["PromoID"].ToString() == Convert.ToInt32(Promotions.OFRFreebies).ToString())
                {
                    prop.dtBookingDateFrom = clsUtils.ConvertddmmyyyytoDateTime(prop.dtBValidFrom.ToString());
                    prop.dtBookingDateTo = clsUtils.ConvertddmmyyyytoDateTime(prop.dtBValidTo.ToString());
                    prop.dtStayDateFrom = clsUtils.ConvertddmmyyyytoDateTime(prop.dtSValidFrom.ToString());
                    prop.dtStayDateTo = clsUtils.ConvertddmmyyyytoDateTime(prop.dtSValidTo.ToString());

                    #region Date Validations
                    if (prop.dtBookingDateFrom > prop.dtBookingDateTo)
                    { TempData["Error"] = "BookingFrom Date Can't be greater than BookingTo Date"; return SetData(prop, true); }

                    if (prop.dtStayDateFrom > prop.dtStayDateTo)
                    { TempData["Error"] = "StayFrom Date Can't be greater than StayTo Date"; return SetData(prop, true); }

                    if (prop.dtStayDateFrom < prop.dtBookingDateFrom)
                    { TempData["Error"] = "StayFrom Date Can't be less than BookingFrom Date"; return SetData(prop, true); }


                    #endregion
                }
                else
                {
                    prop.dtBookingDateFrom = clsUtils.ConvertddmmyyyytoDateTime(prop.dtBValidFrom.ToString());
                    prop.dtBookingDateTo = clsUtils.ConvertddmmyyyytoDateTime(prop.dtBValidTo.ToString());

                    prop.dtStayDateFrom = clsUtils.ConvertddmmyyyytoDateTime(prop.dtSValidFrom.ToString());
                    prop.dtStayDateTo = clsUtils.ConvertddmmyyyytoDateTime(prop.dtSValidTo.ToString());

                    prop.dtRPValidityFrom = clsUtils.ConvertddmmyyyytoDateTime(prop.dtRPValidFrom.ToString());
                    prop.dtRPValidityTo = clsUtils.ConvertddmmyyyytoDateTime(prop.dtRPValidTo.ToString());

                    #region Date Validations
                    if (prop.dtBookingDateFrom > prop.dtBookingDateTo)
                    { TempData["Error"] = "BookingFrom Date Can't be greater than BookingTo Date"; return SetData(prop, true); }

                    if (prop.dtStayDateFrom > prop.dtStayDateTo)
                    { TempData["Error"] = "StayFrom Date Can't be greater than StayTo Date"; return SetData(prop, true); }

                    if (prop.dtStayDateFrom < prop.dtBookingDateFrom)
                    { TempData["Error"] = "StayFrom Date Can't be less than BookingFrom Date"; return SetData(prop, true); }

                    if (prop.iMinLengthStay > prop.iMaxLengthStay)
                    { TempData["Error"] = "Minimum length of stay should be less than Maximum length of stay"; return SetData(prop, true); }

                    if ((prop.dtRPValidityFrom <= prop.dtStayDateFrom && prop.dtRPValidityTo >= prop.dtStayDateFrom) && (prop.dtRPValidityFrom <= prop.dtStayDateTo && prop.dtRPValidityTo >= prop.dtStayDateTo))
                    { }
                    else { TempData["Error"] = "Stay Validity should be within range of Rate Plan Validity"; return SetData(prop, true); }

                    if (prop.CancellationPolicyJSonData == "[]" || prop.CancellationPolicyJSonData == null)
                    {
                        TempData["Error"] = "Please Select Cancellation Policy."; return SetData(prop, true);
                    }
                    else
                    {
                        JArray jsonResponse = JArray.Parse(prop.CancellationPolicyJSonData.Replace("\\", "\""));
                        List<CancellationPolicyGrid> jArray = (List<CancellationPolicyGrid>)JsonConvert.DeserializeObject<List<CancellationPolicyGrid>>(jsonResponse.ToString());

                        var counter = 0;
                        foreach (var item in jArray)
                        {
                            if (clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidFrom) <= clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidTo)
                                && prop.dtStayDateFrom <= clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidFrom)
                                && prop.dtStayDateTo >= clsUtils.ConvertddmmyyyytoDateTime(item.CancellationValidTo))
                            { }
                            else { counter = counter + 1; }
                        }

                        if (counter > 0)
                        {
                            TempData["Error"] = "Cancellation policy validity should be with in range of stay dates."; return SetData(prop, true);
                        }
                    }


                    #endregion
                }

                // E => Edit, N=> New,  U => Update

                if (Entrytype == "E" || Entrytype == "N")
                {
                    val = Edit(prop);
                }
                else if (Entrytype == "U")
                {
                    val = Update(prop);
                }
                if (val == 1)
                {
                    return RedirectToAction("Index");
                }
                else if (val == 2)
                {
                    TempData["Error"] = "Record already exists with similar values"; return SetData(prop, true);
                }
                else if (val == 6)
                {
                    TempData["Error"] = "Cancellation policy for all dates must be added in selected stay validity range"; return SetData(prop, true);
                }
                else
                {
                    TempData["Error"] = "Please select atleast one room type"; return SetData(prop, true);
                }

            }
            else
            {
                if (Session["ErrorMsg"] != null)
                {
                    TempData["Error"] = Session["ErrorMsg"].ToString();
                    Session["ErrorMsg"] = null;
                }
                return SetData(prop, true);
            }
        }

        public int Edit(etblPropertyPromotionMap prop)
        {
            try
            {


                prop.iPropId = Convert.ToInt32(Session["PropId"].ToString());
                prop.iPromoId = Convert.ToInt32(Session["PromoID"].ToString());
                prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                prop.dtActionDate = DateTime.Now;
                prop.bIsPlus = Convert.ToBoolean(prop.iIsPlus);
                prop.bIsPercent = Convert.ToBoolean(prop.iIsPercent);
                prop.bIsSecretDeal = Convert.ToBoolean(prop.bIsSecretDeal);
                prop.cStatus = "A";
                List<CancellationPolicyGrid> jArray = new List<CancellationPolicyGrid>();
                if (prop.iPromoId == Convert.ToInt32(Promotions.OFRFreebies))
                {

                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedRoomType != null)
                    {
                        prop.sRoomTypeId = prop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedAmenityID != null)
                    {
                        prop.sAmenityId = prop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }

                }
                else
                {
                    JArray jsonResponse = JArray.Parse(prop.CancellationPolicyJSonData.Replace("\\", "\""));
                    jArray = (List<CancellationPolicyGrid>)JsonConvert.DeserializeObject<List<CancellationPolicyGrid>>(jsonResponse.ToString());


                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedRoomType != null)
                    {
                        prop.sRoomTypeId = prop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedAmenityID != null)
                    {
                        prop.sAmenityId = prop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedCancellationPolicy != null)
                    {
                        prop.sCancellationPolicy = prop.SelectedCancellationPolicy.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                }


                int result = BL_tblPropertyPromotionMap.AddRecord(prop, jArray);
                if (result == 1)
                {
                    TempData["msg"] = "Record Saved Successfully";
                    return result;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }



        }
        public int Update(etblPropertyPromotionMap prop)
        {
            try
            {
                prop.iPropId = Convert.ToInt32(Session["PropId"].ToString());
                prop.iPromoId = Convert.ToInt32(Session["PromoID"].ToString());
                prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                prop.dtActionDate = DateTime.Now;
                prop.bIsPlus = Convert.ToBoolean(prop.iIsPlus);
                prop.bIsPercent = Convert.ToBoolean(prop.iIsPercent);
                prop.cStatus = "A";
                List<CancellationPolicyGrid> jArray = new List<CancellationPolicyGrid>();
                if (prop.iPromoId == Convert.ToInt32(Promotions.OFRFreebies))
                {
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedRoomType != null)
                    {
                        prop.sRoomTypeId = prop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedAmenityID != null)
                    {
                        prop.sAmenityId = prop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                }
                else
                {
                    JArray jsonResponse = JArray.Parse(prop.CancellationPolicyJSonData.Replace("\\", "\""));
                    jArray = (List<CancellationPolicyGrid>)JsonConvert.DeserializeObject<List<CancellationPolicyGrid>>(jsonResponse.ToString());


                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedRoomType != null)
                    {
                        prop.sRoomTypeId = prop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedAmenityID != null)
                    {
                        prop.sAmenityId = prop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                    //get all Cancellation Policies comma seperated
                    if (prop.SelectedCancellationPolicy != null)
                    {
                        prop.sCancellationPolicy = prop.SelectedCancellationPolicy.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }
                }



                int result = BL_tblPropertyPromotionMap.UpdateRecord(prop, jArray);
                if (result == 1)
                {
                    TempData["msg"] = "Record Updated Successfully";
                    return result;
                }
                else
                {
                    TempData["Error"] = "Something Went Wrong Please check";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(etblPropertyPromotionMap prop)
        {
            try
            {
                string tmpStatus = prop.cStatus;
                if (prop.cStatus == "A") { prop.cStatus = "I"; }
                else if (prop.cStatus == "I") { prop.cStatus = "A"; }

                int result = BL_tblPropertyPromotionMap.DeleteRecord(prop);
                if (result == 1)
                {
                    if (tmpStatus == "A")
                    {
                        TempData["msg"] = "Record Disabled Successfully";
                    }
                    else if (tmpStatus == "I")
                    {
                        TempData["msg"] = "Record Enabled Successfully";
                    }

                }
            }
            catch (Exception) { throw; }
        }


        public ActionResult SetData(etblPropertyPromotionMap Modelprop, bool IsError)
        {
            try
            {

                if (IsError == true && Modelprop != null)
                {
                    Session["IsError"] = "True";
                    if (Session["PromoID"] != null) { Modelprop.iPromoId = Convert.ToInt32(Session["PromoID"]); }
                    Modelprop.iIsPlus = Modelprop.bIsPlus == true ? 1 : 0;
                    Modelprop.iIsPercent = Modelprop.bIsPercent == true ? 1 : 0;

                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.OFRFreebies))
                    {
                        Modelprop.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(Convert.ToInt32(Session["PropId"].ToString()));
                        Modelprop.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox();

                    }
                    else
                    {

                        if (Modelprop.SelectedRoomType != null)
                        {
                            Modelprop.sRoomTypeId = Modelprop.SelectedRoomType.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        if (Modelprop.SelectedAmenityID != null)
                        {
                            Modelprop.sAmenityId = Modelprop.SelectedAmenityID.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        Modelprop.RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(Modelprop.iPropId, Modelprop.iID, Modelprop.iRPId);
                        Modelprop.Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox(Modelprop.sAmenityId);
                        Modelprop.CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(Modelprop.iPropId, Modelprop.sCancellationPolicy);
                        Modelprop.iCancelationChkBox = Modelprop.CancellationPolicy.Count();

                    }





                    ViewData["ValiditFrom"] = Modelprop.dtRPValidFrom;
                    ViewData["ValiditTo"] = Modelprop.dtRPValidTo;


                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = Modelprop.CancellationPolicyJSonData;
                    ViewData["cancellationJSON"] = json;


                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.BasicDeal)) { return View("BasicPromotions", Modelprop); }
                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.MinimumStay)) { return View("MinimumLengthPromotion", Modelprop); }
                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.EarlyBooker)) { return View("EarlyBookerPromotion", Modelprop); }
                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.LastMinutes)) { return View("LastMinutePromotion", Modelprop); }
                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.HrsPromotion)) { return View("HrsPromotions", Modelprop); }
                    if (Modelprop.iPromoId == Convert.ToInt32(Promotions.OFRFreebies)) { return View("OFR", Modelprop); }
                    else { return View("../PromotionManagement"); }
                }
                else
                {
                    List<etblPropertyPromotionMap> prop = new List<etblPropertyPromotionMap>();
                    if (HttpContext.Request.Params["Entrytype"] != null) { Session["Entrytype"] = HttpContext.Request.Params["Entrytype"]; }
                    if (HttpContext.Request.Params["promo"] != null) { Session["PromoID"] = Convert.ToInt32(HttpContext.Request.Params["promo"]); }
                    if (HttpContext.Request.Params["id"] != null) { Session["id"] = Convert.ToInt32(HttpContext.Request.Params["id"]); }

                    int a = Convert.ToInt32(Session["id"]) == null ? 0 : Convert.ToInt32(Session["id"]);

                    if (a > 0)
                    {
                        prop = BL_tblPromotionManagement.getPropertyPromoDataByID(a);
                        prop[0].iIsPlus = prop[0].bIsPlus == true ? 1 : 0;
                        prop[0].iIsPercent = prop[0].bIsPercent == true ? 1 : 0;

                        prop[0].dtBValidFrom = prop[0].dtBookingDateFrom.ToString("dd/MM/yyyy");
                        prop[0].dtBValidTo = prop[0].dtBookingDateTo.ToString("dd/MM/yyyy");
                        prop[0].dtSValidFrom = prop[0].dtStayDateFrom.ToString("dd/MM/yyyy");
                        prop[0].dtSValidTo = prop[0].dtStayDateTo.ToString("dd/MM/yyyy");


                        if (Convert.ToInt32(Session["PromoID"].ToString()) == Convert.ToInt32(Promotions.OFRFreebies))
                        {
                            prop[0].RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(prop[0].iPropId, prop[0].iID);
                            prop[0].Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox(prop[0].sAmenityId);

                        }
                        else
                        {
                            prop[0].RoomTypeItems = BL_tblPromotionManagement.GetRoomTypeCheckBox(prop[0].iPropId, prop[0].iID, prop[0].iRPId);
                            prop[0].Amenties = BL_tblPromotionManagement.GetAmentiesCheckBox(prop[0].sAmenityId);
                            prop[0].CancellationPolicy = BL_tblPromotionManagement.GetCancellationPolicy(prop[0].iPropId, prop[0].sCancellationPolicy);
                            prop[0].iCancelationChkBox = prop[0].CancellationPolicy.Count();
                        }




                        ViewData["ValiditFrom"] = prop[0].dtRPValidFrom;
                        ViewData["ValiditTo"] = prop[0].dtRPValidTo;


                        var jsonSerialiser = new JavaScriptSerializer();
                        var json = jsonSerialiser.Serialize(prop[0].CancellationPolicyGrid);
                        ViewData["cancellationJSON"] = json;


                        if (Session["Entrytype"].ToString() == "D")
                        {
                            Delete(prop[0]);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (prop[0].iPromoId == Convert.ToInt32(Promotions.BasicDeal)) { return View("BasicPromotions", prop[0]); }
                            if (prop[0].iPromoId == Convert.ToInt32(Promotions.MinimumStay)) { return View("MinimumLengthPromotion", prop[0]); }
                            if (prop[0].iPromoId == Convert.ToInt32(Promotions.EarlyBooker)) { return View("EarlyBookerPromotion", prop[0]); }
                            if (prop[0].iPromoId == Convert.ToInt32(Promotions.LastMinutes)) { return View("LastMinutePromotion", prop[0]); }
                            if (prop[0].iPromoId == Convert.ToInt32(Promotions.HrsPromotion)) { return View("HrsPromotions", prop[0]); }
                            if (prop[0].iPromoId == Convert.ToInt32(Promotions.OFRFreebies)) { return View("OFR", prop[0]); }
                            else { return View("../PromotionManagement"); }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(Session["PromoID"]) == Convert.ToInt32(Promotions.BasicDeal)) { return RedirectToAction("BasicPromotions"); }
                        if (Convert.ToInt32(Session["PromoID"]) == Convert.ToInt32(Promotions.MinimumStay)) { return RedirectToAction("MinimumLengthPromotion"); }
                        if (Convert.ToInt32(Session["PromoID"]) == Convert.ToInt32(Promotions.EarlyBooker)) { return RedirectToAction("EarlyBookerPromotion"); }
                        if (Convert.ToInt32(Session["PromoID"]) == Convert.ToInt32(Promotions.LastMinutes)) { return RedirectToAction("LastMinutePromotion"); }
                        if (Convert.ToInt32(Session["PromoID"]) == Convert.ToInt32(Promotions.HrsPromotion)) { return RedirectToAction("HrsPromotions"); }
                        if (Convert.ToInt32(Session["PromoID"]) == Convert.ToInt32(Promotions.OFRFreebies)) { return RedirectToAction("OFR"); }
                        else { return View("../PromotionManagement"); }
                    }

                }




            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult BindRoomType(int Id, string type)
        {
            List<etblPropertyPromotionMap> prop = new List<etblPropertyPromotionMap>();
            List<CheckBoxList> results = new List<CheckBoxList>();
            int PropId = Convert.ToInt32(Session["PropId"].ToString());

            if (Session["Entrytype"].ToString() == "N")
            {
                if (type == "First")
                {
                    if (Id != 0)
                        results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId, Id, Id);
                    else
                        results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId);
                }
                else if (type == "RPChange")
                {
                    results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId, Id, Id);
                }
                else
                {
                    results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId);
                }
            }
            else if (Session["Entrytype"].ToString() == "U" || Session["Entrytype"].ToString() == "E")
            {
                if (type == "First")
                {
                    if (Session["IsError"] == "True")
                    {
                        results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId, Id, Id);
                        Session["IsError"] = null;
                    }
                    else if (Session["id"] != null)
                    {
                        int a = Convert.ToInt32(Session["id"]) == null ? 0 : Convert.ToInt32(Session["id"]);
                        if (a != 0)
                        {
                            prop = BL_tblPromotionManagement.getPropertyPromoDataByID(a);
                            results = BL_tblPromotionManagement.GetRoomTypeCheckBox(prop[0].iPropId, prop[0].iID, prop[0].iRPId);
                        }
                    }
                }
                else if (type == "RPChange")
                {
                    results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId, Id, Id);
                }
                else
                {
                    results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId);
                }
            }
            else
            {
                results = BL_tblPromotionManagement.GetRoomTypeCheckBox(PropId);
            }

            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetRatePlanValidity(int Id)
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            List<etblPropertyPromotionMap> results = BL_tblPropertyPromotionMap.GetRatePlanValidity(Id, PropId);
            Validity obj = new Validity();
            if (results != null)
            {
                obj.ValidFrom = results[0].dtRPValidFrom;
                obj.ValidTo = results[0].dtRPValidTo;
            }
            else
            {
                obj.ValidFrom = null;
                obj.ValidTo = null;
            }
            return Json(new
            {
                suggestions = obj
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BindCancellationPolicy()
        {
            List<CheckBoxList> results = new List<CheckBoxList>();
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            if (PropId != 0)
            {
                results = BL_tblPromotionManagement.GetCancellationPolicy(PropId);
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
    }
}