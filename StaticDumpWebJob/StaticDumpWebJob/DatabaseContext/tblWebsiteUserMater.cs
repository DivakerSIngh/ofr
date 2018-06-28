namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblWebsiteUserMater")]
    public partial class tblWebsiteUserMater
    {
        public tblWebsiteUserMater()
        {
            tblBookingAmedTxes = new HashSet<tblBookingAmedTx>();
            tblBookingTxes = new HashSet<tblBookingTx>();
            tblCustomerPointsMaps = new HashSet<tblCustomerPointsMap>();
            tblMyWishListTxes = new HashSet<tblMyWishListTx>();
            tblWebsiteUserClaims = new HashSet<tblWebsiteUserClaim>();
            tblWebsiteUserLogins = new HashSet<tblWebsiteUserLogin>();
            tblRecentViewsTxes = new HashSet<tblRecentViewsTx>();
            tblWebsiteUserRoleMasters = new HashSet<tblWebsiteUserRoleMaster>();
        }

        public long Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(5)]
        public string Title { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string DisplayName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string MartialStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AnniversaryDate { get; set; }

        [StringLength(100)]
        public string SpouseName { get; set; }

        [StringLength(200)]
        public string ProfileImageUrl { get; set; }

        [StringLength(500)]
        public string CurrentAddressLine1 { get; set; }

        [StringLength(500)]
        public string CurrentAddressLine2 { get; set; }

        [StringLength(500)]
        public string CurrentAddressLine3 { get; set; }

        public int? PinCode { get; set; }

        public int? CountryId { get; set; }

        public int? StateId { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        [StringLength(100)]
        public string ID_Type { get; set; }

        [StringLength(50)]
        public string ID_Number { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpirationDate { get; set; }

        [StringLength(200)]
        public string GovApprovedPhotoIdUrl { get; set; }

        [StringLength(200)]
        public string SmokingRoom { get; set; }

        [StringLength(100)]
        public string PreferedStarRating { get; set; }

        [StringLength(500)]
        public string SpecialRequest { get; set; }

        public bool? NewsNotifications { get; set; }

        public bool? Facilities_For_Disabled_Guest { get; set; }

        public bool? Fitness_Center { get; set; }

        public bool? Internet_Services { get; set; }

        public bool? Pets_Allowed { get; set; }

        public bool? Luggage_Storage { get; set; }

        public bool? Pool { get; set; }

        public bool? Front_Desk_24_Hour { get; set; }

        public bool? Restaurant { get; set; }

        public bool? Spa_And_Wellness_Center { get; set; }

        public bool? Bar { get; set; }

        public bool? Family_Rooms { get; set; }

        public bool? Airport_Suttle { get; set; }

        public bool? Parking { get; set; }

        public long? OFRPoints { get; set; }

        [StringLength(10)]
        public string sReferralCode { get; set; }

        public virtual ICollection<tblBookingAmedTx> tblBookingAmedTxes { get; set; }

        public virtual ICollection<tblBookingTx> tblBookingTxes { get; set; }

        public virtual ICollection<tblCustomerPointsMap> tblCustomerPointsMaps { get; set; }

        public virtual ICollection<tblMyWishListTx> tblMyWishListTxes { get; set; }

        public virtual ICollection<tblWebsiteUserClaim> tblWebsiteUserClaims { get; set; }

        public virtual ICollection<tblWebsiteUserLogin> tblWebsiteUserLogins { get; set; }

        public virtual ICollection<tblRecentViewsTx> tblRecentViewsTxes { get; set; }

        public virtual ICollection<tblWebsiteUserRoleMaster> tblWebsiteUserRoleMasters { get; set; }
    }
}
