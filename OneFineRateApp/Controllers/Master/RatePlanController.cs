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
    public class RatePlanController : BaseController
    {
        // GET: RatePlan
        [Route("RatePlan")]
        public ActionResult Index()
        {
            return View();
        }
        public string AddRatePlan(string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eRatePlan eObj = new eRatePlan();
                eObj.sRatePlan = name;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_RatePlan.AddRecord(eObj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("RatePlan", 1) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("RatePlan", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("RatePlan", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteRatePlan(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_RatePlan.DeleteRecord(id);
                if (i == 1)
                {
                    result = new { st = 1, msg = "Deleted Successfully ." };
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

        public string UpdateRatePlan(int id, string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eRatePlan obj = new eRatePlan();
                obj = BL_RatePlan.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.sRatePlan = name;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_RatePlan.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("RatePlan", 2) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("RatePlan", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("RatePlan", 0) };
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
                eRatePlan obj = new eRatePlan();
                obj = BL_RatePlan.GetSingleRecordById(id);
                obj.dtActionDate = DateTime.Now;
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_RatePlan.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("", 4, status) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("RatePlan", 0) };
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