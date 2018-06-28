using OneFineRate;
using OneFineRateBLL.WebUserEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OneFineRateBLL.WebUserBL.UserRepository
{
    public class UserRepository<TUser> where TUser : WebsiteUser
    {
        public void Insert(TUser user)
        {
            using (var db = new OneFineRateEntities())
            {
                var tblUser = (tblWebsiteUserMater)OneFineRateAppUtil.clsUtils.ConvertToObject(user, new tblWebsiteUserMater());

                db.tblWebsiteUserMaters.Add(tblUser);
                db.SaveChanges();
            }
        }

        public void Delete(TUser user)
        {
            using (var db = new OneFineRateEntities())
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Id", user.Id}
                };

                db.Database.ExecuteSqlCommand(@"DELETE FROM dbo.tblWebsiteUserMater WHERE Id=@Id", parameters);
            }
        }

        public IQueryable<TUser> GetAll()
        {
            List<TUser> users = new List<TUser>();

            using (var db = new OneFineRateEntities())
            {
                var userList = db.tblWebsiteUserMaters.ToList();

                foreach (var item in userList)
                {
                    var user = (TUser)Activator.CreateInstance(typeof(TUser));
                    user.Id = item.Id;
                    user.FirstName = item.FirstName;
                    user.LastName = item.LastName;
                    user.DisplayName = item.DisplayName;
                    user.LockoutEnabled = item.LockoutEnabled;
                    user.LockoutEndDate = item.LockoutEndDateUtc;
                    user.ModifiedOn = item.ModifiedOn;
                    user.PasswordHash = item.PasswordHash;
                    user.PhoneNumber = item.PhoneNumber;
                    user.PhoneNumberConfirmed = item.PhoneNumberConfirmed;
                    user.SecurityStamp = item.SecurityStamp;
                    user.TwoFactorEnabled = item.TwoFactorEnabled;
                    user.UserName = item.UserName;
                    user.Email = item.Email;
                    user.AccessFailedCount = item.AccessFailedCount;
                    users.Add(user);
                }

            }
            return users.AsQueryable<TUser>();
        }

        public TUser GetById(long userId)
        {
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            using (var db = new OneFineRateEntities())
            {
                try
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    var tblUser = db.tblWebsiteUserMaters.Find(userId);
                    if (tblUser != null)
                    {
                        user = (TUser)OneFineRateAppUtil.clsUtils.ConvertToObject(tblUser, new WebsiteUser());
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {

                }
            }
            return user;
        }

        public TUser GetByName(string userName)
        {
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            using (var db = new OneFineRateEntities())
            {
                var tblUser = db.tblWebsiteUserMaters.FirstOrDefault(x =>
                       x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                    || x.Email.Equals(userName, StringComparison.OrdinalIgnoreCase)
                    || x.CorporateEmail.Equals(userName, StringComparison.OrdinalIgnoreCase)
                    || x.PhoneNumber.Equals(userName, StringComparison.OrdinalIgnoreCase));
                if (tblUser != null)
                {
                    user = (TUser)OneFineRateAppUtil.clsUtils.ConvertToObject(tblUser, new WebsiteUser());
                }
            }
            return user;
        }

        public TUser GetByPhone(string phone)
        {
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            using (var db = new OneFineRateEntities())
            {
                var tblUser = db.tblWebsiteUserMaters.FirstOrDefault(x => x.UserName.Equals(phone, StringComparison.OrdinalIgnoreCase));
                if (tblUser != null)
                {
                    user = (TUser)OneFineRateAppUtil.clsUtils.ConvertToObject(tblUser, new WebsiteUser());
                }
            }
            return user;
        }

        public TUser GetByEmail(string email)
        {
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            using (var db = new OneFineRateEntities())
            {
                var tblUser = db.tblWebsiteUserMaters.FirstOrDefault(x =>
                    x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                    || x.UserName.Equals(email, StringComparison.OrdinalIgnoreCase)
                    || x.PhoneNumber.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (tblUser != null)
                {
                    user = (TUser)OneFineRateAppUtil.clsUtils.ConvertToObject(tblUser, new WebsiteUser());
                }
            }
            return user;
        }

        public void Update(TUser user)
        {
            using (var db = new OneFineRateEntities())
            {
                var tblUser = (tblWebsiteUserMater)OneFineRateAppUtil.clsUtils.ConvertToObject(user, new tblWebsiteUserMater());
                db.tblWebsiteUserMaters.Attach(tblUser);
                db.Entry(tblUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
