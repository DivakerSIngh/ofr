namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAccessbilityM")]
    public partial class tblAccessbilityM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iAccessibilityId { get; set; }

        [StringLength(50)]
        public string sAccessibility { get; set; }
    }
}
