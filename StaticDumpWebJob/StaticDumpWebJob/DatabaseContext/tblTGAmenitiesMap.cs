namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblTGAmenitiesMap")]
    public partial class tblTGAmenitiesMap
    {
        [Key]
        [StringLength(15)]
        public string iVendorId { get; set; }

        public bool? H_Int { get; set; }

        public bool? H_Park { get; set; }

        public bool? H_Smoke { get; set; }

        public bool? H_Disable { get; set; }

        public bool? H_Fitness { get; set; }

        public bool? H_Family { get; set; }

        public bool? H_Restaurant { get; set; }

        public bool? H_RoomServ { get; set; }

        public bool? H_Transfer { get; set; }

        public bool? H_Pets { get; set; }

        public bool? H_Spa { get; set; }

        public bool? H_OutDoorPool { get; set; }

        public bool? H_InDoorPool { get; set; }

        public bool? H_ChildCare { get; set; }

        public bool? H_KidsClub { get; set; }

        public bool? H_Laundary { get; set; }

        public bool? H_AC { get; set; }

        public bool? R_AC { get; set; }

        public bool? R_Tub { get; set; }

        public bool? R_TV { get; set; }

        public bool? R_Sound { get; set; }

        public bool? R_View { get; set; }

        public bool? R_Pool { get; set; }

        public bool? R_Int { get; set; }

        public bool? R_RoomServ { get; set; }
    }
}
