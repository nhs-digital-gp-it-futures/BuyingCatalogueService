using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class NativeDesktopConnectivityDetailsController : ControllerBase
    {
        private readonly IMediator mediator;

        public NativeDesktopConnectivityDetailsController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-connection-details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetConnectivity([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));

            return Ok(new GetNativeDesktopConnectivityDetailsResult(clientApplication?.NativeDesktopMinimumConnectionSpeed));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-connection-details")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatedConnectivity(
            [Required] string id,
            UpdateNativeDesktopConnectivityDetailsViewModel model)
        {
            return (await mediator.Send(new UpdateSolutionNativeDesktopConnectivityDetailsCommand(
                id,
                model?.NativeDesktopMinimumConnectionSpeed))).ToActionResult();
        }
    }
}
