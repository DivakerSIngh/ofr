using OneFineRate;
using OneFineRateWebBLL.Entities;
using OneFineRateWebBLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateWebBLL
{
    public class BL_WebsiteUser
    {
        #region functions

        //Add new record
        public static int AddRecord(WebsiteUserMaster user)
        {
            int operationStatusValue = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblWebsiteUserMater dbuser = (OneFineRate.tblWebsiteUserMater)OneFineRateAppUtil.clsUtils.ConvertToObject(user, new OneFineRate.tblWebsiteUserMater());
                    db.tblWebsiteUserMaters.Add(dbuser);
                    db.SaveChanges();
                    operationStatusValue = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatusValue;
        }

        //Delete a record
        public static int DeleteRecord(int id)
        {
            int operationStatus = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblWebsiteUserMaters.SingleOrDefault(u => u.Id == id);
                    db.tblWebsiteUserMaters.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    operationStatus = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatus;
        }

        //Update a record
        public static int UpdateRecord(WebsiteUserMaster eobj)
        {
            int operationStatus = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblWebsiteUserMater obj = (OneFineRate.tblWebsiteUserMater)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblWebsiteUserMater());
                    db.tblWebsiteUserMaters.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    operationStatus = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatus;
        }

        //Get Single Record
        public static WebsiteUserMaster GetSingleRecordById(int id)
        {
            WebsiteUserMaster eobj = new WebsiteUserMaster();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteUserMaters.SingleOrDefault(u => u.Id == id);
                if (dbobj != null)
                    eobj = (WebsiteUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get all records
        public static List<WebsiteUserMaster> GetAllRecord()
        {
            List<WebsiteUserMaster> eobj = new List<WebsiteUserMaster>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                //TO DO
                foreach (OneFineRate.tblWebsiteUserMater item in db.tblWebsiteUserMaters.Where(u => u.AccessFailedCount == 0).ToList())
                {
                    eobj.Add((WebsiteUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new WebsiteUserMaster()));
                }
            }
            return eobj;
        }

        #endregion
    }
}
