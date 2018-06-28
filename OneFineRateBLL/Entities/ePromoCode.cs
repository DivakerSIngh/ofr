using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL.Entities
{
    public class AgeValidationAttribute : ValidationAttribute
    {
        private int MinimumAge { get; set; }

        public AgeValidationAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var earliestDate = DateTime.Now.AddYears(-MinimumAge);
            var isValid = earliestDate >= clsUtils.ConvertddmmyyyytoDateTime(value.ToString());
            if (!isValid)
            {
                return new ValidationResult("You must be " + MinimumAge + " years old.");
            }
            return null;
        }
    }
    public class StayDateFromValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            var model = (Entities.ePromoCode)validationContext.ObjectInstance;
            //DateTime _BookingTo = clsUtils.ConvertddmmyyyytoDateTime(value.ToString());
            DateTime _BookingFrom = clsUtils.ConvertddmmyyyytoDateTime(model.BookingFrom);
            DateTime _StayFrom = clsUtils.ConvertddmmyyyytoDateTime(model.StayFrom);

            if (_BookingFrom > _StayFrom)
            {
                return new ValidationResult
                    ("Stay From Date can not be less than Booking From date.");
            }
            else
            {
                return null;//ValidationResult.Success;
            }
        }
    }
    public class StayDateToValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            var model = (Entities.ePromoCode)validationContext.ObjectInstance;
            //DateTime _BookingTo = clsUtils.ConvertddmmyyyytoDateTime(value.ToString());
            DateTime _BookingFrom = clsUtils.ConvertddmmyyyytoDateTime(model.BookingFrom);
            DateTime _StayTo = clsUtils.ConvertddmmyyyytoDateTime(model.StayTo);

            if (_BookingFrom > _StayTo)
            {
                return new ValidationResult
                    ("Stay To Date can not be less than Booking From date.");
            }
            else
            {
                return null;//ValidationResult.Success;
            }
        }
    }
    public class ePromoCode : IValidatableObject
    {
        public int iPromoCodeId { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Promo Code")]
        [Remote("ValidatePromoCode", "PromoCode", ErrorMessage = "Promo code already exists.", AdditionalFields = "InitialPromoCode")]
        public string sPromoCode { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Title")]
        public string sPromoTitle { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Description")]
        public string sPromoDescription { get; set; }
        public string cPercentageValue { get; set; }
        [DisplayName("Discount")]
        public Nullable<decimal> dValue { get; set; }
        public Nullable<int> iAmenityId { get; set; }

        [DisplayName("Booking From")]
        public DateTime dtBookingFrom { get; set; }

        [DisplayName("Booking To")]
        public DateTime dtBookingTo { get; set; }

        [DisplayName("Stay From")]
        public DateTime dtStayFrom { get; set; }

        [DisplayName("Stay To")]
        public DateTime dtStayTo { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
        public string Mode { get; set; }

        [Required(ErrorMessage = "Required")]
        public string BookingFrom { get; set; }

        [Required(ErrorMessage = "Required")]
        public string BookingTo { get; set; }
        //[StayDateFromValidation]
        [Required(ErrorMessage = "Required")]
        public string StayFrom { get; set; }
        //[StayDateToValidation]
        [Required(ErrorMessage = "Required")]
        public string StayTo { get; set; }

        public string heading { get; set; }
        public string msg { get; set; }
        public string st { get; set; }

        public string sTermCondition { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var pBooking = new[] { "BookingFrom" };
            if (clsUtils.ConvertddmmyyyytoDateTime(BookingFrom) > clsUtils.ConvertddmmyyyytoDateTime(BookingTo))
            {
                //  errormsg.Add(new ValidationResult("Booking To Date must be greater than Booking From Date", new[] { "dtBookingFrom" }));
                yield return new ValidationResult("Booking To Date must be greater than Booking From Date.", pBooking);
            }
            var pStayFrom = new[] { "StayFrom" };
            if (clsUtils.ConvertddmmyyyytoDateTime(StayFrom) > clsUtils.ConvertddmmyyyytoDateTime(StayTo))
            {
                //  errormsg.Add(new ValidationResult("Booking To Date must be greater than Booking From Date", new[] { "dtBookingFrom" }));
                yield return new ValidationResult("Stay To Date must be greater than Stay From Date.", pStayFrom);
            }
            if (clsUtils.ConvertddmmyyyytoDateTime(BookingFrom) > clsUtils.ConvertddmmyyyytoDateTime(StayFrom))
            {
                yield return new ValidationResult("Stay From Date can not be less than Booking From Date.", pStayFrom);
            }
            var pStayTo = new[] { "StayTo" };
            if (clsUtils.ConvertddmmyyyytoDateTime(BookingFrom) > clsUtils.ConvertddmmyyyytoDateTime(StayTo))
            {
                yield return new ValidationResult("Stay To Date can not be less than Booking From Date.", pStayTo);
            }
            var pDiscount = new[] { "dValue" };
            if (((dValue == null || dValue <= 0) && (iAmenityId == null || iAmenityId <= 0)) || (dValue > 0 && iAmenityId > 0))
            {
                yield return new ValidationResult("Either Discount value should be provided or any one Amenity should be provided.", pDiscount);
            }
        }
    }
    public class ePromoCodeDisp
    {
        public int iPromoCodeId { get; set; }
        public string sPromoCode { get; set; }
        public string sPromoTitle { get; set; }
        public string sPromoDescription { get; set; }
        public string dValue { get; set; }
        public string sAmenityName { get; set; }
        public Nullable<System.DateTime> dtBookingFrom { get; set; }
        public Nullable<System.DateTime> dtBookingTo { get; set; }
        public Nullable<System.DateTime> dtStayFrom { get; set; }
        public Nullable<System.DateTime> dtStayTo { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string ActionBy { get; set; }
        public int HotelCount { get; set; }
    }
}