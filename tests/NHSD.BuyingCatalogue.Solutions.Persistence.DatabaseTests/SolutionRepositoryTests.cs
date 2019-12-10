using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionRepositoryTests
    {
        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();

        private readonly Guid _org1Id = Guid.NewGuid();
        private readonly string _orgName = "Org1";

        private readonly string _supplierId = "Sup 1";
        private readonly DateTime _lastUpdated = DateTime.Today;

        private readonly string _solution1Id = "Sln1";

        private ISolutionRepository _solutionRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            await OrganisationEntityBuilder.Create()
                .WithName(_orgName)
                .WithId(_org1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithOrganisation(_org1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithDescription("Cap1Desc").Build().InsertAsync()
                .ConfigureAwait(false);
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithDescription("Cap2Desc").Build().InsertAsync()
                .ConfigureAwait(false);

            TestContext testContext = new TestContext();
            _solutionRepository = testContext.SolutionRepository;
        }

        [Test]
        public async Task ShouldGetById()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithOnLastUpdated(_lastUpdated)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(_lastUpdated);
            solution.Summary.Should().Be("Sln1Summary");
            solution.Description.Should().Be("Sln1Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().Be("Features");
            solution.ClientApplication.Should().Be("Browser-based");
            solution.OrganisationName.Should().Be(_orgName);
            solution.IsFoundation.Should().BeFalse();
            solution.PublishedStatus.Should().Be(PublishedStatus.Published);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGetByIdFoundation(bool isFoundation)
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithFoundation(isFoundation)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken()).ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldGetByIdNotPresent()
        {
            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken()).ConfigureAwait(false);
            solution.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetByIdSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithOnLastUpdated(_lastUpdated)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(_lastUpdated);
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
                .WithId(_solution1Id)
                .WithOnLastUpdated(_lastUpdated)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithSupplierStatusId(1)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns(_solution1Id);
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            _solutionRepository.CheckExists(_solution1Id, new CancellationToken()).Result.Should().BeTrue();

            await _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken()).ConfigureAwait(false);

            var solution = await SolutionEntity.GetByIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);

            (await solution.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);

            solution.SupplierStatusId.Should().Be(2);
        }

        [Test]
        public void SolutionIdDoesNotExist()
        {
            _solutionRepository.CheckExists(_solution1Id, new CancellationToken()).Result.Should().BeFalse();
        }

        [Test]
        public void ShouldThrowOnUpdateSupplierStatusSolutionNotPresent()
        {
            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns(_solution1Id);
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            Assert.ThrowsAsync<SqlException>(() => _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken()));
        }

        [Test]
        public void ShouldThrowOnUpdateSupplierStatusNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _solutionRepository.UpdateSupplierStatusAsync(null, new CancellationToken()));
        }
    }
}
