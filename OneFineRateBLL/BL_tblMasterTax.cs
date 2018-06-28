using Microsoft.SqlServer.Server;
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
    public class BL_tblMasterTaxMap
    {
        public static etblMasterTaxMap GetMasterTaxMapById(int masterTaxId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var model = (from mt in db.tblMasterTaxes
                             join rr in db.tblRoomRateRangeMs on mt.iRangeId equals rr.iRangeId
                             where mt.iMasterTaxId == masterTaxId
                             select new etblMasterTaxMap
                             {
                                 iMasterTaxId = mt.iMasterTaxId,
                                 cStatus = mt.cStatus,
                                 dtActionDate = mt.dtActionDate,
                                 iActionBy = mt.iActionBy,
                                 iRangeId = rr.iRangeId,
                                 dtStayFrom = mt.dtStayFrom,
                                 dtStayTo = mt.dtStayTo,
                                 IsAllStates = mt.bAllStates,
                                 iStateIdList = (from mtsm in db.tblMasterTaxStateMaps
                                                 where mtsm.iMasterTaxId == mt.iMasterTaxId
                                                 select mtsm).Select(x => x.iStateId.Value).ToList(),

                                 ListTaxes = (from mtm in db.tblMasterTaxMaps
                                              join tx in db.tblTaxMs on mtm.iTaxId equals tx.iTaxId
                                              where mtm.iMasterTaxId == mt.iMasterTaxId
                                              select new { mtm, tx }).Select(x => new eTax
                                              {
                                                  iTaxId = x.mtm.iTaxId,
                                                  sTaxName = x.tx.sTaxName,
                                                  Type = x.mtm.bIsPercent ? "p" : "v",
                                                  value = x.mtm.dValue.ToString()

                                              }).ToList()

                             }).FirstOrDefault();

                return model;
            }

        }

        public static List<eRoomRateRange> GetRoomRateRange()
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var model = (from t1 in db.tblRoomRateRangeMs
                             where t1.cStatus == "A"
                             select new eRoomRateRange
                             {
                                 iRangeId = t1.iRangeId,
                                 sRangeValue = t1.dAmountFrom + " - " + t1.dAmountTo
                             }).ToList();

                return model;
            }
        }

        public static KeyValuePair<int, string> ToggleStatus(int masterTaxMapId, bool enable, int actionBy)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var result = new KeyValuePair<int, string>();

                SqlParameter[] MyParam = new SqlParameter[3];
                MyParam[0] = new SqlParameter("@iMasterTaxId", masterTaxMapId);
                MyParam[1] = new SqlParameter("@cStatus", enable ? "A" : "I");
                MyParam[2] = new SqlParameter("@iActionBy", actionBy);

                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "[dbo].[uspEnableDisableMasterTax]", MyParam);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = new KeyValuePair<int, string>(Convert.ToInt32(ds.Tables[0].Rows[0]["status"].ToString()), ds.Tables[0].Rows[0]["errmsg"].ToString());
                }

                return result;
            }
        }

        public static KeyValuePair<int, string> AddUpdateMasterTaxMapping(etblMasterTaxMap model)
        {
            try
            {
                var result = new KeyValuePair<int, string>();

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    #region RoomOccupancySearch

                    DataTable dtStateIds = new DataTable("Ids");
                    dtStateIds.Columns.AddRange(new DataColumn[1]
                    {
                        new DataColumn("ID", typeof(int))
                    });

                    DataTable dtTaxes = new DataTable("Tax");
                    dtTaxes.Columns.AddRange(new DataColumn[3]
                    {
                    new DataColumn("iTaxId", typeof(int)),
                    new DataColumn("bIsPercent", typeof(int)),
                     new DataColumn("dValue", typeof(decimal))
                    });

                    for (int i = 0; i < model.iStateIds.Count(); i++)
                    {
                        DataRow stateRow = dtStateIds.NewRow();
                        stateRow["ID"] = model.iStateIds[i];

                        dtStateIds.Rows.Add(stateRow);
                    }

                    foreach (var item in model.ListTaxes)
                    {
                        DataRow taxRow = dtTaxes.NewRow();
                        taxRow["iTaxId"] = item.iTaxId;
                        taxRow["bIsPercent"] = item.Type == "p" ? true : false;
                        taxRow["dValue"] = item.value;
                        dtTaxes.Rows.Add(taxRow);
                    }

                    #endregion

                    SqlParameter[] MyParam = new SqlParameter[8];
                    MyParam[0] = new SqlParameter("@iMasterTaxId", model.iMasterTaxId);
                    MyParam[1] = new SqlParameter("@iRangeId", model.iRangeId);
                    MyParam[2] = new SqlParameter("@dtStayFrom", model.dtStayFrom);
                    MyParam[3] = new SqlParameter("@dtStayTo", model.dtStayTo);
                    MyParam[4] = new SqlParameter("@bAllStates", model.IsAllStates);
                    MyParam[5] = new SqlParameter("@Ids", dtStateIds);
                    MyParam[6] = new SqlParameter("@iActionBy", model.iActionBy);
                    MyParam[7] = new SqlParameter("@Tax", dtTaxes);

                    var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "[dbo].[uspSetMasterTax]", MyParam);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        result = new KeyValuePair<int, string>(Convert.ToInt32(ds.Tables[0].Rows[0]["status"].ToString()), ds.Tables[0].Rows[0]["errmsg"].ToString());
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new KeyValuePair<int, string>(-1, "An error occured, Details : " + ex.Message);
            }
        }

        public static List<etblMasterTaxMapForDatatable> GetMasterTaxMappings(jQueryDataTableParamModel param, out int TotalCount)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var list = new List<etblMasterTaxMapForDatatable>();

                    SqlParameter[] MyParam = new SqlParameter[6];
                    MyParam[0] = new SqlParameter("@Search", "%" + param.sSearch + "%");
                    MyParam[1] = new SqlParameter("@DisplayLength", param.iDisplayLength);
                    MyParam[2] = new SqlParameter("@DisplayStart", param.iDisplayStart);
                    MyParam[3] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
                    MyParam[4] = new SqlParameter("@SortBy", param.iSortingCols);
                    MyParam[5] = new SqlParameter("@TotalCount", 0)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };

                    var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "[dbo].[uspGetMasterTaxMappings]", MyParam);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            var record = new etblMasterTaxMapForDatatable();
                            record.iMasterTaxId = Convert.ToInt32(ds.Tables[0].Rows[i]["iMasterTaxId"]);
                            record.iRangeId = Convert.ToInt32(ds.Tables[0].Rows[i]["iRangeId"]);
                            record.sStayFrom = Convert.ToString(ds.Tables[0].Rows[i]["sStayFrom"]);
                            record.sStayTo = Convert.ToString(ds.Tables[0].Rows[i]["sStayTo"]);
                            record.sTax = Convert.ToString(ds.Tables[0].Rows[i]["sTax"]);
                            record.sAmountFrom = Convert.ToString(ds.Tables[0].Rows[i]["sAmountFrom"]);
                            record.sAmountTo = Convert.ToString(ds.Tables[0].Rows[i]["sAmountTo"]);
                            record.sTaxName = Convert.ToString(ds.Tables[0].Rows[i]["sTaxName"]);
                            record.cStatus = Convert.ToString(ds.Tables[0].Rows[i]["cStatus"]);
                            record.sActionBy = Convert.ToString(ds.Tables[0].Rows[i]["sActionBy"]);
                            record.sActionDate = Convert.ToString(ds.Tables[0].Rows[i]["sActionDate"]);
                            record.sState = Convert.ToString(ds.Tables[0].Rows[i]["sState"]);
                            record.IsAllState = Convert.ToBoolean(ds.Tables[0].Rows[i]["bAllStates"]);
                            list.Add(record);
                        }
                    }

                    TotalCount = Convert.ToInt32(MyParam[5].Value);
                    return list;

                }
            }
            catch (Exception ex)
            {

            }
            TotalCount = 0;
            return new List<etblMasterTaxMapForDatatable>();
        }
    }
}
