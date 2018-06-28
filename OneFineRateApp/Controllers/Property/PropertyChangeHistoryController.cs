using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace OneFineRateApp.Controllers.Property
{
    [Authorize]
    public class PropertyChangeHistoryController : BaseController
    {
        // GET: PropertyChangeHistory
        [Route("PropertyChangeHistory")]
        public ActionResult Index()
        {
            etblChangeHistory obj = new etblChangeHistory();
            return View(obj);
        }

        public ActionResult FillRatePlan(int RoomType)
        {
            int PropId = Convert.ToInt32(Session["PropId"].ToString());
            var rateplan = BL_tblChangeHistory.GetRatePlanByRoomID(RoomType,PropId);
            return Json(rateplan, JsonRequestBehavior.AllowGet);
        }
    }
}