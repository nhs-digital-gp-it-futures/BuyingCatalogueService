using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    internal class TestContext
    {
        private readonly Scope _scope;
        public IMarketingContactRepository MarketingContactRepository => _scope.MarketingContactRepository;

        public ISolutionCapabilityRepository SolutionCapabilityRepository => _scope.SolutionCapabilityRepository;
        public ISolutionEpicRepository SolutionEpicRepository => _scope.SolutionEpicRepository;

        public ISolutionDetailRepository SolutionDetailRepository => _scope.SolutionDetailRepository;

        public ISolutionRepository SolutionRepository => _scope.SolutionRepository;

        public ISupplierRepository SupplierRepository => _scope.SupplierRepository;

        public IDbConnector DbConnector => _scope.DbConnector;

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

            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        private class Scope
        {
            public IMarketingContactRepository MarketingContactRepository { get; }

            public ISolutionCapabilityRepository SolutionCapabilityRepository { get; }
            public ISolutionEpicRepository SolutionEpicRepository { get; }

            public ISolutionDetailRepository SolutionDetailRepository { get; }

            public ISolutionRepository SolutionRepository { get; }

            public ISupplierRepository SupplierRepository { get; }

            public IDbConnector DbConnector { get; }

            public Scope(IMarketingContactRepository marketingContactRepository,
                ISolutionCapabilityRepository solutionCapabilityRepository,
                ISolutionEpicRepository solutionEpicRepository,
                ISolutionDetailRepository solutionDetailRepository,
                ISolutionRepository solutionRepository,
                ISupplierRepository supplierRepository,
                IDbConnector dbConnector)
            {
                MarketingContactRepository = marketingContactRepository;
                SolutionCapabilityRepository = solutionCapabilityRepository;
                SolutionEpicRepository = solutionEpicRepository;
                SolutionDetailRepository = solutionDetailRepository;
                SolutionRepository = solutionRepository;
                SupplierRepository = supplierRepository;
                DbConnector = dbConnector;
            }
        }
    }
}
