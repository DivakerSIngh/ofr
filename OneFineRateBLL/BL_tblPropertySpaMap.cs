using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblPropertySpaMap
    {
        //Check record exist or not
        public static bool CheckDataExist(string Name , int propId)
        {
            bool ifNotExist = true;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertySpaMaps.Select(u => new { u.sSpaName, u.iPropId }).SingleOrDefault(u => u.sSpaName == Name && u.iPropId == propId);
                if (dbobj != null)
                    ifNotExist = false;
            }
            return ifNotExist;

        }
        //Get Single Record
        public static etblPropertySpaMap GetSingleRecordById(int id)
        {
            etblPropertySpaMap eobj = new etblPropertySpaMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertySpaMaps.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertySpaMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;
        }
        public static string CheckDataExist(int id)
        {
            string eobj = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertySpaMaps.Select(u => new { u.sSpaName, u.iPropId }).SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = ((etblPropertySpaMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new etblPropertySpaMap())).sSpaName;
            }
            return eobj;

        }
        //Add new record
        public static int AddRecord(etblPropertySpaMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertySpaMap dbuser = (OneFineRate.tblPropertySpaMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertySpaMap());
                    db.tblPropertySpaMaps.Add(dbuser);
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
        public static int UpdateRecord(etblPropertySpaMap eobj,bool Type)
        {
          
            int retval = 0;
            using (OneFineRateEntities dbnew = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertySpaMap obj = (OneFineRate.tblPropertySpaMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertySpaMap());
                    if (Type)
                    {

                        dbnew.tblPropertySpaMaps.Attach(obj);
                        dbnew.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        dbnew.SaveChanges();
                        retval = 1;
                    }
                    else
                    {
                        dbnew.tblPropertySpaMaps.Add(obj);
                        dbnew.SaveChanges();
                        retval = 1;
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
    }
}