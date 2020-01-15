using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopConnectivityDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopConnectivityDetailsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetConnectivity([FromRoute] [Required] string id)
        {
            var clientApplication =
                await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetNativeDesktopConnectivityDetailsResult(clientApplication?.NativeDesktopMinimumConnectionSpeed));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdatedConnectivity([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopConnectivityDetailsViewModel viewModel)
        {
            return (await _mediator
                .Send(new UpdateSolutionNativeDesktopConnectivityDetailsCommand(id,
                    viewModel?.NativeDesktopMinimumConnectionSpeed)).ConfigureAwait(false)).ToActionResult();
        }
    }
}
