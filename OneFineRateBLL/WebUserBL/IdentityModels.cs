using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using OneFineRateBLL.WebUserEntities;
using OneFineRate;
using System.Collections.Generic;
using System.Security.Principal;
using OneFineRateAppUtil;
using System.Configuration;

namespace OneFineRateBLL.WebUserBL
{
    public class IdentityRole : IRole<long>
    {
        public IdentityRole()
        {
        }

        public IdentityRole(string name)
        {
            Name = name;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class BaseIdentityUser : IUser<long>
    {
        public virtual long Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTime? LockoutEndDate { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual List<string> Roles { get; set; }
        public virtual List<IdentityUserClaim> Claims { get; set; }
        public virtual List<UserLoginInfo> Logins { get; set; }

        public BaseIdentityUser()
        {
            this.Claims = new List<IdentityUserClaim>();
            this.Roles = new List<string>();
            this.Logins = new List<UserLoginInfo>();
            LockoutEnabled = true;
        }

        public BaseIdentityUser(string userName)
            : this()
        {
            this.UserName = userName;
        }
    }

    public sealed class IdentityUserLogin
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
    }

    public class IdentityUserClaim
    {
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }

    public static class IdentityPrincipalExtensions
    {
        public static string FullName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var fullNameClaim = ((ClaimsIdentity)user.Identity).FindFirst(PropertyConstant.UserFullName);
                if (fullNameClaim != null)
                {
                    return fullNameClaim.Value;
                }
                return user.Identity.GetUserName();
            }
            return string.Empty;
        }

        public static string PhoneNumber(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var phoneNumberClaim = ((ClaimsIdentity)user.Identity).FindFirst(PropertyConstant.UserPhoneNumber);
                if (phoneNumberClaim != null)
                {
                    return phoneNumberClaim.Value;
                }
            }
            return string.Empty;
        }

        public static string ReferralCode(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var referralCodeClaim = ((ClaimsIdentity)user.Identity).FindFirst(PropertyConstant.UserReferralCode);
                if (referralCodeClaim != null)
                {
                    return referralCodeClaim.Value;
                }
            }
            return string.Empty;
        }

        public static string ProfileImageUrl(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var existingUserImageUrl = BL_WebsiteUser.GetProfileImageUrl(user.Identity.GetUserId<long>());
                return existingUserImageUrl;
            }
            return string.Empty;
        }
    }
}