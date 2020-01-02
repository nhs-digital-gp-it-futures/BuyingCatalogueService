using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeMobile
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class HardwareRequirementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HardwareRequirementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-mobile-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetHardwareRequirements([FromRoute][Required] string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetHardwareRequirementsResult(clientApplication?.NativeMobileHardwareRequirements));
        }

        [HttpPut]
        [Route("{id}/sections/native-mobile-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateHardwareRequirements([FromRoute] [Required] string id, [FromBody] [Required] UpdateHardwareRequirementsRequest viewModel) =>
            (await _mediator.Send(new UpdateSolutionNativeMobileHardwareRequirementsCommand(id, viewModel.ThrowIfNull().HardwareRequirements)).ConfigureAwait(false)).ToActionResult();
    }
}
