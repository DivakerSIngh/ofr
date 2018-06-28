using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace OneFineRateBLL
{
    public class BL_CustomerPointsMap
    {
        #region functions


        //Add new record
        public static int AddRecord(eCustomerPointsMap customerPointsMap)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblCustomerPointsMap dbCustomerPoint = (OneFineRate.tblCustomerPointsMap)OneFineRateAppUtil.clsUtils.ConvertToObject(customerPointsMap, new OneFineRate.tblCustomerPointsMap());
                    db.tblCustomerPointsMaps.Add(dbCustomerPoint);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Delete a record
        public static int DeleteRecord(int id)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblCustomerPointsMaps.SingleOrDefault(u => u.iId == id);
                    db.tblCustomerPointsMaps.Attach(obj);
                    db.tblCustomerPointsMaps.Remove(obj);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Update a record
        public static int UpdateRecord(eCustomerPointsMap eobj)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblCustomerPointsMap obj = (OneFineRate.tblCustomerPointsMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblCustomerPointsMap());
                    db.tblCustomerPointsMaps.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    return db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                    throw e;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Get Single Record
        public static eCustomerPointsMap GetRecordById(int id)
        {
            eCustomerPointsMap eobj = new eCustomerPointsMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblCustomerPointsMaps.SingleOrDefault(u => u.iId == id);
                if (dbobj != null)
                    eobj = (eCustomerPointsMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;
        }

        public static async Task<etblLoyalityPoints> GetLoyalityPointDetails()
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                etblLoyalityPoints loyalityPoint = null;

                var dbobj = await db.tblLoyalityPointsMs.FirstOrDefaultAsync();

                if (dbobj != null)
                {
                    loyalityPoint = new etblLoyalityPoints();

                    loyalityPoint = (etblLoyalityPoints)clsUtils.ConvertToObject(dbobj, new etblLoyalityPoints());
                }

                return loyalityPoint;
            }
        }

        public static etblCustomerPointsHistoryTx GetCustomerPointData(long userId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var idParam = new SqlParameter[1];
                idParam[0] = new SqlParameter("@iCustomerId", userId);
                return db.Database.SqlQuery<etblCustomerPointsHistoryTx>("uspGetCustomerRewardPointData @iCustomerId", idParam).FirstOrDefault();
            }
        }

        #endregion
    }
}