using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowserHardwareRequirementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrowserHardwareRequirementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetHardwareRequirementsAsync([FromRoute][Required]string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetBrowserHardwareRequirementsResult(clientApplication?.HardwareRequirements));
        }

        [HttpPut]
        [Route("{id}/sections/browser-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateHardwareRequirementsAsync([FromRoute][Required]string id, [FromBody][Required] UpdateSolutionBrowserHardwareRequirementsViewModel updateSolutionHardwareRequirementsViewModel) =>
            (await _mediator
                .Send(new UpdateSolutionBrowserHardwareRequirementsCommand(id, updateSolutionHardwareRequirementsViewModel))
                .ConfigureAwait(false)).ToActionResult();
    }
}
