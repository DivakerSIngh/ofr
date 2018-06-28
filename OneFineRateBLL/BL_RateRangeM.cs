using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_RoomRateRangeM
    {
        public static KeyValuePair<int, string> AddOrUpdate(etblRoomRateRangeM model)
        {
            try
            {
                var dbModel = new tblRoomRateRangeM();

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    if (model.iRoomRateRangeId.HasValue && model.iRoomRateRangeId.Value != 0)
                    {
                        var duplicateCount = db.tblRoomRateRangeMs
                            .Any(x => x.iRangeId != model.iRoomRateRangeId
                        && ((model.dAmountFrom >= x.dAmountFrom && model.dAmountFrom <= x.dAmountTo)
                      || (model.dAmountTo >= x.dAmountFrom && model.dAmountTo <= x.dAmountTo)
                      || (model.dAmountFrom <= x.dAmountFrom && model.dAmountTo >= x.dAmountTo)));

                        if (duplicateCount)
                        {
                            return new KeyValuePair<int, string>(-1, "Overlapping range can not be assigned!");
                        }

                        dbModel.iRangeId = model.iRoomRateRangeId.Value;
                    }
                    else
                    {
                        var duplicateCount = db.tblRoomRateRangeMs
                            .Any(x => (model.dAmountFrom >= x.dAmountFrom && model.dAmountFrom <= x.dAmountTo)
                      || (model.dAmountTo >= x.dAmountFrom && model.dAmountTo <= x.dAmountTo)
                      || (model.dAmountFrom <= x.dAmountFrom && model.dAmountTo >= x.dAmountTo));

                        if (duplicateCount)
                        {
                            return new KeyValuePair<int, string>(-1, "Overlapping range can not be assigned!");
                        }
                    }

                    //var duplicateCount = db.tblRoomRateRangeMs.Any(x => x.iRangeId != model.iRoomRateRangeId
                    //   && (model.dAmountFrom>= x.dAmountFrom&&model.dAmountTo<x.dAmountTo));

                    //if (duplicateCount)
                    //{
                    //    return new KeyValuePair<int, string>(-1, "This amount range has already been assigned!");
                    //}

                    dbModel.iRangeId = model.iRoomRateRangeId.Value;
                    dbModel.cStatus = model.cStatus;
                    dbModel.dtActionDate = model.dtActionDate;
                    dbModel.dAmountFrom = model.dAmountFrom;
                    dbModel.dAmountTo = model.dAmountTo;
                    dbModel.iActionBy = model.iActionBy.Value;

                    if (dbModel.iRangeId > 0)
                    {
                        db.Entry(dbModel).State = EntityState.Modified;
                        db.SaveChanges();
                        return new KeyValuePair<int, string>(1, "Record has been added successfully!");
                    }
                    else
                    {
                        dbModel.cStatus = "A";
                        db.tblRoomRateRangeMs.Add(dbModel);
                        db.SaveChanges();
                        return new KeyValuePair<int, string>(1, "Record has been updated successfully!");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static etblRoomRateRangeM GetRateRangeById(int RateRangeId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var RateRangeModel =
                    (from m in db.tblRoomRateRangeMs
                     where m.iRangeId == RateRangeId
                     join u in db.tblUserMs on m.iActionBy equals u.iUserId
                     select new etblRoomRateRangeM
                     {
                         iRoomRateRangeId = m.iRangeId,
                         dAmountFrom = m.dAmountFrom.Value,
                         dAmountTo = m.dAmountTo.Value,
                         cStatus = m.cStatus,
                         iActionBy = m.iActionBy,
                         sActionByName = u.sFirstName + " " + u.sLastName,
                         dtActionDate = m.dtActionDate
                     }).FirstOrDefault();

                return RateRangeModel;
            }
        }

        public static bool DeleteRoomRateRange(int RateRangeId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var RateRange = db.tblRoomRateRangeMs.FirstOrDefault(x => x.iRangeId == RateRangeId);

                if (RateRange != null)
                {
                    db.tblRoomRateRangeMs.Remove(RateRange);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }


        public static KeyValuePair<int, string> ToggleStatus(int RateRangeId, bool enable)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var comp = db.tblRoomRateRangeMs.Where(x => x.iRangeId == RateRangeId).FirstOrDefault();

                    if (comp != null)
                    {
                        if (enable)
                            comp.cStatus = "A";
                        else comp.cStatus = "I";
                        db.Entry(comp).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return new KeyValuePair<int, string>(1, "Status updated successfully!");
                    }

                    return new KeyValuePair<int, string>(-1, "An error occured while updating the record.");
                }
            }
            catch (Exception)
            {
                return new KeyValuePair<int, string>(-1, "An error occured while updating the record.");
            }
        }

        public static List<etblRoomRateRangeM> GetRateRanges(jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblRoomRateRangeM> macroarea = new List<etblRoomRateRangeM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var objTblMacroArea =
                    (from m in db.tblRoomRateRangeMs
                     join u in db.tblUserMs on m.iActionBy equals u.iUserId into S1
                     from user in S1.DefaultIfEmpty()
                     select new etblRoomRateRangeM
                     {
                         iRoomRateRangeId = m.iRangeId,
                         dAmountFrom = m.dAmountFrom.Value,
                         dAmountTo = m.dAmountTo.Value,
                         cStatus = m.cStatus,
                         iActionBy = m.iActionBy,
                         sActionByName = user.sFirstName + " " + user.sLastName,
                         dtActionDate = m.dtActionDate

                     }).Where(x => x.sActionByName.Contains(param.sSearch)
                     || x.dAmountFrom.ToString().Contains(param.sSearch)
                     || x.dAmountTo.ToString().Contains(param.sSearch)).AsQueryable();

                TotalCount = objTblMacroArea.Count();

                switch (param.iSortingCols)
                {
                    case 0:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.iRoomRateRangeId) : objTblMacroArea.OrderByDescending(u => u.iRoomRateRangeId);
                        break;
                    case 1:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.dAmountFrom) : objTblMacroArea.OrderByDescending(u => u.dAmountFrom);
                        break;
                    case 2:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.dAmountTo) : objTblMacroArea.OrderByDescending(u => u.dAmountTo);
                        break;
                    case 4:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.dtActionDate) : objTblMacroArea.OrderByDescending(u => u.dtActionDate);
                        break;
                    default:
                        objTblMacroArea = objTblMacroArea.OrderBy(u => u.sActionByName);
                        break;
                }

                var data = objTblMacroArea.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return data;
            }
        }
    }
}