namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLoyalityAmenityMap")]
    public partial class tblLoyalityAmenityMap
    {
        [Key]
        public int iLoyalityAmenityId { get; set; }

        public int iPoints { get; set; }

        public int iAmenityId { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblAmenityM tblAmenityM { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
