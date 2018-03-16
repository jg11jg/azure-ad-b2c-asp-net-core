using System.Collections.Generic;
using FunctionApp2;
using Microsoft.AspNetCore.Http;

namespace CalCrunch.Services.Identity
{
    public class AdUserFactory
    {
        public AdUser FromHttpRequest(HttpRequest req)
        {
            if (!req.IsHttps && req.Host.Host == "localhost")
            {
                return new AdUser("debug-localhost-" + System.Environment.UserName, new Dictionary<string, string>());
            }

            var userClaimsResolver = new UserClaimsResolver();

            var userClaims = userClaimsResolver.GetClaimsAsDictionary(req);

            return new AdUser(userClaims["oid"], userClaims);
        }
    }
}
