using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides an endpoint to manage the client application types for a given solution.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class ClientApplicationTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypeController"/> class.
        /// </summary>
        public ClientApplicationTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the client application types section for a given solution.</returns>
        [HttpGet]
        [Route("{id}/sections/client-application-types")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetClientApplicationTypesAsync([FromRoute][Required]string id)
        {
            var solution = await _mediator.Send(new GetSolutionByIdQuery(id));
            return solution == null ? (ActionResult)new NotFoundResult() : Ok(new GetClientApplicationTypesResult(solution.ClientApplication));
        }

        /// <summary>
        /// Updates the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionClientApplicationTypesViewModel">The details of a client application type that includes any updated information.</param>
        /// <returns>A task representing an operation to update the details of the client application types for a solution.</returns>
        [HttpPut]
        [Route("{id}/sections/client-application-types")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateClientApplicationTypesAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionClientApplicationTypesViewModel updateSolutionClientApplicationTypesViewModel)
        {
            await _mediator.Send(new UpdateSolutionClientApplicationTypesCommand(id, updateSolutionClientApplicationTypesViewModel));

            return NoContent();
        }
    }
}
