using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblNegotiationNotification
    {
        public int RecId { get; set; }
        public int iPropId { get; set; }
        public string sValidity { get; set; }
        public string sType { get; set; }
        public string sLinkage { get; set; }
        public string sRatePlan { get; set; }
        public string cStatus { get; set; }
        public string sRooms { get; set; }
        public string sAmenitiesMeals  { get; set; }

        public decimal? dNegotiationPer { get; set; }
    
    }

    public class NegotiationEmailTempleteModel
    {
        public string CallbackUrl { get; set; }

        public string Notification_Link { get; set; } 
      
        public string Status { get; set; }
        public bool IsRevenueManagerFormat { get; set; }
        public bool IsHotelFormat { get; set; }
        public bool IsNegotiationAcceptedBySystem { get; set; }
        public eBookingModify BookingModify { get; set; }

    }
   

    //TO DO
    public class BalancePaymentModel : NegotiationForHotelPendingModel
    {

        public List<etblBookingDetailsTx> BookingDetailList { get; set; }
        public List<TaxesDateRoomRateWise> lstTaxesDateWise_OfferReview { get; set; }
        public List<eBookingPolicies> BookingPoliciesList { get; set; }

        public List<eComplementaryRoomFacilities> lstroomfac { get; set; }
        public List<eComplementaryViews> lstViews { get; set; }
        public List<eComplementaryFacility> lstfacilities { get; set; }
        public List<eComplementaryNegoRoomData> lstRoomDataCurrent { get; set; }
        public List<eComplementaryNegoRoomData> lstRoomData { get; set; }
        public int iSelected { get; set; }
        public long iLinkedBookId { get; set; }
        public decimal? dGSTOnServiceCharge { get; set; }
        public decimal? dOfrServiceCharge { get; set; }
        public DateTime dtCheckIn { get; set; }
        public DateTime dtCheckOut { get; set; }
        public string dGSTServiceType { get; set; }
        public string dGSTValue { get; set; }
        #region To show Negotiation

        //public decimal GuestNegotiatedRate { get; set; }
        //public decimal GuestNegotiatedRateTax { get; set; }

        //public decimal CounterOfferRate { get; set; }
        //public decimal CounterOfferRateTax { get; set; }

        //public decimal AdvanceNegotiateAmount { get; set; }



        #endregion
    }
}