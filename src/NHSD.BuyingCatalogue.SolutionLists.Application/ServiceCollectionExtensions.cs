using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.SolutionLists.Application.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionListApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<SolutionListReader>();
        }
    }
}
