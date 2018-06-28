namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelPartnerM")]
    public partial class tblHotelPartnerM
    {
        [Key]
        public int iID { get; set; }

        [StringLength(100)]
        public string sHotelName { get; set; }

        public byte? iStarCategory { get; set; }

        [StringLength(250)]
        public string sWebsiteUrl { get; set; }

        [StringLength(200)]
        public string sAddress { get; set; }

        [StringLength(50)]
        public string sCity { get; set; }

        [StringLength(50)]
        public string sState { get; set; }

        [StringLength(50)]
        public string sCountry { get; set; }

        [StringLength(10)]
        public string sPIN { get; set; }

        [StringLength(15)]
        public string sBoardLineNumber { get; set; }

        [StringLength(50)]
        public string sFirstName { get; set; }

        [StringLength(50)]
        public string sLastName { get; set; }

        [StringLength(50)]
        public string sDesignation { get; set; }

        [StringLength(100)]
        public string sEmail { get; set; }

        [StringLength(15)]
        public string sMobile { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
