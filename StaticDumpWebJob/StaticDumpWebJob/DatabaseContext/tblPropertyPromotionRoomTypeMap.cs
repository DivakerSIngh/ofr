namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPromotionRoomTypeMap")]
    public partial class tblPropertyPromotionRoomTypeMap
    {
        [Key]
        public int iID { get; set; }

        public int iPropPromoID { get; set; }

        public long? iRoomId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyPromotionMap tblPropertyPromotionMap { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
