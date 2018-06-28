namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelCommonServiceM")]
    public partial class tblHotelCommonServiceM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelCommonServiceId { get; set; }

        [StringLength(50)]
        public string sCommonService { get; set; }
    }
}
