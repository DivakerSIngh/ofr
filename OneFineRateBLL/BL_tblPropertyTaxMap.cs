using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Data.SqlClient;
using OneFineRateAppUtil;
using System.Data;

namespace OneFineRateBLL
{
    public class BL_tblPropertyTaxMap
    {

        public static int CheckStatus(int PropId, int RatePlanId, int RoomTypeId, DateTime stayfrom, DateTime stayto, int TaxId)
        {
            int result = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                result = db.tblPropertyTaxMaps.Where(u => u.iPropId == PropId && u.iRPId == RatePlanId && u.iRoomId == RoomTypeId && ((stayfrom <= u.dtStayFrom && stayto >= u.dtStayFrom) || (stayfrom <= u.dtStayTo && stayto >= u.dtStayTo) || (stayfrom >= u.dtStayFrom && stayto <= u.dtStayTo)) && u.iPropTaxId != TaxId).AsQueryable().Count();
            }
            return result;

        }
        //Get Single Record
        public static etblPropertyTaxMap GetSingleRecordById(int id)
        {
            etblPropertyTaxMap eobj = new etblPropertyTaxMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyTaxMaps.SingleOrDefault(u => u.iPropTaxId == id);
                if (dbobj != null)
                    eobj = (etblPropertyTaxMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get Single Record
        public static Boolean CheckRecordExist(int ipropid, Nullable<long> iroomId, int iproptaxId)
        {
            Boolean result = false;
            etblPropertyTaxMap eobj = new etblPropertyTaxMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyTaxMaps.SingleOrDefault(u => u.iPropId == ipropid && u.iRoomId == iroomId && u.iPropTaxId != iproptaxId);
                if (dbobj != null)
                    eobj = (etblPropertyTaxMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);

                if (eobj.iPropTaxId != 0)
                    result = true;
                else
                    result = false;
            }
            return result;

        }
        //Add new record
        public static int AddRecord(etblPropertyTaxMap eobj)
        {
            int PropTaxId = 0;
            string Status = "";
            int retval = 0;
            int count = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    bool checkExists = false;
                    //checkExists = CheckTaxRecordExists(eobj);
                    if (eobj.iRoomId != null)
                    {
                        checkExists = db.tblPropertyTaxMaps.Any(
                                           p => p.iPropId == eobj.iPropId
                                               && (p.iRoomId == eobj.iRoomId || p.iRoomId == null)
                                               && p.cStatus == "A"
                                                && (((p.dtStayFrom <= eobj.dtStayFrom && p.dtStayTo >= eobj.dtStayFrom) || (p.dtStayFrom <= eobj.dtStayTo && p.dtStayTo >= eobj.dtStayTo))
                                                    || ((eobj.dtStayFrom <= p.dtStayFrom && eobj.dtStayTo >= p.dtStayFrom) || (eobj.dtStayFrom <= p.dtStayTo && eobj.dtStayTo >= p.dtStayTo)))
                                               );
                    }
                    else
                    {
                        checkExists = db.tblPropertyTaxMaps.Any(
                                                p => p.iPropId == eobj.iPropId
                                                    //&& ((p.iRoomId != null && p.cStatus != "A") || (p.iRoomId == null))
                                                    && (((p.iRoomId != null) && p.cStatus == "A") || ((p.iRoomId == null)))

                                                    && (((p.dtStayFrom <= eobj.dtStayFrom && p.dtStayTo >= eobj.dtStayFrom) || (p.dtStayFrom <= eobj.dtStayTo && p.dtStayTo >= eobj.dtStayTo))
                                                    || ((eobj.dtStayFrom <= p.dtStayFrom && eobj.dtStayTo >= p.dtStayFrom) || (eobj.dtStayFrom <= p.dtStayTo && eobj.dtStayTo >= p.dtStayTo)))
                                                    );

                    }

                    if (checkExists != true)
                    {
                        OneFineRate.tblPropertyTaxMap dbuser = (OneFineRate.tblPropertyTaxMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyTaxMap());
                        db.tblPropertyTaxMaps.Add(dbuser);
                        db.SaveChanges();
                        PropTaxId = dbuser.iPropTaxId;
                        Status = dbuser.cStatus;

                        OneFineRate.tblPropertyTaxesMap dbnew = (OneFineRate.tblPropertyTaxesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyTaxesMap());
                        //Add mapings
                        if (eobj.PropertyTaxesList != null)
                        {
                            db.tblPropertyTaxesMaps.AddRange(eobj.PropertyTaxesList.Select(x => new tblPropertyTaxesMap()
                            {
                                iPropTaxId = PropTaxId,
                                iTaxId = x.iTaxId,
                                bIsPercent = x.bIsPercent,
                                dValue = x.dValue,
                                dtActionDate = x.dtActionDate,
                                iActionBy = x.iActionBy
                            }).ToList());
                        }

                        db.SaveChanges();

                        #region Propperty Room Tax List

                        List<PropertyRoomTaxList> LPRPC = new List<PropertyRoomTaxList>();

                        if (eobj.iRoomId != null)
                        {
                            DateTime CurrentDate = (DateTime)eobj.dtStayFrom;
                            while (CurrentDate <= eobj.dtStayTo)
                            {
                                PropertyRoomTaxList PRPC = new PropertyRoomTaxList();
                                PRPC.dtStay = CurrentDate;
                                PRPC.iPropTaxId = PropTaxId;
                                PRPC.iPropId = eobj.iPropId;
                                PRPC.iRoomId = (long)eobj.iRoomId;
                                PRPC.iActionBy = (int)eobj.iActionBy;
                                PRPC.dtActionDate = DateTime.Now;
                                PRPC.cStatus = Status;

                                LPRPC.Add(PRPC);
                                CurrentDate = CurrentDate.AddDays(1);
                            }
                        }
                        else
                        {
                            List<PNames> objRoomList = new List<PNames>();
                            objRoomList = BL_tblPropertyRoomMap.GetAllPropertyTypes(eobj.iPropId);
                            foreach (var item in objRoomList)
                            {
                                DateTime CurrentDate = (DateTime)eobj.dtStayFrom;
                                while (CurrentDate <= eobj.dtStayTo)
                                {
                                    PropertyRoomTaxList PRPC = new PropertyRoomTaxList();
                                    PRPC.dtStay = CurrentDate;
                                    PRPC.iPropTaxId = PropTaxId;
                                    PRPC.iPropId = eobj.iPropId;
                                    PRPC.iRoomId = Convert.ToInt64(item.Id);
                                    PRPC.iActionBy = (int)eobj.iActionBy;
                                    PRPC.dtActionDate = DateTime.Now;
                                    PRPC.cStatus = Status;


                                    LPRPC.Add(PRPC);
                                    CurrentDate = CurrentDate.AddDays(1);
                                }
                            }
                        }

                        if (LPRPC != null)
                        {
                            db.tblPropertyRoomTaxMaps.AddRange(LPRPC.Select(m => new tblPropertyRoomTaxMap()
                            {
                                dtStay = m.dtStay,
                                iPropTaxId = m.iPropTaxId,
                                iPropId = m.iPropId,
                                iRoomId = (long)m.iRoomId,
                                dtActionDate = m.dtActionDate,
                                iActionBy = m.iActionBy,
                                cStatus = m.cStatus
                            }));
                        }

                        db.SaveChanges();
                        #endregion

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
        public static int UpdateRecord(etblPropertyTaxMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var dbobjRoomId = db.tblPropertyTaxMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId && u.iPropTaxId == eobj.iPropTaxId);
                    eobj.iRoomId = dbobjRoomId.iRoomId;

                    bool checkExists = false;

                    if (dbobjRoomId.iRoomId != null)
                    {
                        checkExists = db.tblPropertyTaxMaps.Any(
                                               p => p.iPropId == eobj.iPropId
                                                    && (p.iRoomId == eobj.iRoomId || p.iRoomId == null)
                                                    && p.cStatus == "A"
                                                    && p.iPropTaxId != eobj.iPropTaxId
                                                   // && p.iRoomId == null
                                                   // && p.cStatus == "A"
                                                   && (((p.dtStayFrom <= eobj.dtStayFrom && p.dtStayTo >= eobj.dtStayFrom) || (p.dtStayFrom <= eobj.dtStayTo && p.dtStayTo >= eobj.dtStayTo))
                                                        || ((eobj.dtStayFrom <= p.dtStayFrom && eobj.dtStayTo >= p.dtStayFrom) || (eobj.dtStayFrom <= p.dtStayTo && eobj.dtStayTo >= p.dtStayTo)))
                                                   );

                    }
                    else
                    {
                        checkExists = db.tblPropertyTaxMaps.Any(
                                          p => p.iPropId == eobj.iPropId
                                              && ((p.iRoomId == null) || (p.iRoomId != null && p.cStatus == "A"))
                                              && p.iPropTaxId != eobj.iPropTaxId
                                              && (((p.dtStayFrom <= eobj.dtStayFrom && p.dtStayTo >= eobj.dtStayFrom) || (p.dtStayFrom <= eobj.dtStayTo && p.dtStayTo >= eobj.dtStayTo))
                                                    || ((eobj.dtStayFrom <= p.dtStayFrom && eobj.dtStayTo >= p.dtStayFrom) || (eobj.dtStayFrom <= p.dtStayTo && eobj.dtStayTo >= p.dtStayTo)))
                                              );
                    }

                    if (checkExists != true)
                    {
                        using (OneFineRateEntities db1 = new OneFineRateEntities())
                        {
                            OneFineRate.tblPropertyTaxMap obj = (OneFineRate.tblPropertyTaxMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyTaxMap());
                            db1.tblPropertyTaxMaps.Attach(obj);
                            db1.Entry(obj).State = System.Data.Entity.EntityState.Modified;

                            //remove old mapings
                            db1.tblPropertyTaxesMaps.RemoveRange(db1.tblPropertyTaxesMaps.Where(x => x.iPropTaxId == eobj.iPropTaxId));

                            if (eobj.PropertyTaxesList != null)
                            {
                                db1.tblPropertyTaxesMaps.AddRange(eobj.PropertyTaxesList.Select(x => new tblPropertyTaxesMap()
                                {
                                    iPropTaxId = eobj.iPropTaxId,
                                    iTaxId = x.iTaxId,
                                    bIsPercent = x.bIsPercent,
                                    dValue = x.dValue,
                                    dtActionDate = x.dtActionDate,
                                    iActionBy = x.iActionBy
                                }).ToList());
                            }

                            db1.SaveChanges();




                            #region Propperty Room Tax List

                            //db1.tblPropertyRoomTaxMaps.RemoveRange(db1.tblPropertyRoomTaxMaps.Where(n => n.iPropTaxId == eobj.iPropTaxId));

                            SqlParameter[] MyParam = new SqlParameter[1];
                            MyParam[0] = new SqlParameter("@PropTaxId", eobj.iPropTaxId);

                            db.Database.ExecuteSqlCommand("uspRemovetblPropertyRoomTaxMaps @PropTaxId", MyParam);


                            List<PropertyRoomTaxList> LPRPC = new List<PropertyRoomTaxList>();
                            if (eobj.iRoomId != null)
                            {
                                DateTime CurrentDate = (DateTime)eobj.dtStayFrom;
                                while (CurrentDate <= eobj.dtStayTo)
                                {
                                    PropertyRoomTaxList PRPC = new PropertyRoomTaxList();
                                    PRPC.dtStay = CurrentDate;
                                    PRPC.iPropTaxId = eobj.iPropTaxId;
                                    PRPC.iPropId = eobj.iPropId;
                                    PRPC.iRoomId = (long)eobj.iRoomId;
                                    PRPC.iActionBy = (int)eobj.iActionBy;
                                    PRPC.dtActionDate = DateTime.Now;
                                    PRPC.cStatus = eobj.cStatus;

                                    LPRPC.Add(PRPC);
                                    CurrentDate = CurrentDate.AddDays(1);
                                }
                            }
                            else
                            {
                                List<PNames> objRoomList = new List<PNames>();
                                objRoomList = BL_tblPropertyRoomMap.GetAllPropertyTypes(eobj.iPropId);
                                foreach (var item in objRoomList)
                                {
                                    DateTime CurrentDate = (DateTime)eobj.dtStayFrom;
                                    while (CurrentDate <= eobj.dtStayTo)
                                    {
                                        PropertyRoomTaxList PRPC = new PropertyRoomTaxList();
                                        PRPC.dtStay = CurrentDate;
                                        PRPC.iPropTaxId = eobj.iPropTaxId; ;
                                        PRPC.iPropId = eobj.iPropId;
                                        PRPC.iRoomId = Convert.ToInt64(item.Id);
                                        PRPC.iActionBy = (int)eobj.iActionBy;
                                        PRPC.dtActionDate = DateTime.Now;
                                        PRPC.cStatus = eobj.cStatus;

                                        LPRPC.Add(PRPC);
                                        CurrentDate = CurrentDate.AddDays(1);
                                    }
                                }
                            }

                            if (LPRPC != null)
                            {
                                db.tblPropertyRoomTaxMaps.AddRange(LPRPC.Select(m => new tblPropertyRoomTaxMap()
                                {
                                    dtStay = m.dtStay,
                                    iPropTaxId = m.iPropTaxId,
                                    iPropId = m.iPropId,
                                    iRoomId = (long)m.iRoomId,
                                    dtActionDate = m.dtActionDate,
                                    iActionBy = m.iActionBy,
                                    cStatus = m.cStatus
                                }));
                            }

                            db.SaveChanges();
                            #endregion
                            retval = 1;
                        }

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
        // Please Check In
        //Update active inactive
        public static int UpdateStatus(etblPropertyTaxMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var dbobjRoomId = db.tblPropertyTaxMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId && u.iPropTaxId == eobj.iPropTaxId);
                    eobj.iRoomId = dbobjRoomId.iRoomId;
                    if (eobj.cStatus == "A")
                    {
                        bool checkExists = false;
                        if (dbobjRoomId.iRoomId != null)
                        {
                            checkExists = db.tblPropertyTaxMaps.Any(
                                                   p => p.iPropId == eobj.iPropId
                                                         //&& p.iRoomId == null
                                                         //&& p.cStatus == "A"
                                                         && (p.iRoomId == eobj.iRoomId || p.iRoomId == null)
                                                    && p.cStatus == "A"
                                                    && p.iPropTaxId != eobj.iPropTaxId
                                                       && (((p.dtStayFrom <= eobj.dtStayFrom && p.dtStayTo >= eobj.dtStayFrom) || (p.dtStayFrom <= eobj.dtStayTo && p.dtStayTo >= eobj.dtStayTo))
                                                            || ((eobj.dtStayFrom <= p.dtStayFrom && eobj.dtStayTo >= p.dtStayFrom) || (eobj.dtStayFrom <= p.dtStayTo && eobj.dtStayTo >= p.dtStayTo)))
                                                       );
                        }
                        else
                        {
                            checkExists = db.tblPropertyTaxMaps.Any(
                                              p => p.iPropId == eobj.iPropId
                                                   //&& (p.iRoomId != null)
                                                   //&& p.cStatus == "A"
                                                   && ((p.iRoomId == null) || (p.iRoomId != null && p.cStatus == "A"))
                                              && p.iPropTaxId != eobj.iPropTaxId
                                                  && (((p.dtStayFrom <= eobj.dtStayFrom && p.dtStayTo >= eobj.dtStayFrom) || (p.dtStayFrom <= eobj.dtStayTo && p.dtStayTo >= eobj.dtStayTo))
                                                        || ((eobj.dtStayFrom <= p.dtStayFrom && eobj.dtStayTo >= p.dtStayFrom) || (eobj.dtStayFrom <= p.dtStayTo && eobj.dtStayTo >= p.dtStayTo)))
                                                  );
                        }

                        if (checkExists != true)
                        {
                            using (OneFineRateEntities db1 = new OneFineRateEntities())
                            {
                                OneFineRate.tblPropertyTaxMap obj = (OneFineRate.tblPropertyTaxMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyTaxMap());
                                db1.tblPropertyTaxMaps.Attach(obj);
                                db1.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                                db1.SaveChanges();
                                retval = 1;
                            }

                        }
                        else
                        {
                            retval = 2;
                        }
                    }
                    else
                    {
                        using (OneFineRateEntities db2 = new OneFineRateEntities())
                        {

                            OneFineRate.tblPropertyTaxMap obj = (OneFineRate.tblPropertyTaxMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyTaxMap());
                            db2.tblPropertyTaxMaps.Attach(obj);
                            db2.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                            db2.SaveChanges();
                            retval = 1;
                        }

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
        public static List<PropertyTaxList> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, int propid, out int TotalCount)
        {

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<PropertyTaxList> user = new List<PropertyTaxList>();
                SqlParameter[] MyParam = new SqlParameter[7];
                MyParam[0] = new SqlParameter("@RoomType", param.sSearch + "%");
                MyParam[1] = new SqlParameter("@DisplayLength", param.iDisplayLength);
                MyParam[2] = new SqlParameter("@DisplayStart", param.iDisplayStart);
                MyParam[3] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
                MyParam[4] = new SqlParameter("@SortBy", param.iSortingCols);
                MyParam[5] = new SqlParameter("@PropId", propid);
                MyParam[6] = new SqlParameter("@TotalCount", 0);
                MyParam[6].Direction = System.Data.ParameterDirection.Output;

                user = db.Database.SqlQuery<PropertyTaxList>("uspGetPropertyTaxByRoomType @RoomType, @DisplayLength, @DisplayStart, @SortDirection, @SortBy,@PropId, @TotalCount out", MyParam).ToList();

                //get Total Count for show total records
                TotalCount = Convert.ToInt16(MyParam[6].Value); //user.Count();
                //TotalCount = user.Count();
                return user;
            }
        }



        public static Boolean CheckTaxRecordExists(etblPropertyTaxMap eobj)
        {
            Boolean result = false;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<PropertyTaxList> user = new List<PropertyTaxList>();
                SqlParameter[] MyParam = new SqlParameter[5];
                MyParam[0] = new SqlParameter("@StayFrom", eobj.dtStayFrom);
                MyParam[1] = new SqlParameter("@StayTo", eobj.dtStayTo);
                MyParam[2] = new SqlParameter("@PropId", eobj.iPropId);
                MyParam[3] = new SqlParameter("@RoomId", eobj.iRoomId == null ? 0 : eobj.iRoomId);
                MyParam[4] = new SqlParameter("@TotalCount", 0);
                MyParam[4].Direction = System.Data.ParameterDirection.Output;

                user = db.Database.SqlQuery<PropertyTaxList>("uspCheckTaxRecordExists @StayFrom, @StayTo, @PropId , @RoomId , @TotalCount out", MyParam).ToList();

                if (user.Count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;

        }
        


        public static KeyValuePair<int, string> CheckAffectedBookingsAfterPropertyTaxChange(DateTime stayFrom, DateTime stayTo)
        {
            try
            {
                var result = new KeyValuePair<int, string>();

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[3];
                    MyParam[0] = new SqlParameter("@bAllDates", 0);
                    MyParam[1] = new SqlParameter("@dtStart", stayFrom);
                    MyParam[2] = new SqlParameter("@dtEnd", stayTo);

                    var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "[dbo].[uspGetBookingsForTaxChange]", MyParam);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        result = new KeyValuePair<int, string>(Convert.ToInt32(ds.Tables[0].Rows[0]["status"].ToString()), ds.Tables[0].Rows[0]["errmsg"].ToString());
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new KeyValuePair<int, string>(-1, "An error occured, Details : " + ex.Message);
            }
        }
    }
}