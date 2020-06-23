using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class PriceControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private PriceController _controller;

        private const string _solutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new PriceController(_mockMediator.Object);
        }

        [Test]
        public void Constructor_NullRepository_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new PriceController(null);
            });
        }

        [Test]
        public async Task ShouldGetSingleFlatPricing()
        {
            var flatPricing = FlatPriceBuilder.Create().Build();
            var cataloguePriceList = new List<ICataloguePrice> { flatPricing };

            var priceResult = CreatePrices(cataloguePriceList);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPriceBySolutionIdQuery>(q => q.SolutionId == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cataloguePriceList);

            var response = (await _controller.GetListAsync(_solutionId));
            response.Should().BeEquivalentTo(new ActionResult<PricingResult>(priceResult));
        }

        [Test]
        public async Task ShouldGetSingleTieredPricing()
        {
            var tieredPricing = TieredPriceBuilder.Create().Build();
            var cataloguePriceList = new List<ICataloguePrice> { tieredPricing };

            var priceResult = CreatePrices(cataloguePriceList);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPriceBySolutionIdQuery>(q => q.SolutionId == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cataloguePriceList);

            var response = (await _controller.GetListAsync(_solutionId));
            response.Should().BeEquivalentTo(new ActionResult<PricingResult>(priceResult));
        }

        [Test]
        public async Task ShouldGetMultipleFlatAndTieredPricing()
        {
            var flatPricing = FlatPriceBuilder.Create().Build();
            var tieredPricing = TieredPriceBuilder.Create().Build();
            var cataloguePriceList = new List<ICataloguePrice> { flatPricing, tieredPricing };

            var priceResult = CreatePrices(cataloguePriceList);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPriceBySolutionIdQuery>(q => q.SolutionId == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cataloguePriceList);

            var response = (await _controller.GetListAsync(_solutionId));
            response.Should().BeEquivalentTo(new ActionResult<PricingResult>(priceResult));
        }

        private static PricingResult CreatePrices(IEnumerable<ICataloguePrice> cataloguePrice)
        {
            var pricingResult = new PricingResult
            {
                Id = _solutionId,
                Name = "Item Name",
                Prices = cataloguePrice.Select(x => new PriceResult
                {
                    PriceId = x.CataloguePriceId,
                    Type = x.Type,
                    CurrencyCode = x.CurrencyCode,
                    ItemUnit = new ItemUnitResult
                    {
                        Name = x.PricingUnit.Name,
                        Description = x.PricingUnit.Description,
                        TierName = x.PricingUnit.TierName
                    },
                    TimeUnit = x.TimeUnit is null ? null : new TimeUnitResult
                    {
                        Name = x.TimeUnit.Name,
                        Description = x.TimeUnit.Description
                    },
                    Price = (x as FlatCataloguePriceDto)?.Price,
                    Tiers = (x as TieredCataloguePriceDto)?.TieredPrices.Select(x => new TierResult
                    {
                        Start = x.BandStart,
                        End = x.BandEnd,
                        Price = x.Price
                    })
                })
            };

            return pricingResult;
        }
    }
}
