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
    public class LocalityController : BaseController
    {
        // GET: Locality
        [Route("Locality")]
        public ActionResult Index()
        {
            return View("Locality");
        }

        public string AddLocality(int countryid, int stateid, int cityid, int areaid, string localityname)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblLocality eObj = new etblLocality();
                eObj.iCountryId = countryid;
                eObj.iStateId = stateid;
                eObj.iCityId = cityid;
                eObj.iAreaId = areaid;
                eObj.sLocality = localityname;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = "A";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Locality.AddRecord(eObj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Locality", 1) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality", 3) };
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteLocality(int id,string status)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblLocality obj = new etblLocality();
                obj = BL_Locality.GetSingleRecordById(id);
                obj.cStatus = status;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Locality.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Locality", 4, status) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string UpdateLocality(int id, int countryid, int stateid, int cityid, int areaid, string localityname)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                etblLocality obj = new etblLocality();
                obj = BL_Locality.GetSingleRecordById(id);
                obj.iCountryId = countryid;
                obj.iStateId = stateid;
                obj.iCityId = cityid;
                obj.iAreaId = areaid;
                obj.sLocality = localityname;
                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Locality.UpdateRecord(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Locality", 2) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality", 0) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality", 1) };
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
        public JsonResult BindMacroArea(string id)
        {
            int cid = int.Parse(id);
            List<eMacroAreaM> results = BL_tblMacroAreaM.GetAllRecordsById(cid);
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
    }
}