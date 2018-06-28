using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_Menu
    {

        public static MvcHtmlString GetMenu(int userid, int menutype)
        {

            // return MvcHtmlString.Create("<div id='cssmenu'><ul><li><a href='#'>Home</a></li><li class='has-sub'><a href='#'>Products</a>  <ul><li class='has-sub'><a href='#'>Product 1</a><ul><li><a href='#'>Sub Product</a></li><li><a href='#'>Sub Product</a></li></ul></li><li class='has-sub'><a href='#'>Product 2</a><ul><li><a href='#'>Sub Product</a></li><li class='last'><a href='#'>Sub Product</a></li></ul></li></ul></li><li><a href='#'>About</a></li><li class='last'><a href='#'>Contact</a></li></ul></div>");
            string Message = "";//, breadcrumb;
            List<eMenu> objMenuLst = new List<eMenu>();
            objMenuLst = GetAllMenu(userid, menutype);

            if (objMenuLst.Count == 0)
            {
                Message = "No Menu Found For This User.";
            }


            StringBuilder strMenu = new StringBuilder();
            strMenu.AppendLine("<div id=\"cssmenu\"><ul>");
            foreach (eMenu obj in objMenuLst.Where(p => p.iParentId == -1))
            {
                if (obj.sPath == "#")
                    strMenu.AppendLine("<li class='has-sub'><a href=" + obj.sPath + ">" + obj.sMenuName + "</a>");
                else
                    strMenu.AppendLine("<li><a href=" + obj.sPath + ">" + obj.sMenuName + "</a>");
                strMenu.AppendLine(GenrateSubMenu(obj.iMenuId, objMenuLst));
                strMenu.AppendLine("</li>");
            }
            strMenu.AppendLine("</ul></nav>");
            // breadcrumb = strMenu.ToString();
            var res = strMenu.ToString();
            return MvcHtmlString.Create(strMenu.ToString());
        }

        protected static string GenrateSubMenu(int parentid, List<eMenu> objMenuLst)
        {
            List<eMenu> objMenuLstTemp = objMenuLst;
            StringBuilder strSubMenu = new StringBuilder();
            int i = 0;
            foreach (eMenu objSubMenu in objMenuLstTemp.Where(p => p.iParentId == parentid).OrderBy(p => p.iDispSeq))
            {
                if (i == 0) strSubMenu.Append("<ul>");
                strSubMenu.Append("<li><a href=" + objSubMenu.sPath + ">" + objSubMenu.sMenuName + "</a>");
                strSubMenu.Append(GenrateSubMenu(objSubMenu.iMenuId, objMenuLst));
                strSubMenu.Append("</li>");
                i++;
            }
            if (i > 0) strSubMenu.Append("</ul>");
            return strSubMenu.ToString();
        }
        public static MvcHtmlString GetPropertyMenu(int userid)
        {
            string Message = "";
            List<eMenu> objMenuLst = new List<eMenu>();
            objMenuLst = GetPropertyMenuByUserId(userid);

            if (objMenuLst.Count == 0)
            {
                Message = "No Menu Found For This User.";
            }


            StringBuilder strMenu = new StringBuilder();
            strMenu.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"menubgtable\">");
            //int i = 0;
            foreach (eMenu obj in objMenuLst)
            {
                //if (i == 0)
                //{
                //    strMenu.AppendLine("<tr class=\"active\">");
                //}
                //else
                //{
                strMenu.AppendLine("<tr class=\"menubg\">");
                //}
                strMenu.AppendLine("<td style=\"border-top:0px;\"><a href=" + obj.sPath + ">" + obj.sMenuName + "</a></td>");
                strMenu.AppendLine("<td style=\"border-top:0px;\">&nbsp;</td>");
                //i++;
            }
            strMenu.AppendLine("</table>");
            var res = strMenu.ToString();
            return MvcHtmlString.Create(strMenu.ToString());
        }
        public static bool ValidateUserAccess(string ControllerName, string ActionName)
        {
            bool bIsValid = false;
            List<ControllerAction> objControllerActionLst = new List<ControllerAction>();

            objControllerActionLst = HttpContext.Current.Session["ControllerAction"] as List<ControllerAction>;
            foreach (ControllerAction obj in objControllerActionLst)
            {
                if (obj.ControllerName == "PendingNegotiations")
                    HttpContext.Current.Session["PendingNegotiations"] = true;
                //if (ControllerName == obj.ControllerName && (ActionName == obj.ActionName || ActionName.Replace("Index", "") == ""))
                if (ControllerName.Equals(obj.ControllerName,StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return bIsValid;
        }
        public static List<eMenu> GetPropertyMenuByUserId(int UserId)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.Database.SqlQuery<eMenu>("uspGetPropertyMenuByUserId @UserID", new SqlParameter("@UserID", UserId));

                return result.ToList();
            }

        }
        public static List<eMenu> GetAllMenu(int UserId, int menutype)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@iUserId", UserId);
                MyParam[1] = new SqlParameter("@menutype", menutype);
                var result = _context.Database.SqlQuery<eMenu>("uspMenuSelectByUserId @iUserId, @menutype", MyParam);

                return result.ToList();
            }

        }
        public static string GetHomePageByUserId(int UserId)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.Database.SqlQuery<eMenu>("uspGetHomeMenuByUserId @UserID", new SqlParameter("@UserID", UserId)).ToList();
                if (result.Count > 0)
                {
                    return result[0].sPath;
                }
                else
                {
                    return null;
                }

            }

        }
        public static bool ValidateLinkForUser(int UserId, string ControllerName, string ActionName)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[3];
                MyParam[0] = new SqlParameter("@iUserId", UserId);
                MyParam[1] = new SqlParameter("@ControllerName", ControllerName);
                MyParam[2] = new SqlParameter("@ActionName", ActionName);
                var result = _context.Database.SqlQuery<clsValid>("uspControllerActionByUserId @iUserId, @ControllerName, @ActionName", MyParam);

                bool i;
                i = Convert.ToBoolean(result.ToList()[0].Valid);

                return i;
            }

        }
        public static List<ControllerAction> GetControllerActionByUserId(int UserId)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.Database.SqlQuery<ControllerAction>("uspGetControllerActionByUserId @iUserId", new SqlParameter("@iUserId", UserId));

                return result.ToList();
            }

        }
    }
}