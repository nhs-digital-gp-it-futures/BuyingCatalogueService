using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.HealthChecks;
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
            serviceCollection.AddSingleton<IConfiguration>(new Mock<IConfiguration>().Object);

            serviceCollection.RegisterPersistence();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<IDbConnectionFactory>().Should().BeOfType<DbConnectionFactory>();
            provider.GetService<IRepositoryHealthCheck>().Should().BeOfType<RepositoryHealthCheck>();
            provider.GetService<ICapabilityRepository>().Should().BeOfType<CapabilityRepository>();
            provider.GetService<ISolutionListRepository>().Should().BeOfType<SolutionListRepository>();
            provider.GetService<ISolutionRepository>().Should().BeOfType<SolutionRepository>();
            provider.GetService<ISolutionDetailRepository>().Should().BeOfType<SolutionDetailRepository>();
        }
    }
}
