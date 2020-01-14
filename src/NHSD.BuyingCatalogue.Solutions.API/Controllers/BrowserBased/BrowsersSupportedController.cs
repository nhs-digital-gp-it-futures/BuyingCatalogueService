using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowsersSupportedController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedController"/> class.
        /// </summary>
        public BrowsersSupportedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the browsers supported for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the browsers supported section.</returns>
        [HttpGet]
        [Route("{id}/sections/browser-browsers-supported")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBrowsersSupportedAsync([FromRoute][Required]string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetBrowsersSupportedResult(clientApplication));
        }

        /// <summary>
        /// Updates the browsers supported of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="viewModel">The details of the supported browsers.</param>
        /// <returns>A task representing an operation to update the details of the browser supported section.</returns>
        [HttpPut]
        [Route("{id}/sections/browser-browsers-supported")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateBrowsersSupportedAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateBrowserBasedBrowsersSupportedViewModel viewModel) =>
            (await _mediator.Send(new UpdateSolutionBrowsersSupportedCommand(id, viewModel?.Trim())).ConfigureAwait(false))
            .ToActionResult();
    }
}
