namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPromoRateMap")]
    public partial class tblPropertyPromoRateMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropPromoID { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime dtStayDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        public decimal? dSingleRate { get; set; }

        public decimal? dDoubleRate { get; set; }

        public decimal? dTripleRate { get; set; }

        public decimal? dQuadrupleRate { get; set; }

        public decimal? dQuintrupleRate { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyPromotionMap tblPropertyPromotionMap { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }
    }
}
