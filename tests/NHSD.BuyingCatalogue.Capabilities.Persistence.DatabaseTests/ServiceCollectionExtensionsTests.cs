using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NHSD.BuyingCatalogue.Capabilities.Persistence.Repositories;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.DatabaseTests
{
    [TestFixture]
    public sealed class ServiceCollectionExtensionsTests
    {
        [Test]
        public void ShouldRegisterRepositories()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(Mock.Of<IDbConnector>());
            serviceCollection.RegisterCapabilityPersistence();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<ICapabilityRepository>().Should().BeOfType<CapabilityRepository>();
        }
    }
}
