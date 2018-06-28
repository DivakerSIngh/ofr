namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAccomodationM")]
    public partial class tblAccomodationM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iAccomodationId { get; set; }

        [StringLength(50)]
        public string sAccomodation { get; set; }
    }
}
