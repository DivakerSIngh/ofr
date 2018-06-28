using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace OneFineRateBLL
{
    public class BL_Dashboard
    {
        // Get Modify Booking Details for Notifications
        public static eDashBoardNotifications GetDashBoardNotifications_Test(int PropId)
        {
            eDashBoardNotifications obj = new eDashBoardNotifications();
            try
            {
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@iPropId", PropId);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspDashBoardNotifications", MyParam);

                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        List<Noti_Dates> lstOne = new List<Noti_Dates>();
                        foreach (DataRow drR in ds.Tables[0].Rows)
                        {
                            Noti_Dates objDates = new Noti_Dates();
                            objDates.sDate = Convert.ToString(drR["dtDate"]);
                            lstOne.Add(objDates);
                        }
                        obj.lstOne = lstOne;
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<Noti_Dates> lstTwo = new List<Noti_Dates>();
                        foreach (DataRow drR in ds.Tables[1].Rows)
                        {
                            Noti_Dates objDates = new Noti_Dates();
                            objDates.sDate = Convert.ToString(drR["dtDate"]);
                            lstTwo.Add(objDates);
                        }
                        obj.lstTwo = lstTwo;
                    }

                    List<string> lstAll = new List<string>();
                    if (!String.IsNullOrWhiteSpace(ds.Tables[2].Rows[0]["comp"].ToString()))
                    {
                        lstAll.Add(ds.Tables[2].Rows[0]["comp"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[3].Rows[0]["lost"].ToString()))
                    {
                        lstAll.Add(ds.Tables[3].Rows[0]["lost"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[4].Rows[0]["availability"].ToString()))
                    {
                        lstAll.Add(ds.Tables[4].Rows[0]["availability"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[5].Rows[0]["conversion_rate"].ToString()))
                    {
                        lstAll.Add(ds.Tables[5].Rows[0]["conversion_rate"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[6].Rows[0]["ranking_position"].ToString()))
                    {
                        lstAll.Add(ds.Tables[6].Rows[0]["ranking_position"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[7].Rows[0]["daily_visitors"].ToString()))
                    {
                        lstAll.Add(ds.Tables[7].Rows[0]["daily_visitors"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[8].Rows[0]["shortlisted"].ToString()))
                    {
                        lstAll.Add(ds.Tables[8].Rows[0]["shortlisted"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[9].Rows[0]["change"].ToString()))
                    {
                        lstAll.Add(ds.Tables[9].Rows[0]["change"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[10].Rows[0]["promotions"].ToString()))
                    {
                        lstAll.Add(ds.Tables[10].Rows[0]["promotions"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[11].Rows[0]["nights"].ToString()))
                    {
                        lstAll.Add(ds.Tables[11].Rows[0]["nights"].ToString());
                    }
                    if (!String.IsNullOrWhiteSpace(ds.Tables[12].Rows[0]["negotiation"].ToString()))
                    {
                        lstAll.Add(ds.Tables[12].Rows[0]["negotiation"].ToString());
                    }
                    obj.lstAll = lstAll;
                }

            }
            catch (Exception)
            {
            }
            return obj;
        }


        public static eDashBoardNotifications GetDashBoardNotifications(int PropId)
        {
            eDashBoardNotifications obj = new eDashBoardNotifications();
            try
            {
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@iPropId", PropId);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspDashBoardNotifications", MyParam);

                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        List<Noti_Dates> lstOne = new List<Noti_Dates>();
                        foreach (DataRow drR in ds.Tables[0].Rows)
                        {
                            Noti_Dates objDates = new Noti_Dates();
                            objDates.sDate = Convert.ToString(drR["dtDate"]);
                            lstOne.Add(objDates);
                        }
                        obj.lstOne = lstOne;
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<Noti_Dates> lstTwo = new List<Noti_Dates>();
                        foreach (DataRow drR in ds.Tables[1].Rows)
                        {
                            Noti_Dates objDates = new Noti_Dates();
                            objDates.sDate = Convert.ToString(drR["dtDate"]);
                            lstTwo.Add(objDates);
                        }
                        obj.lstTwo = lstTwo;
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        List<Noti_Dates> lstThree = new List<Noti_Dates>();
                        foreach (DataRow drR in ds.Tables[2].Rows)
                        {
                            Noti_Dates objDates = new Noti_Dates();
                            objDates.sDate = Convert.ToString(drR["dtDate"]);
                            lstThree.Add(objDates);
                        }
                        obj.lstThree = lstThree;
                    }

                    List<string> lstAll = new List<string>();
                    
                    if (ds.Tables[3] !=null && ds.Tables[3].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[3].Rows[0]["comp"].ToString()))
                    {
                        lstAll.Add(ds.Tables[3].Rows[0]["comp"].ToString());
                    }

                    if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[4].Rows[0]["compC"].ToString()))
                    {
                        lstAll.Add(ds.Tables[4].Rows[0]["compC"].ToString());
                    }

                    if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[5].Rows[0]["compB"].ToString()))
                    {
                        lstAll.Add(ds.Tables[5].Rows[0]["compB"].ToString());
                    }

                    if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[6].Rows[0]["lost"].ToString()))
                    {
                        lstAll.Add(ds.Tables[6].Rows[0]["lost"].ToString());
                    }

                    if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[7].Rows[0]["availability"].ToString()))
                    {
                        lstAll.Add(ds.Tables[7].Rows[0]["availability"].ToString());
                    }
                    if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[8].Rows[0]["conversion_rate"].ToString()))
                    {
                        lstAll.Add(ds.Tables[8].Rows[0]["conversion_rate"].ToString());
                    }
                    if (ds.Tables[9] != null && ds.Tables[9].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[9].Rows[0]["ranking_position"].ToString()))
                    {
                        lstAll.Add(ds.Tables[9].Rows[0]["ranking_position"].ToString());
                    }
                    if (ds.Tables[10] != null && ds.Tables[10].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[10].Rows[0]["daily_visitors"].ToString()))
                    {
                        lstAll.Add(ds.Tables[10].Rows[0]["daily_visitors"].ToString());
                    }
                    if (ds.Tables[11] != null && ds.Tables[11].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[11].Rows[0]["shortlisted"].ToString()))
                    {
                        lstAll.Add(ds.Tables[11].Rows[0]["shortlisted"].ToString());
                    }
                    if (ds.Tables[12] != null && ds.Tables[12].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[12].Rows[0]["change"].ToString()))
                    {
                        lstAll.Add(ds.Tables[12].Rows[0]["change"].ToString());
                    }
                    if (ds.Tables[13] != null && ds.Tables[13].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[13].Rows[0]["promotions"].ToString()))
                    {
                        lstAll.Add(ds.Tables[13].Rows[0]["promotions"].ToString());
                    }
                    if (ds.Tables[14] != null && ds.Tables[14].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[14].Rows[0]["nights"].ToString()))
                    {
                        lstAll.Add(ds.Tables[14].Rows[0]["nights"].ToString());
                    }
                    if (ds.Tables[15] != null && ds.Tables[15].Rows.Count > 0 && !String.IsNullOrWhiteSpace(ds.Tables[15].Rows[0]["negotiation"].ToString()))
                    {
                        lstAll.Add(ds.Tables[15].Rows[0]["negotiation"].ToString());
                    }
                    obj.lstAll = lstAll;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }
    }
}