using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Data.Entity.Validation;
using System.Globalization;
using OneFineRateAppUtil;
using System.IO;
using System.Configuration;

namespace OneFineRateApp.Controllers.Property
{
    public class BankDetailViewController : BaseController
    {
        // GET: BankDetailView
        public ActionResult Index()
        {
            ViewBag.FixedOFRFinanceDetails = BL_tblBankDetailM.GetFixedOFRFinanceDetails();
            var bankDetail = GetBankDetailByPropId();
            var rootBlobPath = ConfigurationManager.AppSettings["BlobUrl"];
            bankDetail.sCancelledCheque = rootBlobPath + bankDetail.sCancelledCheque;
            bankDetail.sLetterhead_BankAccount = rootBlobPath + bankDetail.sLetterhead_BankAccount;
            bankDetail.sPanCard = rootBlobPath + bankDetail.sPanCard;
            return View(bankDetail);
        }
        public etblBankDetail GetBankDetailByPropId()
        {
            etblBankDetail objBank = new etblBankDetail();
            try
            {
                objBank = BL_tblBankDetailM.GetSingleRecordById(Convert.ToInt32(Session["PropId"].ToString()));
            }
            catch (Exception)
            {
                throw;
            }
            return objBank;
        }
    }
}