using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Pricing
{
    [TestFixture]
    internal sealed class GetPricingBySolutionIdTests
    {
        private TestContext _context;

        private const string SolutionId = "Sln1";
        private CancellationToken _cancellationToken;
        private List<ICataloguePriceListResult> _cataloguePriceResult;
        private bool _solutionExists;

        private readonly static ICataloguePriceListResult Price1 =
            Mock.Of<ICataloguePriceListResult>(p => p.CatalogueItemId == SolutionId &&
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

            _solutionExists = true;

            _context.MockSolutionRepository
                .Setup(r => r.CheckExists(SolutionId, _cancellationToken)).ReturnsAsync(() => _solutionExists);

            _context.MockPriceRepository
                .Setup(r => r.GetPricesBySolutionIdQueryAsync(SolutionId, _cancellationToken))
                .ReturnsAsync(() => _cataloguePriceResult);

            _cataloguePriceResult = new List<ICataloguePriceListResult>();
        }

        [Test]
        public async Task RecordDoesNotExist_ReturnEmptyObject()
        {
            var prices = await _context.GetPriceBySolutionIdHandler.Handle(new GetPriceBySolutionIdQuery(SolutionId),
                _cancellationToken);

            prices.Should().BeEmpty();
        }

        [Test]
        public async Task GetListPrices()
        {
            _cataloguePriceResult.Add(Price1);

            var prices = (await _context.GetPriceBySolutionIdHandler.Handle(new GetPriceBySolutionIdQuery(SolutionId),
                new CancellationToken())).ToList();

            prices.Count.Should().Be(1);
            var price = prices.FirstOrDefault();

            price.CatalogueItemId.Should().Be(SolutionId);
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
