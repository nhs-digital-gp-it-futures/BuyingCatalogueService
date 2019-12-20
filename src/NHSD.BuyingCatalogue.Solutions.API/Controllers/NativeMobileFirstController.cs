using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class NativeMobileFirstController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeMobileFirstController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetMobileFirstAsync([FromRoute] [Required] string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return clientApplication == null
                ? (ActionResult)new NotFoundResult()
                : Ok(new GetNativeMobileFirstResult(clientApplication));
        }

        [HttpPut]
        [Route("{id}/sections/mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateMobileFirstAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionNativeMobileFirstViewModel viewModel)
        {
            var validationResult = await _mediator.Send(new UpdateSolutionNativeMobileFirstCommand(id, viewModel))
                .ConfigureAwait(false);

            return validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : BadRequest(new UpdateSolutionNativeMobileFirstResult(validationResult));
        }
    }
}
