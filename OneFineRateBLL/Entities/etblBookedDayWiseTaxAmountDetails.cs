using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookedDayWiseTaxAmountDetails
    {
        public int iBookingID { get; set; }
        public int iBookingDetailsID { get; set; }
        public System.DateTime dtStayDay { get; set; }
        public Nullable<decimal> dAmount { get; set; }
        public Nullable<decimal> dTaxPerc { get; set; }
        public Nullable<decimal> dTaxVal { get; set; }
        public bool? bIsPromo { get; set; }
        public int iOccupancy { get; set; }
        public int RoomID { get; set; }
        public int RPID { get; set; }
    }
    public class etblBookedDayWiseTaxAmountDetailsAll
    {
        public int iBookingID { get; set; }
        public int iBookingDetailsID { get; set; }
        public System.DateTime dtStayDay { get; set; }
        public Nullable<decimal> dAmount { get; set; }
        public Nullable<decimal> dTaxPerc { get; set; }
        public Nullable<decimal> dTaxVal { get; set; }
        public int iOccupancy { get; set; }
        public int RoomID { get; set; }
        public int RPID { get; set; }
        public int iTaxId { get; set; }
        public string sTaxName { get; set; }
    }
}