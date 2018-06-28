using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OneFineRateAppUtil;
using OneFineRateBLL.WebUserBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateBLL.WebUserEntities
{
    public class WebsiteUser : BaseIdentityUser
    {

        #region ProfileBase

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MartialStatus { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string SpouseName { get; set; }
        public string ProfileImageUrl { get; set; }

        public string CurrentAddressLine1 { get; set; }
        public string CurrentAddressLine2 { get; set; }
        public string CurrentAddressLine3 { get; set; }
        public int? PinCode { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string City { get; set; }

        public string sCountryPhoneCode { get; set; }

        public long? OFRPoints { get; set; }

        public string sReferralCode { get; set; }

        public string CorporateEmail { get; set; }
        
        public bool CorporateEmailConfirmed { get; set; }

        public string CompanyName { get; set; }

        public CorporateEmailStatus CorporateCustomerStatus { get; set; }

        #endregion ProfileBase


        #region UserPreference

        public string SmokingRoom { get; set; }
        public string PreferedStarRating { get; set; }

        public string SpecialRequest { get; set; }
        public bool NewsNotifications { get; set; }

        public bool Facilities_For_Disabled_Guest { get; set; }
        public bool Fitness_Center { get; set; }
        public bool Internet_Services { get; set; }
        public bool Pets_Allowed { get; set; }
        public bool Luggage_Storage { get; set; }
        public bool Pool { get; set; }
        public bool Front_Desk_24_Hour { get; set; }
        public bool Restaurant { get; set; }
        public bool Spa_And_Wellness_Center { get; set; }
        public bool Bar { get; set; }
        public bool Family_Rooms { get; set; }
        public bool Airport_Suttle { get; set; }
        public bool Parking { get; set; }


        #endregion UserPreference


        #region GovernmentApprovedId

        public string ID_Type { get; set; }
        public string ID_Number { get; set; }
        public DateTime ExpirationDate { get; set; }

        #endregion GovernmentApprovedId


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<WebsiteUser, long> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            var userFullName = this.DisplayName ?? this.FirstName ?? this.Email;

            userIdentity.AddClaim(new Claim(PropertyConstant.UserFullName, userFullName));

            userIdentity.AddClaim(new Claim(PropertyConstant.UserReferralCode, this.sReferralCode ?? string.Empty));

            return userIdentity;
        }

        public string GovApprovedPhotoIdUrl { get; set; }
        public string GstnEntityType { get; set; }
        public string GstnNumber { get; set; }
        public string GstnEntityName { get; set; }
    }


    public class etblWebsiteGuestUserMaster
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string sCountryPhoneCode { get; set; }
        public string GstnEntityType { get; set; }
        public string GstnNumber { get; set; }
        public string GstnEntityName { get; set; }
        public Nullable<int> iStateId { get; set; }
    }
}
