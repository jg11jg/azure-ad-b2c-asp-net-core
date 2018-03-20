using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace BestPracticeFunctionApp
{
    public static class Request
    {
        [FunctionName("Request")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "patch", "put", "delete")]
            HttpRequest req, TraceWriter log)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ContentType: {req.ContentType}");
            sb.AppendLine($"Host: {req.Host}");
            sb.AppendLine($"IsHttps: {req.IsHttps}");
            sb.AppendLine($"Method: {req.Method}");
            sb.AppendLine($"PathBase: {req.PathBase}");
            sb.AppendLine($"Path: {req.Path}");
            sb.AppendLine($"Protocol: {req.Protocol}");

            sb.AppendLine($"Cookies: ({req.Cookies.Count})");
            foreach (var cookie in req.Cookies)
            {
                sb.AppendLine($"\t{cookie.Key}: {cookie.Value}");
            }

            sb.AppendLine($"Query: ({req.Query.Count}) ");
            foreach (var query in req.Query)
            {
                sb.AppendLine($"\t{query.Key}: {query.Value}");
            }

            sb.AppendLine($"Headers: ({req.Headers.Count}) ");
            foreach (var header in req.Headers)
            {
                sb.AppendLine($"\t{header.Key}: {header.Value}");
            }

            var body = await new StreamReader(req.Body).ReadToEndAsync();

            sb.AppendLine($"Body: ");
            sb.AppendLine($"\t{body}");

            sb.AppendLine($"HttpContext.User.Identity: ");

            DocumentIdentity(req.HttpContext.User.Identity, sb);

            sb.AppendLine($"HttpContext.User.Claims: ({req.HttpContext.User.Claims.Count()})");

            foreach (var claim in req.HttpContext.User.Claims)
            {
                sb.AppendLine($"\t{claim.Type}: {claim.Value}");
            }

            sb.AppendLine($"ClaimsPrincipal.Current.Identity:");

            DocumentIdentity(ClaimsPrincipal.Current?.Identity, sb);

            if (ClaimsPrincipal.Current != null)
            {
                foreach (var claim in ClaimsPrincipal.Current.Claims)
                {
                    sb.AppendLine($"\t\t{claim.Type}: {claim.Value}");
                }
            }

            sb.AppendLine($"Thread.CurrentPrincipal.Identity:");

            DocumentIdentity(Thread.CurrentPrincipal?.Identity, sb);

            return new OkObjectResult(sb.ToString());
        }

        private static void DocumentIdentity(IIdentity identity, StringBuilder sb)
        {
            if (identity == null)
            {
                sb.AppendLine("\t<null>");
                return;
            }

            sb.AppendLine($"\tAuthenticationType: {identity.AuthenticationType}");
            sb.AppendLine($"\tIsAuthenticated: {identity.IsAuthenticated}");
            sb.AppendLine($"\tName: {identity.Name}");
        }
    }
}