using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Pricing
{
    [TestFixture]
    internal sealed class GetPricingTests
    {
        private TestContext _context;

        private const string CatalogueItemId = "Sln1-A99";
        private CancellationToken _cancellationToken;
        private List<ICataloguePriceListResult> _cataloguePriceResult;

        private static readonly ICataloguePriceListResult Price1 =
            Mock.Of<ICataloguePriceListResult>(p =>
                p.CataloguePriceId == 0 &&
                p.CatalogueItemId == CatalogueItemId &&
                p.CatalogueItemName == "name" &&
                p.CataloguePriceTypeId == 1 &&
                p.ProvisioningTypeId == 1 &&
                p.PricingUnitName == "Pricing unit name" &&
                p.PricingUnitDescription == "desc" &&
                p.PricingUnitTierName == "tier" &&
                p.TimeUnitId == 1 &&
                p.CurrencyCode == "GBP");

        private static readonly ICataloguePriceListResult Price2 =
            Mock.Of<ICataloguePriceListResult>(p =>
                p.CataloguePriceId == 1 &&
                p.CatalogueItemId == "Sln1" &&
                p.CatalogueItemName == "name" &&
                p.CataloguePriceTypeId == 1 &&
                p.ProvisioningTypeId == 1 &&
                p.PricingUnitName == "Pricing unit name" &&
                p.PricingUnitDescription == "desc" &&
                p.PricingUnitTierName == "tier" &&
                p.TimeUnitId == 1 &&
                p.CurrencyCode == "GBP");

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _cancellationToken = new CancellationToken();
            _cataloguePriceResult = new List<ICataloguePriceListResult>();
        }

        [Test]
        public async Task GetPrices_NoPrices_ReturnEmptyObject()
        {
            var prices = await _context.GetPricesHandler.Handle(
                new GetPricesQuery(CatalogueItemId),
                _cancellationToken);

            prices.Should().BeEmpty();
        }

        [Test]
        public async Task GetPrices_ByCatalogueItemId_ReturnsPriceForThatCatalogueItem()
        {
            _cataloguePriceResult.Add(Price1);

            SetUpMockPriceRepository(CatalogueItemId);
            var prices = (await _context.GetPricesHandler.Handle(
                new GetPricesQuery(CatalogueItemId),
                new CancellationToken())).ToList();

            prices.Count.Should().Be(1);
            var price = prices.First();

            price.CatalogueItemId.Should().BeEquivalentTo(Price1.CatalogueItemId);
            price.CatalogueItemName.Should().BeEquivalentTo(Price1.CatalogueItemName);
            price.Type.Should()
                .BeEquivalentTo(Enumerator.FromValue<CataloguePriceType>(Price1.CataloguePriceTypeId).Name);
            price.ProvisioningType.Should()
                .BeEquivalentTo(Enumerator.FromValue<ProvisioningType>(Price1.CataloguePriceTypeId).Name);
            price.PricingUnit.Name.Should().BeEquivalentTo(Price1.PricingUnitName);
            price.PricingUnit.Description.Should().BeEquivalentTo(Price1.PricingUnitDescription);
            price.PricingUnit.TierName.Should().BeEquivalentTo(Price1.PricingUnitTierName);
            price.TimeUnit.Name.Should().BeEquivalentTo(Enumerator.FromValue<TimeUnit>(Price1.TimeUnitId).Name);
            price.TimeUnit.Description.Should()
                .BeEquivalentTo(Enumerator.FromValue<TimeUnit>(Price1.TimeUnitId).Description);
            price.CurrencyCode.Should().BeEquivalentTo(Price1.CurrencyCode);
        }

        [Test]
        public async Task GetPrices_Filter_ReturnsAllPrices()
        {
            _cataloguePriceResult.Add(Price1);
            _cataloguePriceResult.Add(Price2);

            SetUpMockPriceRepository(null);

            var prices = (await _context.GetPricesHandler.Handle(
                new GetPricesQuery(null),
                new CancellationToken())).ToList();

            prices.Count.Should().Be(2);
        }

        private void SetUpMockPriceRepository(string filter)
        {
            _context.MockPriceRepository
                .Setup(r => r.GetPricesAsync(filter, _cancellationToken))
                .ReturnsAsync(() => _cataloguePriceResult);
        }
    }
}
