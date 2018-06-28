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
    public class CountryController : BaseController
    {
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        public string AddCountry(string name, string countryname)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eCountries eObj = new eCountries();
                eObj.sCountry = name;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = 1;
                int i = BL_Country.AddRecord(eObj);
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
        public string DeleteCountry(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_Country.DeleteRecord(id);
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
        public string UpdateCountry(int id, string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eCountries obj = new eCountries();
                obj = BL_Country.GetSingleRecordById(id);
                obj.sCountry = name;
                int i = BL_Country.UpdateRecord(obj);
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