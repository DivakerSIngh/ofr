namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBasicPropertyImgTGSpecific_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(300)]
        public string sThumbImgUrl { get; set; }

        [StringLength(50)]
        public string sThumbCategory { get; set; }

        [StringLength(50)]
        public string sThumbCaption { get; set; }

        [StringLength(300)]
        public string sLargeImgUrl { get; set; }

        [StringLength(50)]
        public string sLargeCategory { get; set; }

        [StringLength(50)]
        public string sLargeCaption { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
