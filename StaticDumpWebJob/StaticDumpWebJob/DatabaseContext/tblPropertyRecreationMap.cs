namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRecreationMap")]
    public partial class tblPropertyRecreationMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [StringLength(50)]
        public string sRecreationFacilityId { get; set; }

        [StringLength(50)]
        public string sLandActivityId { get; set; }

        [StringLength(50)]
        public string sGolfId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
