using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;

namespace NHSD.BuyingCatalogue.Solutions.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionsPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISolutionRepository, SolutionRepository>();
            serviceCollection.AddTransient<ISolutionDetailRepository, SolutionDetailRepository>();
            serviceCollection.AddTransient<ISolutionCapabilityRepository, SolutionCapabilityRepository>();
            serviceCollection.AddTransient<IMarketingContactRepository, MarketingContactRepository>();
            return serviceCollection;
        }
    }
}
