using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using OneFineRateAPI.Models;
using OneFineRateAPI.Providers;
using OneFineRateAPI.Results;
using OneFineRateAppUtil;
using System.Web.Http.Description;
using OneFineRateBLL;
using OneFineRateBLL.WebUserEntities;
using OneFineRateBLL.WebUserBL;
using System.Net;

namespace OneFineRateAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        public AccountController()
        {
        }

        public AccountController(WebsiteUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        private WebsiteUserManager _userManager;

        public WebsiteUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<WebsiteUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private WebsiteUserSignInManager _signInManager;

        public WebsiteUserSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<WebsiteUserSignInManager>();
            }
            private set { _signInManager = value; }
        }


        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        // POST api/Account/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("CheckIfUserExist")]
        public IHttpActionResult CheckIfUserExist(RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.Phone) || string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Phone number or email id Null");
            }
            else
            {
                bool isPhoneExists = BL_WebsiteUser.IsPhoneNumberOrUserNameExists(model.Phone);
                bool isEmailExists = BL_WebsiteUser.IsEmailExists(model.Email);

                if (isPhoneExists && isEmailExists)
                {
                    return BadRequest("Phone number already Associated with an account." + Environment.NewLine + " Email already Associated with an account.");
                }
                else if (isPhoneExists)
                {
                    return BadRequest("Phone number already Associated with an account.");
                }
                else if (isEmailExists)
                {
                    return BadRequest("Email already Associated with an account.");
                }
                else
                {
                    return Ok();
                }
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("SendVerificationCode")]
        public IHttpActionResult SendVerificationCode(string phoneNumber)
        {
            try
            {
                var phoneVerificationCode = new Random().Next(100000, 999999);
                var encodedCode = clsUtils.Encode(phoneVerificationCode.ToString());
                //clsUtils.sendSMS(phoneNumber, string.Format("Your phone number verification code is {0}", phoneVerificationCode));
                return Ok(encodedCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("CreateUser")]
        public async Task<IHttpActionResult> CreateUser(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new WebsiteUser { FirstName = model.Name, UserName = model.Phone, PhoneNumber = model.Phone, PhoneNumberConfirmed = true, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok();
                //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
            }
            else
            {
                var resultError = GetErrorResult(result);
                return resultError;
            }
        }
       

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
