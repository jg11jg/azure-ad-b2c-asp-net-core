using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestService.Controllers
{
    public class RootController
    {
        private readonly HttpRequest _req;

        public RootController(HttpRequest req)
        {
            _req = req;
        }

        public IActionResult Me()
        {
            var dic = new Dictionary<string, object>();

            try
            {
                dic.Add("HttpContext", _req.HttpContext);
            }
            catch(Exception ex)
            {
                dic.Add("HttpContext", ex);
            }

            try
            {
                dic.Add("HttpContext.User", _req.HttpContext.User);
            }
            catch (Exception ex)
            {
                dic.Add("HttpContext.User", ex);
            }

            try
            {
                dic.Add("ClaimsPrincipal.Current", ClaimsPrincipal.Current);
            }
            catch (Exception ex)
            {
                dic.Add("ClaimsPrincipal.Current", ex);
            }

            try
            {
                dic.Add("ClaimsPrincipal.Current.Identity", ClaimsPrincipal.Current.Identity);
            }
            catch (Exception ex)
            {
                dic.Add("ClaimsPrincipal.Current.Identity", ex);
            }

            try
            {
                dic.Add("ClaimsPrincipal.Current.Identity.Claims", ClaimsPrincipal.Current.Claims.ToList());
            }
            catch (Exception ex)
            {
                dic.Add("ClaimsPrincipal.Current.Identity.Claims", ex);
            }

            var settings = new JsonSerializerSettings()
                {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
            return new OkObjectResult(JsonConvert.SerializeObject(dic, Formatting.Indented, settings));
        }
    }
}
