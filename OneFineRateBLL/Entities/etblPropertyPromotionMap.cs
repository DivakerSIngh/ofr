using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyPromotionMap
    {

        [Required]
        [DisplayName("Property ID")]
        public int iPropId { get; set; }
        [Required]
        [DisplayName("Promotion ID")]
        public int iPromoId { get; set; }

        //[Required(ErrorMessage = "Please Select Rate Plan")]
        //[DisplayName("Rate Plan")]
        public int? iRPId { get; set; }

        [Required(ErrorMessage = "Please Select Is Plus")]
        [DisplayName("Is Plus")]
        public bool bIsPlus { get; set; }

        [Required(ErrorMessage = "Please Select Is Percentage")]
        [DisplayName("Is Percentage")]
        public bool bIsPercent { get; set; }

        [Required(ErrorMessage = "Please Enter Discount Value")]
        [DisplayName("Discount Value")]
        public decimal dValue { get; set; }


        public string CancellationPolicyJSonData { get; set; }
        public List<CancellationPolicyGrid> CancellationPolicyGrid { get; set; }

        public string sRoomTypeId { get; set; }
        public string sAmenityId { get; set; }
        public string sAmenity { get; set; }
        public bool bIsSecretDeal { get; set; }
        public Int16 iHrsDays { get; set; }
        public Int16 iMinLengthStay { get; set; }
        public Int16 iMaxLengthStay { get; set; }

        [Required(ErrorMessage = "Please Select Booking Date From")]
        [DisplayName("Booking Date From")]
        public DateTime dtBookingDateFrom { get; set; }

        [Required(ErrorMessage = "Please Select Booking Date To")]
        [DisplayName("Booking Date To")]
        public DateTime dtBookingDateTo { get; set; }

        [Required(ErrorMessage = "Please Select Stay Date From")]
        [DisplayName("Stay Date From")]
        public DateTime dtStayDateFrom { get; set; }

        [Required(ErrorMessage = "Please Select Stay Date To")]
        [DisplayName("Stay Date To")]
        public DateTime dtStayDateTo { get; set; }

        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }

        public List<CheckBoxList> RoomTypeItems { get; set; }
        [Required(ErrorMessage = "Please Select Room Type")]
        public List<Int32> SelectedRoomType { get; set; }
        public List<CheckBoxList> Amenties { get; set; }
        public List<Int32> SelectedAmenityID { get; set; }

        public string sCancellationPolicy { get; set; }
        public List<CheckBoxList> CancellationPolicy { get; set; }
        public List<Int32> SelectedCancellationPolicy { get; set; }

        public string dtCancellationValidFrom { get; set; }
        public string dtCancellationValidTo { get; set; }

        public int iIsPlus { get; set; }
        public int iIsPercent { get; set; }
        public int iIsSecretDeal { get; set; }

        public string cStatus { get; set; }
        public string UserName { get; set; }
        public int iID { get; set; }

        public string sIsPercent { get; set; }
        public string sRPName { get; set; }
        public List<etblRoomTypeId> RoomTypeList { get; set; }
        public bool bIsValidation { get; set; }

        public string dtRPValidFrom { get; set; }
        public string dtRPValidTo { get; set; }

        public DateTime dtValidFrom { get; set; }
        public DateTime dtValidTo { get; set; }

        public DateTime dtRPValidityFrom { get; set; }
        public DateTime dtRPValidityTo { get; set; }

        public string dtBValidFrom { get; set; }
        public string dtBValidTo { get; set; }
        public string dtSValidFrom { get; set; }
        public string dtSValidTo { get; set; }

        public decimal dNegotiationPer { get; set; }
        public int? iCancelationChkBox { get; set; }
        public bool bParentActive { get; set; }

    }
    public  enum Promotions
    {
        BasicDeal = 1,
        MinimumStay = 2,
        EarlyBooker = 3,
        LastMinutes = 4,
        HrsPromotion = 5,
        OFRFreebies = 6

    }


    public class etblPropertyPromotionMapMain
    {

        public int iRPId { get; set; }
        public int? iPropId { get; set; }
        public string sRatePlan { get; set; }
        public string cRatePlanType { get; set; }
        public DateTime dtValidFrom { get; set; }
        public DateTime dtValidTo { get; set; }
        public bool bLinkToExistingRatePlan { get; set; }
        public int? iLinkRatePlanId { get; set; }
        public bool? bIsPlus { get; set; }
        public bool? bIsPercent { get; set; }
        public decimal? dValue { get; set; }
        public string sAmenityId { get; set; }
        public string sAmenity { get; set; }
        public Int16 iMinLengthStay { get; set; }
        public Int16 iMaxLengthStay { get; set; }
        public Int16 dHrsDays { get; set; }
        public string cHrsDays { get; set; }
        public bool bIsBefore { get; set; }
        public string sCancellationPolicy { get; set; }
        public DateTime dtCancellationValidFrom { get; set; }
        public DateTime dtCancellationValidTo { get; set; }
        public bool bIsSecretDeal { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }

        public string UserName { get; set; }

    }



    public class etblRoomTypeId
    {
        public short iRoomTypeId { get; set; }
    }


}