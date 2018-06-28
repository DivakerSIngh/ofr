using OfficeOpenXml;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class ExchangeController : BaseController
    {
        // GET: Exchange
        [Route("Exchange")]
        public ActionResult Index()
        {
            if (TempData["Data"] != null)
            {
                ViewData["Data"] = TempData["Data"].ToString();
            }
            else
            {
                ViewData["Data"] = "";
            }
            return View();
        }

        public ActionResult Upload(FormCollection formCollection)
        {
            string msg = "";
            if (Request != null)
            {
                try
                {
                    var ERList = new List<ExchangeRate>();
                    int user = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            if (workSheet.Cells[1, 1].Value.ToString() == "From" && workSheet.Cells[1, 2].Value.ToString() == "To" && workSheet.Cells[1, 3].Value.ToString() == "Rate")
                            {
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    var ER = new ExchangeRate();
                                    ER.sCurrencyCodeFrom = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    ER.sCurrencyCodeTo = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    ER.dRate = Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value.ToString());
                                    ERList.Add(ER);
                                }
                            }
                            else
                            {
                                msg = "Incorrect excel file!";
                                TempData["Data"] = msg;
                                return RedirectToAction("index", "Exchange");
                            }
                        }
                    }
                    if (ERList.Count > 0)
                    {
                        int a = BL_ExchangeRate.UpdateRecord(ERList, user);
                        if (a == 1)
                        {
                            msg = "Exchange rates uploaded successfully.";
                        }
                        else
                        {
                            msg = "Kindly try after some time.";
                        }
                    }
                    else
                    {
                        msg = "No records provided";
                    }
                }
                catch (Exception)
                {
                    msg = "Incorrect excel file!";
                }
            }
            TempData["Data"] = msg;
            return RedirectToAction("index", "Exchange");
        }

        public string Delete(string from, string to)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_ExchangeRate.DeleteRecord(from.Substring(0, 3), to.Substring(0, 3));
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
    }
}