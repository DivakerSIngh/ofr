using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using OneFineRateBLL;
using OneFineRateBLL.Entities;

namespace OneFineRateApp.Controllers.Master
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult Index()
        {
            ViewBag.CountryList = OneFineRateBLL.BL_tblStateM.CountryList();
            return View();
        }
        public string AddState(int countryid, string statename)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblStateM eObj = new etblStateM();
                eObj.iCountryId = countryid;
                eObj.sState = statename;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = 1;
                int i = BL_tblStateM.AddRecord(eObj);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Added successfully." };
                }
                else
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteState(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_tblStateM.DeleteRecord(id);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Deleted successfully." };
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
        public string UpdateState(int id, int countryid, string statename)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblStateM obj = new etblStateM();
                obj = BL_tblStateM.GetSingleRecordById(id);
                obj.iCountryId = countryid;
                obj.sState = statename;
                int i = BL_tblStateM.UpdateRecord(obj);
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