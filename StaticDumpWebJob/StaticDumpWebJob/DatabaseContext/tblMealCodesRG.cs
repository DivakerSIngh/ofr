namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMealCodesRG")]
    public partial class tblMealCodesRG
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iMealCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string sDescription { get; set; }
    }
}
