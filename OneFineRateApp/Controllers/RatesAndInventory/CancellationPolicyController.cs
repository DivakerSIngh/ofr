using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;

namespace OneFineRateApp.Controllers.RatesAndInventory
{

    public class CancellationPolicyController : BaseController
    {
        etblPropertyCancellationPolicyMap obj = new etblPropertyCancellationPolicyMap();
        // GET: CancellationPolicy
        [Route("CancellationPolicy")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPolicy()
        {
            obj.Mode = "Add";
            ViewBag.HeaderText = "Add Policy";
            return PartialView("CancellationPolicy", obj);
        }
        public ActionResult Edit(int Id)
        {
            ViewBag.HeaderText = "Modify Policy";
            obj = BL_tblPropertyCancellationPolicyMap.GetSingleRecordById(Id);
            obj.Mode = "Edit";
            if (obj.dtValidFrom != null)
            {
                obj.validfrom = String.Format("{0:dd/MM/yyyy}", obj.dtValidFrom);
            }
            if (obj.dtValidTo != null)
            {
                obj.validTo = String.Format("{0:dd/MM/yyyy}", obj.dtValidTo);
            }

            return PartialView("CancellationPolicy", obj);
        }

        [HttpPost]
        public ActionResult AddUpdate(etblPropertyCancellationPolicyMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {

                    eObj.dtValidFrom = Convert.ToDateTime(eObj.validfrom);
                    eObj.dtValidTo = Convert.ToDateTime(eObj.validTo);

                    if (eObj.bIsNonRefundable == false)
                    {
                        if (eObj.dValue.Value < 0)
                        {
                            result = new { st = 0, msg = "Charge cannot be negative." };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (eObj.dValue.Value > 100)
                        {
                            result = new { st = 0, msg = "Charge cannot be more than 100." };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (eObj.dHrsDays.Value < 0)
                        {
                            result = new { st = 0, msg = "Cancellation duration cannot be negative." };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (eObj.dValue.Value > 100)
                        {
                            result = new { st = 0, msg = "Cancellation duration cannot be more than 100." };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        string charrgetype = eObj.bIsPercent == true ? "%" : "Nights";
                        string arrival = eObj.cHrsDays == "Hrs" ? "Hours" : "Days";
                        eObj.sPolicyName = eObj.dValue.Value.ToString().Replace(".00", "") + "" + charrgetype + " - " + eObj.dHrsDays + "" + arrival;
                    }
                    else
                    {
                        eObj.sPolicyName = "Non-Refundable";
                    }


                    if (eObj.Mode == "Add")
                    {
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.dtActionDate = DateTime.Now;
                        eObj.cStatus = "A";

                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        if (BL_tblPropertyCancellationPolicyMap.CheckIfPolicyNameExists(eObj.sPolicyName, eObj.iPropId))
                        {
                            result = new { st = 0, msg = "Policy Name already exists." };
                        }
                        else
                        {


                            int j = BL_tblPropertyCancellationPolicyMap.AddRecord(eObj);
                            if (j == 1)
                            {
                                result = new { st = 1, msg = "Added successfully." };
                            }
                            else
                            {
                                result = new { st = 0, msg = "Kindly try after some time." };
                            }
                        }
                    }
                    else if (eObj.Mode == "Edit")
                    {
                        if (BL_tblPropertyCancellationPolicyMap.CheckIfPolicyNameExistsOnUpdate(eObj.sPolicyName.Replace(".00", ""), eObj.iCancellationPolicyId, eObj.iPropId))
                        {
                            result = new { st = 0, msg = "This Policy Name already exists." };
                        }
                        else
                        {

                            int j = BL_tblPropertyCancellationPolicyMap.UpdateRecord(eObj);
                            if (j == 1)
                            {
                                result = new { st = 1, msg = "Updated successfully." };
                            }
                            else
                            {
                                result = new { st = 0, msg = "Kindly try after some time." };
                            }
                        }
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

        public string UpdateStatus(int Id, bool Mode)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                obj = BL_tblPropertyCancellationPolicyMap.GetSingleRecordById(Id);
                if (Mode == true)
                {
                    obj.cStatus = "A";
                }
                else
                {
                    obj.cStatus = "I";
                    string s = BL_tblPropertyCancellationPolicyMap.CheckPolicyMapping(Id);
                    if (s != "")
                    {
                        result = new { st = 0, msg = s };
                        strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
                        return strReturn;
                    }
                }
                int i = BL_tblPropertyCancellationPolicyMap.UpdateRecord(obj);
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