using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class BrowserHardwareRequirementsController : ControllerBase
    {
        private readonly IMediator mediator;

        public BrowserHardwareRequirementsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-hardware-requirements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHardwareRequirementsAsync([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));
            return Ok(new GetBrowserHardwareRequirementsResult(clientApplication?.HardwareRequirements));
        }

        [HttpPut]
        [Route("{id}/sections/browser-hardware-requirements")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateHardwareRequirementsAsync(
            [Required] string id,
            UpdateBrowserBasedHardwareRequirementViewModel model) =>
            (await mediator.Send(new UpdateSolutionBrowserHardwareRequirementsCommand(
                id,
                model?.HardwareRequirements))).ToActionResult();
    }
}
