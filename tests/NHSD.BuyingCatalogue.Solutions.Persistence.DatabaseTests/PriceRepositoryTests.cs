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
        private IPriceRepository _priceRepository;


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

            await InsertPriceAsync(_solutionId);

            TestContext testContext = new TestContext();
            _priceRepository = testContext.PriceRepository;
        }

        [Test]
        public async Task GetPricingBySolutionIdQueryAsync_InvalidSolutionId_ReturnsEmptyList()
        {
            var request = await _priceRepository.GetPricingBySolutionIdQueryAsync("INVALID", CancellationToken.None);

            request.Count().Should().Be(0);
        }

        [Test]
        public async Task GetPricingBySolutionIdQueryAsync_ValidSolutionId_ReturnsPriceListResult()
        {
            await InsertPriceAsync(_solutionId);

            var request = await _priceRepository.GetPricingBySolutionIdQueryAsync(_solutionId, CancellationToken.None);

            request.Count().Should().Be(2);
        }

        private static async Task InsertPriceAsync(string solutionId)
        {
            await CataloguePriceEntityBuilder.Create()
                .WithCatalogueItemId(solutionId)
                .Build().InsertAsync();
        }
    }
}
