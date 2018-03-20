using BestPracticeImplementations;
using BestPracticeInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BestPracticeDependencyInjection
{
    public static class ServiceCollectionFactory
    {
        public static IServiceCollection GetServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IRootController, RootController>();

            return serviceCollection;
        }
    }
}
