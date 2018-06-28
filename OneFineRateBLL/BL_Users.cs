using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Users
    {
        #region functions
        //Add new record
        public static int AddRecord(eUsers eobj, string Groups)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblUserM dbuser = (OneFineRate.tblUserM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblUserM());

                    var dbobj = db.tblUserMs.SingleOrDefault(u => u.sUserName == dbuser.sUserName);
                    if (dbobj != null)
                    {
                        return 2;
                    }

                    dbobj = db.tblUserMs.SingleOrDefault(u => u.sEmail == dbuser.sEmail);
                    if (dbobj != null)
                    {
                        return 3;
                    }

                    db.tblUserMs.Add(dbuser);
                    db.SaveChanges();

                    try
                    {
                        var obj = db.tblUserGroupMs.Where(u => u.iUserId == dbuser.iUserId);
                        db.tblUserGroupMs.RemoveRange(obj);
                    }
                    catch (Exception) { }


                    var grps = Groups.Split(',').ToList();
                    db.tblUserGroupMs.AddRange(grps.Select(x => new tblUserGroupM()
                    {
                        cStatus = "A",
                        dtActionDate = DateTime.Now,
                        dtCreationDate = DateTime.Now,
                        iActionBy = dbuser.iActionBy,
                        iCreatedBy = dbuser.iActionBy,
                        iGroupId = Convert.ToInt32(x),
                        iUserId = dbuser.iUserId
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
        //Delete a record
        public static int DeleteRecord(int id)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    try
                    {
                        var obj1 = db.tblUserGroupMs.Where(u => u.iUserId == id);
                        db.tblUserGroupMs.RemoveRange(obj1);
                    }
                    catch (Exception) { }

                    var obj = db.tblUserMs.SingleOrDefault(u => u.iUserId == id);
                    db.tblUserMs.Attach(obj);
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
        public static int UpdateRecord(eUsers eobj, string Groups)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblUserM dbuser = (OneFineRate.tblUserM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblUserM());

                    var dbobj = db.tblUserMs.SingleOrDefault(u => u.sUserName != dbuser.sUserName && u.sEmail == dbuser.sEmail);
                    if (dbobj != null)
                    {
                        return 3;
                    }

                    OneFineRate.tblUserM obj = (OneFineRate.tblUserM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblUserM());
                    db.tblUserMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    try
                    {
                        var obj1 = db.tblUserGroupMs.Where(u => u.iUserId == dbuser.iUserId);
                        db.tblUserGroupMs.RemoveRange(obj1);
                    }
                    catch (Exception) { }


                    var grps = Groups.Split(',').ToList();
                    db.tblUserGroupMs.AddRange(grps.Select(x => new tblUserGroupM()
                    {
                        cStatus = "A",
                        dtActionDate = DateTime.Now,
                        dtCreationDate = DateTime.Now,
                        iActionBy = dbuser.iActionBy,
                        iCreatedBy = dbuser.iActionBy,
                        iGroupId = Convert.ToInt32(x),
                        iUserId = dbuser.iUserId
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

        //Get Single Record
        public static eUsers GetSingleRecordById(int id)
        {
            eUsers eobj = new eUsers();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblUserMs.SingleOrDefault(u => u.iUserId == id);
                if (dbobj != null)
                    eobj = (eUsers)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        public static bool UpdateEmailVerified(long userId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                db.tblWebsiteUserMaters.Where(user => user.Id == userId).SingleOrDefault().EmailConfirmed = true;
                return db.SaveChanges()>0;
            }
        }

        //Get list of records
        public static List<eUserList> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    List<eUserList> objTblUserList = new List<eUserList>(); 
                    List<eUserList> objTblUser = new List<eUserList>();
                    param.sSearch = param.sSearch == null ? "" : param.sSearch;

                    objTblUser = db.Database.SqlQuery<eUserList>("uspGetAllUsers").ToList();
                    var result = objTblUser.Where(a => a.sEmail.ToLower().Contains(param.sSearch.ToLower())
                                                   || a.sFirstName.ToLower().Contains(param.sSearch.ToLower())
                                                   || a.sGroup.ToLower().Contains(param.sSearch.ToLower())
                                                   || a.sContact.ToLower().Contains(param.sSearch.ToLower()));

                    //get Total Count for show total records
                    TotalCount = result.Count();

                    //for sorting
                    switch (param.iSortingCols)
                    {
                        case 0:
                            result = param.sortDirection == "asc" ? result.OrderBy(u => u.sUserName) : result.OrderByDescending(u => u.sUserName);
                            break;
                        case 1:
                            result = param.sortDirection == "asc" ? result.OrderBy(u => u.sFirstName) : result.OrderByDescending(u => u.sFirstName);
                            break;
                        //case 3:
                        //    objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sEmail) : objTblUser.OrderByDescending(u => u.sEmail);
                        //    break;
                        case 4:
                            result = param.sortDirection == "asc" ? result.OrderBy(u => u.cStatus) : result.OrderByDescending(u => u.cStatus);
                            break;

                    }


                    ////implemented paging
                    var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                    //convert bll entity object to db entity object
                    foreach (var item in lstUser)
                    {
                        objTblUserList.Add((eUserList)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eUserList()));
                    }
                    return objTblUserList;

                }
                catch (Exception Ex)
                {

                    throw;
                }                
            }
        }

        public static string getGroupName()
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var groups = (from s in db.tblGroupMs
                              select new
                              {
                                  s.iGroupId,
                                  s.sGroupName,
                                  s.cStatus
                              }).Where(u => u.cStatus == "A").AsQueryable();

                var lstGroup = groups.ToList();

                string groupArr = "";

                foreach (var item in lstGroup)
                {
                    groupArr += "{ \"id\" : " + item.iGroupId + ", \"name\": \"" + item.sGroupName + "\"},";

                }
                try
                {
                    groupArr = "[" + groupArr.Substring(0, groupArr.Length - 1) + "]";
                }
                catch (Exception)
                {
                    groupArr = "[]";
                }
                return groupArr;
            }
        }

        public static string getMappedGroups(int a)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var groups = (from s in db.tblUserGroupMs
                              select new
                              {
                                  s.iGroupId,
                                  s.iUserId,

                              }).Where(u => u.iUserId == a).AsQueryable();

                var lstGroup = groups.ToList();

                string groupArr = "";

                foreach (var item in lstGroup)
                {
                    groupArr += "{ \"id\" : " + item.iGroupId + "},";

                }
                try
                {
                    groupArr = "[" + groupArr.Substring(0, groupArr.Length - 1) + "]";
                }
                catch (Exception)
                {
                    groupArr = "[]";
                }
                return groupArr;

            }
        }


        #endregion
    }
}