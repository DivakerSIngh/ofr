using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;
using System.Linq.Expressions;
using Newtonsoft.Json;
using OneFineRateAppUtil;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class RankManagementController : BaseController
    {
        // GET: RankManagement
        [Route("RankManagement")]
        public ActionResult Index()
        {
            etblRankManagement obj = new etblRankManagement();
            return View(obj);
        }

        public ActionResult Save(etblRankManagement prop)
        {
            string strReturn = string.Empty;
            int val = 0;
            object result = null;
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    string str;
                    str = "a";
                    // DoSomethingWith(error);
                }
            }
            if (ModelState.IsValid)
            {

                #region Add Rank
                if (prop.Mode == "Add")
                {
                    etblRankManagement eobj = new etblRankManagement();
                    eobj.iPropId = prop.iPropId;
                    eobj.dtStartdate = clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidFrom);
                    eobj.dtEndDate = clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidTo);

                    if (prop.IsSponsoredYes == false && prop.IsSponsoredNo == false)
                    {
                        eobj.IsSponsored = null;
                    }
                    else
                    {
                        eobj.IsSponsored = prop.IsSponsoredYes;
                    }
                    
                    eobj.IsRateDisparity = prop.IsRateDisparity;

                    if (prop.IsSponsoredNo == false) {
                        eobj.iNewRank = null;
                        eobj.iOldRank = Convert.ToInt32(0);
                    }
                    else
                    {
                        eobj.iNewRank = prop.iNewRank;
                        eobj.iOldRank = prop.iOldRank;
                    }


                    eobj.cStatus = "A";
                    eobj.dtActionDate = DateTime.Now;
                    eobj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                    val = BL_tblRankManagement.AddRecord(eobj);
                    if (val == 1)
                    {
                        strReturn = "Added successfully.";
                        return Json(new { st = 1, msg = strReturn }, JsonRequestBehavior.AllowGet);
                    }
                    else if (val == 2)
                    {
                        strReturn = "Property within same validity already exists";
                        return Json(new { st = 0, msg = strReturn }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        strReturn = "Kindly try after some time.";
                        return Json(new { st = 0, msg = strReturn }, JsonRequestBehavior.AllowGet);
                    }

                }
                #endregion


                #region Update Rank
                if (prop.Mode == "Update")
                {
                    prop.dtStartdate = clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidFrom);
                    prop.dtEndDate = clsUtils.ConvertddmmyyyytoDateTime(prop.dtValidTo);
                    if (prop.IsSponsoredYes == false && prop.IsSponsoredNo == false)
                    {
                        prop.IsSponsored = null;
                    }
                    else
                    {
                        prop.IsSponsored = prop.IsSponsoredYes;
                    }

                    if (prop.IsSponsoredNo == false)
                    {
                        prop.iNewRank = null;
                        prop.iOldRank = Convert.ToInt32(0);
                    }
                    

                    val = BL_tblRankManagement.UpdateRecord(prop);
                    if (val == 1)
                    {
                        strReturn = "Updated successfully.";
                        return Json(new { st = 1, msg = strReturn }, JsonRequestBehavior.AllowGet);
                    }
                    else if (val == 2)
                    {
                        strReturn = "Property within same validity already exists";
                        return Json(new { st = 0, msg = strReturn }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        strReturn = "Kindly try after some time.";
                        return Json(new { st = 0, msg = strReturn }, JsonRequestBehavior.AllowGet);
                    }

                }
                #endregion



            }
            else
            {
                strReturn = "Kindly try after some time.";
            }
            return Json(new { st = 0, msg = strReturn }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            etblRankManagement obj = new etblRankManagement();
            obj.IsSponsored = true;
            obj.Mode = "Add";
            return PartialView("pvRankManagement", obj);
        }
        public ActionResult Update(int Id)
        {
            etblRankManagement obj = new etblRankManagement();
            obj = BL_tblRankManagement.GetRankManagementByID(Id);
            obj.Mode = "Update";
            obj.dtValidFrom = obj.dtStartdate.ToString("dd/MM/yyyy");
            obj.dtValidTo = obj.dtEndDate.ToString("dd/MM/yyyy");
            if (obj.IsSponsored == true)
            {
                obj.IsSponsoredYes = true;
                obj.IsSponsoredNo = false;
            }
            else if (obj.IsSponsored == false)
            {
                obj.IsSponsoredYes = false;
                obj.IsSponsoredNo = true;
            }
            else
            {
                obj.IsSponsoredYes = false;
                obj.IsSponsoredNo = false;
            }

            if(obj.iNewRank == 0)
            {
                obj.iNewRank = null;
            }


            return PartialView("pvRankManagement", obj);
        }
        public ActionResult Delete(int Id)
        {
            string strReturn = string.Empty;
            try
            {
                etblRankManagement prop = new etblRankManagement();
                prop = BL_tblRankManagement.GetRankManagementByID(Id);
                if (prop.cStatus == "A") { prop.cStatus = "I"; }
                else if (prop.cStatus == "I") { prop.cStatus = "A"; }

                int val = BL_tblRankManagement.DeleteRecord(prop);
                if (val == 1)
                {
                    if (prop.cStatus == "I")
                    {
                        TempData["msg"] = "Disabled successfully";
                    }
                    else
                    {
                        TempData["msg"] = "Enabled successfully";
                    }
                }
                else
                {
                    TempData["ERROR"] = "Kindly try after some time";
                }

            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }

        public string GetRank(string CheckInDate, string ProbId)
        {
            string result = "";
            try
            {
                result = BL_tblRankManagement.GetRank(CheckInDate, Convert.ToInt32(ProbId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}