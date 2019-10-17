using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Application.Persistence;

namespace NHSD.BuyingCatalogue.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CapabilityReader>();
            serviceCollection.AddTransient<SolutionListReader>();
            serviceCollection.AddTransient<SolutionReader>();
            serviceCollection.AddTransient<SolutionSummaryUpdater>();
            serviceCollection.AddTransient<SolutionFeaturesUpdater>();
            return serviceCollection;
        }
    }
}
