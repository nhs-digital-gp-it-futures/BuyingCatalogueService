using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings
{
    /// <summary>
    /// Provides an endpoint to manage the private cloud hosting details for a given solution.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class PrivateCloudHostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrivateCloudHostingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/hosting-type-private-cloud")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get([FromRoute] [Required] string id)
        {
            var hosting =
                await _mediator.Send(new GetHostingBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetPrivateCloudResult(hosting?.PrivateCloud));
        }

        [HttpPut]
        [Route("{id}/sections/hosting-type-private-cloud")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute] [Required] string id, [FromBody] [Required] UpdatePrivateCloudViewModel viewModel) =>
            (await _mediator.Send(new UpdatePrivateCloudCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();
    }
}
