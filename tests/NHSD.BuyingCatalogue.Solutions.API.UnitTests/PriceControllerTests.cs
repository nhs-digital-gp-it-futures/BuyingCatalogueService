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
        private const int _priceId = 1;

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
        public async Task GetPriceAsync_PriceIdDoesNotExist_ReturnNotFound()
        {
            var response = await _controller.GetPriceAsync(-1);
            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetPriceAsync_HasFlatPrice_RetrievesPricing()
        {
            var flatPricing = FlatCataloguePriceDtoBuilder.Create().Build();

            var priceResult = CreatePrice(flatPricing);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPriceByPriceIdQuery>(q => q.PriceId == _priceId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(flatPricing);

            var response = await _controller.GetPriceAsync(_priceId);
            response.Value.Should().BeEquivalentTo(priceResult);
        }

        [Test]
        public async Task GetPriceAsync_HasTieredPricing_RetrievesPricing()
        {
            var tieredPricing = TieredCataloguePriceDtoBuilder.Create().Build();

            var priceResult = CreatePrice(tieredPricing);
            
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPriceByPriceIdQuery>(q => q.PriceId == _priceId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tieredPricing);

            var response = await _controller.GetPriceAsync(_priceId);
            response.Value.Should().BeEquivalentTo(priceResult);
        }

        [Test]
        public async Task GetListByCatalogueItemId_HasSingleFlatPricing_RetrievesPricing()
        {
            var flatPricing = FlatCataloguePriceDtoBuilder.Create().Build();
            var cataloguePriceList = new List<ICataloguePrice> { flatPricing };

            var priceResult = CreatePricesForQueryingByCatalogueItemId(cataloguePriceList);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPricesQuery>(q => q.CatalogueItemId == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cataloguePriceList);

            var response = (await _controller.GetPricesAsync(_solutionId));
            response.Value.Should().BeEquivalentTo(priceResult);
        }

        [Test]
        public async Task GetListByCatalogueItemId_HasSingleTieredPricing_RetrievesPricing()
        {
            var tieredPricing = TieredCataloguePriceDtoBuilder.Create().Build();
            var cataloguePriceList = new List<ICataloguePrice> { tieredPricing };

            var priceResult = CreatePricesForQueryingByCatalogueItemId(cataloguePriceList);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPricesQuery>(q => q.CatalogueItemId == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cataloguePriceList);

            var response = (await _controller.GetPricesAsync(_solutionId));
            response.Value.Should().BeEquivalentTo(priceResult);
        }

        [Test]
        public async Task GetListByCatalogueItemId_HasMultipleFlatAndTieredPricing_RetrievesPricing()
        {
            var flatPricing = FlatCataloguePriceDtoBuilder.Create().Build();
            var tieredPricing = TieredCataloguePriceDtoBuilder.Create().Build();
            var cataloguePriceList = new List<ICataloguePrice> { flatPricing, tieredPricing };

            var priceResult = CreatePricesForQueryingByCatalogueItemId(cataloguePriceList);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetPricesQuery>(q => q.CatalogueItemId == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cataloguePriceList);

            var response = await _controller.GetPricesAsync(_solutionId);
            response.Value.Should().BeEquivalentTo(priceResult);
        }

        private static PriceResult CreatePrice(ICataloguePrice cataloguePrice)
        {
            return new()
            {
                PriceId = cataloguePrice.CataloguePriceId,
                Type = cataloguePrice.Type,
                CurrencyCode = cataloguePrice.CurrencyCode,
                ItemUnit =
                    new ItemUnitResult
                    {
                        Name = cataloguePrice.PricingUnit.Name,
                        Description = cataloguePrice.PricingUnit.Description,
                        TierName = cataloguePrice.PricingUnit.TierName
                    },
                TimeUnit =
                    cataloguePrice.TimeUnit is null
                        ? null
                        : new TimeUnitResult
                        {
                            Name = cataloguePrice.TimeUnit.Name,
                            Description = cataloguePrice.TimeUnit.Description
                        },
                Price = (cataloguePrice as FlatCataloguePriceDto)?.Price,
                Tiers = (cataloguePrice as TieredCataloguePriceDto)?.TieredPrices.Select(x => new TierResult
                {
                    Start = x.BandStart,
                    End = x.BandEnd,
                    Price = x.Price
                })
            };
        }

        private static PricingResult CreatePrices(IEnumerable<ICataloguePrice> cataloguePrice)
        {
            var pricingResult = new PricingResult
            {
                Id = _solutionId,
                Name = "Item Name",
                Prices = cataloguePrice.Select(CreatePrice)
            };

            return pricingResult;
        }

        private static PricingResult CreatePricesForQueryingByCatalogueItemId(IEnumerable<ICataloguePrice> cataloguePrice)
        => new()
            { Prices = cataloguePrice.Select(CreatePrice) };
    }
}
