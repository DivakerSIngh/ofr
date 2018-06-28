using ClosedXML.Excel;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class MasterTaxController : BaseController
    {
        [Route("MasterTax")]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AddUpdate(int? id)
        {
            var model = new etblMasterTaxMap();

            if (id.HasValue)
            {
                var dbModel = BL_tblMasterTaxMap.GetMasterTaxMapById(id.Value);

                if (dbModel != null)
                {
                    model = dbModel;

                    model.sStayFrom = string.Format("{0:dd/MM/yyyy}", dbModel.dtStayFrom);
                    model.sStayTo = string.Format("{0:dd/MM/yyyy}", dbModel.dtStayTo);
                    model.iStateIds = dbModel.iStateIdList.ToArray();
                }
            }
            else
            {
                //model.ListTaxes = BL_Tax.GetAllTax().Where(x => x.sTaxName.ToLower() == "sgst" || x.sTaxName.ToLower() == "cgst").ToList();
                model.ListTaxes = BL_Tax.GetAllTax().Where(x => x.cStatus.ToUpper() == "A").ToList();
            }

            model.ListRoomRateRange = BL_tblMasterTaxMap.GetRoomRateRange();

            return PartialView("_AddUpdate", model);
        }


        [HttpPost]
        public ActionResult AddUpdate(etblMasterTaxMap model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.dtActionDate = DateTime.Now;
                    model.iActionBy = ((BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    model.dtStayFrom = DateTime.ParseExact(model.sStayFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    model.dtStayTo = DateTime.ParseExact(model.sStayTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    var result = BL_tblMasterTaxMap.AddUpdateMasterTaxMapping(model);

                    if (result.Key == 1 || result.Key == 2)
                    {
                        return Json(new { status = true, Msg = result.Value, statusCode = result.Key }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { status = false, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }

                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return Json(new { status = false, Msg = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Msg = "An error occured while adding the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult ToggleStatus(int id, bool status)
        {
            try
            {
                var iActionBy = ((BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                var result = BL_tblMasterTaxMap.ToggleStatus(id, status, iActionBy);

                return Json(new { status = result.Key == 1, Msg = result.Value }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { status = false, Msg = "An error occured while updating the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
        }

        public void ExportToExcel(string taxIds)
        {
            try
            {
                if (string.IsNullOrEmpty(taxIds))
                {
                    throw new Exception("Property Ids can not be null or empty when exporting to excel.");
                }

                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@sIds", taxIds);

                DataTable dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(),
                    CommandType.StoredProcedure, "[uspGetMasterTaxMappingsToExport]", MyParam).Tables[0];

                using (var wb = new XLWorkbook())
                {
                    wb.Properties.Title = "MinimumIncomeReport";
                    wb.Properties.Company = "OneFineRate";
                    wb.Worksheets.Add(dt, "MinimumIncomeReport1");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=MasterTaxMappings.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                        System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Something went wrong !!! Details : " + ex.Message + " ')</script>");
            }
        }
    }
}