namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAmenityM")]
    public partial class tblAmenityM
    {
        public tblAmenityM()
        {
            tblLoyalityAmenityMaps = new HashSet<tblLoyalityAmenityMap>();
            tblPromoCodeMs = new HashSet<tblPromoCodeM>();
            tblPropertyWeekendBiddingMaps = new HashSet<tblPropertyWeekendBiddingMap>();
            tblPropertyWeekendBiddingMaps1 = new HashSet<tblPropertyWeekendBiddingMap>();
            tblPropertyLeadTimeBiddingMaps = new HashSet<tblPropertyLeadTimeBiddingMap>();
            tblPropertyLeadTimeBiddingMaps1 = new HashSet<tblPropertyLeadTimeBiddingMap>();
            tblPropertyLOSBiddingMaps = new HashSet<tblPropertyLOSBiddingMap>();
            tblPropertyLOSBiddingMaps1 = new HashSet<tblPropertyLOSBiddingMap>();
            tblPropertyRoomsBiddingMaps = new HashSet<tblPropertyRoomsBiddingMap>();
            tblPropertyRoomsBiddingMaps1 = new HashSet<tblPropertyRoomsBiddingMap>();
        }

        [Key]
        public int iAmenityId { get; set; }

        [StringLength(100)]
        public string sAmenityName { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual ICollection<tblLoyalityAmenityMap> tblLoyalityAmenityMaps { get; set; }

        public virtual ICollection<tblPromoCodeM> tblPromoCodeMs { get; set; }

        public virtual ICollection<tblPropertyWeekendBiddingMap> tblPropertyWeekendBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyWeekendBiddingMap> tblPropertyWeekendBiddingMaps1 { get; set; }

        public virtual ICollection<tblPropertyLeadTimeBiddingMap> tblPropertyLeadTimeBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyLeadTimeBiddingMap> tblPropertyLeadTimeBiddingMaps1 { get; set; }

        public virtual ICollection<tblPropertyLOSBiddingMap> tblPropertyLOSBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyLOSBiddingMap> tblPropertyLOSBiddingMaps1 { get; set; }

        public virtual ICollection<tblPropertyRoomsBiddingMap> tblPropertyRoomsBiddingMaps { get; set; }

        public virtual ICollection<tblPropertyRoomsBiddingMap> tblPropertyRoomsBiddingMaps1 { get; set; }
    }
}
