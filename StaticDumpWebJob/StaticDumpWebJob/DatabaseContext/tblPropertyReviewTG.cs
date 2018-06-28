namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyReviewTG")]
    public partial class tblPropertyReviewTG
    {
        [Key]
        public long iPropertyReviewId { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }

        [StringLength(100)]
        public string sAvgGuestRating { get; set; }

        public short? iOverallRating { get; set; }

        public string sComments { get; set; }

        public short? iRoomQuality { get; set; }

        public short? iServiceQuality { get; set; }

        public short? iDiningQuality { get; set; }

        public short? iCleanliness { get; set; }

        public DateTime? dtPost_Date { get; set; }

        [StringLength(200)]
        public string sConsumer_city { get; set; }

        [StringLength(200)]
        public string sConsumer_Country { get; set; }

        [StringLength(200)]
        public string sCustomer_name { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
