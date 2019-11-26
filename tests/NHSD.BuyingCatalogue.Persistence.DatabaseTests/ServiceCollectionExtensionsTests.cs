using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Persistence;
using NHSD.BuyingCatalogue.Capabilities.Persistence.Repositories;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.SolutionLists.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence.Repositories;
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
            serviceCollection.AddSingleton(Mock.Of<ISettings>());
            serviceCollection.RegisterData();
            serviceCollection.RegisterSolutionsPersistence();
            serviceCollection.RegisterCapabilityPersistence();
            serviceCollection.RegisterSolutionListPersistence();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<ICapabilityRepository>().Should().BeOfType<CapabilityRepository>();
            provider.GetService<ISolutionListRepository>().Should().BeOfType<SolutionListRepository>();
            provider.GetService<ISolutionRepository>().Should().BeOfType<SolutionRepository>();
            provider.GetService<ISolutionDetailRepository>().Should().BeOfType<SolutionDetailRepository>();
        }
    }
}
