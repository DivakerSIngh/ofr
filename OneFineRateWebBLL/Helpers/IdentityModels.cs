using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using OneFineRateWebBLL.Entities;

namespace OneFineRateWebBLL.Helpers
{

    public class WebsiteUserLogin : IdentityUserLogin<int> { }
    public class WebsiteUserClaim : IdentityUserClaim<int> { }
    public class WebsiteUserRole : IdentityUserRole<int> { }
    public class WebsiteUserRoleMaster : IdentityRole<int, WebsiteUserRole>, IRole<int>
    {
        public string Description { get; set; }

        public WebsiteUserRoleMaster() : base() { }
        public WebsiteUserRoleMaster(string name)
            : this()
        {
            this.Name = name;
        }

        public WebsiteUserRoleMaster(string name, string description)
            : this(name)
        {
            this.Description = description;
        }
    }


    public class WebsiteDataContext
        : IdentityDbContext<WebsiteUserMaster, WebsiteUserRoleMaster, int,
        WebsiteUserLogin, WebsiteUserRole, WebsiteUserClaim>
    {
        public WebsiteDataContext()
            : base("name=OneFineRateEntitiesTest")
        {
        }

        static WebsiteDataContext()
        {
            Database.SetInitializer<WebsiteDataContext>(new ApplicationDbInitializer());
        }

        public static WebsiteDataContext Create()
        {
            return new WebsiteDataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebsiteUserMaster>().ToTable("tblWebsiteUserMater");

            modelBuilder.Entity<WebsiteUserRoleMaster>().ToTable("tblWebsiteUserRoleMaster");

            modelBuilder.Entity<WebsiteUserRole>().ToTable("tblWebsiteUserRoles");

            modelBuilder.Entity<WebsiteUserClaim>().ToTable("tblWebsiteUserClaims");

            modelBuilder.Entity<WebsiteUserLogin>().ToTable("tblWebsiteUserLogins");

        }
    }


    public class WebsiteUserStore :
    UserStore<WebsiteUserMaster, WebsiteUserRoleMaster, int,
    WebsiteUserLogin, WebsiteUserRole, WebsiteUserClaim>, IUserStore<WebsiteUserMaster, int>, IDisposable
    {
        public WebsiteUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public WebsiteUserStore(DbContext context)
            : base(context)
        {
        }
    }


    public class WebsiteUserRoleStore
    : RoleStore<WebsiteUserRoleMaster, int, WebsiteUserRole>,
    IQueryableRoleStore<WebsiteUserRoleMaster, int>,
    IRoleStore<WebsiteUserRoleMaster, int>, IDisposable
    {
        public WebsiteUserRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public WebsiteUserRoleStore(DbContext context)
            : base(context)
        {
        }
    }

}