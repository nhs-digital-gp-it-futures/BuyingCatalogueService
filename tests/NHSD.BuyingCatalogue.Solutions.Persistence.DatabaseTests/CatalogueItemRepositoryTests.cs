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
        private ICatalogueItemRepository _catalogueItemRepository;
        private const string _supplierId1 = "Sup1";
        private const string _supplierId2 = "Sup2";
        
        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId1)
                .Build()
                .InsertAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId2)
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            _catalogueItemRepository = testContext.CatalogueItemRepository;
        }

        [Test]
        public async Task GetByIdAsync_CatalogueItemIdIsNull_ReturnsNull()
        {
            var request = await _catalogueItemRepository.GetByIdAsync(null, CancellationToken.None);

            request.Should().BeNull();
        }

        [Test]
        public async Task GetByIdAsync_CatalogueItemExists_ReturnsResult()
        {
            const string catalogueItemId = "100000-001";

            var catalogueItemEntity = await CreateCatalogueItemEntity(catalogueItemId, _supplierId1, (int)CatalogueItemType.Solution);

            var request = await _catalogueItemRepository.GetByIdAsync(catalogueItemId, CancellationToken.None);
            request.CatalogueItemId.Should().BeEquivalentTo(catalogueItemEntity.CatalogueItemId);
            request.Name.Should().BeEquivalentTo(catalogueItemEntity.Name);
        }

        [TestCase(null, null)]
        [TestCase(null, CatalogueItemType.Solution)]
        [TestCase(null, CatalogueItemType.AdditionalService)]
        [TestCase(null, CatalogueItemType.AssociatedService)]
        [TestCase(_supplierId1, null)]
        [TestCase(_supplierId1, null)]
        [TestCase(_supplierId1, null)]
        [TestCase(_supplierId2, CatalogueItemType.Solution)]
        [TestCase(_supplierId1, CatalogueItemType.AdditionalService)]
        [TestCase(_supplierId1, CatalogueItemType.AssociatedService)]
        public async Task ListAsync_NoFilter_ReturnsAllResults(string supplierId, CatalogueItemType? catalogueItemType)
        {
            var catalogueItemsEntity = new List<CatalogueItemEntity>
            {
                await CreateCatalogueItemEntity("100000-001", _supplierId1, (int)CatalogueItemType.Solution),
                await CreateCatalogueItemEntity("100000-002", _supplierId1, (int)CatalogueItemType.AdditionalService),
                await CreateCatalogueItemEntity("100000-003", _supplierId2, (int)CatalogueItemType.AssociatedService),
                await CreateCatalogueItemEntity("100000-004", _supplierId1, (int)CatalogueItemType.Solution)
            };

            var request = await _catalogueItemRepository.ListAsync(supplierId, catalogueItemType, CancellationToken.None);

            var filteredExpected = catalogueItemsEntity?.Where(x => (supplierId is null || x.SupplierId == supplierId) && (catalogueItemType is null || x.CatalogueItemTypeId == (int)catalogueItemType));

            var expected = filteredExpected.Select(x => new { x.CatalogueItemId, x.Name });

            request.Should().BeEquivalentTo(expected);
        }

        public async Task<CatalogueItemEntity> CreateCatalogueItemEntity(string catalogueItemId, string supplierId, int catalogueItemTypeId)
        {
            var catalogueItem = CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(catalogueItemId)
                .WithSupplierId(supplierId)
                .WithCatalogueItemTypeId(catalogueItemTypeId)
                .Build();

            await catalogueItem.InsertAsync();
            return catalogueItem;
        }
    }
}
