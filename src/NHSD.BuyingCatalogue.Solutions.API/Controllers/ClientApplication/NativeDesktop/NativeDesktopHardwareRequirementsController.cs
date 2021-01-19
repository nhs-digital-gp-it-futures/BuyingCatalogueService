using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class NativeDesktopHardwareRequirementsController : ControllerBase
    {
        private readonly IMediator mediator;

        public NativeDesktopHardwareRequirementsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{id}/sections/native-desktop-hardware-requirements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHardwareRequirements([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));
            return Ok(new GetNativeDesktopHardwareRequirementsResult(clientApplication?.NativeDesktopHardwareRequirements));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-hardware-requirements")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateHardwareRequirements(
            [Required] string id,
            UpdateNativeDesktopHardwareRequirementsViewModel model)
        {
            return (await mediator.Send(new UpdateNativeDesktopHardwareRequirementsCommand(
                id,
                model?.HardwareRequirements))).ToActionResult();
        }
    }
}
