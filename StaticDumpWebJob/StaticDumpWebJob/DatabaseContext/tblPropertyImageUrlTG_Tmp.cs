namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyImageUrlTG_Tmp
    {
        [Key]
        public long IPropertyImageUrlId { get; set; }

        [StringLength(200)]
        public string sContentTitle { get; set; }

        [StringLength(500)]
        public string sImageUrl { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
