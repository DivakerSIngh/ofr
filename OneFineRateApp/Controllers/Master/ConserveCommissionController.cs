using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class ConserveCommissionController : BaseController
    {
        // GET: ConserveCommission
        [Route("ConserveCommission/Index")]
        [Route("ConserveCommission")]
        public ActionResult Index()
        {
            eConserveCommission obj = new eConserveCommission();

            return View(obj);
        }
        public ActionResult AddConserveCommission(eConserveCommission eObj)
        {
            //string sPromoTitle, string sPromoDescription, string sPromoCode, string cPercentageValue, string dValue, string dtBookingFrom, string dtBookingTo, string dtStayFrom, string dtStayTo
            string strReturn = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    eObj.dtFrom = clsUtils.ConvertddmmyyyytoDateTime(eObj.StayFrom);
                    eObj.dtTo = clsUtils.ConvertddmmyyyytoDateTime(eObj.StayTo);

                    eObj.dtActionDate = DateTime.Now;
                    eObj.cStatus = "A";
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;


                    if (eObj.iCCId > 0)//eObj.Mode == "E" &&
                    {
                        string s = BL_ConserveCommission.CheckHotelMappingWhileEditOrEnable(eObj);
                        if (s == "")
                        {
                            int i = BL_ConserveCommission.UpdateRecord(eObj);
                            if (i == 1)
                            {
                                eObj.msg = clsUtils.ErrorMsg("Conserve Commission", 2);
                                eObj.st = "1";
                            }
                            else
                            {
                                eObj.msg = "Please try again later.";
                                eObj.st = "0";
                            }
                        }
                        else
                        {
                            eObj.msg = s;
                            eObj.st = "0";
                        }
                    }
                    else
                    {
                        int i = BL_ConserveCommission.AddRecord(eObj);
                        if (i == 1)
                        {
                            eObj.msg = clsUtils.ErrorMsg("Conserve Commission", 1);
                            eObj.st = "1";
                        }
                        else
                        {
                            eObj.msg = "Please try again later.";
                            eObj.st = "0";
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
                    eObj.msg = errormsg;
                    eObj.st = "0";
                }
            }
            catch (Exception)
            {
                eObj.msg = clsUtils.ErrorMsg("", 3);
                eObj.st = "0";

            }
            return PartialView("_ConserveCommission", eObj);
        }
        //public ActionResult SetData()
        //{
        //    try
        //    {
        //        int a = 0;
        //        string Entrytype = "E";
        //        if (HttpContext.Request.Params["Entrytype"] != null) { Entrytype = HttpContext.Request.Params["Entrytype"]; }
        //        if (HttpContext.Request.Params["id"] != null) { a = Convert.ToInt32(HttpContext.Request.Params["id"]); }
        //        ePromoCode prop = new ePromoCode();
        //        prop = BL_PromoCode.GetSingleRecordById(a);
        //        prop.Mode = "E";
        //        return View("Index", prop);//RedirectToAction("Index", "PromoCode", prop);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public ActionResult Add()
        {
            eConserveCommission obj = new eConserveCommission();
            obj.Mode = "ADD";
            obj.heading = "Add Conserve Commission";
            return PartialView("_ConserveCommission", obj);
        }
        public ActionResult Edit(int Id)
        {
            eConserveCommission obj = new eConserveCommission();

            obj = BL_ConserveCommission.GetSingleRecordById(Id);
            obj.heading = "Modify Conserve Commission";
            obj.Mode = "Edit";
            if (obj.dtFrom != null)
            {
                obj.StayFrom = String.Format("{0:dd/MM/yyyy}", obj.dtFrom);
            }
            if (obj.dtTo != null)
            {
                obj.StayTo = String.Format("{0:dd/MM/yyyy}", obj.dtTo);
            }
            return PartialView("_ConserveCommission", obj);
        }
        //public string DeletePromoCode(int id)
        //{
        //    object result = null;
        //    string strReturn = string.Empty;

        //    try
        //    {
        //        int i = BL_Amenity.DeleteRecord(id);
        //        if (i == 1)
        //        {
        //            result = new { st = 1, msg = clsUtils.ErrorMsg("Promo Code", 5) };
        //        }
        //        else
        //        {
        //            result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

        //    }
        //    strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
        //    return strReturn;
        //}

        //public string UpdatePromoCode(int id, string name)
        //{
        //    object result = null;
        //    string strReturn = string.Empty;

        //    try
        //    {
        //        ePromoCode obj = new ePromoCode();
        //        //obj = BL_PromoCode.GetSingleRecordById(id);
        //        obj.dtActionDate = DateTime.Now;
        //        obj.sPromoCode = name;
        //        obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
        //        int i = BL_PromoCode.UpdateRecord(obj);
        //        if (i == 1)
        //        {
        //            result = new { st = 1, msg = clsUtils.ErrorMsg("Promo Code", 2) };
        //        }
        //        else
        //        {
        //            result = new { st = 0, msg = clsUtils.ErrorMsg("Promo Code", 0) };
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

        //    }
        //    strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
        //    return strReturn;
        //}
        public string UpdateStatus(int id, string status)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eConserveCommission obj = new eConserveCommission();
                obj = BL_ConserveCommission.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                string s = "";
                if (status == "A")
                {
                    s = BL_ConserveCommission.CheckHotelMappingWhileEditOrEnable(obj);
                }
                if (s == "")
                {
                    int i = BL_ConserveCommission.UpdateRecord(obj);
                    if (i == 1)
                    {
                        result = new { st = 1, msg = clsUtils.ErrorMsg("Conserve Commission", 4, status) };
                    }
                    else
                    {
                        result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };
                    }
                }
                else
                {
                    result = new { st = 0, msg = s };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        //public JsonResult ValidatePromoCode(string sPromoCode, string InitialPromoCode)
        //{
        //    bool ifNotExist = true;
        //    if (sPromoCode == InitialPromoCode)
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    ifNotExist = BL_PromoCode.CheckDataExist(sPromoCode);

        //    return Json(ifNotExist, JsonRequestBehavior.AllowGet);

        //}
    }
}