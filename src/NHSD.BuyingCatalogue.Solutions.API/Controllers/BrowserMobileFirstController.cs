using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowserMobileFirstController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrowserMobileFirstController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetMobileFirstAsync([FromRoute] [Required] string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetBrowserMobileFirstResult(clientApplication));
        }

        [HttpPut]
        [Route("{id}/sections/browser-mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateMobileFirstAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionBrowserMobileFirstViewModel viewModel) =>
            (await _mediator.Send(new UpdateSolutionBrowserMobileFirstCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();
    }
}
