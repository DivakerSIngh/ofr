using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Web.Routing;
using OneFineRateWeb.Models;

namespace OneFineRateWeb.Controllers
{
    public class HomeController : BaseController
    {
        //[OutputCache(Duration = 10, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult Index()
        {
            //throw new System.ArgumentException("Parameter cannot be null", "original");
            //TO DO TEST MAIL
            //FutureSoft.Util.MailComponent.SendEmail("himanshuS@futuresoftindia.com", "", null, "OneFineRate-New Booking! Confirmation No:", "", null, null, true);

            //***************LIVE**************
            //ViewBag.banners = new object();
            //etblVideoUrlM vidobj = new etblVideoUrlM();
            //ViewBag.url = vidobj.sVideoUrl + "?rel=0&autoplay=1";
            //ViewBag.urlImg = "http://ofrblobstorage.blob.core.windows.net" + vidobj.sImgUrl;
            //ViewBag.PromotionalHotels = new object();
            //ViewBag.TopBannerData = new object();
            //TempData["VideoUrl"] = BL_tblVideoUrlM.GetParameters().sVideoUrl;
            //return View("ComingSoon");

            //*******************UAT************
            etblVideoUrlM vidobj = new etblVideoUrlM();
            ViewBag.banners = BL_tblBannerM.GetAllBannersRecords();
            vidobj = BL_tblVideoUrlM.GetParameters();
            ViewBag.url = string.IsNullOrEmpty(vidobj.sVideoUrl) ? "" : vidobj.sVideoUrl.Replace("watch?v=", "embed/") + "?rel=0&autoplay=1";
            ViewBag.urlImg = string.IsNullOrEmpty(vidobj.sImgUrl) ? "" : System.Configuration.ConfigurationManager.AppSettings["BlobUrl"].ToString() + (vidobj.sImgUrl.IndexOf("/") == 0 ? vidobj.sImgUrl.Substring(1) : vidobj.sImgUrl);
            ViewBag.PromotionalHotels = BL_WebsiteHomePage.GetAllPromotionalHotels(Convert.ToString(Session["CurrencyCode"]));
            ViewBag.TopBannerData = OneFineRateAppUtil.clsUtils.ConvertToJson(((List<OneFineRateBLL.Entities.etblBannerM>)ViewBag.banners).Where(item => item.sBannerType == "Page Top"));
            return View();
        }

        public ActionResult UnAuthrizedCorporate()
        {
            TempData["ERROR"] = "Sorry! You are not a authorized corporate customer.";
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string GetSavings(string Id, string Ctype, int Index)
        {
            object result = null;
            string strReturn = string.Empty;
            List<eRecentSavings> lst = new List<eRecentSavings>();
            try
            {
                int TypeId = Id == "" ? 0 : Convert.ToInt32(Id);

                lst = BL_WebsiteHomePage.GetAllRecentSavvings(TypeId, Ctype, Convert.ToInt16(Index), Convert.ToString(Session["CurrencyCode"]));
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(lst);
            return strReturn;
        }
        public string GetCountriesFLags(string Code)
        {
            object result = null;
            string strReturn = string.Empty;
            List<eCurrencyFlags> lst = new List<eCurrencyFlags>();
            try
            {
                lst = BL_WebsiteHomePage.GetAllCurrenyFlags(Code);
                Session["Currency"] = "INR";
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(lst);
            return strReturn;
        }
        public string FetchCountryDetails()
        {
            object result = null;
            string strReturn = string.Empty;
            eLocationDetails obj = new eLocationDetails();
            try
            {

                obj = BL_WebsiteHomePage.GetUserCountryDetails();
                Session["CurrencyCode"] = obj.CurrencyCode;

            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(obj);
            return strReturn;
        }


        [OutputCache(Duration = 60 * 60 * 24, VaryByParam = "*")] // Cached for 1 day
        public string GetSearchData(string txt)
        {
            object result = null;
            string strReturn = string.Empty;
            List<eSearchData> lst = new List<eSearchData>();
            try
            {
                lst = BL_WebsiteHomePage.GetSearchData(txt).ToList();
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(lst);
            return strReturn;
        }
        public ActionResult ChangeCurrency(string CountryCode)
        {

            List<eCurrencyFlags> lst = new List<eCurrencyFlags>();
            lst = BL_WebsiteHomePage.GetAllCurrenyFlags(CountryCode);
            if (lst.Count > 0)
            {
                Session["CountryList"] = OneFineRateAppUtil.clsUtils.ConvertToJson(lst);
                string CurrenyCode = lst.Where(u => u.iActive == true).FirstOrDefault().sCurrencyCode;
                Session["CurrencyCode"] = CurrenyCode;
            }
            else
            {
                Session["CountryList"] = string.Empty;
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSearchBarForm()
        {
            return PartialView("pSearchBar");
        }


        public void SendTestMail()
        {
            //TO DO TEST MAIL
            FutureSoft.Util.MailComponent.SendEmail("himanshuS@futuresoftindia.com", "", null, "OneFineRate-New Booking! Confirmation No:", "", null, null, true, null, null);
        }

        

        [HttpPost]
        public ActionResult SubmitQuery(string Name, string Organization,string Email,string PhoneNumber)
        {
            try
            {
                var EmailData =  BL_tblEmailSettingsM.GetRecord("RevenueManager");
                FutureSoft.Util.MailComponent.SendEmail(EmailData.sEmail.Split(',')[0] /*"himanshuS@futuresoftindia.com"*/, "", null,"Query - One Fine Rate Contact", "New Query has been came from a user, Please Find Details Below \n Name : " + Name + "\n Organization : " + Organization + "\n Email Address : " + Email + "\n Phone Number : " + PhoneNumber, null, null, true, null, null);
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
                throw;
            }
        }

    }
}