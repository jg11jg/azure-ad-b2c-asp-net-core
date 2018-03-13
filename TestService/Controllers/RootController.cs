using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B2CGraphShell;
using Microsoft.Extensions.Options;
using TestApp;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestService.Controllers
{
    [Authorize]
    [Route("/[action]")]
    public class RootController : Controller
    {
        private readonly TenantAdminOptions _tenantAdminOptions;

        public RootController(IOptions<TenantAdminOptions> tenantAdminOptions)
        {
            _tenantAdminOptions = tenantAdminOptions.Value;
        }

        [AllowAnonymous]
        public IActionResult Echo(string textToEcho = null)
        {
            return new OkObjectResult(textToEcho ?? DateTime.UtcNow.ToString("O"));
        }

        public IActionResult Me()
        {
            return new OkObjectResult("Me");
        }

        [Route("/")]
        public IActionResult Root()
        {
            return RedirectToMe();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToMe();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var callbackUrl = Url.Action("Me", "Root", values: null, protocol: Request.Scheme);
            await HttpContext.SignOutAsync(Constants.OpenIdConnectAuthenticationScheme,
                new AuthenticationProperties(new Dictionary<string, string> { { Constants.B2CPolicy, User.FindFirst(Constants.AcrClaimType).Value } })
                {
                    RedirectUri = callbackUrl
                });

            return new EmptyResult();
        }

        private static IActionResult RedirectToMe()
        {
            return new RedirectResult("/Me");
        }

        public async Task<IActionResult> Delete()
        {
            var objectId = "";

            var client = new B2CGraphClient(_tenantAdminOptions.ClientId, _tenantAdminOptions.ClientSecret, _tenantAdminOptions.Tenant);

            if (HttpContext.User.HasClaim(x => x.Type == ""))
            {
                await client.DeleteUser(objectId);
            }

            return await SignOut();
        }
    }
}
