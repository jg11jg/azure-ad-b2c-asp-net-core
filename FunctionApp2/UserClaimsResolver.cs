using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CalCrunch.Utils;
using Microsoft.AspNetCore.Http;

namespace FunctionApp2
{
    public class UserClaimsResolver
    {
        public IEnumerable<Claim> GetClaims(HttpRequest req)
        {
            if (!req.Headers.ContainsKey("X-MS-TOKEN-AAD-ID-TOKEN"))
            {
                throw new ProblemJsonException(401, CalCrunch.Utils.Shared.DocumentationRoot + "missing-X-MS-TOKEN-AAD-ID-TOKEN",
                    "Caller does not appear to be authenticated.",
                    "You do not appear to be authenticated, therefore cannot check authorization for this resource.",
                    req.Path);
            }

            var jwtToJson = new JwtParser(req.Headers["X-MS-TOKEN-AAD-ID-TOKEN"]);

            if (!jwtToJson.TryGetToken(out var token))
            {
                throw new ProblemJsonException(401, CalCrunch.Utils.Shared.DocumentationRoot + "invalid-X-MS-TOKEN-AAD-ID-TOKEN",
                    "Could not parse jwt.",
                    "The authentication token was invalid.",
                    req.Path);
            }

            return token.Claims;
        }

        public IDictionary<string, string> GetClaimsAsDictionary(HttpRequest req)
        {
            return GetClaims(req)?.ToDictionary(x => x.Type, x => x.Value);
        }
    }
}
