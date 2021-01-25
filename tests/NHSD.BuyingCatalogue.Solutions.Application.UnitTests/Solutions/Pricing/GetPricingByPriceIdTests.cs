using System;
using System.Collections.Generic;
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
    internal sealed class GetPricingByPriceIdTests
    {
        private const int PriceId = 1;

        private static readonly Expression<Func<ICataloguePriceListResult, bool>> priceListResult = p =>
            p.CataloguePriceId == PriceId
            && p.CatalogueItemName == "name"
            && p.CatalogueItemId == "id"
            && p.CataloguePriceTypeId == 1
            && p.ProvisioningTypeId == 1
            && p.CataloguePriceTypeId == 1
            && p.PricingUnitName == "Pricing unit name"
            && p.PricingUnitDescription == "desc"
            && p.PricingUnitTierName == "tier"
            && p.TimeUnitId == 1
            && p.CurrencyCode == "GBP";

        private static readonly ICataloguePriceListResult price1 = Mock.Of(priceListResult);

        private TestContext context;
        private CancellationToken cancellationToken;
        private List<ICataloguePriceListResult> cataloguePriceResult;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();
            cancellationToken = new CancellationToken();

            context.MockPriceRepository
                .Setup(r => r.GetPriceByPriceIdQueryAsync(PriceId, cancellationToken))
                .ReturnsAsync(() => cataloguePriceResult);

            cataloguePriceResult = new List<ICataloguePriceListResult>();
        }

        [Test]
        public async Task PriceIdDoesNotExist_ReturnNull()
        {
            var prices = await context.GetPricingByPriceIdHandler.Handle(
                new GetPriceByPriceIdQuery(PriceId),
                cancellationToken);

            prices.Should().BeNull();
        }

        [Test]
        public async Task PriceIdExists_ReturnsResult()
        {
            cataloguePriceResult.Add(price1);

            var price = await context.GetPricingByPriceIdHandler.Handle(
                new GetPriceByPriceIdQuery(PriceId),
                cancellationToken);

            price.CataloguePriceId.Should().Be(PriceId);
            price.CatalogueItemId.Should().BeEquivalentTo(price1.CatalogueItemId);
            price.CatalogueItemName.Should().BeEquivalentTo(price1.CatalogueItemName);
            price.Type.Should().BeEquivalentTo(
                Enumerator.FromValue<CataloguePriceType>(price1.CataloguePriceTypeId).Name);

            price.ProvisioningType.Should().BeEquivalentTo(
                Enumerator.FromValue<ProvisioningType>(price1.CataloguePriceTypeId).Name);

            price.PricingUnit.Name.Should().BeEquivalentTo(price1.PricingUnitName);
            price.PricingUnit.Description.Should().BeEquivalentTo(price1.PricingUnitDescription);
            price.PricingUnit.TierName.Should().BeEquivalentTo(price1.PricingUnitTierName);
            price.TimeUnit.Name.Should().BeEquivalentTo(
                price1.TimeUnitId is null ? null : Enumerator.FromValue<TimeUnit>(price1.TimeUnitId.Value).Name);

            price.TimeUnit.Description.Should().BeEquivalentTo(
                price1.TimeUnitId is null ? null : Enumerator.FromValue<TimeUnit>(price1.TimeUnitId.Value).Description);

            price.CurrencyCode.Should().BeEquivalentTo(price1.CurrencyCode);
        }
    }
}
