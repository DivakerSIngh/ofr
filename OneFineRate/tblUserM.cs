//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneFineRate
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUserM
    {
        public tblUserM()
        {
            this.tblAdditionalCommisions = new HashSet<tblAdditionalCommision>();
            this.tblAmenityMs = new HashSet<tblAmenityM>();
            this.tblBlackListedDomains = new HashSet<tblBlackListedDomain>();
            this.tblChainMs = new HashSet<tblChainM>();
            this.tblCityMs = new HashSet<tblCityM>();
            this.tblCorporateCompanyMs = new HashSet<tblCorporateCompanyM>();
            this.tblCorporateDomainMaps = new HashSet<tblCorporateDomainMap>();
            this.tblCountryMs = new HashSet<tblCountryM>();
            this.tblCurrencyMs = new HashSet<tblCurrencyM>();
            this.tblGroupMs = new HashSet<tblGroupM>();
            this.tblGroupMs1 = new HashSet<tblGroupM>();
            this.tblGroupMenuMs = new HashSet<tblGroupMenuM>();
            this.tblGroupMenuMs1 = new HashSet<tblGroupMenuM>();
            this.tblLoyalityAmenityMaps = new HashSet<tblLoyalityAmenityMap>();
            this.tblLoyalityPointsMs = new HashSet<tblLoyalityPointsM>();
            this.tblMacroAreaMs = new HashSet<tblMacroAreaM>();
            this.tblPromoCodeMs = new HashSet<tblPromoCodeM>();
            this.tblPropertyBasicBiddingMaps = new HashSet<tblPropertyBasicBiddingMap>();
            this.tblPropertyBasicBiddingMapCorps = new HashSet<tblPropertyBasicBiddingMapCorp>();
            this.tblPropertyLeadTimeBiddingMaps = new HashSet<tblPropertyLeadTimeBiddingMap>();
            this.tblPropertyLeadTimeBiddingMapCorps = new HashSet<tblPropertyLeadTimeBiddingMapCorp>();
            this.tblPropertyLOSBiddingMaps = new HashSet<tblPropertyLOSBiddingMap>();
            this.tblPropertyLOSBiddingMapCorps = new HashSet<tblPropertyLOSBiddingMapCorp>();
            this.tblPropertyMs = new HashSet<tblPropertyM>();
            this.tblPropertyParkingMaps = new HashSet<tblPropertyParkingMap>();
            this.tblPropertyPhotoMaps = new HashSet<tblPropertyPhotoMap>();
            this.tblPropertyPolicyMaps = new HashSet<tblPropertyPolicyMap>();
            this.tblPropertyPrimaryPhotoMaps = new HashSet<tblPropertyPrimaryPhotoMap>();
            this.tblPropertyPromoMaps = new HashSet<tblPropertyPromoMap>();
            this.tblPropertyPromotionMaps = new HashSet<tblPropertyPromotionMap>();
            this.tblPropertyPromotionRoomTypeMaps = new HashSet<tblPropertyPromotionRoomTypeMap>();
            this.tblPropertyRatePlanMaps = new HashSet<tblPropertyRatePlanMap>();
            this.tblPropertyRoomAmentiesMaps = new HashSet<tblPropertyRoomAmentiesMap>();
            this.tblPropertyRoomInventories = new HashSet<tblPropertyRoomInventory>();
            this.tblPropertyRoomMaps = new HashSet<tblPropertyRoomMap>();
            this.tblPropertyRoomRatePlanInventoryMaps = new HashSet<tblPropertyRoomRatePlanInventoryMap>();
            this.tblPropertyRoomsBiddingMaps = new HashSet<tblPropertyRoomsBiddingMap>();
            this.tblPropertyRoomsBiddingMapCorps = new HashSet<tblPropertyRoomsBiddingMapCorp>();
            this.tblPropertyRoomTypeRoomAmentiesMaps = new HashSet<tblPropertyRoomTypeRoomAmentiesMap>();
            this.tblPropertyTaxesMaps = new HashSet<tblPropertyTaxesMap>();
            this.tblPropertyTaxMaps = new HashSet<tblPropertyTaxMap>();
            this.tblPropertyWeekendBiddingMaps = new HashSet<tblPropertyWeekendBiddingMap>();
            this.tblPropertyWeekendBiddingMapCorps = new HashSet<tblPropertyWeekendBiddingMapCorp>();
            this.tblStateMs = new HashSet<tblStateM>();
            this.tblTaxMs = new HashSet<tblTaxM>();
            this.tblUserGroupMs = new HashSet<tblUserGroupM>();
            this.tblUserGroupMs1 = new HashSet<tblUserGroupM>();
            this.tblUserGroupMs2 = new HashSet<tblUserGroupM>();
            this.tblUserM1 = new HashSet<tblUserM>();
            this.tblVideoUrlMs = new HashSet<tblVideoUrlM>();
            this.tblUserPropertyMaps = new HashSet<tblUserPropertyMap>();
            this.tblUserPropertyMaps1 = new HashSet<tblUserPropertyMap>();
            this.tblUserPropertyMaps2 = new HashSet<tblUserPropertyMap>();
        }
    
        public int iUserId { get; set; }
        public string sUserName { get; set; }
        public string sPassword { get; set; }
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sEmail { get; set; }
        public string sContact { get; set; }
        public System.DateTime dtCreatedOn { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    
        public virtual ICollection<tblAdditionalCommision> tblAdditionalCommisions { get; set; }
        public virtual ICollection<tblAmenityM> tblAmenityMs { get; set; }
        public virtual ICollection<tblBlackListedDomain> tblBlackListedDomains { get; set; }
        public virtual ICollection<tblChainM> tblChainMs { get; set; }
        public virtual ICollection<tblCityM> tblCityMs { get; set; }
        public virtual ICollection<tblCorporateCompanyM> tblCorporateCompanyMs { get; set; }
        public virtual ICollection<tblCorporateDomainMap> tblCorporateDomainMaps { get; set; }
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
        public virtual ICollection<tblPropertyBasicBiddingMapCorp> tblPropertyBasicBiddingMapCorps { get; set; }
        public virtual ICollection<tblPropertyLeadTimeBiddingMap> tblPropertyLeadTimeBiddingMaps { get; set; }
        public virtual ICollection<tblPropertyLeadTimeBiddingMapCorp> tblPropertyLeadTimeBiddingMapCorps { get; set; }
        public virtual ICollection<tblPropertyLOSBiddingMap> tblPropertyLOSBiddingMaps { get; set; }
        public virtual ICollection<tblPropertyLOSBiddingMapCorp> tblPropertyLOSBiddingMapCorps { get; set; }
        public virtual ICollection<tblPropertyM> tblPropertyMs { get; set; }
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
        public virtual ICollection<tblPropertyRoomsBiddingMapCorp> tblPropertyRoomsBiddingMapCorps { get; set; }
        public virtual ICollection<tblPropertyRoomTypeRoomAmentiesMap> tblPropertyRoomTypeRoomAmentiesMaps { get; set; }
        public virtual ICollection<tblPropertyTaxesMap> tblPropertyTaxesMaps { get; set; }
        public virtual ICollection<tblPropertyTaxMap> tblPropertyTaxMaps { get; set; }
        public virtual ICollection<tblPropertyWeekendBiddingMap> tblPropertyWeekendBiddingMaps { get; set; }
        public virtual ICollection<tblPropertyWeekendBiddingMapCorp> tblPropertyWeekendBiddingMapCorps { get; set; }
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