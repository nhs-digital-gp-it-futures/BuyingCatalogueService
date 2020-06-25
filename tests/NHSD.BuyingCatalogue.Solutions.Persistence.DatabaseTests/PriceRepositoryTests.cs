﻿using System.Linq;
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
    internal sealed class PriceRepositoryTests
    {
        private IPriceRepository _priceRepository;
        private int _priceId = -1;
        private const string _solutionId = "Sln1";
        private const string _supplierId = "Sup1";

        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .Build()
                .InsertAsync();
            
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(_solutionId)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(_solutionId)
                .Build()
                .InsertAsync();

            _priceId = await InsertPriceAsync(_solutionId);

            TestContext testContext = new TestContext();
            _priceRepository = testContext.PriceRepository;
        }

        [Test]
        public async Task GetPricesBySolutionIdQueryAsync_InvalidSolutionId_ReturnsEmptyList()
        {
            var request = await _priceRepository.GetPricesBySolutionIdQueryAsync("INVALID", CancellationToken.None);

            request.Count().Should().Be(0);
        }

        [Test]
        public async Task GetPricesBySolutionIdQueryAsync_ValidSolutionId_ReturnsPriceListResult()
        {
            await InsertPriceAsync(_solutionId);

            var request = await _priceRepository.GetPricesBySolutionIdQueryAsync(_solutionId, CancellationToken.None);

            request.Count().Should().Be(2);
        }

        [Test]
        public async Task GetPricingByPriceIdQueryAsync_InvalidPriceId_ReturnsEmptyList()
        {
            var request = await _priceRepository.GetPriceByPriceIdQueryAsync(-999, CancellationToken.None);

            request.Should().BeEmpty();
        }

        [Test]
        public async Task GetPricingByPriceIdQueryAsync_ValidPriceIdAndFlatPrice_ReturnsPriceListResult()
        {
            int validPriceId = await InsertPriceAsync(_solutionId);

            var request = await _priceRepository.GetPriceByPriceIdQueryAsync(validPriceId, CancellationToken.None);

            request.Count().Should().Be(1);
        }

        [Test]
        public async Task GetPricingByPriceIdQueryAsync_ValidPriceIdAndTiredPrice_ReturnsTieredPriceResult()
        {
            int validPriceId = await InsertTieredPriceAsync(_solutionId);

            var response = await _priceRepository.GetPriceByPriceIdQueryAsync(validPriceId, CancellationToken.None);
            response.Count(t => t.CataloguePriceTypeId == 2).Should().Be(2);
        }

        private static async Task<int> InsertPriceAsync(string solutionId)
        {
            return await CataloguePriceEntityBuilder.Create()
                .WithCatalogueItemId(solutionId)
                .Build().InsertAsync<int>();
        }

        private static async Task<int> InsertTieredPriceAsync(string solutionId)
        {
            var priceId = await CataloguePriceEntityBuilder.Create()
                .WithPriceTypeId(2)
                .WithCatalogueItemId(solutionId)
                .Build().InsertAsync<int>();
            await CataloguePriceTierEntityBuilder.Create()
                .WithCataloguePriceId(priceId)
                .WithBandEnd(1)
                .WithBandStart(2)
                .WithPrice(1.0m)
                .Build().InsertAsync();
            await CataloguePriceTierEntityBuilder.Create()
                .WithCataloguePriceId(priceId)
                .WithBandEnd(3)
                .WithBandStart(4)
                .WithPrice(1.50m)
                .Build().InsertAsync();
            return priceId;
        }
    }
}
