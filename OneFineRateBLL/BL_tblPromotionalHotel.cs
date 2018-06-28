using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblPromotionalHotel
    {
        #region Functions

        //Add new record
        public static int AddUpdateRecord(eTblPromotionalHotel eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPromotionalHotelsM dbstate = (OneFineRate.tblPromotionalHotelsM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPromotionalHotelsM());
                    db.tblPromotionalHotelsMs.Add(dbstate);
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

        public static Dictionary<int, string> GetPropertyListForDropdown()
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    Dictionary<int, string> keyValueList = new Dictionary<int, string>();

                    var list = db.tblPropertyMs.Where(x => x.bIsTG == null || x.bIsTG == false).Select(x => new { x.iPropId, sHotelName = x.sHotelName + " (" + x.iPropId + ")" }).ToList();

                    foreach (var item in list)
                    {
                        keyValueList.Add(item.iPropId, item.sHotelName);
                    }

                    return keyValueList;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        //Delete a record
        public static int DeleteRecord(string sPosition)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblPromotionalHotelsMs.SingleOrDefault(u => u.sPosition == sPosition);
                    db.tblPromotionalHotelsMs.Attach(obj);
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
        public static int UpdateRecord(eTblPromotionalHotel eobj)
        {
            int retval = -1;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var existingModel = db.tblPromotionalHotelsMs.FirstOrDefault(x => x.sPosition == eobj.sPosition);
                    if(existingModel!=null)
                    {
                        existingModel.iPropId = eobj.iPropId;
                        existingModel.sDescription = eobj.sDescription;
                        existingModel.sImageUrl = eobj.sImageUrl;
                        existingModel.iActionBy = eobj.iActionBy;
                        existingModel.dtActionDate = eobj.dtActionDate;
                        existingModel.sPosition = eobj.sPosition;
                    }
                    //db.Entry(existingModel).State = System.Data.Entity.EntityState.Modified;
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
        public static eTblPromotionalHotel GetSingleRecordById(string sPosition)
        {
            eTblPromotionalHotel eobj = new eTblPromotionalHotel();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPromotionalHotelsMs.SingleOrDefault(u => u.sPosition == sPosition);
                if (dbobj != null)
                {
                    eobj = (eTblPromotionalHotel)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new eTblPromotionalHotel());
                }
            }
            return eobj;

        }

        //Get all records
        public static List<eTblPromotionalHotel> GetAllRecords()
        {
            List<eTblPromotionalHotel> eobj = new List<eTblPromotionalHotel>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblPromotionalHotelsM item in db.tblPromotionalHotelsMs.ToList())
                {
                    eobj.Add((eTblPromotionalHotel)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eTblPromotionalHotel()));
                }
            }
            return eobj;
        }

        #endregion
    }
}