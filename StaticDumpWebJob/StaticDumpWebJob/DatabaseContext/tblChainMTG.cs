namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChainMTG")]
    public partial class tblChainMTG
    {
        [Key]
        [StringLength(12)]
        public string iChainID { get; set; }

        [StringLength(50)]
        public string sChainName { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(50)]
        public string sChainGroupId { get; set; }
    }
}
