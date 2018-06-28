namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGroupM")]
    public partial class tblGroupM
    {
        public tblGroupM()
        {
            tblGroupMenuMs = new HashSet<tblGroupMenuM>();
            tblUserGroupMs = new HashSet<tblUserGroupM>();
        }

        [Key]
        public int iGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string sGroupName { get; set; }

        [StringLength(100)]
        public string sDescription { get; set; }

        public DateTime dtCreationDate { get; set; }

        public int? iCreatedBy { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual tblUserM tblUserM1 { get; set; }

        public virtual ICollection<tblGroupMenuM> tblGroupMenuMs { get; set; }

        public virtual ICollection<tblUserGroupM> tblUserGroupMs { get; set; }
    }
}
