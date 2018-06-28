using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyPolicyMap
    {
        public int iPropId { get; set; }
        public string sCreditCardId { get; set; }
        public Nullable<bool> b24HrsCheckIn { get; set; }
        public string sCheckInHH { get; set; }
        public string sCheckInMM { get; set; }
        public Nullable<bool> bEarlyCheckInChargeable { get; set; }
        public Nullable<bool> b24HrsCheckout { get; set; }
        public string sCheckoutHH { get; set; }
        public string sCheckoutMM { get; set; }
        public Nullable<bool> bEarlyCheckoutChargeable { get; set; }
        public Nullable<byte> iMinCheckInAge { get; set; }
        public bool bChildrenAllowed { get; set; }
        public Nullable<byte> iComplimentaryStayAge { get; set; }
        public Nullable<byte> iExtraBedRequiredFrom { get; set; }
        public Nullable<decimal> dExtraBedCharges { get; set; }
        public bool bAlcoholAllowedOnsite { get; set; }
        public bool bAlcoholServedOnsite { get; set; }
        public bool bVisitorsAllowed { get; set; }
        public bool bPetsAllowed { get; set; }
        public bool bPartiesAllowed { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

        public List<CheckBoxList> CreditCardApprovalItems { get; set; }
        public List<Int32> SelectedCreditCardApproval { get; set; }
        public string sCheckInHHOld { get; set; }
        public string sCheckOutHHOld { get; set; }
    }
}