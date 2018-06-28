using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblEmailSettingsM
    {
        public static int AddRecord(etblEmailSettingsM emailSetting)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblEmailSettingsM emailSettingForDb = (OneFineRate.tblEmailSettingsM)OneFineRateAppUtil.clsUtils.ConvertToObject(emailSetting, new OneFineRate.tblEmailSettingsM());
                    db.tblEmailSettingsMs.Add(emailSettingForDb);
                    return db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static int DeleteRecord(string sModule)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblEmailSettingsMs.SingleOrDefault(u => u.sModule == sModule);
                    db.tblEmailSettingsMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    return db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static int UpdateRecord(etblEmailSettingsM eobj)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblEmailSettingsM obj = (OneFineRate.tblEmailSettingsM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblEmailSettingsM());
                    db.tblEmailSettingsMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    return db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static etblEmailSettingsM GetRecord(string sModule)
        {
            etblEmailSettingsM eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblEmailSettingsMs.SingleOrDefault(u => u.sModule == sModule);
                if (dbobj != null)
                {
                    eobj = (etblEmailSettingsM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new etblEmailSettingsM());
                }
            }
            return eobj;
        }
    }
}