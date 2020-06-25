using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Swagger doesn't allow static functions. Suppression will be removed when the proper implementation is added")]
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
            var result = GetPriceResult(pricing);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet]
        [Route("/api/v1/solutions/{solutionId}/prices")]
        public async Task<ActionResult<PricingResult>> GetListAsync(string solutionId)
        {
            var prices = (await _mediator.Send(new GetPriceBySolutionIdQuery(solutionId))).ToList();

            var result = new PricingResult
            {
                Id = solutionId,
                Name = prices.First()?.CatalogueItemName,
                Prices = prices.Select(GetPriceResult)
            };

            return result;
        }

        private static PriceResult GetPriceResult(ICataloguePrice cataloguePrice)
        {
            return new PriceResult
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
