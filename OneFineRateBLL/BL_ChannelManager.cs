using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_ChannelManager
    {
        public static List<eChannelManager> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eChannelManager> ChannelManager = new List<eChannelManager>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;


                var objTblUser = (from PCM in db.tblPropertyChannelMgrMaps
                                  join U in db.tblUserMs on PCM.iActionBy equals U.iUserId into S1
                                  from Users in S1.DefaultIfEmpty()
                                  join P in db.tblPropertyMs on PCM.iPropId equals P.iPropId into Prop
                                  from Property in Prop.DefaultIfEmpty()
                                  join CM in db.tblChannelMgrMs on PCM.iChannelMgr equals CM.iChannelMgr into ChMgr
                                  from channelmanager in ChMgr.DefaultIfEmpty()
                                  join C in db.tblCityMs on Property.iCityId equals C.iCityId into Cit
                                  from City in Cit.DefaultIfEmpty()
                                  select new
                                  {
                                      PCM.iPropId,
                                      sHotelName = Property.sHotelName + " , " + City.sCity,
                                      channelmanager.sChannelMgrName,
                                      PCM.dtActionDate,
                                      sActionBy = Users.sFirstName + " " + Users.sLastName
                                  }).Distinct().Where(u => u.sHotelName.Contains(param.sSearch) || u.sChannelMgrName.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sHotelName) : objTblUser.OrderByDescending(u => u.sHotelName);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sChannelMgrName) : objTblUser.OrderByDescending(u => u.sChannelMgrName);
                        break;
                }

                ////implemented paging
                //List<tblUserM> lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                ChannelManager.AddRange(lstUser.Select(usr => (eChannelManager)OneFineRateAppUtil.clsUtils.ConvertToObject(usr, new eChannelManager())));

                return ChannelManager;
            }
        }

        //Delete a record
        public static int DeleteRecord(int id)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblPropertyChannelMgrMaps.SingleOrDefault(u => u.iPropId == id);
                    db.tblPropertyChannelMgrMaps.Attach(obj);
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
        public static int UpdateRecord(int PropId, int Channelmgr, int user)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblPropertyChannelMgrMaps.SingleOrDefault(u => u.iPropId == PropId);
                    if (obj != null)
                    {
                        db.tblPropertyChannelMgrMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }

                    tblPropertyChannelMgrMap CM = new tblPropertyChannelMgrMap();
                    CM.dtActionDate = DateTime.Now;
                    CM.iActionBy = user;
                    CM.iChannelMgr = Channelmgr;
                    CM.iPropId = PropId;

                    db.tblPropertyChannelMgrMaps.Attach(CM);
                    db.Entry(CM).State = System.Data.Entity.EntityState.Added;
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

        //Check a record
        public static int CheckRecord(int PropId, int Channelmgr)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblPropertyChannelMgrMaps.SingleOrDefault(u => u.iPropId == PropId && u.iChannelMgr == Channelmgr);
                    if (obj != null)
                        return 1;

                    obj = db.tblPropertyChannelMgrMaps.SingleOrDefault(u => u.iPropId == PropId);
                    if (obj != null)
                        return 2;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        public static List<PNames> GetAllPropertyName()
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyMs.Where(a => (a.bIsTG == null || a.bIsTG == false) && a.cStatus == "A")
                              join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                              select new PNames()
                              {
                                  Id = t1.iPropId.ToString(),
                                  Name = t1.sHotelName.Trim() + " , " + t4.sCity.Trim() + "(" + t1.iPropId + ")"
                              }).AsQueryable().ToList();
                return objMapping;
            }
        }

        public static List<PNames> GetAllChannelManagers()
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblChannelMgrMs
                              select new PNames()
                              {
                                  Id = t1.iChannelMgr.ToString(),
                                  Name = t1.sChannelMgrName.Trim()
                              }).AsQueryable().ToList();
                return objMapping;
            }
        }
    }
}