namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustomerPointsHistoryTx")]
    public partial class tblCustomerPointsHistoryTx
    {
        [Key]
        public long iId { get; set; }

        public long iCustomerId { get; set; }

        public int? iTotalPoints { get; set; }

        public DateTime? dtAction { get; set; }

        [StringLength(1)]
        public string cType { get; set; }
    }
}
