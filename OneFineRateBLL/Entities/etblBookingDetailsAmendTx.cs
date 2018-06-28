﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookingDetailsAmendTx
    {
        public long iId { get; set; }
        public long iAmendId { get; set; }
        public long iBookingDetailsID { get; set; }
        public long iBookingId { get; set; }
        public string iRoomId { get; set; }
        public string iRPId { get; set; }
        public string sRoomName { get; set; }
        public string sRPName { get; set; }
        public Nullable<short> iOccupancy { get; set; }
        public Nullable<short> iRooms { get; set; }
        public Nullable<decimal> dRoomRate { get; set; }
        public Nullable<decimal> dExtraBedRate { get; set; }
        public Nullable<decimal> dRoomDiscountedRate { get; set; }
        public Nullable<decimal> dTaxes { get; set; }
        public Nullable<decimal> dTaxesForHotel { get; set; }
        public Nullable<decimal> dBasicDiscountBid { get; set; }
        public Nullable<decimal> dLOSDiscountBid { get; set; }
        public Nullable<decimal> dLeadDiscountBid { get; set; }
        public Nullable<decimal> dWWDiscountBid { get; set; }
        public Nullable<decimal> dMRDiscountBid { get; set; }
        public string sAmenityRatePlan { get; set; }
        public Nullable<decimal> dUpgradeCharge { get; set; }
        public Nullable<short> iPromoType { get; set; }
        public Nullable<short> iAdults { get; set; }
        public Nullable<short> iChildren { get; set; }
        public string sChildrenAge { get; set; }
        public Nullable<short> iExtraBeds { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<decimal> dCancellationCharges { get; set; }
    }
}