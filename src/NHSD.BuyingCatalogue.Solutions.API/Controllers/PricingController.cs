using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PricingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PricingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{solutionId}/pricing")]
        public async Task<ActionResult<PricingResult>> Get(string solutionId)
        {
            var pricing = await _mediator.Send(new GetPricingBySolutionIdQuery(solutionId));

            var result = new PricingResult
            {
                Id = solutionId,
                Prices = pricing.Select(x => new PricesResult
                {
                    PriceId = x.CataloguePriceId,
                    CurrencyCode = x.CurrencyCode
                })
            };

            return Ok(result);
        }
    }
}
