namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMenuM")]
    public partial class tblMenuM
    {
        public tblMenuM()
        {
            tblGroupMenuMs = new HashSet<tblGroupMenuM>();
            tblMenuM1 = new HashSet<tblMenuM>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iMenuId { get; set; }

        public int? iParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string sMenuName { get; set; }

        [Required]
        [StringLength(100)]
        public string sDescription { get; set; }

        public DateTime? dtCreationDate { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        [StringLength(255)]
        public string sPath { get; set; }

        public byte? iShownInMenu { get; set; }

        public short iDispSeq { get; set; }

        public byte? iMenuType { get; set; }

        public virtual ICollection<tblGroupMenuM> tblGroupMenuMs { get; set; }

        public virtual ICollection<tblMenuM> tblMenuM1 { get; set; }

        public virtual tblMenuM tblMenuM2 { get; set; }
    }
}
