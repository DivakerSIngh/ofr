namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyParkingMap")]
    public partial class tblPropertyParkingMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string sCarName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string cAirportRail { get; set; }

        public bool? bIsFree { get; set; }

        public decimal? dOnewayCharge { get; set; }

        public decimal? dTwowayCharge { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
