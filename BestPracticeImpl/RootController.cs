using BestPracticeInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BestPracticeImplementations
{
    public class RootController : IRootController
    {
        public IActionResult Me()
        {
            return new OkObjectResult("Me!");
        }
    }
}
