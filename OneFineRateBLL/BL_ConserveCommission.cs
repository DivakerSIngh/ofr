using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_ConserveCommission
    { //Get list of records
        public static List<eConserveCommissionDisp> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eConserveCommissionDisp> CClst = new List<eConserveCommissionDisp>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                var objTblCC = (from a in db.tblConserveCommissionMs
                                join c in db.tblUserMs on a.iActionBy equals c.iUserId into PC
                                from T in PC.DefaultIfEmpty()
                                join PPM in db.tblPropConserveCommissionMaps on a.iCCId equals PPM.iCCId into PPM1
                                from PPM2 in PPM1.DefaultIfEmpty()
                                select new
                                {
                                    a.iCCId,
                                    a.dCommission,
                                    a.dtFrom,
                                    a.dtTo,
                                    bDisplayRateComm = (bool)a.bDisplayRateComm ? "Yes" : "No",
                                    bBidComm = (bool)a.bBidComm ? "Yes" : "No",
                                    bCOComm = (bool)a.bCOComm ? "Yes" : "No",
                                    dCounterTrigger = (bool)a.bCOComm ? a.dCounterTrigger : 0,
                                    a.dtActionDate,
                                    cStatus = a.cStatus == "A" ? "Live" : "Disable",
                                    ActionBy = T.sFirstName + " " + T.sLastName,
                                    HotelCount = PPM1.Count()
                                }).Distinct().AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblCC.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblCC = param.sortDirection == "asc" ? objTblCC.OrderBy(u => u.iCCId) : objTblCC.OrderByDescending(u => u.iCCId);
                        break;
                    case 6:
                        objTblCC = param.sortDirection == "asc" ? objTblCC.OrderBy(u => u.dtFrom) : objTblCC.OrderByDescending(u => u.dtFrom);
                        break;
                    case 7:
                        objTblCC = param.sortDirection == "asc" ? objTblCC.OrderBy(u => u.dtTo) : objTblCC.OrderByDescending(u => u.dtTo);
                        break;
                    case 8:
                        objTblCC = param.sortDirection == "asc" ? objTblCC.OrderBy(u => u.cStatus) : objTblCC.OrderByDescending(u => u.cStatus);
                        break;
                }

                ////implemented paging
                var lstUser = objTblCC.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    CClst.Add((eConserveCommissionDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eConserveCommissionDisp()));
                }
                return CClst;
            }
        }

        public static int AddRecord(eConserveCommission eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblConserveCommissionM dbuser = (OneFineRate.tblConserveCommissionM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblConserveCommissionM());
                    db.tblConserveCommissionMs.Add(dbuser);
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
        public static int UpdateRecord(eConserveCommission eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblConserveCommissionM obj = (OneFineRate.tblConserveCommissionM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblConserveCommissionM());
                    db.tblConserveCommissionMs.Attach(obj);
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
        public static eConserveCommission GetSingleRecordById(int id)
        {
            eConserveCommission eobj = new eConserveCommission();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblConserveCommissionMs.SingleOrDefault(u => u.iCCId == id);
                if (dbobj != null)
                    eobj = (eConserveCommission)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        public static string GenrateMenu(int iCCid, string search)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.uspGetPropertiesAndChainByConserveCommission(iCCid, "%" + search + "%");

                // var lstResult = result.Where(x => x.Mapped == 1).ToList();
                var lstResult = result.ToList();

                string ReturnString = "";

                foreach (var item in lstResult)
                {
                    ReturnString += "{ \"id\" : " + item.iPropId + ", \"name\": \"" + item.PropName.Trim() + "\", \"parent\": \"" + item.chainName.Trim() + "\", \"checked\": \"" + item.Mapped + "\"},";
                }
                try
                {
                    ReturnString = "[" + ReturnString.Substring(0, ReturnString.Length - 1) + "]";
                }
                catch (Exception)
                {
                    ReturnString = "[]";
                }
                return ReturnString;
            }
        }
        public static int SaveHotelMapping(int id, string Hotels, int iActionBy)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    try
                    {
                        var obj1 = db.tblPropConserveCommissionMaps.Where(u => u.iCCId == id);
                        db.tblPropConserveCommissionMaps.RemoveRange(obj1);
                    }
                    catch (Exception) { }


                    var Hotel = Hotels.Split(',').ToList();

                    db.tblPropConserveCommissionMaps.AddRange(Hotel.Where(hotel => !hotel.Contains("Parent")).Select(hotel => new tblPropConserveCommissionMap()
                    {
                        cStatus = "A",
                        dtActionDate = DateTime.Now,
                        iActionBy = iActionBy,
                        iPropId = Convert.ToInt32(hotel),
                        iCCId = id
                    }));
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

        public static int SaveFilterHotelMapping(int id, string MapHotels, string UnmapHotels, int iActionBy)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    try
                    {
                        var TempHotelDel = UnmapHotels.Split(',').ToList();
                        var HotelDel = TempHotelDel.Where(hotel => !hotel.Contains("Parent"));
                        var obj1 = db.tblPropConserveCommissionMaps.Where(u => u.iCCId == id && HotelDel.Contains(u.iPropId.ToString()));
                        db.tblPropConserveCommissionMaps.RemoveRange(obj1);

                    }
                    catch (Exception) { }


                    var Hotel = !string.IsNullOrEmpty(MapHotels.Trim())? MapHotels.Split(',').ToList():null;
                    if (Hotel!=null)
                    {
                        db.tblPropConserveCommissionMaps.AddRange(Hotel.Where(hotel => !hotel.Contains("Parent")).Select(hotel => new tblPropConserveCommissionMap()
                        {
                            cStatus = "A",
                            dtActionDate = DateTime.Now,
                            iActionBy = iActionBy,
                            iPropId = Convert.ToInt32(hotel),
                            iCCId = id
                        }));
                    }
                    
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

        public static string CheckConserveCommissionHotelMapping(int id, string MapHotels)
        {
            string s = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    eConserveCommission eCC = GetSingleRecordById(id);
                    List<string> htls = MapHotels.Split(',').ToList();

                    var data = (from a in db.tblConserveCommissionMs.Where(u => u.iCCId != id && u.dtFrom <= eCC.dtTo && u.dtTo >= eCC.dtFrom && u.cStatus == "A")
                                join PPM in db.tblPropConserveCommissionMaps on a.iCCId equals PPM.iCCId into PPM1
                                from PPM2 in PPM1.DefaultIfEmpty()
                                join Prop in db.tblPropertyMs on PPM2.iPropId equals Prop.iPropId into Properties
                                from Property in Properties.DefaultIfEmpty()
                                select new
                                {
                                    a.iCCId,
                                    Property.sHotelName,
                                    Property.iPropId
                                }).Distinct().OrderBy(u => u.iCCId).AsQueryable().ToList();

                    if (data != null && data.Count > 0)
                    {
                        StringBuilder SS = new StringBuilder();
                        int i = 0;
                        SS.Append("");
                        foreach (var item in data)
                        {
                            if (htls.Contains(item.iPropId.ToString()))
                                if (i < 10)
                                {
                                    SS.Append(item.sHotelName + " (" + item.iPropId.ToString() + ")" + " is mapped with conserve commission ID : " + item.iCCId.ToString() + ".<br>");
                                    i++;
                                }
                                else
                                {
                                    SS.Append("And more...");
                                    break;
                                }
                               
                        }
                        s = SS.ToString();
                    }
                }
                catch (Exception) { }
            }
            return s;
        }
        //Update a record
        public static string CheckHotelMappingWhileEditOrEnable(eConserveCommission eobj)
        {
            string s = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var hotels = (from a in db.tblPropConserveCommissionMaps.Where(u => u.iCCId == eobj.iCCId)
                                  select new
                                  {
                                      a.iPropId
                                  }).Distinct().AsQueryable().ToList(); //MapHotels.Split(',').ToList();

                    List<string> htls = new List<string>();

                    foreach (var item in hotels)
                    {
                        htls.Add(item.iPropId.ToString());
                    }

                    var data = (from a in db.tblConserveCommissionMs.Where(u => u.iCCId != eobj.iCCId && u.dtFrom <= eobj.dtTo && u.dtTo >= eobj.dtFrom && u.cStatus == "A")
                                join PPM in db.tblPropConserveCommissionMaps on a.iCCId equals PPM.iCCId into PPM1
                                from PPM2 in PPM1.DefaultIfEmpty()
                                join Prop in db.tblPropertyMs on PPM2.iPropId equals Prop.iPropId into Properties
                                from Property in Properties.DefaultIfEmpty()
                                select new
                                {
                                    a.iCCId,
                                    Property.sHotelName,
                                    Property.iPropId
                                }).Distinct().OrderBy(u => u.iCCId).AsQueryable().ToList();

                    if (data != null && data.Count > 0)
                    {
                        StringBuilder SS = new StringBuilder();
                        int i = 0;
                        SS.Append("");
                        foreach (var item in data)
                        {
                            if (htls.Contains(item.iPropId.ToString()))
                                if (i < 10)
                                {
                                    SS.Append(item.sHotelName + " (" + item.iPropId.ToString() + ")" + " is mapped with conserve commission ID : " + item.iCCId.ToString() + ".<br>");
                                    i++;
                                }
                                else
                                {
                                    SS.Append(" And more...");
                                    break;
                                }
                            
                        }
                        s = SS.ToString();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return s;
        }
    }
}