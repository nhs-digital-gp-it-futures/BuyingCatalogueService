using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PlugInsController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlugInsController"/> class.
        /// </summary>
        /// <param name="mediator"> An <see cref="IMediator"/> instance.</param>
        public PlugInsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the plug ins for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the plug ins section.</returns>
        [HttpGet]
        [Route("{id}/sections/browser-plug-ins-or-extensions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPlugInsAsync([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));
            return Ok(new GetPlugInsResult(clientApplication?.Plugins));
        }

        /// <summary>
        /// Updates the plug ins of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of the plug ins.</param>
        /// <returns>A task representing an operation to update the details of the plug ins section.</returns>
        [HttpPut]
        [Route("{id}/sections/browser-plug-ins-or-extensions")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePlugInsAsync([Required] string id, UpdateBrowserBasedPluginsViewModel model) =>
            (await mediator.Send(new UpdateSolutionPluginsCommand(id, model))).ToActionResult();
    }
}
