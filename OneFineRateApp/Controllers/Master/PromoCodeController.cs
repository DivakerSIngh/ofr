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
    public class PromoCodeController : BaseController
    {

        // GET: PromoCode
        [Route("PromoCode/Index")]
        [Route("PromoCode")]
        public ActionResult Index()
        {
            ePromoCode obj = new ePromoCode();

            return View(obj);
        }
        public ActionResult AddPromoCode(ePromoCode eObj)
        {
            //string sPromoTitle, string sPromoDescription, string sPromoCode, string cPercentageValue, string dValue, string dtBookingFrom, string dtBookingTo, string dtStayFrom, string dtStayTo
            string strReturn = string.Empty;

            try
            {
                //ePromoCode eObj = new ePromoCode();
                //eObj.sPromoCode = objPromo.sPromoCode;
                //eObj.sPromoTitle = objPromo.sPromoTitle;
                //eObj.sPromoDescription = objPromo.sPromoDescription;
                //eObj.cPercentageValue = objPromo.cPercentageValue;
                //eObj.dValue = Convert.ToDecimal(objPromo.dValue);
                if (ModelState.IsValid)
                {
                    eObj.dtBookingFrom = clsUtils.ConvertddmmyyyytoDateTime(eObj.BookingFrom);
                    eObj.dtBookingTo = clsUtils.ConvertddmmyyyytoDateTime(eObj.BookingTo);
                    eObj.dtStayFrom = clsUtils.ConvertddmmyyyytoDateTime(eObj.StayFrom);
                    eObj.dtStayTo = clsUtils.ConvertddmmyyyytoDateTime(eObj.StayTo);

                    eObj.dtActionDate = DateTime.Now;
                    eObj.cStatus = "A";
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                    if (eObj.dtStayTo >= eObj.dtBookingTo)
                    {
                        if (eObj.iPromoCodeId > 0)//eObj.Mode == "E" &&
                        {

                            int i = BL_PromoCode.UpdateRecord(eObj);
                            if (i == 1)
                            {
                                eObj.msg = clsUtils.ErrorMsg("Promo Code", 2);
                                eObj.st = "1";
                            }
                            else
                            {
                                eObj.msg = clsUtils.ErrorMsg("Promo Code", 0);
                                eObj.st = "0";
                            }
                        }
                        else
                        {
                            int i = BL_PromoCode.AddRecord(eObj);
                            if (i == 1)
                            {
                                eObj.msg = clsUtils.ErrorMsg("Promo Code", 1);
                                eObj.st = "1";
                            }
                            else
                            {
                                eObj.msg = clsUtils.ErrorMsg("Promo Code", 0);
                                eObj.st = "0";
                            }
                        }
                    }
                    else
                    {
                        eObj.msg = clsUtils.ErrorMsg("", 8);
                        eObj.st = "0";
                    }
                    //return RedirectToAction("Index", "PromoCode");
                }
            }
            catch (Exception)
            {
                eObj.msg = clsUtils.ErrorMsg("", 3);
                eObj.st = "0";

            }
            return PartialView("_PromoCode", eObj);
        }
        public ActionResult SetData()
        {
            try
            {
                int a = 0;
                string Entrytype = "E";
                if (HttpContext.Request.Params["Entrytype"] != null) { Entrytype = HttpContext.Request.Params["Entrytype"]; }
                if (HttpContext.Request.Params["id"] != null) { a = Convert.ToInt32(HttpContext.Request.Params["id"]); }
                ePromoCode prop = new ePromoCode();
                prop = BL_PromoCode.GetSingleRecordById(a);
                prop.Mode = "E";
                return View("Index", prop);//RedirectToAction("Index", "PromoCode", prop);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Add()
        {
            ePromoCode obj = new ePromoCode();
            obj.Mode = "ADD";
            obj.heading = "Add Promo Code";
            return PartialView("_PromoCode", obj);
        }
        public ActionResult Edit(int Id)
        {
            ePromoCode obj = new ePromoCode();

            obj = BL_PromoCode.GetSingleRecordById(Id);
            obj.heading = "Modify Promo Code";
            obj.Mode = "Edit";
            if (obj.dtStayFrom != null)
            {
                obj.StayFrom = String.Format("{0:dd/MM/yyyy}", obj.dtStayFrom);
            }
            if (obj.dtStayTo != null)
            {
                obj.StayTo = String.Format("{0:dd/MM/yyyy}", obj.dtStayTo);
            }
            if (obj.dtBookingFrom != null)
            {
                obj.BookingFrom = String.Format("{0:dd/MM/yyyy}", obj.dtBookingFrom);
            }
            if (obj.dtBookingTo != null)
            {
                obj.BookingTo = String.Format("{0:dd/MM/yyyy}", obj.dtBookingTo);
            }
            return PartialView("_PromoCode", obj);
        }
        public string DeletePromoCode(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_Amenity.DeleteRecord(id);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Promo Code", 5) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string UpdatePromoCode(int id, string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                ePromoCode obj = new ePromoCode();
                //obj = BL_PromoCode.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.sPromoCode = name;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_PromoCode.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Promo Code", 2) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Promo Code", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string UpdateStatus(int id, string status)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                ePromoCode obj = new ePromoCode();
                obj = BL_PromoCode.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_PromoCode.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Promo Code", 4, status) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public JsonResult ValidatePromoCode(string sPromoCode, string InitialPromoCode)
        {
            bool ifNotExist = true;
            if (sPromoCode == InitialPromoCode)
                return Json(true, JsonRequestBehavior.AllowGet);
            ifNotExist = BL_PromoCode.CheckDataExist(sPromoCode);

            return Json(ifNotExist, JsonRequestBehavior.AllowGet);

        }
    }
}