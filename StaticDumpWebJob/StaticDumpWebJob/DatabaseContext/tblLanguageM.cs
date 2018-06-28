namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLanguageM")]
    public partial class tblLanguageM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iLanguageId { get; set; }

        [StringLength(50)]
        public string sLanguage { get; set; }
    }
}
