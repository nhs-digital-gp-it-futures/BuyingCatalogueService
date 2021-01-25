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
    internal sealed class PriceRepositoryTests
    {
        private const string SolutionId = "Sln1";
        private const string SupplierId = "Sup1";

        private IPriceRepository priceRepository;

        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(SolutionId)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(SolutionId)
                .Build()
                .InsertAsync();

            await InsertPriceAsync(SolutionId);

            TestContext testContext = new TestContext();
            priceRepository = testContext.PriceRepository;
        }

        [Test]
        public async Task GetPriceByPriceIdQueryAsync_InvalidPriceId_ReturnsEmptyList()
        {
            var request = await priceRepository.GetPriceByPriceIdQueryAsync(-999, CancellationToken.None);

            request.Should().BeEmpty();
        }

        [Test]
        public async Task GetPricingByPriceIdQueryAsync_ValidPriceIdAndFlatPrice_ReturnsPriceListResult()
        {
            int validPriceId = await InsertPriceAsync(SolutionId);

            var request = await priceRepository.GetPriceByPriceIdQueryAsync(validPriceId, CancellationToken.None);

            request.Count().Should().Be(1);
        }

        [Test]
        public async Task GetPricingByPriceIdQueryAsync_ValidPriceIdAndTiredPrice_ReturnsTieredPriceResult()
        {
            int validPriceId = await InsertTieredPriceAsync(SolutionId);
            await InsertCataloguePriceTier(validPriceId, 1, 2, 1.0m);
            await InsertCataloguePriceTier(validPriceId, 3, 4, 1.50m);
            var response = await priceRepository.GetPriceByPriceIdQueryAsync(validPriceId, CancellationToken.None);
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
            return priceId;
        }

        private static async Task InsertCataloguePriceTier(int priceId, int startBand, int? endBand, decimal price)
        {
            await CataloguePriceTierEntityBuilder.Create()
                .WithCataloguePriceId(priceId)
                .WithBandStart(startBand)
                .WithBandEnd(endBand)
                .WithPrice(price)
                .Build().InsertAsync();
        }
    }
}
