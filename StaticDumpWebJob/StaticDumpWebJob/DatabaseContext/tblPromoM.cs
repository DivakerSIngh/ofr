namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPromoM")]
    public partial class tblPromoM
    {
        public tblPromoM()
        {
            tblPropertyPromotionMaps = new HashSet<tblPropertyPromotionMap>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPromoId { get; set; }

        [StringLength(100)]
        public string sPromoName { get; set; }

        public virtual ICollection<tblPropertyPromotionMap> tblPropertyPromotionMaps { get; set; }
    }
}
