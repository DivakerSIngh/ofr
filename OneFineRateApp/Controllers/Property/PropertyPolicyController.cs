using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;

namespace OneFineRateApp.Controllers.Property
{
    public class PropertyPolicyController : BaseController
    {
        etblPropertyPolicyMap obj = new etblPropertyPolicyMap();
        // GET: PropertyPolicy
        [Route("PropertyPolicy")]
        public ActionResult Index()
        {
            obj = BL_tblPropertyPolicyMap.GetSingleRecordById(Convert.ToInt32(Session["PropId"]));
            if (obj.iPropId != 0)
            {
                if (obj.b24HrsCheckIn == true)
                {
                    obj.sCheckInHH = "00:00";
                    obj.sCheckInHHOld = "00";
                    obj.sCheckInMM = "00";
                }
                else
                {
                    obj.sCheckInHHOld = obj.sCheckInHH;
                    obj.sCheckInHH = (string.IsNullOrEmpty(obj.sCheckInHH) ? "00" : obj.sCheckInHH) + ':' + (string.IsNullOrEmpty(obj.sCheckInMM) ? "00" : obj.sCheckInMM);
                }
                if (obj.b24HrsCheckout == true)
                {
                    obj.sCheckoutHH = "00:00";
                    obj.sCheckOutHHOld = "00";
                    obj.sCheckoutMM = "00";
                }
                else
                {
                    obj.sCheckOutHHOld = obj.sCheckoutHH;
                    obj.sCheckoutHH = (string.IsNullOrEmpty(obj.sCheckoutHH) ? "00" : obj.sCheckoutHH) + ':' + (string.IsNullOrEmpty(obj.sCheckoutMM) ? "00" : obj.sCheckoutMM);                     
                }

            }
            else
            {             
                obj.sCheckInHHOld = "00";
                obj.sCheckInMM = "00";
                obj.sCheckOutHHOld = "00";
                obj.sCheckoutMM = "00";

            }
            obj.CreditCardApprovalItems = BL_tblCrediCardM.GetCrediCards(obj.sCreditCardId);
            return View("Policy", obj);
        }
        [HttpPost]
        public ActionResult Add(etblPropertyPolicyMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string[] chekinTime = new string[0];
                    string[] chekOutTime = new string[0];

                    if (eObj.sCheckInHH != null)
                    {
                        chekinTime = eObj.sCheckInHH.Split(':');
                    }
                    if (eObj.sCheckoutHH != null)
                    {
                        chekOutTime = eObj.sCheckoutHH.Split(':');
                    }
                    if (eObj.b24HrsCheckIn == false)
                    {
                        eObj.sCheckInHH = chekinTime[0];
                        eObj.sCheckInMM = chekinTime[1];
                    }
                    else
                    {
                        eObj.sCheckInHH = null;
                        eObj.sCheckInMM = null;
                    }
                    if (eObj.b24HrsCheckout == false)
                    {
                        eObj.sCheckoutHH = chekOutTime[0];
                        eObj.sCheckoutMM = chekOutTime[1];
                    }
                    else
                    {
                        eObj.sCheckoutHH = null;
                        eObj.sCheckoutMM = null;
                    }

                    //get all credit cards accepted comma seperated
                    if (eObj.SelectedCreditCardApproval != null)
                    {
                        eObj.sCreditCardId = eObj.SelectedCreditCardApproval.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                    }

                    if (eObj.bChildrenAllowed == false)
                    {
                        eObj.iComplimentaryStayAge = null;
                    }

                    eObj.dtActionDate = DateTime.Now;
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    eObj.iPropId = Convert.ToInt32(Session["PropId"]);

                    int j = BL_tblPropertyPolicyMap.UpdateRecord(eObj);
                    if (j == 1)
                    {
                        result = new { st = 1, msg = "Updated successfully." };
                    }
                    else
                    {
                        result = new { st = 0, msg = "Kindly try after some time." };
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