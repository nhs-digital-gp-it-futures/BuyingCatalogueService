using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Persistence.HealthChecks;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests.HealthChecks
{
    [TestFixture]
    public sealed class RepositoryHealthCheckTests
    {
        private Mock<IConfiguration> _configuration;

        private RepositoryHealthCheck _repositoryHealthCheck;

        [SetUp]
        public  void Setup()
        {
            _configuration = new Mock<IConfiguration>();
        }

        [Test]
        public void ShouldConstruct()
        {
            Assert.Throws<ArgumentNullException>(() => new RepositoryHealthCheck(null, _configuration.Object));
            Assert.Throws<ArgumentNullException>(() => new RepositoryHealthCheck(new DbConnectionFactory(_configuration.Object), null));
        }

        [Test]
        public async Task ShouldReportHealthy()
        {
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());
            _repositoryHealthCheck = new RepositoryHealthCheck(new DbConnectionFactory(_configuration.Object), _configuration.Object);

            await _repositoryHealthCheck.RunAsync(new CancellationToken());
        }

        [Test]
        public void ShouldReportUnhealthyAfterFiveSeconds()
        {
            var start = DateTime.Now;

            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString().Replace("localhost", "someotherserver"));
            _repositoryHealthCheck = new RepositoryHealthCheck(new DbConnectionFactory(_configuration.Object), _configuration.Object);

            Assert.ThrowsAsync<SqlException>(async () => await _repositoryHealthCheck.RunAsync(new CancellationToken()));

            DateTime.Now.Subtract(start).TotalSeconds.Should().BeInRange(5, 8);
        }
    }
}
