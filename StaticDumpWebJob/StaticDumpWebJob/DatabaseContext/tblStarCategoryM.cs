namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblStarCategoryM")]
    public partial class tblStarCategoryM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iStarCategoryID { get; set; }

        [StringLength(20)]
        public string sStarCategory { get; set; }
    }
}
