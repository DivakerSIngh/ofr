using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data.SqlClient;
using System.Data;

namespace OneFineRateBLL
{
    public class eLocationDetails
    {
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string TimeZone { get; set; }

    }
    public class eSearchData
    {
        public string category { get; set; }
        public string label { get; set; }
        public string Id { get; set; }
    }
    public class eCurrencyFlags
    {
        public string sCurrencyCode { get; set; }
        public string sImg { get; set; }
        public string sSymbol { get; set; }
        public string sCountryCode { get; set; }
        public bool iActive { get; set; }
    }
    public class eBidRangeValues
    {
        public int? MinRange { get; set; }
        public int? MaxRange { get; set; }
        public int? MinAllowed { get; set; }
        public string Msg { get; set; }
        public string sSymbol { get; set; }

        public List<LatLngBounds> LatLngBoundList { get; set; }
    }

    public class LatLngBounds
    {
        public decimal dLatitude { get; set; }
        public decimal dLongitude { get; set; }
    }
    public class BL_WebsiteHomePage
    {

        public static eLocationDetails GetUserCountryDetails()
        {
            eLocationDetails obj = new eLocationDetails();
                string ipAddress = OneFineRateAppUtil.clsUtils.getIpAddress();
                if (ipAddress == "::1" || ipAddress == "127.0.0.1")
                { ipAddress = "119.82.65.67"; }
                else if (ipAddress == null || ipAddress == "") { ipAddress = "127.0.0.1"; }
                   

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    obj = db.Database.SqlQuery<eLocationDetails>("uspGetCountryCode @IP", new SqlParameter("@IP", ipAddress)).FirstOrDefault();
                }
                return obj;
        }
        public static int GetParaxisStateId()
        {
            using (var db=new OneFineRateEntities())
            {
                return db.tblStateMs.Where(x => x.sState == "Haryana").SingleOrDefault().iStateId;
            }
        }
        public static List<eRecentSavings> GetAllRecentSavvings(int id, string Type, int Index, string CurrencyCode)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eRecentSavings> eobj = new List<eRecentSavings>();
                SqlParameter[] MyParam = new SqlParameter[4];
                MyParam[0] = new SqlParameter("@Id", id);
                MyParam[1] = new SqlParameter("@Type", Type);
                MyParam[2] = new SqlParameter("@startRowIndex", Index);
                MyParam[3] = new SqlParameter("@sCurrencyCode", CurrencyCode);
                var result = db.Database.SqlQuery<eRecentSavings>("uspGetRecentSavings @Id,@Type,@startRowIndex,@sCurrencyCode", MyParam).ToList();
                foreach (var item in result)
                {
                    eobj.Add((eRecentSavings)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eRecentSavings()));
                }
                return eobj;
            }
        }
       
        public static List<eSearchData> GetSearchData(string msg)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@SearchText", msg + "%");
                MyParam[1] = new SqlParameter("@SearchText2", "% " + msg + "%");
                var result = db.Database.SqlQuery<eSearchData>("uspGetSearchText @SearchText, @SearchText2", MyParam).ToList();               
                return result;
            }
        }
        public static List<ePromotionalHotels> GetAllPromotionalHotels(string CurrencyCode)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<ePromotionalHotels> eobj = new List<ePromotionalHotels>();
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@sCurrencyCode", CurrencyCode);
                var result = db.Database.SqlQuery<ePromotionalHotels>("uspGetPromotionalHotels @sCurrencyCode", MyParam).ToList();
                foreach (var item in result)
                {
                    eobj.Add((ePromotionalHotels)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new ePromotionalHotels()));
                }
                return eobj;
            }
        }
        public static List<eCurrencyFlags> GetAllCurrenyFlags(string CountryCode)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eCurrencyFlags> eobj = new List<eCurrencyFlags>();
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@CountryCode", CountryCode);
                var result = db.Database.SqlQuery<eCurrencyFlags>("uspGetAllCurrency @CountryCode", MyParam).ToList();
                foreach (var item in result)
                {
                    eobj.Add((eCurrencyFlags)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eCurrencyFlags()));
                }
                return eobj;
            }
        }
        
        //public static List<CheckBoxListBidding> GetAreaLocalityForBid(string txt, int Type)
        //{
        //    List<CheckBoxListBidding> selectItems = null;
        //    using (OneFineRateEntities db = new OneFineRateEntities())
        //    {

        //        SqlParameter[] MyParam = new SqlParameter[2];
        //        MyParam[0] = new SqlParameter("@Type", txt);
        //        MyParam[1] = new SqlParameter("@Value", Type);
        //        var result = db.Database.SqlQuery<CheckBoxListBidding>("uspGetAeaLocalityForBid @Type,@Value ", MyParam).ToList();
        //        //foreach (var item in result)
        //        //{
        //        //    selectItems.Add((CheckBoxListBidding)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new CheckBoxListBidding()));
        //        //}
        //        return result;
        //    }
        //}
      
    }
}