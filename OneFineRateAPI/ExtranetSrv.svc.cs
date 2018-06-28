using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using OneFineRateBLL.Entities;


namespace OneFineRateAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtranetSrv" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ExtranetSrv.svc or ExtranetSrv.svc.cs at the Solution Explorer and start debugging.
    public class ExtranetSrv : IExtranetSrv
    {
        public string GetAllRecords1(int sEcho, int iDisplayLength, int iDisplayStart, string sSortDir_0, string sSearch, int iSortCol_0)
        {
            OneFineRateAppUtil.jQueryDataTableParamModel tblobj = new OneFineRateAppUtil.jQueryDataTableParamModel();
           // int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            tblobj.iDisplayLength = iDisplayLength;// ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            tblobj.iDisplayStart = iDisplayStart;// ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            tblobj.sortDirection = sSortDir_0;//HttpContext.Current.Request.Params["sSortDir_0"];
            tblobj.sSearch = sSearch;// HttpContext.Current.Request.Params["sSearch"];
            tblobj.iSortingCols = iSortCol_0;// Convert.ToInt32(HttpContext.Current.Request.Params["iSortCol_0"]);

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
        public static int ToInt(string toParse)
        {
            int result;
            if (int.TryParse(toParse, out result)) return result;

            return result;
        }
    }
}
