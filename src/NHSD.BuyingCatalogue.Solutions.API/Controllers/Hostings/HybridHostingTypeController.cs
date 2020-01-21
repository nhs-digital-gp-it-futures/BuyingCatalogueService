using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.HybridHostingType;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class HybridHostingTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HybridHostingTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/hosting-type-hybrid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromRoute] [Required] string id)
        {
            var hosting = await _mediator.Send(new GetHostingBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetHybridHostingTypeResult(hosting?.HybridHostingType));
        }

        [HttpPut]
        [Route("{id}/sections/hosting-type-hybrid")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute] [Required] string id, [FromBody] [Required] UpdateHybridHostingTypeViewModel viewModel) =>
            (await _mediator.Send(new UpdateHybridHostingTypeCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();
    }
}
