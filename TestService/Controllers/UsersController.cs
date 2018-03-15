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
    [Authorize]
    public class UsersController : Controller
    {
        [Route("[controller]")]
        public IActionResult List()
        {
            return new OkObjectResult("list");
        }

        [Route("[controller]/{userId}")]
        public IActionResult List(string userId)
        {
            return new OkObjectResult(userId);
        }

    }
}