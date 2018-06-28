namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyRoomLevelAmenityTG_Tmp
    {
        [Key]
        public long iPropertyRoomLevelAmenityId { get; set; }

        public int? iAmenityId { get; set; }

        [StringLength(200)]
        public string sDescription { get; set; }

        [StringLength(200)]
        public string sRoomType { get; set; }

        [StringLength(12)]
        public string iRoomTypeId { get; set; }

        public DateTime? dtActionDate { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }
    }
}
