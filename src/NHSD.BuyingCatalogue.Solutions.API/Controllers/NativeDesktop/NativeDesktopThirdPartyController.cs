using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopThirdPartyController
    {
        private readonly IMediator _mediator;

        public NativeDesktopThirdPartyController(IMediator mediator) =>
            _mediator = mediator;

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
