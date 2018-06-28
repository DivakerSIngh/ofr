using OneFineRateAppUtil;
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
    public class GroupsController : BaseController
    {
        // GET: Groups

        [Route("Groups")]
        public ActionResult Groups()
        {
            return View();
        }
        public string AddGroups(string Groupname, string Desc, bool Active, string Menus)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eGroups eObj = new eGroups();
                eObj.sGroupName = Groupname;
                eObj.sDescription = Desc;
                eObj.dtCreationDate = DateTime.Now;
                eObj.dtActionDate = DateTime.Now;
                eObj.cStatus = Active ? "A" : "D";
                eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                eObj.iCreatedBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Groups.AddRecord(eObj, Menus);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Group", 1) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("", 6) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Group", 0) };
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string DeleteGroups(int id)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_Groups.DeleteRecord(id);
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

        public string UpdateGroups(int id, string Groupname, string Desc, bool Active, string Menus)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                eGroups Obj = new eGroups();
                Obj = BL_Groups.GetSingleRecordById(id);
                Obj.sGroupName = Groupname;
                Obj.sDescription = Desc;
                //Obj.dtCreationDate = DateTime.Now;
                Obj.dtActionDate = DateTime.Now;
                Obj.cStatus = Active ? "A" : "I";
                Obj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                int i = BL_Groups.UpdateRecord(Obj, Menus);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("Group", 2) };
                }
                else if (i == 2)
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("", 6) };
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("Group", 0) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
    }
}