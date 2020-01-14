using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ConnectivityAndResolutionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConnectivityAndResolutionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-connectivity-and-resolution")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetConnectivityAndResolution([FromRoute][Required]string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetConnectivityAndResolutionResult(clientApplication?.MinimumConnectionSpeed, clientApplication?.MinimumDesktopResolution));
        }

        [HttpPut]
        [Route("{id}/sections/browser-connectivity-and-resolution")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateConnectivityAndResolutionAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateBrowserBasedConnectivityAndResolutionViewModel viewModel) =>
            (await _mediator.Send(new UpdateSolutionConnectivityAndResolutionCommand(id, viewModel))
                .ConfigureAwait(false)).ToActionResult();
    }
}
