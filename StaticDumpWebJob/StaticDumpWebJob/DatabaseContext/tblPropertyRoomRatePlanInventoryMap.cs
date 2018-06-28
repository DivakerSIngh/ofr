namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRoomRatePlanInventoryMap")]
    public partial class tblPropertyRoomRatePlanInventoryMap
    {
        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime dtInventoryDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iRPId { get; set; }

        public bool bCloseOut { get; set; }

        public short? iMinLengthStay { get; set; }

        public short? iMaxLengthStay { get; set; }

        public bool bCTA { get; set; }

        public bool bCTD { get; set; }

        public decimal? dSingleRate { get; set; }

        public decimal? dDoubleRate { get; set; }

        public decimal? dTripleRate { get; set; }

        public decimal? dQuadrupleRate { get; set; }

        public decimal? dQuintrupleRate { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyRatePlanMap tblPropertyRatePlanMap { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
