using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Tax
    {
        #region functions
        //Add new record
        public static int AddRecord(eTax eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblTaxMs.Where(u => u.sTaxName == eobj.sTaxName.Trim())
                                 select new
                                 {
                                     s.iTaxId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblTaxM dbuser = (OneFineRate.tblTaxM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblTaxM());
                    db.tblTaxMs.Add(dbuser);
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
        //Delete a record
        public static int DeleteRecord(int id)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblTaxMs.SingleOrDefault(u => u.iTaxId == id);
                    db.tblTaxMs.Attach(obj);
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
        //Update a record
        public static int UpdateRecord(eTax eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    int dbobj = (from s in db.tblTaxMs.Where(u => u.sTaxName == eobj.sTaxName.Trim() && u.iTaxId != eobj.iTaxId)
                                 select new
                                 {
                                     s.iTaxId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblTaxM obj = (OneFineRate.tblTaxM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblTaxM());
                    db.tblTaxMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        //Get Single Record
        public static eTax GetSingleRecordById(int id)
        {
            eTax eobj = new eTax();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblTaxMs.SingleOrDefault(u => u.iTaxId == id);
                if (dbobj != null)
                    eobj = (eTax)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get all records
        public static List<eTax> GetAllTax()
        {

            List<eTax> eobj = new List<eTax>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblTaxM item in db.tblTaxMs.Where(u => u.cStatus == "A").AsQueryable().ToList())
                {
                    eobj.Add((eTax)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eTax()));
                }
            }
            return eobj;
        }
        //Get all records
        public static List<eTax> GetAllTaxOfProperty(int id)
        {

            List<eTax> eobj = new List<eTax>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                eobj = (from s in db.tblTaxMs
                        join c in db.tblPropertyTaxesMaps on new { p1 = s.iTaxId, p2 = id } equals new { p1 = c.iTaxId, p2 = c.iPropTaxId }
                        into Tax
                        from T in Tax.DefaultIfEmpty()
                        select new eTax
                        {
                            iTaxId = s.iTaxId,
                            sTaxName = s.sTaxName,
                            value = T.dValue.ToString(),
                            Type = T.bIsPercent.ToString() == "True" ? "1" : "0",
                            iActionBy = T.iPropTaxId,
                            cStatus = s.cStatus
                        }).Where(u => u.cStatus == "A").Distinct().ToList();
                //.Where(u => u.iActionBy == id)
            }
            return eobj;
        }

        //Get list of records
        public static List<eTaxDisp> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eTaxDisp> taxlst = new List<eTaxDisp>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //SqlParameter[] MyParam = new SqlParameter[5];
                //MyParam[0] = new SqlParameter("@Tax", param.sSearch + "%");
                //MyParam[1] = new SqlParameter("@DisplayLength", param.iDisplayLength);
                //MyParam[2] = new SqlParameter("@DisplayStart", param.iDisplayStart);
                //MyParam[3] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
                //MyParam[4] = new SqlParameter("@TotalCount", 0);
                //MyParam[4].Direction = System.Data.ParameterDirection.Output;
                ////var objTblUser = db.Database.SqlQuery<eTaxDisp>("uspGetAmenitiesByName @Tax, @DisplayLength, @DisplayStart, @SortDirection", new SqlParameter("@Tax", param.sSearch + "%"),new SqlParameter("@DisplayLength", param.iDisplayLength),new SqlParameter("@DisplayStart", param.iDisplayStart),new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D"));
                //user = db.Database.SqlQuery<eTaxDisp>("uspGetTaxByName @Tax, @DisplayLength, @DisplayStart, @SortDirection, @TotalCount out", MyParam).ToList();

                ////get Total Count for show total records
                //TotalCount = Convert.ToInt16(MyParam[4].Value);//user.Count();
                var objTblTax = (from s in db.tblTaxMs
                                 join c in db.tblUserMs on s.iActionBy equals c.iUserId into Tax
                                 from T in Tax.DefaultIfEmpty()

                                 select new
                                 {
                                     s.iTaxId,
                                     s.sTaxName,
                                     s.dtActionDate,
                                     cStatus = s.cStatus == "A" ? "Live" : "Disable",
                                     ActionBy = T.sFirstName + " " + T.sLastName
                                 }).Where(u => u.sTaxName.Contains(param.sSearch)).AsQueryable();


                //get Total Count for show total records
                TotalCount = objTblTax.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblTax = param.sortDirection == "asc" ? objTblTax.OrderBy(u => u.sTaxName) : objTblTax.OrderByDescending(u => u.sTaxName);
                        break;
                    case 1:
                        objTblTax = param.sortDirection == "asc" ? objTblTax.OrderBy(u => u.cStatus) : objTblTax.OrderByDescending(u => u.cStatus);
                        break;

                }

                ////implemented paging
                var lstUser = objTblTax.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    taxlst.Add((eTaxDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eTaxDisp()));
                }
                return taxlst;

            }
        }


        #endregion
    }
}