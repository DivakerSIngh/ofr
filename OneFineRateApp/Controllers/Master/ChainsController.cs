using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using OneFineRateAppUtil;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class ChainsController : BaseController
    {
        // GET: Chains
        
        [Route("Chains")]
        public ActionResult Index()
        {           
            return View();
        }
        public string AddChains(string name)
        {
            object result = null;
            string strReturn = string.Empty;
          
            try
            {
                eChains eObj = new eChains();
                eObj.sChainName = name;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                 int i = BL_Chains.AddRecord(eObj);
                if(i==1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Chain", 1) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Chain", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Chain", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };
                
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteChains(int id, string status)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eChains obj = new eChains();
                obj = BL_Chains.GetSingleRecordById(id);
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Chains.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Chain", 4, status) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Chain", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string UpdateChains(int id,string name)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eChains obj = new eChains();
                obj = BL_Chains.GetSingleRecordById(id);
                obj.sChainName = name;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Chains.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Chain", 2) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Chain", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Chain", 3) };
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