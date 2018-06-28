using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Currency
    {
        //Get all records
        public static List<eCurrency> GetAllRecords()
        {

            List<eCurrency> eobj = new List<eCurrency>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblCurrencyM item in db.tblCurrencyMs.ToList())
                {
                    eobj.Add((eCurrency)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eCurrency()));
                }
            }
            return eobj;
        }
        public static string GetCurrencyByPropId(int PropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from s in db.tblPropertyMs
                             join c in db.tblCurrencyMs on s.iCurrencyId equals c.iCurrencyId
                             select new
                             {
                                 s.iPropId,
                                 c.sCurrencyCode,
                             }).Where(u => u.iPropId == PropId).AsQueryable();
                var list = dbobj.ToList();
                if (list.Count != 0)
                {
                    return list[0].sCurrencyCode;
                }
            }
            return "";
        }
        public static string GetSymbolByPropId(int PropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from s in db.tblPropertyMs
                             join c in db.tblCurrencyMs on s.iCurrencyId equals c.iCurrencyId
                             select new
                             {
                                 s.iPropId,
                                 c.sSymbol,
                             }).Where(u => u.iPropId == PropId).AsQueryable();
                var list = dbobj.ToList();
                if (list.Count != 0)
                {
                    return list[0].sSymbol;
                }
            }
            return "";
        }
        public static string GetFlagByPropId(int PropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from s in db.tblPropertyMs
                             join c in db.tblCurrencyMs on s.iCurrencyId equals c.iCurrencyId
                             select new
                             {
                                 s.iPropId,
                                 c.sImg,
                             }).Where(u => u.iPropId == PropId).AsQueryable();
                var list = dbobj.ToList();
                if (list.Count != 0)
                {
                    return ConfigurationManager.AppSettings["BlobUrl"].ToString() + list[0].sImg.Substring(1);
                }
            }
            return "";
        }
    }
}