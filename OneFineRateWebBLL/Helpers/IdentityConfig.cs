using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using OneFineRateWebBLL.Entities;

namespace OneFineRateWebBLL.Helpers
{
    // *** PASS IN TYPE ARGUMENT TO BASE CLASS:
    public class WebsiteUserManager : UserManager<WebsiteUserMaster, int>
    {
        // *** ADD INT TYPE ARGUMENT TO CONSTRUCTOR CALL:
        public WebsiteUserManager(IUserStore<WebsiteUserMaster, int> store)
            : base(store)
        {
        }

        public static WebsiteUserManager Create(
            IdentityFactoryOptions<WebsiteUserManager> options,
            IOwinContext context)
        {
            // *** PASS CUSTOM APPLICATION USER STORE AS CONSTRUCTOR ARGUMENT:
            var manager = new WebsiteUserManager(
                new WebsiteUserStore(context.Get<WebsiteDataContext>()));

            // Configure validation logic for usernames

            // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
            manager.UserValidator = new UserValidator<WebsiteUserMaster, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. 
            // This application uses Phone and Emails as a step of receiving a 
            // code for verifying the user You can write your own provider and plug in here.

            // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<WebsiteUserMaster, int>
                {
                    MessageFormat = "Your security code is: {0}"
                });

            // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<WebsiteUserMaster, int>
                {
                    Subject = "SecurityCode",
                    BodyFormat = "Your security code is {0}"
                });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                // *** ADD INT TYPE ARGUMENT TO METHOD CALL:
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<WebsiteUserMaster, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }


    // PASS CUSTOM APPLICATION ROLE AND INT AS TYPE ARGUMENTS TO BASE:
    public class ApplicationRoleManager : RoleManager<WebsiteUserRoleMaster, int>
    {
        // PASS CUSTOM APPLICATION ROLE AND INT AS TYPE ARGUMENTS TO CONSTRUCTOR:
        public ApplicationRoleManager(IRoleStore<WebsiteUserRoleMaster, int> roleStore)
            : base(roleStore)
        {
        }

        // PASS CUSTOM APPLICATION ROLE AS TYPE ARGUMENT:
        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(
                new WebsiteUserRoleStore(context.Get<WebsiteDataContext>()));
        }
    }


    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }


    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<WebsiteDataContext>
    {
        protected override void Seed(WebsiteDataContext context)
        {
            base.Seed(context);
        }
    }


    public class WebsiteUserSignInManager : SignInManager<WebsiteUserMaster, int>
    {
        public WebsiteUserSignInManager(WebsiteUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(WebsiteUserMaster user)
        {
            return user.GenerateUserIdentityAsync((WebsiteUserManager)UserManager);
        }

        public static WebsiteUserSignInManager Create(IdentityFactoryOptions<WebsiteUserSignInManager> options, IOwinContext context)
        {
            return new WebsiteUserSignInManager(context.GetUserManager<WebsiteUserManager>(), context.Authentication);
        }
    }
}
