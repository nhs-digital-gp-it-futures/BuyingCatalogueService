using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class ImplementationTimescalesController : ControllerBase
    {
        private readonly IMediator mediator;

        public ImplementationTimescalesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the implementation timescales section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the implementation timescales section of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/implementation-timescales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var implementationTimescales = await mediator.Send(new GetImplementationTimescalesBySolutionIdQuery(id));
            return Ok(new ImplementationTimescalesResult(implementationTimescales?.Description));
        }

        /// <summary>
        /// Updates the implementation timescales details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of the implementation timescales.</param>
        /// <returns>A task representing an operation to update the details of the implementation timescales section.</returns>
        [HttpPut]
        [Route("{id}/sections/implementation-timescales")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateImplementationTimescalesViewModel model) =>
            (await mediator.Send(new UpdateImplementationTimescalesCommand(id, model?.Description))).ToActionResult();
    }
}
