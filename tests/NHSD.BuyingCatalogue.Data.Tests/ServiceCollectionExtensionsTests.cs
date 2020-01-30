using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Data.HealthChecks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Data.Tests
{
    [TestFixture]
    public sealed class ServiceCollectionExtensionsTests
    {
        [Test]
        public void ShouldRegisterRepositories()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(Mock.Of<ISettings>());
            var logger = new Mock<ILogger<DbConnector>>();

            serviceCollection.AddSingleton(logger.Object);
            serviceCollection.RegisterData();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<IDbConnectionFactory>().Should().BeOfType<DbConnectionFactory>();
            provider.GetService<IDbConnector>().Should().BeOfType<DbConnector>();
            provider.GetService<IRepositoryHealthCheck>().Should().BeOfType<RepositoryHealthCheck>();
        }
    }
}
