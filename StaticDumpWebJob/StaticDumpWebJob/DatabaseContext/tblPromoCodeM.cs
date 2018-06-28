namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPromoCodeM")]
    public partial class tblPromoCodeM
    {
        public tblPromoCodeM()
        {
            tblPropertyPromoMaps = new HashSet<tblPropertyPromoMap>();
        }

        [Key]
        public int iPromoCodeId { get; set; }

        [StringLength(100)]
        public string sPromoCode { get; set; }

        [StringLength(100)]
        public string sPromoTitle { get; set; }

        [StringLength(200)]
        public string sPromoDescription { get; set; }

        [StringLength(1)]
        public string cPercentageValue { get; set; }

        public decimal? dValue { get; set; }

        public int? iAmenityId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingTo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayTo { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(4000)]
        public string sTermCondition { get; set; }

        public virtual tblAmenityM tblAmenityM { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual ICollection<tblPropertyPromoMap> tblPropertyPromoMaps { get; set; }
    }
}
