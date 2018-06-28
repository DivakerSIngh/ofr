using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using System.Text;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Data;
using ClosedXML.Excel;

namespace OneFineRateApp.Services
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllRecords()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eChains> list = BL_Chains.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllRecords1()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eCountries> list = BL_Country.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        /// <summary>
        /// Get the list of all corporate company details to show in grid
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetCorporateCompanies()
        {

            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eCorporateCompanyM> list = BL_CorporateCompanyM.GetCorporateCompanies(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }

            return outputJson;
        }



        /// <summary>
        /// Get the list of all corporate company details to show in grid
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetBlackListDomains()
        {

            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eBlackListedDomainM> list = BL_BlackListedDomainM.GetBlackListDomains(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }

            return outputJson;
        }


        /// <summary>
        /// Get the list of all corporate company details to show in grid
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetServiceChargeData()
        {

            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eServiceChargeM> list = BL_ServiceChargeM.GetServiceCharges(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }

            return outputJson;
        }



        /// <summary>
        /// Get the list of all corporate company details to show in grid
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetRoomRateRangeData()
        {

            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblRoomRateRangeM> list = BL_RoomRateRangeM.GetRateRanges(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }

            return outputJson;
        }



        /// <summary>
        /// Get the list of all corporate company details to show in grid
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetHotelsForMimimumIncomeSetup()
        {

            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);
            
            int cityId = 0;

            var starCategoryIds = Convert.ToString(HttpContext.Current.Request.Params["starCategory"]);
            var propertyIds = Convert.ToString(HttpContext.Current.Request.Params["propertyIds"]);
            var stateIds = Convert.ToString(HttpContext.Current.Request.Params["stateIds"]);
            int.TryParse(HttpContext.Current.Request.Params["cityId"], out cityId);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eMinimumIncome> list = BL_MinimumIncome.GetHotelList(tblobj,starCategoryIds, propertyIds, stateIds, cityId, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }

            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllUsers()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eUserList> list = BL_Users.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllGroups()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<GroupsForGrid> list = BL_Groups.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllStateRecords()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblStateM> list = BL_tblStateM.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllMacroAreaRecords()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eMacroAreaMList> list = BL_tblMacroAreaM.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllLocalityRecords()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblLocalityList> list = BL_Locality.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetGroupForUserMap()
        {
            return BL_Users.getGroupName();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetUserMappedGroups()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            return BL_Users.getMappedGroups(a);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllProperties()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblPropertyM> list = BL_tblPropertyM.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetMenuForGroupMap()
        {
            int id = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            var menuList = BL_Groups.getMenuName(id);
            return menuList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetGroupMappedMenus()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            return BL_Groups.getMappedMenus(a);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public List<etblPropertyPromotionMap> GetPropertyPromoDataByID()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            return BL_tblPromotionManagement.getPropertyPromoDataByID(a);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetUserMappedHotels()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            string s = HttpContext.Current.Request.Params["s"];
            return BL_UserPropertyMap.GenrateMenu(a, s);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetUserMappedHotelsForShow()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            return BL_UserPropertyMap.GenrateMenuForMappingShow(a);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllPropertyDining()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int propid = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblPropertyDiningMap> list = BL_tblPropertyDiningMap.getRecordForSearch(tblobj, propid, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllPropertyRooms()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int propid = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblPropertyRoomMap> list = BL_tblPropertyRoomMap.getRecordForSearch(tblobj, propid, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        // Added For Additional Commission Details
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllPropertyCommission()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int propid = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblAdditionalCommission> list = BL_AdditionalCommission.getRecordForSearch(tblobj, propid, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllPropertyPromoRecords()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int id = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            int PropId = Convert.ToInt32(Session["PropId"].ToString());

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblPropertyPromotionMap> list = BL_tblPromotionManagement.getRecordForSearch(tblobj, out TotalCount, id, PropId).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllAmenities()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eAmenityDisp> list = BL_Amenity.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllTax()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eTaxDisp> list = BL_Tax.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllRatePlan()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eRatePlanDisp> list = BL_RatePlan.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllPropertyRatePlan()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["PropId"]);
            List<etblPropertyRatePlanMap> list = BL_tblPropertyRatePlanMap.getRecordForSearch(tblobj, out TotalCount, a).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllUserPropertyList()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eUsersPropertyMap> list = BL_tblPropertyM.GetUserPropertyList(tblobj, a, out TotalCount).ToList();
            if (list.Count > 0)
            {

                foreach (var item in list)
                {
                    item.sWebSite = System.Configuration.ConfigurationManager.AppSettings["OFRBaseUrl"].ToString() + "OfferReview?Id=" + OneFineRateAppUtil.clsUtils.Encode(item.iPropId.ToString());
                }
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        
        public string GetPropertyTaxes()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int propid = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);
            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<PropertyTaxList> list = BL_tblPropertyTaxMap.getRecordForSearch(tblobj, propid, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetMasterTaxeMappings()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);
            
            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblMasterTaxMapForDatatable> list = BL_tblMasterTaxMap.GetMasterTaxMappings(tblobj, out TotalCount).ToList();           
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        /// <summary>
        /// Get the list of all corporate company details to show in grid
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetTaxAffectedBookings()
        {

            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            var bookingId = Convert.ToInt32(HttpContext.Current.Request.Params["bookingId"]);
            var propertyIds = Convert.ToString(HttpContext.Current.Request.Params["propertyIds"]);
            var cityIds = Convert.ToString(HttpContext.Current.Request.Params["cityIds"]);
           
            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eTaxDifferenceBookings> list = BL_Booking.GetTaxAffectedBookings(tblobj, bookingId, propertyIds, cityIds, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }

            return outputJson;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllLoyalityPoints()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eLoyalityAmenityDisp> list = BL_LoyalityAmenityMap.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllLoyalityPointsCustomerData()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eLoyalityPointsCustomerData> list = BL_LoyalityAmenityMap.getRecordForSearchCustomerData(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetPropertyCancellationPolicy()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int propid = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);

            //DateTime from, to;
            //if (HttpContext.Current.Request.Params["DateFrom"] == "")
            //{
            //    from = Convert.ToDateTime("01-01-0001 00:00:00");
            //}
            //else
            //{
            //    from = Convert.ToDateTime(HttpContext.Current.Request.Params["DateFrom"]);
            //}
            //if (HttpContext.Current.Request.Params["DateTo"] == "")
            //{
            //    to = Convert.ToDateTime("01-01-0001 00:00:00");
            //}
            //else
            //{
            //    to = Convert.ToDateTime(HttpContext.Current.Request.Params["DateTo"]);
            //}
            //DateTime datefrom = from;
            //DateTime dateTo = to;

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblPropertyCancellationPolicyMap> list = BL_tblPropertyCancellationPolicyMap.getRecordForSearch(tblobj, propid, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetChangeHistoryData()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount = 0;
            List<etblChangeHistory> list = new List<etblChangeHistory>();

            int PropId = Convert.ToInt32(Session["PropId"]);
            string chkradio = Convert.ToString(HttpContext.Current.Request.Params["chkradio"]);
            if (chkradio == "R")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtTo"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                int roomType = HttpContext.Current.Request.Params["roomType"] == "null" ? 0 : Convert.ToInt32(HttpContext.Current.Request.Params["roomType"]);
                int ratePlan = HttpContext.Current.Request.Params["ratePlan"] == "null" ? 0 : Convert.ToInt32(HttpContext.Current.Request.Params["ratePlan"]);
                DateTime dtEffectiveFrom = HttpContext.Current.Request.Params["dtEffectiveFrom"] == "" ? DateTime.Now : Convert.ToDateTime(HttpContext.Current.Request.Params["dtEffectiveFrom"]);
                DateTime dtEffectiveTo = HttpContext.Current.Request.Params["dtEffectiveTo"] == "" ? DateTime.Now : Convert.ToDateTime(HttpContext.Current.Request.Params["dtEffectiveTo"]);
                list = BL_tblChangeHistory.getRecordForSearch(tblobj, out TotalCount, PropId, dtFrom, dtTo, roomType, ratePlan, dtEffectiveFrom, dtEffectiveTo).ToList();
            }

            if (chkradio == "I" || chkradio == "B")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtTo"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string Id = Convert.ToString(HttpContext.Current.Request.Params["Id"]);
                list = BL_tblChangeHistory.getRecordForSearch(tblobj, out TotalCount, PropId, dtFrom, dtTo, Id).ToList();
            }

            if (chkradio == "Promotion")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtTo"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string Id = Convert.ToString(HttpContext.Current.Request.Params["Id"]);
                list = BL_tblChangeHistory.getRecordForSearchPromotions(tblobj, out TotalCount, dtFrom, dtTo, Id, PropId).ToList();
            }

            if (chkradio == "RatePlan")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtTo"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                list = BL_tblChangeHistory.getRecordForSearchRatePlan(tblobj, out TotalCount, dtFrom, dtTo, PropId).ToList();
            }


            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        /// <summary>
        /// Himanshu Sharma
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetChangeHistoryDataMain()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount = 0;
            List<etblChangeHistory> list = new List<etblChangeHistory>();

            int PropId = Convert.ToInt32(HttpContext.Current.Request.Params["PropId"]);
            string chkradio = Convert.ToString(HttpContext.Current.Request.Params["chkradio"]);
            if (chkradio == "R")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"],"MM/dd/yyy",CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtTo"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
                int roomType = 0;
                int ratePlan = 0;
                DateTime dtEffectiveFrom = HttpContext.Current.Request.Params["dtEffectiveFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtEffectiveFrom"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
                DateTime dtEffectiveTo = HttpContext.Current.Request.Params["dtEffectiveTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtEffectiveTo"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
                list = BL_tblChangeHistory.getRecordForSearch(tblobj, out TotalCount, PropId, dtFrom, dtTo, roomType, ratePlan, dtEffectiveFrom, dtEffectiveTo).ToList();
            }

            if (chkradio == "I" || chkradio == "B")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : Convert.ToDateTime(HttpContext.Current.Request.Params["dtTo"]);
                string Id = Convert.ToString(HttpContext.Current.Request.Params["Id"]);
                list = BL_tblChangeHistory.getRecordForSearch(tblobj, out TotalCount, PropId, dtFrom, dtTo, Id).ToList();
            }

            if (chkradio == "Promotion")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : Convert.ToDateTime(HttpContext.Current.Request.Params["dtTo"]);
                string Id = Convert.ToString(HttpContext.Current.Request.Params["Id"]);
                list = BL_tblChangeHistory.getRecordForSearchPromotions(tblobj, out TotalCount, dtFrom, dtTo, Id, PropId).ToList();
            }

            if (chkradio == "RatePlan")
            {
                DateTime dtFrom = HttpContext.Current.Request.Params["dtFrom"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtFrom"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtTo = HttpContext.Current.Request.Params["dtTo"] == "" ? DateTime.Now : DateTime.ParseExact(HttpContext.Current.Request.Params["dtTo"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                list = BL_tblChangeHistory.getRecordForSearchRatePlan(tblobj, out TotalCount, dtFrom, dtTo, PropId).ToList();
            }


            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


      



        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetUnFinishedTransactionData()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount = 0;
            List<etblUnfinishedTransactions> list = new List<etblUnfinishedTransactions>();

            string sSearchType = HttpContext.Current.Request.Params["sSearchType"] == "null" ? "" : HttpContext.Current.Request.Params["sSearchType"];
            string sName = HttpContext.Current.Request.Params["sName"] == "null" ? "" : HttpContext.Current.Request.Params["sName"];
            string sEmailID = HttpContext.Current.Request.Params["sEmailID"] == "null" ? "" : HttpContext.Current.Request.Params["sEmailID"];
            string iTelephone = HttpContext.Current.Request.Params["iTelephone"] == "null" ? "" : HttpContext.Current.Request.Params["iTelephone"];
            Int64 iBookingID = HttpContext.Current.Request.Params["iBookingID"] == "null" ? 0 : Convert.ToInt32(HttpContext.Current.Request.Params["iBookingID"]);
            string sDate = HttpContext.Current.Request.Params["sDate"] == "null" ? "" : HttpContext.Current.Request.Params["sDate"];

            list = BL_UnfinishedTransactions.getRecordForSearch(tblobj, out TotalCount, sSearchType, sName, sEmailID, iTelephone, iBookingID, sDate).ToList();

            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllPromoCodes()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<ePromoCodeDisp> list = BL_PromoCode.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllConserveCommission()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eConserveCommissionDisp> list = BL_ConserveCommission.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetPromoMappedHotels()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            string s = HttpContext.Current.Request.Params["s"];
            return BL_tblPropertyPromoMap.GenrateMenu(a, s);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetCCMappedHotels()
        {
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);
            string s = HttpContext.Current.Request.Params["s"];
            return BL_ConserveCommission.GenrateMenu(a, s);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllBanners()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<etblBannerM> list = BL_tblBannerM.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetRankManagementList()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;

            List<etblRankManagement> list = BL_tblRankManagement.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetBookingData()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount = 0;
            List<eBooking> list = new List<eBooking>();

            int PropId = Convert.ToInt32(Session["PropId"]);
            string datetype = Convert.ToString(HttpContext.Current.Request.Params["datetype"]);
            string fromdate = Convert.ToString(HttpContext.Current.Request.Params["fdate"]);
            string todate = Convert.ToString(HttpContext.Current.Request.Params["todate"]);
            string type = Convert.ToString(HttpContext.Current.Request.Params["type"]);
            string bookid = Convert.ToString(HttpContext.Current.Request.Params["bookid"]);
            string firstname = Convert.ToString(HttpContext.Current.Request.Params["fname"]);
            string lastname = Convert.ToString(HttpContext.Current.Request.Params["lname"]);
            string email = Convert.ToString(HttpContext.Current.Request.Params["email"]);
            string status = Convert.ToString(HttpContext.Current.Request.Params["status"]);
            int displayLength = Convert.ToInt32(HttpContext.Current.Request.Params["iDisplayLength"]);
            list = BL_Booking.getRecordForSearch(tblobj, out TotalCount, PropId, datetype, fromdate, todate, type, bookid, firstname, lastname, email, status, displayLength).ToList();


            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetPendingNegotiationsByPropertyID()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);


            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["PropId"]);
            List<ePendingChanges> list = BL_PendingChanges.GetPendingNegotiations(tblobj, out TotalCount, a).ToList();
            etblExchangeRatesM objExchange = new etblExchangeRatesM();
            foreach (var item in list)
            {
                Decimal? ExchangeRate = 1;
                if (item.Symbol != "INR")
                {
                    objExchange = BL_ExchangeRate.GetSingleRecordById("INR", item.Symbol);
                    if (objExchange.dRate != 0)
                    {
                        ExchangeRate = objExchange.dRate;
                    }
                }
                item.ActualAmount = item.ActualAmount == null ? item.ActualAmount : Math.Round(((decimal)item.ActualAmount * (decimal)ExchangeRate), 2);
                item.NegotiateRate = item.NegotiateRate == null ? item.NegotiateRate : Math.Round(((decimal)item.NegotiateRate * (decimal)ExchangeRate), 2);
                item.BidRate = item.BidRate == null ? item.BidRate : Math.Round(((decimal)item.BidRate * (decimal)ExchangeRate), 2);
                item.sID = OneFineRateAppUtil.clsUtils.Encode(item.ID.ToString());
            }

            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetAllRecForNegotiation()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            int a = Convert.ToInt32(HttpContext.Current.Request.Params["PropId"]);
            List<etblNegotiationNotification> list = BL_NegotiationNotification.getRecordForSearch(tblobj, out TotalCount, a).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetExchangeList()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eExchange> list = BL_ExchangeRate.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
        public string GetChannelManagerMapping()
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

            int a = Convert.ToInt32(HttpContext.Current.Request.Params["id"]);

            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();
            int TotalCount;
            List<eChannelManager> list = BL_ChannelManager.getRecordForSearch(tblobj, out TotalCount).ToList();
            if (list.Count > 0)
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": ");
                sb.Append(TotalCount.ToString());
                sb.Append(", ");
                sb.Append(@"""aaData"":  ");
                sb.Append(OneFineRateAppUtil.clsUtils.ConvertToJson(list));
                sb.Append("}");
                outputJson = sb.ToString();

            }
            else
            {
                sb.Clear();
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

            }
            return outputJson;
        }


        #region Functions
        public static int ToInt(string toParse)
        {
            int result;
            if (int.TryParse(toParse, out result)) return result;

            return result;
        }

        #endregion


    }
}
