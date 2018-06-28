using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_BlackListedDomainM
    {
        public static eBlackListedDomainM GetBlacklistedDomain(string domainName)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var m = db.tblBlackListedDomains.Where(x => x.sDomainName == domainName).FirstOrDefault();

                var blackListModel = new eBlackListedDomainM();

                blackListModel.sDomainName = m.sDomainName;
                blackListModel.cStatus = m.cStatus;
                blackListModel.dtActionDate = m.dtActionDate;
                blackListModel.iActionBy = m.iActionBy;

                return blackListModel;
            }

        }

        public static KeyValuePair<int, string> AddEditBlacklistDomain(eBlackListedDomainM domain)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[10];
                    MyParam[0] = new SqlParameter("@sDomainName", domain.sDomainName);
                    MyParam[1] = new SqlParameter("@sActionType", domain.sActionType);
                    MyParam[2] = new SqlParameter("@cStatus", domain.cStatus);
                    MyParam[3] = new SqlParameter("@iActionBy", domain.iActionBy);

                    var dataSet = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString,
                        CommandType.StoredProcedure, "uspAddDeleteBlackListDomain", MyParam);

                    int statusId = -1;
                    string message = string.Empty;

                    if (dataSet != null && dataSet.Tables[0] != null)
                    {
                        statusId = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
                        message = Convert.ToString(dataSet.Tables[0].Rows[0][1]);
                    }

                    return new KeyValuePair<int, string>(statusId, message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static KeyValuePair<int, string> DeleteBlackListDomain(eBlackListedDomainM domain)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@sDomainName", domain.sDomainName);

                    var dataSet = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspDeleteBlackListDomain", MyParam);

                    int statusId = -1;
                    string message = string.Empty;

                    if (dataSet != null && dataSet.Tables[0] != null)
                    {
                        statusId = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
                        message = Convert.ToString(dataSet.Tables[0].Rows[0][1]);
                    }

                    return new KeyValuePair<int, string>(statusId, message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static KeyValuePair<int, string> ToggleStatus(string sDomainName, bool enable)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var comp = db.tblBlackListedDomains.Where(x => x.sDomainName == sDomainName).FirstOrDefault();

                if (comp != null)
                {
                    if (enable)
                        comp.cStatus = "A";
                    else comp.cStatus = "I";
                    db.tblBlackListedDomains.Attach(comp);
                    db.Entry(comp).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return new KeyValuePair<int, string>(1, "Record has been updated successfully!");
                }

                return new KeyValuePair<int, string>(-1, "An error occured while updating the record.");
            }
        }




        public static List<eBlackListedDomainM> GetBlackListDomains(jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eBlackListedDomainM> macroarea = new List<eBlackListedDomainM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var blacklistedDomains = (from m in db.tblBlackListedDomains
                                          join u in db.tblUserMs on m.iActionBy equals u.iUserId into S1
                                          from user in S1.DefaultIfEmpty()
                                          select new eBlackListedDomainM
                                          {
                                              sDomainName = m.sDomainName,
                                              cStatus = m.cStatus,
                                              dtActionDate = m.dtActionDate,
                                              iActionBy = m.iActionBy,
                                              sActionBy = user.sFirstName + " " + user.sLastName
                                          }
                                   )
                                   .Distinct()
                                   .Where(u => u.sDomainName.Contains(param.sSearch)
                                       || u.sActionBy.Contains(param.sSearch))
                                   .AsQueryable();


                TotalCount = blacklistedDomains.Count();


                switch (param.iSortingCols)
                {
                    case 0:
                        blacklistedDomains = param.sortDirection == "asc" ? blacklistedDomains.OrderBy(u => u.sDomainName)
                            : blacklistedDomains.OrderByDescending(u => u.sDomainName);
                        break;
                    case 1:
                        blacklistedDomains = param.sortDirection == "asc" ? blacklistedDomains.OrderBy(u => u.sActionBy)
                            : blacklistedDomains.OrderByDescending(u => u.sActionBy);
                        break;
                    case 2:
                        blacklistedDomains = param.sortDirection == "asc" ? blacklistedDomains.OrderBy(u => u.cStatus)
                            : blacklistedDomains.OrderByDescending(u => u.cStatus);
                        break;
                    default:
                        blacklistedDomains = blacklistedDomains.OrderBy(u => u.sDomainName);
                        break;
                }

                var data = blacklistedDomains.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return data;
            }
        }
    }
}