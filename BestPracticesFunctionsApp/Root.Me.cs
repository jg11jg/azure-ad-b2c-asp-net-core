using BestPracticeDependencyInjection;
using BestPracticeInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;

namespace BestPracticeFunctionApp
{
    public static class RootMe
    {
        [FunctionName("Me")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {

            var serviceCollection = ServiceCollectionFactory.GetServiceCollection();

            serviceCollection.AddSingleton(req);
            serviceCollection.AddSingleton(log);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var rootController = serviceProvider.GetService<IRootController>();

            return rootController.Me();
        }
    }
}
