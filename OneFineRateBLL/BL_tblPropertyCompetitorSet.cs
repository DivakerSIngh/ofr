using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblPropertyCompetitorSet
    {
        //Update a record
        public static int AddUpdateRecord(etblPropertyCompetitorSet eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    List<etblPropertyCompetitorSet> objOld = new List<etblPropertyCompetitorSet>();
                    List<etblPropertyCompetitorSet> objCommon = new List<etblPropertyCompetitorSet>();
                    List<etblPropertyCompetitorSet> objDelete = new List<etblPropertyCompetitorSet>();
                    List<etblPropertyCompetitorSet> objInsert = new List<etblPropertyCompetitorSet>();

                    #region Assign values to new object
                    objInsert = eobj.PropertyCompetitorSetList;
                    #endregion

                    #region Get Old Data
                    var resultOld = (from s in db.tblPropertyCompetitorSets
                                     select new
                                     {
                                         s.iPropId,
                                         s.iCPropId
                                     }).Where(u => u.iPropId == eobj.iPropId).ToList();

                    foreach (var item in resultOld)
                    {
                        objOld.Add((etblPropertyCompetitorSet)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyCompetitorSet()));
                    }
                    objDelete = objOld;
                    #endregion

                    #region Get Common Result and Remove from Existing Objects
                    var resultCommon = (from m in objDelete
                                        join n in objInsert on new { m.iPropId, m.iCPropId } equals new { n.iPropId, n.iCPropId }
                                        select new
                                        {
                                            m.iPropId,
                                            m.iCPropId
                                        }).Distinct().ToList();

                    foreach (var item in resultCommon)
                    {
                        objCommon.Add((etblPropertyCompetitorSet)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyCompetitorSet()));
                    }
                
                    foreach (var item in objCommon)
                    {
                        objDelete.RemoveAll(x => x.iPropId == item.iPropId && x.iCPropId == item.iCPropId);
                        objInsert.RemoveAll(x => x.iPropId == item.iPropId && x.iCPropId == item.iCPropId);
                    }

                    #endregion

                    #region Delete Records

                    for (int i = 0; i < objDelete.Count; i++)
                    {
                        OneFineRate.tblPropertyCompetitorSet obj = (OneFineRate.tblPropertyCompetitorSet)OneFineRateAppUtil.clsUtils.ConvertToObject(objDelete[i], new OneFineRate.tblPropertyCompetitorSet());
                        db.tblPropertyCompetitorSets.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }
                    #endregion

                    #region Insert Records
                    //Add mapings
                    if (eobj.PropertyCompetitorSetList != null)
                    {
                        db.tblPropertyCompetitorSets.AddRange(objInsert.Select(x => new tblPropertyCompetitorSet()
                        {
                            iPropId = x.iPropId,
                            iCPropId = x.iCPropId,
                            dtActionDate = x.dtActionDate,
                            iActionBy = x.iActionBy
                        }).ToList());
                    }
                    db.SaveChanges();
                    retval = 1;
                    #endregion

                    if (objDelete.Count > 0 || objInsert.Count > 0)
                    {
                        retval = 1;
                    }
                    else {
                        retval = 0;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        public static List<PNames> GetPropertyLocalityMap(int propId)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<PNames> lstPNames = BL_tblPropertyM.GetAllPropertyList(propId);
                objMapping = (from map in db.tblPropertyCompetitorSets.ToList()
                              join loc in lstPNames on map.iCPropId equals Convert.ToInt32(loc.Id)
                              where map.iPropId == propId
                              select new PNames()
                              {
                                  Id = loc.Id,
                                  Name = loc.Name
                              }).ToList();
                return objMapping;
            }
        }


    }
}