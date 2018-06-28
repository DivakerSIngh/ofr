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
    [Result.ExceptionFilters]
    [Authorize]
    public class MacroAreaController : BaseController
    {
        // GET: MacroArea

        [Route("MacroArea")]
        public ActionResult Index()
        {
            return View();
        }

        public string AddMacroArea(int countryid, int stateid, int cityid, string macroareaname)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eMacroAreaM eObj = new eMacroAreaM();
                eObj.iCountryId = countryid;
                eObj.iStateId = stateid;
                eObj.iCityId = cityid;
                eObj.sArea = macroareaname;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_tblMacroAreaM.AddRecord(eObj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("MacroArea", 1) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("MacroArea", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("MacroArea", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteMacroArea(int id, string status)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eMacroAreaM obj = new eMacroAreaM();
                obj = BL_tblMacroAreaM.GetSingleRecordById(id);
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_tblMacroAreaM.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("MacroArea", 4, status) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("MacroArea", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string UpdateMacroArea(int id, int countryid, int stateid, int cityid, string macroareaname)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eMacroAreaM obj = new eMacroAreaM();
                obj = BL_tblMacroAreaM.GetSingleRecordById(id);
                obj.iCountryId = countryid;
                obj.iStateId = stateid;
                obj.iCityId = cityid;
                obj.sArea = macroareaname;
                int i = BL_tblMacroAreaM.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("MacroArea", 2) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("MacroArea", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("MacroArea", 3) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public JsonResult BindStates(string id)
        {
            int cid = int.Parse(id);

            List<etblStateM> results = BL_tblStateM.GetAllRecordsById(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult BindCity(string id)
        {
            int cid = int.Parse(id);
            List<etblCityM> results = BL_tblCityM.GetAllRecordsById(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
    }
}