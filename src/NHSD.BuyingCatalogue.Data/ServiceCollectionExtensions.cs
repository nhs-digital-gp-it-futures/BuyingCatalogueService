using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Data.Infrastructure;

namespace NHSD.BuyingCatalogue.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterData(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IDbConnectionFactory, DbConnectionFactory>()
                .AddSingleton<IDbConnector, DbConnector>();
        }
    }
}
