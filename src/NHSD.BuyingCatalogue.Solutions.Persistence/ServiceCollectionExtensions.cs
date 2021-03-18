using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Persistence.Clients;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;

namespace NHSD.BuyingCatalogue.Solutions.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionsPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISolutionRepository, SolutionRepository>();
            serviceCollection.AddTransient<ISolutionCapabilityRepository, SolutionCapabilityRepository>();
            serviceCollection.AddTransient<IMarketingContactRepository, MarketingContactRepository>();
            serviceCollection.AddTransient<ISupplierRepository, SupplierRepository>();
            serviceCollection.AddHttpClient<IDocumentsApiClient, DocumentsApiClient>();
            serviceCollection.AddTransient<IDocumentRepository, DocumentRepository>();
            serviceCollection.AddTransient<ISolutionEpicRepository, SolutionEpicRepository>();
            serviceCollection.AddTransient<IEpicRepository, EpicRepository>();
            serviceCollection.AddTransient<ISolutionEpicStatusRepository, SolutionEpicStatusRepository>();
            serviceCollection.AddTransient<IPriceRepository, PriceRepository>();
            serviceCollection.AddTransient<IAdditionalServiceRepository, AdditionalServiceRepository>();
            serviceCollection.AddTransient<ICatalogueItemRepository, CatalogueItemRepository>();
            serviceCollection.AddTransient<ISolutionFrameworkRepository, SolutionFrameworkRepository>();

            return serviceCollection;
        }
    }
}
