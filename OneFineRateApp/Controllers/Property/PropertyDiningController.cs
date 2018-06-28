using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;

namespace OneFineRateApp.Controllers.Property
{
    public class PropertyDiningController : BaseController
    {

        // GET: PropertyDining
        [Route("PropertyDining")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(etblPropertyDiningMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {

                    if (eObj.Mode != "Edit")
                    {
                        //string[] From = eObj.sTimingFromHH.Split(':');
                        //string[] To = eObj.sTimingToHH.Split(':');
                        //if ((From.Length < 2) || (To.Length < 2) )
                        //{
                        //     result = new { st = 0, msg = "Invalid Time." };
                        //     return Json(result, JsonRequestBehavior.AllowGet);
                        //}
                        eObj.dtActionDate = DateTime.Now;
                        //eObj.sTimingFromHH = From[0];
                        //eObj.sTimingFromMM = From[1];
                        //eObj.sTimingToHH = To[0];
                        //eObj.sTimingToMM = To[1];
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        eObj.bActive = true;
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);

                        int i = BL_tblPropertyDiningMap.AddRecord(eObj);
                        if (i == 1)
                        {
                            result = new { st = 1, msg = "Added successfully." };
                        }
                        else if (i == 2)
                        {
                            result = new { st = 0, msg = "Restaurant already exists with same name." };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }
                    }
                    else
                    {
                        //string[] From = eObj.sTimingFromHH.Split(':');
                        //string[] To = eObj.sTimingToHH.Split(':');
                        //if ((From.Length < 2) || (To.Length < 2))
                        //{
                        //    result = new { st = 0, msg = "Invalid Time." };
                        //    return Json(result, JsonRequestBehavior.AllowGet);
                        //}
                        eObj.dtActionDate = DateTime.Now;
                        //eObj.sTimingFromHH = From[0];
                        //eObj.sTimingFromMM = From[1];
                        //eObj.sTimingToHH = To[0];
                        //eObj.sTimingToMM = To[1];
                        eObj.bActive = true;
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        int i = BL_tblPropertyDiningMap.UpdateRecord(eObj);
                        if (i == 1)
                        {
                            result = new { st = 1, msg = "Updated successfully." };
                        }
                        else if (i == 2)
                        {
                            result = new { st = 0, msg = "Restaurant already exists with same name." };
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
            //return Json(new
            //{
            //    Result = false,
            //    Message = "Insert Successfully."
            //}, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
            //ModelState.AddModelError(string.Empty, "AJAX Post");
            //return PartialView("PropertyParking", eObj);
        }
        public ActionResult Edit(int Id, string Name)
        {
            etblPropertyDiningMap obj = new etblPropertyDiningMap();
            ViewBag.HeaderText = "Modify Property Dining";
            obj = BL_tblPropertyDiningMap.GetSingleRecord(Id, Name);
            obj.Mode = "Edit";
            //obj.sTimingFromHHOld = obj.sTimingFromHH;
            //obj.sTimingToHHOld = obj.sTimingToHH;
            //obj.sTimingFromHH = obj.sTimingFromHH + ":" + obj.sTimingFromMM;
            //obj.sTimingToHH = obj.sTimingToHH + ":" + obj.sTimingToMM;
           
            return PartialView("PropertyDining", obj);
        }
        public ActionResult Add()
        {
            etblPropertyDiningMap obj = new etblPropertyDiningMap();
            obj.Mode = "Add";
            //obj.sTimingFromHHOld = "00";
            //obj.sTimingFromMM = "00";
            //obj.sTimingToHHOld = "00";
            //obj.sTimingToMM = "00";
            ViewBag.HeaderText = "Add Property Dining";
            return PartialView("PropertyDining", obj);
        }
        public string UpdateStatus(int Id, string Name, bool Mode)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblPropertyDiningMap obj=  BL_tblPropertyDiningMap.GetSingleRecord(Id,Name);
                obj.bActive = Mode ? true : false;
                int i = BL_tblPropertyDiningMap.UpdateRecord(obj);
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
       
        
    }
}