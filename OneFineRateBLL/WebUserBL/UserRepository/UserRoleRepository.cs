using OneFineRate;
using OneFineRateBLL.WebUserEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OneFineRateBLL.WebUserBL.UserRepository
{
    public class UserRoleRepository<TUser> where TUser : WebsiteUser
    {

        public void Insert(TUser user, string roleName)
        {
            //using (var conn = new MySqlConnection(_connectionString))
            //{
            //    var parameters = new Dictionary<string, object>
            //    {
            //        {"@RoleName", roleName}
            //    };

            //    var idObject = MySqlHelper.ExecuteScalar(conn, CommandType.Text,
            //        @"SELECT Id FROM aspnetroles WHERE Name=@RoleName", parameters);
            //    string roleId = idObject == null ? null : idObject.ToString();


            //    if (!string.IsNullOrEmpty(roleId))
            //    {

            //        using (var conn1 = new MySqlConnection(_connectionString))
            //        {
            //            var parameters1 = new Dictionary<string, object>
            //            {
            //                {"@Id", user.Id},
            //                {"@RoleId", roleId}
            //            };

            //            MySqlHelper.ExecuteNonQuery(conn1, @"Insert into aspnetuserroles(UserId,RoleId) VALUES(@Id,@RoleId)", parameters1);
            //        }
            //    }
            //}

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var roleId = db.tblWebsiteUserRoleMasters.Where(x => x.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).FirstOrDefault();
                if (roleId != 0)
                {
                    var userRoles = new IdentityRole() { 
                     
                    
                    };
                }
            }
        }

        public void Delete(TUser user, string roleName)
        {

        }

        public List<String> PopulateRoles(long userId)
        {
            var roleIds = new List<long>();
            var listRoles = new List<string>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
               // roleIds = db.tblWebsiteUserRoles.Where(x => x.Id == userId).ToList();
            }

            foreach (var roleId in roleIds)
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    listRoles.AddRange(db.tblWebsiteUserRoleMasters.Where(x => x.Id == roleId).Select(x => x.Name).ToList());
                }
            }

            return listRoles;

        }
    }
}
