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
    public class CorporateCompanyController : BaseController
    {
        [Route("CorporateCompany")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddUpdateCompany(int? id)
        {
            var model = new eCorporateCompanyM();

            if (id.HasValue)
            {
                model = BL_CorporateCompanyM.GetCorporateCompanyById(id.Value);
            }
            else
            {
                model.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();
            }

            return PartialView("_AddCompany", model);
        }

        [HttpPost]
        public ActionResult AddUpdateCompany(eCorporateCompanyM company)
        {

            if (ModelState.IsValid)
            {
                var domainNames = company.sDomainNames.Split(',');
                var domainType = company.DomainType.ToString();

                company.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                              
                if (company.iCompanyId != 0)
                {
                    company.sActionType = "E";
                }
                else
                {
                    company.cStatus = "A";
                    company.sActionType = "A";
                }

                DataTable dtTblDomainNames = new DataTable();

                var domainNameColumn = new DataColumn("sDomainName", typeof(string));
                var domainTypeColumn = new DataColumn("sDomainType", typeof(string));

                dtTblDomainNames.Columns.Add(domainNameColumn);
                dtTblDomainNames.Columns.Add(domainTypeColumn);

                foreach (var item in domainNames)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        DataRow domainNameRow = dtTblDomainNames.NewRow();
                        domainNameRow["sDomainName"] = item;
                        domainNameRow["sDomainType"] = domainType;
                        dtTblDomainNames.Rows.Add(domainNameRow);
                    }
                }

                company.DomainDataTable = dtTblDomainNames;

                var result = BL_CorporateCompanyM.AddEditCorporateCompany(company);

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

        [HttpPost]
        public ActionResult DeleteCompany(int id)
        {
            if (ModelState.IsValid)
            {
                var company = new eCorporateCompanyM();
                company.iCompanyId = id;

                var result = BL_CorporateCompanyM.DeleteCorporateCompany(company);

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
        public ActionResult ToggleStatus(int companyId, bool status)
        {
            if (ModelState.IsValid)
            {
                var result = BL_CorporateCompanyM.ToggleStatus(companyId, status);

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


        [HttpGet]
        public JsonResult BindStates(int countryId)
        {
            var results = BL_tblStateM.GetAllRecordsById(countryId).Select(x => new { x.iStateId, x.sState });
            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetStatesByGST(string gstFirstTwoDigit)
        {
            var results = BL_tblStateM.GetSingleRecordByGST(gstFirstTwoDigit);
            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult BindCity(int stateId)
        {
            var results = BL_tblCityM.GetAllRecordsById(stateId).Select(x => new { x.iCityId, x.sCity });
            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);
        }
    }
}