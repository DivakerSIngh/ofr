namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRoomFacilityDropDownM")]
    public partial class tblRoomFacilityDropDownM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iRoomFacilityId { get; set; }

        [Required]
        [StringLength(50)]
        public string sRoomFacilites { get; set; }

        public byte? iRank { get; set; }

        [StringLength(150)]
        public string sImgUrl { get; set; }
    }
}
