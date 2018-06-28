namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyRoomRatePlanInventoryMap_log
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

        [Key]
        [Column(Order = 4)]
        public bool bCloseOut { get; set; }

        public short? iMinLengthStay { get; set; }

        public short? iMaxLengthStay { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool bCTA { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool bCTD { get; set; }

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
        [Column(Order = 7)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
