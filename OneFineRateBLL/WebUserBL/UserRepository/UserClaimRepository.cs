using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using OneFineRateBLL.WebUserEntities;
using OneFineRate;
using System.Linq;

namespace OneFineRateBLL.WebUserBL.UserRepository
{
    public class UserClaimRepository<TUser> where TUser : WebsiteUser
    {

        public void Insert(TUser user, Claim claim)
        {

            var tblClaim = new tblWebsiteUserClaim() {             
                ClaimType= claim.Type,
                ClaimValue= claim.Value,
                UserId= user.Id
            };

            using(OneFineRateEntities db= new OneFineRateEntities ())
            {
                db.tblWebsiteUserClaims.Add(tblClaim);
                db.SaveChanges();
            }
        }

        public void Delete(TUser user, Claim claim)
        {
            var tblClaim = new tblWebsiteUserClaim()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                UserId = user.Id
            };

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                db.tblWebsiteUserClaims.Add(tblClaim);
                db.SaveChanges();
            }
        }

        public List<IdentityUserClaim> PopulateClaims(long userId)
        {
            var claims = new List<IdentityUserClaim>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (var item in db.tblWebsiteUserClaims.Where(x=>x.UserId==userId).ToList())
                {
                    claims.Add(new IdentityUserClaim() { ClaimType = item.ClaimType, ClaimValue = item.ClaimValue });
                }
            }

            return claims;
        }
    }
}
