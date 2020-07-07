using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    internal sealed class AdditionalServiceRepositoryTests
    {
        private IAdditionalServiceRepository _additionalServiceRepository;

        private const string SupplierId = "Sup1";

        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            _additionalServiceRepository = testContext.AdditionalServiceRepository;
        }

        [Test]
        public async Task GetAdditionalServiceByCatalogueItemIdAsync_InvalidSolutionId_ReturnsEmptyList()
        {
            var request =
                await _additionalServiceRepository.GetAdditionalServiceBySolutionIdsAsync(
                    new List<string> { "INVALID" }, CancellationToken.None);

            request.Count().Should().Be(0);
        }

        [Test]
        public async Task GetAdditionalServiceByCatalogueItemIdAsync_SingleSolutionId_ReturnsOneAdditionalService()
        {
            const string solutionId = "Sln1";

            await CreateSolution(solutionId);
            await CreateAdditionalService("Cat", solutionId);

            var request =
                await _additionalServiceRepository.GetAdditionalServiceBySolutionIdsAsync(
                    new List<string> { solutionId }, CancellationToken.None);

            request.Count().Should().Be(1);
        }

        [Test]
        public async Task GetAdditionalServiceByCatalogueItemIdAsync_MultipleSolutionIds_ReturnsAdditionalServices()
        {
            const string solutionId1 = "Sln1";
            const string solutionId2 = "Sln2";

            await CreateSolution(solutionId1);
            await CreateSolution(solutionId2);
            await CreateAdditionalService("Cat1", solutionId1);
            await CreateAdditionalService("Cat2", solutionId2);

            var request =
                await _additionalServiceRepository.GetAdditionalServiceBySolutionIdsAsync(
                    new List<string> { solutionId1, solutionId2 }, CancellationToken.None);

            request.Count().Should().Be(2);
        }

        private async Task CreateSolution(string solutionId)
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(solutionId)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(solutionId)
                .Build()
                .InsertAsync();
        }

        private async Task CreateAdditionalService(string catalogueItemId, string solutionId)
        {
            await CatalogueItemEntityBuilder.Create()
                .WithCatalogueItemId(catalogueItemId)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await AdditionalServiceEntityBuilder.Create()
                .WithCatalogueItemId(catalogueItemId)
                .WithSolutionId(solutionId)
                .Build()
                .InsertAsync();
        }
    }
}
