using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class IntegrationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public IntegrationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the integrations section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the integrations section of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/integrations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var integrations = await mediator.Send(new GetIntegrationsBySolutionIdQuery(id));
            return Ok(new IntegrationsResult(integrations?.Url));
        }

        /// <summary>
        /// Updates the integrations details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of the integrations.</param>
        /// <returns>A task representing an operation to update the details of the integrations section.</returns>
        [HttpPut]
        [Route("{id}/sections/integrations")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateIntegrationsViewModel model) =>
            (await mediator.Send(new UpdateIntegrationsCommand(id, model?.Url))).ToActionResult();
    }
}
