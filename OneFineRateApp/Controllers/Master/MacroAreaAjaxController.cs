using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class MacroAreaAjaxController : Controller
    {
        // GET: MacroAreaAjax
        public ActionResult Index()
        {
            eMacroAreaM obj = new eMacroAreaM();
            obj.StateList = BL_tblMacroAreaM.StateList(obj.iStateId, obj.iCountryId);
            obj.CityList = BL_tblMacroAreaM.CityList(obj.iCityId, obj.iStateId, obj.iCountryId);
            return View(obj);
        }
        [HttpPost]
        public ActionResult AddMacroArea(eMacroAreaM eObj)
        {
            string strReturn = string.Empty;

            try
            {
                if (eObj.iAreaId == 0)
                {
                    eObj.dtActionDate = DateTime.Now;
                    eObj.cStatus = "A";
                    eObj.iActionBy = 1;
                    int i = BL_tblMacroAreaM.AddRecord(eObj);
                    if (i == 1)
                    {
                        strReturn = "Added successfully.";
                    }
                    else
                    {
                        strReturn = "Kindly try after some time.";
                    }
                }
                else
                {
                    eObj.dtActionDate = DateTime.Now;
                    int i = BL_tblMacroAreaM.UpdateRecord(eObj);
                    if (i == 1)
                    {
                        strReturn = "Updated successfully.";
                    }
                    else
                    {
                        strReturn = "Kindly try after some time.";
                    }

                }
            }
            catch (Exception)
            {

                strReturn = "Kindly try after some time.";

            }
            TempData["result"] = strReturn;
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            eMacroAreaM obj = new eMacroAreaM();
            obj = BL_tblMacroAreaM.GetSingleRecordById(id);
            obj.StateList = BL_tblMacroAreaM.StateList(obj.iStateId, obj.iCountryId);
            obj.CityList = BL_tblMacroAreaM.CityList(obj.iCityId,obj.iStateId, obj.iCountryId);
            ViewBag.OpenEditModel = "Y";
            return PartialView("Index", obj);
        }
    }
}