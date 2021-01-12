using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
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

            var solutionEntity = await CreateSolution(solutionId);
            (CatalogueItemEntity, AdditionalServiceEntity) entity = await CreateAdditionalService("Cat", solutionId);

            var expected = ConvertEntityToResult(entity, solutionEntity);

            var request =
                (await _additionalServiceRepository.GetAdditionalServiceBySolutionIdsAsync(
                    new List<string> { solutionId }, CancellationToken.None)).ToList();

            request.Count.Should().Be(1);
            request.First().Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAdditionalServiceByCatalogueItemIdAsync_MultipleSolutionIds_ReturnsAdditionalServices()
        {
            const string solutionId1 = "Sln1";
            const string solutionId2 = "Sln2";

            var solutionEntity1 = await CreateSolution(solutionId1);
            var solutionEntity2 = await CreateSolution(solutionId2);
            var entity1 = await CreateAdditionalService("Cat1", solutionId1);
            var entity2 = await CreateAdditionalService("Cat2", solutionId2);

            var expected1 = ConvertEntityToResult(entity1, solutionEntity1);
            var expected2 = ConvertEntityToResult(entity2, solutionEntity2);

            var request =
                (await _additionalServiceRepository.GetAdditionalServiceBySolutionIdsAsync(
                    new List<string> { solutionId1, solutionId2 }, CancellationToken.None)).ToList();

            request.Count.Should().Be(2);
            request.First().Should().BeEquivalentTo(expected1);
            request.Last().Should().BeEquivalentTo(expected2);
        }

        private static async Task<CatalogueItemEntity> CreateSolution(string solutionId)
        {
            var solutionCatalogueEntity = CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(solutionId)
                .WithSupplierId(SupplierId)
                .Build();

            await solutionCatalogueEntity.InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(solutionId)
                .Build()
                .InsertAsync();

            return solutionCatalogueEntity;
        }

        private static async Task<(CatalogueItemEntity, AdditionalServiceEntity)> CreateAdditionalService(string catalogueItemId, string solutionId)
        {
            var catalogueItemEntity = CatalogueItemEntityBuilder.Create()
                .WithCatalogueItemId(catalogueItemId)
                .WithSupplierId(SupplierId)
                .Build();
            await catalogueItemEntity.InsertAsync();

            var additionServiceEntity = AdditionalServiceEntityBuilder.Create()
                .WithCatalogueItemId(catalogueItemId)
                .WithSolutionId(solutionId)
                .Build();

            await additionServiceEntity.InsertAsync();
            return (catalogueItemEntity, additionServiceEntity);
        }

        private static IAdditionalServiceResult ConvertEntityToResult(
            (CatalogueItemEntity catalogueItemEntity, AdditionalServiceEntity additionalServiceEntity) entity, CatalogueItemEntity solutionEntity)
        {
            return new AdditionalServiceResult
            {
                CatalogueItemId = entity.catalogueItemEntity.CatalogueItemId,
                CatalogueItemName = entity.catalogueItemEntity.Name,
                Summary = entity.additionalServiceEntity.Summary,
                SolutionId = entity.additionalServiceEntity.SolutionId,
                SolutionName = solutionEntity.Name,
            };
        }
    }
}
