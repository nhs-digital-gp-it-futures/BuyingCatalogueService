using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Capabilities.Application.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCapabilitiesApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<CapabilityReader>();
        }
    }
}
