using Microsoft.AspNet.Identity;
using OneFineRateBLL.WebUserBL;
using OneFineRateBLL.WebUserBL.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateBLL.WebUserBL
{

    public class WebsiteUserRoleStore<TRole> : IQueryableRoleStore<TRole,long>, IRoleStore<TRole,long>
        where TRole : IdentityRole
    {
       
        private readonly RoleRepository<TRole> _roleRepository;
       
        public WebsiteUserRoleStore()
        {
            _roleRepository = new RoleRepository<TRole>();
        }

        public IQueryable<TRole> Roles
        {
            get
            {
                return _roleRepository.GetRoles();
            }
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleRepository.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            _roleRepository.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(long roleId)
        {
            var result = _roleRepository.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            var result = _roleRepository.GetRoleByName(roleName) as TRole;
            return Task.FromResult(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            _roleRepository.Update(role);

            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
            // connection is automatically disposed
        }

    }
}
