using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class RoomRateRangeController : BaseController
    {
        [Route("RoomRateRange")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddUpdate(int? id)
        {
            var model = new etblRoomRateRangeM();
            model.iRoomRateRangeId = 0;

            if (id.HasValue)
            {
                model = BL_RoomRateRangeM.GetRateRangeById(id.Value);
            }

            return PartialView("_AddUpdate", model);
        }

        [HttpPost]
        public ActionResult AddUpdate(etblRoomRateRangeM model)
        {
            try
            {
                string message = string.Empty;

                if (ModelState.IsValid)
                {
                    model.dtActionDate = DateTime.Now;
                    model.iActionBy = ((BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    model.dAmountFrom = model.dAmountFrom;
                    model.dAmountTo = model.dAmountTo;

                    var result = BL_RoomRateRangeM.AddOrUpdate(model);

                    if (result.Key == -1)
                    {
                        return Json(new { status = false, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                    }
                    else if (result.Key == 1)
                    {
                        return Json(new { status = true, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { status = true, Msg = message }, JsonRequestBehavior.AllowGet);
                }

                message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return Json(new { status = false, Msg = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Msg = "An error occured while adding the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteRoomRateRange(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = BL_RoomRateRangeM.DeleteRoomRateRange(id);

                    return Json(new { status = true, Msg = "Record deleted successfully." }, JsonRequestBehavior.AllowGet);
                }

                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return Json(new { status = false, Msg = message }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { status = false, Msg = "An error occured while deleting the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ToggleStatus(int id, bool status)
        {
            try
            {
                var result = BL_RoomRateRangeM.ToggleStatus(id, status);

                if (result.Key == -1)
                {
                    return Json(new { status = false, Msg = result.Value }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = true, Msg = result.Value }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { status = false, Msg = "An error occured while deleting the record, Kindly try after some time." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}