using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_ServiceChargeM
    {
        public static KeyValuePair<int, string> AddOrUpdate(eServiceChargeM model)
        {
            try
            {
                var dbModel = new tblServiceChargeM();

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    if (model.ServiceChargeId.HasValue && model.ServiceChargeId.Value != 0)
                    {
                        var duplicateCount = db.tblServiceChargeMs.Any(x => x.iServiceChargeId != model.ServiceChargeId
                        && ((model.dtFrom >= x.dtFrom && model.dtFrom <= x.dtTo)
                      || (model.dtTo >= x.dtFrom && model.dtTo <= x.dtTo)
                      || (model.dtFrom <= x.dtFrom && model.dtTo >= x.dtTo)) && x.cStatus == "A");

                        if (duplicateCount)
                        {
                            return new KeyValuePair<int, string>(-1, "Overlapping date range can not be assigned!");
                        }

                        dbModel.iServiceChargeId = model.ServiceChargeId.Value;
                    }
                    else
                    {
                        var duplicateCount = db.tblServiceChargeMs.Any(x => ((model.dtFrom >= x.dtFrom && model.dtFrom <= x.dtTo)
                      || (model.dtTo >= x.dtFrom && model.dtTo <= x.dtTo)
                      || (model.dtFrom <= x.dtFrom && model.dtTo >= x.dtTo)) && x.cStatus == "A");

                        if (duplicateCount)
                        {
                            return new KeyValuePair<int, string>(-1, "Overlapping date range can not be assigned!");
                        }
                    }

                    dbModel.cGstValueType = model.GstValueType;
                    dbModel.cStatus = model.Status;
                    dbModel.dGstValue = model.GstValue;
                    dbModel.dServiceCharge = model.ServiceCharge;
                    dbModel.dtActionDate = model.ActionDate;
                    dbModel.dtFrom = DateTime.ParseExact(model.From, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dbModel.dtTo = DateTime.ParseExact(model.To, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dbModel.iActionBy = model.ActionById.Value;

                    if (dbModel.iServiceChargeId > 0)
                    {
                        db.Entry(dbModel).State = EntityState.Modified;
                        db.SaveChanges();
                        return new KeyValuePair<int, string>(1, "Record has been added successfully!");
                    }
                    else
                    {
                        dbModel.cStatus = "A";
                        db.tblServiceChargeMs.Add(dbModel);
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

        public static eServiceChargeM GetServiceChargeById(int serviceChargeId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var serviceChargeModel =
                    (from m in db.tblServiceChargeMs
                     where m.iServiceChargeId == serviceChargeId
                     join u in db.tblUserMs on m.iActionBy equals u.iUserId
                     select new eServiceChargeM
                     {
                         ServiceChargeId = m.iServiceChargeId,
                         dtFrom = m.dtFrom,
                         dtTo = m.dtTo,
                         Status = m.cStatus,
                         ServiceCharge = m.dServiceCharge,
                         GstValueType = m.cGstValueType,
                         GstValue = m.dGstValue,
                         ActionById = m.iActionBy,
                         ActionByName = u.sFirstName + " " + u.sLastName,
                         ActionDate = m.dtActionDate
                     }).FirstOrDefault();

                serviceChargeModel.From = serviceChargeModel.dtFrom.Value.ToString("dd/MM/yyyy");
                serviceChargeModel.To = serviceChargeModel.dtTo.Value.ToString("dd/MM/yyyy");

                return serviceChargeModel;
            }
        }

        public static bool DeleteServiceCharge(int serviceChargeId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var serviceCharge = db.tblServiceChargeMs.FirstOrDefault(x => x.iServiceChargeId == serviceChargeId);

                if (serviceCharge != null)
                {
                    db.tblServiceChargeMs.Remove(serviceCharge);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }


        public static KeyValuePair<int, string> ToggleStatus(int serviceChargeId, bool enable)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var model = db.tblServiceChargeMs.Where(x => x.iServiceChargeId == serviceChargeId).FirstOrDefault();

                    var duplicateCount = db.tblServiceChargeMs.Any(x => x.iServiceChargeId != serviceChargeId
                        && ((model.dtFrom >= x.dtFrom && model.dtFrom <= x.dtTo)
                      || (model.dtTo >= x.dtFrom && model.dtTo <= x.dtTo)
                      || (model.dtFrom <= x.dtFrom && model.dtTo >= x.dtTo)) && x.cStatus == "A");

                    if (duplicateCount)
                    {
                        return new KeyValuePair<int, string>(-1, "Overlapping date range can not be assigned!");
                    }

                    if (model != null)
                    {
                        if (enable)
                            model.cStatus = "A";
                        else model.cStatus = "I";
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
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

        public static List<eServiceChargeM> GetServiceCharges(jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eServiceChargeM> macroarea = new List<eServiceChargeM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var objTblMacroArea =
                    (from m in db.tblServiceChargeMs
                     join u in db.tblUserMs on m.iActionBy equals u.iUserId into S1
                     from user in S1.DefaultIfEmpty()
                     select new eServiceChargeM
                     {
                         ServiceChargeId = m.iServiceChargeId,
                         dtFrom = m.dtFrom,
                         dtTo = m.dtTo,
                         Status = m.cStatus,
                         ServiceCharge = m.dServiceCharge,
                         GstValueType = m.cGstValueType,
                         GstValue = m.dGstValue,
                         ActionById = m.iActionBy,
                         ActionByName = user.sFirstName + " " + user.sLastName,
                         ActionDate = m.dtActionDate
                     }).Where(u => u.GstValue.ToString().Contains(param.sSearch)
                     || u.GstValueType.Contains(param.sSearch)
                     || u.ActionByName.Contains(param.sSearch)
                     || u.ServiceCharge.ToString().Contains(param.sSearch)).AsQueryable();

                TotalCount = objTblMacroArea.Count();

                switch (param.iSortingCols)
                {
                    case 0:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.ServiceChargeId) : objTblMacroArea.OrderByDescending(u => u.ServiceChargeId);
                        break;
                    case 1:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.ServiceCharge) : objTblMacroArea.OrderByDescending(u => u.ServiceCharge);
                        break;
                    case 2:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.GstValueType) : objTblMacroArea.OrderByDescending(u => u.GstValueType);
                        break;
                    case 3:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.GstValue) : objTblMacroArea.OrderByDescending(u => u.GstValue);
                        break;
                    case 4:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.From) : objTblMacroArea.OrderByDescending(u => u.From);
                        break;
                    case 5:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.To) : objTblMacroArea.OrderByDescending(u => u.To);
                        break;
                    case 6:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.ActionDate) : objTblMacroArea.OrderByDescending(u => u.ActionDate);
                        break;
                    default:
                        objTblMacroArea = objTblMacroArea.OrderBy(u => u.ActionByName);
                        break;
                }

                var data = objTblMacroArea.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in data)
                {
                    item.From = item.dtFrom.Value.ToString("dd/MM/yyyy");
                    item.To = item.dtTo.Value.ToString("dd/MM/yyyy");
                }

                return data;
            }
        }
    }
}