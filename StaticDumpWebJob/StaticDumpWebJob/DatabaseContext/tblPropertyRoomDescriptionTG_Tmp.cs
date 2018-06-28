namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyRoomDescriptionTG_Tmp
    {
        [Key]
        public long iPropertyRoomDescriptionId { get; set; }

        public string sDescription { get; set; }

        [StringLength(200)]
        public string sRoomType { get; set; }

        [StringLength(12)]
        public string iRoomTypeId { get; set; }

        public short? iMax_Adult_Occupancy { get; set; }

        public short? iMax_Child_Occupancy { get; set; }

        public short? iMax_Infant_Occupancy { get; set; }

        public short? iMax_Guest_Occupancy { get; set; }

        [StringLength(500)]
        public string sRoomType_ImagePath { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
