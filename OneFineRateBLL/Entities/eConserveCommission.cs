using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eConserveCommission : IValidatableObject
    {
        public int iCCId { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Commission")]
        public Nullable<decimal> dCommission { get; set; }
        [DisplayName("From")]
        public Nullable<System.DateTime> dtFrom { get; set; }
        [DisplayName("To")]
        public Nullable<System.DateTime> dtTo { get; set; }
        public bool bDisplayRateComm { get; set; }
        public bool bCOComm { get; set; }
        public Nullable<decimal> dCounterTrigger { get; set; }
        public bool bBidComm { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string cStatus { get; set; }

        //[StayDateFromValidation]
        [Required(ErrorMessage = "Required")]
        public string StayFrom { get; set; }
        //[StayDateToValidation]
        [Required(ErrorMessage = "Required")]
        public string StayTo { get; set; }

        public string heading { get; set; }
        public string msg { get; set; }
        public string st { get; set; }
        public string Mode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {           
            var pStayFrom = new[] { "StayFrom" };
            if (clsUtils.ConvertddmmyyyytoDateTime(StayFrom) > clsUtils.ConvertddmmyyyytoDateTime(StayTo))
            {
                //  errormsg.Add(new ValidationResult("Booking To Date must be greater than Booking From Date", new[] { "dtBookingFrom" }));
                yield return new ValidationResult("Stay To Date must be greater than Stay From Date.", pStayFrom);
            }
            var pDiscount = new[] { "dCounterTrigger" };
            if ((bCOComm == true) && dCounterTrigger == null )
            {
                yield return new ValidationResult("Please provide Counter Offer trigger value.", pDiscount);
            }
            if (dCounterTrigger < 0 || dCounterTrigger >= 100)
            {
                yield return new ValidationResult("Counter Offer trigger value should be at least 0 and less than 100.", pDiscount);
            }
            var Disc = new[] { "dCommission" };
            if (dCommission < 0 || dCommission >= 100)
            {
                yield return new ValidationResult("Commission value should be at least 0 and less than 100.", pDiscount);
            }
        }
    }
    public class eConserveCommissionDisp
    {
        public int iCCId { get; set; }
        public decimal dCommission { get; set; }
        public Nullable<System.DateTime> dtFrom { get; set; }
        public Nullable<System.DateTime> dtTo { get; set; }
        public string bDisplayRateComm { get; set; }
        public string bCOComm { get; set; }
        public Nullable<decimal> dCounterTrigger { get; set; }
        public string bBidComm { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string ActionBy { get; set; }
        public string cStatus { get; set; }
        public int HotelCount { get; set; }
    }
}