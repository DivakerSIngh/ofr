using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblPurchaseHistory
    {
        #region functions

        public static long AddRecordAndGetId(eTblPurchaseHistory eobj)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPurchaseHistory purchangeHistory = (OneFineRate.tblPurchaseHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPurchaseHistory());
                    db.tblPurchaseHistories.Add(purchangeHistory);
                    var status = db.SaveChanges();
                    return purchangeHistory.iPurchaseId;
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static int DeleteRecord(int purchaseId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblPurchaseHistories.Find(purchaseId);
                    db.tblPurchaseHistories.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    return db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static int UpdateRecord(eTblPurchaseHistory eobj)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPurchaseHistory obj = (OneFineRate.tblPurchaseHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPurchaseHistory());
                    db.tblPurchaseHistories.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    return db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static eTblPurchaseHistory GetRecord(int purchaseId)
        {
            eTblPurchaseHistory eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPurchaseHistories.Find(purchaseId);
                if (dbobj != null)
                {
                    eobj = (eTblPurchaseHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new eTblPurchaseHistory());
                }
            }
            return eobj;
        }

        public static List<eTblPurchaseHistory> GetAllRecords()
        {
            List<eTblPurchaseHistory> eobj = new List<eTblPurchaseHistory>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblPurchaseHistory item in db.tblPurchaseHistories.ToList())
                {
                    eobj.Add((eTblPurchaseHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eTblPurchaseHistory()));
                }
            }
            return eobj;
        }

        public static List<eTblPurchaseHistory> GetRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eTblPurchaseHistory> purchaseHistoryList = new List<eTblPurchaseHistory>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var purchaseHistoryList_AsQueryable = db.tblPurchaseHistories.Where(u => u.FIRSTNAME.Contains(param.sSearch)).AsQueryable();


                TotalCount = purchaseHistoryList_AsQueryable.Count();

                switch (param.iSortingCols)
                {
                    case 0:
                        purchaseHistoryList_AsQueryable = param.sortDirection == "asc" ? purchaseHistoryList_AsQueryable.OrderBy(u => u.FIRSTNAME) : purchaseHistoryList_AsQueryable.OrderByDescending(u => u.FIRSTNAME);
                        break;

                }

                List<tblPurchaseHistory> lstUser = purchaseHistoryList_AsQueryable.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    purchaseHistoryList.Add((eTblPurchaseHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eTblPurchaseHistory()));
                }
                return purchaseHistoryList;
            }
        }

        #endregion
    }
}