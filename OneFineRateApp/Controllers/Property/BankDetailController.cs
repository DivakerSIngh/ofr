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
    public class BankDetailController : BaseController
    {
        // GET: BankDetail
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.FixedOFRFinanceDetails = BL_tblBankDetailM.GetFixedOFRFinanceDetails();
            var bankDetail = GetBankDetailByPropId();
            var rootBlobPath = ConfigurationManager.AppSettings["BlobUrl"];
            bankDetail.sCancelledCheque = rootBlobPath + bankDetail.sCancelledCheque;
            bankDetail.sLetterhead_BankAccount = rootBlobPath + bankDetail.sLetterhead_BankAccount;
            bankDetail.sPanCard = rootBlobPath + bankDetail.sPanCard;
            return View("BankDetail", bankDetail);
        }

        [HttpPost]
        public ActionResult Add(etblBankDetail bank)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    bank.dtActionDate = DateTime.UtcNow;
                    if (Session["UserDetails"] != null)
                    {
                        bank.iActionBy = Convert.ToInt16(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                    }

                    var containerName = Convert.ToInt32(Session["PropId"].ToString()).ToString();

                    if (bank.CancelledCheque != null)
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            bank.CancelledCheque.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();
                            // Since old file will automatically replaced by new one (as their name is same ) , So no need to delete exiting one.
                            var absoluteUrl = clsUtils.fnUploadFileINBlobStorage(containerName, "cancelledcheque" + Path.GetExtension(bank.CancelledCheque.FileName), data, false);
                            bank.sCancelledCheque = containerName + "/cancelledcheque" + Path.GetExtension(bank.CancelledCheque.FileName);

                        }
                    }
                    if (bank.PanCard != null)
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            bank.PanCard.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();
                            var absoluteUrl = clsUtils.fnUploadFileINBlobStorage(containerName, "pancard" + Path.GetExtension(bank.PanCard.FileName), data, false);
                            bank.sPanCard = containerName + "/pancard" + Path.GetExtension(bank.PanCard.FileName);

                        }
                    }
                    if (bank.Letterhead_BankAccount != null)
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            bank.Letterhead_BankAccount.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();
                            var absoluteUrl = clsUtils.fnUploadFileINBlobStorage(containerName, "letterheadbankaccount" + Path.GetExtension(bank.Letterhead_BankAccount.FileName), data, false);
                            bank.sLetterhead_BankAccount = containerName + "/letterheadbankaccount" + Path.GetExtension(bank.Letterhead_BankAccount.FileName);
                        }
                    }

                    var returnValue = BL_tblBankDetailM.AddOrUpdateRecord(bank);

                    if (returnValue == 1)
                    {
                        TempData["msg"] = "Details has been added";
                    }
                    else if (returnValue == 2)
                    {
                        TempData["msg"] = "Details has been modified";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ERROR"] = ex.Message;
                }
            }
            else
            {
                TempData["ERROR"] = "Validation failed ! Please correct the errors and try again.";
            }
            return View("BankDetail", bank);
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

        [HttpGet]
        public ActionResult AdditionalCommision()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddEditCommission(int? commissionId)
        {
            var PropId = Convert.ToInt32(Session["PropId"].ToString());
            decimal value = BL_AdditionalCommission.GetPropertyCommission(PropId);

            if (commissionId.HasValue && commissionId != 0)
            {
                etblAdditionalCommission obj = BL_AdditionalCommission.GetSingleRecordById(commissionId.Value);
                obj.baseDCommission = value;
                ViewBag.HeaderText = "Edit Commission";
                return PartialView("_AddCommission", obj);
            }
            else
            {
                ViewBag.HeaderText = "Add Commission";
                return PartialView("_AddCommission", new etblAdditionalCommission() { baseDCommission = value });
            }
        }

        [HttpPost]
        public ActionResult AddEditCommission(etblAdditionalCommission model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string sStartDate = model.dtCommisionStartDate;
                    string sEndDate = model.dtCommisionEndDate;

                    model.iPropId = Convert.ToInt32(Session["PropId"].ToString());
                    model.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                    model.dtCommisionStartDate = clsUtils.ConvertddmmyyyytommDDyyyy(model.dtCommisionStartDate);
                    model.dtCommisionEndDate = clsUtils.ConvertddmmyyyytommDDyyyy(model.dtCommisionEndDate);

                    var startDate = DateTime.ParseExact(model.dtCommisionStartDate, "mm/dd/yyyy", null);
                    var endDate = DateTime.ParseExact(model.dtCommisionEndDate, "mm/dd/yyyy", null);

                    if(startDate > endDate)
                    {
                        return Json(new { status = -1, msg = "Start Date must be less than End Date ! " }, JsonRequestBehavior.AllowGet);
                    }

                    string statusMessage = string.Empty;

                    int status = BL_AdditionalCommission.AddUpdateRecord(model);

                    if (status == 0)
                    {
                        statusMessage = "Commission already exist between " + sStartDate + " and " + sEndDate;
                    }
                    else if (model.iAdditionalCommisionId != 0 && status == 1)
                    {
                        statusMessage = "Commission updated successfully";
                    }
                    else
                    {
                        statusMessage = "Commission added successfully";
                    }

                    return Json(new { status = status, msg = statusMessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { status = -1, msg = "Some unknown error has occurred ! Please try after some time." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = -1, msg = "Valiation error..." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [ValidateInput(false)]
        public JsonResult UpdateCommissionStatus(int commissionId, bool status)
        {
            object result = null;
            try
            {
                var i = BL_AdditionalCommission.UpdateCommissionStatus(commissionId, status);
                if (i == 1)
                {
                    if (status)
                    {
                        result = new { status = 1, msg = "Commission enabled successfully." };
                    }
                    else
                    {
                        result = new { status = 1, msg = "Commission disabled successfully." };
                    }

                }
                else
                {
                    result = new { status = -1, msg = "Some unknown error has occurred ! Please try after some time." };
                }
            }
            catch (Exception)
            {
                result = new { status = -1, msg = "Some unknown error has occurred ! Please try after some time." };

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}