using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class BlackListDomainController : BaseController
    {
        [Route("BlackListDomain")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddBlackListDomain()
        {
            return PartialView("_AddUpdateBlackListDomain");
        }

        [HttpPost]
        public ActionResult AddUpdateBlackListDomain(eBlackListedDomainM domain)
        {
            if (ModelState.IsValid)
            {
                domain.sActionType = "A";
                domain.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                //domain.iActionBy = 11;

                var result = BL_BlackListedDomainM.AddEditBlacklistDomain(domain);

                if (result.Key == -1)
                {
                    return Json(new { status = false, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }
                else if (result.Key == 1)
                {
                    return Json(new { status = true, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { status = false, Msg = "An error occured while adding the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string domain)
        {
            if (ModelState.IsValid)
            {
                var blackListDomain = new eBlackListedDomainM();

                if (!string.IsNullOrEmpty(domain))
                {
                    //company.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    blackListDomain.iActionBy = 11;
                    blackListDomain.sDomainName = domain;
                }

                var result = BL_BlackListedDomainM.DeleteBlackListDomain(blackListDomain);

                if (result.Key == -1)
                {
                    return Json(new { status = false, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }
                else if (result.Key == 1)
                {
                    return Json(new { status = true, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { status = false, Msg = "An error occured while deleting the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ToggleStatus(string sDomainName, bool status)
        {
            if (ModelState.IsValid)
            {
                var result = BL_BlackListedDomainM.ToggleStatus(sDomainName, status);

                if (result.Key == -1)
                {
                    return Json(new { status = false, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }
                else if (result.Key == 1)
                {
                    return Json(new { status = true, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { status = false, Msg = "An error occured while deleting the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
        }
    }
}