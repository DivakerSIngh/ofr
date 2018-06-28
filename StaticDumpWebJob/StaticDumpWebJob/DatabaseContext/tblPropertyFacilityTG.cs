namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyFacilityTG")]
    public partial class tblPropertyFacilityTG
    {
        [Key]
        public long iPropertyFacilityId { get; set; }

        public int? iAmenityId { get; set; }

        [StringLength(100)]
        public string sAmenityType { get; set; }

        [StringLength(200)]
        public string sDescription { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
