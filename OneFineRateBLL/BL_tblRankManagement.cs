using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using OneFineRateAppUtil;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OneFineRateBLL
{
    public class BL_tblRankManagement
    {
        public static List<PropNames> GetPropertyList()
        {
            List<PropNames> data = new List<PropNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                data = (from t1 in db.tblPropertyMs
                        select new PropNames
                        {
                            PropId = t1.iPropId,
                            Name = t1.sHotelName + " (" + t1.iPropId + ")",
                            Id = t1.iPropId,
                            bIsTG = t1.bIsTG
                        }).Where(u => u.bIsTG == null || u.bIsTG == false).OrderBy(u => u.Name).AsQueryable().ToList();
                return data;
            }

        }

        //Add Rank
        public static int AddRecord(etblRankManagement eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    bool checkExists = false;
                    checkExists = db.tblRankManagements.Any(
                                         p => p.iPropId == eobj.iPropId
                                               && (((p.dtStartdate <= eobj.dtStartdate && p.dtEndDate >= eobj.dtStartdate) || (p.dtStartdate <= eobj.dtEndDate && p.dtEndDate >= eobj.dtEndDate))
                                                  || ((eobj.dtStartdate <= p.dtStartdate && eobj.dtEndDate >= p.dtStartdate) || (eobj.dtStartdate <= p.dtEndDate && eobj.dtEndDate >= p.dtEndDate)))
                                             );
                    if (checkExists != true)
                    {
                        OneFineRate.tblRankManagement dbuser = (OneFineRate.tblRankManagement)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblRankManagement());
                        db.tblRankManagements.Add(dbuser);
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
        //Update Rank
        public static int UpdateRecord(etblRankManagement eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    bool checkExists = false;
                    checkExists = db.tblRankManagements.Any(
                                        p => p.iPropId == eobj.iPropId
                                              && p.iID != eobj.iID
                                              && (((p.dtStartdate <= eobj.dtStartdate && p.dtEndDate >= eobj.dtStartdate) || (p.dtStartdate <= eobj.dtEndDate && p.dtEndDate >= eobj.dtEndDate))
                                                 || ((eobj.dtStartdate <= p.dtStartdate && eobj.dtEndDate >= p.dtStartdate) || (eobj.dtStartdate <= p.dtEndDate && eobj.dtEndDate >= p.dtEndDate)))
                                            );
                    if (checkExists != true)
                    {
                        OneFineRate.tblRankManagement obj = (OneFineRate.tblRankManagement)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblRankManagement());
                        db.tblRankManagements.Attach(obj);
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
        //Delete Rank
        public static int DeleteRecord(etblRankManagement eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblRankManagement obj = (OneFineRate.tblRankManagement)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblRankManagement());
                    db.tblRankManagements.Attach(obj);
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
        public static etblRankManagement GetRankManagementByID(int a)
        {
            etblRankManagement promorec = new etblRankManagement();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var rec = (from m in db.tblRankManagements
                           join c in db.tblUserMs on m.iActionBy equals c.iUserId
                           select new
                           {
                               m.iID,
                               m.iPropId,
                               m.dtStartdate,
                               m.dtEndDate,
                               m.IsSponsored,
                               m.IsRateDisparity,
                               m.iNewRank,
                               m.iOldRank,
                               m.cStatus,
                               UserName = c.sFirstName + " " + c.sLastName,
                               m.dtActionDate,
                               m.iActionBy
                           }).Where(u => u.iID == a).AsQueryable();
                var data = rec.ToList();

                etblRankManagement objMain = new etblRankManagement();
                objMain = (etblRankManagement)OneFineRateAppUtil.clsUtils.ConvertToObject(data[0], objMain);

                promorec.iID = objMain.iID;
                promorec.iPropId = objMain.iPropId;
                promorec.dtStartdate = objMain.dtStartdate;
                promorec.dtEndDate = objMain.dtEndDate;
                promorec.IsSponsored = objMain.IsSponsored;
                promorec.IsRateDisparity = objMain.IsRateDisparity;
                promorec.iNewRank = objMain.iNewRank;
                promorec.iOldRank = objMain.iOldRank;
                promorec.cStatus = objMain.cStatus;
                promorec.dtActionDate = objMain.dtActionDate;
                promorec.iActionBy = objMain.iActionBy;

                return promorec;
            }
        }


        public static List<etblRankManagement> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblRankManagement> RankManagementList = new List<etblRankManagement>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var objTblRatePlan = db.Database.SqlQuery<etblRankManagement>("uspGetRankManagement").ToList();
                var result = objTblRatePlan.Where(a => (a.sHotelName.ToLower().Contains(param.sSearch.ToLower())));

                //get Total Count for show total records 
                TotalCount = result.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 1:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.sHotelName) : result.OrderByDescending(u => u.sHotelName);
                        break;
                    case 5:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.iNewRank) : result.OrderByDescending(u => u.iNewRank);
                        break;
                    case 6:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.sRateDisparity) : result.OrderByDescending(u => u.sRateDisparity);
                        break;
                }

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    RankManagementList.Add((etblRankManagement)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblRankManagement()));
                }

                foreach (var item in RankManagementList)
                {
                    if (item.iNewRank == 0)
                    {
                        item.iNewRank = null;
                    }
                }

                return RankManagementList;
            }
        }

        // Get Rank
        public static string GetRank(string CheckInDate, int Propid)
        {
            string result = "0";
            try
            {
                string[] CDate = CheckInDate.Split('/');
                CheckInDate = Convert.ToString(CDate[1]) + "/" + Convert.ToString(CDate[0]) + "/" + Convert.ToString(CDate[2]);

                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@dtCheckIn", CheckInDate);
                MyParam[1] = new SqlParameter("@PropId", Propid);

                DataTable dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspCalculateRankOfHotel", MyParam).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    result = Convert.ToString(dt.Rows[0][1]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}