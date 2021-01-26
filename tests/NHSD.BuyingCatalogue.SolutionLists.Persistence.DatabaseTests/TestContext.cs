using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence.DatabaseTests
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
            serviceCollection.RegisterSolutionListPersistence();
            var logger = new Mock<ILogger<DbConnector>>();

            serviceCollection.AddSingleton(logger.Object);
            serviceCollection.AddSingleton<Scope>();

            scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        public ISolutionListRepository SolutionListRepository => scope.SolutionListRepository;

        private sealed class Scope
        {
            public Scope(ISolutionListRepository solutionListRepository)
            {
                SolutionListRepository = solutionListRepository;
            }

            public ISolutionListRepository SolutionListRepository { get; }
        }
    }
}
