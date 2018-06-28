using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_PendingChanges
    {
        public static List<ePendingChanges> GetPendingNegotiations(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int iPropId)
        {
            List<ePendingChanges> objMapping = new List<ePendingChanges>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var Negotiations = (from B in db.tblBookingTxes.Where(a => a.cBookingType == "N" && a.BookingStatus == "NC" && a.iPropId == iPropId)
                                    join BD in db.tblBookingDetailsTxes on B.iBookingId equals BD.iBookingId into AllBooking
                                    from Bookings in AllBooking
                                    join P in db.tblPropertyMs on B.iPropId equals P.iPropId
                                    join C in db.tblCurrencyMs on P.iCurrencyId equals C.iCurrencyId into CC
                                    from Curr in CC.DefaultIfEmpty()
                                    join E in db.tblExchangeRatesMs on new { a = B.sCurrencyCode, b = Curr.sCurrencyCode } equals new { a = E.sCurrencyCodeFrom, b = E.sCurrencyCodeTo } into RATES
                                    from R in RATES.DefaultIfEmpty()
                                    select new
                                     {
                                         ID = B.iBookingId,
                                         StayFrom = B.dtCheckIn,
                                         StayTo = B.dtChekOut,
                                         NegotiateRate = B.dTotalNegotiateAmount,
                                         ActualAmount = B.dTotalAmount + B.dTotalExtraBedAmount,
                                         BidRate = B.dBidAmount,
                                         Timer1 = B.dtReservationDate,
                                         CurrentTime = DateTime.Now,
                                         ActionDate = B.dtReservationDate,
                                         RatePlanRoomType = (Bookings.iPromoType == 1) ? Bookings.sRoomName + " - Promo(Basic)" :
                                                            (Bookings.iPromoType == 2) ? Bookings.sRoomName + " - Promo(Minimum Stay)" :
                                                            (Bookings.iPromoType == 3) ? Bookings.sRoomName + " - Promo(Early Booker)" :
                                                            (Bookings.iPromoType == 4) ? Bookings.sRoomName + " - Promo(Last Minute)" :
                                                            (Bookings.iPromoType == 5) ? Bookings.sRoomName + " - Promo(24 Hrs Promotions)" :
                                                            (Bookings.iPromoType == 6) ? Bookings.sRoomName + " - Promo(OFR Freebies)" :
                                                            Bookings.sRoomName + " - " + Bookings.sRPName,
                                         Attempts = B.iNegotiateAttempts,
                                         NoOfRooms = AllBooking.Count(),
                                         Symbol = Curr.sCurrencyCode == null ? "INR" : Curr.sCurrencyCode,
                                         Code = Curr.sSymbol
                                     }
                                ).Distinct().AsQueryable();

                var lstUser = Negotiations.OrderBy(a => a.ActionDate).ToList();

                TotalCount = Negotiations.Count();
                objMapping.AddRange(lstUser.Select(item => (ePendingChanges)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new ePendingChanges())));

                int counter = 0;
                for (int i = 0; i < TotalCount; i++)
                {
                    if (i == 0)
                    {
                        objMapping[i].Timer = (objMapping[i].CurrentTime - objMapping[i].Timer1).ToString(@"hh\:mm");
                        continue;
                    }
                    else
                    {
                        if (objMapping[counter].ID == objMapping[i].ID)
                        {
                            objMapping[counter].RatePlanRoomType = objMapping[counter].RatePlanRoomType + ",<br/>" + objMapping[i].RatePlanRoomType;
                            objMapping.RemoveAt(i);
                            i--;
                            TotalCount--;
                            continue;
                        }
                        else
                        {
                            counter = i;
                            objMapping[i].Timer = (objMapping[i].CurrentTime - objMapping[i].Timer1).ToString(@"hh\:mm");
                            continue;
                        }
                    }
                }

                // ALSO ADD COUNT OF NEGOTIATIONS
            }

            return objMapping;
        }

        public static string UpdateNegotiation(int user, int id, string status, int CO)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var s = db.uspUpdateNegotiationByHotel(id, status, CO, user);
                    var ss = s.ToList();
                    return ss[0].Status;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public static eDataForNegotiationActionMail DataForNegotiationActionMail(int id)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                eDataForNegotiationActionMail obj = new eDataForNegotiationActionMail();
                try
                {
                    var Negotiations = (from B in db.tblBookingTxes.Where(a => a.iBookingId == id)
                                        select new
                                        {
                                            ID = B.iBookingId,
                                            StayFrom = B.dtCheckIn,
                                            StayTo = B.dtChekOut,
                                            Name = B.sTitleOFR + " " + B.sFirstNameOFR + " " + B.sLastNameOFR,
                                            Email = B.sEmailOFR,
                                            Phone = B.sMobileOFR
                                        }
                    ).Distinct().AsQueryable();

                    var lstUser = Negotiations.ToList();
                    obj = (eDataForNegotiationActionMail)OneFineRateAppUtil.clsUtils.ConvertToObject(lstUser[0], new eDataForNegotiationActionMail());
                }
                catch (Exception ex)
                {
                    return obj;
                }

                return obj;
            }
        }
    }
}

