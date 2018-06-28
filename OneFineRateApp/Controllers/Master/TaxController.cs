using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using OneFineRateAppUtil;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class TaxController : BaseController
    {
        // GET: Tax
        [Route("Tax")]
        public ActionResult Index()
        {
            return View();
        }
        public string AddTax(string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eTax eObj = new eTax();
                eObj.sTaxName = name;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Tax.AddRecord(eObj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Tax", 1) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Tax", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Tax", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteTax(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_Tax.DeleteRecord(id);
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

        public string UpdateTax(int id, string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eTax obj = new eTax();
                obj = BL_Tax.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.sTaxName = name;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Tax.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Tax", 2) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Tax", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Tax", 3) };
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
                eTax obj = new eTax();
                obj = BL_Tax.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Tax.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Tax", 4, status) };
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
    }
}