using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRoomInventory
    {
        //Update a record
        public static int AddUpdateRecord(int propid, int ActionBy, DataTable PropertyRoomInventory, DataTable PropertyRoomRatePlanInventoryMap,Boolean Type)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[5];
                    MyParam[0] = new SqlParameter("@PropertyRoomInventory", PropertyRoomInventory);
                    MyParam[0].TypeName = "[dbo].[PropertyRoomInventory]";
                    MyParam[1] = new SqlParameter("@PropertyRoomRatePlanInventoryMap", PropertyRoomRatePlanInventoryMap);
                    MyParam[1].TypeName = "[dbo].[PropertyRoomRatePlanInventoryMap]";
                    MyParam[2] = new SqlParameter("@iActionBy", ActionBy);
                    MyParam[3] = new SqlParameter("@iPropId", propid);
                    MyParam[4] = new SqlParameter("@CloseOut", Type);


                    db.Database.ExecuteSqlCommand("uspUpdateRateInventory @PropertyRoomInventory, @PropertyRoomRatePlanInventoryMap, @iActionBy, @iPropId,@CloseOut ", MyParam);
                    retval = 1;

                }
                catch (Exception)
                {
                    retval = 0;
                    throw;
                }
            }
            return retval;
        }
        //Get Single Record
        public static etblPropertyRoomInventory GetSingleRecordById(int roomid, DateTime inventorydate)
        {
            etblPropertyRoomInventory eobj = new etblPropertyRoomInventory();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from t1 in db.tblPropertyRoomInventories
                             join t2 in db.tblPropertyRoomMaps on new { t1.iRoomId, t1.iPropId } equals new { t2.iRoomId, t2.iPropId }
                             select new
                             {
                                 t1.dtInventoryDate,
                                 t1.iPropId,
                                 t1.iRoomId,
                                 t1.iInventory,
                                 t1.bStopSell,
                                 t1.iCutOff,
                                 t1.dtActionDate,
                                 t1.iActionBy,
                                 t1.iAvailableInventory,
                                 t2.sRoomName
                             }).Where(u => u.dtInventoryDate == inventorydate && u.iRoomId == roomid).AsQueryable().SingleOrDefault();

                if (dbobj != null)
                    eobj = (etblPropertyRoomInventory)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);

            }
            return eobj;

        }
     
        //Update a record
        public static int UpdateRecord(etblPropertyRoomInventory eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    //remove old mapings
                    db.tblPropertyRoomInventories.RemoveRange(db.tblPropertyRoomInventories.Where(x => x.iRoomId == eobj.iRoomId && x.dtInventoryDate == eobj.dtInventoryDate));

                    OneFineRate.tblPropertyRoomInventory obj = (OneFineRate.tblPropertyRoomInventory)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomInventory());
                    db.tblPropertyRoomInventories.Add(obj);

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

        public static DataSet GetDataforPropertyRateInventory(int propid, string cdate)
        {
            clsDB obj= new clsDB();
            DataSet ds = new DataSet();
            ds = obj.ListReminders(propid, cdate);
            return ds;
        }


        public class roomdesc
        {
            public int RowID { get; set; }
            public string sname { get; set; }
            public int ctype { get; set; }
            public int ilinked { get; set; }
            public int irid { get; set; }
            public int irpid { get; set; }
        }
        public class calenderdates
        {
            public string name { get; set; }
            public string cdate { get; set; }
        }
        public class RoomInventory
        {
            public List<roomdesc> desc { get; set; }
            public List<calenderdates> cdates { get; set; }
        }
   
    }
}