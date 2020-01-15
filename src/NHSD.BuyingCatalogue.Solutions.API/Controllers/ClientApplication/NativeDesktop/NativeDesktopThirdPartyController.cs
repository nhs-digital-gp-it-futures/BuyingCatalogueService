using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopThirdPartyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopThirdPartyController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-third-party")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetThirdParty([FromRoute] [Required] string id)
        {
            var clientApplication =
                await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetNativeDesktopThirdPartyResult(clientApplication?.NativeDesktopThirdParty));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-third-party")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdatedThirdParty([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopThirdPartyViewModel viewModel) =>
            (await _mediator.Send(new UpdateSolutionNativeDesktopThirdPartyCommand(id,
                viewModel)).ConfigureAwait(false)).ToActionResult();
    }
}
