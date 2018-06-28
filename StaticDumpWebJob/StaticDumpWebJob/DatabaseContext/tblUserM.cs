namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblUserM")]
    public partial class tblUserM
    {
        public tblUserM()
        {
            tblAdditionalCommisions = new HashSet<tblAdditionalCommision>();
            tblAmenityMs = new HashSet<tblAmenityM>();
            tblChainMs = new HashSet<tblChainM>();
            tblCityMs = new HashSet<tblCityM>();
            tblCountryMs = new HashSet<tblCountryM>();
            tblCurrencyMs = new HashSet<tblCurrencyM>();
            tblGroupMs = new HashSet<tblGroupM>();
            tblGroupMs1 = new HashSet<tblGroupM>();
            tblGroupMenuMs = new HashSet<tblGroupMenuM>();
            tblGroupMenuMs1 = new HashSet<tblGroupMenuM>();
            tblLoyalityAmenityMaps = new HashSet<tblLoyalityAmenityMap>();
            tblLoyalityPointsMs = new HashSet<tblLoyalityPointsM>();
            tblMacroAreaMs = new HashSet<tblMacroAreaM>();
            tblPromoCodeMs = new HashSet<tblPromoCodeM>();
            tblPropertyBasicBiddingMaps = new HashSet<tblPropertyBasicBiddingMap>();
            tblPropertyLeadTimeBiddingMaps = new HashSet<tblPropertyLeadTimeBiddingMap>();
            tblPropertyLOSBiddingMaps = new HashSet<tblPropertyLOSBiddingMap>();
            tblPropertyParkingMaps = new HashSet<tblPropertyParkingMap>();
            tblPropertyPhotoMaps = new HashSet<tblPropertyPhotoMap>();
            tblPropertyPolicyMaps = new HashSet<tblPropertyPolicyMap>();
            tblPropertyPrimaryPhotoMaps = new HashSet<tblPropertyPrimaryPhotoMap>();
            tblPropertyPromoMaps = new HashSet<tblPropertyPromoMap>();
            tblPropertyPromotionMaps = new HashSet<tblPropertyPromotionMap>();
            tblPropertyPromotionRoomTypeMaps = new HashSet<tblPropertyPromotionRoomTypeMap>();
            tblPropertyRatePlanMaps = new HashSet<tblPropertyRatePlanMap>();
            tblPropertyRoomAmentiesMaps = new HashSet<tblPropertyRoomAmentiesMap>();
            tblPropertyRoomInventories = new HashSet<tblPropertyRoomInventory>();
            tblPropertyRoomMaps = new HashSet<tblPropertyRoomMap>();
            tblPropertyRoomRatePlanInventoryMaps = new HashSet<tblPropertyRoomRatePlanInventoryMap>();
            tblPropertyRoomsBiddingMaps = new HashSet<tblPropertyRoomsBiddingMap>();
            tblPropertyRoomTypeRoomAmentiesMaps = new HashSet<tblPropertyRoomTypeRoomAmentiesMap>();
            tblPropertyTaxesMaps = new HashSet<tblPropertyTaxesMap>();
            tblPropertyTaxMaps = new HashSet<tblPropertyTaxMap>();
            tblPropertyWeekendBiddingMaps = new HashSet<tblPropertyWeekendBiddingMap>();
            tblStateMs = new HashSet<tblStateM>();
            tblTaxMs = new HashSet<tblTaxM>();
            tblUserGroupMs = new HashSet<tblUserGroupM>();
            tblUserGroupMs1 = new HashSet<tblUserGroupM>();
            tblUserGroupMs2 = new HashSet<tblUserGroupM>();
            tblUserM1 = new HashSet<tblUserM>();
            tblVideoUrlMs = new HashSet<tblVideoUrlM>();
            tblUserPropertyMaps = new HashSet<tblUserPropertyMap>();
            tblUserPropertyMaps1 = new HashSet<tblUserPropertyMap>();
            tblUserPropertyMaps2 = new HashSet<tblUserPropertyMap>();
        }

        [Key]
        public int iUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string sUserName { get; set; }

        [Required]
        [StringLength(50)]
        public string sPassword { get; set; }

        [StringLength(50)]
        public string sFirstName { get; set; }

        [StringLength(50)]
        public string sLastName { get; set; }

        [StringLength(200)]
        public string sEmail { get; set; }

        [StringLength(15)]
        public string sContact { get; set; }

        public DateTime dtCreatedOn { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual ICollection<tblAdditionalCommision> tblAdditionalCommisions { get; set; }

        public virtual ICollection<tblAmenityM> tblAmenityMs { get; set; }

        public virtual ICollection<tblChainM> tblChainMs { get; set; }

        public virtual ICollection<tblCityM> tblCityMs { get; set; }

        public virtual ICollection<tblCountryM> tblCountryMs { get; set; }

        public virtual ICollection<tblCurrencyM> tblCurrencyMs { get; set; }

        public virtual ICollection<tblGroupM> tblGroupMs { get; set; }

        public virtual ICollection<tblGroupM> tblGroupMs1 { get; set; }

        public virtual ICollection<tblGroupMenuM> tblGroupMenuMs { get; set; }

        public virtual ICollection<tblGroupMenuM> tblGroupMenuMs1 { get; set; }

        public virtual ICollection<tblLoyalityAmenityMap> tblLoyalityAmenityMaps { get; set; }

        public virtual ICollection<tblLoyalityPointsM> tblLoyalityPointsMs { get; set; }

        public virtual ICollection<tblMacroAreaM> tblMacroAreaMs { get; set; }

        public virtual ICollection<tblPromoCodeM> tblPromoCodeMs { get; set; }

        public virtual ICollection<tblPropertyBasicBiddingMap> tblPropertyBasicBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyLeadTimeBiddingMap> tblPropertyLeadTimeBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyLOSBiddingMap> tblPropertyLOSBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyParkingMap> tblPropertyParkingMaps { get; set; }

        public virtual ICollection<tblPropertyPhotoMap> tblPropertyPhotoMaps { get; set; }

        public virtual ICollection<tblPropertyPolicyMap> tblPropertyPolicyMaps { get; set; }

        public virtual ICollection<tblPropertyPrimaryPhotoMap> tblPropertyPrimaryPhotoMaps { get; set; }

        public virtual ICollection<tblPropertyPromoMap> tblPropertyPromoMaps { get; set; }

        public virtual ICollection<tblPropertyPromotionMap> tblPropertyPromotionMaps { get; set; }

        public virtual ICollection<tblPropertyPromotionRoomTypeMap> tblPropertyPromotionRoomTypeMaps { get; set; }

        public virtual ICollection<tblPropertyRatePlanMap> tblPropertyRatePlanMaps { get; set; }

        public virtual ICollection<tblPropertyRoomAmentiesMap> tblPropertyRoomAmentiesMaps { get; set; }

        public virtual ICollection<tblPropertyRoomInventory> tblPropertyRoomInventories { get; set; }

        public virtual ICollection<tblPropertyRoomMap> tblPropertyRoomMaps { get; set; }

        public virtual ICollection<tblPropertyRoomRatePlanInventoryMap> tblPropertyRoomRatePlanInventoryMaps { get; set; }

        public virtual ICollection<tblPropertyRoomsBiddingMap> tblPropertyRoomsBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyRoomTypeRoomAmentiesMap> tblPropertyRoomTypeRoomAmentiesMaps { get; set; }

        public virtual ICollection<tblPropertyTaxesMap> tblPropertyTaxesMaps { get; set; }

        public virtual ICollection<tblPropertyTaxMap> tblPropertyTaxMaps { get; set; }

        public virtual ICollection<tblPropertyWeekendBiddingMap> tblPropertyWeekendBiddingMaps { get; set; }

        public virtual ICollection<tblStateM> tblStateMs { get; set; }

        public virtual ICollection<tblTaxM> tblTaxMs { get; set; }

        public virtual ICollection<tblUserGroupM> tblUserGroupMs { get; set; }

        public virtual ICollection<tblUserGroupM> tblUserGroupMs1 { get; set; }

        public virtual ICollection<tblUserGroupM> tblUserGroupMs2 { get; set; }

        public virtual ICollection<tblUserM> tblUserM1 { get; set; }

        public virtual tblUserM tblUserM2 { get; set; }

        public virtual ICollection<tblVideoUrlM> tblVideoUrlMs { get; set; }

        public virtual ICollection<tblUserPropertyMap> tblUserPropertyMaps { get; set; }

        public virtual ICollection<tblUserPropertyMap> tblUserPropertyMaps1 { get; set; }

        public virtual ICollection<tblUserPropertyMap> tblUserPropertyMaps2 { get; set; }
    }
}
