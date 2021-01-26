using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.DatabaseTests
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
            serviceCollection.RegisterCapabilityPersistence();
            var logger = new Mock<ILogger<DbConnector>>();

            serviceCollection.AddSingleton(logger.Object);
            serviceCollection.AddSingleton<Scope>();

            scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        public ICapabilityRepository CapabilityRepository => scope.CapabilityRepository;

        private sealed class Scope
        {
            public Scope(ICapabilityRepository capabilityRepository)
            {
                CapabilityRepository = capabilityRepository;
            }

            public ICapabilityRepository CapabilityRepository { get; }
        }
    }
}
