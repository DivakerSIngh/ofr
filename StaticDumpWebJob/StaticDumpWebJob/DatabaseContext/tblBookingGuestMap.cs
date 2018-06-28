namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingGuestMap")]
    public partial class tblBookingGuestMap
    {
        [Key]
        public long iBookingGuestId { get; set; }

        public long iBookingDetailsID { get; set; }

        [StringLength(100)]
        public string sGuestName { get; set; }

        [StringLength(200)]
        public string sGuestEmail { get; set; }

        [StringLength(15)]
        public string sGuestMobile { get; set; }

        [StringLength(50)]
        public string sBedPreference { get; set; }

        public DateTime? dtActionDate { get; set; }

        public bool? bIsPrimary { get; set; }

        public virtual tblBookingDetailsTx tblBookingDetailsTx { get; set; }

        public virtual tblBookingGuestMap tblBookingGuestMap1 { get; set; }

        public virtual tblBookingGuestMap tblBookingGuestMap2 { get; set; }
    }
}
