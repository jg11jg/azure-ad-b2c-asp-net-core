//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CalCrunch.Utils
{
    public class JwtParser
    {
        private readonly string _jwtInput;

        public JwtParser(string jwtInput)
        {
            _jwtInput = jwtInput;
        }

        public bool TryParse(out string jwtOutput)
        {
            var jwtHeader = "{";
            /*  if (!TryGetToken(out var token))
              {
                  jwtOutput = "The token doesn't seem to be in a proper JWT format.";
                  return false;
              }

              //Extract the headers of the JWT
              var headers = token.Header;
              var jwtHeader = "{";
              foreach (var h in headers)
              {
                  jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
              }*/

            jwtHeader += "}";
            jwtOutput = "Header:\r\n" + JToken.Parse(jwtHeader).ToString(Formatting.Indented);

            //Extract the payload of the JWT
            //var claims = token.Claims;
            //var jwtPayload = claims.Aggregate("{",
              //  (current, c) => current + ('"' + c.Type + "\":\"" + c.Value + "\","));

            //jwtPayload += "}";

            //jwtOutput += "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);

            return true;
        }

 /*       public bool TryGetToken(out JwtSecurityToken token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            if (jwtHandler.CanReadToken(_jwtInput) != true)
            {
                token = null;
                return false;
            }

            token = jwtHandler.ReadJwtToken(_jwtInput);

            return true;
        }*/
    }
}