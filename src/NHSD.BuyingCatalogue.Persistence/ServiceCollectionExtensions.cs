using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.HealthChecks;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;

namespace NHSD.BuyingCatalogue.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            serviceCollection.AddSingleton<DbConnector>();
            serviceCollection.AddSingleton<IRepositoryHealthCheck, RepositoryHealthCheck>();
            serviceCollection.AddTransient<ICapabilityRepository, CapabilityRepository>();
            serviceCollection.AddTransient<ISolutionListRepository, SolutionListRepository>();
            serviceCollection.AddTransient<ISolutionRepository, SolutionRepository>();
            serviceCollection.AddTransient<ISolutionDetailRepository, SolutionDetailRepository>();
            serviceCollection.AddTransient<ISolutionCapabilityRepository, SolutionCapabilityRepository>();
            serviceCollection.AddTransient<IMarketingContactRepository, MarketingContactRepository>();
            return serviceCollection;
        }
    }
}
