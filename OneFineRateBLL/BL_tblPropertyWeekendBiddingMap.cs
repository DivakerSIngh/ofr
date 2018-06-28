using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data;
using System.Data.SqlClient;

namespace OneFineRateBLL
{
    public class BL_tblPropertyWeekendBiddingMap
    {
        //get single record
        public static etblPropertyWeekendBiddingMapForOverview GetSingleRecordById(DateTime Date, int PropId, int type,string corporateOrPublic)
        {
            etblPropertyWeekendBiddingMapForOverview OBJ = new etblPropertyWeekendBiddingMapForOverview();
            etblPropertyWeekendBiddingMap eobjPublic = new etblPropertyWeekendBiddingMap();
            etblPropertyWeekendBiddingMap eobjCorp = new etblPropertyWeekendBiddingMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                // Getting Data for Public rates
                {
                    var dbobj = (from c in db.tblPropertyWeekendBiddingMaps
                                 join d in db.tblPropertyBasicBiddingMaps on new { c.dtEffectiveDate, c.iPropId } equals new { d.dtEffectiveDate, d.iPropId }
                                 where c.dtEffectiveDate == Date && c.iPropId == PropId
                                 select new etblPropertyWeekendBiddingMap
                                {
                                    iPropId = c.iPropId,
                                    dtEffectiveDate = c.dtEffectiveDate,
                                    bIsClosedweek = d.bIsClosedweek,
                                    bCTAweek = d.bCTAweek,
                                    bCTDweek = d.bCTDweek,
                                    bWeekend = c.bWeekend,
                                    dWeekendDiscount = c.dWeekendDiscount,
                                    iAmenityId1 = c.iAmenityId1,
                                    iApplicabilityId1 = c.iApplicabilityId1,
                                    iAmenityId2 = c.iAmenityId2,
                                    iApplicabilityId2 = c.iApplicabilityId2,
                                    dtActionDate = c.dtActionDate,
                                    iActionBy = c.iActionBy
                                }).SingleOrDefault();

                    if (dbobj != null)
                        eobjPublic = (etblPropertyWeekendBiddingMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobjPublic);
                }

                // Getting Data for Corp rates
                {
                    var dbobj = (from c in db.tblPropertyWeekendBiddingMapCorps
                                 join d in db.tblPropertyBasicBiddingMapCorps on new { c.dtEffectiveDate, c.iPropId } equals new { d.dtEffectiveDate, d.iPropId }
                                 where c.dtEffectiveDate == Date && c.iPropId == PropId
                                 select new etblPropertyWeekendBiddingMap
                                 {
                                     iPropId = c.iPropId,
                                     dtEffectiveDate = c.dtEffectiveDate,
                                     bIsClosedweek = d.bIsClosedweek,
                                     bCTAweek = d.bCTAweek,
                                     bCTDweek = d.bCTDweek,
                                     bWeekend = c.bWeekend,
                                     dWeekendDiscount = c.dWeekendDiscount,
                                     iAmenityId1 = c.iAmenityId1,
                                     iApplicabilityId1 = c.iApplicabilityId1,
                                     iAmenityId2 = c.iAmenityId2,
                                     iApplicabilityId2 = c.iApplicabilityId2,
                                     dtActionDate = c.dtActionDate,
                                     iActionBy = c.iActionBy
                                 }).SingleOrDefault();

                    if (dbobj != null)
                        eobjCorp = (etblPropertyWeekendBiddingMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobjCorp);
                }
            }

            if(corporateOrPublic ==null && type == 9)
            {
                OBJ.Self = eobjPublic;
                OBJ.Other = eobjCorp;
                OBJ.IsPublic = true;
            }
            else
            {
                if (corporateOrPublic == "p")
                {
                    OBJ.Self = eobjPublic;
                    OBJ.Other = eobjCorp;
                    OBJ.IsPublic = true;
                }
                else
                {
                    OBJ.Self = eobjCorp;
                    OBJ.Other = eobjPublic;
                    OBJ.IsPublic = false;
                }
            }
           
            //if (type % 2 == 1) // Public discounts as main. Corporate as other.
            //{
            //    OBJ.Self = eobjPublic;
            //    OBJ.Other = eobjCorp;
            //    OBJ.IsPublic = true;
            //}
            //else
            //{
            //    OBJ.Self = eobjCorp;
            //    OBJ.Other = eobjPublic;
            //    OBJ.IsPublic = false;
            //}
            return OBJ;

        }
        //Update a record
        public static int UpdateRecord(etblPropertyWeekendBiddingMapForOverview eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    //OneFineRate.tblPropertyBasicBiddingMap objbasic = (OneFineRate.tblPropertyBasicBiddingMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyBasicBiddingMap());

                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iDays", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("dDiscount", typeof(Decimal));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId1", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId1", typeof(Int16));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iAmenityId2", typeof(Int32));
                    BidRange.Columns.Add(col);
                    col = new DataColumn("iApplicabilityId2", typeof(Int16));
                    BidRange.Columns.Add(col);

                    DataRow drBidRange = BidRange.NewRow();
                    drBidRange["dtEffectiveDate"] = eobj.Self.dtEffectiveDate;
                    drBidRange["iDays"] = eobj.Self.bWeekend;
                    drBidRange["dDiscount"] = eobj.Self.dWeekendDiscount == null ? (object)DBNull.Value : eobj.Self.dWeekendDiscount;
                    drBidRange["iAmenityId1"] = eobj.Self.iAmenityId1 == 0 || eobj.Self.iAmenityId1 == null ? (object)DBNull.Value : eobj.Self.iAmenityId1;
                    drBidRange["iApplicabilityId1"] = eobj.Self.iApplicabilityId1 == 0 || eobj.Self.iApplicabilityId1 == null ? (object)DBNull.Value : eobj.Self.iApplicabilityId1;
                    drBidRange["iAmenityId2"] = eobj.Self.iAmenityId2 == 0 || eobj.Self.iAmenityId2 == null ? (object)DBNull.Value : eobj.Self.iAmenityId2;
                    drBidRange["iApplicabilityId2"] = eobj.Self.iApplicabilityId2 == 0 || eobj.Self.iApplicabilityId2 == null ? (object)DBNull.Value : eobj.Self.iApplicabilityId2;
                    BidRange.Rows.Add(drBidRange);

                    SqlParameter[] MyParam = new SqlParameter[7];
                    MyParam[0] = new SqlParameter("@BidRange", BidRange);
                    MyParam[0].TypeName = "[dbo].[BidRange]";
                    MyParam[1] = new SqlParameter("@iPropId", eobj.Self.iPropId);
                    MyParam[2] = new SqlParameter("@bCloseOut", eobj.Self.bIsClosedweek);
                    MyParam[3] = new SqlParameter("@bCTA", eobj.Self.bCTAweek);
                    MyParam[4] = new SqlParameter("@bCTD", eobj.Self.bCTDweek);
                    MyParam[5] = new SqlParameter("@iActionBy", eobj.Self.iActionBy);
                    MyParam[6] = new SqlParameter("@typ", eobj.IsPublic ? 1 : 0);
                    db.Database.ExecuteSqlCommand("uspSaveWeekenBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);

                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
    }
    
}