using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyRatePlanMap
    {
        public int iRPId { get; set; }
        public int? iPropId { get; set; }
        public Int16 iRatePlanId { get; set; }
        [StringLength(50)]
        public string sRatePlan { get; set; }
        public string cRatePlanType { get; set; }
        public string dtValidFrom { get; set; }
        public string dtValidTo { get; set; }
        public bool bLinkToExistingRatePlan { get; set; }
        public int? iLinkRatePlanId { get; set; }
        public bool? bIsPlus { get; set; }
        public bool? bIsPercent { get; set; }
        public decimal? dValue { get; set; }
        public string sAmenityId { get; set; }
        public string sAmenity { get; set; }
        public Int16 iMinLengthStay { get; set; }
        public Int16 iMaxLengthStay { get; set; }

        [Required(ErrorMessage = "Please select hours")]
        public Int16 dHrsDays { get; set; }
        public string cHrsDays { get; set; }
        public bool bIsBefore { get; set; }
        public string sCancellationPolicy { get; set; }
        public bool bIsSecretDeal { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }

        public string ActionBy { get; set; }
        public string sRoomTypeId { get; set; }
        public string CancellationPolicyJSonData { get; set; }

        public int? iCancellationChkBox { get; set; }


        public List<CancellationPolicyGrid> CancellationPolicyGrid { get; set; }


        public List<CheckBoxList> RoomTypeItems { get; set; }
        [Required(ErrorMessage = "Please Select Room Type")]
        public List<Int32> SelectedRoomType { get; set; }
        public List<CheckBoxList> Amenties { get; set; }
        public List<Int32> SelectedAmenityID { get; set; }


        public List<CheckBoxList> CancellationPolicy { get; set; }
        public List<Int32> SelectedCancellationPolicy { get; set; }

        public int iIsPlus { get; set; }
        public int iIsPercent { get; set; }
        public int iIsSecretDeal { get; set; }

        public int iIsBefore { get; set; }


        public string UserName { get; set; }
        public int iID { get; set; }

        public string sIsPercent { get; set; }
        public string sRPName { get; set; }
        public List<etblRoomTypeId> RoomTypeList { get; set; }
        public bool bIsValidation { get; set; }
        public bool IsReturn { get; set; }

        public string Mode { get; set; }
        public string heading { get; set; }

        public int iUpdateRPId { get; set; }

        public string dtCancellationValidFrom { get; set; }
        public string dtCancellationValidTo { get; set; }


        public string sValidity { get; set; }
        public string sType { get; set; }
        public string sLinkage { get; set; }
        public string sRooms { get; set; }
        public string sAmenitiesMeals { get; set; }
        public string sConditions { get; set; }
        public string sCPolicy { get; set; }

    }

    public class etblPropertyRatePlanMapMain
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
        public string sRoomId { get; set; }
        public bool bParentActive { get; set; }

    }



    public class RPNames
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PropId { get; set; }
    }

    public class CancellationPolicyGrid
    {
        public List<string> CancellationPolicyId { get; set; }
        public List<string> CancellationPolicyName { get; set; }
        public string CancellationValidFrom { get; set; }
        public string CancellationValidTo { get; set; }
        public string sPolicyId { get; set; }
        public string sPolicyName { get; set; }

    }

    public class CancellationPolicyGridMain
    {
        public List<string> CancellationPolicyId { get; set; }
        public List<string> CancellationPolicyName { get; set; }
        public DateTime CancellationValidFrom { get; set; }
        public DateTime CancellationValidTo { get; set; }
        public string sPolicyId { get; set; }
        public string sPolicyName { get; set; }

    }


}