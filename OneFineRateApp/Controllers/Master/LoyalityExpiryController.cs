using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class LoyalityExpiryController : BaseController
    {
        // GET: LoyalityExpiry
        [Route("LoyalityExpiry")]
        public ActionResult Index()
        {
            return View("Index", BL_LoyalityAmenityMap.GetSingleRecordById());
            //return View();
        }

        public string UpdateLoyalityPointsExpiry(string ExpiryDate, Array gridData)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eLoyalityPointsCustomerData objData = new eLoyalityPointsCustomerData();
                List<eLoyalityPointsCustomerData> objDataList = new List<eLoyalityPointsCustomerData>();

                foreach (var item in gridData)
                {
                    objData = BL_LoyalityAmenityMap.GetRecordById(Convert.ToInt32(item));
                    objData.dtExpiry = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(ExpiryDate));
                    objDataList.Add(objData);
                }
                int counter = 0;
                for (int c = 0; c < objDataList.Count; c++)
                {
                    BL_LoyalityAmenityMap.UpdateLoyalityPointsExpiryDate(objDataList[c]);
                    counter++;
                }
                if (objDataList.Count == counter)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Expiry Date for the selected records", 2) };
                }
                else
                {
                    var diff = objDataList.Count - counter;
                    result = new { st = 0, msg = "Not able to update" + diff + "records out off" + objDataList.Count };
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