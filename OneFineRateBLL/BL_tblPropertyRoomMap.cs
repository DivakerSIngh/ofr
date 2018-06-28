using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRoomMap
    {
        //get all room names property wise with currency
        public static List<PropertyRooms> GetAllPropertyRoomNamesCurrency(int propid)
        {
            List<PropertyRooms> objMapping = new List<PropertyRooms>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var p = (from t1 in db.tblPropertyRoomMaps
                         join t2 in db.tblPropertyMs on t1.iPropId equals t2.iPropId
                         join t3 in db.tblCurrencyMs on t2.iCurrencyId equals t3.iCurrencyId
                         where t1.iPropId == propid && t2.cStatus == "A" && t1.bActive == true
                         select new PropertyRooms()
                         {
                             Roomid = t1.iRoomId,
                             Name = t1.sRoomName,
                             Currency = t3.sCurrencyCode
                         }).OrderBy(u => u.Name).AsQueryable();
                objMapping = p.ToList();
                return objMapping;
            }
        }
        //get all room names property wise
        public static List<PNames> GetAllPropertyRoomNames(int propId)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyRoomMaps
                              where t1.iPropId == propId && t1.bActive == true
                              select new PNames()
                              {
                                  Id = t1.iRoomId.ToString(),
                                  Name = t1.sRoomName.Trim()
                              }).OrderBy(u => u.Name).AsQueryable().ToList();
                return objMapping;
            }
        }
        //get all room names property wise
        public static List<PNames> GetAllPropertyTypes(int propId)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyRoomMaps
                              where t1.iPropId == propId
                              select new PNames()
                              {
                                  Id = t1.iRoomId.ToString(),
                                  Name = t1.bActive == true ? t1.sRoomName : t1.sRoomName + " (Disabled)"
                              }).OrderBy(u => u.Name).AsQueryable().ToList();
                return objMapping;
            }
        }

        //Get Single Record
        public static etblPropertyRoomMap GetSingleRecordById(int id)
        {
            etblPropertyRoomMap eobj = new etblPropertyRoomMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyRoomMaps.SingleOrDefault(u => u.iRoomId == id);
                if (dbobj != null)
                    eobj = (etblPropertyRoomMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Add new record
        public static int AddRecord(etblPropertyRoomMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    long RoomId = 0;
                    var CheckRoomCodeExists = db.tblPropertyRoomMaps.Any(
                                                               p => p.iPropId == eobj.iPropId
                                                                    && p.sRoomCode == eobj.sRoomCode);
                    if (CheckRoomCodeExists == false)
                    {
                        OneFineRate.tblPropertyRoomMap dbuser = (OneFineRate.tblPropertyRoomMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomMap());
                        db.tblPropertyRoomMaps.Add(dbuser);
                        db.SaveChanges();



                        RoomId = dbuser.iRoomId;
                        var CheckAllRoomExists = db.tblPropertyTaxMaps.Any(
                                                               p => p.iPropId == eobj.iPropId
                                                                    && p.iRoomId == null);
                        List<PropertyRoomTaxList> objPropTaxIdList = new List<PropertyRoomTaxList>();
                        if (CheckAllRoomExists == true)
                        {
                            objPropTaxIdList = (from t1 in db.tblPropertyTaxMaps
                                                where t1.iPropId == eobj.iPropId && t1.iRoomId == null
                                                select new PropertyRoomTaxList()
                                                {
                                                    dtStayFrom = (DateTime) t1.dtStayFrom,
                                                    dtStayTo = (DateTime) t1.dtStayTo,
                                                    iPropId = t1.iPropId,
                                                    iRoomId =  t1.iRoomId,
                                                    iPropTaxId = t1.iPropTaxId,
                                                    cStatus = t1.cStatus
                                                }).ToList();


                            #region Propperty Room Tax List
                            if (objPropTaxIdList != null)
                            {
                                List<PropertyRoomTaxList> LPRPC = new List<PropertyRoomTaxList>();
                                for (int i = 0; i < objPropTaxIdList.Count; i++)
                                {
                                        DateTime CurrentDate = (DateTime)objPropTaxIdList[i].dtStayFrom;
                                        while (CurrentDate <= objPropTaxIdList[i].dtStayTo)
                                        {
                                            PropertyRoomTaxList PRPC = new PropertyRoomTaxList();
                                            PRPC.dtStay = CurrentDate;
                                            PRPC.iPropTaxId = objPropTaxIdList[i].iPropTaxId;
                                            PRPC.iPropId = objPropTaxIdList[i].iPropId;
                                            PRPC.iRoomId = RoomId;
                                            PRPC.iActionBy = (int)eobj.iActionBy;
                                            PRPC.dtActionDate = DateTime.Now;
                                            PRPC.cStatus = objPropTaxIdList[i].cStatus;

                                            LPRPC.Add(PRPC);
                                            CurrentDate = CurrentDate.AddDays(1);
                                        }
                                }

                                if (LPRPC != null)
                                {
                                    db.tblPropertyRoomTaxMaps.AddRange(LPRPC.Select(m => new tblPropertyRoomTaxMap()
                                    {
                                        dtStay = m.dtStay,
                                        iPropTaxId = m.iPropTaxId,
                                        iPropId = m.iPropId,
                                        iRoomId = (long) m.iRoomId,
                                        dtActionDate = m.dtActionDate,
                                        iActionBy = m.iActionBy,
                                        cStatus = m.cStatus
                                    }));
                                    db.SaveChanges();
                                }
                            }
                            #endregion

                        }
                        
                        retval = 1;
                    }
                    else
                    {
                        retval = 2;
                    }



                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        //Update a record
        public static int UpdateRecord(etblPropertyRoomMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var CheckRoomCodeExists = db.tblPropertyRoomMaps.Any(
                                                              p => p.iPropId == eobj.iPropId
                                                                  && p.iRoomId != eobj.iRoomId
                                                                   && p.sRoomCode == eobj.sRoomCode);
                    if (CheckRoomCodeExists == false)
                    {
                        OneFineRate.tblPropertyRoomMap obj = (OneFineRate.tblPropertyRoomMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomMap());
                        db.tblPropertyRoomMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        retval = 1;
                    }
                    else
                    {
                        retval = 2;
                    }


                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Get list of records
        public static List<etblPropertyRoomMap> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, int propid, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblPropertyRoomMap> user = new List<etblPropertyRoomMap>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                //var objTblUser = db.tblPropertyRoomMaps.Where(u => u.sRoomName.Contains(param.sSearch)).AsQueryable();
                var objTblUser = (from t1 in db.tblPropertyRoomMaps
                                  join t2 in db.tblRoomTypeMs on t1.iRoomTypeId equals t2.iRoomTypeId
                                  where t1.iPropId == propid
                                  select new
                                  {
                                      t1.iRoomId,
                                      t1.iPropId,
                                      t1.sRoomName,
                                      t1.sRoomCode,
                                      t1.iMaxOccupancy,
                                      t1.iMaxChildren,
                                      t1.bActive,
                                      t1.dSizeMtr,
                                      t1.dSizeSqft,
                                      t2.sRoomType
                                  }).Where(u => u.sRoomName.Contains(param.sSearch) || u.sRoomType.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    //case 0:
                    //    objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.iPropId) : objTblUser.OrderByDescending(u => u.iPropId);
                    //    break;
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sRoomName) : objTblUser.OrderByDescending(u => u.sRoomName);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sRoomType) : objTblUser.OrderByDescending(u => u.sRoomType);
                        break;
                    case 3:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.iMaxOccupancy) : objTblUser.OrderByDescending(u => u.iMaxOccupancy);
                        break;
                    case 4:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.iMaxChildren) : objTblUser.OrderByDescending(u => u.iMaxChildren);
                        break;

                }

                ////implemented paging
                var data = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in data)
                {
                    user.Add((etblPropertyRoomMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyRoomMap()));
                }
                return user;
            }
        }
    }
}