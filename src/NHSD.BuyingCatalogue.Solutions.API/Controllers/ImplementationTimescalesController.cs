using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ImplementationTimescalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImplementationTimescalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the implementation timescales section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the implementation timescales section of a solution.</returns>

        [HttpGet]
        [Route("{id}/sections/implementation-timescales")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> Get([FromRoute][Required]string id)
        {
            var implementationTimescales = await _mediator.Send(new GetImplementationTimescalesBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new ImplementationTimescalesResult(implementationTimescales?.Description));
        }

        /// <summary>
        /// Updates the implementation timescales details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="viewModel">The details of the implementation timescales.</param>
        /// <returns>A task representing an operation to update the details of the implementation timescales section.</returns>
        [HttpPut]
        [Route("{id}/sections/implementation-timescales")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute][Required]string id, [FromBody][Required]UpdateImplementationTimescalesViewModel implementationTimescales) =>
            (await _mediator.Send(new UpdateImplementationTimescalesCommand(id, implementationTimescales?.Description)).ConfigureAwait(false)).ToActionResult();
    }
}
