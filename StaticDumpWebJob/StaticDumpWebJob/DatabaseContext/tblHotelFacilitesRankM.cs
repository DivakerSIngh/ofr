namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelFacilitesRankM")]
    public partial class tblHotelFacilitesRankM
    {
        [Key]
        public int iHoteFacilityId { get; set; }

        [Required]
        [StringLength(50)]
        public string sHotelFacilites { get; set; }

        public byte? iRank { get; set; }

        [StringLength(150)]
        public string sImgUrl { get; set; }
    }
}
