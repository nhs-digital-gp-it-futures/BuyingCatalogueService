using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    internal sealed class CatalogueItemRepositoryTests
    {
        private ICatalogueItemRepository catalogueItemRepository;
        private const string SupplierId1 = "Sup1";
        private const string SupplierId2 = "Sup2";
        
        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId1)
                .Build()
                .InsertAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId2)
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            catalogueItemRepository = testContext.CatalogueItemRepository;
        }

        [Test]
        public async Task GetByIdAsync_CatalogueItemIdIsNull_ReturnsNull()
        {
            var request = await catalogueItemRepository.GetByIdAsync(null, CancellationToken.None);

            request.Should().BeNull();
        }

        [Test]
        public async Task GetByIdAsync_CatalogueItemExists_ReturnsResult()
        {
            const string catalogueItemId = "100000-001";

            var catalogueItemEntity = await CreateCatalogueItemEntity(catalogueItemId, SupplierId1, CatalogueItemType.Solution);

            var request = await catalogueItemRepository.GetByIdAsync(catalogueItemId, CancellationToken.None);
            request.CatalogueItemId.Should().BeEquivalentTo(catalogueItemEntity.CatalogueItemId);
            request.Name.Should().BeEquivalentTo(catalogueItemEntity.Name);
        }

        [TestCase(null, null, null)]
        [TestCase(null, CatalogueItemType.Solution, null)]
        [TestCase(null, CatalogueItemType.AdditionalService, null)]
        [TestCase(null, CatalogueItemType.AssociatedService, null)]
        [TestCase(null, CatalogueItemType.Solution, PublishedStatus.Published)]
        [TestCase(SupplierId1, null, null)]
        [TestCase(SupplierId1, null, null)]
        [TestCase(SupplierId1, null, null)]
        [TestCase(SupplierId2, CatalogueItemType.Solution, null)]
        [TestCase(SupplierId1, CatalogueItemType.AdditionalService, null)]
        [TestCase(SupplierId1, CatalogueItemType.AssociatedService, null)]
        [TestCase(SupplierId1, CatalogueItemType.Solution, PublishedStatus.Published)]
        public async Task ListAsync_NoFilter_ReturnsAllResults(
            string supplierId,
            CatalogueItemType? catalogueItemType,
            PublishedStatus? publishedStatus)
        {
            var catalogueItemsEntity = new List<CatalogueItemEntity>
            {
                await CreateCatalogueItemEntity("100000-001", SupplierId1, CatalogueItemType.Solution),
                await CreateCatalogueItemEntity("100000-002", SupplierId1, CatalogueItemType.AdditionalService),
                await CreateCatalogueItemEntity("100000-003", SupplierId2, CatalogueItemType.AssociatedService),
                await CreateCatalogueItemEntity("100000-004", SupplierId1, CatalogueItemType.Solution),
                await CreateCatalogueItemEntity("100000-005", SupplierId1, CatalogueItemType.Solution, PublishedStatus.Draft),
                await CreateCatalogueItemEntity("100000-006", SupplierId2, CatalogueItemType.Solution, PublishedStatus.Withdrawn),
            };

            var request = await catalogueItemRepository.ListAsync(
                supplierId,
                catalogueItemType,
                publishedStatus,
                CancellationToken.None);

            bool MatchesSupplier(CatalogueItemEntity item) => supplierId is null || item.SupplierId == supplierId;
            bool MatchesCatalogueItemType(CatalogueItemEntity item) => catalogueItemType is null || item.CatalogueItemTypeId == (int)catalogueItemType;
            bool MatchesPublishedStatus(CatalogueItemEntity item) => publishedStatus is null || item.PublishedStatusId == (int)publishedStatus;
            bool MatchesFilters(CatalogueItemEntity item) => MatchesSupplier(item) 
                && MatchesCatalogueItemType(item)
                && MatchesPublishedStatus(item);

            var filteredExpected = catalogueItemsEntity.Where(MatchesFilters);

            var expected = filteredExpected.Select(i => new { i.CatalogueItemId, i.Name });

            request.Should().BeEquivalentTo(expected);
        }

        public static async Task<CatalogueItemEntity> CreateCatalogueItemEntity(
            string catalogueItemId,
            string supplierId,
            CatalogueItemType catalogueItemType,
            PublishedStatus publishedStatus = PublishedStatus.Published)
        {
            var catalogueItem = CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(catalogueItemId)
                .WithSupplierId(supplierId)
                .WithCatalogueItemTypeId((int)catalogueItemType)
                .WithPublishedStatusId((int)publishedStatus)
                .Build();

            await catalogueItem.InsertAsync();
            return catalogueItem;
        }
    }
}
