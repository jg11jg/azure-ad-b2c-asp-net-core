using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using CalCrunch.Services.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestService.Controllers
{
    public class RootController
    {
        private readonly HttpRequest _req;
        private readonly TraceWriter _log;

        public RootController(HttpRequest req, TraceWriter log)
        {
            _req = req;
            _log = log;
        }

        public IActionResult Me()
        {
            var adUser = new AdUserFactory().FromHttpRequest(_req);

            return new OkObjectResult(adUser);
        }

        public IActionResult Me2()
        {
            var dic = new Dictionary<string, object>();

            /*   try
               {
                   dic.Add("HttpContext", _req.HttpContext);
               }
               catch(Exception ex)
               {
                   dic.Add("HttpContext", ex);
               }*/

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new IgnoreErrorPropertiesResolver(_log)
            };


            try
            {
                dic.Add("HttpContext.User", JsonConvert.SerializeObject(_req.HttpContext.User.Claims.First().Value, Formatting.Indented, settings));
            }
            catch (Exception ex)
            {
                dic.Add("HttpContext.User", ex);
            }

            try
            {
                dic.Add("HttpContext.User.Identity", JsonConvert.SerializeObject(_req.HttpContext.User.Identity, Formatting.Indented, settings));
            }
            catch (Exception ex)
            {
                dic.Add("HttpContext.User.Identity", ex);
            }

            try
            {
                dic.Add("HttpContext.User.Claims", JsonConvert.SerializeObject(_req.HttpContext.User.Claims.ToList(), Formatting.Indented, settings));
            }
            catch (Exception ex)
            {
                dic.Add("HttpContext.Claims", ex);
            }



            try
            {
                dic.Add("ClaimsPrincipal.Current", JsonConvert.SerializeObject(ClaimsPrincipal.Current, Formatting.Indented, settings));
            }
            catch (Exception ex)
            {
                dic.Add("ClaimsPrincipal.Current", ex);
            }

            try
            {
                dic.Add("ClaimsPrincipal.Current.Identity", JsonConvert.SerializeObject(ClaimsPrincipal.Current.Identity, Formatting.Indented, settings)); 
            }
            catch (Exception ex)
            {
                dic.Add("ClaimsPrincipal.Current.Identity", ex);
            }

            try
            {
                dic.Add("ClaimsPrincipal.Current.Identity.Claims", JsonConvert.SerializeObject(ClaimsPrincipal.Current.Claims.ToList(), Formatting.Indented, settings));
            }
            catch (Exception ex)
            {
                dic.Add("ClaimsPrincipal.Current.Identity.Claims", ex);
            }

            return new OkObjectResult(JsonConvert.SerializeObject(dic, Formatting.Indented, settings));
        }
    }
    public class IgnoreErrorPropertiesResolver : DefaultContractResolver
    {
        private readonly TraceWriter _log;

        public IgnoreErrorPropertiesResolver(TraceWriter log)
        {
            _log = log;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (new [] {
                "InputStream",
                "Filter",
                "Length",
                "Position",
                "ReadTimeout",
                "WriteTimeout",
                "LastActivityDate",
                "LastUpdatedDate",
                "Session","Claims"
            }.Contains(property.PropertyName)) {
                property.Ignored = true;
            }

            _log.Info(property.PropertyName);

            return property;
        }
    }
}
