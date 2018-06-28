using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using OneFineRate;
using System.Linq;
using System.Data.Entity;

namespace OneFineRateBLL.WebUserBL.UserRepository
{
    public class UserLoginRepository
    {
        public void Insert(BaseIdentityUser user, UserLoginInfo login)
        {
            var tblLogin = new tblWebsiteUserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                db.tblWebsiteUserLogins.Add(tblLogin);

                db.SaveChanges();
            }
        }

        public void Delete(BaseIdentityUser user, UserLoginInfo login)
        {
            var tblLogin = new tblWebsiteUserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var entry = db.Entry(tblLogin);
                if (entry.State == EntityState.Detached)
                {
                    db.tblWebsiteUserLogins.Attach(tblLogin);
                }

                db.tblWebsiteUserLogins.Remove(tblLogin);
                db.SaveChanges();
            }
        }

        public long GetByUserLoginInfo(UserLoginInfo login)
        {
            long userId;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                userId = db.tblWebsiteUserLogins.Where(x => x.LoginProvider.Equals(login.LoginProvider) && x.ProviderKey.Equals(login.ProviderKey)).Select(x => x.UserId).FirstOrDefault();
            }
            return userId;
        }

        public List<UserLoginInfo> PopulateLogins(long userId)
        {
            var listLogins = new List<UserLoginInfo>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (var item in db.tblWebsiteUserLogins.Where(x => x.UserId == userId).ToList())
                {
                    listLogins.Add(new UserLoginInfo(item.LoginProvider, item.ProviderKey));
                }
            }
            return listLogins;
        }
    }
}
