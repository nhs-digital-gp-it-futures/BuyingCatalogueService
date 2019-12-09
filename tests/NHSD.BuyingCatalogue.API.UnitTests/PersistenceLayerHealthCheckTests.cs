using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class PersistenceLayerHealthCheckTests
    {
        private PersistenceLayerHealthCheck _persistenceLayerHealthCheck;
        private Mock<IRepositoryHealthCheck> _repositoryHealthCheck;
        private Mock<ILogger<PersistenceLayerHealthCheck>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<PersistenceLayerHealthCheck>>();
        }

        [Test]
        public void ShouldThrowsConstructorNullArguments()
        {
            _repositoryHealthCheck = new Mock<IRepositoryHealthCheck>();
            Assert.Throws<ArgumentNullException>(() => new PersistenceLayerHealthCheck(null, _logger.Object));
            Assert.Throws<ArgumentNullException>(() => new PersistenceLayerHealthCheck(_repositoryHealthCheck.Object, null));
        }

        [Test]
        public async Task ShouldGetHealthyAsync()
        {
            _repositoryHealthCheck = new Mock<IRepositoryHealthCheck>();
            _persistenceLayerHealthCheck = new PersistenceLayerHealthCheck(_repositoryHealthCheck.Object, _logger.Object);

            var context = new HealthCheckContext();
            var cancelToken = new CancellationToken();

            var result = (await _persistenceLayerHealthCheck.CheckHealthAsync(context, cancelToken).ConfigureAwait(false));

            result.Status.Should().Be(2);
        }

        [Test]
        public async Task ShouldGetUnhealthyAsync()
        {
            _repositoryHealthCheck = new Mock<IRepositoryHealthCheck>(MockBehavior.Strict);
            _repositoryHealthCheck.Setup(x => x.RunAsync(new CancellationToken())).ThrowsAsync(new IOException());

            _persistenceLayerHealthCheck = new PersistenceLayerHealthCheck(_repositoryHealthCheck.Object, _logger.Object);

            var context = new HealthCheckContext(){Registration = new HealthCheckRegistration("Test", _persistenceLayerHealthCheck, HealthStatus.Unhealthy, new List<string>())};
            var cancelToken = new CancellationToken();

            var result = (await _persistenceLayerHealthCheck.CheckHealthAsync(context, cancelToken).ConfigureAwait(false));

            result.Status.Should().Be(0);
            result.Exception.Message.Should().Be(new IOException().Message);
        }
    }
}
