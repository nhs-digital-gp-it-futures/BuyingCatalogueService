using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/pricing")]
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
        public ActionResult<PriceResult> GetPrice(int priceId)
        {
            return new PriceResult
            {
                PriceId = priceId,
                Type = "flat",
                CurrencyCode = "GBP",
                ProvisioningModel = "OnDemand",
                ItemUnit = new ItemUnitResult { Name = "Consultation", Description = "Per Consultation" },
                Price = 1.64m
            };
        }
        
        [HttpGet]
        [Route("/api/v1/solutions/{solutionId}/prices")]
        public async Task<ActionResult<PricingResult>> GetListAsync(string solutionId)
        {
            var pricing = (await _mediator.Send(new GetPriceBySolutionIdQuery(solutionId))).ToList();

            var result = new PricingResult
            {
                Id = solutionId,
                Name = pricing.First()?.CatalogueItemName,
                Prices = pricing.Select(x => new PriceResult
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

            return result;
        }
    }
}
