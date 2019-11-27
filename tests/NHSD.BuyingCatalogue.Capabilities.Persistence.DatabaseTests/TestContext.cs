using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.DatabaseTests
{
    internal class TestContext
    {
        public ICapabilityRepository CapabilityRepository => _scope.CapabilityRepository;

        public IDbConnector DbConnector => _scope.DbConnector;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            var settings = new Mock<ISettings>();
            settings.Setup(s => s.ConnectionString).Returns(ConnectionStrings.ServiceConnectionString());
            serviceCollection.AddSingleton(settings.Object);

            serviceCollection.RegisterData();
            serviceCollection.RegisterCapabilityPersistence();

            serviceCollection.AddSingleton<Scope>();

            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }
        
        private class Scope
        {
            public ICapabilityRepository CapabilityRepository { get; }

            public IDbConnector DbConnector { get; }

            public Scope(ICapabilityRepository capabilityRepository,
                IDbConnector dbConnector)
            {
                CapabilityRepository = capabilityRepository;
                DbConnector = dbConnector;
            }
        }
    }
}
