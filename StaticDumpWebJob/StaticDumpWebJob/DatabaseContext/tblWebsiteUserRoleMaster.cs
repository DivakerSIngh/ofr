namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblWebsiteUserRoleMaster")]
    public partial class tblWebsiteUserRoleMaster
    {
        public tblWebsiteUserRoleMaster()
        {
            tblWebsiteUserMaters = new HashSet<tblWebsiteUserMater>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public virtual ICollection<tblWebsiteUserMater> tblWebsiteUserMaters { get; set; }
    }
}
