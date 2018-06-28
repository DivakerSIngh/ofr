using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OneFineRateWebBLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateWebBLL.Entities
{
    public class WebsiteUserMaster : IdentityUser<int, WebsiteUserLogin, WebsiteUserRole, WebsiteUserClaim>, IUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string ExternalLoginProfilePictureUrl { get; set; }

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager<WebsiteUserMaster, int> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class WebsiteUserFacebookLoginDetail
    {

    }

    public class WebsiteUserGoogleLoginDetail
    {
    }
}
