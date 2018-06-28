using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_ExchangeRate
    {
        //Get Single Record
        public static etblExchangeRatesM GetSingleRecordById(string CurrencyCodefrom, string CurrencyCodeTo)
        {
            etblExchangeRatesM eobj = new etblExchangeRatesM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblExchangeRatesMs.SingleOrDefault(u => u.sCurrencyCodeFrom == CurrencyCodefrom && u.sCurrencyCodeTo == CurrencyCodeTo);
                if (dbobj != null)
                    eobj = (etblExchangeRatesM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        public static List<eExchange> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eExchange> Exchange = new List<eExchange>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;


                var objTblUser = (from ER in db.tblExchangeRatesMs
                                  join U in db.tblUserMs on ER.iActionBy equals U.iUserId into S1
                                  from Users in S1.DefaultIfEmpty()
                                  join C1 in db.tblCurrencyMs on ER.sCurrencyCodeFrom equals C1.sCurrencyCode into Curr1
                                  from Currency1 in Curr1.DefaultIfEmpty()
                                  join C2 in db.tblCurrencyMs on ER.sCurrencyCodeTo equals C2.sCurrencyCode into Curr2
                                  from Currency2 in Curr2.DefaultIfEmpty()
                                  select new
                                  {
                                      sCurrencyCodeFrom = ER.sCurrencyCodeFrom + " - " + Currency1.sCurrency,
                                      sCurrencyCodeTo = ER.sCurrencyCodeTo + " - " + Currency2.sCurrency,
                                      ER.dRate,
                                      ER.dtActionDate,
                                      sActionBy = Users.sFirstName + " " + Users.sLastName
                                  }).Distinct().Where(u => u.sCurrencyCodeFrom.Contains(param.sSearch) || u.sCurrencyCodeTo.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sCurrencyCodeFrom) : objTblUser.OrderByDescending(u => u.sCurrencyCodeFrom);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sCurrencyCodeTo) : objTblUser.OrderByDescending(u => u.sCurrencyCodeTo);
                        break;
                }

                ////implemented paging
                //List<tblUserM> lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                Exchange.AddRange(lstUser.Select(usr => (eExchange)OneFineRateAppUtil.clsUtils.ConvertToObject(usr, new eExchange())));

                return Exchange;
            }
        }

        //Delete a record
        public static int DeleteRecord(string from, string to)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblExchangeRatesMs.SingleOrDefault(u => u.sCurrencyCodeFrom == from && u.sCurrencyCodeTo == to);
                    db.tblExchangeRatesMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        //Update records
        public static int UpdateRecord(List<ExchangeRate> ER, int User)
        {
            int retval = 0;

            DataTable dtExchange = new DataTable();
            DataColumn col = null;
            col = new DataColumn("sCurrencyCodeFrom", typeof(String));
            dtExchange.Columns.Add(col);
            col = new DataColumn("sCurrencyCodeTo", typeof(String));
            dtExchange.Columns.Add(col);
            col = new DataColumn("dRate", typeof(Double));
            dtExchange.Columns.Add(col);
            foreach (ExchangeRate E in ER)
            {
                try
                {
                    DataRow drExchange = dtExchange.NewRow();
                    drExchange["sCurrencyCodeFrom"] = E.sCurrencyCodeFrom;
                    drExchange["sCurrencyCodeTo"] = E.sCurrencyCodeTo;
                    drExchange["dRate"] = E.dRate;
                    dtExchange.Rows.Add(drExchange);

                }
                catch (Exception) { }
            }

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[2];
                    MyParam[0] = new SqlParameter("@ExchangeRate", dtExchange);
                    MyParam[0].TypeName = "[dbo].[ExchangeRate]";
                    MyParam[1] = new SqlParameter("@User", User);
                    db.Database.SqlQuery<InvalidRatePlans>("uspSetExchangeRate @ExchangeRate, @User", MyParam).ToList();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }


        public static string GetSymbolByCurrencyCode(string currencyCode = "INR")
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var symbol = db.tblCurrencyMs.Where(x => x.sCurrencyCode == currencyCode).Select(x => x.sSymbol).FirstOrDefault();
                    return symbol;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}