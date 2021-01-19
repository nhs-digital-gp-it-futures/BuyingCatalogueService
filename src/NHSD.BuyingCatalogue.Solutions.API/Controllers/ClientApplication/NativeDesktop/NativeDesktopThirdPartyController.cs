using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class NativeDesktopThirdPartyController : ControllerBase
    {
        private readonly IMediator mediator;

        public NativeDesktopThirdPartyController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-third-party")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetThirdParty([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));

            return Ok(new GetNativeDesktopThirdPartyResult(clientApplication?.NativeDesktopThirdParty));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-third-party")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatedThirdParty(
            [Required] string id,
            UpdateNativeDesktopThirdPartyViewModel model) =>
            (await mediator.Send(new UpdateSolutionNativeDesktopThirdPartyCommand(id, model))).ToActionResult();
    }
}
