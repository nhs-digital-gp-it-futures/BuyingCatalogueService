using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class HybridHostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HybridHostingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/hosting-type-hybrid")]
        public async Task<ActionResult> GetHostingTypeHybrid([FromRoute] [Required] string id)
        {
            var hosting = await _mediator.Send(new GetHostingBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetHostingTypeHybridResult(hosting?.HostingTypeHybrid));

        }
    }
}
