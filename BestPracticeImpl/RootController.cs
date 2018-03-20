using BestPracticeInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Host;

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
