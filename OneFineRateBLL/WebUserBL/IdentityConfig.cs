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
using OneFineRateBLL.WebUserEntities;
using OneFineRate;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.Text;
using FutureSoft.Util;
using OneFineRateAppUtil;
using OneFineRateBLL.WebUserBL;

namespace OneFineRateBLL.WebUserBL
{

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.

            MailComponent.SendEmail(message.Subject, "", "", "Link to reset your password for " + clsUtils.ApplicationName() + "", message.Body, null, null, true,null,null);

            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.

            clsUtils.sendSMS(message.Destination, message.Body);
            //message.Destination = Keys.ToPhone; //your number here
            //var twilio = new TwilioRestClient(Keys.TwilioSid, Keys.TwilioToken);
            //var result = twilio.SendMessage(Keys.FromPhone, message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class WebsiteUserManager : UserManager<WebsiteUser, long>
    {
        public WebsiteUserManager(IUserStore<WebsiteUser, long> store)
            : base(store)
        {
        }

        public static WebsiteUser FindByPhone(string phone)
        {
            var userStore = new WebsiteUserStore<WebsiteUser>();
            return userStore.FindByPhoneAsync(phone);
        }

        public static WebsiteUserManager Create(IdentityFactoryOptions<WebsiteUserManager> options, IOwinContext context)
        {
            //var manager = new WebsiteUserManager(new UserStore<WebsiteUser>(context.Get<ApplicationDbContext>()));
            var userStore = new WebsiteUserStore<WebsiteUser>();
            var manager = new WebsiteUserManager(userStore);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<WebsiteUser, long>(manager)
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
            manager.DefaultAccountLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<WebsiteUser, long>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<WebsiteUser, long>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<WebsiteUser, long>(dataProtectionProvider.Create("OnefineRate User Identity"))
                {
                    TokenLifespan = System.TimeSpan.FromHours(24)
                    //TokenLifespan = System.TimeSpan.FromMinutes(1)
                };

            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class WebsiteUserSignInManager : SignInManager<WebsiteUser, long>
    {
        public WebsiteUserSignInManager(WebsiteUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(WebsiteUser user)
        {
            return user.GenerateUserIdentityAsync((WebsiteUserManager)UserManager);
        }

        public static WebsiteUserSignInManager Create(IdentityFactoryOptions<WebsiteUserSignInManager> options, IOwinContext context)
        {
            return new WebsiteUserSignInManager(context.GetUserManager<WebsiteUserManager>(), context.Authentication);
        }
    }

    public class WebsiteUserRoleManager : RoleManager<IdentityRole, long>
    {
        public WebsiteUserRoleManager(IRoleStore<IdentityRole, long> roleStore)
            : base(roleStore)
        {
        }

        // PASS CUSTOM APPLICATION ROLE AS TYPE ARGUMENT:
        public static WebsiteUserRoleManager Create(
            IdentityFactoryOptions<WebsiteUserRoleManager> options, IOwinContext context)
        {
            return new WebsiteUserRoleManager(new WebsiteUserRoleStore<IdentityRole>());
        }
    }

}
