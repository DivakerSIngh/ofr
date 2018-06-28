namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRoomTypeRoomAmentiesMap")]
    public partial class tblPropertyRoomTypeRoomAmentiesMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [StringLength(50)]
        public string sBasicRoomAmenities { get; set; }

        [StringLength(50)]
        public string sAdditionalRoomAmenities { get; set; }

        [StringLength(50)]
        public string sBathRoomAmenities { get; set; }

        [StringLength(50)]
        public string sBeddingRoomAmenities { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public int? iBasicAmenitiesRadio { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
