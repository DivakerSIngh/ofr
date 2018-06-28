namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelRoomAmenityAdditionalM")]
    public partial class tblHotelRoomAmenityAdditionalM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelRoomAmenityAdditionalId { get; set; }

        [StringLength(50)]
        public string sRoomAmenityAdditional { get; set; }
    }
}
