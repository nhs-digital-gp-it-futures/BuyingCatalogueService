using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopHardwareRequirementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopHardwareRequirementsController(IMediator mediator)
        {
            mediator.ThrowIfNull();
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-desktop-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetHardwareRequirements([FromRoute][Required] string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetNativeDesktopHardwareRequirementsResult(clientApplication?.NativeDesktopHardwareRequirements));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateHardwareRequirements([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopHardwareRequirementsViewModel viewModel)
        {
            return (await _mediator.Send(
                    new UpdateNativeDesktopHardwareRequirementsCommand(id, viewModel?.HardwareRequirements))
                .ConfigureAwait(false)).ToActionResult();
        }
    }
}
