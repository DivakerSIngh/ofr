using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data.SqlClient;
using System.Data;

namespace OneFineRateBLL
{
    public class BL_tblPropertyLeadTimeBiddingMap
    {

        //Update a record
        public static int AddUpdateRecord(etblPropertyLeadTimeBiddingMap eobj, DataTable BidRange)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[7];
                    MyParam[0] = new SqlParameter("@BidRange", BidRange);
                    MyParam[0].TypeName = "[dbo].[BidRange]";
                    MyParam[1] = new SqlParameter("@iPropId", eobj.iPropId);
                    MyParam[2] = new SqlParameter("@iActionBy", eobj.iActionBy);
                    MyParam[3] = new SqlParameter("@bCloseOut", eobj.bIsClosed);
                    MyParam[4] = new SqlParameter("@bCTA", eobj.bCTA);
                    MyParam[5] = new SqlParameter("@bCTD", eobj.bCTD);
                    MyParam[6] = new SqlParameter("@typ", eobj.IsPublic ? 1 : 0);
                    db.Database.ExecuteSqlCommand("uspSaveLeadTimeBidding @BidRange, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @typ", MyParam);
                    retval = 1;

                }
                catch (Exception)
                {
                    retval = 0;
                    throw;
                }
            }
            return retval;
        }

        //Get records on behalf of date 
        public static PropertyBiddingMapForOverview GetRecords(DateTime EffectiveDate, int PropId, int typ, string corporateOrPublic)
        {
            PropertyBiddingMapForOverview OBJ = new PropertyBiddingMapForOverview();
            List<PropertyBiddingMap> eobjPublic = new List<PropertyBiddingMap>();
            List<PropertyBiddingMap> eobjCorp = new List<PropertyBiddingMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                // Getting Data for Public rates
                { 
                    var eobj = (from c in db.tblPropertyLeadTimeBiddingMaps
                                join d in db.tblPropertyBasicBiddingMaps on new { c.dtEffectiveDate, c.iPropId } equals new { d.dtEffectiveDate, d.iPropId }
                                where c.dtEffectiveDate == EffectiveDate && c.iPropId == PropId
                                select new
                                {
                                    ifrom = c.iLeadDays,
                                    bIsClosed = d.bIsClosedlead,
                                    bCTA = d.bCTAlead,
                                    bCTD = d.bCTDlead,
                                    dLeadDiscount = c.dLeadDiscount,
                                    iAmenityId1 = c.iAmenityId1,
                                    iApplicabilityId1 = c.iApplicabilityId1,
                                    iAmenityId2 = c.iAmenityId2,
                                    iApplicabilityId2 = c.iApplicabilityId2
                                }).OrderBy(t => t.ifrom).ToList();

                    short from = 0, to = 0, ApplId1 = 0, ApplId2 = 0;
                    int AmenID1 = 0, AmenID2 = 0;
                    bool Close = false, CTA = false, CTD = false;
                    decimal Disc = 0;

                    for (int i = 0; i < eobj.Count; i++)
                    {
                        if (i == 0)
                        {
                            from = (short)eobj[i].ifrom;
                            to = (short)eobj[i].ifrom;
                            AmenID1 = eobj[i].iAmenityId1 == null ? 0 : (int)eobj[i].iAmenityId1;
                            AmenID2 = eobj[i].iAmenityId2 == null ? 0 : (int)eobj[i].iAmenityId2;
                            ApplId1 = eobj[i].iApplicabilityId1 == null ? (short)0 : (short)eobj[i].iApplicabilityId1;
                            ApplId2 = eobj[i].iApplicabilityId2 == null ? (short)0 : (short)eobj[i].iApplicabilityId2;
                            Close = eobj[i].bIsClosed == null ? false : (bool)eobj[i].bIsClosed;
                            CTA = eobj[i].bCTA == null ? false : (bool)eobj[i].bCTA;
                            CTD = eobj[i].bCTD == null ? false : (bool)eobj[i].bCTD;
                            Disc = (decimal)eobj[i].dLeadDiscount;
                        }
                        else
                        {
                            if (AmenID1 == (eobj[i].iAmenityId1 == null ? 0 : (int)eobj[i].iAmenityId1) &&
                                AmenID2 == (eobj[i].iAmenityId2 == null ? 0 : (int)eobj[i].iAmenityId2) &&
                                ApplId1 == (eobj[i].iApplicabilityId1 == null ? (short)0 : (short)eobj[i].iApplicabilityId1) &&
                                ApplId2 == (eobj[i].iApplicabilityId2 == null ? (short)0 : (short)eobj[i].iApplicabilityId2) &&
                                Disc == (decimal)eobj[i].dLeadDiscount)
                            {
                                to = eobj[i].ifrom;
                            }
                            else
                            {
                                eobjPublic.Add(new PropertyBiddingMap()
                                {
                                    ifrom = from,
                                    iTo = to,
                                    bIsClosed = Close,
                                    bCTA = CTA,
                                    bCTD = CTD,
                                    dLeadDiscount = Disc,
                                    iAmenityId1 = AmenID1,
                                    iApplicabilityId1 = ApplId1,
                                    iAmenityId2 = AmenID2,
                                    iApplicabilityId2 = ApplId2
                                });

                                from = (short)eobj[i].ifrom;
                                to = (short)eobj[i].ifrom;
                                AmenID1 = eobj[i].iAmenityId1 == null ? 0 : (int)eobj[i].iAmenityId1;
                                AmenID2 = eobj[i].iAmenityId2 == null ? 0 : (int)eobj[i].iAmenityId2;
                                ApplId1 = eobj[i].iApplicabilityId1 == null ? (short)0 : (short)eobj[i].iApplicabilityId1;
                                ApplId2 = eobj[i].iApplicabilityId2 == null ? (short)0 : (short)eobj[i].iApplicabilityId2;
                                Close = eobj[i].bIsClosed == null ? false : (bool)eobj[i].bIsClosed;
                                CTA = eobj[i].bCTA == null ? false : (bool)eobj[i].bCTA;
                                CTD = eobj[i].bCTD == null ? false : (bool)eobj[i].bCTD;
                                Disc = (decimal)eobj[i].dLeadDiscount;
                            }
                        }
                    }

                    eobjPublic.Add(new PropertyBiddingMap()
                    {
                        ifrom = from,
                        iTo = to,
                        bIsClosed = Close,
                        bCTA = CTA,
                        bCTD = CTD,
                        dLeadDiscount = Disc,
                        iAmenityId1 = AmenID1,
                        iApplicabilityId1 = ApplId1,
                        iAmenityId2 = AmenID2,
                        iApplicabilityId2 = ApplId2
                    });
                }

                // Getting Data for Corp rates
                {
                    var eobj = (from c in db.tblPropertyLeadTimeBiddingMapCorps
                                join d in db.tblPropertyBasicBiddingMapCorps on new { c.dtEffectiveDate, c.iPropId } equals new { d.dtEffectiveDate, d.iPropId }
                                where c.dtEffectiveDate == EffectiveDate && c.iPropId == PropId
                                select new
                                {
                                    ifrom = c.iLeadDays,
                                    bIsClosed = d.bIsClosedlead,
                                    bCTA = d.bCTAlead,
                                    bCTD = d.bCTDlead,
                                    dLeadDiscount = c.dLeadDiscount,
                                    iAmenityId1 = c.iAmenityId1,
                                    iApplicabilityId1 = c.iApplicabilityId1,
                                    iAmenityId2 = c.iAmenityId2,
                                    iApplicabilityId2 = c.iApplicabilityId2
                                }).OrderBy(t => t.ifrom).ToList();

                    short from = 0, to = 0, ApplId1 = 0, ApplId2 = 0;
                    int AmenID1 = 0, AmenID2 = 0;
                    bool Close = false, CTA = false, CTD = false;
                    decimal Disc = 0;

                    for (int i = 0; i < eobj.Count; i++)
                    {
                        if (i == 0)
                        {
                            from = (short)eobj[i].ifrom;
                            to = (short)eobj[i].ifrom;
                            AmenID1 = eobj[i].iAmenityId1 == null ? 0 : (int)eobj[i].iAmenityId1;
                            AmenID2 = eobj[i].iAmenityId2 == null ? 0 : (int)eobj[i].iAmenityId2;
                            ApplId1 = eobj[i].iApplicabilityId1 == null ? (short)0 : (short)eobj[i].iApplicabilityId1;
                            ApplId2 = eobj[i].iApplicabilityId2 == null ? (short)0 : (short)eobj[i].iApplicabilityId2;
                            Close = eobj[i].bIsClosed == null ? false : (bool)eobj[i].bIsClosed;
                            CTA = eobj[i].bCTA == null ? false : (bool)eobj[i].bCTA;
                            CTD = eobj[i].bCTD == null ? false : (bool)eobj[i].bCTD;
                            Disc = (decimal)eobj[i].dLeadDiscount;
                        }
                        else
                        {
                            if (AmenID1 == (eobj[i].iAmenityId1 == null ? 0 : (int)eobj[i].iAmenityId1) &&
                                AmenID2 == (eobj[i].iAmenityId2 == null ? 0 : (int)eobj[i].iAmenityId2) &&
                                ApplId1 == (eobj[i].iApplicabilityId1 == null ? (short)0 : (short)eobj[i].iApplicabilityId1) &&
                                ApplId2 == (eobj[i].iApplicabilityId2 == null ? (short)0 : (short)eobj[i].iApplicabilityId2) &&
                                Disc == (decimal)eobj[i].dLeadDiscount)
                            {
                                to = eobj[i].ifrom;
                            }
                            else
                            {
                                eobjCorp.Add(new PropertyBiddingMap()
                                {
                                    ifrom = from,
                                    iTo = to,
                                    bIsClosed = Close,
                                    bCTA = CTA,
                                    bCTD = CTD,
                                    dLeadDiscount = Disc,
                                    iAmenityId1 = AmenID1,
                                    iApplicabilityId1 = ApplId1,
                                    iAmenityId2 = AmenID2,
                                    iApplicabilityId2 = ApplId2
                                });

                                from = (short)eobj[i].ifrom;
                                to = (short)eobj[i].ifrom;
                                AmenID1 = eobj[i].iAmenityId1 == null ? 0 : (int)eobj[i].iAmenityId1;
                                AmenID2 = eobj[i].iAmenityId2 == null ? 0 : (int)eobj[i].iAmenityId2;
                                ApplId1 = eobj[i].iApplicabilityId1 == null ? (short)0 : (short)eobj[i].iApplicabilityId1;
                                ApplId2 = eobj[i].iApplicabilityId2 == null ? (short)0 : (short)eobj[i].iApplicabilityId2;
                                Close = eobj[i].bIsClosed == null ? false : (bool)eobj[i].bIsClosed;
                                CTA = eobj[i].bCTA == null ? false : (bool)eobj[i].bCTA;
                                CTD = eobj[i].bCTD == null ? false : (bool)eobj[i].bCTD;
                                Disc = (decimal)eobj[i].dLeadDiscount;
                            }
                        }
                    }

                    eobjCorp.Add(new PropertyBiddingMap()
                    {
                        ifrom = from,
                        iTo = to,
                        bIsClosed = Close,
                        bCTA = CTA,
                        bCTD = CTD,
                        dLeadDiscount = Disc,
                        iAmenityId1 = AmenID1,
                        iApplicabilityId1 = ApplId1,
                        iAmenityId2 = AmenID2,
                        iApplicabilityId2 = ApplId2
                    });
                }
            }

            if (string.IsNullOrEmpty(corporateOrPublic))
            {
                if (typ % 2 == 1) // Public discounts as main. Corporate as other.
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

            else if (corporateOrPublic == null && typ == 9)
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

            return OBJ;
        }
     
    }
}