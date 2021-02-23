using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    internal sealed class ServiceCollectionExtensionsTests
    {
        [Test]
        public void ShouldRegisterRepositories()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(Mock.Of<IDbConnector>());
            serviceCollection.RegisterSolutionsPersistence();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<ISolutionRepository>().Should().BeOfType<SolutionRepository>();
        }
    }
}
