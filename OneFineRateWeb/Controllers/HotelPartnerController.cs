using FutureSoft.Util;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
namespace OneFineRateWeb.Controllers
{
    public class HotelPartnerController : BaseController
    {
        // GET: HotelPartner
        public ActionResult Index()
        {
            return View(new etblHotelPartnerM());
        }

        [HttpPost]
        public ActionResult Index(etblHotelPartnerM model)
        {

            if (ModelState.IsValid)
            {
                if (model.bIsAgree == true)
                {
                    model.dtActionDate = DateTime.Now;
                    model.iID = 1;
                    int result = BL_tblHotelPartner.AddRecord(model);
                    if (result == 1)
                    {
                        TempData["msg"] = "Submitted Successfully";

                        string sToEmail = model.sEmail;
                        string sCcEmail = "";
                        string sBCcEmail = "";
                        string sSubject = "Hotel Partner  Application Submitted Successfully";
                        string sBody = "Your application has been submitted successfully. Thank you.";
                        MailComponent.SendEmail(sToEmail, sCcEmail, sBCcEmail, sSubject, sBody, null, null, true, null, null);

                        string adminEmail = BL_tblHotelPartner.GetPartnerEmailId();
                        string adminsCcEmail = "himanshuS@futuresoftindia.com";
                        string adminsBCcEmail = "";
                        string adminsSubject = "Hotel Partner  Application Submitted Successfully";
                        string adminsBody = "<table border='1'>";
                        adminsBody += "<tr><td>Hotel Name: </td><td>" + model.sHotelName + "</td>";
                        adminsBody += "<tr><td>Star Category: </td><td>" + model.iStarCategory + "</td>";
                        adminsBody += "<tr><td>Hotel Website: </td><td>" + model.sWebsiteUrl + "</td>";
                        adminsBody += "<tr><td>Address: </td><td>" + model.sAddress + "</td>";
                        adminsBody += "<tr><td>City: </td><td>" + model.sCity + "</td>";
                        adminsBody += "<tr><td>State : </td><td>" + model.sState + "</td>";
                        adminsBody += "<tr><td>Country : </td><td>" + model.sCountry + "</td>";
                        adminsBody += "<tr><td>Pin Code: </td><td>" + model.sPIN + "</td>";
                        adminsBody += "<tr><td>Board line Number: </td><td>" + model.sBoardLineNumber + "</td>";
                        adminsBody += "<tr><td>First Name: </td><td>" + model.sFirstName + "</td>";
                        adminsBody += "<tr><td>Last Name: </td><td>" + model.sLastName + "</td>";
                        adminsBody += "<tr><td>Designation: </td><td>" + model.sDesignation + "</td>";
                        adminsBody += "<tr><td>Email Address: </td><td>" + model.sEmail + "</td>";
                        adminsBody += "<tr><td>Mobile Number: </td><td>" + model.sMobile + "</td>";
                        adminsBody += "</table>";
                        MailComponent.SendEmail(adminEmail, adminsCcEmail, adminsBCcEmail, adminsSubject, adminsBody, null, null, true, null, null);
                        return RedirectToAction("Index");
                    }
                    else if (result == 2)
                    {
                        TempData["Error"] = "Hotel with same Email Id already exists.";
                        return View(model);
                    }
                }
                else
                {
                    TempData["Error"] = "Please read and agree on Terms and Conditions.";
                    return View(model);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }

            return View(model);

        }

        [HttpGet]
        public ActionResult BecomeCorporate()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult BecomeCorporate(eCorporateCompanyM model)
        {
            try
            {
                model.dtActionDate = DateTime.Now;

                if (!string.IsNullOrEmpty(model.sEmailAddress))
                {
                    TempData["msg"] = "Thank you for your Registration. Our executive will contact you shortly.";

                    string sToEmail = model.sEmailAddress;
                    string sCcEmail = "";
                    string sBCcEmail = "";
                    string sSubject = "Become Corporate Application Submitted Successfully";
                    string sBody = "Your application has been submitted successfully. Thank you.";
                    MailComponent.SendEmail(sToEmail, sCcEmail, sBCcEmail, sSubject, sBody, null, null, true, null, null);

                    string adminEmail = BL_tblHotelPartner.GetPartnerEmailId();
                    string adminsCcEmail = "";
                    string adminsBCcEmail = "";
                    string adminsSubject = "Corporate Partner Application Submitted Successfully";
                    string adminsBody = "<table border='1'>";
                    adminsBody += "<tr><td>Company Name: </td><td>" + model.sCompanyName + "</td>";
                    adminsBody += "<tr><td>Email Address: </td><td>" + model.sEmailAddress + "</td>";
                    adminsBody += "<tr><td>Telephone Number: </td><td>" + model.sTelephoneNumber + "</td>";
                    adminsBody += "<tr><td>Mobile Number: </td><td>" + model.sMobileNumber + "</td>";
                    //adminsBody += "<tr><td>Action Date: </td><td>" + DateTime.Now + "</td>";
                    adminsBody += "</table>";
                    MailComponent.SendEmail(adminEmail, adminsCcEmail, adminsBCcEmail, adminsSubject, adminsBody, null, null, true, null, null);
                }
                else
                {
                    ModelState.AddModelError("", "Email Address Required!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                return View(model);
            }
            ModelState.Clear();
            return View();
        }

    }
}