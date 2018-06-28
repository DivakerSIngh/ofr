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
    public class AmenityController : BaseController
    {
        // GET: Amenity
         [Route("Amenity")]
        public ActionResult Index()
        {
            return View();
        }
         public string AddAmenity(string name)
         {
             object result = null;
             string strReturn = string.Empty;

             try
             {
                 eAmenity eObj = new eAmenity();
                 eObj.sAmenityName = name;
                 eObj.dtActionDate = DateTime.Now;
                 eObj.cStatus = "A";
                 eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                 if (name.Trim() == "")
                 {
                     result = new { st = 0, msg = clsUtils.ErrorMsg("", 9) };
                 }
                 else
                 {
                     int i = BL_Amenity.AddRecord(eObj);
                     if (i == 1)
                     {
                         result = new { st = 1, msg = clsUtils.ErrorMsg("Amenity", 1) };
                     }
                     else if (i == 2)
                     {
                         result = new { st = 0, msg = clsUtils.ErrorMsg("Amenity", 0) };
                     }
                     else
                     {
                         result = new { st = 0, msg = clsUtils.ErrorMsg("Amenity", 3) };
                     }
                 }

                 
             }
             catch (Exception)
             {
                 result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

             }
             strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
             return strReturn;
         }
         public string DeleteAmenity(int id)
         {
             object result = null;
             string strReturn = string.Empty;

             try
             {
                 int i = BL_Amenity.DeleteRecord(id);
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

         public string UpdateAmenity(int id, string name)
         {
             object result = null;
             string strReturn = string.Empty;

             try
             {
                 eAmenity obj = new eAmenity();
                 obj = BL_Amenity.GetSingleRecordById(id);
                 obj.dtActionDate = DateTime.Now;
                 obj.sAmenityName = name;
                 obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                 if (name.Trim() == "")
                 {
                     result = new { st = 0, msg = clsUtils.ErrorMsg("", 9) };
                 }
                 else
                 {
                     int i = BL_Amenity.UpdateRecord(obj);
                     if (i == 1)
                     {
                         result = new { st = 1, msg = clsUtils.ErrorMsg("Amenity", 2) };
                     }
                     else if (i == 2)
                     {
                         result = new { st = 0, msg = clsUtils.ErrorMsg("Amenity", 0) };
                     }
                     else
                     {
                         result = new { st = 0, msg = clsUtils.ErrorMsg("Amenity", 3) };
                     }
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
                 eAmenity obj = new eAmenity();
                 obj = BL_Amenity.GetSingleRecordById(id);
                 obj.dtActionDate = DateTime.Now;
                 obj.cStatus = status;
                 obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                 int i = BL_Amenity.UpdateRecord(obj);
                 if (i == 1)
                 {
                     result = new { st = 1, msg = clsUtils.ErrorMsg("Amenity", 4, status) };
                 }
                 else
                 {
                     result = new { st = 0, msg = clsUtils.ErrorMsg("Amenity", 0) };
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