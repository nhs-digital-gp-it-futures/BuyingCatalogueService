using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Persistence.Repositories;

namespace NHSD.BuyingCatalogue.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISolutionListRepository, SolutionListRepository>();
            serviceCollection.AddTransient<ISolutionRepository, SolutionRepository>();
            serviceCollection.AddTransient<ISolutionDetailRepository, SolutionDetailRepository>();
            serviceCollection.AddTransient<ISolutionCapabilityRepository, SolutionCapabilityRepository>();
            serviceCollection.AddTransient<IMarketingContactRepository, MarketingContactRepository>();
            return serviceCollection;
        }
    }
}
