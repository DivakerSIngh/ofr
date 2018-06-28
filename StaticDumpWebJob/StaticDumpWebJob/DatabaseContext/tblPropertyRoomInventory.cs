namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRoomInventory")]
    public partial class tblPropertyRoomInventory
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

        public short? iInventory { get; set; }

        public short? iSoldInventory { get; set; }

        public short? iAvailableInventory { get; set; }

        public bool bStopSell { get; set; }

        public short? iCutOff { get; set; }

        public short? iBlockedInventory { get; set; }

        public DateTime? dtInventoryUpdateDate { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyM tblPropertyM { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
