using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblBannerM
    {


        //Get all records for home page banners
        public static List<etblBannerM> GetAllBannersRecords()
        {
            List<etblBannerM> eobj = new List<etblBannerM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var bannerList = db.tblBannerMs.Where(u =>  u.cstatus == "A" ).ToList();

                foreach (OneFineRate.tblBannerM item in bannerList) //&& u.sImgUrl != null
                {
                    eobj.Add((etblBannerM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblBannerM()));
                }
            }
            return eobj;
        }
        //Add new Banner
        public static int AddRecord(etblBannerM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    bool chkPageBottom = false; 
                    bool chkExists = db.tblBannerMs.Any(
                                            p => p.sBannerName == eobj.sBannerName
                                                //&& p.sBannerType == eobj.sBannerType
                                                && p.cstatus == eobj.cstatus
                                                 );
                    if (eobj.sBannerType == "Page Bottom")
                    {
                        chkPageBottom = db.tblBannerMs.Any(
                                                                    p => p.sBannerType == eobj.sBannerType
                                                                );

                    }

                    if (chkExists != true)
                    {
                        if (chkPageBottom != true)
                        {
                            OneFineRate.tblBannerM dbuser = (OneFineRate.tblBannerM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblBannerM());
                            db.tblBannerMs.Add(dbuser);
                            db.SaveChanges();
                            retval = 1;
                        }
                        else
                        {  retval = 3; }
                    }
                    else
                    { retval = 2; }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Update new Banner
        public static int UpdateRecord(etblBannerM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
     
                        OneFineRate.tblBannerM obj = (OneFineRate.tblBannerM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblBannerM());
                        db.tblBannerMs.Attach(obj);
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
        //Delete a record
        public static int DeleteRecord(etblBannerM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblBannerM obj = (OneFineRate.tblBannerM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblBannerM());
                    db.tblBannerMs.Attach(obj);
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
        public static etblBannerM GetBannerDetailsByID(int BannerId)
        {
            etblBannerM promorec = new etblBannerM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var rec = (from m in db.tblBannerMs
                           join c in db.tblUserMs on m.iActionBy equals c.iUserId
                           select new
                           {
                               m.iBannerId,
                               m.sBannerName,
                               m.sBannerType,
                               m.sDescription,
                               m.sTextPosition,
                               m.sLinkId,
                               m.cstatus,
                               m.sImgUrl,
                               m.iResolutionH,
                               m.iResolutionW,
                               m.UniqueAzureFileName,
                               UserName = c.sFirstName + " " + c.sLastName
                           }
                                   ).Where(u => u.iBannerId == BannerId).AsQueryable();
                var data = rec.ToList();
                promorec = (etblBannerM)OneFineRateAppUtil.clsUtils.ConvertToObject(data[0], promorec);
                return promorec;
            }
        }

        public static List<etblBannerM> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblBannerM> bannerRec = new List<etblBannerM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                var objTbl = (from m in db.tblBannerMs
                              join u in db.tblUserMs on m.iActionBy equals u.iUserId
                              select new
                              {
                                  m.iBannerId,
                                  m.sBannerName,
                                  m.sBannerType,
                                  m.sDescription,
                                  m.sTextPosition,
                                  m.sLinkId,
                                  m.sImgUrl,
                                  m.iResolutionH,
                                  m.iResolutionW,
                                  m.UniqueAzureFileName,
                                  cstatus = m.cstatus == "A" ? "Live" : "Disable"
                                  // iActionBy = u.sFirstName + " " + u.sLastName
                              }
                                   ).Where(u => u.sBannerName.Contains(param.sSearch) || u.sBannerType.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTbl.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTbl = param.sortDirection == "asc" ? objTbl.OrderBy(u => u.sBannerType) : objTbl.OrderByDescending(u => u.sBannerType);
                        break;
                    case 2:
                        objTbl = param.sortDirection == "asc" ? objTbl.OrderBy(u => u.sBannerName) : objTbl.OrderByDescending(u => u.sBannerName);
                        break;
                    case 5:
                        objTbl = param.sortDirection == "asc" ? objTbl.OrderBy(u => u.cstatus) : objTbl.OrderByDescending(u => u.cstatus);
                        break;
                }

                ////implemented paging
                var data = objTbl.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();



                //convert bll entity object to db entity object
                foreach (var item in data)
                {
                    bannerRec.Add((etblBannerM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblBannerM()));
                }
                return bannerRec;
            }
        }

    }
}