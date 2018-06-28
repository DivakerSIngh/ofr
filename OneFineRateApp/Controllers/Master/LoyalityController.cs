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
    [Authorize]
    public class LoyalityController : BaseController
    {
        // GET: Loyality
        [Route("Loyality")]
        public ActionResult Index()
        {
            return View("Index", BL_LoyalityAmenityMap.GetSingleRecordById());
        }
        public string UpdateLoyalityPoints(int iLoyalityId, int EarnRupees, int EarnPoints, int RedeemPoints, int RedeemRupees , int iReferredPoints, int iReferPointsSignUp, int iSignUpPointsWithoutRef, int iExpiryDays)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eLoyalityPoints obj = new eLoyalityPoints();
                obj = BL_LoyalityAmenityMap.GetSingleRecordById();
                obj.dtActionDate = DateTime.Now;
                obj.iEarnMoney = EarnRupees;
                obj.iEarnPoints = EarnPoints;
                obj.iRedeemMoney = RedeemRupees;
                obj.iRedeemPoints = RedeemPoints;
                obj.iReferredPoints = iReferredPoints;
                obj.iReferPointsSignUp = iReferPointsSignUp;
                obj.iSignUpPointsWithoutRef = iSignUpPointsWithoutRef;
                obj.iExpiryDays = iExpiryDays;

                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_LoyalityAmenityMap.UpdateLoyalityPoints(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Loyality Points", 2) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Loyality Points", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string AddLoyalityPointsAmenityMap(int iPoints, int iAmenityId)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eLoyalityAmenityMap obj = new eLoyalityAmenityMap();                
                obj.dtActionDate = DateTime.Now;
                obj.iPoints = iPoints;
                obj.iAmenityId = iAmenityId;
                obj.cStatus = "A";

                obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_LoyalityAmenityMap.AddLoyalityAmenityMap(obj);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Locality Mapping", 1) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality Mapping", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteData(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_LoyalityAmenityMap.DeleteData(id);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Locality Mapping", 5) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Locality Mapping", 0) };
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