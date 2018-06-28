using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;
using System.Globalization;

namespace OneFineRateApp.Controllers.Property
{
    public class FitnessRecreationController : BaseController
    {
         [Route("FitnessRecreation")]
        // GET: FitnessRecreation
         //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            etblPropertyRecreationMap obj = new etblPropertyRecreationMap();
            obj = BL_tblPropertyRecreationMap.GetSingleRecordById(Convert.ToInt32(Session["PropId"]));
            obj.iPropId = Convert.ToInt32(Session["PropId"]);
            obj.OnSiteRecreationfacilitiesItems = BL_tblOnsiteRecreationFacilitiesM.GetOnsiteRecreationFacilities(obj.sRecreationFacilityId);
            obj.LandActivitiesItems = BL_tblLandActivitiesM.GetLandActivities(obj.sLandActivityId);
            obj.GolfItems = BL_tblGolfM.GetGolf(obj.sGolfId);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Modify(etblPropertyRecreationMap obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        obj.dtActionDate = DateTime.Now;
                        //get all On Site Recreation Facilities comma seperated
                        if (obj.SelectedOnSiteRecreationfacilities != null)
                        {
                            obj.sRecreationFacilityId = obj.SelectedOnSiteRecreationfacilities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Land Activities comma seperated
                        if (obj.SelectedLandActivities != null)
                        {
                            obj.sLandActivityId = obj.SelectedLandActivities.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all golf comma seperated
                        if (obj.SelectedGolf != null)
                        {
                            obj.sGolfId = obj.SelectedGolf.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => s1 + "," + s2);
                        }
                        //get all Meetings comma seperated
                        obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        int result = BL_tblPropertyRecreationMap.UpdateRecord(obj);
                        if (result == 1)
                        {
                            TempData["msg"] = "Fitness and Recreation Modified Successfully";
                            return RedirectToAction("Index");
                          
                        }
                        else
                        {
                            TempData["ERROR"] = "Kindly try after some time.";
                            return RedirectToAction("Index");
                            
                        }
                        obj.OnSiteRecreationfacilitiesItems = BL_tblOnsiteRecreationFacilitiesM.GetOnsiteRecreationFacilities(obj.sRecreationFacilityId);
                        obj.LandActivitiesItems = BL_tblLandActivitiesM.GetLandActivities(obj.sLandActivityId);
                        obj.GolfItems = BL_tblGolfM.GetGolf(obj.sGolfId);
                    }
                    catch (Exception)
                    {
                        TempData["ERROR"] = "Kindly try after some time.";
                        throw;
                    }
                }

            }
            catch (Exception)
            {
                TempData["ERROR"] = "Kindly try after some time.";
            }
            return View("~/Views/FitnessRecreation/Index.cshtml",obj);
        }
    }

}