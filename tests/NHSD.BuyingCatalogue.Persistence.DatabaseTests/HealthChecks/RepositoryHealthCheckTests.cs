using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data.HealthChecks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests.HealthChecks
{
    [TestFixture]
    public sealed class RepositoryHealthCheckTests
    {
        private Mock<ISettings> _settings;

        private RepositoryHealthCheck _repositoryHealthCheck;

        [SetUp]
        public  void Setup()
        {
            _settings = new Mock<ISettings>();
        }

        [Test]
        public void ShouldConstruct()
        {
            Assert.Throws<ArgumentNullException>(() => new RepositoryHealthCheck(null, _settings.Object));
            Assert.Throws<ArgumentNullException>(() => new RepositoryHealthCheck(new DbConnectionFactory(_settings.Object), null));
        }

        [Test]
        public async Task ShouldReportHealthy()
        {
            _settings.Setup(a => a.ConnectionString).Returns(ConnectionStrings.ServiceConnectionString());
            _repositoryHealthCheck = new RepositoryHealthCheck(new DbConnectionFactory(_settings.Object), _settings.Object);

            await _repositoryHealthCheck.RunAsync(new CancellationToken());
        }

        [Test]
        public void ShouldReportUnhealthyInLessThanTenSeconds()
        {
            var start = DateTime.Now;

            _settings.Setup(a => a.ConnectionString).Returns(ConnectionStrings.ServiceConnectionString().Replace("localhost", "someotherserver"));
            _repositoryHealthCheck = new RepositoryHealthCheck(new DbConnectionFactory(_settings.Object), _settings.Object);

            Assert.ThrowsAsync<SqlException>(async () => await _repositoryHealthCheck.RunAsync(new CancellationToken()));

            DateTime.Now.Subtract(start).TotalSeconds.Should().BeLessThan(10);
        }
    }
}
