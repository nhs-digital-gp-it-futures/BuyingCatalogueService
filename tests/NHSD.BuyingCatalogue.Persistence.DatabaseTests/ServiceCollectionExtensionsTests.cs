using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence.Repositories;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;
using NUnit.Framework;


namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public sealed class ServiceCollectionExtensionsTests
    {
        [Test]
        public void ShouldRegisterRepositories()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(Mock.Of<IDbConnector>());
            serviceCollection.RegisterSolutionsPersistence();
            serviceCollection.RegisterSolutionListPersistence();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<ISolutionListRepository>().Should().BeOfType<SolutionListRepository>();
            provider.GetService<ISolutionRepository>().Should().BeOfType<SolutionRepository>();
            provider.GetService<ISolutionDetailRepository>().Should().BeOfType<SolutionDetailRepository>();
        }
    }
}
