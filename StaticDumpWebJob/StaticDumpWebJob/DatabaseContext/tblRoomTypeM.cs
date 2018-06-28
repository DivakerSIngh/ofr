namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRoomTypeM")]
    public partial class tblRoomTypeM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iRoomTypeId { get; set; }

        [StringLength(50)]
        public string sRoomType { get; set; }
    }
}
