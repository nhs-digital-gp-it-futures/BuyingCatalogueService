using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class OnPremiseHostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OnPremiseHostingController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/hosting-type-on-premise")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get([FromRoute] [Required] string id)
        {
            var hosting =
                await _mediator.Send(new GetHostingBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetOnPremiseResult(hosting?.OnPremise));
        }

        [HttpPut]
        [Route("{id}/sections/hosting-type-on-premise")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute] [Required] string id, [FromBody] [Required] UpdateOnPremiseViewModel viewModel) =>
            (await _mediator.Send(new UpdateOnPremiseCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();

    }
}
