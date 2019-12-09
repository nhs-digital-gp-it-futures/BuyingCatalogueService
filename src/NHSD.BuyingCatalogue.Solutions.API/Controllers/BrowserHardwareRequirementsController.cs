using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
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
            var solution = await _mediator.Send(new GetSolutionByIdQuery(id)).ConfigureAwait(false);
            return solution == null ? (ActionResult)new NotFoundResult() : Ok(new GetBrowserHardwareRequirementsResult(solution.ClientApplication.HardwareRequirements));
        }

        [HttpPut]
        [Route("{id}/sections/browser-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdateHardwareRequirementsAsync([FromRoute][Required]string id, [FromBody][Required] UpdateSolutionHardwareRequirementsViewModel updateSolutionHardwareRequirementsViewModel)
        {
            _cannedData[id] = updateSolutionHardwareRequirementsViewModel.ThrowIfNull().HardwareRequirements;
            return NoContent();
        }

        //canned data

        private static readonly Dictionary<string, string> _cannedData = new Dictionary<string, string>();

    }
}
