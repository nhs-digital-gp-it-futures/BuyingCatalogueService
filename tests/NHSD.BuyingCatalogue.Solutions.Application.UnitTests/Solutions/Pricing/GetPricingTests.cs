using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private const string CatalogueItemId = "Sln1-A99";

        private static readonly Expression<Func<ICataloguePriceListResult, bool>> priceListResult1 = p =>
            p.CataloguePriceId == 0
            && p.CatalogueItemId == CatalogueItemId
            && p.CatalogueItemName == "name"
            && p.CataloguePriceTypeId == 1
            && p.ProvisioningTypeId == 1
            && p.PricingUnitName == "Pricing unit name"
            && p.PricingUnitDescription == "desc"
            && p.PricingUnitTierName == "tier"
            && p.TimeUnitId == 1
            && p.CurrencyCode == "GBP";

        private static readonly ICataloguePriceListResult price1 = Mock.Of(priceListResult1);

        private static readonly Expression<Func<ICataloguePriceListResult, bool>> priceLIstResult2 = p =>
            p.CataloguePriceId == 1
            && p.CatalogueItemId == "Sln1"
            && p.CatalogueItemName == "name"
            && p.CataloguePriceTypeId == 1
            && p.ProvisioningTypeId == 1
            && p.PricingUnitName == "Pricing unit name"
            && p.PricingUnitDescription == "desc"
            && p.PricingUnitTierName == "tier"
            && p.TimeUnitId == 1
            && p.CurrencyCode == "GBP";

        private static readonly ICataloguePriceListResult Price2 = Mock.Of(priceLIstResult2);

        private TestContext context;
        private CancellationToken cancellationToken;
        private List<ICataloguePriceListResult> cataloguePriceResult;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();
            cancellationToken = new CancellationToken();
            cataloguePriceResult = new List<ICataloguePriceListResult>();
        }

        [Test]
        public async Task GetPrices_NoPrices_ReturnEmptyObject()
        {
            var prices = await context.GetPricesHandler.Handle(
                new GetPricesQuery(CatalogueItemId),
                cancellationToken);

            prices.Should().BeEmpty();
        }

        [Test]
        public async Task GetPrices_ByCatalogueItemId_ReturnsPriceForThatCatalogueItem()
        {
            cataloguePriceResult.Add(price1);

            SetUpMockPriceRepository(CatalogueItemId);
            var prices = (await context.GetPricesHandler.Handle(
                new GetPricesQuery(CatalogueItemId),
                new CancellationToken())).ToList();

            prices.Count.Should().Be(1);
            var price = prices.First();

            price.Should().BeEquivalentTo(TransformResultToDto(price1));
        }

        [Test]
        public async Task GetPrices_Filter_ReturnsAllPrices()
        {
            cataloguePriceResult.Add(price1);
            cataloguePriceResult.Add(Price2);

            SetUpMockPriceRepository(null);

            var prices = (await context.GetPricesHandler.Handle(
                new GetPricesQuery(null),
                new CancellationToken())).ToList();

            prices.Count.Should().Be(2);

            var expectedPrices = cataloguePriceResult.Select(TransformResultToDto);
            prices.Should().BeEquivalentTo(expectedPrices);
        }

        private static object TransformResultToDto(ICataloguePriceListResult result)
        {
            return new
            {
                result.CatalogueItemId,
                result.CatalogueItemName,
                result.CataloguePriceId,
                result.CurrencyCode,
                Type = Enumerator.FromValue<CataloguePriceType>(result.CataloguePriceTypeId).Name,
                PricingUnit = new
                {
                    Name = result.PricingUnitName,
                    Description = result.PricingUnitDescription,
                    TierName = result.PricingUnitTierName,
                },
                TimeUnit = new
                {
                    Name = result.TimeUnitId is null ? null : Enumerator.FromValue<TimeUnit>(result.TimeUnitId.Value).Name,
                    Description = result.TimeUnitId is null ? null : Enumerator.FromValue<TimeUnit>(result.TimeUnitId.Value).Description,
                },
            };
        }

        private void SetUpMockPriceRepository(string filter)
        {
            context.MockPriceRepository
                .Setup(r => r.GetPricesAsync(filter, cancellationToken))
                .ReturnsAsync(() => cataloguePriceResult);
        }
    }
}
