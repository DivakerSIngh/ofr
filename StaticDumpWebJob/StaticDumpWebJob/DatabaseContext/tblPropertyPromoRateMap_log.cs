namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyPromoRateMap_log
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

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
