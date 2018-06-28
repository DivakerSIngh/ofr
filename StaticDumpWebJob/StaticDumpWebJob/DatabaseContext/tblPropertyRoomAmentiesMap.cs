namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRoomAmentiesMap")]
    public partial class tblPropertyRoomAmentiesMap
    {
        [Key]
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

        public int? iBasicAmentiesRadio { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
