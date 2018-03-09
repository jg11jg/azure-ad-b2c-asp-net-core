using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestApp;

namespace TestService.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly B2CPolicies policies;

        public AccountController(IOptions<B2CPolicies> policies)
        {
            this.policies = policies.Value;
        }

        [Authorize]
        public IActionResult SignIn()
        {
            return Home();
        }

        public IActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(
                    Constants.OpenIdConnectAuthenticationScheme,
                    new AuthenticationProperties(new Dictionary<string, string> { { Constants.B2CPolicy, policies.EditProfilePolicy } })
                    {
                        RedirectUri = "/"
                    });
            }

            return Home();
        }

        public IActionResult ResetPassword()
        {
            return new ChallengeResult(
                    Constants.OpenIdConnectAuthenticationScheme,
                    new AuthenticationProperties(new Dictionary<string, string> { { Constants.B2CPolicy, policies.ResetPasswordPolicy } })
                    {
                        RedirectUri = "/"
                    });
        }

        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var callbackUrl = Url.Action("SignOutCallback", "Account", values: null, protocol: Request.Scheme);
                await HttpContext.SignOutAsync(Constants.OpenIdConnectAuthenticationScheme,
                    new AuthenticationProperties(new Dictionary<string, string> { { Constants.B2CPolicy, User.FindFirst(Constants.AcrClaimType).Value } })
                    {
                        RedirectUri = callbackUrl
                    });

                return new EmptyResult();
            }

            return Home();
        }

        public IActionResult SignOutCallback()
        {
                // Redirect to home page if the user is authenticated.
            return Home();
        }

        public IActionResult Home()
        {
            return new OkObjectResult(HttpContext.User.Identity.IsAuthenticated ? $"Hi {HttpContext.User.FindFirst("emails")} ({HttpContext.User.Identity.AuthenticationType})" : "Signed out.");
        }


    }
}