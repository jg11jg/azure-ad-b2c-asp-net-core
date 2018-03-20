using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BestPracticeFunctionApp
{
    namespace Root
    {
        public static class Echo
        {
            [FunctionName("Echo")]
            public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                HttpRequest req, TraceWriter log)
            {
                return new OkObjectResult(DateTime.UtcNow.ToString("O"));
            }
        }
    }
}
