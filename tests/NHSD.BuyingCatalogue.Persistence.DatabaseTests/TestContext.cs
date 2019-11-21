using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal class TestContext
    {
        public ICapabilityRepository CapabilityRepository => _scope.CapabilityRepository;

        public IMarketingContactRepository MarketingContactRepository => _scope.MarketingContactRepository;

        public ISolutionCapabilityRepository SolutionCapabilityRepository => _scope.SolutionCapabilityRepository;

        public ISolutionDetailRepository SolutionDetailRepository => _scope.SolutionDetailRepository;

        public ISolutionListRepository SolutionListRepository => _scope.SolutionListRepository;

        public ISolutionRepository SolutionRepository => _scope.SolutionRepository;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());
            serviceCollection.AddSingleton<IConfiguration>(configuration.Object);

            serviceCollection.RegisterPersistence();

            serviceCollection.AddSingleton<Scope>();

            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }
        
        private class Scope
        {
            public ICapabilityRepository CapabilityRepository { get; }

            public IMarketingContactRepository MarketingContactRepository { get; }

            public ISolutionCapabilityRepository SolutionCapabilityRepository { get; }

            public ISolutionDetailRepository SolutionDetailRepository { get; }

            public ISolutionListRepository SolutionListRepository { get; }

            public ISolutionRepository SolutionRepository { get; }

            public Scope(ICapabilityRepository capabilityRepository,
                IMarketingContactRepository marketingContactRepository,
                ISolutionCapabilityRepository solutionCapabilityRepository,
                ISolutionDetailRepository solutionDetailRepository,
                ISolutionListRepository solutionListRepository,
                ISolutionRepository solutionRepository)
            {
                CapabilityRepository = capabilityRepository;
                MarketingContactRepository = marketingContactRepository;
                SolutionCapabilityRepository = solutionCapabilityRepository;
                SolutionDetailRepository = solutionDetailRepository;
                SolutionListRepository = solutionListRepository;
                SolutionRepository = solutionRepository;
            }
        }
    }
}
