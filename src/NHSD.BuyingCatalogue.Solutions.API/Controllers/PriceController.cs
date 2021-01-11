using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/prices")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PriceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PriceController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{priceId}")]
        public async Task<ActionResult<PriceResult>> GetPriceAsync(int priceId)
        {
            var pricing = await _mediator.Send(new GetPriceByPriceIdQuery(priceId));
            if (pricing is null)
                return NotFound();

            var result = GetPriceResult(pricing);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<PricingResult>> GetPricesAsync(string catalogueItemId)
        {
            var prices = (await _mediator.Send(new GetPricesQuery(catalogueItemId))).ToList();

            return new PricingResult { Prices = prices.Select(GetPriceResult) };
        }

        private static PriceResult GetPriceResult(ICataloguePrice cataloguePrice)
        {
            return new()
            {
                PriceId = cataloguePrice.CataloguePriceId,
                Type = cataloguePrice.Type,
                ProvisioningType = cataloguePrice.ProvisioningType,
                CurrencyCode = cataloguePrice.CurrencyCode,
                ItemUnit = new ItemUnitResult
                {
                    Name = cataloguePrice.PricingUnit.Name,
                    Description = cataloguePrice.PricingUnit.Description,
                    TierName = cataloguePrice.PricingUnit.TierName
                },
                TimeUnit = cataloguePrice.TimeUnit is null ? null : new TimeUnitResult
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
    }
}
