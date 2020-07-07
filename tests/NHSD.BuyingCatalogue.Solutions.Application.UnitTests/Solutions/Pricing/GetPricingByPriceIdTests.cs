using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Pricing
{
    [TestFixture]
    internal sealed class GetPricingByPriceIdTests
    {
        private TestContext _context;
        private CancellationToken _cancellationToken;
        private List<ICataloguePriceListResult> _cataloguePriceResult;

        private const int PriceId = 1;

        private static readonly ICataloguePriceListResult Price1 =
            Mock.Of<ICataloguePriceListResult>(
                 p => p.CataloguePriceId == PriceId &&
                                                    p.CatalogueItemName == "name" &&
                                                    p.CatalogueItemId == "id" &&
                                                    p.CataloguePriceTypeId == 1 &&
                                                    p.ProvisioningTypeId == 1 &&
                                                    p.CataloguePriceTypeId == 1 &&
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

            _context.MockPriceRepository.Setup(r => r.GetPriceByPriceIdQueryAsync(PriceId, _cancellationToken))
                .ReturnsAsync(() => _cataloguePriceResult);

            _cataloguePriceResult = new List<ICataloguePriceListResult>();
        }

        [Test]
        public async Task PriceIdDoesNotExist_ReturnNull()
        {
            var prices = await _context.GetPricingByPriceIdHandler.Handle(new GetPriceByPriceIdQuery(PriceId),
                _cancellationToken);

            prices.Should().BeNull();
        }

        [Test]
        public async Task PriceIdExists_ReturnsResult()
        {
            _cataloguePriceResult.Add(Price1);

            var price = await _context.GetPricingByPriceIdHandler.Handle(new GetPriceByPriceIdQuery(PriceId),
                _cancellationToken);

            price.CataloguePriceId.Should().Be(PriceId);
            price.CatalogueItemId.Should().BeEquivalentTo(Price1.CatalogueItemId);
            price.CatalogueItemName.Should().BeEquivalentTo(Price1.CatalogueItemName);
            price.Type.Should().BeEquivalentTo(Enumerator.FromValue<CataloguePriceType>(Price1.CataloguePriceTypeId).Name);
            price.ProvisioningType.Should()
                .BeEquivalentTo(Enumerator.FromValue<ProvisioningType>(Price1.CataloguePriceTypeId).Name);
            price.PricingUnit.Name.Should().BeEquivalentTo(Price1.PricingUnitName);
            price.PricingUnit.Description.Should().BeEquivalentTo(Price1.PricingUnitDescription);
            price.PricingUnit.TierName.Should().BeEquivalentTo(Price1.PricingUnitTierName);
            price.TimeUnit.Name.Should().BeEquivalentTo(Enumerator.FromValue<TimeUnit>(Price1.TimeUnitId).Name);
            price.TimeUnit.Description.Should().BeEquivalentTo(Enumerator.FromValue<TimeUnit>(Price1.TimeUnitId).Description);
            price.CurrencyCode.Should().BeEquivalentTo(Price1.CurrencyCode);
        }
    }
}
