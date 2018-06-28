namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblWebsiteGuestUserMaster")]
    public partial class tblWebsiteGuestUserMaster
    {
        public long Id { get; set; }

        [StringLength(5)]
        public string Title { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
}
