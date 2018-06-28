namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelRoomAmenityBeddingM")]
    public partial class tblHotelRoomAmenityBeddingM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelRoomAmenityBeddingId { get; set; }

        [StringLength(50)]
        public string sRoomAmenityBedding { get; set; }
    }
}
