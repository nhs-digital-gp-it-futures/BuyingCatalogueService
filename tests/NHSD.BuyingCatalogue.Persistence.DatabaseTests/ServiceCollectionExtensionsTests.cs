using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
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
            serviceCollection.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            serviceCollection.AddSingleton<IConfiguration>(new Mock<IConfiguration>().Object);

            serviceCollection.RegisterPersistence();

            var provider = serviceCollection.BuildServiceProvider();

            provider.GetService<ICapabilityRepository>().Should().BeOfType<CapabilityRepository>();
            provider.GetService<ISolutionRepository>().Should().BeOfType<SolutionRepository>();
            provider.GetService<IMarketingDetailRepository>().Should().BeOfType<MarketingDetailRepository>();
        }
    }
}
