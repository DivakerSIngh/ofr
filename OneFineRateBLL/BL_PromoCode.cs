using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace OneFineRateBLL
{
    public class BL_PromoCode
    {
        //Add new record
        public static int AddRecord(ePromoCode eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblPromoCodeM dbuser = (OneFineRate.tblPromoCodeM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPromoCodeM());
                    db.tblPromoCodeMs.Add(dbuser);
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
        public static int UpdateRecord(ePromoCode eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPromoCodeM obj = (OneFineRate.tblPromoCodeM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPromoCodeM());
                    db.tblPromoCodeMs.Attach(obj);
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
        public static ePromoCode GetSingleRecordById(int id)
        {
            ePromoCode eobj = new ePromoCode();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPromoCodeMs.SingleOrDefault(u => u.iPromoCodeId == id);
                if (dbobj != null)
                    eobj = (ePromoCode)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Check record exist or not
        public static bool CheckDataExist(string Name)
        {
            bool ifNotExist = true;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPromoCodeMs.Select(u => new { u.sPromoCode }).SingleOrDefault(u => u.sPromoCode == Name);
                if (dbobj != null)
                    ifNotExist = false;
            }
            return ifNotExist;

        }
        public static ePromoCode GetSingleRecordById()
        {
            ePromoCode eobj = new ePromoCode();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPromoCodeMs.SingleOrDefault();
                if (dbobj != null)
                    eobj = (ePromoCode)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get list of records
        public static List<ePromoCodeDisp> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<ePromoCodeDisp> Promolst = new List<ePromoCodeDisp>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                var objTblPromo = (from a in db.tblPromoCodeMs
                                   join c in db.tblUserMs on a.iActionBy equals c.iUserId into PC
                                   from T in PC.DefaultIfEmpty()
                                   join am in db.tblAmenityMs on a.iAmenityId equals am.iAmenityId into S2
                                   from am1 in S2.DefaultIfEmpty()
                                   join PPM in db.tblPropertyPromoMaps on a.iPromoCodeId equals PPM.iPromoCodeId into PPM1
                                   from PPM2 in PPM1.DefaultIfEmpty()
                                   select new
                                   {
                                       a.iPromoCodeId,
                                       a.sPromoCode,
                                       a.sPromoTitle,
                                       a.sPromoDescription,
                                       sAmenityName = am1.sAmenityName == null || am1.sAmenityName == "" ? (a.cPercentageValue == "1" ? a.dValue.ToString() + " %" : "Value: " + a.dValue.ToString()) : am1.sAmenityName,
                                       a.dtBookingFrom,
                                       a.dtBookingTo,
                                       a.dtStayFrom,
                                       a.dtStayTo,
                                       a.dtActionDate,
                                       cStatus = a.cStatus == "A" ? "Live" : "Disable",
                                       ActionBy = T.sFirstName + " " + T.sLastName,
                                       HotelCount = PPM1.Count()
                                   }).Distinct().Where(u => u.sPromoCode.Contains(param.sSearch) || u.sPromoTitle.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblPromo.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.sPromoCode) : objTblPromo.OrderByDescending(u => u.sPromoCode);
                        break;
                    case 3:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.dtBookingFrom) : objTblPromo.OrderByDescending(u => u.dtBookingFrom);
                        break;
                    case 4:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.dtBookingTo) : objTblPromo.OrderByDescending(u => u.dtBookingTo);
                        break;
                    case 5:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.dtStayFrom) : objTblPromo.OrderByDescending(u => u.dtStayFrom);
                        break;
                    case 6:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.dtStayTo) : objTblPromo.OrderByDescending(u => u.dtStayTo);
                        break;
                    case 7:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.cStatus) : objTblPromo.OrderByDescending(u => u.cStatus);
                        break;
                    case 8:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.ActionBy) : objTblPromo.OrderByDescending(u => u.ActionBy);
                        break;
                    case 9:
                        objTblPromo = param.sortDirection == "asc" ? objTblPromo.OrderBy(u => u.dtActionDate) : objTblPromo.OrderByDescending(u => u.dtActionDate);
                        break;
                }

                ////implemented paging
                var lstUser = objTblPromo.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    Promolst.Add((ePromoCodeDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new ePromoCodeDisp()));
                }
                return Promolst;
            }
        }
    }
}