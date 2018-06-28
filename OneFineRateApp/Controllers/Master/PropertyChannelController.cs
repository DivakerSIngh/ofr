using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class PropertyChannelController : BaseController
    {
        // GET: PropertyChannel
        [Route("PropertyChannel")]
        public ActionResult Index()
        {
            return View();
        }

        public string Delete(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_ChannelManager.DeleteRecord(id);
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

        public string Update(int PropId, int ChannMgr)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int user = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                int i = BL_ChannelManager.UpdateRecord(PropId, ChannMgr, user);
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

        public string Check(int PropId, int ChannMgr)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int user = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                int i = BL_ChannelManager.CheckRecord(PropId, ChannMgr);
                if (i == 1)
                {
                    result = new { st = 1, msg = "This property is mapped with the same Channel Manager. Do you want to reassign?" };
                }
                else if (i == 2)
                {
                    result = new { st = 1, msg = "This property is mapped with the some other Channel Manager. Do you want to reassign?" };
                }
                else
                {
                    result = new { st = 0, msg = "This property is not mapped with any Channel Manager. Do you want to assign?" };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string GetProperties()
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                List<PNames> AM = new List<PNames>();
                AM = BL_ChannelManager.GetAllPropertyName();

                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(AM);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string GetChannelMgr()
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                List<PNames> AM = new List<PNames>();
                AM = BL_ChannelManager.GetAllChannelManagers();

                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(AM);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

    }
}