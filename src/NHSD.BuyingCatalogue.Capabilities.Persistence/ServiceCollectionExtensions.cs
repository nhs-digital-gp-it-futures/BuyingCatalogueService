using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NHSD.BuyingCatalogue.Capabilities.Persistence.Repositories;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCapabilityPersistence(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<ICapabilityRepository, CapabilityRepository>();
        }
    }
}
