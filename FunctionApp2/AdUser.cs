using System.Collections.Generic;
using System.Linq;

namespace CalCrunch.Services.Identity
{
    public class AdUser
    {
        public readonly string UserObjectId;
        public readonly IDictionary<string, string> Claims;

        public AdUser(string userObjectId, IDictionary<string, string> claims)
        {
            UserObjectId = userObjectId;
            Claims = claims;
        }

        public IDictionary<string, string> ClaimsForDisplay()
        {
            var toDisplay = new HashSet<string>()
            {
                "displayName",
                "emails",
                "oid",
                "idp"
            };

            return Claims.Where(x => toDisplay.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
