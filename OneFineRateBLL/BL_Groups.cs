using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Groups
    {
        #region functions
        //Add new record
        public static int AddRecord(eGroups eobj, string Menus)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblGroupM dbGroup = (OneFineRate.tblGroupM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblGroupM());

                    var dbobj = db.tblGroupMs.SingleOrDefault(u => u.sGroupName == dbGroup.sGroupName);
                    if (dbobj != null)
                    {
                        return 2;
                    }
                    else
                    {
                        var mns = Menus.Split(',').ToList();
                        if (mns[0] != "")
                        {
                            db.tblGroupMs.Add(dbGroup);
                            db.SaveChanges();
                            db.tblGroupMenuMs.AddRange(mns.Select(menu => new tblGroupMenuM
                            {
                                cStatus = "A",
                                dtActionDate = DateTime.Now,
                                dtCreationDate = DateTime.Now,
                                iActionBy = dbGroup.iActionBy,
                                iCreatedBy = dbGroup.iActionBy,
                                iMenuId = Convert.ToInt32(menu),
                                iGroupId = dbGroup.iGroupId
                            }));

                            db.SaveChanges();
                            retval = 1;
                        }
                        else
                        {
                            retval = 2;
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
                        var obj1 = db.tblGroupMenuMs.Where(u => u.iGroupId == id);
                        db.tblGroupMenuMs.RemoveRange(obj1);
                    }
                    catch (Exception) { }

                    try
                    {
                        var obj1 = db.tblUserGroupMs.Where(u => u.iUserId == id);
                        db.tblUserGroupMs.RemoveRange(obj1);
                    }
                    catch (Exception) { }

                    var obj = db.tblGroupMs.SingleOrDefault(u => u.iGroupId == id);
                    db.tblGroupMs.Attach(obj);
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
        public static int UpdateRecord(eGroups eobj, string Menus)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var mns = Menus.Split(',').ToList();
                    if (mns.Count > 1)
                    {
                        OneFineRate.tblGroupM obj = (OneFineRate.tblGroupM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblGroupM());
                        db.tblGroupMs.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        try
                        {
                            var obj1 = db.tblGroupMenuMs.Where(u => u.iGroupId == obj.iGroupId);
                            db.tblGroupMenuMs.RemoveRange(obj1);
                        }
                        catch (Exception) { }

                        db.tblGroupMenuMs.AddRange(mns.Select(menu => new tblGroupMenuM
                         {
                             cStatus = "A",
                             dtActionDate = DateTime.Now,
                             dtCreationDate = DateTime.Now,
                             iActionBy = obj.iActionBy,
                             iCreatedBy = obj.iActionBy,
                             iMenuId = Convert.ToInt32(menu),
                             iGroupId = obj.iGroupId
                         }));
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

        //Get Single Record
        public static eGroups GetSingleRecordById(int id)
        {
            eGroups eobj = new eGroups();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblGroupMs.SingleOrDefault(u => u.iGroupId == id);
                if (dbobj != null)
                    eobj = (eGroups)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get list of records
        public static List<GroupsForGrid> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<GroupsForGrid> groups = new List<GroupsForGrid>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                //var objTblUser = db.tblUserMs.Where(u => u.sUserName.Contains(param.sSearch) || u.sFirstName.Contains(param.sSearch) || u.sLastName.Contains(param.sSearch) || u.sEmail.Contains(param.sSearch)).AsQueryable();

                var objTblGroup = (from s in db.tblGroupMs
                                   join c in db.tblUserMs on s.iCreatedBy equals c.iUserId into Groups
                                   from grps in Groups.DefaultIfEmpty()
                                   join c1 in db.tblUserMs on s.iActionBy equals c1.iUserId into Users
                                   from usrs in Users.DefaultIfEmpty()

                                   select new
                                   {
                                       s.iGroupId,
                                       s.sGroupName,
                                       s.sDescription,
                                       s.dtCreationDate,
                                       cStatus = s.cStatus == "A" ? "Live" : "Disable",
                                       s.dtActionDate,
                                       CreatedBy = grps.sFirstName + " " + grps.sLastName,
                                       ActionBy = usrs.sFirstName + " " + usrs.sLastName
                                   }).Where(u => u.sGroupName.Contains(param.sSearch)).AsQueryable();


                //get Total Count for show total records
                TotalCount = objTblGroup.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblGroup = param.sortDirection == "asc" ? objTblGroup.OrderBy(u => u.sGroupName) : objTblGroup.OrderByDescending(u => u.sGroupName);
                        break;
                    case 2:
                        objTblGroup = param.sortDirection == "asc" ? objTblGroup.OrderBy(u => u.cStatus) : objTblGroup.OrderByDescending(u => u.cStatus);
                        break;

                }


                ////implemented paging
                //List<tblUserM> lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var lstUser = objTblGroup.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object

                groups.AddRange(lstUser.Select(user => (GroupsForGrid)OneFineRateAppUtil.clsUtils.ConvertToObject(user, new GroupsForGrid())));

                return groups;
            }
        }

        public static string getMenuName(int imenutype)
        {


            string groupArr = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var objTblGroup = (from s in db.tblMenuMs
                                   select new
                                   {
                                       s.iMenuId,
                                       s.sMenuName,
                                       s.iParentId,
                                       s.iMenuType,
                                       s.iShownInMenu,
                                       s.iDispSeq
                                   }).OrderBy(u => u.iDispSeq).Where(u => (u.iParentId == null) && u.iMenuType == imenutype && u.iShownInMenu == 1).AsQueryable();


                //get Total Count for show total records
                int TotalCount = objTblGroup.Count();

                var lstUser = objTblGroup.ToList();



                foreach (var item in lstUser)
                {

                    groupArr += "{ \"item\": { \"id\":\"" + item.iMenuId + "\", \"label\": \"" + item.sMenuName + "\", \"checked\": false }, \"children\":[" + getMenuListing(imenutype, Convert.ToInt32(item.iMenuId)) + "]},";
                }
            }

            if (groupArr.Length > 0)
                return "[" + groupArr.Substring(0, groupArr.Length - 1) + "]";
            else
                return groupArr;

        }

        public static string getMenuListing(int imenutype, int iParentID)
        {
            string groupArr = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var objTblGroup = (from s in db.tblMenuMs
                                   select new
                                   {
                                       s.iMenuId,
                                       s.sMenuName,
                                       s.iParentId,
                                       s.iMenuType,
                                       s.iShownInMenu,
                                       s.iDispSeq
                                   }).OrderBy(u => u.iDispSeq).Where(u => (u.iParentId == iParentID) && u.iMenuType == imenutype && u.iShownInMenu == 1).AsQueryable();


                //get Total Count for show total records
                int TotalCount = objTblGroup.Count();

                var lstUser = objTblGroup.ToList();



                foreach (var item in lstUser)
                {

                    groupArr += "{ \"item\": { \"id\":\"" + item.iMenuId + "\", \"label\": \"" + item.sMenuName + "\", \"checked\": false }, \"children\":[" + getMenuListing(imenutype, Convert.ToInt32(item.iMenuId)) + "]},";
                }
            }
            if (groupArr.Length > 0)
                return groupArr.Substring(0, groupArr.Length - 1);
            else
                return groupArr;
        }

        public static string getMappedMenus(int a)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var groups = (from s in db.tblGroupMenuMs
                              select new
                              {
                                  s.iGroupId,
                                  s.iMenuId,

                              }).Where(u => u.iGroupId == a).AsQueryable();

                var lstGroup = groups.ToList();

                string groupArr = "";

                foreach (var item in lstGroup)
                {
                    groupArr += "{ \"id\" : " + item.iMenuId + "},";

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