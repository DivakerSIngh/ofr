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
    public class BL_CorporateCompanyM
    {
        public static eCorporateCompanyM GetCorporateCompanyById(int companyId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var m = db.tblCorporateCompanyMs.Where(x => x.iCompanyId == companyId).FirstOrDefault();

                var companyModel = new eCorporateCompanyM();

                var sDomainNames = string.Join(",", m.tblCorporateDomainMaps.Select(y => y.sDomainName));
                var domainType = m.tblCorporateDomainMaps.Select(y => y.sDomainType).FirstOrDefault() == DomainType.Domain.ToString() ? DomainType.Domain : DomainType.Email;

                companyModel.iCompanyId = m.iCompanyId;
                companyModel.sCompanyName = m.sCompanyName;
                companyModel.sDomainNames = sDomainNames;
                companyModel.DomainType = domainType;
                companyModel.sEmailAddress = m.sEmailAddress;
                companyModel.sMobileNumber = m.sMobileNumber;
                companyModel.sPrimaryContact = m.sPrimaryContact;
                companyModel.sTelephoneNumber = m.sTelephoneNumber;
                companyModel.cStatus = m.cStatus;
                companyModel.dtActionDate = m.dtActionDate;
                companyModel.iActionBy = m.iActionBy;
                companyModel.GstnEntityType = m.sEntityType;
                companyModel.GstnEntityName = m.sRegisteredEntityName;
                companyModel.GstnNumber = m.sGstinNumber;
                companyModel.PinCode = m.sPINCode;
                companyModel.CountryId = m.iCountryId;
                companyModel.StateId = m.iStateId;
                companyModel.CityId = m.iCityId;
                companyModel.Address = m.sAddress;

                companyModel.CountryCodePhoneList = BL_Country.GetAllCountryPhoneCodes();

                return companyModel;
            }

        }

        public static List<eCorporateCompanyM> GetCorporateCompaniesAutoComplete(string companySearchTxt)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var result = db.tblCorporateCompanyMs.Where(company => company.sCompanyName.Contains(companySearchTxt))
                    .Select(compny => new eCorporateCompanyM { iCompanyId = compny.iCompanyId, sCompanyName = compny.sCompanyName }).ToList();
                return result;
            }
        }

        public static KeyValuePair<int, string> ToggleStatus(int companyId, bool enable)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var comp = db.tblCorporateCompanyMs.Where(x => x.iCompanyId == companyId).FirstOrDefault();

                if (comp != null)
                {
                    if (enable)
                        comp.cStatus = "A";
                    else comp.cStatus = "I";
                    db.tblCorporateCompanyMs.Attach(comp);
                    db.Entry(comp).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return new KeyValuePair<int, string>(1, "Record has been updated successfully!");
                }

                return new KeyValuePair<int, string>(-1, "An error occured while updating the record.");
            }
        }

        public static KeyValuePair<int, string> AddEditCorporateCompany(eCorporateCompanyM company)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[18];
                    MyParam[0] = new SqlParameter("@iCompanyId", company.iCompanyId);
                    MyParam[1] = new SqlParameter("@sCompanyName", company.sCompanyName);
                    MyParam[2] = new SqlParameter("@sPrimaryContact", company.sPrimaryContact);
                    MyParam[3] = new SqlParameter("@sTelephoneNumber", company.sTelephoneNumber);
                    MyParam[4] = new SqlParameter("@sEmailAddress", company.sEmailAddress);
                    MyParam[5] = new SqlParameter("@sMobileNumber", company.sMobileNumber);
                    MyParam[6] = new SqlParameter("@domain", company.DomainDataTable);
                    MyParam[7] = new SqlParameter("@sActionType", company.sActionType);
                    MyParam[8] = new SqlParameter("@cStatus", company.cStatus);
                    MyParam[9] = new SqlParameter("@iActionBy", company.iActionBy);

                    MyParam[10] = new SqlParameter("@sEntityType", company.GstnEntityType);
                    MyParam[11] = new SqlParameter("@sRegisteredEntityName", company.GstnEntityName);
                    MyParam[12] = new SqlParameter("@sGstinNumber", company.GstnNumber);
                    MyParam[13] = new SqlParameter("@sPINCode", company.PinCode);
                    MyParam[14] = new SqlParameter("@iCountryId", company.CountryId);
                    MyParam[15] = new SqlParameter("@iStateId", company.StateId);
                    MyParam[16] = new SqlParameter("@iCityId", company.CityId);
                    MyParam[17] = new SqlParameter("@sAddress", company.Address);

                    var dataSet = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspAddUpdateDeleteCorporateCompany", MyParam);

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

        public static KeyValuePair<int, string> DeleteCorporateCompany(eCorporateCompanyM company)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@iCompanyId", company.iCompanyId);

                    var dataSet = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspDeleteCorporateCompany", MyParam);

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

        public static List<eCorporateCompanyM> GetCorporateCompanies(jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eCorporateCompanyM> macroarea = new List<eCorporateCompanyM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var objTblMacroArea = (from m in db.tblCorporateCompanyMs
                                       join c in db.tblCorporateDomainMaps on m.iCompanyId equals c.iCompanyId into ccm
                                       join u in db.tblUserMs on m.iActionBy equals u.iUserId into S1
                                       from user in S1.DefaultIfEmpty()
                                       select new eCorporateCompanyM
                                       {
                                           iCompanyId = m.iCompanyId,
                                           sCompanyName = m.sCompanyName,
                                           sDomainNameList = ccm.Select(y => y.sDomainName),
                                           sDomainNames = ccm.Select(y => y.sDomainName).FirstOrDefault(),
                                           sEmailAddress = m.sEmailAddress,
                                           sMobileNumber = m.sMobileNumber,
                                           sPrimaryContact = m.sPrimaryContact,
                                           sTelephoneNumber = m.sTelephoneNumber,
                                           cStatus = m.cStatus,
                                           dtActionDate = m.dtActionDate,
                                           iActionBy = m.iActionBy,
                                           sActionBy = user.sFirstName + " " + user.sLastName,
                                           GstnEntityType = m.sEntityType,
                                           GstnEntityName = m.sRegisteredEntityName,
                                           GstnNumber = m.sGstinNumber
                                       }
                                   )
                                   .Where(u => u.sCompanyName.Contains(param.sSearch)
                                   || u.sDomainNameList.Contains(param.sSearch)
                                   || u.sEmailAddress.Contains(param.sSearch)
                                   || u.sMobileNumber.Contains(param.sSearch)
                                   || u.sPrimaryContact.Contains(param.sSearch)
                                   || u.sActionBy.Contains(param.sSearch)).AsQueryable();

                TotalCount = objTblMacroArea.Count();



                switch (param.iSortingCols)
                {
                    case 0:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sCompanyName) : objTblMacroArea.OrderByDescending(u => u.sCompanyName);
                        break;
                    case 1:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sPrimaryContact) : objTblMacroArea.OrderByDescending(u => u.sPrimaryContact);
                        break;
                    case 2:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sTelephoneNumber) : objTblMacroArea.OrderByDescending(u => u.sTelephoneNumber);
                        break;
                    case 3:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sMobileNumber) : objTblMacroArea.OrderByDescending(u => u.sMobileNumber);
                        break;
                    case 4:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sEmailAddress) : objTblMacroArea.OrderByDescending(u => u.sEmailAddress);
                        break;
                    case 5:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sDomainNames) : objTblMacroArea.OrderByDescending(u => u.sDomainNames);
                        break;
                    case 6:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.cStatus) : objTblMacroArea.OrderByDescending(u => u.cStatus);
                        break;
                    default:
                        objTblMacroArea = objTblMacroArea.OrderBy(u => u.sCompanyName);
                        break;
                }

                var data = objTblMacroArea.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in data)
                {
                    item.sDomainNames = string.Join("<br/>", item.sDomainNameList);
                }

                return data;
            }
        }
    }
}