using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence.Repositories;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionListPersistence(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<ISolutionListRepository, SolutionListRepository>();
        }
    }
}
