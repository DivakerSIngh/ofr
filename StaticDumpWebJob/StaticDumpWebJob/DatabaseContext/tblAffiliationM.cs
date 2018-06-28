namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAffiliationM")]
    public partial class tblAffiliationM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iAffiliationId { get; set; }

        [StringLength(50)]
        public string sAffiliation { get; set; }
    }
}
