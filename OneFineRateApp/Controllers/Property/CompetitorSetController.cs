using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace OneFineRateApp.Controllers.Property
{
    public class CompetitorSetController : BaseController
    {
        etblPropertyCompetitorSet obj = new etblPropertyCompetitorSet();
        // GET: CompetitorSet
        [Route("CompetitorSet")]
        public ActionResult Index()
        {

            ViewBag.PropertyCompetitorSet = OneFineRateAppUtil.clsUtils.ConvertToJson(BL_tblPropertyCompetitorSet.GetPropertyLocalityMap(Convert.ToInt32(Session["PropId"])));
            return View(obj);
        }
        public JsonResult GetAllProperty(string txt)
        {
            List<PNames> data = BL_tblPropertyM.GetAllPropertyName(Convert.ToInt32(Session["PropId"]), txt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddUpdate(etblPropertyCompetitorSet eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    eObj.dtActionDate = DateTime.Now;

                    if (eObj.SelectedCompetitorSet != null)
                    {
                        JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.SelectedCompetitorSet.Replace("\\", "\""));
                        if (jArray != null)
                        {
                            List<etblPropertyCompetitorSet> lstPropertycompetitorset = new List<etblPropertyCompetitorSet>();
                            foreach (var item in jArray)
                            {
                                lstPropertycompetitorset.Add(new etblPropertyCompetitorSet()
                                {
                                    iPropId = eObj.iPropId,
                                    iCPropId = Convert.ToInt32(item["value"]),
                                    dtActionDate = DateTime.Now,
                                    iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId
                                });
                            }
                            eObj.PropertyCompetitorSetList = lstPropertycompetitorset;

                        }
                    }
                    int j = BL_tblPropertyCompetitorSet.AddUpdateRecord(eObj);
                    if (j == 1)
                    {
                        result = new { st = 1, msg = "Updated successfully." };
                    }
                    
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}