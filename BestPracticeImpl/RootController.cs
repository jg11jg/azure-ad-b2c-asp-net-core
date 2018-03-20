using BestPracticeInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Serialization;

namespace BestPracticeImplementations
{
    public class RootController : IRootController
    {
        public RootController(HttpRequest req, TraceWriter log)
        {

        }
        public IActionResult Me()
        {
            return new OkObjectResult("Me!");
        }
    }
}
