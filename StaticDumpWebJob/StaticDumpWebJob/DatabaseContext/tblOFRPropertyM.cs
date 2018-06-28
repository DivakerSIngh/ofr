namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblOFRPropertyM")]
    public partial class tblOFRPropertyM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        public DateTime? dtCreateDate { get; set; }

        public virtual tblPropertyM tblPropertyM { get; set; }
    }
}
