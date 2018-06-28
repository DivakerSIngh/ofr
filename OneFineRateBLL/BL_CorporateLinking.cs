using Newtonsoft.Json;
using OneFineRate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_CorporateLinking
    {
        public static string Save(int iUserId, int PropId, string Todate, string Fromdate, string basicDisc, string LengthDisc, string LengthAmenity1, string LengthAppl1,
            string LengthAmenity2, string LengthAppl2, string MultipleDisc, string MultipleAmenity1, string MultipleAppl1, string MultipleAmenity2, string MultipleAppl2,
            string LeadDisc, string LeadAmenity1, string LeadAppl1, string LeadAmenity2, string LeadAppl2, string WeekendDisc, string WeekendAmenity1, string WeekendAppl1,
            string WeekendAmenity2, string WeekendAppl2)
        {

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    DataTable CorporateLinking = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtDate", typeof(DateTime));
                    CorporateLinking.Columns.Add(col);
                    col = new DataColumn("sType", typeof(String));
                    CorporateLinking.Columns.Add(col);
                    col = new DataColumn("dDisc", typeof(Decimal));
                    CorporateLinking.Columns.Add(col);
                    col = new DataColumn("iRateInclusion1", typeof(Int32));
                    CorporateLinking.Columns.Add(col);
                    col = new DataColumn("iApplicability1", typeof(Int16));
                    CorporateLinking.Columns.Add(col);
                    col = new DataColumn("iRateInclusion2", typeof(Int32));
                    CorporateLinking.Columns.Add(col);
                    col = new DataColumn("iApplicability2", typeof(Int16));
                    CorporateLinking.Columns.Add(col);


                    DateTime Start = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(Fromdate);
                    DateTime End = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(Todate);

                    while (Start <= End)
                    {
                        if (basicDisc != "")
                        {
                            DataRow drBasic = CorporateLinking.NewRow();
                            drBasic["dtDate"] = Start;
                            drBasic["sType"] = "Basic";
                            drBasic["dDisc"] = Convert.ToDecimal(basicDisc);
                            drBasic["iRateInclusion1"] = 0;
                            drBasic["iApplicability1"] = 0;
                            drBasic["iRateInclusion2"] = 0;
                            drBasic["iApplicability2"] = 0;
                            CorporateLinking.Rows.Add(drBasic);
                        }
                        if (LengthDisc != "")
                        {
                            DataRow drLength = CorporateLinking.NewRow();
                            drLength["dtDate"] = Start;
                            drLength["sType"] = "Length";
                            drLength["dDisc"] = Convert.ToDecimal(LengthDisc);
                            drLength["iRateInclusion1"] = Convert.ToInt32(LengthAmenity1);
                            drLength["iApplicability1"] = Convert.ToInt32(LengthAppl1);
                            drLength["iRateInclusion2"] = Convert.ToInt32(LengthAmenity2);
                            drLength["iApplicability2"] = Convert.ToInt32(LengthAppl2);
                            CorporateLinking.Rows.Add(drLength);
                        }
                        if (MultipleDisc != "")
                        {
                            DataRow drMultiple = CorporateLinking.NewRow();
                            drMultiple["dtDate"] = Start;
                            drMultiple["sType"] = "Rooms";
                            drMultiple["dDisc"] = Convert.ToDecimal(MultipleDisc);
                            drMultiple["iRateInclusion1"] = Convert.ToInt32(MultipleAmenity1);
                            drMultiple["iApplicability1"] = Convert.ToInt32(MultipleAppl1);
                            drMultiple["iRateInclusion2"] = Convert.ToInt32(MultipleAmenity2);
                            drMultiple["iApplicability2"] = Convert.ToInt32(MultipleAppl2);
                            CorporateLinking.Rows.Add(drMultiple);
                        }
                        if (LeadDisc != "")
                        {
                            DataRow drLead = CorporateLinking.NewRow();
                            drLead["dtDate"] = Start;
                            drLead["sType"] = "Lead";
                            drLead["dDisc"] = Convert.ToDecimal(LeadDisc);
                            drLead["iRateInclusion1"] = Convert.ToInt32(LeadAmenity1);
                            drLead["iApplicability1"] = Convert.ToInt32(LeadAppl1);
                            drLead["iRateInclusion2"] = Convert.ToInt32(LeadAmenity2);
                            drLead["iApplicability2"] = Convert.ToInt32(LeadAppl2);
                            CorporateLinking.Rows.Add(drLead);
                        }
                        if (WeekendDisc != "")
                        {
                            DataRow drWeekend = CorporateLinking.NewRow();
                            drWeekend["dtDate"] = Start;
                            drWeekend["sType"] = "Weekend";
                            drWeekend["dDisc"] = Convert.ToDecimal(WeekendDisc);
                            drWeekend["iRateInclusion1"] = Convert.ToInt32(WeekendAmenity1);
                            drWeekend["iApplicability1"] = Convert.ToInt32(WeekendAppl1);
                            drWeekend["iRateInclusion2"] = Convert.ToInt32(WeekendAmenity2);
                            drWeekend["iApplicability2"] = Convert.ToInt32(WeekendAppl2);
                            CorporateLinking.Rows.Add(drWeekend);
                        }
                        Start = Start.AddDays(1);
                    }
                    List<MissingBidDates> Dates = new List<MissingBidDates>();
                    SqlParameter[] MyParam = new SqlParameter[3];
                    MyParam[0] = new SqlParameter("@CorporateLinking", CorporateLinking);
                    MyParam[0].TypeName = "[dbo].[CorporateLinking]";
                    MyParam[1] = new SqlParameter("@iPropId", PropId);
                    MyParam[2] = new SqlParameter("@iActionBy", iUserId);
                    Dates = db.Database.SqlQuery<MissingBidDates>("uspSaveCorporateLinking @CorporateLinking, @iPropId, @iActionBy", MyParam).ToList();
                    if (Dates.Count > 0)
                    {
                        var js = JsonConvert.SerializeObject(Dates);
                        return js.ToString();
                    }
                    else
                    {
                        return "a";
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }


        }


        public static CorporateDiscountModel GetCorporateDiscountRange(int propertyId)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    CorporateDiscountModel corporateDiscountModel = null;

                    SqlParameter[] sqlParameters = new SqlParameter[1];

                    sqlParameters[0] = new SqlParameter("@iPropId", propertyId);

                    var dataSet = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspGetLinkingRange", sqlParameters);

                    if (dataSet != null)
                    {
                        corporateDiscountModel = new CorporateDiscountModel();

                        #region Basic Discount

                        if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                            {
                                var baseDiscountModel = new BaseDiscountModel();
                                baseDiscountModel.dtFrom = Convert.ToString(dataSet.Tables[0].Rows[i]["dtFrom"]);
                                baseDiscountModel.dtTo = Convert.ToString(dataSet.Tables[0].Rows[i]["dtTo"]);
                                baseDiscountModel.dDisc = Convert.ToString(dataSet.Tables[0].Rows[i]["dDisc"]);
                                //baseDiscountModel.Amen1 = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amen1"]);
                                //baseDiscountModel.Appl1 = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Appl1"]);
                                //baseDiscountModel.Amen2 = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amen2"]);
                                //baseDiscountModel.Appl2 = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Appl2"]);

                                corporateDiscountModel.BasicDiscount.Add(baseDiscountModel);
                            }
                        }

                        #endregion

                        #region Length of Stay

                        if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dataSet.Tables[1].Rows.Count; i++)
                            {
                                var baseDiscountModel = new BaseDiscountModel();
                                baseDiscountModel.dtFrom = Convert.ToString(dataSet.Tables[1].Rows[i]["dtFrom"]);
                                baseDiscountModel.dtTo = Convert.ToString(dataSet.Tables[1].Rows[i]["dtTo"]);
                                baseDiscountModel.dDisc = Convert.ToString(dataSet.Tables[1].Rows[i]["dDisc"]);
                                baseDiscountModel.Amen1 = Convert.ToString(dataSet.Tables[1].Rows[i]["Amen1"]);
                                baseDiscountModel.Appl1 = Convert.ToString(dataSet.Tables[1].Rows[i]["Appl1"]);
                                baseDiscountModel.Amen2 = Convert.ToString(dataSet.Tables[1].Rows[i]["Amen2"]);
                                baseDiscountModel.Appl2 = Convert.ToString(dataSet.Tables[1].Rows[i]["Appl2"]);

                                corporateDiscountModel.LengthOfStay.Add(baseDiscountModel);
                            }
                        }

                        #endregion


                        #region Multiple Rooms

                        if (dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                        {
                            for (int i = 0; i < dataSet.Tables[2].Rows.Count; i++)
                            {
                                var baseDiscountModel = new BaseDiscountModel();
                                baseDiscountModel.dtFrom = Convert.ToString(dataSet.Tables[2].Rows[i]["dtFrom"]);
                                baseDiscountModel.dtTo = Convert.ToString(dataSet.Tables[2].Rows[i]["dtTo"]);
                                baseDiscountModel.dDisc = Convert.ToString(dataSet.Tables[2].Rows[i]["dDisc"]);
                                baseDiscountModel.Amen1 = Convert.ToString(dataSet.Tables[2].Rows[i]["Amen1"]);
                                baseDiscountModel.Appl1 = Convert.ToString(dataSet.Tables[2].Rows[i]["Appl1"]);
                                baseDiscountModel.Amen2 = Convert.ToString(dataSet.Tables[2].Rows[i]["Amen2"]);
                                baseDiscountModel.Appl2 = Convert.ToString(dataSet.Tables[2].Rows[i]["Appl2"]);

                                corporateDiscountModel.MultipleRooms.Add(baseDiscountModel);
                            }
                        }

                        #endregion


                        #region Lead Time

                        if (dataSet.Tables[3] != null && dataSet.Tables[3].Rows.Count > 0)
                        {
                            for (int i = 0; i < dataSet.Tables[3].Rows.Count; i++)
                            {
                                var baseDiscountModel = new BaseDiscountModel();
                                baseDiscountModel.dtFrom = Convert.ToString(dataSet.Tables[3].Rows[i]["dtFrom"]);
                                baseDiscountModel.dtTo = Convert.ToString(dataSet.Tables[3].Rows[i]["dtTo"]);
                                baseDiscountModel.dDisc = Convert.ToString(dataSet.Tables[3].Rows[i]["dDisc"]);
                                baseDiscountModel.Amen1 = Convert.ToString(dataSet.Tables[3].Rows[i]["Amen1"]);
                                baseDiscountModel.Appl1 = Convert.ToString(dataSet.Tables[3].Rows[i]["Appl1"]);
                                baseDiscountModel.Amen2 = Convert.ToString(dataSet.Tables[3].Rows[i]["Amen2"]);
                                baseDiscountModel.Appl2 = Convert.ToString(dataSet.Tables[3].Rows[i]["Appl2"]);

                                corporateDiscountModel.LeadTime.Add(baseDiscountModel);
                            }
                        }

                        #endregion

                        #region Weekend / Weekday

                        if (dataSet.Tables[4] != null && dataSet.Tables[4].Rows.Count > 0)
                        {
                            for (int i = 0; i < dataSet.Tables[4].Rows.Count; i++)
                            {
                                var baseDiscountModel = new BaseDiscountModel();
                                baseDiscountModel.dtFrom = Convert.ToString(dataSet.Tables[4].Rows[i]["dtFrom"]);
                                baseDiscountModel.dtTo = Convert.ToString(dataSet.Tables[4].Rows[i]["dtTo"]);
                                baseDiscountModel.dDisc = Convert.ToString(dataSet.Tables[4].Rows[i]["dDisc"]);
                                baseDiscountModel.Amen1 = Convert.ToString(dataSet.Tables[4].Rows[i]["Amen1"]);
                                baseDiscountModel.Appl1 = Convert.ToString(dataSet.Tables[4].Rows[i]["Appl1"]);
                                baseDiscountModel.Amen2 = Convert.ToString(dataSet.Tables[4].Rows[i]["Amen2"]);
                                baseDiscountModel.Appl2 = Convert.ToString(dataSet.Tables[4].Rows[i]["Appl2"]);

                                corporateDiscountModel.WeekendWeekday.Add(baseDiscountModel);
                            }
                        }

                        #endregion
                    }

                    return corporateDiscountModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    public class MissingBidDates
    {
        public string dtDate { get; set; }
        public string sType { get; set; }
    }

    public class CorporateDiscountModel
    {
        public CorporateDiscountModel()
        {
            BasicDiscount = new List<BaseDiscountModel>();
            LengthOfStay = new List<BaseDiscountModel>();
            MultipleRooms = new List<BaseDiscountModel>();
            LeadTime = new List<BaseDiscountModel>();
            WeekendWeekday = new List<BaseDiscountModel>();
        }

        public List<BaseDiscountModel> BasicDiscount { get; set; }
        public List<BaseDiscountModel> LengthOfStay { get; set; }
        public List<BaseDiscountModel> MultipleRooms { get; set; }
        public List<BaseDiscountModel> LeadTime { get; set; }
        public List<BaseDiscountModel> WeekendWeekday { get; set; }
    }

    public class BaseDiscountModel
    {
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
        public string dDisc { get; set; }
        public string Amen1 { get; set; }
        public string Appl1 { get; set; }
        public string Amen2 { get; set; }
        public string Appl2 { get; set; }
    }
}