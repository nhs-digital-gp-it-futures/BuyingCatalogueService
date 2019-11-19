using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private SolutionRepository _solutionRepository;

        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();

        private readonly Guid _org1Id = Guid.NewGuid();
        private readonly string _orgName = "Org1";

        private readonly string _supplierId = "Sup 1";
        private readonly DateTime _lastUpdated = DateTime.Today;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await OrganisationEntityBuilder.Create()
                .WithName(_orgName)
                .WithId(_org1Id)
                .Build()
                .InsertAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithOrganisation(_org1Id)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithDescription("Cap1Desc").Build().InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithDescription("Cap2Desc").Build().InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());

            _solutionRepository = new SolutionRepository(new DbConnector(new DbConnectionFactory(_configuration.Object)));
        }

        [Test]
        public async Task ShouldGetById()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOnLastUpdated(_lastUpdated)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(_lastUpdated.ToString());
            solution.Summary.Should().Be("Sln1Summary");
            solution.Description.Should().Be("Sln1Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().Be("Features");
            solution.ClientApplication.Should().Be("Browser-based");
            solution.OrganisationName.Should().Be(_orgName);
            solution.IsFoundation.Should().BeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGetByIdFoundation(bool isFoundation)
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithFoundation(isFoundation)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldGetByIdNotPresent()
        {
            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetByIdSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOnLastUpdated(_lastUpdated)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(_lastUpdated.ToString());
            solution.Summary.Should().BeNull();
            solution.Description.Should().BeNull();
            solution.OrganisationName.Should().Be(_orgName);
            solution.AboutUrl.Should().BeNull();
            solution.Features.Should().BeNull();
            solution.ClientApplication.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateSupplierStatus()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOnLastUpdated(_lastUpdated)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithSupplierStatusId(1)
                .Build()
                .InsertAsync();

            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            await _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.SupplierStatusId.Should().Be(2);
        }

        [Test]
        public void ShouldThrowOnUpdateSupplierStatusSolutionNotPresent()
        {
            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            Assert.ThrowsAsync<SqlException>(() =>  _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken()));
        }
    }
}
