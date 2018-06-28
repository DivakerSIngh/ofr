using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using OneFineRateWeb.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Security.Claims;
using OneFineRateAppUtil;
using OneFineRateBLL.WebUserBL;
using OneFineRateBLL.WebUserEntities;
using System.Net.Http;
using Microsoft.Owin.Security.Facebook;
using System.Web.Script.Serialization;
using OneFineRateWeb.ViewModels.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Web;
using System.Net;
using Facebook;
namespace OneFineRateWeb
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<WebsiteUserManager>(WebsiteUserManager.Create);
            app.CreatePerOwinContext<WebsiteUserSignInManager>(WebsiteUserSignInManager.Create);
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = "OneFineRate_ApplicationCookie",
                //LoginPath = new PathString("/Account/Login"),
                LoginPath = new PathString("/Home/Index"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<WebsiteUserManager, WebsiteUser, long>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        // Need to add THIS line because we added the third type argument (long) above:
                            getUserIdCallback: (claim) => long.Parse(claim.GetUserId()))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");


            var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            {
                AppId = ConfigurationManager.AppSettings.Get(PropertyConstant.FacebookAppId),
                AppSecret = ConfigurationManager.AppSettings.Get(PropertyConstant.FacebookAppSecret),                
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                //UserInformationEndpoint = "https://graph.facebook.com/v2.9/me?fields=id,name,email,first_name,last_name,gender,picture.width(300).height(300)",
                Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        var client = new FacebookClient(context.AccessToken);

                        dynamic info = client.Get("me", new { fields = "id,name,email,first_name,last_name,gender,picture.width(300).height(300)" });

                        context.Identity.AddClaim(new Claim(PropertyConstant.FacebookUser, info.ToString()));

                        return Task.FromResult(true);
                    }
                }
            };

            app.UseFacebookAuthentication(facebookOptions);
           

            var googleOption = new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings[PropertyConstant.GoogleClientId],
                ClientSecret = ConfigurationManager.AppSettings[PropertyConstant.GoogleClientSecret],
                Provider = new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim(PropertyConstant.GoogleUser, context.User.ToString()));

                        return Task.FromResult(true);
                    }
                }
            };
            app.UseGoogleAuthentication(googleOption);
        }
    }


    public class FacebookBackChannelHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);
            if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                return result;

            // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
            var content = await result.Content.ReadAsStringAsync();
            var facebookOauthResponse = JsonConvert.DeserializeObject<FacebookOauthResponse>(content);

            var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
            outgoingQueryString.Add("access_token", facebookOauthResponse.access_token);
            outgoingQueryString.Add("expires_in", facebookOauthResponse.expires_in.ToString());
            outgoingQueryString.Add("token_type", facebookOauthResponse.token_type);
            var postdata = outgoingQueryString.ToString();

            var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(postdata)
            };

            return modifiedResult;
        }
    }

    public class FacebookOauthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    //public class FacebookBackChannelHandler : HttpClientHandler
    //{
    //    protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        // Replace the RequestUri so it's not malformed
    //        if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
    //        {
    //            request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
    //        }

    //        return await base.SendAsync(request, cancellationToken);
    //    }
    //}
}