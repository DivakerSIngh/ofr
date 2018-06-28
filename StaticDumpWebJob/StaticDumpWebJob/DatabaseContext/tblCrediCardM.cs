namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCrediCardM")]
    public partial class tblCrediCardM
    {
        [Key]
        public short iCreditCardId { get; set; }

        [StringLength(100)]
        public string sCreditCard { get; set; }

        [StringLength(100)]
        public string sImg { get; set; }
    }
}
