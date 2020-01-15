using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopAdditionalInformationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopAdditionalInformationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-desktop-additional-information")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAsync([FromRoute] [Required] string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetNativeDesktopAdditionalInformationResult(clientApplication?.NativeDesktopAdditionalInformation));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-additional-information")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateAdditionalInformationAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopAdditionalInformationViewModel viewModel) =>
            (await _mediator
                .Send(new UpdateNativeDesktopAdditionalInformationCommand(id, viewModel?.NativeDesktopAdditionalInformation))
                .ConfigureAwait(false)).ToActionResult();
    }
}
