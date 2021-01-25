using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    internal sealed class TestContext
    {
        private readonly Scope scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            var settings = new Mock<ISettings>();
            settings.Setup(s => s.ConnectionString).Returns(ConnectionStrings.ServiceConnectionString());
            serviceCollection.AddSingleton(settings.Object);

            serviceCollection.RegisterData();
            serviceCollection.RegisterSolutionsPersistence();
            var logger = new Mock<ILogger<DbConnector>>();

            serviceCollection.AddSingleton(logger.Object);
            serviceCollection.AddSingleton<Scope>();

            scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        public IMarketingContactRepository MarketingContactRepository => scope.MarketingContactRepository;

        public ISolutionCapabilityRepository SolutionCapabilityRepository => scope.SolutionCapabilityRepository;

        public ISolutionEpicRepository SolutionEpicRepository => scope.SolutionEpicRepository;

        public ISolutionDetailRepository SolutionDetailRepository => scope.SolutionDetailRepository;

        public ISolutionRepository SolutionRepository => scope.SolutionRepository;

        public ISupplierRepository SupplierRepository => scope.SupplierRepository;

        public IEpicRepository EpicRepository => scope.EpicRepository;

        public ISolutionEpicStatusRepository SolutionEpicStatusRepository => scope.SolutionEpicStatusRepository;

        public IPriceRepository PriceRepository => scope.PriceRepository;

        public IAdditionalServiceRepository AdditionalServiceRepository => scope.AdditionalServiceRepository;

        public ICatalogueItemRepository CatalogueItemRepository => scope.CatalogueItemRepository;

        public IDbConnector DbConnector => scope.DbConnector;

        private class Scope
        {
            public Scope(
                IMarketingContactRepository marketingContactRepository,
                ISolutionCapabilityRepository solutionCapabilityRepository,
                ISolutionEpicRepository solutionEpicRepository,
                ISolutionDetailRepository solutionDetailRepository,
                ISolutionRepository solutionRepository,
                ISupplierRepository supplierRepository,
                IEpicRepository epicRepository,
                ISolutionEpicStatusRepository solutionEpicStatusRepository,
                IPriceRepository priceRepository,
                IAdditionalServiceRepository additionalServiceRepository,
                ICatalogueItemRepository catalogueItemRepository,
                IDbConnector dbConnector)
            {
                MarketingContactRepository = marketingContactRepository;
                SolutionCapabilityRepository = solutionCapabilityRepository;
                SolutionEpicRepository = solutionEpicRepository;
                SolutionDetailRepository = solutionDetailRepository;
                SolutionRepository = solutionRepository;
                SupplierRepository = supplierRepository;
                EpicRepository = epicRepository;
                SolutionEpicStatusRepository = solutionEpicStatusRepository;
                PriceRepository = priceRepository;
                AdditionalServiceRepository = additionalServiceRepository;
                CatalogueItemRepository = catalogueItemRepository;
                DbConnector = dbConnector;
            }

            public IMarketingContactRepository MarketingContactRepository { get; }

            public ISolutionCapabilityRepository SolutionCapabilityRepository { get; }

            public ISolutionEpicRepository SolutionEpicRepository { get; }

            public ISolutionDetailRepository SolutionDetailRepository { get; }

            public ISolutionRepository SolutionRepository { get; }

            public ISupplierRepository SupplierRepository { get; }

            public IEpicRepository EpicRepository { get; }

            public ISolutionEpicStatusRepository SolutionEpicStatusRepository { get; }

            public IPriceRepository PriceRepository { get; }

            public IAdditionalServiceRepository AdditionalServiceRepository { get; }

            public ICatalogueItemRepository CatalogueItemRepository { get; }

            public IDbConnector DbConnector { get; }
        }
    }
}
