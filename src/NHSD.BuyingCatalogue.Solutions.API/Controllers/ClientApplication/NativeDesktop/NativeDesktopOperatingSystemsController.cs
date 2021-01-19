using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class NativeDesktopOperatingSystemsController : ControllerBase
    {
        private readonly IMediator mediator;

        public NativeDesktopOperatingSystemsController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-operating-systems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSupportedOperatingSystems([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));

            return Ok(new GetNativeDesktopOperatingSystemsResult(clientApplication?.NativeDesktopOperatingSystemsDescription));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-operating-systems")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatedSupportedOperatingSystems(
            [Required] string id,
            UpdateNativeDesktopOperatingSystemsViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return (await mediator.Send(new UpdateSolutionNativeDesktopOperatingSystemsCommand(
                id,
                model.NativeDesktopOperatingSystemsDescription))).ToActionResult();
        }
    }
}
