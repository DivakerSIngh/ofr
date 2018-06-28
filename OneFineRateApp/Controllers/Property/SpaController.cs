using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;

namespace OneFineRateApp.Controllers.Property
{
    public class SpaController : BaseController
    {
        // GET: Spa
        [HttpGet]
        [Route("Spa")]
        public ActionResult Index()
        {
            etblPropertySpaMap obj = new etblPropertySpaMap();
            obj = BL_tblPropertySpaMap.GetSingleRecordById(Convert.ToInt32(Session["PropId"]));
            return View("~/Views/Spa/view.cshtml", obj);
        }
        [HttpPost]
        public ActionResult Modify(etblPropertySpaMap obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        bool type = false;
                        obj.dtActionDate = DateTime.Now;
                        obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId; 
                        obj.iPropId = Convert.ToInt32(Session["PropId"]); 
                        string Name = BL_tblPropertySpaMap.CheckDataExist(obj.iPropId);
                        if (Name != string.Empty)
                        {
                            type = true;
                        }

                        int result = BL_tblPropertySpaMap.UpdateRecord(obj, type);
                        if (result == 1)
                        {
                            TempData["msg"] = "Wellness Facilites Modified Successfully";
                            return RedirectToAction("Index");

                        }
                        else
                        {
                            TempData["ERROR"] = "Kindly try after some time.";
                            return RedirectToAction("Index");

                        }
                    }
                    catch (Exception)
                    {
                        TempData["ERROR"] = "Kindly try after some time.";
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                TempData["ERROR"] = "Kindly try after some time.";
            }
            return View("~/Views/Spa/view.cshtml", obj);
        }
        public JsonResult ValidateSpaName(string sSpaName, string InitialSpaName)
        {
            bool ifNotExist = true;
            if (sSpaName == InitialSpaName)
                return Json(true, JsonRequestBehavior.AllowGet);
            ifNotExist = BL_tblPropertySpaMap.CheckDataExist(sSpaName, Convert.ToInt32(Session["PropId"]));

            return Json(ifNotExist, JsonRequestBehavior.AllowGet);

        }
    }
}