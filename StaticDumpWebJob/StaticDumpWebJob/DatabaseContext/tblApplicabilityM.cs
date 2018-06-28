namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblApplicabilityM")]
    public partial class tblApplicabilityM
    {
        public tblApplicabilityM()
        {
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
        public short iApplicabilityId { get; set; }

        [StringLength(20)]
        public string sApplicability { get; set; }

        public short? iSortOrder { get; set; }

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
